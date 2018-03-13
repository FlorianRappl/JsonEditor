namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using System;

    sealed class IntegerViewModel : TokenViewModel
    {
        private Int64 _value;

        public IntegerViewModel(JValue token, ITokenContainer container)
            : base(token, container)
        {
            _value = token.Value<Int64>();
        }

        public Int64 Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); ChangeTo(new JValue(value)); }
        }
    }
}
