namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using System;

    sealed class StringViewModel : TokenViewModel
    {
        private String _value;

        public StringViewModel(JValue token, ITokenContainer container)
            : base(token, container)
        {
            _value = token.Value<String>();
        }

        public String Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); ChangeTo(new JValue(value)); }
        }
    }
}
