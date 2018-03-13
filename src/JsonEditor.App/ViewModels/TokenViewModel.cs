namespace JsonEditor.App.ViewModels
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;

    abstract class TokenViewModel : BaseViewModel
    {
        private readonly ITokenContainer _container;
        private JToken _token;
        private Boolean _changed;
        private JSchema _schema;

        public TokenViewModel(JToken token, ITokenContainer container)
        {
            _token = token;
            _container = container;
        }

        public Boolean IsChanged
        {
            get { return _changed; }
            set { _changed = value; RaisePropertyChanged(); _container.IsChanged = value; }
        }

        public JSchema Schema
        {
            get { return _schema; }
            set { _schema = value; RaisePropertyChanged(); SetSchema(value); }
        }

        public JToken Token
        {
            get { return _token; }
        }

        protected void ChangeTo(JToken token)
        {
            _container.Replace(_token, token);
            _token = token;
        }

        protected virtual void SetSchema(JSchema value)
        {
        }
    }
}
