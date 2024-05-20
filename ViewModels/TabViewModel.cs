using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Interweb_Searcher.ViewModels
{
    public class TabViewModel : INotifyPropertyChanged
    {
        private string _tabText = "New Tab";
        private WebBrowser _browser;
        private MainWindowViewModel _mainWindowViewModel;

        public TabViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _browser = new WebBrowser();
            _browser.Navigated += Browser_Navigated;
            _browser.LoadCompleted += Browser_LoadCompleted;
            Browser.Navigate("https://www.google.com"); // Default home page
            RemoveTabCommand = new RelayCommand(RemoveTab);
        }

        public string TabText
        {
            get => _tabText;
            set
            {
                _tabText = value;
                OnPropertyChanged();
            }
        }

        public WebBrowser Browser
        {
            get => _browser;
            set
            {
                _browser = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand RemoveTabCommand { get; }

        private void RemoveTab(object parameter)
        {
            _mainWindowViewModel.RemoveTab(this);
        }

        private void Browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // This ensures that the current URL is updated correctly
            _mainWindowViewModel.CurrentUrl = e.Uri?.ToString() ?? _mainWindowViewModel.CurrentUrl;
        }

        private void Browser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // This fetches the title of the webpage and updates the tab text
            string title = (string)Browser.InvokeScript("eval", "document.title.toString()");
            TabText = string.IsNullOrEmpty(title) ? TruncateText(_mainWindowViewModel.CurrentUrl, 27) : TruncateText(title, 27);
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
}
