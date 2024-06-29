using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Interweb_Searcher.Views;

namespace Interweb_Searcher
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            //args = new string[] { "", "ie", "https://www.google.com/" }; -- test line

            Window mainWindow;
            string startupUrl = "https://www.google.com/";  // Default home page

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

            if (args.Length == 3)
            {
                if (args[1].Equals("ie") && File.Exists("C:\\Program Files\\Internet Explorer\\iexplore.exe"))
                {
                    mainWindow.WindowState = WindowState.Minimized;
                    Thread thread = new Thread(() => HttpMethod(args[2]));
                    thread.Start();
                    startupUrl = "http://localhost:7676";  // Use the URL provided in the command line argument
                }
                else
                {
                    startupUrl = args[2];
                }
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

        private void HttpMethod(string site)
        {
            string htmlResponse = $"<html><script>window.open('{site}', '_blank');</script></html>";

            // Create a new HttpListener instance
            HttpListener listener = new HttpListener();

            // Add prefixes (URLs) to listen to
            listener.Prefixes.Add("http://localhost:7676/");

            // Start listening for incoming requests
            listener.Start();

            // Wait for a request to come in
            HttpListenerContext context = listener.GetContext();

            // Prepare a response
            byte[] responseBuffer = Encoding.UTF8.GetBytes(htmlResponse);

            // Get the response object
            HttpListenerResponse response = context.Response;

            // Set content type and length
            response.ContentType = "text/html";
            response.ContentLength64 = responseBuffer.Length;

            // Write the response to the output stream
            response.OutputStream.Write(responseBuffer, 0, responseBuffer.Length);

            // Close the output stream
            response.OutputStream.Close();

            // Stop listening for incoming requests
            listener.Stop();

            this.Dispatcher.Invoke(() =>
            {
                Current.Shutdown();
            });
        }
    }
}
