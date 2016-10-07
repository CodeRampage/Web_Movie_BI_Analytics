using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Oracle.DataAccess.Client;
using System.Text.RegularExpressions;

namespace Web_Movie_BI_Analytics
{
    public class DataProcessor
    {
        protected static OracleConnection conn;
        protected static OracleCommand cmd;

        public static void openConnection()
        {
            conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=196.253.61.51)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL))); User Id = IntelTechs; Password = umdeniwedb");
            conn.Open();
        }

        //MongoData
        public class Mongo
        {
            protected static IMongoCollection<ObjectClasses.MovieData> movieCollection;
            protected static IMongoClient client = new MongoClient("mongodb://intelTechs:umdeniwedb@196.253.61.51:27017/itrw321DB");
            protected static IMongoDatabase db = client.GetDatabase("itrw321D");

            DataPump pump = new DataPump();

            public void mongoInsert(ObjectClasses.MovieData data)
            {
                movieCollection = db.GetCollection<ObjectClasses.MovieData>("Movies");
                movieCollection.InsertOne(data);
                //var movieList = movieCollection.Find(new BsonDocument()).ToList();
            }

            public void mongoResetCollections()
            {

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

                            pump.movieInsert(doc.Name,doc.Year,"Released",doc.Rating,doc.Overview,doc.Release,doc.Budget,doc.Revenue,doc.HomePage,"Genre");

                            foreach (var name in cast)
                            {
                                //pump.castInsert();
                            }
                        }
                    }
                }
            }
        }

        //Oracle Data
        public class Oracle
        {
            public void closeConnection()
            {
                conn.Close();
            }

            public void insertSysUser(string username, string fname, string lname, string password)
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
                    cmd.ExecuteNonQuery();
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

                rating = rating.Replace(".", ",");
                
                budget = Regex.Replace(budget, @"[$.\n\n]", "");
                budget = budget.Substring(0, budget.Length - 2);
                
                revenue = Regex.Replace(revenue, @"[$.\n]", "");
                revenue = revenue.Substring(0, revenue.Length - 1);

                float rate = (float) Convert.ToDouble(rating);
                //int movie_budget = Convert.ToInt32(budget.Trim());
                //float movie_revenue = (float)Convert.ToDouble(revenue);

                Console.WriteLine(name+" "+date);
                Console.WriteLine(revenue.Trim());

                //openConnection();
                //using (conn)
                //{
                //    cmd = new OracleCommand("INSERT_MOVIE", conn);
                //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //    cmd.Parameters.Add("movie_name", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = name;
                //    cmd.Parameters.Add("movie_year", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = year;
                //    cmd.Parameters.Add("release_status", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = release;
                //    cmd.Parameters.Add("rating", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = rating;
                //    cmd.Parameters.Add("overview", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = overview;
                //    cmd.Parameters.Add("release_", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = date;
                //    cmd.Parameters.Add("budget", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = budget;
                //    cmd.Parameters.Add("revenue", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = revenue;
                //    cmd.Parameters.Add("home_", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = home;
                //    cmd.Parameters.Add("genre_id", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = genre;
                //    cmd.ExecuteNonQuery();
                //    conn.Close();
                //}
            }

            

            public void castInsert()
            {

            }
        }
    }
}
