using System;
using System.Windows;
using System.Windows.Input;
using Interweb_Searcher.ViewModels;

namespace Interweb_Searcher.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Pass the WebBrowser control instance to the ViewModel
            var viewModel = new MainWindowViewModel(webBrowser);
            viewModel.NavigateCommand.Execute(viewModel.CurrentUrl);
            DataContext = viewModel;
        }

        private void UrlBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Execute the NavigateCommand when Enter key is pressed
                if (DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.NavigateCommand.Execute(UrlBox.Text);
                }
            }
        }
    }
}
