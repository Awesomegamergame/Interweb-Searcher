using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Interweb_Searcher
{
    public partial class MainWindow : Window
    {
        public static MainWindow ISWindow;
        public static List<string> WebPages;
        int Current = 0;
        public MainWindow()
        {
            InitializeComponent();
            ISWindow = this;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WebPages = new List<string>();
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
                InternetB.Source = new Uri(Link);
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
            InternetB.Source = new Uri("http://www.google.com");
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
    }
}
