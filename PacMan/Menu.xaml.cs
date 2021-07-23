using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PacMan
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService mars = NavigationService.GetNavigationService(this);

            if( sender == play)
            {
                Uri uri = new Uri("Level1.xaml", UriKind.Relative);
                mars.Navigate(uri);
            }

            if (sender == exit)
            {
                Application.Current.Shutdown();
            }

        }
    }
}
