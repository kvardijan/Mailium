using Mailium.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    /// Interaction logic for Inbox.xaml
    /// </summary>
    public partial class Inbox : UserControl
    {
        public Inbox()
        {
            InitializeComponent();

            LoadMessagesFromBackend();
        }

        private async Task LoadMessagesFromBackend()
        {
            int clientId = UserManager.GetCurrentUser().Id;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = "http://localhost:5294/Message/getInbox/" + clientId;
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        dynamic jsonResult = JsonConvert.DeserializeObject(responseBody);

                        if (jsonResult.messages != null)
                        {
                            var messages = new ObservableCollection<Mail>();

                            foreach (var jsonMessage in jsonResult.messages)
                            {
                                string title = jsonMessage.title;
                                string content = jsonMessage.content;

                                messages.Add(new Mail
                                {
                                    SenderEmail = jsonMessage.senderEmail,
                                    Title = EncryptionManager.DecryptSymmetric(Convert.FromBase64String(title), clientId.ToString()),
                                    Content = EncryptionManager.DecryptSymmetric(Convert.FromBase64String(content), clientId.ToString()),
                                    SentAt = DateTime.Parse(jsonMessage.sentAt.ToString())
                                });
                            }

                            messages = new ObservableCollection<Mail>(messages.Reverse());

                            lstMessages.ItemsSource = messages;
                        }
                        else
                        {
                            MessageBox.Show("No messages found.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("An error occurred. " + response.ReasonPhrase.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred. " + ex.Message);
                }
            }
        }
    }
}
