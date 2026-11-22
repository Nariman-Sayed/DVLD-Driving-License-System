using System;
using System.Drawing;
using System.Windows.Forms;
using DVLD.Desktop.Services;

namespace DVLD.Desktop
{
    public partial class frmMain : Form
    {
        private bool _closingForUnauthorized;

        public frmMain()
        {
            InitializeComponent();
            ApiClient.Instance.Unauthorized += ApiClient_Unauthorized;
            FormClosed += FrmMain_FormClosed;
        }

        private void FrmMain_FormClosed(object? sender, FormClosedEventArgs e)
        {
            ApiClient.Instance.Unauthorized -= ApiClient_Unauthorized;
        }

        private void ApiClient_Unauthorized(object? sender, EventArgs e)
        {
            if (_closingForUnauthorized)
            {
                return;
            }

            _closingForUnauthorized = true;

            MessageBox.Show(
                "Your session has expired. Please sign in again.",
                "Session Expired",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            Close();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            lblLoggedInUser.Text = "LoggedIn User: (not implemented yet)";
            this.Refresh();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void retakeTestToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void vehiclesLicensesServicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void manageLocalDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ManageInternationaDrivingLicenseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ManageDetainedLicensestoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void servicesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private async void btnTestApi_Click(object sender, EventArgs e)
        {
            if (!btnTestApi.Enabled)
            {
                return;
            }

            btnTestApi.Enabled = false;

            try
            {
                string content = await ApiClient.Instance.GetStringAsync("api/person");

                MessageBox.Show($"Success!\n\n{content}", "API Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ApiException ex)
            {
                // A 401 is already handled by ApiClient_Unauthorized, which closes this form.
                if (!ex.IsUnauthorized)
                {
                    MessageBox.Show(ex.Message, "API Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "API Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    btnTestApi.Enabled = true;
                }
            }
        }
    }
}