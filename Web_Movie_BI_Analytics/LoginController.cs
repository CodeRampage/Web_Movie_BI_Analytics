using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Web_Movie_BI_Analytics
{
    public class LoginController
    {
        public static OracleConnection conn;
        public static OracleCommand cmd;

        List<ObjectClasses.System_user> user = new List<ObjectClasses.System_user>();

        public void openConnection()
        {
            conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=196.253.61.51)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL))); User Id = IntelTechs; Password = umdeniwedb");
            conn.Open();
        }

        public void closeConnection()
        {
            conn.Close();
        }

        public string userLogin(string user, string pass, ref string last,ref string user_type)
        {
            openConnection();
            using (conn)
            {
                cmd = new OracleCommand("LOGIN", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("usern", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = user;
                cmd.Parameters.Add("pass", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = pass;
                cmd.Parameters.Add("user_f", OracleDbType.Varchar2, 200).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("user_l", OracleDbType.Varchar2, 200).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("user_t", OracleDbType.Varchar2, 200).Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                string fname = cmd.Parameters["user_f"].Value.ToString();
                last = cmd.Parameters["user_l"].Value.ToString();
                user_type = cmd.Parameters["user_t"].Value.ToString();
                conn.Close();
                return fname;
            }
        }

        public List<ObjectClasses.System_user> retrieveAllUsers()
        {
            openConnection();
            using (conn)
            {
                cmd = new OracleCommand("GET_SYSTE_USERS", conn);
                cmd.CommandType = System.Data.CommandType.TableDirect;

                OracleDataReader reader = cmd.ExecuteReader();
                
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);

                        user.Add(new ObjectClasses.System_user { FIRST = reader.GetString(0), LAST = reader.GetString(1), USERN = reader.GetString(2), PASS = reader.GetString(3), USERT = reader.GetString(4), NAMES = reader.GetString(0)+" "+reader.GetString(1)});
                    }
                }

                conn.Close();

                return user;
            }
        }

        public void updateUser(string username,string first, string last, string user_type,string pass)
        {
            openConnection();
            using (conn)
            {
                cmd = new OracleCommand("UPDATE_USER", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("usern", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = username;
                cmd.Parameters.Add("user_first", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = first;
                cmd.Parameters.Add("user_last", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = last;
                cmd.Parameters.Add("user_pass", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = pass;
                cmd.Parameters.Add("user_t", OracleDbType.Varchar2, System.Data.ParameterDirection.Input).Value = user_type;
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}
