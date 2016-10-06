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
        HtmlWeb web = new HtmlWeb();

        protected static IMongoClient client = new MongoClient("mongodb://intelTechs:umdeniwedb@196.253.61.51:27017/itrw321DB");
        protected static IMongoDatabase db = client.GetDatabase("itrw321D");

        protected static OracleConnection conn;

        public CrawlerForm()
        {
            InitializeComponent();
            openConnection();            
        }

        public class MovieData
        {
            public string Link { get; set; }
            public string Name { get; set; }
            public string Year { get; set; }
            public string Rating { get; set; }
            public string Overview { get; set; }
            public string Language { get; set; }
            public string Runtime { get; set; }
            public string Budget { get; set; }
            public string Revenue { get; set; }
            public string HomePage { get; set; }
            public string Release { get; set; }

            public IEnumerable<Person> Cast { get; set; }
        }

        public class Person
        {
            public string Name { get; set; }
            public string Department { get; set; }
            public string Role { get; set; }
            public string Character { get; set; }
        }

        protected void openConnection()
        {
            //conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=196.253.61.51)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL))); User Id = IntelTechs; Password = umdeniwedb");
            //conn.Open();
        }

        protected void closeConnection()
        {
            conn.Close();
        }

        protected async Task<List<MovieData>> crawl(int finalPage)
        {
            string url = "https://www.themoviedb.org/movie?";

            if (finalPage != 0 && finalPage > 0)
                url += url + "page=" + finalPage.ToString();

            var webPage = await Task.Factory.StartNew(() => web.Load(url));

            var anchorNodes = webPage.DocumentNode.SelectNodes("//*[@id=\"main\"]/div/div/div/div/a");

            if (anchorNodes == null)
            {
                label1.Text = "No data from webserver";
                return new List<MovieData>();
            }

            var link = anchorNodes.Select(anchor => anchor.GetAttributeValue("href", string.Empty));
            var name = anchorNodes.Select(anchor => anchor.GetAttributeValue("title", string.Empty));

            return link.Zip(name, (links, names) => new MovieData() { Link = "https://www.themoviedb.org"+ links, Name = names }).ToList();
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

                    release = movieDate;
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
                IEnumerable<Person> castZip = null;
                try
                {
                    var castNameNumerable = castNameNode.Select(node => node.InnerText);
                    var castCharacterNumerable = castCharacterNameNode.Select(node => node.InnerText);

                     castZip = castNameNumerable.Zip(castCharacterNumerable, (name, character) => new Person() { Name = name, Character = character });

                    foreach(var person in castZip)
                    {
                        string name = person.Name;
                        string character = person.Character;

                        listBox2.Items.Add(name+" "+character);
                    }
                }
                catch
                {
                    ;
                }

                MovieData data = new MovieData
                {
                    Name = movieName,
                    Year = year,
                    Rating = rating,
                    Overview = overview,
                    Language = language,
                    Runtime = runtime,
                    Budget = budget,
                    Revenue = revenue,
                    Release = revenue,
                    HomePage = homepage,
                    Cast = castZip
                };

                //mongoInsert(data);
            }
            catch
            {
                ;
            }
        }

        protected void mongoInsert(MovieData data)
        {
            var movieCollection = db.GetCollection<MovieData>("Movies");
            movieCollection.InsertOne(data);
            var movieList = movieCollection.Find(new BsonDocument()).ToList();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int pageNum = 0;

            int crawlCount = 0;
            //Small Change
            var moviePages = await crawl(pageNum);

                while(moviePages.Count > 0)
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
    }
}
