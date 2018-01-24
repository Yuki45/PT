using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class CMdmEpass
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;

        public SqlDataAdapter getMdmEpass()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ITEM_MODEL AS [MODEL], ITEM_MATERIAL AS[PART NO], ITEM_DESC AS [DESC],ITEM_REG_DATE AS [REGISTER DATE], ITEM_REG_USER AS [USER]  FROM [PROD].[dbo].[TBL_MASTER_EPASS] ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getEpassID(string _id)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ITEM_MODEL AS [MODEL], ITEM_MATERIAL AS[PART NO], ITEM_DESC AS [DESC],ITEM_REG_DATE AS [REGISTER DATE], ITEM_REG_USER AS [USER]  FROM [PROD].[dbo].[TBL_MASTER_EPASS]  " +
            "WHERE ITEM_MODEL LIKE '%" + _id + "%'  " +
            "ORDER BY ITEM_MODEL");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public string saveCause(string _model, string _desc, string key, string _user)
        {
            string output = "Save data failed, Model Already Exist";
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT * FROM [PROD].[dbo].[TBL_MASTER_EPASS] WHERE ITEM_MODEL='" + _model + "' AND ITEM_MATERIAL='"+key+"'" + " ORDER BY ITEM_MODEL");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            if (hasil.Rows.Count <= 0)
            {
                sql = string.Format("INSERT INTO [PROD].[dbo].[TBL_MASTER_EPASS] (ITEM_MODEL,ITEM_MATERIAL,ITEM_DESC, ITEM_REG_USER) VALUES('"+ _model + "','" + key + "','"  + _desc + "','" + _user + "')");
                //connection.Open();
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

        public string updateCause(string _model, string _desc, string key, string _user)
        {
            string output = "Update data failed";
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT * FROM [PROD].[dbo].[TBL_MASTER_EPASS] WHERE ITEM_MODEL='" + _model + "' AND ITEM_MATERIAL='"+key+"'" + " ORDER BY ITEM_MODEL");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            if (hasil.Rows.Count <= 0)
            {
                 sql = string.Format("UPDATE [PROD].[dbo].[TBL_MASTER_EPASS] SET ITEM_MATERIAL='" + key + "', ITEM_REG_DATE=GETDATE(), ITEM_REG_USER='" + _user + "'  WHERE ITEM_MODEL='" + _model + "' AND ITEM_DESC='" + _desc + "'");
                //connection.Open();
                command = new SqlCommand(sql, connection);
                int numberOfRecords = command.ExecuteNonQuery();
                connection.Close();
                if (numberOfRecords > 0)
                {
                    output = "UPDATE";
                }
            }

            return output;
        }

        public string deleteCause(string _model, string _desc)
        {
            string output = "Delete data Failed";
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("DELETE FROM [PROD].[dbo].[TBL_MASTER_EPASS] WHERE ITEM_MODEL='" + _model + "' AND ITEM_DESC='" + _desc + "'");
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
