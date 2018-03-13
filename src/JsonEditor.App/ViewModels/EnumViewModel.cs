namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    sealed class EnumViewModel : TokenViewModel
    {
        private readonly ObservableCollection<JToken> _options;

        public EnumViewModel(JToken token, ITokenContainer container)
            : base(token, container)
        {
            _options = new ObservableCollection<JToken>();
            _options.Add(token);
        }

        public IEnumerable<JToken> Options
        {
            get { return _options; }
        }

        public JToken Value
        {
            get { return Token; }
            set { ChangeTo(value); RaisePropertyChanged(); }
        }
    }
}
