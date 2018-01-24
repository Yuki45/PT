using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class Execution
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;

        public SqlDataAdapter getTableGI()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, " +
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE'," +
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE'," +
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV]," +
            "[R_CAUSE] AS 'CAUSE',[R_LOCATION] AS 'LOCATION',[R_REMARK] AS 'REMARK', [R_ID] " +
            "FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE [R_STATUS]='REPAIRED' AND ISNULL(R_STS_GI,'')=''");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getTableGICN(string _cn)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, " +
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE'," +
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE'," +
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV]," +
            "[R_CAUSE] AS 'CAUSE',[R_LOCATION] AS 'LOCATION',[R_REMARK] AS 'REMARK', [R_ID]  " +
            "FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE [R_STATUS]='REPAIRED' AND ISNULL(R_STS_GI,'')='' AND [R_CN] LIKE '%" + _cn + "%'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getTableAbolish()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, " +
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE'," +
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE'," +
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV]," +
            "[R_CAUSE] AS 'CAUSE',[R_LOCATION] AS 'LOCATION',[R_REMARK] AS 'REMARK', [R_ID] " +
            "FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE ([R_STATUS]='ABOLISH' OR [R_STATUS]='RMA') AND ISNULL(R_STS_GI,'')=''");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getTableAbolishCN(string _cn)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, " +
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE'," +
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE'," +
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV]," +
            "[R_CAUSE] AS 'CAUSE',[R_LOCATION] AS 'LOCATION',[R_REMARK] AS 'REMARK', [R_ID]  " +
            "FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE ([R_STATUS]='ABOLISH' OR [R_STATUS]='RMA') AND ISNULL(R_STS_GI,'')='' AND [R_CN] LIKE '%" + _cn + "%'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public string GetCountRepeatNV(string _un, string _model)
        {
            string output = "0";
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [REPAIR].dbo.[getCountRepeat] '" + _un + "','" + _model + "'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            foreach (DataRow row in hasil.Rows)
            {
                output = row[0].ToString();
            }
            return output;
        }

        public string[] GetDefectNV(string _defect_code)
        {
            string[] output = new string[2];
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [REPAIR].dbo.[getSymptomName] '" + _defect_code + "'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            foreach (DataRow row in hasil.Rows)
            {
                output[0] = row[0].ToString();
                output[1] = row[1].ToString();
            }
            return output;
        }

        public DataTable getDtRowNV(string nv)
        {
            DataTable hasil = new DataTable();
            string[] arrCn = nv.Split('|');
            string _cn = arrCn[1];
            string _un = arrCn[2];
            string _line = arrCn[3];
            string _gen = arrCn[4];
            string _cause = arrCn[5];
            string _value = arrCn[6];
            string _standar_l = arrCn[7];
            string _standar_u = arrCn[8];

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("exec [dbo].[getDtRowNV] '" + _cn + "','" + _un + "','" + _line + "','" + _gen + "','" + _cause + "','" + _value + "','" + _standar_l + "','" + _standar_u + "'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();

            return hasil;
        }

        public string updateGI(string _nv, string _user, string _id)
        {
            string output = "ERROR, GI failed";
            DataTable hasil = new DataTable();
            var count = _nv.Count(x => x == '|');
            if (count != 12)
            {
                output = "FORMAT NV TIDAK SESUAI";
            }
            else
            {
                string[] arrCn = _nv.Split('|');
                string _cn = arrCn[1];
                string _un = arrCn[2];
                string _process = arrCn[3];
                string _symptom = arrCn[4];
                string _testitem = arrCn[5];
                string _value = arrCn[6];
                string _standar_l = arrCn[7];
                string _standar_u = arrCn[8];

                SqlConnection connection = _Conn.Connect();
                string sql = string.Format("UPDATE [REPAIR].[dbo].[TBL_REPAIR_STATUS] SET R_STS_GI = 'GI', R_STS_GI_DATE = GETDATE() WHERE R_ID = '" + _id + "'");
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

            return output;
        }

        public string updateAbolish(string _nv, string _gen, string _id)
        {
            string output = "ERROR, GI failed";
            DataTable hasil = new DataTable();
            var count = _nv.Count(x => x == '|');
            if (count != 12)
            {
                output = "FORMAT NV TIDAK SESUAI";
            }
            else
            {
                string[] arrCn = _nv.Split('|');
                string _cn = arrCn[1];
                string _un = arrCn[2];
                string _process = arrCn[3];
                string _symptom = arrCn[4];
                string _testitem = arrCn[5];
                string _value = arrCn[6];
                string _standar_l = arrCn[7];
                string _standar_u = arrCn[8];

                SqlConnection connection = _Conn.Connect();
                string sql = string.Format("UPDATE [REPAIR].[dbo].[TBL_REPAIR_STATUS] SET R_STS_GI = 'GI', R_STS_GI_DATE = GETDATE() WHERE R_ID = '" + _id + "'");
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

            return output;
        }
    }
}
