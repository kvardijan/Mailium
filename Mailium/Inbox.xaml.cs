using Mailium.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Mailium
{
    /// <summary>
    /// Interaction logic for Inbox.xaml
    /// </summary>
    public partial class Inbox : UserControl
    {
        public Inbox()
        {
            InitializeComponent();

            LoadMessagesFromBackend();
        }

        private async void LoadMessagesFromBackend()
        {
            var messages = new ObservableCollection<Mail>
            {
                new Mail { SenderEmail = "example@example.com", Title = "Hello", Content = "This is a test message", SentAt = DateTime.Now },
                new Mail { SenderEmail = "another@example.com", Title = "Greetings", Content = "Another test message", SentAt = DateTime.Now }
            };

            lstMessages.ItemsSource = messages;
        }
    }
}
