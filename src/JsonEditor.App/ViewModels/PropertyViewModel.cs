namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using System;

    sealed class PropertyViewModel : ItemViewModel
    {
        private readonly String _name;

        public PropertyViewModel(String name, JToken value, ITokenContainer container)
            : base(value, container)
        {
            _name = name;
        }

        public String Name
        {
            get { return _name; }
        }

        public String Description
        {
            get { return Value?.Schema?.Description ?? "No description given."; }
        }
    }
}
