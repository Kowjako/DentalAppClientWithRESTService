﻿using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DentalClientWithRESTService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isLightTheme = true;

        public MainWindow()
        {
            InitializeComponent();
            var uri = new Uri("Themes/themeLight.xaml", UriKind.Relative);
            var resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        /// <summary>
        /// Draggable header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void header_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// Close app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeBtn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Change app theme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeThemeBtn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var uri = new Uri(isLightTheme ? "Themes/themeDark.xaml" : "Themes/themeLight.xaml", UriKind.Relative);
            var resourceDictionary = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            changeThemeBtn.Source = isLightTheme ? new BitmapImage(new Uri(@"/Icons/light.png", UriKind.Relative))
                                                 : new BitmapImage(new Uri(@"/Icons/dark.png", UriKind.Relative));
            closeBtn.Source = isLightTheme ? new BitmapImage(new Uri(@"/Icons/closeLight.png", UriKind.Relative))
                                           : new BitmapImage(new Uri(@"/Icons/closeDark.png", UriKind.Relative));
            isLightTheme = !isLightTheme;
            
        }
    }
}
