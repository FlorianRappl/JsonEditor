namespace JsonEditor.App.ViewModels
{
    using MaterialDesignThemes.Wpf;
    using Microsoft.Win32;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;

    sealed class FileViewModel : BaseViewModel, ITokenContainer
    {
        private readonly ItemViewModel _content;
        private readonly SchemaViewModel _schema;
        private String _fileName;
        private String _filePath;
        private Boolean _changed;

        private FileViewModel(JToken token)
        {
            _schema = new SchemaViewModel();
            _content = new ItemViewModel(token, this);
            var validation = _schema.ValidateAsync(_content);
            SelectSchema = async (s, ev) =>
            {
                var schemaSelection = ev.Parameter as SchemaSelectionViewModel;

                if (schemaSelection != null)
                {
                    var path = schemaSelection.IsCurrent ? null : schemaSelection.Path;
                    await _schema.ChangeSchemaToAsync(path, _content);
                }
            };
        }

        public FileViewModel()
            : this(new JObject())
        {
            _fileName = "New JSON";
        }

        public FileViewModel(String path)
            : this(ReadJson(path))
        {
            Path = path;
        }

        public Boolean IsChanged
        {
            get { return _changed; }
            set { _changed = value; RaisePropertyChanged(); }
        }

        public ItemViewModel Content
        {
            get { return _content; }
        }

        public String Name
        {
            get { return _fileName; }
            private set { _fileName = value; RaisePropertyChanged(); }
        }

        public SchemaViewModel Schema
        {
            get { return _schema; }
        }

        public DialogClosingEventHandler SelectSchema
        {
            get;
        }

        public String Path
        {
            get { return _filePath; }
            set { _filePath = value; Name = System.IO.Path.GetFileName(value); RaisePropertyChanged(); }
        }

        public void Save()
        {
            if (!String.IsNullOrEmpty(_filePath))
            {
                SaveTo(_filePath);
            }
            else
            {
                SaveAs();
            }
        }

        public void SaveAs()
        {
            var sfd = new SaveFileDialog
            {
                Title = "Save JSON as ...",
                Filter = "JSON File (*.json)|*.json"
            };

            if (sfd.ShowDialog() == true)
            {
                Path = sfd.FileName;
                SaveTo(_filePath);
            }
        }

        void ITokenContainer.Replace(JToken original, JToken replacement)
        {
        }

        private void SaveTo(String path)
        {
            using (var sw = File.CreateText(path))
            {
                using (var writer = new JsonTextWriter(sw) { Indentation = 2, Formatting = Formatting.Indented })
                {
                    _content.Value.Token.WriteTo(writer);
                }
            }

            _content.Value.Saved();
        }

        private static JToken ReadJson(String path)
        {
            using (var sr = new StreamReader(path))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    return JToken.Load(reader);
                }
            }
        }
    }
}
