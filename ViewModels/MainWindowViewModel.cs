using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace Interweb_Searcher.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _currentUrl;
        private WebBrowser _selectedBrowser;
        private int _selectedTabIndex;

        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<WebBrowser> { new WebBrowser() };
            SelectedTabIndex = 0;
            SelectedBrowser = Tabs[0];

            AddTabCommand = new RelayCommand(AddTab);
            RemoveTabCommand = new RelayCommand(RemoveTab, CanRemoveTab);
            NavigateCommand = new RelayCommand(Navigate);
            BackCommand = new RelayCommand(Back, CanGoBack);
            ForwardCommand = new RelayCommand(Forward, CanGoForward);
            RefreshCommand = new RelayCommand(Refresh);
            NavigateHomeCommand = new RelayCommand(NavigateHome);

            CurrentUrl = "https://www.google.com";
            SelectedBrowser.Navigate(CurrentUrl);  // Navigate the initial tab to the home page

            // Subscribe to the Navigated event to update the address bar
            SelectedBrowser.Navigated += SelectedBrowser_Navigated;
        }

        public ObservableCollection<WebBrowser> Tabs { get; }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();
                if (value >= 0 && value < Tabs.Count)
                {
                    SelectedBrowser = Tabs[value];
                    CurrentUrl = SelectedBrowser.Source?.ToString() ?? CurrentUrl;
                }
            }
        }

        public WebBrowser SelectedBrowser
        {
            get => _selectedBrowser;
            set
            {
                _selectedBrowser = value;
                OnPropertyChanged();
            }
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

        public ICommand AddTabCommand { get; }
        public ICommand RemoveTabCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand ForwardCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NavigateHomeCommand { get; }

        private void AddTab(object parameter)
        {
            var newBrowser = new WebBrowser();
            newBrowser.Navigate("https://www.google.com");  // Navigate the new tab to the home page
            Tabs.Add(newBrowser);
            SelectedTabIndex = Tabs.Count - 1;  // Switch to the new tab

            // Subscribe to the Navigated event to update the address bar
            newBrowser.Navigated += SelectedBrowser_Navigated;
        }

        private void RemoveTab(object parameter)
        {
            if (SelectedTabIndex >= 0 && Tabs.Count > 1)
            {
                Tabs.RemoveAt(SelectedTabIndex);
                SelectedTabIndex = Tabs.Count - 1;
            }
        }

        private bool CanRemoveTab(object parameter)
        {
            return Tabs.Count > 1;
        }

        private void Navigate(object parameter)
        {
            if (parameter is string url && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                CurrentUrl = url;
                SelectedBrowser?.Navigate(url);
            }
        }

        private bool CanGoBack(object parameter)
        {
            return SelectedBrowser?.CanGoBack ?? false;
        }

        private void Back(object parameter)
        {
            SelectedBrowser?.GoBack();
        }

        private bool CanGoForward(object parameter)
        {
            return SelectedBrowser?.CanGoForward ?? false;
        }

        private void Forward(object parameter)
        {
            SelectedBrowser?.GoForward();
        }

        private void Refresh(object parameter)
        {
            SelectedBrowser?.Refresh();
        }

        private void NavigateHome(object parameter)
        {
            CurrentUrl = "https://www.google.com";
            SelectedBrowser?.Navigate(CurrentUrl);
        }

        private void SelectedBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            CurrentUrl = e.Uri?.ToString() ?? CurrentUrl;
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
