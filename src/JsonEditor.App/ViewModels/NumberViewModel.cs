namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using System;

    sealed class NumberViewModel : TokenViewModel
    {
        private Double _value;

        public NumberViewModel(JValue token, ITokenContainer container)
            : base(token, container)
        {
            _value = token.Value<Double>();
        }

        public Double Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); ChangeTo(new JValue(value)); }
        }
    }
}
