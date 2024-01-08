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
    /// Interaction logic for Compose.xaml
    /// </summary>
    public partial class Compose : UserControl
    {
        public Compose()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string recipient = txtRecipient.Text;
            string subject = txtSubject.Text;
            string content = txtContent.Text;

            if (string.IsNullOrWhiteSpace(recipient) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await SendEmail(recipient, subject, content);
        }

        private async Task SendEmail(string recipient, string subject, string content)
        {
            //TODO: KRIPTIRAJ
            string encryptedContent = content;
            string encryptedSubject = subject;
            int senderId = UserManager.GetCurrentUser().Id;

            var messageDTO = new { senderId = senderId, receiverEmail = recipient, title = encryptedSubject, content = encryptedContent, sentAt = DateTime.Now.ToString() };

            var json = JsonConvert.SerializeObject(messageDTO);
            var mailContent = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost:5294/Message/send", mailContent);
                if(response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Poruka poslana!");
                    txtRecipient.Clear();
                    txtSubject.Clear();
                    txtContent.Clear();
                }
                else
                {
                    MessageBox.Show("Poruka nije poslana!");
                }
            }
        }
    }
}
