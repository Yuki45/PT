using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repairhhp
{
    public class Conn
    {
        public static string _dbHost = "107.102.47.105";
        public static string _dbName = "WPC";
        public static string _dbUser = "sa";
        public static string _dbPass = "seinadminhhp";
        //private string _dbHost;
        //private string _dbName;
        //private string _dbUser;
        //private string _dbPass;

        private SqlConnection connection;

        public SqlConnection Connection
        {
            get { return connection; }
        }

        public bool IsConnected
        {
            get
            {
                if (connection == null) return false;
                return connection.State == ConnectionState.Open;
            }
        }

        //public Conn(string DbHost, string Dbname, string DbUser, string DbPass)
        //{
        //    _dbHost = DbHost;
        //    _dbName = Dbname;
        //    _dbUser = DbUser;
        //    _dbPass = DbPass;
        //}


        public SqlConnection Connect()
        {
            try
            {
                string connetionString = null;
                SqlConnection connection;
                connetionString = "Data Source=107.102.47.105;Initial Catalog=WPC;User ID=sa;Password=seinadminhhp";
                connection = new SqlConnection(connetionString);
                return connection;
            }
            catch (Exception)
            {
                return connection;
            }

        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                connection.Close();
            }
        }

        public bool IsAvailable
        {
            get
            {
                try
                {
                    SqlConnectionStringBuilder ConnBuilder = new SqlConnectionStringBuilder();

                    ConnBuilder.DataSource = _dbHost;
                    ConnBuilder.Encrypt = true;
                    ConnBuilder.TrustServerCertificate = true;
                    ConnBuilder.InitialCatalog = _dbName;
                    ConnBuilder.PersistSecurityInfo = true;
                    ConnBuilder.UserID = _dbUser;
                    ConnBuilder.Password = _dbPass;
                    ConnBuilder.ConnectTimeout = 380;

                    connection = new SqlConnection(ConnBuilder.ToString());

                    connection.Open();

                    return true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
                finally
                {
                    Disconnect();
                }
            }
        }
    }
}