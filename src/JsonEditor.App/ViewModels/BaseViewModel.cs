namespace JsonEditor.App.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected static ICommand Cmd(Action action)
        {
            return new RelayCommand(action);
        }

        protected static ICommand Cmd<T>(Action<T> action)
        {
            return new RelayCommand<T>(action);
        }

        private class RelayCommand : ICommand
        {
            private readonly Action _action;

            public RelayCommand(Action action)
            {
                _action = action;
            }

#pragma warning disable 67
            public event EventHandler CanExecuteChanged;
#pragma warning restore 67

            public Boolean CanExecute(Object parameter)
            {
                return true;
            }

            public void Execute(Object parameter)
            {
                _action.Invoke();
            }
        }

        private class RelayCommand<T> : ICommand
        {
            private readonly Action<T> _action;

            public RelayCommand(Action<T> action)
            {
                _action = action;
            }

#pragma warning disable 67
            public event EventHandler CanExecuteChanged;
#pragma warning restore 67

            public Boolean CanExecute(Object parameter)
            {
                return true;
            }

            public void Execute(Object parameter)
            {
                _action.Invoke((T)parameter);
            }
        }
    }
}
