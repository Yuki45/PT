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

        public DataTable getTracking(string model, string status)
        {
            DataTable hasil = new DataTable();
            string models = (model == "" || model == "ALL") ? " WHERE [BASIC MODEL]=[BASIC MODEL] " : "WHERE [BASIC MODEL]= '" + model + "'";
            string statuss = (status == "") ? "" : " AND [UN]= '" + status + "'";
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format(@"SELECT  [LINE NAME]
                                        ,[PROCESS NAME]
                                        ,[BASIC MODEL]
                                        ,[IMEI NO]
                                        ,[UN]
                                        ,[DEFECT NAME]
                                        ,[HW VERSION]
                                        ,[RESULT]
                                        ,[PC NO]
                                        ,[PORT NO]
                                        ,[IP PC]
                                        ,[TACT TIME]
                                        ,[INSPECT TIME]
                                        FROM [PROD].[dbo].[SPC_MDL_RWK]" + models + statuss );
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public DataTable getTracking( string status)
        {
            DataTable hasil = new DataTable();
            string statuss = (status == "") ? "" : " Where [WG_SN]= '" + status + "'";
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format(@"SELECT [WG_SN] AS [IMEI NO]
                                        ,[WG_VALUE] AS [WEIGHT SCALE RESULT]
                                        ,[WG_DATE] AS [WEIGHT SCALE CHECK]
                                        FROM [PROD].[dbo].[WEIGHT_SCALE_HSTY]" + statuss);
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public DataTable getTable(string model, string item, string status, string start, string end )
        {
            DataTable hasil = new DataTable();
            string date = "AND UPLOAD_DATE between '"+start+"' and '"+end+"' ";
            string models = (model == "" || model == "ALL") ? " AND [BASIC MODEL]=[BASIC MODEL] " : " AND [BASIC MODEL]= '" + model + "'";
            string items = (item == "" || item == "ALL") ? " AND [TEST ITEM]= [TEST ITEM]" : " AND [TEST ITEM]= '" + item + "'";
            string statuss = (status == "" || status == "ALL") ? " [RESULT]= [RESULT] " : " [RESULT]= '" + status + "'";
                     SqlConnection connection = _Conn.Connect();
                     string sql = string.Format(@"SELECT UPLOAD_DATE AS [DATE],[BASIC MODEL]
                                                    ,[UNIQUE NO]
                                                    ,[TEST ITEM]
                                                    ,[CHANNEL]
                                                    ,ROUND([VSL],2) AS VALUE
                                                    ,ROUND([AVG],2) AS [AVG]
                                                    ,ABS(ROUND([VERIFICATION],2)) AS [GAP]
                                                    ,[SPEC ITEM]
                                                    ,[RESULT]
                                                    FROM [PROD].[dbo].[LOSS_CAL_VIEW_NEW]  WHERE " + statuss +date + models + items ) ;
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
            }
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

        public string LossCal(string _model, string _un, string _item, string _vsl)
        {
            DataTable hasil = new DataTable();
            string time = "";
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [PROD].[dbo].[ADD_LOSS_CAL_NEW]  '" + _model + "','" + _un + "','" + _item + "'," + _vsl);
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            foreach (DataRow row in hasil.Rows)
            {
                time = row["RESULT"].ToString();
            }
            return time;
        }

        public int AddLossCal(string _model, string _un, string _item, string _vsl)
        {
            DataTable hasil = new DataTable();
            string time = "";
            int numberOfRecords = 0;
            SqlConnection connection = _Conn.Connect();
            try
            {
                connection.Open();

                 string   sql = string.Format(@"INSERT INTO [PROD].[dbo].[VER_LOSS_CAL]
                                            ([BASIC_MODEL]
                                            ,[UNIQUE_NO]
                                            ,[TEST_ITEM]
                                            ,[VSL])
                                            VALUES" +
                                       "  ('" + _model + "' ,'" + _un + "','" + _item + "' ," + _vsl + ")");
                    command = new SqlCommand(sql, connection);
                    numberOfRecords = command.ExecuteNonQuery();
                
               
            }
            catch { }
            connection.Close();

            return numberOfRecords;
        }

    }
}
