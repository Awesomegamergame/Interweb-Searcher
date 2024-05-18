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

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            TabControl.SelectionChanged += TabControl_SelectionChanged;

            _viewModel.NavigateCommand.Execute(_viewModel.CurrentUrl);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (TabControl.SelectedIndex >= 0 && TabControl.SelectedIndex < viewModel.Tabs.Count)
                {
                    viewModel.SelectedBrowser = viewModel.Tabs[TabControl.SelectedIndex];
                    viewModel.CurrentUrl = viewModel.SelectedBrowser.Source?.ToString() ?? "https://www.google.com";
                }
            }
        }

        private void UrlBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.NavigateCommand.Execute(UrlBox.Text);
                }
            }
        }

        private async void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            string newFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "new");

            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }

            await Task.Delay(500);

            System.Windows.Forms.Application.Restart();
            Application.Current.Shutdown();
        }
    }
}
