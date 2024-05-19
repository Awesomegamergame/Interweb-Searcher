using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Interweb_Searcher.ViewModels
{
    public class TabViewModel : INotifyPropertyChanged
    {
        private string _tabText = "New Tab";
        private WebBrowser _browser = new WebBrowser();

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

        public TabViewModel()
        {
            // Set up any additional initialization logic for the tab
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
