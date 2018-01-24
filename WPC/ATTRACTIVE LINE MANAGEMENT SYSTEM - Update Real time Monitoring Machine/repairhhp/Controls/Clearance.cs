using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class Clearance
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;

        public SqlDataAdapter getRepeat()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY QTY DESC) AS NO, * FROM( " +
            "SELECT [R_MODEL] AS MODEL,[R_NV_UN] AS UN, [R_NV_PART] AS PART, COUNT([R_NV_UN]) AS QTY " +
            "FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS] " +
            "WHERE ISNULL(ADJUST_STATUS,'') = '' AND R_NV_PART = 'PBA' " +
            "GROUP BY R_NV_UN, R_MODEL, R_NV_PART " +
            ") X WHERE QTY>3 AND ISNULL(UN,'')!='' " +
            "ORDER BY QTY DESC");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getRepeatUN(string _un)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY QTY DESC) AS NO, * FROM( " +
            "SELECT [R_MODEL] AS MODEL,[R_NV_UN] AS UN, [R_NV_PART] AS PART, COUNT([R_NV_UN]) AS QTY " +
            "FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS] " +
            "WHERE ISNULL(ADJUST_STATUS,'') = '' AND R_NV_PART = 'PBA' " +
            "GROUP BY R_NV_UN, R_MODEL, R_NV_PART " +
            ") X WHERE QTY>3 AND ISNULL(UN,'')!='' AND UN LIKE '%" + _un + "%' " +
            "ORDER BY QTY DESC");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }


        public string clearanceUN(string _un)
        {
            string output = "Clearance Failed";
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [REPAIR].[dbo].[clearance_adjustment] '" + _un + "'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            foreach (DataRow row in hasil.Rows)
            {
                if (row[0].ToString() == "OK")
                {
                    output = "OK";
                }
                else
                {
                    output = row[0].ToString();
                }
            }

            return output;
        }

    }
}
