using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Interweb_Searcher.ViewModels;

namespace Interweb_Searcher.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public string StartupUrl { get; set; } = "https://www.google.com";  // Default home page

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            TabControl.SelectionChanged += TabControl_SelectionChanged;

            Loaded += MainWindow_Loaded;  // Add event handler for Loaded event
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Navigate to the StartupUrl when the window is loaded
            _viewModel.NavigateCommand.Execute(StartupUrl);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                // Update the ViewModel's CurrentUrl property with the new URL
                if (TabControl.SelectedItem is WebBrowser selectedBrowser)
                {
                    _viewModel.CurrentUrl = selectedBrowser.Source?.ToString();
                }
            }
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
