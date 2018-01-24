using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class Losscal
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;
        
        public DataTable getTable()
        {
            DataTable hasil = new DataTable();
                     SqlConnection connection = _Conn.Connect();
            string sql = string.Format(@"SELECT TOP 1000 [BASIC MODEL]
                                        ,[UNIQUE NO]
                                        ,[TEST ITEM]
                                        ,ROUND([VSL],2) AS [VSL]
                                        ,ROUND([AVG],2) AS [AVG]
                                        ,ABS(ROUND([VERIFICATION],2))AS VERIFICATION
                                        ,SPEC_ITEM
                                        ,(CASE
                                        WHEN ABS(ROUND([VERIFICATION],2)) > SPEC_ITEM 
                                        THEN 'NG' 
                                        ELSE 'OK' END) AS [RESULT]
                                        FROM [PROD].[dbo].[LOSS_CAL_VIEW]
                                        INNER JOIN [PROD].dbo.TBL_MAPPING_LOSS_CAL on TBL_MAPPING_LOSS_CAL.TEST_ITEM = [LOSS_CAL_VIEW].[TEST ITEM] ");
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

        public string UpTimeWPC(string _id, string _req, string _reason, string _dept, string _ip, string _before, string _after)
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
                if (hasil.Rows.Count > 0)
                {

                    sql = string.Format("INSERT INTO [dbo].[TBL_HISTORY]  ([LOGIN_ID],[REASON],[REQUESTER],IP_PC,[DEPT],[CHANGE_DATE])" +
                                       " VALUES ('" + _id + "' ,'" + _reason + "','" + _req + "' ,'" + _ip + "','" + _dept + "',GETDATE())");
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
