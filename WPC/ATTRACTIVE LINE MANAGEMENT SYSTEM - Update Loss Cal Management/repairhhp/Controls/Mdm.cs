using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class Mdm
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;

        public SqlDataAdapter getMdmCause()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT BASIC_MODEL AS MODEL, HW AS VERSION FROM " +
            "[PROD].[dbo].[MODEL_INFO] " +
            "ORDER BY BASIC_MODEL");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getMdmCauseID(string _id)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT BASIC_MODEL AS MODEL, HW AS VERSION FROM " +
            "[PROD].[dbo].[MODEL_INFO] " +
            "WHERE BASIC_MODEL LIKE '%" + _id + "%'  " +
            "ORDER BY BASIC_MODEL");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getMdmDefect()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format(@"SELECT  [TEST_ITEM] AS [TEST ITEM]
                                        ,[CHANNEL_ITEM]  AS [CHANNEL]
                                        ,[CATEGORY_ITEM]  AS [CATEGORY]
                                        ,[RANGE_ITEM] AS [RANGE ITEM]
                                        ,[SPEC_ITEM] AS [SPEC ITEM]
                                        ,[DESC_ITEM] AS [DESC ITEM]
                                        FROM [PROD].[dbo].[TBL_MAPPING_LOSS_CAL]");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getMdmDefectID(string _id)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format(@"SELECT  [TEST_ITEM] AS [TEST ITEM]
                                        ,[CHANNEL_ITEM]  AS [CHANNEL]
                                        ,[CATEGORY_ITEM]  AS [CATEGORY]
                                        ,[RANGE_ITEM] AS [RANGE ITEM]
                                        ,[SPEC_ITEM] AS [SPEC ITEM]
                                        ,[DESC_ITEM] AS [DESC ITEM]
                                        FROM [PROD].[dbo].[TBL_MAPPING_LOSS_CAL] " +
            "WHERE TEST_ITEM LIKE '%" + _id + "%' AND SYMPTOM_NAME LIKE '%" + _id + "%' " +
            "ORDER BY SYMPTOM_SOURCE DESC,SYMPTOM_REMARK DESC,SYMPTOM_SEQ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public string saveCause(string _model, string _desc)
        {
            string output = "Save data failed, Model Already Exist";
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT BASIC_MODEL AS MODEL, HW AS VERSION FROM " +
            "[PROD].[dbo].[MODEL_INFO] WHERE BASIC_MODEL='"+_model+"' " +
            "ORDER BY BASIC_MODEL");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            if (hasil.Rows.Count <= 0)
            {
                sql = string.Format("INSERT INTO [PROD].[dbo].[MODEL_INFO] (BASIC_MODEL,CAUSE_DESC) VALUES('" + _model + "','" + _desc + "')");
                connection.Open();
                command = new SqlCommand(sql, connection);
                int numberOfRecords = command.ExecuteNonQuery();
                connection.Close();
                if (numberOfRecords > 0)
                {
                    output = "SAVE";
                }
            }

            return output;
        }

        public string updateCause(string _id, string _desc)
        {
            string output = "Update data failed";
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("UPDATE [PROD].[dbo].[MODEL_INFO] SET HW='" + _desc + "' WHERE BASIC_MODEL='" + _id + "'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            int numberOfRecords = command.ExecuteNonQuery();
            connection.Close();
            if (numberOfRecords > 0)
            {
                output = "UPDATE";
            }

            return output;
        }

        public string deleteCause(string _id)
        {
            string output = "Delete data Failed";
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("DELETE FROM [PROD].[dbo].[MODEL_INFO] WHERE BASIC_MODEL = '" + _id + "'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            int numberOfRecords = command.ExecuteNonQuery();
            connection.Close();
            if (numberOfRecords > 0)
            {
                output = "OK";
            }

            return output;
        }

    }
}
