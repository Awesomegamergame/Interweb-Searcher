using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Interweb_Searcher.MainWindow;

namespace Interweb_Searcher
{
    internal class StartupF
    {
        public static void StartupFun(string link)
        {
            ISWindow.wb1.Url = new Uri(link);
            ISWindow.area.Text = link;
            WebPages.Add(link);
            AddMenuItem(link);
        }
    }
}
