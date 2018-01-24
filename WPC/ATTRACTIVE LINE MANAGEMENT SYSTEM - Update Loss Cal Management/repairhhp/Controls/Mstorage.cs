using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class Mstorage
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;
        
        public DataTable getTable(string cline,string start, string end, string timestart, string timeend)
        {
            DataTable hasil = new DataTable();
            string shiftc = "";
            string line = (cline == "" || cline == "ALL") ? "" : "And h.[LINE NAME]= '" + cline + "' ";
            string datec = " And d.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";
            string date = " Where h.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";
            
            shiftc = datec; 

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("exec [PROD].[dbo].[SPDisplay_STORAGE_NEW]'" + start + " " + timestart + ":00:00.000','" + end + " " + timeend + ":00:00.000'; ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public DataTable getHistory(string cline, string start, string end, string timestart, string timeend)
        {
            DataTable hasil = new DataTable();
            string shiftc = "";
            string line = (cline == "" || cline == "ALL") ? "" : "And h.[LINE NAME]= '" + cline + "' ";
            string datec = " And d.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";
            string date = " Where h.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";

            shiftc = datec;

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("exec [PROD].[dbo].[SPDisplay_STORAGE_HISTORY]'" + start + " " + timestart + ":00:00.000','" + end + " " + timeend + ":00:00.000'; ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public DataTable getHW(string cline)
        {
            DataTable hasil = new DataTable();
            string line = (cline == "" || cline == "ALL") ? "" : "WHERE [LINE NAME]= '" + cline + "' ";

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format(@"SELECT TOP 1000 [LINE NAME]
                                    ,[BASIC MODEL]
                                    ,[PC NO]
                                    ,[HW VERSION]
                                    ,MODEL_INFO.HW AS [HW SPEC]
                                    , CASE When([HW VERSION] = MODEL_INFO.HW)
                                    THEN 'OK'
                                    ELSE 'NG'
                                    END AS RESULT
                                    FROM [PROD].[dbo].[HW_MONITORING]
                                    INNER JOIN [PROD].dbo.MODEL_INFO  on MODEL_INFO.BASIC_MODEL = [HW_MONITORING].[BASIC MODEL] " +  line + "  ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public DataTable getFinal(string proses,string start, string end, string timestart, string timeend)
        {
            DataTable hasil = new DataTable();
            string shiftc = "";
            //string line = (cline == "" || cline == "ALL") ? "" : "And h.[LINE NAME]= '" + cline + "' ";
           // string datec = " And d.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";
           // string date = " Where h.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";

          //  shiftc = datec;

            string tProcess = (proses=="TOTAL")?"":" AND [TYPE_NAME]= '" + proses + "' ";
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format(@"SELECT [SN],[TYPE_NAME],[SCAN_TIME] FROM [PROD].[dbo].[UNIT_STORAGE_STATUS]  WHERE  [SCAN_TIME] Between '"+ start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' "+tProcess);
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public DataTable getFinalH(string proses, string start, string end, string timestart, string timeend)
        {
            DataTable hasil = new DataTable();
            string shiftc = "";
            //string line = (cline == "" || cline == "ALL") ? "" : "And h.[LINE NAME]= '" + cline + "' ";
            // string datec = " And d.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";
            // string date = " Where h.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";

            //  shiftc = datec;

            string tProcess = (proses == "TOTAL") ? "" : " AND [TYPE_NAME]= '" + proses + "' ";
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format(@"SELECT [SN],[TYPE_NAME],[SCAN_TIME] FROM [PROD].[dbo].[UNIT_STORAGE_STATUS_H]  WHERE  [SCAN_TIME] Between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' " + tProcess);
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public DataTable getLcia(string cline, string start, string end, string timestart, string timeend)
        {
            DataTable hasil = new DataTable();
            string shiftc = "";
            string line = (cline == "" || cline == "ALL") ? "" : "And h.[LINE NAME]= '" + cline + "' ";
            string datec = " And d.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";
            string date = " Where h.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";

            shiftc = datec;

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT h.[LINE NAME] ,COUNT([UN]) AS [TOTAL QTY], " +
                         "(SELECT COUNT(*) FROM [PROD].[dbo].[SPC_LCIA] AS d WHERE d.[LINE NAME]=h.[LINE NAME] AND [RESULT]='PASS' " + shiftc + line + " ) AS [OK QTY], " +
                         "(COUNT([UN])-(SELECT COUNT(*) FROM [PROD].[dbo].[SPC_LCIA] AS d WHERE d.[LINE NAME]=h.[LINE NAME] AND [RESULT]='PASS' " + shiftc + line + " )) AS [DEFECT QTY], " +
                         "CAST((((CAST(COUNT([UN]) AS FLOAT)-CAST((SELECT COUNT(*) FROM [PROD].[dbo].[SPC_LCIA] AS d WHERE d.[LINE NAME]=h.[LINE NAME] AND [RESULT]='PASS' " + shiftc + line + " ) AS FLOAT))/COUNT([UN])*CAST(100 AS FLOAT))) AS DECIMAL(10,2)) AS [RATE(%)] " +
                          "FROM [PROD].[dbo].[SPC_LCIA] AS h  " + date + line + "  GROUP BY h.[LINE NAME]  ORDER BY  h.[LINE NAME] ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public DataTable getErrorMachine(string sn)
        {
            DataTable hasil = new DataTable();
            

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format(@" SELECT TOP 1000 [SN],[TYPE_NAME],[SCAN_TIME] FROM [PROD].[dbo].[UNIT_STORAGE_STATUS_H]  WHERE SN='"+sn+"' ");
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
