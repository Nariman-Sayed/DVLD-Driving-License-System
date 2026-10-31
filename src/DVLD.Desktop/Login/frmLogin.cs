using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;
using DVLD.Desktop.Common;
using DVLD.Shared;

namespace DVLD.Desktop.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUserName.Text.Trim();
                string password = txtPassword.Text.Trim();

                using var client = new HttpClient();
                client.BaseAddress = new Uri(ApiSettings.BaseUrl);

                var request = new LoginRequestDto { Username = username, Password = password };
                var response = await client.PostAsJsonAsync("api/auth/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var loginResult = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                    if (loginResult is null)
                    {
                        throw new InvalidOperationException("Login succeeded but the server returned an empty response body.");
                    }

                    SessionManager.Token = loginResult.Token;
                    SessionManager.UserID = loginResult.UserID;
                    SessionManager.Username = loginResult.Username;

                    SaveRememberedUsername(username);

                    frmMain mainForm = new frmMain();
                    this.Hide();
                    mainForm.ShowDialog();
                    this.Close();
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Invalid Username/Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string details = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Login failed. Status: {(int)response.StatusCode} {response.ReasonPhrase}. Body: {details}");
                    MessageBox.Show($"Login failed. Status: {(int)response.StatusCode} {response.ReasonPhrase}", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string rememberedUsername = DVLD.Desktop.Settings.Default.RememberedUsername;

            if (!string.IsNullOrEmpty(rememberedUsername))
            {
                txtUserName.Text = rememberedUsername;
                chkRememberMe.Checked = true;
            }
            else
            {
                txtUserName.Text = string.Empty;
                chkRememberMe.Checked = false;
            }
        }

        private void SaveRememberedUsername(string username)
        {
            if (chkRememberMe.Checked)
            {
                DVLD.Desktop.Settings.Default.RememberedUsername = username;
            }
            else
            {
                DVLD.Desktop.Settings.Default.RememberedUsername = string.Empty;
            }

            DVLD.Desktop.Settings.Default.Save();
        }
    }
}