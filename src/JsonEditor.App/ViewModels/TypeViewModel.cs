namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;

    sealed class TypeViewModel : BaseViewModel
    {
        private static readonly String[] _availableTypes = new[]
        {
            "number",
            "integer",
            "enum",
            "boolean",
            "string",
            "object",
            "array"
        };

        public event EventHandler<TypeChangedEventArgs> TypeChanged;
        
        private String _type;

        public TypeViewModel(JToken token)
        {
            _type = FindType(token);
        }

        public IEnumerable<String> AvailableTypes
        {
            get { return _availableTypes; }
        }

        public String SelectedType
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    TypeChanged?.Invoke(this, new TypeChangedEventArgs(value));
                    RaisePropertyChanged();
                }
            }
        }

        private static String FindType(JToken value)
        {
            switch (value.Type)
            {
                case JTokenType.Float:   return _availableTypes[0];
                case JTokenType.Integer: return _availableTypes[1];
                case JTokenType.Null:    return _availableTypes[2];
                case JTokenType.Boolean: return _availableTypes[3];
                case JTokenType.String:  return _availableTypes[4];
                case JTokenType.Object:  return _availableTypes[5];
                case JTokenType.Array:   return _availableTypes[6];
            }

            return null;
        }
    }
}
