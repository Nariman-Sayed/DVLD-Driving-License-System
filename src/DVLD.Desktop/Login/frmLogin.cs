using System.Windows.Forms;
using DVLD.Desktop.Common;
using DVLD.Desktop.Services;
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
            if (!btnLogin.Enabled)
            {
                return;
            }

            btnLogin.Enabled = false;

            try
            {
                string username = txtUserName.Text.Trim();
                string password = txtPassword.Text.Trim();

                var request = new LoginRequestDto { Username = username, Password = password };
                var loginResult = await ApiClient.Instance.PostAsync<LoginRequestDto, LoginResponseDto>(
                    "api/auth/login",
                    request);

                if (loginResult is null)
                {
                    MessageBox.Show(
                        "Login succeeded but the server returned an empty response body.",
                        "Login Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                SessionManager.Token = loginResult.Token;
                SessionManager.UserID = loginResult.UserID;
                SessionManager.Username = loginResult.Username;

                SaveRememberedUsername(username);

                var mainForm = new frmMain();
                this.Hide();
                mainForm.ShowDialog();

                SessionManager.ClearSession();
                txtPassword.Clear();
                this.Show();
            }
            catch (ApiException ex) when (ex.IsUnauthorized)
            {
                MessageBox.Show(
                    ex.Message,
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (ApiException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Login Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An unexpected error occurred: {ex.Message}",
                    "Login Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
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
