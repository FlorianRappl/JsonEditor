namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    sealed class ValidationViewModel : BaseViewModel
    {
        private readonly ObservableCollection<String> _errors;

        public ValidationViewModel()
        {
            _errors = new ObservableCollection<String>();
        }

        public IEnumerable<String> Errors
        {
            get { return _errors; }
        }

        public void Validate(JSchema schema, JToken value)
        {
            _errors.Clear();

            if (schema != null)
            {
                using (var reader = new JSchemaValidatingReader(value.CreateReader()))
                {
                    reader.Schema = schema;
                    reader.ValidationEventHandler += OnValidationError;

                    while (reader.Read()) ;
                }
            }
        }

        private void OnValidationError(Object sender, SchemaValidationEventArgs e)
        {
            _errors.Add(e.Message);
        }
    }
}
