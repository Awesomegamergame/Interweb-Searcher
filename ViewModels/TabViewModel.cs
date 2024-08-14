using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Interweb_Searcher.Models;
using SHDocVw;

namespace Interweb_Searcher.ViewModels
{
    public class TabViewModel : INotifyPropertyChanged, IDisposable
    {
        private string _tabText = "New Tab";
        private System.Windows.Controls.WebBrowser _browser;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private string _currentUrl;

        public TabViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _browser = new System.Windows.Controls.WebBrowser();
            _browser.Navigated += Browser_Navigated;
            _browser.LoadCompleted += Browser_LoadCompleted;
            _browser.Navigate("https://www.google.com/"); // Default home page
            RemoveTabCommand = new RelayCommand(RemoveTab);

            BackCommand = new RelayCommand(Back, CanGoBack);
            ForwardCommand = new RelayCommand(Forward, CanGoForward);
            RefreshCommand = new RelayCommand(Refresh);
            NavigateHomeCommand = new RelayCommand(NavigateHome);
            NavigateCommand = new RelayCommand(Navigate);
            testclickc = new RelayCommand(testclick);
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

        public System.Windows.Controls.WebBrowser Browser
        {
            get => _browser;
            set
            {
                _browser = value;
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

        public bool IsSpecialTab { get; set; }

        public ICommand RemoveTabCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand ForwardCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand testclickc { get; }

        private void RemoveTab(object parameter)
        {
            _mainWindowViewModel.RemoveTab(this);
        }

        private bool CanGoBack(object parameter)
        {
            return Browser?.CanGoBack ?? false;
        }

        private void Back(object parameter)
        {
            Browser?.GoBack();
        }

        private bool CanGoForward(object parameter)
        {
            return Browser?.CanGoForward ?? false;
        }

        private void Forward(object parameter)
        {
            Browser?.GoForward();
        }

        private void Refresh(object parameter)
        {
            Browser?.Refresh();
        }

        private void NavigateHome(object parameter)
        {
            CurrentUrl = "https://www.google.com/";
            Browser?.Navigate(CurrentUrl);
        }

        private void Navigate(object parameter)
        {
            if (parameter is string url && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                CurrentUrl = url;
                Browser?.Navigate(url);
            }
        }

        private void Browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            CurrentUrl = e.Uri?.ToString() ?? CurrentUrl;
            (BackCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ForwardCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void Browser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string title = (string)Browser.InvokeScript("eval", "document.title.toString()");
            TabText = string.IsNullOrEmpty(title) ? TruncateText(CurrentUrl, 27) : TruncateText(title, 27);
        }

        private string TruncateText(string text, int maxLength)
        {
            return text.Length <= maxLength ? text : text.Substring(0, maxLength);
        }

        private void testclick(object sender)
        {
            MessageBox.Show("Test");
        }

        public void Dispose()
        {
            if (_browser != null)
            {
                _browser.Navigated -= Browser_Navigated;
                _browser.LoadCompleted -= Browser_LoadCompleted;
                _browser.Dispose();
                _browser = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /*
         * Some code that ill need later for zoom controls when i get the ui created
         * 
            dynamic activeX = this.Browser.GetType().InvokeMember("ActiveXInstance",
                System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, this.Browser, new object[] { });

            IWebBrowser2 webBrowser2 = (IWebBrowser2)activeX;

            webBrowser2.ExecWB(OLECMDID.OLECMDID_CLOSE, OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER);
        */
    }
}
