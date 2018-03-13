namespace JsonEditor.App.ViewModels
{
    using System;

    sealed class SchemaSelectionViewModel : BaseViewModel
    {
        private Boolean _current;
        private String _path;

        public SchemaSelectionViewModel()
        {
            _current = true;
            _path = String.Empty;
        }

        public Boolean IsCurrent
        {
            get { return _current; }
            set { _current = value; RaisePropertyChanged(); RaisePropertyChanged("IsNotCurrent"); }
        }

        public Boolean IsNotCurrent
        {
            get { return !_current; }
            set { _current = !value; RaisePropertyChanged(); RaisePropertyChanged("IsCurrent"); }
        }

        public String Path
        {
            get { return _path; }
            set { _path = value; if (!String.IsNullOrEmpty(value)) { IsCurrent = false; } RaisePropertyChanged(); }
        }
    }
}
