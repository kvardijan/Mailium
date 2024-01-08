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
        }

        public static User GetCurrentUser()
        {
            return _currentUser;
        }
    }
}
