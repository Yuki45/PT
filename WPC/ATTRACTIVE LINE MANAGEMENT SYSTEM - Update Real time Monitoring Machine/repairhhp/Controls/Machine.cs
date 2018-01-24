using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class Machine
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
            string sql = string.Format("SELECT h.[LINE NAME] ,COUNT([UN]) AS [TOTAL QTY], " +
                         "(SELECT COUNT(*) FROM [PROD].[dbo].[SPC_AGENT] AS d WHERE d.[LINE NAME]=h.[LINE NAME] AND [RESULT]='PASS' " + shiftc + line + " ) AS [OK QTY], " +
                         "(COUNT([UN])-(SELECT COUNT(*) FROM [PROD].[dbo].[SPC_AGENT] AS d WHERE d.[LINE NAME]=h.[LINE NAME] AND [RESULT]='PASS' " + shiftc + line + " )) AS [DEFECT QTY], " +
                         "CAST((((CAST(COUNT([UN]) AS FLOAT)-CAST((SELECT COUNT(*) FROM [PROD].[dbo].[SPC_AGENT] AS d WHERE d.[LINE NAME]=h.[LINE NAME] AND [RESULT]='PASS' " + shiftc + line + " ) AS FLOAT))/COUNT([UN])*CAST(100 AS FLOAT))) AS DECIMAL(10,2)) AS [RATE(%)] " +
                          "FROM [PROD].[dbo].[SPC_AGENT] AS h  " + date + line + "  GROUP BY h.[LINE NAME]  ORDER BY  h.[LINE NAME] ");
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
            string sql = string.Format("SELECT [LINE NAME] ,[BASIC MODEL],[PC NO],[HW VERSION]  FROM [PROD].[dbo].[HW_MONITORING]  " +  line + "  ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public DataTable getFinal(string cline, string start, string end, string timestart, string timeend)
        {
            DataTable hasil = new DataTable();
            string shiftc = "";
            string line = (cline == "" || cline == "ALL") ? "" : "And h.[LINE NAME]= '" + cline + "' ";
            string datec = " And d.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";
            string date = " Where h.[INSPECT TIME] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' ";

            shiftc = datec;

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT h.[LINE NAME] ,COUNT([UN]) AS [TOTAL QTY], " +
                         "(SELECT COUNT(*) FROM [PROD].[dbo].[SPC_FINAL] AS d WHERE d.[LINE NAME]=h.[LINE NAME] AND [RESULT]='PASS' " + shiftc + line + " ) AS [OK QTY], " +
                         "(COUNT([UN])-(SELECT COUNT(*) FROM [PROD].[dbo].[SPC_FINAL] AS d WHERE d.[LINE NAME]=h.[LINE NAME] AND [RESULT]='PASS' " + shiftc + line + " )) AS [DEFECT QTY], " +
                         "CAST((((CAST(COUNT([UN]) AS FLOAT)-CAST((SELECT COUNT(*) FROM [PROD].[dbo].[SPC_FINAL] AS d WHERE d.[LINE NAME]=h.[LINE NAME] AND [RESULT]='PASS' " + shiftc + line + " ) AS FLOAT))/COUNT([UN])*CAST(100 AS FLOAT))) AS DECIMAL(10,2)) AS [RATE(%)] " +
                          "FROM [PROD].[dbo].[SPC_FINAL] AS h  " + date + line + "  GROUP BY h.[LINE NAME]  ORDER BY  h.[LINE NAME] ");
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

        public DataTable getErrorMachine(string cline, string start, string end, string timestart, string timeend)
        {
            DataTable hasil = new DataTable();
            

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT TOP 1000 [LINE NAME],COUNT([QTY]) AS QTY "+
                                         " FROM [DB_LOST_HUNTER].[dbo].[MESIN_ERROR] "+
                                          "Where [ERROR DATE] between '" + start + " " + timestart + ":00:00.000' And '" + end + " " + timeend + ":59:59.000' "+
                                         " GROUP BY [LINE NAME]  ");
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
