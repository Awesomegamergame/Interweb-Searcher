using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Interweb_Searcher
{
    public partial class App : Application
    {
        MainWindow window = new MainWindow();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {

                window.Show();
                StartupF.StartupFun(args[1]);
            }
            else
            {
                window.Show();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            File.WriteAllText(window.ProgramLocation, window.Zoom.ToString());
        }
    }
}
