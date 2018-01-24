using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class Profile
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;

        public string UpProfile(string _id, string _name, string _pass, string _retype, string _dept)
        {
            DataTable hasil = new DataTable();
            string time = "NG";
            SqlConnection connection = _Conn.Connect();
            try
            {
                    string sql1 = "SELECT TOP 1000 [USER_ID],[USER_NAME],[USER_DEPT],[USER_AUTH]  FROM [WPC].[dbo].[TBL_LOGIN] WHERE [USER_ID]='" + _id + "' AND [USER_PASS] = '" + _pass + "'";
                    command = new SqlCommand(sql1, connection);
                    connection.Open();
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        connection.Close();
                        connection.Open();
                        string sql = string.Format("UPDATE [dbo].[TBL_LOGIN]  SET [USER_NAME] = '"+_name+"',[USER_PASS] = '" + _retype + "',[USER_DEPT] = '" + _dept + "',[USER_ACTIVE] = 'Y' WHERE [USER_ID] = '" + _id + "'");
                        command = new SqlCommand(sql, connection);
                        int numberOfRecords = command.ExecuteNonQuery();
                        if (numberOfRecords > 0) time = "OK";
                    }
                    else
                    {
                        time = "NG";
                    }
            }
            catch { }
            connection.Close();

            return time;
        }
    }
}
