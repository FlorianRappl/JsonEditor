namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    sealed class SchemaViewModel : BaseViewModel
    {
        private static readonly JSchema DefaultSchema = new JSchema { Title = "Schemaless" };

        private JSchema _schema;
        private String _path;

        public SchemaViewModel()
        {
            _schema = DefaultSchema;
        }

        public String Title
        {
            get { return _schema.Title; }
        }

        public String Path
        {
            get { return _path; }
        }

        internal async Task ValidateAsync(ItemViewModel content)
        {
            var obj = content.Value.Token as JObject;
            _path = GetSchemaPath(obj);
            _schema = await LoadSchemaAsync(_path);
            RaisePropertyChanged("Path");
            RaisePropertyChanged("Title");
            content.Validate(_schema);
        }

        public async Task ChangeSchemaToAsync(String path, ItemViewModel content)
        {
            if (String.IsNullOrEmpty(path))
            {
                _schema = DefaultSchema;
            }
            else
            {
                _schema = await LoadSchemaAsync(path);
            }

            _path = path;
            RaisePropertyChanged("Path");
            RaisePropertyChanged("Title");
            content.Validate(_schema);
        }

        private String GetSchemaPath(JObject obj)
        {
            var token = default(JToken);

            if (obj != null && obj.TryGetValue("$schema", out token) && token.Type == JTokenType.String)
            {
                return token.Value<String>();
            }

            return null;
        }

        private static async Task<JSchema> LoadSchemaAsync(String path)
        {
            try
            {
                var schema = path != null ? await ReadJSchemaAsync(path) : null;
                var title = path != null ? System.IO.Path.GetFileNameWithoutExtension(path) : null;

                if (schema != null && schema.Title == null)
                {
                    schema.Title = title;
                }

                return schema ?? DefaultSchema;
            }
            catch
            {
                return DefaultSchema;
            }
        }

        private static async Task<JSchema> ReadJSchemaAsync(String path)
        {
            var url = default(Uri);

            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path))
                {
                    using (var reader = new JsonTextReader(sr))
                    {
                        return JSchema.Load(reader);
                    }
                }
            }
            else if (Uri.TryCreate(path, UriKind.Absolute, out url))
            {
                var http = new HttpClient();
                var response = await http.GetStringAsync(url);
                return JSchema.Parse(response);
            }

            return null;
        }
    }
}
