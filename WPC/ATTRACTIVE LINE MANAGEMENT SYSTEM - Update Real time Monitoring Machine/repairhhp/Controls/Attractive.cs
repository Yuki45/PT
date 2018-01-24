using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class Attractive
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;

        public SqlDataAdapter getTable()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT TOP 100  ROW_NUMBER() OVER (ORDER BY CHANGE_DATE DESC) AS NO, TBL_LOGIN.[USER_NAME] ,[REASON] ,[REQUESTER]  ,[DEPT] ,[CHANGE_DATE] AS [ATTRACTIVE DATE] ,[IP_PC] AS [IP PC] " +
             "FROM [WPC].[dbo].[TBL_HISTORY]  inner join [WPC].dbo.TBL_LOGIN on TBL_LOGIN.[USER_ID] = [LOGIN_ID] " +
            "ORDER BY CHANGE_DATE DESC ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public DataTable getData(string sql)
        {
            SqlConnection connection = _Conn.Connect();
            DataTable hasil = new DataTable();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataAdapter reader = new SqlDataAdapter(command);
                reader.Fill(hasil);

            }
            catch
            {
                hasil = null;
                //connectection failed
            }//try-catch	   
            //connection ok!
            connection.Close();
            return hasil;
        }
        public SqlDataAdapter getAtt()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT TOP 100  ROW_NUMBER() OVER (ORDER BY CHANGE_DATE DESC) AS NO, TBL_LOGIN.[USER_NAME] ,[REASON] ,[REQUESTER]  ,[DEPT] ,[CHANGE_DATE] AS [ATTRACTIVE DATE] ,[IP_PC] AS [IP PC] " +
             "FROM [WPC].[dbo].[TBL_HISTORY]  inner join [WPC].dbo.TBL_LOGIN on TBL_LOGIN.[USER_ID] = [LOGIN_ID] " +
            "ORDER BY CHANGE_DATE DESC ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public string getTime()
        {
            DataTable hasil = new DataTable();
            string time = "";
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT TOP 1 [WPC_TIME],[WPC_MODEL]  FROM [WPC].[dbo].[WPC_TIME] WHERE WPC_MODEL = 'COMMON' ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            foreach(DataRow row in hasil.Rows)
            {
                time = row["WPC_TIME"].ToString();
            }
            return time;
        }

        public string UpTimeWPC(string _id,string _req,string _reason, string _dept, string _ip,string  _before,string  _after)
        {
            DataTable hasil = new DataTable();
            string time = "";
            SqlConnection connection = _Conn.Connect();
            try
            {
                string sql = string.Format("EXEC [dbo].[WPC_ATTRACTIVE] " + _after);
                connection.Open();
                command = new SqlCommand(sql, connection);
                SqlDataAdapter reader = new SqlDataAdapter(command);
                reader.Fill(hasil);
                if (hasil.Rows.Count>0)
                {

                     sql = string.Format("INSERT INTO [dbo].[TBL_HISTORY]  ([LOGIN_ID],[REASON],[REQUESTER],IP_PC,[DEPT],[CHANGE_DATE])"+
                                        " VALUES ('"+_id+"' ,'"+_reason+"','"+_req+"' ,'"+_ip+"','"+_dept +"',GETDATE())");
                    command = new SqlCommand(sql, connection);
                    int numberOfRecords = command.ExecuteNonQuery();
                }
                time = "OK";
            }
            catch { }
            connection.Close();
            
            return time;
        }

    }
}
