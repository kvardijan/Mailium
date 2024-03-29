﻿using Newtonsoft.Json;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        private const string BaseUrl = "http://localhost:5294";
        private static readonly HttpClient client = new HttpClient();
        ContentControl _contentContainer;
        private DHKeyExchange clientDH;

        public Login(ContentControl contentContainer)
        {
            _contentContainer = contentContainer;
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                SendLoginRequestAsync();
            }
        }

        private async void SendLoginRequestAsync()
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            var body = new { email = email, password = password };
            string bodyJson = JsonConvert.SerializeObject(body);
            var parameters = new StringContent(bodyJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{BaseUrl}/Auth/login", parameters);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var responseJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                var user = new User
                {
                    Id = int.Parse(responseJson["id"]),
                    Name = responseJson["name"],
                    Email = responseJson["email"]
                };
                UserManager.SetCurrentUser(user);
                EnableLoggedUserControls();
                await CreateSharedSecretAsync();
                _contentContainer.Content = new Inbox();
            }
            else
            {
                MessageBox.Show("Login failed. " + response.ReasonPhrase.ToString());
            }
        }

        private void EnableLoggedUserControls()
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.btnCompose.Visibility = Visibility.Visible;
            mainWindow.btnInbox.Visibility = Visibility.Visible;
            mainWindow.btnLogout.Visibility = Visibility.Visible;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("One or both of the fields are empty.");
                return false;
            }
            return true;
        }

        private async Task CreateSharedSecretAsync()
        {
            string sharedSecret = "";
            clientDH = new DHKeyExchange();
            string clientPublicKey = clientDH.GetClientPublicKey();
            string serverPublicKey = await ExchangeKeys(clientPublicKey);
            sharedSecret = clientDH.GenerateSharedSecret(serverPublicKey);
            SharedSecretManager.SetSharedSecret(sharedSecret);
        }

        private async Task<string> ExchangeKeys(string clientPublicKey)
        {

            var publicKeyDTO = new { clientId = UserManager.GetCurrentUser().Id.ToString(), publicKey = clientPublicKey };
            var json = JsonConvert.SerializeObject(publicKeyDTO);
            var parameters = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{BaseUrl}/Key/exchangeKeys", parameters);
            response.EnsureSuccessStatusCode();

            string serverPublicKey = await response.Content.ReadAsStringAsync();
            return serverPublicKey;
        }
    }
}
