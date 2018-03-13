namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using System;

    internal class ItemViewModel : BaseViewModel
    {
        private readonly ValidationViewModel _validation;
        private readonly TypeViewModel _type;
        private readonly ITokenContainer _container;
        private TokenViewModel _value;

        public ItemViewModel(JToken token, ITokenContainer container)
        {
            _validation = new ValidationViewModel();
            _type = new TypeViewModel(token);
            _type.TypeChanged += OnTypeChanged;
            _container = container;
            Value = Wrap(token, container);
        }

        public ValidationViewModel Validation
        {
            get { return _validation; }
        }

        public TypeViewModel Type
        {
            get { return _type; }
        }

        public TokenViewModel Value
        {
            get { return _value; }
            private set { _value = value; RaisePropertyChanged(); }
        }

        private void OnTypeChanged(Object sender, TypeChangedEventArgs e)
        {
            var original = _value.Token;
            var replacement = e.NewToken;
            Value = Wrap(replacement, _container);
            _container.Replace(original, replacement);
        }

        private static TokenViewModel Wrap(JToken value, ITokenContainer container)
        {
            switch (value.Type)
            {
                case JTokenType.Array:   return new ArrayViewModel((JArray)value, container);
                case JTokenType.Boolean: return new BooleanViewModel((JValue)value, container);
                case JTokenType.Float:   return new NumberViewModel((JValue)value, container);
                case JTokenType.Integer: return new IntegerViewModel((JValue)value, container);
                case JTokenType.Object:  return new ObjectViewModel((JObject)value, container);
                case JTokenType.String:  return new StringViewModel((JValue)value, container);
                case JTokenType.Null:    return new EnumViewModel(value, container);
            }

            return null;
        }
    }
}
