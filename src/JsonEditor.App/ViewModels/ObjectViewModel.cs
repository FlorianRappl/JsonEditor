namespace JsonEditor.App.ViewModels
{
    using MaterialDesignThemes.Wpf;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    sealed class ObjectViewModel : EnumerableViewModel<JObject, PropertyViewModel>
    {
        private readonly ObservableCollection<String> _availableProperties;

        public ObjectViewModel(JObject token, ITokenContainer container)
            : base(token, container)
        {
            _availableProperties = new ObservableCollection<String>();

            AddProperty = (s, ev) =>
            {
                var propertyName = ev.Parameter as String;

                if (propertyName != null)
                {
                    PerformAddProperty(propertyName);
                }
            };
            RemoveProperty = Cmd<PropertyViewModel>(PerformRemoveProperty);
        }

        public IEnumerable<String> AvailableProperties
        {
            get { return _availableProperties; }
        }

        public DialogClosingEventHandler AddProperty { get; }

        public ICommand RemoveProperty { get; }

        protected override IEnumerable<PropertyViewModel> ItemsOf(JObject token)
        {
            return token.Properties().Select(property => new PropertyViewModel(property.Name, property.Value, this));
        }

        protected override void SetSchema(JSchema schema)
        {
            base.SetSchema(schema);
            _availableProperties.Clear();

            if (schema != null)
            {
                foreach (var propertyName in schema.Properties.Keys.Except(Children.Select(m => m.Name)))
                {
                    _availableProperties.Add(propertyName);
                }
            }
        }

        protected override void PerformReplace(JToken original, JToken replacement)
        {
            var obj = Value;

            foreach (var property in obj.Properties())
            {
                if (Object.ReferenceEquals(property.Value, original))
                {
                    property.Value = replacement;
                    return;
                }
            }
        }

        private void PerformAddProperty(String propertyName)
        {
            var schema = Schema?.GetPropertySchema(propertyName);
            var value = schema?.CreateInstance() ?? JValue.CreateString("");
            var property = new PropertyViewModel(propertyName, value, this);
            property.Value.Schema = schema;
            Value.Add(propertyName, value);
            _availableProperties.Remove(propertyName);
            AddNewItem(property);
        }

        private void PerformRemoveProperty(PropertyViewModel property)
        {
            Value.Remove(property.Name);
            _availableProperties.Add(property.Name);
            RemoveExistingItem(property);
        }
    }
}
