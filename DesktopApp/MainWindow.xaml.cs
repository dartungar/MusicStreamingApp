using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri(@"UI/Pages/HomePage.xaml", UriKind.Relative));
        }

        private void TracksButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri(@"UI/Pages/TracksPage.xaml", UriKind.Relative));
        }

        private void ArtistsButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri(@"UI/Pages/ArtistsPage.xaml", UriKind.Relative));
        }


    }
}
