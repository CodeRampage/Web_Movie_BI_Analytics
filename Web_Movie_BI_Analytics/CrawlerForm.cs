﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows.Forms.DataVisualization.Charting;

namespace Web_Movie_BI_Analytics
{
    public partial class CrawlerForm : Form
    {
        //My Classes
        LoginController sessionUser;
        DataProcessor.Mongo mongoDataProcessor;
        DataProcessor.Oracle oracleDataProcessor;
        DataProcessor.DataPump pump;

        //The timer
        Timer t = new Timer();
        int interval = 0;

        //Variables for menue design
        int CrawlerCounterId = 0;
        int DashboardCounterId = 0;
        int SystemSettingsCounterId = 0;

        //Classes from other libraries
        HtmlWeb web = new HtmlWeb();

        //User session variables
        string fname = null;
        string lname = null;
        string user_type = null;

        public CrawlerForm()
        {
            InitializeComponent();

            sessionUser = new LoginController();
            mongoDataProcessor = new DataProcessor.Mongo();
            oracleDataProcessor = new DataProcessor.Oracle();
            pump = new DataProcessor.DataPump();

            string last = "";

            //label1.Text = sessionUser.userLogin("Zakes", "zakes123", ref last,ref user_type) + " " + last;          
        }

        protected async Task<List<ObjectClasses.MovieData>> crawl(int finalPage)
        {
            string url = "https://www.themoviedb.org/movie?";

            if (finalPage != 0 && finalPage > 0)
                url += url + "page=" + finalPage.ToString();

            var webPage = await Task.Factory.StartNew(() => web.Load(url));

            var anchorNodes = webPage.DocumentNode.SelectNodes("//*[@id=\"main\"]/div/div/div/div/a");

            if (anchorNodes == null)
            {
                label1.Text = "No data from webserver";
                return new List<ObjectClasses.MovieData>();
            }

            var link = anchorNodes.Select(anchor => anchor.GetAttributeValue("href", string.Empty));
            var name = anchorNodes.Select(anchor => anchor.GetAttributeValue("title", string.Empty));

            return link.Zip(name, (links, names) => new ObjectClasses.MovieData() { Link = "https://www.themoviedb.org" + links, Name = names }).ToList();
        }

        protected async void scrapeData(string item)
        {
            string movieLink = item + "/cast";

            var webPage = await Task.Factory.StartNew(() => web.Load(movieLink));

            try
            {
                var nameNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"section_header\"]/div[1]/h2/a");
                var yearNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"section_header\"]/div[1]/h2/a/span");
                var ratingNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"section_header\"]/div[1]/div/div/span[2]");
                var overviewNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"overview\"]/p/text()");
                var releaseStatusNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"left_column\"]/section[1]/p[1]/text()");
                var languageNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"left_column\"]/section[1]/p[2]/text()");
                var runtimeNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"left_column\"]/section[1]/p[3]/text()");
                var budgetNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"left_column\"]/section[1]/p[4]/text()");
                var revenueNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"left_column\"]/section[1]/p[5]/text()");
                var homepageNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"left_column\"]/section[1]/p[6]/a");
                var releaseDateNode = webPage.DocumentNode.SelectNodes("//*[@id=\"left_column\"]/section[1]/ul/li/text()");
                var genreNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"left_column\"]/section[2]/ul/li[1]/a");

                //Cast Nodes
                var castNameNode = webPage.DocumentNode.SelectNodes("//*[@id=\"main_column\"]/ol[1]/li/div/p/a");
                var castCharacterNameNode = webPage.DocumentNode.SelectNodes("//*[@id=\"main_column\"]/ol[1]/li/div/p/span");
                var castLinkNode = webPage.DocumentNode.SelectSingleNode("//*[@id=\"main_column\"]/ol[1]/li[1]/div/p/a");

                {
                    //Crew Nodes
                    var crewNameNode = webPage.DocumentNode.SelectNodes("//*[@id=\"main_column\"]/ol[2]/li/div/p/a");
                    var crewRoleNode = webPage.DocumentNode.SelectNodes("//*[@id=\"main_column\"]/ol[2]/li/div/p/text()");

                    //Directing Nodes
                    var directingNameNode = webPage.DocumentNode.SelectNodes("//*[@id=\"main_column\"]/ol[3]/li/div/p/a");
                    var directingRoleNode = webPage.DocumentNode.SelectNodes("//*[@id=\"main_column\"]/ol[3]/li/div/p/text()");

                    //Writing Nodes
                    var writingNameNode = webPage.DocumentNode.SelectNodes("//*[@id=\"main_column\"]/ol[4]/li/div/p/a");
                    var writingRoleNode = webPage.DocumentNode.SelectNodes("//*[@id=\"main_column\"]/ol[4]/li/div/p/text()");
                }

                string movieName = null;
                string year = null;
                string rating = null;
                string overview = null;
                string release = null;
                string releaseStatus = null;
                string language = null;
                string runtime = null;
                string budget = null;
                string revenue = null;
                string homepage = null;
                string genre = null;
                string castLink = null;

                //Movie Data
                try
                {
                    movieName = nameNode.InnerText;
                    year = yearNode.InnerText;
                    rating = ratingNode.InnerText;
                    overview = overviewNode.InnerText;

                    var releaseDates = releaseDateNode.Select(node => node.InnerText);
                    string movieDate = null;
                    foreach (var date in releaseDates)
                    {
                        movieDate += " " + date;
                    }

                    release = movieDate.Trim();
                    releaseStatus = releaseStatusNode.InnerText;
                    language = languageNode.InnerText;
                    runtime = runtimeNode.InnerText;
                    budget = budgetNode.InnerText;
                    revenue = revenueNode.InnerText;
                    homepage = homepageNode.InnerText;
                    genre = genreNode.InnerText;
                }
                catch
                {
                    ;
                }

                IEnumerable<ObjectClasses.Person> zipCast = null;
                List<ObjectClasses.Person> castList = new List<ObjectClasses.Person>();

                foreach (var act in castNameNode)
                {
                    //Cast & Crew Data
                    castLink = act.GetAttributeValue("href", string.Empty);
                    castLink = "https://www.themoviedb.org" + castLink;

                    try
                    {
                        var actorPage = await Task.Factory.StartNew(() => web.Load(castLink));
                        var genderNode = actorPage.DocumentNode.SelectNodes("//*[@id=\"media_v4\"]/div/section/p[1]/text()");
                        var birthNode = actorPage.DocumentNode.SelectNodes("//*[@id=\"media_v4\"]/div/section/p[4]/text()");
                        var creditsNode = actorPage.DocumentNode.SelectNodes("//*[@id=\"media_v4\"]/div/section/p[2]/text()");


                        var genderNumerable = genderNode.Select(node => node.InnerText);
                        var birthNumerable = birthNode.Select(node => node.InnerText);
                        var creditsNumerable = creditsNode.Select(node => node.InnerText);

                        var castNameNumerable = castNameNode.Select(node => node.InnerText);
                        var castCharacterNumerable = castCharacterNameNode.Select(node => node.InnerText);

  
                        zipCast = genderNumerable.Zip(birthNumerable, (first, second) => new ObjectClasses.Person() { Gender = first, BirthPlace = second }).Zip(
                            creditsNumerable, (first, second) => new ObjectClasses.Person() { Gender = first.Gender, BirthPlace = first.BirthPlace, Credits = second }).Zip(
                            castNameNumerable, (first, second) => new ObjectClasses.Person() { Gender = first.Gender, BirthPlace = first.BirthPlace, Credits = first.Credits, Name = act.InnerText }).Zip(
                            castCharacterNumerable, (first, second) => new ObjectClasses.Person() { Gender = first.Gender, BirthPlace = first.BirthPlace, Credits = first.Credits, Name = first.Name, Character = second });


                        foreach (ObjectClasses.Person person in zipCast)
                        {
                            string name = person.Name;
                            string character = person.Character;
                            castList.Add(person);
                            listBox4.Items.Add(name + " " + character + " " + person.BirthPlace + " " + person.Gender + " " + person.Credits);
                        }
                    }
                    catch(System.IO.IOException) 
                    {
                        ;
                    }
                    catch (Exception)
                    {
                        ;
                    }
                }


                ////Cast & Crew Data
                //castLink = castLinkNode.GetAttributeValue("href", string.Empty);
                //castLink = "https://www.themoviedb.org" + castLink;

                //var actorPage = await Task.Factory.StartNew(() => web.Load(castLink));

                //IEnumerable<ObjectClasses.Person> zipCast = null;
                //try
                //{
                //    var genderNode = actorPage.DocumentNode.SelectNodes("//*[@id=\"media_v4\"]/div/section/p[1]/text()");
                //    var birthNode = actorPage.DocumentNode.SelectNodes("//*[@id=\"media_v4\"]/div/section/p[4]/text()");
                //    var creditsNode = actorPage.DocumentNode.SelectNodes("//*[@id=\"media_v4\"]/div/section/p[2]/text()");

                //    var genderNumerable = genderNode.Select(node => node.InnerText);
                //    var birthNumerable = birthNode.Select(node => node.InnerText);
                //    var creditsNumerable = creditsNode.Select(node => node.InnerText);

                //    var castNameNumerable = castNameNode.Select(node => node.InnerText);
                //    var castCharacterNumerable = castCharacterNameNode.Select(node => node.InnerText);

                //    zipCast = genderNumerable.Zip(birthNumerable, (first, second) => new ObjectClasses.Person() { Gender = first, BirthPlace = second }).Zip(
                //        creditsNumerable, (first, second) => new ObjectClasses.Person() { Gender = first.Gender, BirthPlace = first.BirthPlace, Credits = second }).Zip(
                //        castNameNumerable, (first, second) => new ObjectClasses.Person() { Gender = first.Gender, BirthPlace = first.BirthPlace, Credits = first.Credits, Name = second }).Zip(
                //        castCharacterNumerable, (first, second) => new ObjectClasses.Person() { Gender = first.Gender, BirthPlace = first.BirthPlace, Credits = first.Credits, Name = first.Name, Character = second });

                //    foreach (var person in zipCast)
                //    {
                //        string name = person.Name;
                //        string character = person.Character;

                //        listBox4.Items.Add(name + " " + character+" "+person.BirthPlace + " "+person.Gender+" "+person.Credits);
                //    }
                //}
                //catch
                //{
                //    ;
                //}

                ObjectClasses.MovieData data = new ObjectClasses.MovieData
                {
                    Link = movieLink,
                    Name = movieName,
                    Year = year,
                    Rating = rating,
                    Overview = overview,
                    Language = language,
                    Runtime = runtime,
                    Budget = budget,
                    Revenue = revenue,
                    Release = release,
                    ReleaseStatus = releaseStatus,
                    HomePage = homepage,
                    Genre = genre,
                    Cast = castList
                };

                mongoDataProcessor.mongoInsert(data);
            }
            catch
            {
                ;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pump.retrieveMovies();
        }

        private async void btnCrawlData_Click(object sender, EventArgs e)
        {
            int pageNum = 0;

            int crawlCount = 0;

            var moviePages = await crawl(pageNum);

            while (moviePages.Count > 0)
            {
                foreach (var moviePage in moviePages)
                {
                    listBox3.Items.Add(moviePage.Link);
                    scrapeData(moviePage.Link);
                }

                moviePages = await crawl(++pageNum);
                ++crawlCount;

                if (crawlCount == 1)
                    break;
            }
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            pump.retrieveMovies();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCommitData_Click(object sender, EventArgs e)
        {
            string fname = txtSystemUserFirstName.Text;
            string lname = txtSystemuserLastName.Text;
            string pass = txtSystemUserPassword.Text;
            string confirmPass = txtSystemUserConfirmPass.Text;
            string user_type = DropTypeOfUser.SelectedText;

            if (rdbAddUser.Checked)
            {
                if(pass==confirmPass)
                    oracleDataProcessor.insertSysUser(fname, lname, pass, confirmPass, user_type);
                else
                {
                    MessageBox.Show(this, "Error: 201: Passwords do not match", "Unmatching Passwords Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (rdbEditUser.Checked)
            {


               // sessionUser.updateUser();
            }
            else if (rdbRemoveUser.Checked)
            {
                
            }
        }

        private void btnScraper_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Textbox design along with drop downs
            lblDeleteUser.Visible = false;
            this.txtBoxPassword.AutoSize = false;
            this.txtBoxPassword.Size = new System.Drawing.Size(233, 30);

            this.txtBoxUsername.AutoSize = false;
            this.txtBoxUsername.Size = new System.Drawing.Size(233, 30);

            this.txtSystemUserFirstName.AutoSize = false;
            this.txtSystemUserFirstName.Size = new System.Drawing.Size(384, 30);

            this.txtSystemuserLastName.AutoSize = false;
            this.txtSystemuserLastName.Size = new System.Drawing.Size(384, 30);

            this.txtSystemUserPassword.AutoSize = false;
            this.txtSystemUserPassword.Size = new System.Drawing.Size(205, 30);

            this.txtSystemUserConfirmPass.AutoSize = false;
            this.txtSystemUserConfirmPass.Size = new System.Drawing.Size(205, 30);

            this.DropTypeOfUser.AutoSize = false;
            this.DropTypeOfUser.Size = new System.Drawing.Size(315, 30);

            this.dropUserToDeleteOrEdit.AutoSize = false;
            this.dropUserToDeleteOrEdit.Size = new System.Drawing.Size(223, 30);



            txtBoxUsername.Text = "Username";
            txtBoxPassword.Text = "Password";    
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            LoginPanel.Show();
            LoginPanel.BringToFront();
            panel1.Hide();
        }

        private void btnAddNewUser_MouseMove(object sender, MouseEventArgs e)
        {
            btnAddNewUser.Size = new Size(41, 42);
            btnAddNewUser.Location = new Point(256, 460);
        }

        private void btnAddNewUser_MouseLeave(object sender, EventArgs e)
        {
            btnAddNewUser.Size = new Size(31, 32);
            btnAddNewUser.Location = new Point(258, 462);
        }

        private void txtBoxUsername_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        private void txtBoxUsername_KeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void txtBoxUsername_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void txtBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void txtBoxPassword_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void txtBoxUsername_Leave(object sender, EventArgs e)
        {
            if (txtBoxUsername.Text == "")
            {
                txtBoxUsername.Text = "Username";
            }
        }

        private void txtBoxUsername_Enter(object sender, EventArgs e)
        {
            if (txtBoxUsername.Text == "")
            {
                txtBoxUsername.Text = "Password";
            }

            txtBoxUsername.Text = "";
        }

        private void txtBoxUsername_MouseLeave(object sender, EventArgs e)
        {
                
        }

        private void txtBoxPassword_Enter(object sender, EventArgs e)
        {
            if (txtBoxUsername.Text == "")
            {
                txtBoxUsername.Text = "Username";
            }

            txtBoxPassword.Text = "";
        }

        private void txtBoxPassword_Leave(object sender, EventArgs e)
        {
            if (txtBoxPassword.Text == "")
            {
                txtBoxPassword.Text = "Password";
            }
        }

        private void txtFirstName_Leave(object sender, EventArgs e)
        {
            if(txtFirstName.Text =="")
            {
                txtFirstName.Text = "First name";
            }
        }

        private void txtLastName_Leave(object sender, EventArgs e)
        {
            if (txtLastName.Text == "")
            {
                txtLastName.Text = "Last name";
            }
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
            {
                txtUserName.Text = "User name";
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "Password";               
            }

            if (!(txtPassword.Equals("Password")))
                txtPassword.UseSystemPasswordChar = true;
            else
                txtPassword.UseSystemPasswordChar = false;
        }

        private void txtFirstName_Enter(object sender, EventArgs e)
        {
            if (txtFirstName.Text == "")
            {
                txtFirstName.Text = "Password";
                txtLastName.Text = "Last name";
                txtUserName.Text = "User name";
                txtPassword.Text = "Password";
                txtPassword.UseSystemPasswordChar = true;
                txtConfirmPass.Text = "Confirm password";
                txtConfirmPass.UseSystemPasswordChar = true;
            }

            txtFirstName.Text = "";
        }

        private void txtConfirmPass_Leave(object sender, EventArgs e)
        {
            if (txtConfirmPass.Text == "")
            {
                txtConfirmPass.Text = "Confirm Password";
                if (!(txtConfirmPass.Equals("Confirm password")))
                    txtConfirmPass.UseSystemPasswordChar = true;
            }

            if (!(txtConfirmPass.Equals("Confirm password")))
                txtConfirmPass.UseSystemPasswordChar = true;
            else
                txtConfirmPass.UseSystemPasswordChar = false;
        }

        private void txtConfirmPass_Enter(object sender, EventArgs e)
        {
            if (txtConfirmPass.Text == "")
            {
                txtFirstName.Text = "Password";
                txtLastName.Text = "Last name";
                txtUserName.Text = "User name";
                txtPassword.Text = "Password";
                txtPassword.UseSystemPasswordChar = true;
                txtConfirmPass.Text = "Confirm password";
                txtConfirmPass.UseSystemPasswordChar = true;
            }

            txtConfirmPass.Text = "";
        }

        private void txtLastName_Enter(object sender, EventArgs e)
        {
            if (txtLastName.Text == "")
            {
                txtFirstName.Text = "Password";
                txtLastName.Text = "Last name";
                txtUserName.Text = "User name";
                txtPassword.Text = "Password";
                txtPassword.UseSystemPasswordChar = true;
                txtConfirmPass.Text = "Confirm password";
                txtConfirmPass.UseSystemPasswordChar = true;
            }

            txtLastName.Text = "";
        }

        private void txtUserName_Enter(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
            {
                txtFirstName.Text = "Password";
                txtLastName.Text = "Last name";
                txtUserName.Text = "User name";
                txtPassword.Text = "Password";
                txtPassword.UseSystemPasswordChar = true;
                txtConfirmPass.Text = "Confirm password";
                txtConfirmPass.UseSystemPasswordChar = true;
            }

            txtUserName.Text = "";
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtFirstName.Text = "Password";
                txtLastName.Text = "Last name";
                txtUserName.Text = "User name";
                txtPassword.Text = "Password";
                txtPassword.UseSystemPasswordChar = true;
                txtConfirmPass.Text = "Confirm password";
                txtConfirmPass.UseSystemPasswordChar = true;
            }

            txtPassword.Text = "";
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtBoxUsername.Text;
                string pass = txtBoxPassword.Text;
                
                fname = sessionUser.userLogin(username, pass, ref lname,ref user_type);

                if (fname== "null")
                {
                    MessageBox.Show("Incorrect username or password.");
                }
                else
                {
                    pnlDashboard.Visible = true;
                    LoginContainer.Visible = false;
                    pnlSignUp.Visible = false;
                    lblUserLogedIn.Text = fname + " " + lname;
                    lblTypeOfUserLoggedIn.Text = user_type;
                }
            }
            catch
            {
                ;
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string name = txtFirstName.Text;
            string last = txtLastName.Text;
            string pass = txtPassword.Text;
            string repeatPass = txtConfirmPass.Text;
            string username = txtBoxUsername.Text;

            if (pass == repeatPass)
                oracleDataProcessor.insertSysUser(username, name, last, "General System User", pass);
            else
                MessageBox.Show("Error 201: Passwords do not match!", "Unmatching passwords", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox2.Size = new Size(110, 89);
        }

        private void pictureBox2_MouseMove_1(object sender, MouseEventArgs e)
        {
            pictureBox2.Size = new Size(70, 63);
            pictureBox2.Location = new Point(891, 506);

            lblSlyzaTsotsi.Visible = true;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Size = new Size(60, 53);
            pictureBox2.Location = new Point(893, 508);

            lblSlyzaTsotsi.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you really want to Exit the Application?","Exit Confirm Dialog",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            pnlSignUp.Visible = true;
            LoginContainer.Visible = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginContainer.Visible = true;
            pnlSignUp.Visible = false;
        }

        private void linkLblForgotPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Please contact system Admin at Zakes.Musa@hotmail.com\n\t\t for your password","Forgot Password Dialog",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void pnlDashboard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you really want to sign out?","Confirm Sign Out Dialog",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                pnlDashboard.Visible = false;
                LoginContainer.Visible = true;
                pnlSignUp.Visible = false;
            }            
        }

        private void BackDoor_MouseDown(object sender, MouseEventArgs e)
        {
            t.Interval = 1000; // specify interval time as you want
            t.Tick += new EventHandler(timer_Tick);
            t.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            counter();

            if (interval > 10)
            {
                pnlDashboard.Visible = true;
            }
        }

        private void BackDoor_MouseUp(object sender, MouseEventArgs e)
        {
            t.Stop();
        }

        private int counter()
        {
            interval++;

            return interval;
        }

        private void pictureBox4_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox4.Size = new Size(71, 57);
            pictureBox4.Location = new Point(909, 10);

            lblSignOut.Visible = true;            
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Size = new Size(61, 47);
            pictureBox4.Location = new Point(911, 12);

            lblSignOut.Visible = false;
        }

        private void lblSystemManagement_MouseMove(object sender, MouseEventArgs e)
        {
            lblSystemManagement.Font = new Font(lblSystemManagement.Font.FontFamily, 16);
        }

        private void lblSystemManagement_MouseLeave(object sender, EventArgs e)
        {
            if (SystemSettingsCounterId == 0)
            {
                lblSystemManagement.Font = new Font(lblSystemManagement.Font.FontFamily, 14);
            }
        }

        private void lblSystemManagement_MouseClick(object sender, MouseEventArgs e)
        {
            //picBoxSystemManagement.Size = new Size(44, 42);
            //picBoxSystemManagement.Location = new Point(48, 546);
        }

        private void lblSystemManagement_Click(object sender, EventArgs e)
        {
            picBoxSystemManagement.Size = new Size(54, 52);
            picBoxSystemManagement.Location = new Point(40, 545);
            lblSystemManagement.Font = new Font(lblSystemManagement.Font.FontFamily, 16);

            SystemSettingsCounterId++;
            DashboardCounterId = 0;
            CrawlerCounterId = 0;

            lblScrape.Visible = false;
            picBoxDashComponents.Size = new Size(44, 42);
            picBoxDashComponents.Location = new Point(48, 485);
            label4.Font = new Font(label4.Font.FontFamily, 14);

            picBoxCrawler.Size = new Size(44, 42);
            picBoxCrawler.Location = new Point(48, 413);
            label6.Font = new Font(label6.Font.FontFamily, 14);
            picBoxCrawler.BackgroundImage = Properties.Resources.spider;           

            pnlCrawler.Visible = false;
            pnlDashboardComponents.Visible = false;
            btnScraper.Visible = false;
            lblScrape.Visible = false;
            pnlSystemManagement.Visible = true;

            picBoxSystemManagement.BackgroundImage = Properties.Resources.management__1_;

            picBoxDashComponents.BackgroundImage = Properties.Resources.icon__1_;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            picBoxSystemManagement.Size = new Size(44, 42);
            picBoxSystemManagement.Location = new Point(48, 560);
            lblSystemManagement.Font = new Font(lblSystemManagement.Font.FontFamily, 14);

            picBoxDashComponents.Size = new Size(54, 52);
            picBoxDashComponents.Location = new Point(45, 479);
            label4.Font = new Font(label4.Font.FontFamily, 16);
            lblScrape.Visible = false;
            picBoxDashComponents.BackgroundImage = Properties.Resources.icon__2_;

            picBoxSystemManagement.BackgroundImage = Properties.Resources.management;

            DashboardCounterId++;
            CrawlerCounterId = 0;
            SystemSettingsCounterId = 0;

            picBoxCrawler.Size = new Size(44, 42);
            picBoxCrawler.Location = new Point(48, 413);
            label6.Font = new Font(label6.Font.FontFamily, 14);
            picBoxCrawler.BackgroundImage = Properties.Resources.spider;            

            pnlCrawler.Visible = false;
            pnlDashboardComponents.Visible = true;
            btnScraper.Visible = true;            
            pnlSystemManagement.Visible = false;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            picBoxSystemManagement.Size = new Size(44, 42);
            picBoxSystemManagement.Location = new Point(48, 560);
            lblSystemManagement.Font = new Font(lblSystemManagement.Font.FontFamily, 14);

            picBoxDashComponents.Size = new Size(44, 42);
            picBoxDashComponents.Location = new Point(48, 485);
            label4.Font = new Font(label4.Font.FontFamily, 14);
            picBoxDashComponents.BackgroundImage = Properties.Resources.icon__1_;
            lblScrape.Visible = false;

            picBoxSystemManagement.BackgroundImage = Properties.Resources.management;

            picBoxCrawler.Size = new Size(54, 52);
            picBoxCrawler.Location = new Point(48, 405);
            label6.Font = new Font(label6.Font.FontFamily, 16);
            picBoxCrawler.BackgroundImage = Properties.Resources.spiderwhite;
            
            CrawlerCounterId++;
            DashboardCounterId = 0;
            SystemSettingsCounterId = 0;

            pnlCrawler.Visible = true;
            pnlDashboardComponents.Visible = false;
            btnScraper.Visible = false;
            lblScrape.Visible = false;
            pnlSystemManagement.Visible = false;
        }

        private void label4_MouseMove(object sender, MouseEventArgs e)
        {
            label4.Font = new Font(label4.Font.FontFamily, 16);
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            if (DashboardCounterId == 0)
            {
                label4.Font = new Font(label4.Font.FontFamily, 14);
            }
        }

        private void label6_MouseMove(object sender, MouseEventArgs e)
        {            
            label6.Font = new Font(label6.Font.FontFamily, 16);                   
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            if (CrawlerCounterId == 0)
            {
                label6.Font = new Font(label6.Font.FontFamily, 14);
            }           
        }

        private void rdbAddUser_CheckedChanged(object sender, EventArgs e)
        {
            dropUserToDeleteOrEdit.Visible = false;
            lblDeleteUser.Visible = false;
            panel4.Visible = false;

            picBoxDeco10.Visible = true;

            lblDeleteUser.Visible = false;
            lblCommitData.Visible = false;

            txtSystemUserFirstName.Visible = true;
            txtSystemuserLastName.Visible = true;
            txtSystemUserPassword.Visible = true;
            txtSystemUserConfirmPass.Visible = true;
            DropTypeOfUser.Visible = true;

            label5.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;

            btnCommitData.Visible = true;            
        }

        private void rdbEditUser_CheckedChanged(object sender, EventArgs e)
        {
            dropUserToDeleteOrEdit.Visible = true;
            lblDeleteUser.Visible = false;
            panel4.Visible = false;

            picBoxDeco10.Visible = false;

            lblDeleteUser.Visible = false;
            lblCommitData.Visible = false;

            txtSystemUserFirstName.Visible = true;
            txtSystemuserLastName.Visible = true;
            txtSystemUserPassword.Visible = true;
            txtSystemUserConfirmPass.Visible = true;
            DropTypeOfUser.Visible = true;

            label5.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;

            addUsers();
        }


        public void addUsers()
        {
            List<ObjectClasses.System_user> user = sessionUser.retrieveAllUsers();

            dropUserToDeleteOrEdit.DisplayMember = "NAMES";
            dropUserToDeleteOrEdit.ValueMember = "USERN";
            dropUserToDeleteOrEdit.DataSource = user;
        }


        private void rdbRemoveUser_CheckedChanged(object sender, EventArgs e)
        {
            dropUserToDeleteOrEdit.Visible = true;
            lblDeleteUser.Visible = true;
            panel4.Visible = true;

            picBoxDeco10.Visible = false;

            lblDeleteUser.Visible = false;
            lblCommitData.Visible = false;

            txtSystemUserFirstName.Visible = false;
            txtSystemuserLastName.Visible = false;
            txtSystemUserPassword.Visible = false;
            txtSystemUserConfirmPass.Visible = false;
            DropTypeOfUser.Visible = false;

            label5.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;

            btnCommitData.Visible = false;
            lblCommitData.Visible = false;

            addUsers();
        }

        private void btnCommitData_MouseMove(object sender, MouseEventArgs e)
        {
            btnCommitData.Size = new Size(52, 52);
            btnCommitData.Location = new Point(462, 481);

            lblCommitData.Visible = true;
        }

        private void btnCommitData_MouseLeave(object sender, EventArgs e)
        {
            btnCommitData.Size = new Size(42, 42);
            btnCommitData.Location = new Point(464, 483);

            lblCommitData.Visible = false;
        }

        private void btnScraper_MouseMove(object sender, MouseEventArgs e)
        {
            btnScraper.Size = new Size(53, 57);
            btnScraper.Location = new Point(613, 10);

            lblScrape.Visible = true;
        }

        private void btnScraper_MouseLeave(object sender, EventArgs e)
        {
            btnScraper.Size = new Size(43, 47);
            btnScraper.Location = new Point(615, 12);

            lblScrape.Visible = false;
        }

        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            panel4.Size = new Size(53, 52);
            panel4.Location = new Point(312, 200);

            lblDeleteUser.Visible = true;
        }

        private void panel4_MouseLeave(object sender, EventArgs e)
        {
            panel4.Size = new Size(43, 42);
            panel4.Location = new Point(314, 202);

            lblDeleteUser.Visible = false;
        }
        
        private void btnCrawlData_MouseMove(object sender, MouseEventArgs e)
        {
            btnCrawlData.Size = new Size(78, 67);
            btnCrawlData.Location = new Point(313, 208);

            lblCrawlBtn.Visible = true;
        }

        private void btnCrawlData_MouseLeave(object sender, EventArgs e)
        {
            btnCrawlData.Size = new Size(68, 57);
            btnCrawlData.Location = new Point(315, 210);

            lblCrawlBtn.Visible = false;
        }

        private void chcBoxAllPages_CheckedChanged(object sender, EventArgs e)
        {
            if(chcBoxAllPages.Checked)
            {
                trcBarPagesToCrawl.Enabled = false;
            }
            else
            {
                trcBarPagesToCrawl.Enabled = true;
            }            
        }
    }
}
