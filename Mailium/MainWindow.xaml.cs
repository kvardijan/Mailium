using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Mailium
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            contentContainer.Content = new Login(contentContainer);
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            contentContainer.Content = new Registration(contentContainer);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            DisableLoggedUserControls();
            UserManager.SetCurrentUser(null);
            contentContainer.Content = new Login(contentContainer);
        }

        private void DisableLoggedUserControls()
        {
            btnCompose.Visibility = Visibility.Collapsed;
            btnInbox.Visibility = Visibility.Collapsed;
            btnLogout.Visibility = Visibility.Collapsed;
        }

        private void btnInbox_Click(object sender, RoutedEventArgs e)
        {
            contentContainer.Content = new Inbox();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
