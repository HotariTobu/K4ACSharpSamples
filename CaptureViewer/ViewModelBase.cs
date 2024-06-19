using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SharedWPF
{
    /*
     Refer to https://qiita.com/tricogimmick/items/f07ef53dea817d198475
     */
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region == INotifyPropertyChanged ==

        #region == PropertyChanged ==
        
        private event PropertyChangedEventHandler? _PropertyChanged;
        
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add => _PropertyChanged += value;
            remove => _PropertyChanged -= value;
        }

        #endregion

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region == INotifyDataErrorInfo ==
        
        private readonly Dictionary<string, List<object?>> _Errors = new();

        bool INotifyDataErrorInfo.HasErrors => _Errors.Any();

        #region == ErrorsChanged ==
        
        private event EventHandler<DataErrorsChangedEventArgs>? _ErrorsChanged;

        event EventHandler<DataErrorsChangedEventArgs>? INotifyDataErrorInfo.ErrorsChanged
        {
            add => _ErrorsChanged += value;
            remove => _ErrorsChanged -= value;
        }

        #endregion

        IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
        {
            if (propertyName != null && _Errors.TryGetValue(propertyName, out List<object?>? errors))
            {
                return errors;
            }
            else
            {
                return Enumerable.Empty<object>();
            }
        }

        protected void AddError(object? error = null, [CallerMemberName] string propertyName = "")
        {
            if (_Errors.TryGetValue(propertyName, out List<object?>? errors))
            {
                errors?.Add(error);
            }
            else
            {
                _Errors.Add(propertyName, new List<object?> { error });
            }

            RaiseErrorsChanged(propertyName);
        }

        protected void ClearErrors([CallerMemberName] string propertyName = "")
        {
            if (_Errors.ContainsKey(propertyName))
            {
                _Errors.Remove(propertyName);
            }

            RaiseErrorsChanged(propertyName);
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            _ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion

        #region == ICommand Helper ==

        #region == _DelegateCommand ==

        private class _DelegateCommand : ICommand
        {
            private Action<object?> _Command;
            private Func<object?, bool>? _CanExecute;

            public _DelegateCommand(Action<object?> command, Func<object?, bool>? canExecute = null)
            {
                if (command == null)
                {
                    throw new ArgumentNullException(nameof(command));
                }

                _Command = command;
                _CanExecute = canExecute;
            }

            void ICommand.Execute(object? parameter)
            {
                _Command(parameter);
            }

            bool ICommand.CanExecute(object? parameter)
            {
                if (_CanExecute == null)
                {
                    return true;

                }
                else
                {
                    return _CanExecute(parameter);
                }
            }

            event EventHandler? ICommand.CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        protected ICommand CreateCommand(Action<object?> command, Func<object?, bool>? canExecute = null)
        {
            return new _DelegateCommand(command, canExecute);
        }

        #endregion
    }
}