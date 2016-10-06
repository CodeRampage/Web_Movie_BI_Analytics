using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Oracle.DataAccess.Client;

namespace Web_Movie_BI_Analytics
{
    public class DataProcessor
    {
        //MongoData
        public class Mongo
        {
            protected static IMongoClient client = new MongoClient("mongodb://intelTechs:umdeniwedb@196.253.61.51:27017/itrw321DB");
            protected static IMongoDatabase db = client.GetDatabase("itrw321D");
            protected static IMongoCollection<ObjectClasses.MovieData> movieCollection;

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
                using (var cursor = await movieCollection.Find(new BsonDocument()).ToCursorAsync())
                {
                    while (await cursor.MoveNextAsync())
                    {
                        foreach (var doc in cursor.Current)
                        {
                            Console.WriteLine(pump.name(doc.Name));
                        }
                    }
                }
            }
        }

        //Oracle Data
        public class Oracle
        {

            protected static OracleConnection conn;
            protected static OracleCommand cmd;

            public void openConnection()
            {
                conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=196.253.61.51)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL))); User Id = IntelTechs; Password = umdeniwedb");
                conn.Open();
            }

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
            public string name(string ins)
            {
                return ins;
            }
        }
    }
}
