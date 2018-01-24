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
            string sql = string.Format("SELECT CAUSE_ID AS ID, CAUSE_DESC AS DESCRIPTION FROM " +
            "[REPAIR].[dbo].[TBL_MDM_CAUSE] " +
            "ORDER BY CAUSE_ID");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getMdmCauseID(string _id)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT CAUSE_ID AS ID, CAUSE_DESC AS DESCRIPTION FROM " +
            "[REPAIR].[dbo].[TBL_MDM_CAUSE] " +
            "WHERE CAUSE_ID LIKE '%" + _id + "%' OR CAUSE_DESC LIKE '%" + _id + "%' " +
            "ORDER BY CAUSE_ID");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getMdmDefect()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT [SYMPTOM_CODE] AS ID,[SYMPTOM_NAME] AS NAME,[SYMPTOM_SOURCE] AS SOURCE,"+
            "[SYMPTOM_BOOTING] AS BOOTING,[SYMPTOM_REMARK] AS REMARK, [SYMPTOM_SEQ] AS SEQ FROM " +
            "[REPAIR].[dbo].[TBL_MASTER_SYMPTOM] " +
            "ORDER BY SYMPTOM_SOURCE DESC,SYMPTOM_REMARK DESC,SYMPTOM_SEQ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getMdmDefectID(string _id)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT [SYMPTOM_CODE] AS ID,[SYMPTOM_NAME] AS NAME,[SYMPTOM_SOURCE] AS SOURCE,"+
            "[SYMPTOM_BOOTING] AS BOOTING,[SYMPTOM_REMARK] AS REMARK, [SYMPTOM_SEQ] AS SEQ FROM " +
            "[REPAIR].[dbo].[TBL_MASTER_SYMPTOM] " +
            "WHERE SYMPTOM_CODE LIKE '%" + _id + "%' OR SYMPTOM_NAME LIKE '%" + _id + "%' " +
            "ORDER BY SYMPTOM_SOURCE DESC,SYMPTOM_REMARK DESC,SYMPTOM_SEQ");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public string saveCause(string _desc)
        {
            string output = "Save data failed";
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("INSERT INTO [REPAIR].[dbo].[TBL_MDM_CAUSE] (CAUSE_DESC) VALUES('"+_desc+"')");
            connection.Open();
            command = new SqlCommand(sql, connection);
            int numberOfRecords = command.ExecuteNonQuery();
            connection.Close();
            if (numberOfRecords > 0)
            {
                output = "SAVE";
            }

            return output;
        }

        public string updateCause(string _id, string _desc)
        {
            string output = "Update data failed";
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("UPDATE [REPAIR].[dbo].[TBL_MDM_CAUSE] SET CAUSE_DESC='" + _desc + "' WHERE CAUSE_ID='" + _id + "'");
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
            string sql = string.Format("DELETE FROM [REPAIR].[dbo].[TBL_MDM_CAUSE] WHERE CAUSE_ID = '" + _id + "'");
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
