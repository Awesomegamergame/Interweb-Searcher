using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Interweb_Searcher
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                MainWindow window = new MainWindow();
                window.Show();
                StartupF.StartupFun(args[1]);
            }
            else
            {
                MainWindow window = new MainWindow();
                window.Show();
            }
        }
    }
}
