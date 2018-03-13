namespace JsonEditor.App.ViewModels
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    sealed class MainViewModel : BaseViewModel, IFileDropTarget
    {
        private readonly ObservableCollection<FileViewModel> _files;
        private FileViewModel _current;

        public MainViewModel()
        {
            _files = new ObservableCollection<FileViewModel>();
            OpenNew = Cmd(PerformOpenNew);
            OpenExisting = Cmd(PerformOpenExisting);
            SaveCurrent = Cmd(() => Current?.Save());
            SaveCurrentAs = Cmd(() => Current?.SaveAs());
            CloseCurrent = Cmd(PerformCloseCurrent);
            CloseApplication = Cmd(PerformCloseApplication);
            ShowArticle = Cmd(PerformShowArticle);
            ShowAbout = Cmd(PerformShowAbout);
        }

        public ICommand OpenNew { get; }

        public ICommand OpenExisting { get; }

        public ICommand ShowArticle { get; }

        public ICommand ShowAbout { get; }

        public ICommand SaveCurrent { get; }

        public ICommand SaveCurrentAs { get; }

        public ICommand CloseCurrent { get; }

        public ICommand CloseApplication { get; }

        public Boolean HasCurrent
        {
            get { return _current != null; }
        }

        public IEnumerable<FileViewModel> Files
        {
            get { return _files; }
        }

        public FileViewModel Current
        {
            get { return _current; }
            set { _current = value; RaisePropertyChanged(); RaisePropertyChanged("HasCurrent"); }
        }

        public void Dropped(IEnumerable<String> files)
        {
            foreach (var file in files)
            {
                OpenExistingFile(file);
            }
        }

        private void PerformShowAbout()
        {
            var nl = Environment.NewLine;
            var app = Assembly.GetEntryAssembly();
            var name = app.GetCustomAttribute<AssemblyProductAttribute>().Product;
            var copyright = app.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            var version = app.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            MessageBox.Show($"{name}{nl}{version}{nl}{copyright}", "About", MessageBoxButton.OK);
        }

        private void PerformShowArticle()
        {
            Process.Start("https://www.codeproject.com");
        }

        private void PerformOpenNew()
        {
            var newFile = new FileViewModel();
            OpenFile(newFile);
        }

        private void PerformOpenExisting()
        {
            var ofd = new OpenFileDialog
            {
                Title = "Open JSON File",
                Filter = "JSON File (*.json)|*.json",
                Multiselect = true
            };

            if (ofd.ShowDialog() == true)
            {
                foreach (var path in ofd.FileNames)
                {
                    OpenExistingFile(path);
                }
            }
        }

        private void OpenExistingFile(String path)
        {
            foreach (var file in _files)
            {
                if (file.Path?.Equals(path, StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    Current = file;
                    return;
                }
            }

            var oldFile = new FileViewModel(path);
            OpenFile(oldFile);
        }

        private void OpenFile(FileViewModel oldFile)
        {
            _files.Add(oldFile);
            Current = oldFile;
        }

        private void PerformCloseCurrent()
        {
            if (_current != null)
            {
                if (_current.Content.Value.IsChanged)
                {
                    var result = MessageBox.Show("Discard changes in the unsaved file?", "Discard changes?", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                var index = _files.IndexOf(_current);
                _files.RemoveAt(index);

                if (index == _files.Count)
                {
                    index--;
                }

                Current = index >= 0 ? _files[index] : null;
            }
        }

        private void PerformCloseApplication()
        {
            var changed = 0;

            foreach (var file in _files)
            {
                if (file.Content.Value.IsChanged)
                {
                    changed++;
                }
            }

            if (changed > 0)
            {
                var result = MessageBox.Show($"Discard changes in the {changed} unsaved files?", "Discard changes?", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            App.Current.Shutdown();
        }
    }
}
