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

            if (File.Exists("new")) { mainWindow = new MainWindow(); }
            else { Resources.MergedDictionaries.Clear(); mainWindow = new MainWindowOld(); }

            if (args.Length == 2)
            {

                mainWindow.Show();
                //StartupF.StartupFun(args[1]);
            }
            else
            {
                mainWindow.Show();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //File.WriteAllText(window.ProgramLocation, window.Zoom.ToString());
        }
    }
}
