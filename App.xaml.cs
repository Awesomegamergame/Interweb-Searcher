using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Interweb_Searcher.Views;

namespace Interweb_Searcher
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();

            Window mainWindow;
            string startupUrl = "https://www.google.com";  // Default home page

            if (File.Exists("new"))
            {
                mainWindow = new MainWindow();
            }
            else
            {
                Resources.MergedDictionaries.Clear();
                mainWindow = new MainWindowOld();
            }

            if (args.Length == 2)
            {
                startupUrl = args[1];  // Use the URL provided in the command line argument
            }

            if (mainWindow is MainWindow mainWindowCast)
            {
                mainWindowCast.StartupUrl = startupUrl;  // Pass the URL to MainWindow
            }
            
            mainWindow.Show();

            if(mainWindow is MainWindowOld mainWindowOld)
            {
                MainWindowOld.ISWindow.wb1.Url = new Uri(startupUrl);
                MainWindowOld.ISWindow.area.Text = startupUrl;
                MainWindowOld.WebPages.Add(startupUrl);
                MainWindowOld.AddMenuItem(startupUrl);
            }
        }
    }
}
