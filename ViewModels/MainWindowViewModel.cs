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
            Tabs = new ObservableCollection<TabViewModel> { new TabViewModel(this) };
            SelectedTabIndex = 0;
            SelectedBrowser = Tabs[0].Browser;

            AddTabCommand = new RelayCommand(AddTab);
            RemoveTabCommand = new RelayCommand(RemoveTab, CanRemoveTab);
            NavigateCommand = new RelayCommand(Navigate);
            BackCommand = new RelayCommand(Back, CanGoBack);
            ForwardCommand = new RelayCommand(Forward, CanGoForward);
            RefreshCommand = new RelayCommand(Refresh);
            NavigateHomeCommand = new RelayCommand(NavigateHome);

            CurrentUrl = "https://www.google.com";  // Default home page
            SelectedBrowser.Navigate(CurrentUrl);  // Navigate the initial tab to the home page

            // Subscribe to the Navigated event to update the address bar
            SelectedBrowser.Navigated += SelectedBrowser_Navigated;
            SelectedBrowser.LoadCompleted += SelectedBrowser_LoadCompleted;
        }

        public ObservableCollection<TabViewModel> Tabs { get; }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();
                if (value >= 0 && value < Tabs.Count)
                {
                    SelectedBrowser = Tabs[value].Browser;
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
            var newTab = new TabViewModel(this);
            Tabs.Add(newTab);
            SelectedTabIndex = Tabs.Count - 1;  // Switch to the new tab
        }

        public void RemoveTab(object parameter)
        {
            if (parameter is TabViewModel tab && Tabs.Contains(tab))
            {
                int index = Tabs.IndexOf(tab);
                Tabs.Remove(tab);
                tab.Dispose(); // Dispose of the tab to release resources

                // Adjust the selected tab index after removing a tab
                if (Tabs.Count == 0)
                {
                    // Add a new tab if no tabs are left
                    var newTab = new TabViewModel(this);
                    Tabs.Add(newTab);
                    SelectedTabIndex = 0;
                }
                else if (index == Tabs.Count)
                {
                    // If the removed tab was the last one, switch to the previous tab
                    SelectedTabIndex = Tabs.Count - 1;
                }
                else if (index < Tabs.Count)
                {
                    // If the removed tab was not the last one, stay at the same index
                    SelectedTabIndex = index;
                }
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

        private void SelectedBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string title = (string)SelectedBrowser.InvokeScript("eval", "document.title.toString()");
            if (string.IsNullOrEmpty(title))
            {
                Tabs[SelectedTabIndex].TabText = TruncateText(CurrentUrl, 27);
            }
            else
            {
                Tabs[SelectedTabIndex].TabText = TruncateText(title, 27);
            }
        }

        private string TruncateText(string text, int maxLength)
        {
            if (text.Length <= maxLength)
            {
                return text;
            }
            else
            {
                return text.Substring(0, maxLength);
            }
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
