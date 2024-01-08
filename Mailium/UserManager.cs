using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Mailium
{
    internal static class UserManager
    {
        private static User _currentUser { get; set; } = null;
        public static bool IsLoggedIn()
        {
            return _currentUser != null;
        }

        public static void SetCurrentUser(User user)
        {
            _currentUser = user;
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.lblLoggedUser.Content = user?.Name;
        }

        public static void LogoutUser()
        {
            _currentUser = null;
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.lblLoggedUser.Content = "Not logged in yet";
        }

        public static User GetCurrentUser()
        {
            return _currentUser;
        }
    }
}
