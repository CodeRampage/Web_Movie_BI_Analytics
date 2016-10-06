using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace Web_Movie_BI_Analytics
{
    public class LoginController
    {
        public static OracleConnection conn;
        public static OracleCommand cmd;

        public string USERNAME { get; set; }
        public string FIRST { get; set; }
        public string LAST { get; set; }
        public string PASSWORD { get; set; }

        public void openConnection()
        {
            conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=196.253.61.51)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL))); User Id = IntelTechs; Password = umdeniwedb");
            conn = new OracleConnection("intelTechs/umdeniwedb@196.253.61.51:1521/orcl");
            conn.Open();
        }

        public void closeConnection()
        {
            conn.Close();
        }

        public string userLogin(string user, string pass, ref string last)
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
                cmd.ExecuteNonQuery();

                string fname = cmd.Parameters["user_f"].Value.ToString();
                last = cmd.Parameters["user_l"].Value.ToString();
                conn.Close();
                return fname;
            }
        }
    }
}
