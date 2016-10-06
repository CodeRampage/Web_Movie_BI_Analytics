using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//HTML Agility Pack References
using HtmlAgilityPack;
//Mongo References
using MongoDB.Bson;
using MongoDB.Driver;
//Oracle References
using Oracle.DataAccess.Client;

namespace Web_Movie_BI_Analytics
{
    public partial class CrawlerForm : Form
    {
        //My Classes
        LoginController sessionUser;
        DataProcessor.Mongo mongoDataProcessor;
        DataProcessor.Oracle oracleDataProcessor;

        //Classes from other libraries
        HtmlWeb web = new HtmlWeb();

        public CrawlerForm()
        {
            InitializeComponent();

            sessionUser = new LoginController();
            mongoDataProcessor = new DataProcessor.Mongo();
            oracleDataProcessor = new DataProcessor.Oracle();

            string last = "";

           // label1.Text = sessionUser.userLogin("Zakes", "zakes123", ref last) + " " + last;
            //oracleDataProcessor.insertSysUser("Matimu","Matimu","Ngoveni","passit");            
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


                //Cast Nodes
                var castNameNode = webPage.DocumentNode.SelectNodes("//*[@id=\"main_column\"]/ol[1]/li/div/p/a");
                var castCharacterNameNode = webPage.DocumentNode.SelectNodes("//*[@id=\"main_column\"]/ol[1]/li/div/p/span");

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
                string language = null;
                string runtime = null;
                string budget = null;
                string revenue = null;
                string homepage = null;

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
                    language = languageNode.InnerText;
                    runtime = runtimeNode.InnerText;
                    budget = budgetNode.InnerText;
                    revenue = revenueNode.InnerText;
                    homepage = homepageNode.InnerText;
                }
                catch
                {
                    ;
                }

                //Cast & Crew Data
                IEnumerable<ObjectClasses.Person> castZip = null;
                try
                {
                    var castNameNumerable = castNameNode.Select(node => node.InnerText);
                    var castCharacterNumerable = castCharacterNameNode.Select(node => node.InnerText);

                    castZip = castNameNumerable.Zip(castCharacterNumerable, (name, character) => new ObjectClasses.Person() { Name = name, Character = character });

                    foreach (var person in castZip)
                    {
                        string name = person.Name;
                        string character = person.Character;

                        listBox2.Items.Add(name + " " + character);
                    }
                }
                catch
                {
                    ;
                }

                ObjectClasses.MovieData data = new ObjectClasses.MovieData
                {
                    Name = movieName,
                    Year = year,
                    Rating = rating,
                    Overview = overview,
                    Language = language,
                    Runtime = runtime,
                    Budget = budget,
                    Revenue = revenue,
                    Release = release,
                    HomePage = homepage,
                    Cast = castZip
                };

                // mongoDataProcessor.mongoInsert(data);
            }
            catch
            {
                ;
            }
        }

        private async void btnCrawl_Click(object sender, EventArgs e)
        {
            int pageNum = 0;

            int crawlCount = 0;

            var moviePages = await crawl(pageNum);

            while (moviePages.Count > 0)
            {
                foreach (var moviePage in moviePages)
                {
                    listBox1.Items.Add(moviePage.Link);
                    scrapeData(moviePage.Link);
                }

                moviePages = await crawl(++pageNum);
                ++crawlCount;

                if (crawlCount == 1)
                    break;
            }
        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mongoDataProcessor.retrieveMovies();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.txtBoxPassword.AutoSize = false;
            this.txtBoxPassword.Size = new System.Drawing.Size(233, 30);

            this.txtBoxUsername.AutoSize = false;
            this.txtBoxUsername.Size = new System.Drawing.Size(233, 30);

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
            btnAddNewUser.Location = new Point(250, 440);
        }

        private void btnAddNewUser_MouseLeave(object sender, EventArgs e)
        {
            btnAddNewUser.Size = new Size(31, 32);
            btnAddNewUser.Location = new Point(252, 442);
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

        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox2.Size = new Size(110, 89);
        }

        private void pictureBox2_MouseMove_1(object sender, MouseEventArgs e)
        {
            pictureBox2.Size = new Size(70, 63);
            pictureBox2.Location = new Point(893, 20);
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Size = new Size(60, 53);
            pictureBox2.Location = new Point(895, 22);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Would you like to exit the application?","Exit",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
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
            MessageBox.Show("Please contact system Admin at zakes.musa@outlook.com","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
