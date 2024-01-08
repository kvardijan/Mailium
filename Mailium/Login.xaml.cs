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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        private const string BaseUrl = "http://localhost:5294";
        private static readonly HttpClient client = new HttpClient();

        public Login()
        {
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
                MessageBox.Show("Login success.");
                //open inbox
            }
            else
            {
                MessageBox.Show("Login failed." + response.ReasonPhrase.ToString());
            }
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
    }
}
