namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using System;

    sealed class BooleanViewModel : TokenViewModel
    {
        private Boolean _value;

        public BooleanViewModel(JValue token, ITokenContainer container)
            : base(token, container)
        {
            _value = token.Value<Boolean>();
        }

        public Boolean Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); ChangeTo(new JValue(value)); }
        }
    }
}
