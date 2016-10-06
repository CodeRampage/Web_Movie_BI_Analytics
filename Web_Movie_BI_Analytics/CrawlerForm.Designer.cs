namespace Web_Movie_BI_Analytics
{
    partial class CrawlerForm
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
            this.btnCrawl = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LoginPanel = new System.Windows.Forms.Panel();
            this.LoginContainer = new System.Windows.Forms.Panel();
            this.btnSignIn = new System.Windows.Forms.Button();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.linkLblForgotPass = new System.Windows.Forms.LinkLabel();
            this.lblinfo1 = new System.Windows.Forms.Label();
            this.txtBoxUsername = new System.Windows.Forms.TextBox();
            this.txtBoxPassword = new System.Windows.Forms.TextBox();
            this.pnlSignUp = new System.Windows.Forms.Panel();
            this.txtConfirmPass = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.btnSignUp = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.pnlDashboard = new System.Windows.Forms.Panel();
            this.pnlDashMenu = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnAddNewUser = new System.Windows.Forms.PictureBox();
            this.PicBoxHandSign = new System.Windows.Forms.PictureBox();
            this.PicBoxUserIcon = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.LoginPanel.SuspendLayout();
            this.LoginContainer.SuspendLayout();
            this.pnlSignUp.SuspendLayout();
            this.pnlDashboard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddNewUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxHandSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxUserIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCrawl
            // 
            this.btnCrawl.Location = new System.Drawing.Point(230, 302);
            this.btnCrawl.Name = "btnCrawl";
            this.btnCrawl.Size = new System.Drawing.Size(75, 23);
            this.btnCrawl.TabIndex = 0;
            this.btnCrawl.Text = "Crawl Links";
            this.btnCrawl.UseVisualStyleBackColor = true;
            this.btnCrawl.Click += new System.EventHandler(this.btnCrawl_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(16, 10);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(307, 225);
            this.listBox1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.listBox2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.btnCrawl);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(687, 337);
            this.panel1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(369, 301);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(351, 10);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(320, 225);
            this.listBox2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 312);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // LoginPanel
            // 
            this.LoginPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.LoginPanel.Controls.Add(this.pictureBox2);
            this.LoginPanel.Controls.Add(this.LoginContainer);
            this.LoginPanel.Controls.Add(this.pnlSignUp);
            this.LoginPanel.Controls.Add(this.pnlDashboard);
            this.LoginPanel.Location = new System.Drawing.Point(0, 0);
            this.LoginPanel.Name = "LoginPanel";
            this.LoginPanel.Size = new System.Drawing.Size(984, 661);
            this.LoginPanel.TabIndex = 3;
            // 
            // LoginContainer
            // 
            this.LoginContainer.Controls.Add(this.btnAddNewUser);
            this.LoginContainer.Controls.Add(this.btnSignIn);
            this.LoginContainer.Controls.Add(this.lblInfo2);
            this.LoginContainer.Controls.Add(this.PicBoxHandSign);
            this.LoginContainer.Controls.Add(this.linkLblForgotPass);
            this.LoginContainer.Controls.Add(this.lblinfo1);
            this.LoginContainer.Controls.Add(this.PicBoxUserIcon);
            this.LoginContainer.Controls.Add(this.txtBoxUsername);
            this.LoginContainer.Controls.Add(this.txtBoxPassword);
            this.LoginContainer.Location = new System.Drawing.Point(310, 50);
            this.LoginContainer.Name = "LoginContainer";
            this.LoginContainer.Size = new System.Drawing.Size(351, 510);
            this.LoginContainer.TabIndex = 9;
            // 
            // btnSignIn
            // 
            this.btnSignIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.btnSignIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSignIn.Location = new System.Drawing.Point(125, 290);
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.Size = new System.Drawing.Size(88, 27);
            this.btnSignIn.TabIndex = 3;
            this.btnSignIn.Text = "Sign in";
            this.btnSignIn.UseVisualStyleBackColor = false;
            this.btnSignIn.Click += new System.EventHandler(this.btnSignIn_Click);
            // 
            // lblInfo2
            // 
            this.lblInfo2.AutoSize = true;
            this.lblInfo2.BackColor = System.Drawing.Color.Transparent;
            this.lblInfo2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblInfo2.Location = new System.Drawing.Point(211, 382);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(58, 13);
            this.lblInfo2.TabIndex = 7;
            this.lblInfo2.Text = "password?";
            // 
            // linkLblForgotPass
            // 
            this.linkLblForgotPass.AutoSize = true;
            this.linkLblForgotPass.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLblForgotPass.LinkColor = System.Drawing.Color.MidnightBlue;
            this.linkLblForgotPass.Location = new System.Drawing.Point(164, 382);
            this.linkLblForgotPass.Name = "linkLblForgotPass";
            this.linkLblForgotPass.Size = new System.Drawing.Size(49, 13);
            this.linkLblForgotPass.TabIndex = 6;
            this.linkLblForgotPass.TabStop = true;
            this.linkLblForgotPass.Text = "Forgoten";
            this.linkLblForgotPass.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblForgotPass_LinkClicked);
            // 
            // lblinfo1
            // 
            this.lblinfo1.AutoSize = true;
            this.lblinfo1.BackColor = System.Drawing.Color.Transparent;
            this.lblinfo1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblinfo1.Location = new System.Drawing.Point(65, 382);
            this.lblinfo1.Name = "lblinfo1";
            this.lblinfo1.Size = new System.Drawing.Size(104, 13);
            this.lblinfo1.TabIndex = 5;
            this.lblinfo1.Text = "Sign in, or have you ";
            // 
            // txtBoxUsername
            // 
            this.txtBoxUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxUsername.Location = new System.Drawing.Point(50, 221);
            this.txtBoxUsername.Name = "txtBoxUsername";
            this.txtBoxUsername.Size = new System.Drawing.Size(233, 26);
            this.txtBoxUsername.TabIndex = 1;
            this.txtBoxUsername.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxUsername.Enter += new System.EventHandler(this.txtBoxUsername_Enter);
            this.txtBoxUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBoxUsername_KeyDown);
            this.txtBoxUsername.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBoxUsername_KeyUp);
            this.txtBoxUsername.Leave += new System.EventHandler(this.txtBoxUsername_Leave);
            this.txtBoxUsername.MouseLeave += new System.EventHandler(this.txtBoxUsername_MouseLeave);
            this.txtBoxUsername.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtBoxUsername_MouseMove);
            // 
            // txtBoxPassword
            // 
            this.txtBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxPassword.Location = new System.Drawing.Point(50, 256);
            this.txtBoxPassword.Name = "txtBoxPassword";
            this.txtBoxPassword.Size = new System.Drawing.Size(233, 26);
            this.txtBoxPassword.TabIndex = 2;
            this.txtBoxPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxPassword.UseSystemPasswordChar = true;
            this.txtBoxPassword.Enter += new System.EventHandler(this.txtBoxPassword_Enter);
            this.txtBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBoxPassword_KeyDown);
            this.txtBoxPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBoxPassword_KeyUp);
            this.txtBoxPassword.Leave += new System.EventHandler(this.txtBoxPassword_Leave);
            // 
            // pnlSignUp
            // 
            this.pnlSignUp.Controls.Add(this.pictureBox3);
            this.pnlSignUp.Controls.Add(this.txtConfirmPass);
            this.pnlSignUp.Controls.Add(this.txtFirstName);
            this.pnlSignUp.Controls.Add(this.txtLastName);
            this.pnlSignUp.Controls.Add(this.btnSignUp);
            this.pnlSignUp.Controls.Add(this.pictureBox1);
            this.pnlSignUp.Controls.Add(this.linkLabel1);
            this.pnlSignUp.Controls.Add(this.label3);
            this.pnlSignUp.Controls.Add(this.txtUserName);
            this.pnlSignUp.Controls.Add(this.txtPassword);
            this.pnlSignUp.Location = new System.Drawing.Point(301, 42);
            this.pnlSignUp.Name = "pnlSignUp";
            this.pnlSignUp.Size = new System.Drawing.Size(369, 528);
            this.pnlSignUp.TabIndex = 10;
            this.pnlSignUp.Visible = false;
            // 
            // txtConfirmPass
            // 
            this.txtConfirmPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmPass.Location = new System.Drawing.Point(77, 354);
            this.txtConfirmPass.Name = "txtConfirmPass";
            this.txtConfirmPass.Size = new System.Drawing.Size(233, 26);
            this.txtConfirmPass.TabIndex = 17;
            this.txtConfirmPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtConfirmPass.Enter += new System.EventHandler(this.txtConfirmPass_Enter);
            this.txtConfirmPass.Leave += new System.EventHandler(this.txtConfirmPass_Leave);
            // 
            // txtFirstName
            // 
            this.txtFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstName.Location = new System.Drawing.Point(77, 214);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(233, 26);
            this.txtFirstName.TabIndex = 15;
            this.txtFirstName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtFirstName.Enter += new System.EventHandler(this.txtFirstName_Enter);
            this.txtFirstName.Leave += new System.EventHandler(this.txtFirstName_Leave);
            // 
            // txtLastName
            // 
            this.txtLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.Location = new System.Drawing.Point(77, 249);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(233, 26);
            this.txtLastName.TabIndex = 16;
            this.txtLastName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLastName.Enter += new System.EventHandler(this.txtLastName_Enter);
            this.txtLastName.Leave += new System.EventHandler(this.txtLastName_Leave);
            // 
            // btnSignUp
            // 
            this.btnSignUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.btnSignUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSignUp.Location = new System.Drawing.Point(154, 386);
            this.btnSignUp.Name = "btnSignUp";
            this.btnSignUp.Size = new System.Drawing.Size(88, 27);
            this.btnSignUp.TabIndex = 10;
            this.btnSignUp.Text = "Sign up";
            this.btnSignUp.UseVisualStyleBackColor = false;
            this.btnSignUp.Click += new System.EventHandler(this.btnSignUp_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.MidnightBlue;
            this.linkLabel1.Location = new System.Drawing.Point(211, 478);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(40, 13);
            this.linkLabel1.TabIndex = 13;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Cancel";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(160, 478);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Sign up or  ";
            // 
            // txtUserName
            // 
            this.txtUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(77, 284);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(233, 26);
            this.txtUserName.TabIndex = 8;
            this.txtUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUserName.Enter += new System.EventHandler(this.txtUserName_Enter);
            this.txtUserName.Leave += new System.EventHandler(this.txtUserName_Leave);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(77, 319);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(233, 26);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            // 
            // pnlDashboard
            // 
            this.pnlDashboard.Controls.Add(this.pictureBox4);
            this.pnlDashboard.Controls.Add(this.pnlDashMenu);
            this.pnlDashboard.Location = new System.Drawing.Point(0, 0);
            this.pnlDashboard.Name = "pnlDashboard";
            this.pnlDashboard.Size = new System.Drawing.Size(984, 661);
            this.pnlDashboard.TabIndex = 12;
            this.pnlDashboard.Visible = false;
            this.pnlDashboard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDashboard_Paint);
            // 
            // pnlDashMenu
            // 
            this.pnlDashMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(90)))), ((int)(((byte)(100)))));
            this.pnlDashMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlDashMenu.Name = "pnlDashMenu";
            this.pnlDashMenu.Size = new System.Drawing.Size(295, 661);
            this.pnlDashMenu.TabIndex = 0;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::Web_Movie_BI_Analytics.Properties.Resources.back_left_arrow_outline_symbol_in_black_circular_button;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox4.Location = new System.Drawing.Point(911, 12);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(61, 63);
            this.pictureBox4.TabIndex = 1;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::Web_Movie_BI_Analytics.Properties.Resources.exit__1_;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(895, 22);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(60, 53);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            this.pictureBox2.MouseLeave += new System.EventHandler(this.pictureBox2_MouseLeave);
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseMove_1);
            // 
            // btnAddNewUser
            // 
            this.btnAddNewUser.BackgroundImage = global::Web_Movie_BI_Analytics.Properties.Resources.Add_User_Male_104;
            this.btnAddNewUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAddNewUser.Location = new System.Drawing.Point(252, 442);
            this.btnAddNewUser.Name = "btnAddNewUser";
            this.btnAddNewUser.Size = new System.Drawing.Size(31, 32);
            this.btnAddNewUser.TabIndex = 9;
            this.btnAddNewUser.TabStop = false;
            this.btnAddNewUser.Click += new System.EventHandler(this.btnAddNewUser_Click);
            this.btnAddNewUser.MouseLeave += new System.EventHandler(this.btnAddNewUser_MouseLeave);
            this.btnAddNewUser.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnAddNewUser_MouseMove);
            // 
            // PicBoxHandSign
            // 
            this.PicBoxHandSign.BackgroundImage = global::Web_Movie_BI_Analytics.Properties.Resources.Natural_User_Interface_2_104;
            this.PicBoxHandSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PicBoxHandSign.Location = new System.Drawing.Point(145, 323);
            this.PicBoxHandSign.Name = "PicBoxHandSign";
            this.PicBoxHandSign.Size = new System.Drawing.Size(59, 56);
            this.PicBoxHandSign.TabIndex = 4;
            this.PicBoxHandSign.TabStop = false;
            // 
            // PicBoxUserIcon
            // 
            this.PicBoxUserIcon.BackgroundImage = global::Web_Movie_BI_Analytics.Properties.Resources.male_user;
            this.PicBoxUserIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PicBoxUserIcon.Location = new System.Drawing.Point(50, 43);
            this.PicBoxUserIcon.Name = "PicBoxUserIcon";
            this.PicBoxUserIcon.Size = new System.Drawing.Size(233, 172);
            this.PicBoxUserIcon.TabIndex = 8;
            this.PicBoxUserIcon.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::Web_Movie_BI_Analytics.Properties.Resources.team;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox3.Location = new System.Drawing.Point(77, 36);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(233, 172);
            this.pictureBox3.TabIndex = 18;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Web_Movie_BI_Analytics.Properties.Resources.Natural_User_Interface_2_104;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(174, 419);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(59, 56);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // CrawlerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.LoginPanel);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "CrawlerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IntelTechs";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.LoginPanel.ResumeLayout(false);
            this.LoginContainer.ResumeLayout(false);
            this.LoginContainer.PerformLayout();
            this.pnlSignUp.ResumeLayout(false);
            this.pnlSignUp.PerformLayout();
            this.pnlDashboard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddNewUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxHandSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxUserIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCrawl;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Panel LoginPanel;
        private System.Windows.Forms.Button btnSignIn;
        private System.Windows.Forms.TextBox txtBoxPassword;
        private System.Windows.Forms.TextBox txtBoxUsername;
        private System.Windows.Forms.PictureBox PicBoxHandSign;
        private System.Windows.Forms.Label lblInfo2;
        private System.Windows.Forms.LinkLabel linkLblForgotPass;
        private System.Windows.Forms.Label lblinfo1;
        private System.Windows.Forms.PictureBox PicBoxUserIcon;
        private System.Windows.Forms.Panel LoginContainer;
        private System.Windows.Forms.PictureBox btnAddNewUser;
        private System.Windows.Forms.Panel pnlSignUp;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Button btnSignUp;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtConfirmPass;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel pnlDashboard;
        private System.Windows.Forms.Panel pnlDashMenu;
        private System.Windows.Forms.PictureBox pictureBox4;
    }
}

