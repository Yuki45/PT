using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class Exceptional
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;

        public SqlDataAdapter getStockWaiting()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, "+
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE',"+
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE',"+
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV], [R_ID]" +
            " FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]"+
            "WHERE R_STATUS IS NULL AND R_STS_GI_TO_ENG IS NULL AND R_EXCEPTION = 'Y'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getStockWaitingCN(string _cn)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, " +
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE'," +
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE'," +
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV], [R_ID]" +
            " FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE R_STATUS IS NULL AND R_STS_GI_TO_ENG IS NULL AND R_EXCEPTION = 'Y' AND [R_CN] LIKE '%" + _cn + "%'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getExceptionalComp()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, " +
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE'," +
            "[R_NV_TEST_ITEM] AS 'ITEM',[R_NV_VALUE] AS 'VALUE',[R_NV_STANDAR_L] AS 'NVL',[R_NV_STANDAR_U] AS 'NVU'," +
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE'," +
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV], [R_ID]" +
            " FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE R_STATUS IS NULL AND R_STS_GI_TO_ENG IS NOT NULL AND R_EXCEPTION = 'Y'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getExceptionalCompCN(string _cn)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, " +
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE'," +
            "[R_NV_TEST_ITEM] AS 'ITEM',[R_NV_VALUE] AS 'VALUE',[R_NV_STANDAR_L] AS 'NVL',[R_NV_STANDAR_U] AS 'NVU'," +
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE'," +
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV], [R_ID]" +
            " FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE R_STATUS IS NULL AND R_STS_GI_TO_ENG IS NOT NULL AND R_EXCEPTION = 'Y' AND [R_CN] LIKE '%" + _cn + "%'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public string[] GetDefectNV(string _defect_code)
        {
            string[] output = new string [2];
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

        public string AddExceptionalNV(string _part, string _cn, string _un, string _symptom, string _defectname, string _process, string _model, string _line, string _item, string _value, string _nvl, string _nvu, string _name)
        {
            string output = "";
            DataTable hasil = new DataTable();

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [REPAIR].dbo.[input_exceptional_nv] '" + _part + "','" + _cn + "','" + _un + "', '" + _symptom + "', '" + _defectname + "', '" + _process + "', '" + _model + "', '" + _line + "', '" + _item + "', '" + _value + "', '" + _nvl + "', '" + _nvu + "', '" + _name + "'");
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

        public string UpdateExceptionalNV(string _part, string _cn, string _un, string _code, string defectName, string _process, string _model, string _line, string _item, string _value, string _nvl, string _nvu)
        {
            string output = "";
            DataTable hasil = new DataTable();

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [REPAIR].dbo.[update_exceptional_nv] '" + _part + "','" + _cn + "','" + _un + "', '" + _code + "', '" + defectName + "', '" + _process + "', '" + _model + "', '" + _line + "', '" + _item + "', '" + _value + "', '" + _nvl + "', '" + _nvu + "'");
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

        public bool GetNVStatusEngineer(string nv)
        {
            bool output = false;
            DataTable hasil = new DataTable();
            var count = nv.Count(x => x == '|');

            if (count != 12)
            {
                output = false;
            }
            else
            {
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
                string sql = string.Format("EXEC [REPAIR].dbo.[getStatusExecRepaired_nv]  '" + _cn + "','" + _un + "','" + _line + "','" + _gen + "','" + _cause + "','" + _value + "','" + _standar_l + "','" + _standar_u + "'");
                connection.Open();
                command = new SqlCommand(sql, connection);
                SqlDataAdapter reader = new SqlDataAdapter(command);
                reader.Fill(hasil);
                connection.Close();
                foreach (DataRow row in hasil.Rows)
                {
                    if (row[0].ToString() == "OK")
                    {
                        output = true;
                    }
                }
            }

            return output;
        }

        public DataTable getuserSearch(string name)
        {
            DataTable hasil = new DataTable();

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT [TBL_LOGIN].[USER_GEN],[TBL_PROFILE].[P_NAME] AS NAME " +
                                "FROM [REPAIR].[dbo].[TBL_LOGIN]" +
                                "INNER JOIN [REPAIR].[dbo].[TBL_PROFILE] ON " +
                                "[REPAIR].[dbo].[TBL_LOGIN].[USER_ID] = [REPAIR].[dbo].[TBL_PROFILE].[USER_ID] WHERE [REPAIR].[dbo].[TBL_PROFILE].[P_NAME] like '%" + name + "%' OR [REPAIR].[dbo].[TBL_LOGIN].[USER_GEN] like '%" + name + "%'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();

            return hasil;
        }

        public string updateGiEngineeringEx(string _gen, string _id)
        {
            string output = "ERROR, GI data failed";
            DataTable hasil = new DataTable();            

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [REPAIR].dbo.[update_gi_engineering_cn] '" + _id + "', '" + _gen + "'");
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

        public List<string> GetAllModel()
        {
            List<string> output = new List<string>();

            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [REPAIR].dbo.[getAllModel]");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            output = hasil.AsEnumerable().Select(x => x[0].ToString()).ToList();

            return output;
        }

        public List<string> GetAllLine()
        {
            List<string> output = new List<string>();

            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [REPAIR].dbo.[getAllLine]");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            output = hasil.AsEnumerable().Select(x => x[0].ToString()).ToList();

            return output;
        }

        public List<string> GetStdDefect()
        {
            List<string> output = new List<string>();

            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [REPAIR].dbo.[getStdDefect]");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            output = hasil.AsEnumerable().Select(x => x[0].ToString()).ToList();

            return output;
        }

        public string GetStdDefectCode(string _tmpDefect)
        {
            string output = "";
            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [REPAIR].dbo.[getStdDefectCode] '" + _tmpDefect + "'");
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
    }
}
