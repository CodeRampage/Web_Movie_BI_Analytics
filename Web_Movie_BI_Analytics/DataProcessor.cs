using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Oracle.ManagedDataAccess.Client;
using System.Text.RegularExpressions;

namespace Web_Movie_BI_Analytics
{
    public class DataProcessor
    {
        protected static OracleConnection conn;
        protected static OracleCommand cmd;

        protected static IMongoCollection<ObjectClasses.MovieData> movieCollection;

        //protected static IMongoClient client = new MongoClient("mongodb://intelTechs:umdeniwedb@196.253.61.51:27017/itrw321DB");
        //protected static IMongoDatabase db = client.GetDatabase("itrw321D");

        protected static IMongoClient client = new MongoClient("mongodb://105.184.215.78:47975/audioalbums");
        protected static IMongoDatabase db = client.GetDatabase("audioalbums");

        public static void openConnection()
        {
            conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=196.253.61.51)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL))); User Id = IntelTechs; Password = umdeniwedb");
            conn.Open();
        }

        //MongoData
        public class Mongo
        {
            DataPump pump = new DataPump();

            public void mongoInsert(ObjectClasses.MovieData data)
            {
                movieCollection = db.GetCollection<ObjectClasses.MovieData>("Movies");
                movieCollection.InsertOne(data);
            }

            public async void mongoResetCollections()
            {
                movieCollection = db.GetCollection<ObjectClasses.MovieData>("Movies");
                var filter = new BsonDocument();
                var result = await movieCollection.DeleteManyAsync(filter);
            }
        }

        //Oracle Data
        public class Oracle
        {
            public void closeConnection()
            {
                conn.Close();
            }

            public void insertSysUser(string username, string fname, string lname, string password, string user_type)
            {
                openConnection();
                using (conn)
                {
                    cmd = new OracleCommand("INSERT_SYSTEM_USER", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("usern", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = username;
                    cmd.Parameters.Add("user_first", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = fname;
                    cmd.Parameters.Add("user_last", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = lname;
                    cmd.Parameters.Add("user_pass", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = password;
                    cmd.Parameters.Add("user_t", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = user_type;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            public async void deleteMovies()
            {
                openConnection();
                using (conn)
                {
                    cmd = new OracleCommand("DELETE_DATA", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    await Task.Factory.StartNew(() =>cmd.ExecuteNonQuery());
                    conn.Close();
                }
            }
        }

        public class DataPump
        {
            public void movieInsert(string name,string year,string release,string rating,string overview,string date,string budget,string revenue,string home,string genre)
            {
                name = name.Substring(0,name.Length-6).Trim();
                year = Regex.Replace(year, @"[()]|[\n]{2}", "");

                date = Regex.Replace(date, @"[,\n\t]|[\n]{2}", "");
                date = date.Trim();
                //date = date.Replace(' ', '.').Trim();
                string[] dateValues = date.Split(' ');
                date = dateValues[1] + "/" + dateValues[0] + "/" + dateValues[2];

                budget = Regex.Replace(budget, @"[$.,\n\n]", "");
                budget = budget.Substring(0, budget.Length - 2);
                budget = budget.Trim();
                
                revenue = Regex.Replace(revenue, @"[$.\n,]", "");
                revenue = revenue.Substring(0, revenue.Length - 2);
                revenue = revenue.Trim();

                if (release == null)
                    release = "Unknown";
                release = release.Trim();

                if (genre == null)
                    genre = "No genre";

                int movie_budget = 0;
                int movie_revenue = 0;

                if (budget.Length > 1)
                {
                    movie_budget = Convert.ToInt32(budget.Trim());
                }

                if (revenue.Length > 1)
                {
                    movie_revenue = Convert.ToInt32(revenue.Trim());
                }

                int total_cost = 0;

                if (movie_revenue>=0)
                    total_cost = movie_revenue - movie_budget;

                openConnection();
                using (conn)
                {
                    cmd = new OracleCommand("INSERT_MOVIE", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("movie_name", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = name;
                    cmd.Parameters.Add("movie_year", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = year;
                    cmd.Parameters.Add("release_status", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = release;
                    cmd.Parameters.Add("rating", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = rating;
                    cmd.Parameters.Add("overview", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = overview;
                    cmd.Parameters.Add("_release", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = "21/March/2015";
                    cmd.Parameters.Add("_budget", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = movie_budget;
                    cmd.Parameters.Add("_revenue", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = movie_revenue;
                    cmd.Parameters.Add("movie_cost", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = total_cost;
                    cmd.Parameters.Add("home_", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = home;
                    cmd.Parameters.Add("genre_id", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = genre;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            public void castInsert(string name,string character, string gender,string birthPlace, string credits)
            {
                name = name.Trim();
                string [] names = name.Split(' ');

                string first = null;
                string last = null;

                if (names.Length == 0)
                {
                    first = "Unknown";
                    last = "Unknown";
                }
                else if(names.Length == 1)
                {
                    first = names[0];
                    last = "Unknown";
                }
                else if (names.Length == 2)
                {
                    first = names[0];
                    last = names[1];
                }
                else if (names.Length == 3)
                {
                    first = names[0] + " " + names[1];
                    last = names[2];
                }
                else if(names.Length == 4)
                {
                    first = names[0]+" "+names[1]+" "+names[2];
                    last = names[3];
                }

                character = character.Trim();
                first = first.Trim();
                last = last.Trim();

                if (!(character.Length > 1))
                    character = "Unknown";

                if (!(first.Length > 1))
                    first = "Unknown";

                if (!(last.Length > 1))
                    last = "Unknown";

                gender = gender.Trim();
                birthPlace = birthPlace.Trim();
                credits = credits.Trim();

                if (gender == "-")
                    gender = "Uknown";

                if (birthPlace == "--")
                    birthPlace = "Unknown";

                if (first=="Unknown" && last == "Unknown")
                {
                    ;
                }
                else
                {
                    openConnection();
                    using (conn)
                    {
                        cmd = new OracleCommand("INSERT_CAST", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("fname", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = first.Trim();
                        cmd.Parameters.Add("lname", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = last.Trim();
                        cmd.Parameters.Add("movie_character", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = character.Trim();
                        cmd.Parameters.Add("birthPlace", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = birthPlace.Trim();
                        cmd.Parameters.Add("actor_creds", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = credits.Trim();
                        cmd.Parameters.Add("actor_gender", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = gender.Trim();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }

            public async void retrieveMovies()
            {
                movieCollection = db.GetCollection<ObjectClasses.MovieData>("Movies");

                string movieName = null;
                string year = null;
                string rating = null;
                string overview = null;
                string language = null;
                string runtime = null;
                string budget = null;
                string revenue = null;
                string genre = null;
                string releaseStatus = null;

                IEnumerable<ObjectClasses.Person> cast = null;

                using (var cursor = await movieCollection.Find(new BsonDocument()).ToCursorAsync())
                {
                    while (await cursor.MoveNextAsync())
                    {
                        foreach (var doc in cursor.Current)
                        {
                            movieName = doc.Name;
                            year = doc.Year;
                            rating = doc.Overview;
                            overview = doc.Overview;
                            language = doc.Language;
                            runtime = doc.Runtime;
                            budget = doc.Budget;
                            revenue = doc.Revenue;
                            cast = doc.Cast;
                            genre = doc.Genre;
                            releaseStatus = doc.ReleaseStatus;

                            movieInsert(doc.Name, doc.Year, releaseStatus, doc.Rating, doc.Overview, doc.Release, doc.Budget, doc.Revenue, doc.HomePage, genre);

                            foreach (var star in cast)
                            {
                                await Task.Factory.StartNew(() => castInsert(star.Name, star.Character, star.Gender, star.BirthPlace, star.Credits));
                            }
                        }
                    }
                }
            }
            //End
        }//End Class
    }
}