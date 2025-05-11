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

        public string StartupUrl { get; set; } = "https://www.google.com/";  // Default home page

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                this.Top = Properties.Settings.Default.WindowTop;
                this.Left = Properties.Settings.Default.WindowLeft;
                this.Width = Properties.Settings.Default.WindowWidth;
                this.Height = Properties.Settings.Default.WindowHeight;
                this.WindowState = Properties.Settings.Default.WindowState;
            };

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            Loaded += MainWindow_Loaded;  // Add event handler for Loaded event
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Navigate to the StartupUrl when the window is loaded
            var firstTab = _viewModel.Tabs[0];
            firstTab.NavigateCommand.Execute(StartupUrl);
        }

        private void UrlBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Execute the NavigateCommand of the selected tab when Enter key is pressed
                if (TabControl.SelectedItem is TabViewModel selectedTab)
                {
                    selectedTab.NavigateCommand.Execute((sender as TextBox).Text);
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

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.WindowState = this.WindowState;

            if (this.WindowState == WindowState.Normal)
            {
                Properties.Settings.Default.WindowTop = this.Top;
                Properties.Settings.Default.WindowLeft = this.Left;
                Properties.Settings.Default.WindowWidth = this.Width;
                Properties.Settings.Default.WindowHeight = this.Height;
            }
            else
            {
                // Save RestoreBounds if window is maximized or minimized
                Properties.Settings.Default.WindowTop = this.RestoreBounds.Top;
                Properties.Settings.Default.WindowLeft = this.RestoreBounds.Left;
                Properties.Settings.Default.WindowWidth = this.RestoreBounds.Width;
                Properties.Settings.Default.WindowHeight = this.RestoreBounds.Height;
            }

            Properties.Settings.Default.Save();
            base.OnClosing(e);
        }
    }
}
