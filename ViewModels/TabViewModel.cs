using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace Interweb_Searcher.ViewModels
{
    public class TabViewModel : INotifyPropertyChanged, IDisposable
    {
        private string _tabText = "New Tab";
        private WebBrowser _browser;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public TabViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _browser = new WebBrowser();
            _browser.Navigated += Browser_Navigated;
            _browser.LoadCompleted += Browser_LoadCompleted;
            _browser.Navigate("https://www.google.com"); // Default home page
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

        public bool IsSpecialTab { get; set; }

        public ICommand RemoveTabCommand { get; }

        private void RemoveTab(object parameter)
        {
            _mainWindowViewModel.RemoveTab(this);
        }

        public void Dispose()
        {
            if (_browser != null)
            {
                _browser.Navigated -= Browser_Navigated;
                _browser.LoadCompleted -= Browser_LoadCompleted;
                _browser.Dispose(); // Properly dispose of the WebBrowser control
                _browser = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            _mainWindowViewModel.CurrentUrl = e.Uri?.ToString() ?? _mainWindowViewModel.CurrentUrl;
        }

        private void Browser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string title = (string)Browser.InvokeScript("eval", "document.title.toString()");
            if (string.IsNullOrEmpty(title))
            {
                TabText = TruncateText(_mainWindowViewModel.CurrentUrl, 27);
            }
            else
            {
                TabText = TruncateText(title, 27);
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
    }
}
