using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class CFilterAgent
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;

        public DataTable getTable(string defect, string status)
        {
            DataTable hasil = new DataTable();
            string stat = (status == "") ? "" : " AND [STATUS_FILTER]='"+status+"'";
             SqlConnection connection = _Conn.Connect();
             string sql = string.Format(@"SELECT [ERROR_CODE] AS [CODE]
                                        ,[ERROR_NAME] AS [NAME]
                                        ,[STATUS_FILTER] AS [STATUS]
                                        FROM [MAIN].[dbo].[TBL_FLT_DEFT_G]
                                        WHERE [DEL_USE] = 'N' AND [ERROR_NAME] like '%" + defect+"%'"+ stat);
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
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
            foreach (DataRow row in hasil.Rows)
            {
                time = row["WPC_TIME"].ToString();
            }
            return time;
        }

        public bool UpFilter(string _stat, string _defect)
        {
            DataTable hasil = new DataTable();
            bool time = false;
            SqlConnection connection = _Conn.Connect();
            connection.Open();
            try
            {


                string sql = string.Format(@"UPDATE [MAIN].[dbo].[TBL_FLT_DEFT_G] SET [STATUS_FILTER]='" + _stat + "' " +
                                            "WHERE [DEL_USE] = 'N' AND [ERROR_CODE]='"+_defect+"'");
                    command = new SqlCommand(sql, connection);
                    int numberOfRecords = command.ExecuteNonQuery();
                   time = (numberOfRecords>0)?true:false;
            }
            catch { }
            connection.Close();

            return time;
        }
        public string updateHW(string _ip)
        {
            string output = "ERROR, GI failed";
            DataTable hasil = new DataTable();

            try
            {

                SqlConnection connection = _Conn.Connect();
                string sql = string.Format("UPDATE [PROD].dbo.MONITORING_HW SET STATUS = 'O' WHERE IP_PC = '" + _ip + "'");
                connection.Open();
                command = new SqlCommand(sql, connection);
                SqlDataAdapter reader = new SqlDataAdapter(command);
                reader.Fill(hasil);
                int numberOfRecords = command.ExecuteNonQuery();
                connection.Close();
                if (numberOfRecords > 0)
                {
                    output = "OK";
                }
                else
                {
                    output = "GI Failed";
                }
            }
            catch { output = "ERROR, Not Connecting Server..!"; }

            return output;
        }


    }
}
