using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace Interweb_Searcher.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _currentUrl;
        private readonly WebBrowser _webBrowser;

        public MainWindowViewModel(WebBrowser webBrowser)
        {
            _webBrowser = webBrowser;
            NavigateCommand = new RelayCommand(Navigate);
            BackCommand = new RelayCommand(Back, CanGoBack);
            ForwardCommand = new RelayCommand(Forward, CanGoForward);
            RefreshCommand = new RelayCommand(Refresh);
            NavigateHomeCommand = new RelayCommand(NavigateHome);

            CurrentUrl = "https://www.google.com";
        }

        public string CurrentUrl
        {
            get => _currentUrl;
            set
            {
                _currentUrl = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand ForwardCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NavigateHomeCommand { get; }

        private void Navigate(object parameter)
        {
            // Execute navigation only if parameter is a valid URL
            if (parameter is string url && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                CurrentUrl = url;
                _webBrowser.Navigate(url);
            }
        }

        private bool CanGoBack(object parameter)
        {
            return _webBrowser.CanGoBack;
        }

        private void Back(object parameter)
        {
            _webBrowser.GoBack();
        }

        private bool CanGoForward(object parameter)
        {
            return _webBrowser.CanGoForward;
        }

        private void Forward(object parameter)
        {
            _webBrowser.GoForward();
        }

        private void Refresh(object parameter)
        {
            _webBrowser.Refresh();
        }

        private void NavigateHome(object parameter)
        {
            CurrentUrl = "https://www.google.com"; // Set your home URL
            _webBrowser.Navigate(CurrentUrl);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
