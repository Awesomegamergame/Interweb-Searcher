using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Interweb_Searcher.Views;

namespace Interweb_Searcher
{
    public partial class MainWindowOld : Window
    {
        public static MainWindowOld ISWindow;
        public static List<string> WebPages = new List<string>();
        public System.Windows.Forms.WebBrowser wb1;
        public string ProgramLocation = $"{AppDomain.CurrentDomain.BaseDirectory}\\zoom.txt";
        int Current = 0;
        public int Zoom = 0;
        public MainWindowOld()
        {
            InitializeComponent();
            ISWindow = this;
            wb1 = wfh.Child as System.Windows.Forms.WebBrowser;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GoHome();
        }

        void GoHome()
        {
            string link = "http://www.google.com";
            area.Text = link;
            WebPages.Add(link);
            AddMenuItem(link);
        }
        public static void AddMenuItem(string Link)
        {
            MenuItem item = new MenuItem();
            item.Click += ISWindow.MenuClicked;
            item.Header = Link;
            item.Width = 230;
            ISWindow.Menu.Items.Add(item);
        }
        void LoadWebPages(string Link, bool addToList = true)
        {
            area.Text = Link;
            try
            {
                wb1.Url = new Uri(Link);
                AddMenuItem(Link);

                if (addToList)
                {
                    Current++;
                    WebPages.Add(Link);
                }
            }
            catch(UriFormatException)
            { MessageBox.Show("You must have the http:// or the program will fail"); }
        }

        void ToggleWebPages(string Option)
        {
            if (Option == "→")
            {
                if ((WebPages.Count - Current - 1) != 0)
                {
                    Current++;
                    try
                    {
                        LoadWebPages(WebPages[Current], false);
                    }
                    catch (ArgumentOutOfRangeException) { }
                }
            }

            else
            {
                if ((WebPages.Count + Current - 1) >= WebPages.Count)
                {
                    Current--;
                    try
                    {
                        LoadWebPages(WebPages[Current], false);
                    }
                    catch (ArgumentOutOfRangeException) { }
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ToggleWebPages(btn.Content.ToString());
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadWebPages(WebPages[Current]);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            area.Text = "http://www.google.com";
            wb1.Url = new Uri("http://www.google.com");
            WebPages.Add("http://www.google.com");
            Current++;
        }
        private void MenuClicked(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            LoadWebPages(item.Header.ToString());
        }
        private void Area_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoadWebPages(area.Text);
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (WebPages.Count != 0)
            {
                Menu.PlacementTarget = hBTN;
                Menu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                Menu.HorizontalOffset = -200;
                Menu.IsOpen = true;
            }
        }
        private void Button_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Zoom += 25;
            if(Zoom > 1000)
            {
                Zoom = 1000;
            }
            SetZoomPercent(Zoom);
        }
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            Zoom -= 25;
            if (Zoom < 25)
            {
                Zoom = 25;
            }
            SetZoomPercent(Zoom);
        }
        public void SetZoomPercent(int zoom)
        {
            int OLECMDID_OPTICAL_ZOOM = 63;
            int OLECMDEXECOPT_DONTPROMPTUSER = 2;
            dynamic iwb2 = wb1.ActiveXInstance;
            iwb2.ExecWB(OLECMDID_OPTICAL_ZOOM, OLECMDEXECOPT_DONTPROMPTUSER, zoom, zoom);
        }

        private void InternetB_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (!File.Exists(ProgramLocation)) { File.WriteAllText(ProgramLocation, "100"); SetZoomPercent(100); Zoom = 100; }
                else { Zoom = int.Parse(File.ReadAllText(ProgramLocation)); SetZoomPercent(Zoom); }
            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); };
        }

        private async void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            string newFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "new");

            // Delete the "new" file if it exists
            if (!File.Exists(newFilePath))
            {
                File.Create(newFilePath);
            }

            // Wait for 500 milliseconds to allow the checkbox to visually update
            await Task.Delay(500);

            // Restart the application
            System.Windows.Forms.Application.Restart();
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText(ISWindow.ProgramLocation, ISWindow.Zoom.ToString());
        }
    }
}
