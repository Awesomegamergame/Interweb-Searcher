using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Interweb_Searcher.ViewModels;

namespace Interweb_Searcher.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            // Pass the WebBrowser control instance to the ViewModel
            _viewModel = new MainWindowViewModel(webBrowser);
            _viewModel.NavigateCommand.Execute(_viewModel.CurrentUrl);
            DataContext = _viewModel;

            // Attach the Navigated event handler
            webBrowser.Navigated += WebBrowser_Navigated;
        }

        private void WebBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // Update the ViewModel's CurrentUrl property with the new URL
            _viewModel.CurrentUrl = e.Uri.ToString();
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

        private async void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            string newFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "new");

            // Delete the "new" file if it exists
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            // Wait for 500 milliseconds to allow the checkbox to visually update
            await Task.Delay(500);

            // Restart the application
            System.Windows.Forms.Application.Restart();
            Application.Current.Shutdown();
        }
    }
}
