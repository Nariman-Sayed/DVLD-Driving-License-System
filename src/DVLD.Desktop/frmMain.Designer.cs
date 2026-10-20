namespace DVLD
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            msMainMenue = new MenuStrip();
            servicesToolStripMenuItem = new ToolStripMenuItem();
            drivingLicensesToolStripMenuItem = new ToolStripMenuItem();
            oNewDrivingLicenseToolStripMenuItem = new ToolStripMenuItem();
            localLicenseToolStripMenuItem = new ToolStripMenuItem();
            internationalLicenseToolStripMenuItem = new ToolStripMenuItem();
            renewDrivingLicenseToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            releaseDetainedDrivingLicenseToolStripMenuItem = new ToolStripMenuItem();
            retakeTestToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            tsMManageApplications = new ToolStripMenuItem();
            manageLocalDrivingLicenseApplicationsToolStripMenuItem = new ToolStripMenuItem();
            ManageInternationaDrivingLicenseToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            DetainLicensesToolStripMenuItem1 = new ToolStripMenuItem();
            ManageDetainedLicensestoolStripMenuItem1 = new ToolStripMenuItem();
            detainLicenseToolStripMenuItem = new ToolStripMenuItem();
            releaseDetainedLicenseToolStripMenuItem = new ToolStripMenuItem();
            manageApplicationTypesToolStripMenuItem = new ToolStripMenuItem();
            manageTestTypesToolStripMenuItem = new ToolStripMenuItem();
            peopleToolStripMenuItem = new ToolStripMenuItem();
            driversToolStripMenuItem = new ToolStripMenuItem();
            employeesToolStripMenuItem = new ToolStripMenuItem();
            closeToolStripMenuItem = new ToolStripMenuItem();
            currentUserInfoToolStripMenuItem = new ToolStripMenuItem();
            changePasswordToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            signOutToolStripMenuItem = new ToolStripMenuItem();
            lblLoggedInUser = new Label();
            pictureBox1 = new PictureBox();
            btnTestApi = new Button();
            msMainMenue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // msMainMenue
            // 
            msMainMenue.BackColor = Color.White;
            msMainMenue.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            msMainMenue.ImageScalingSize = new Size(20, 20);
            msMainMenue.Items.AddRange(new ToolStripItem[] { servicesToolStripMenuItem, peopleToolStripMenuItem, driversToolStripMenuItem, employeesToolStripMenuItem, closeToolStripMenuItem });
            msMainMenue.LayoutStyle = ToolStripLayoutStyle.Flow;
            msMainMenue.Location = new Point(0, 0);
            msMainMenue.Name = "msMainMenue";
            msMainMenue.Size = new Size(1924, 72);
            msMainMenue.TabIndex = 1;
            msMainMenue.Text = "menuStrip1";
            // 
            // servicesToolStripMenuItem
            // 
            servicesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { drivingLicensesToolStripMenuItem, toolStripSeparator6, tsMManageApplications, toolStripSeparator5, DetainLicensesToolStripMenuItem1, manageApplicationTypesToolStripMenuItem, manageTestTypesToolStripMenuItem });
            servicesToolStripMenuItem.Image = Desktop.Properties.Resources.Applications_64;
            servicesToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            servicesToolStripMenuItem.Name = "servicesToolStripMenuItem";
            servicesToolStripMenuItem.Size = new Size(208, 68);
            servicesToolStripMenuItem.Text = "&Applications";
            servicesToolStripMenuItem.Click += servicesToolStripMenuItem_Click;
            // 
            // drivingLicensesToolStripMenuItem
            // 
            drivingLicensesToolStripMenuItem.BackColor = SystemColors.Control;
            drivingLicensesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { oNewDrivingLicenseToolStripMenuItem, renewDrivingLicenseToolStripMenuItem, toolStripSeparator1, ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem, toolStripSeparator2, releaseDetainedDrivingLicenseToolStripMenuItem, retakeTestToolStripMenuItem1 });
            drivingLicensesToolStripMenuItem.Image = Desktop.Properties.Resources.Driver_License_32;
            drivingLicensesToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            drivingLicensesToolStripMenuItem.Name = "drivingLicensesToolStripMenuItem";
            drivingLicensesToolStripMenuItem.Size = new Size(394, 70);
            drivingLicensesToolStripMenuItem.Text = "&Driving Licenses Services";
            // 
            // oNewDrivingLicenseToolStripMenuItem
            // 
            oNewDrivingLicenseToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { localLicenseToolStripMenuItem, internationalLicenseToolStripMenuItem });
            oNewDrivingLicenseToolStripMenuItem.Image = Desktop.Properties.Resources.New_Driving_License_32;
            oNewDrivingLicenseToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            oNewDrivingLicenseToolStripMenuItem.Name = "oNewDrivingLicenseToolStripMenuItem";
            oNewDrivingLicenseToolStripMenuItem.Size = new Size(509, 38);
            oNewDrivingLicenseToolStripMenuItem.Text = "&New Driving License";
            // 
            // localLicenseToolStripMenuItem
            // 
            localLicenseToolStripMenuItem.Image = Desktop.Properties.Resources.Local_32;
            localLicenseToolStripMenuItem.Name = "localLicenseToolStripMenuItem";
            localLicenseToolStripMenuItem.Size = new Size(296, 32);
            localLicenseToolStripMenuItem.Text = "&Local License";
            localLicenseToolStripMenuItem.Click += localLicenseToolStripMenuItem_Click;
            // 
            // internationalLicenseToolStripMenuItem
            // 
            internationalLicenseToolStripMenuItem.BackColor = Color.White;
            internationalLicenseToolStripMenuItem.Image = Desktop.Properties.Resources.International_32;
            internationalLicenseToolStripMenuItem.Name = "internationalLicenseToolStripMenuItem";
            internationalLicenseToolStripMenuItem.Size = new Size(296, 32);
            internationalLicenseToolStripMenuItem.Text = "&International License";
            internationalLicenseToolStripMenuItem.Click += internationalLicenseToolStripMenuItem_Click;
            // 
            // renewDrivingLicenseToolStripMenuItem
            // 
            renewDrivingLicenseToolStripMenuItem.Image = Desktop.Properties.Resources.Renew_Driving_License_32;
            renewDrivingLicenseToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            renewDrivingLicenseToolStripMenuItem.Name = "renewDrivingLicenseToolStripMenuItem";
            renewDrivingLicenseToolStripMenuItem.Size = new Size(509, 38);
            renewDrivingLicenseToolStripMenuItem.Text = "&Renew Driving License";
            renewDrivingLicenseToolStripMenuItem.Click += renewDrivingLicenseToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(506, 6);
            // 
            // ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem
            // 
            ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem.Image = Desktop.Properties.Resources.Damaged_Driving_License_32;
            ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem.Name = "ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem";
            ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem.Size = new Size(509, 38);
            ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem.Text = "Replacement for Lost or &Damaged License";
            ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem.Click += ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(506, 6);
            // 
            // releaseDetainedDrivingLicenseToolStripMenuItem
            // 
            releaseDetainedDrivingLicenseToolStripMenuItem.Image = Desktop.Properties.Resources.Detained_Driving_License_32;
            releaseDetainedDrivingLicenseToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            releaseDetainedDrivingLicenseToolStripMenuItem.Name = "releaseDetainedDrivingLicenseToolStripMenuItem";
            releaseDetainedDrivingLicenseToolStripMenuItem.Size = new Size(509, 38);
            releaseDetainedDrivingLicenseToolStripMenuItem.Text = "Release Detained Driving License";
            releaseDetainedDrivingLicenseToolStripMenuItem.Click += releaseDetainedDrivingLicenseToolStripMenuItem_Click;
            // 
            // retakeTestToolStripMenuItem1
            // 
            retakeTestToolStripMenuItem1.Image = Desktop.Properties.Resources.Retake_Test_32;
            retakeTestToolStripMenuItem1.ImageScaling = ToolStripItemImageScaling.None;
            retakeTestToolStripMenuItem1.Name = "retakeTestToolStripMenuItem1";
            retakeTestToolStripMenuItem1.Size = new Size(509, 38);
            retakeTestToolStripMenuItem1.Text = "Retake Test";
            retakeTestToolStripMenuItem1.Click += retakeTestToolStripMenuItem1_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(391, 6);
            // 
            // tsMManageApplications
            // 
            tsMManageApplications.DropDownItems.AddRange(new ToolStripItem[] { manageLocalDrivingLicenseApplicationsToolStripMenuItem, ManageInternationaDrivingLicenseToolStripMenuItem1 });
            tsMManageApplications.Image = Desktop.Properties.Resources.Manage_Applications_64;
            tsMManageApplications.ImageScaling = ToolStripItemImageScaling.None;
            tsMManageApplications.Name = "tsMManageApplications";
            tsMManageApplications.Size = new Size(394, 70);
            tsMManageApplications.Text = "Manage Applications";
            // 
            // manageLocalDrivingLicenseApplicationsToolStripMenuItem
            // 
            manageLocalDrivingLicenseApplicationsToolStripMenuItem.Image = Desktop.Properties.Resources.LocalDriving_License;
            manageLocalDrivingLicenseApplicationsToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            manageLocalDrivingLicenseApplicationsToolStripMenuItem.Name = "manageLocalDrivingLicenseApplicationsToolStripMenuItem";
            manageLocalDrivingLicenseApplicationsToolStripMenuItem.Size = new Size(434, 38);
            manageLocalDrivingLicenseApplicationsToolStripMenuItem.Text = "Local Driving License Applications";
            manageLocalDrivingLicenseApplicationsToolStripMenuItem.Click += manageLocalDrivingLicenseApplicationsToolStripMenuItem_Click;
            // 
            // ManageInternationaDrivingLicenseToolStripMenuItem1
            // 
            ManageInternationaDrivingLicenseToolStripMenuItem1.Image = Desktop.Properties.Resources.International_32;
            ManageInternationaDrivingLicenseToolStripMenuItem1.ImageScaling = ToolStripItemImageScaling.None;
            ManageInternationaDrivingLicenseToolStripMenuItem1.Name = "ManageInternationaDrivingLicenseToolStripMenuItem1";
            ManageInternationaDrivingLicenseToolStripMenuItem1.Size = new Size(434, 38);
            ManageInternationaDrivingLicenseToolStripMenuItem1.Text = "International License Applications";
            ManageInternationaDrivingLicenseToolStripMenuItem1.Click += ManageInternationaDrivingLicenseToolStripMenuItem1_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(391, 6);
            // 
            // DetainLicensesToolStripMenuItem1
            // 
            DetainLicensesToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { ManageDetainedLicensestoolStripMenuItem1, detainLicenseToolStripMenuItem, releaseDetainedLicenseToolStripMenuItem });
            DetainLicensesToolStripMenuItem1.Image = Desktop.Properties.Resources.Detain_64;
            DetainLicensesToolStripMenuItem1.ImageScaling = ToolStripItemImageScaling.None;
            DetainLicensesToolStripMenuItem1.Name = "DetainLicensesToolStripMenuItem1";
            DetainLicensesToolStripMenuItem1.Size = new Size(394, 70);
            DetainLicensesToolStripMenuItem1.Text = "Detain Licenses";
            // 
            // ManageDetainedLicensestoolStripMenuItem1
            // 
            ManageDetainedLicensestoolStripMenuItem1.Image = Desktop.Properties.Resources.Detain_32;
            ManageDetainedLicensestoolStripMenuItem1.ImageScaling = ToolStripItemImageScaling.None;
            ManageDetainedLicensestoolStripMenuItem1.Name = "ManageDetainedLicensestoolStripMenuItem1";
            ManageDetainedLicensestoolStripMenuItem1.Size = new Size(363, 38);
            ManageDetainedLicensestoolStripMenuItem1.Text = "Manage Detained Licenses";
            ManageDetainedLicensestoolStripMenuItem1.Click += ManageDetainedLicensestoolStripMenuItem1_Click;
            // 
            // detainLicenseToolStripMenuItem
            // 
            detainLicenseToolStripMenuItem.Image = Desktop.Properties.Resources.Detain_32;
            detainLicenseToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            detainLicenseToolStripMenuItem.Name = "detainLicenseToolStripMenuItem";
            detainLicenseToolStripMenuItem.Size = new Size(363, 38);
            detainLicenseToolStripMenuItem.Text = "Detain License";
            detainLicenseToolStripMenuItem.Click += detainLicenseToolStripMenuItem_Click;
            // 
            // releaseDetainedLicenseToolStripMenuItem
            // 
            releaseDetainedLicenseToolStripMenuItem.Image = Desktop.Properties.Resources.Release_Detained_License_32;
            releaseDetainedLicenseToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            releaseDetainedLicenseToolStripMenuItem.Name = "releaseDetainedLicenseToolStripMenuItem";
            releaseDetainedLicenseToolStripMenuItem.Size = new Size(363, 38);
            releaseDetainedLicenseToolStripMenuItem.Text = "Release Detained License";
            releaseDetainedLicenseToolStripMenuItem.Click += releaseDetainedLicenseToolStripMenuItem_Click;
            // 
            // manageApplicationTypesToolStripMenuItem
            // 
            manageApplicationTypesToolStripMenuItem.Image = Desktop.Properties.Resources.Application_Types_64;
            manageApplicationTypesToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            manageApplicationTypesToolStripMenuItem.Name = "manageApplicationTypesToolStripMenuItem";
            manageApplicationTypesToolStripMenuItem.Size = new Size(394, 70);
            manageApplicationTypesToolStripMenuItem.Text = "Manage Application Types";
            manageApplicationTypesToolStripMenuItem.Click += manageApplicationTypesToolStripMenuItem_Click;
            // 
            // manageTestTypesToolStripMenuItem
            // 
            manageTestTypesToolStripMenuItem.Image = Desktop.Properties.Resources.Test_Type_64;
            manageTestTypesToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            manageTestTypesToolStripMenuItem.Name = "manageTestTypesToolStripMenuItem";
            manageTestTypesToolStripMenuItem.Size = new Size(394, 70);
            manageTestTypesToolStripMenuItem.Text = "Manage Test Types";
            manageTestTypesToolStripMenuItem.Click += manageTestTypesToolStripMenuItem_Click;
            // 
            // peopleToolStripMenuItem
            // 
            peopleToolStripMenuItem.Image = Desktop.Properties.Resources.People_64;
            peopleToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            peopleToolStripMenuItem.Name = "peopleToolStripMenuItem";
            peopleToolStripMenuItem.Size = new Size(153, 68);
            peopleToolStripMenuItem.Text = "People";
            peopleToolStripMenuItem.Click += peopleToolStripMenuItem_Click;
            // 
            // driversToolStripMenuItem
            // 
            driversToolStripMenuItem.Image = Desktop.Properties.Resources.Drivers_64;
            driversToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            driversToolStripMenuItem.Name = "driversToolStripMenuItem";
            driversToolStripMenuItem.Size = new Size(158, 68);
            driversToolStripMenuItem.Text = "Drivers";
            driversToolStripMenuItem.Click += driversToolStripMenuItem_Click;
            // 
            // employeesToolStripMenuItem
            // 
            employeesToolStripMenuItem.Image = Desktop.Properties.Resources.Users_2_64;
            employeesToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            employeesToolStripMenuItem.Name = "employeesToolStripMenuItem";
            employeesToolStripMenuItem.Size = new Size(141, 68);
            employeesToolStripMenuItem.Text = "Users";
            employeesToolStripMenuItem.Click += employeesToolStripMenuItem_Click;
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { currentUserInfoToolStripMenuItem, changePasswordToolStripMenuItem, toolStripSeparator4, signOutToolStripMenuItem });
            closeToolStripMenuItem.Image = Desktop.Properties.Resources.account_settings_64;
            closeToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new Size(251, 68);
            closeToolStripMenuItem.Text = "Account Settings";
            // 
            // currentUserInfoToolStripMenuItem
            // 
            currentUserInfoToolStripMenuItem.Image = Desktop.Properties.Resources.PersonDetails_32;
            currentUserInfoToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            currentUserInfoToolStripMenuItem.Name = "currentUserInfoToolStripMenuItem";
            currentUserInfoToolStripMenuItem.Size = new Size(275, 38);
            currentUserInfoToolStripMenuItem.Text = "&Current User Info";
            currentUserInfoToolStripMenuItem.Click += currentUserInfoToolStripMenuItem_Click;
            // 
            // changePasswordToolStripMenuItem
            // 
            changePasswordToolStripMenuItem.Image = Desktop.Properties.Resources.Password_32;
            changePasswordToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            changePasswordToolStripMenuItem.Size = new Size(275, 38);
            changePasswordToolStripMenuItem.Text = "Change Password";
            changePasswordToolStripMenuItem.Click += changePasswordToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(272, 6);
            // 
            // signOutToolStripMenuItem
            // 
            signOutToolStripMenuItem.Image = Desktop.Properties.Resources.sign_out_32__2;
            signOutToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            signOutToolStripMenuItem.Name = "signOutToolStripMenuItem";
            signOutToolStripMenuItem.Size = new Size(275, 38);
            signOutToolStripMenuItem.Text = "Sign &Out";
            signOutToolStripMenuItem.Click += signOutToolStripMenuItem_Click;
            // 
            // lblLoggedInUser
            // 
            lblLoggedInUser.AutoSize = true;
            lblLoggedInUser.BackColor = SystemColors.Control;
            lblLoggedInUser.Location = new Point(1201, 1067);
            lblLoggedInUser.Name = "lblLoggedInUser";
            lblLoggedInUser.Size = new Size(99, 20);
            lblLoggedInUser.TabIndex = 4;
            lblLoggedInUser.Text = "[UserName]";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Black;
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Center;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = Desktop.Properties.Resources.newpic;
            pictureBox1.Location = new Point(0, 72);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1924, 983);
            pictureBox1.TabIndex = 10;
            pictureBox1.TabStop = false;
            // 
            // btnTestApi
            // 
            btnTestApi.Location = new Point(400, 219);
            btnTestApi.Name = "btnTestApi";
            btnTestApi.Size = new Size(94, 29);
            btnTestApi.TabIndex = 11;
            btnTestApi.Text = "TestApi";
            btnTestApi.UseVisualStyleBackColor = true;
            btnTestApi.Click += btnTestApi_Click;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImage = Desktop.Properties.Resources.newpic;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1924, 1055);
            Controls.Add(btnTestApi);
            Controls.Add(pictureBox1);
            Controls.Add(lblLoggedInUser);
            Controls.Add(msMainMenue);
            DoubleBuffered = true;
            Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            IsMdiContainer = true;
            MainMenuStrip = msMainMenue;
            Margin = new Padding(4);
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main";
            WindowState = FormWindowState.Maximized;
            Load += frmMain_Load;
            msMainMenue.ResumeLayout(false);
            msMainMenue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMainMenue;
        private System.Windows.Forms.ToolStripMenuItem servicesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drivingLicensesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oNewDrivingLicenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renewDrivingLicenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReplacementLostOrDamagedDrivingLicenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem peopleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem driversToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem employeesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localLicenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem internationalLicenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem releaseDetainedDrivingLicenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem retakeTestToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Label lblLoggedInUser;
        private System.Windows.Forms.ToolStripMenuItem currentUserInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem signOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem manageApplicationTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageTestTypesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsMManageApplications;
        private System.Windows.Forms.ToolStripMenuItem manageLocalDrivingLicenseApplicationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem ManageInternationaDrivingLicenseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem DetainLicensesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem detainLicenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem releaseDetainedLicenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ManageDetainedLicensestoolStripMenuItem1;
        private PictureBox pictureBox1;
        private Button btnTestApi;
    }
}

