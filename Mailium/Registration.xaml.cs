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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : UserControl
    {
        private const string BaseUrl = "http://localhost:5294";
        private static readonly HttpClient client = new HttpClient();
        ContentControl _contentContainer;

        public Registration(ContentControl contentContainer)
        {
            _contentContainer = contentContainer;
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                SendRegistrationRequestAsync();
            }
        }

        private async void SendRegistrationRequestAsync()
        {
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            string name = txtUsername.Text;
            var body = new { email = email, password = password, name = name };
            string bodyJson = JsonConvert.SerializeObject(body);
            var parameters = new StringContent(bodyJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{BaseUrl}/Auth/register", parameters);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("Registration success.");
                _contentContainer.Content = new Login(_contentContainer);
            }
            else
            {
                MessageBox.Show("Registration failed." + response.ReasonPhrase.ToString());
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Password) || string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("One or both of the fields are empty.");
                return false;
            }
            return true;
        }
    }
}
