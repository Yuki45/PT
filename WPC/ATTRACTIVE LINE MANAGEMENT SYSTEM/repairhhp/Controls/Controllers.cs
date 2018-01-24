using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class Controllers
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
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV], [R_ID], [R_NV_PART] AS PART" +
            " FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]"+
            "WHERE R_STATUS IS NULL AND R_STS_GI_TO_ENG IS NULL");
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
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV], [R_ID], [R_NV_PART] AS PART" +
            " FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE R_STATUS IS NULL AND R_STS_GI_TO_ENG IS NULL AND [R_CN] LIKE '%"+_cn+"%'");
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

        public string AddFieldNV(string nvpart, string model, string line, string defect, string cn, string user, string partnumber, string countUn)
        {
            string output = "";
            DataTable hasil = new DataTable();
            var count = cn.Count(x => x == '|');
            if (count != 12)
            {
                output = "FORMAT NV TIDAK SESUAI";
            }
            else
            {
                string[] arrCn = cn.Split('|');
                string _cn = arrCn[1];
                string _un = arrCn[2];
                string _line = arrCn[3];
                string _gen = arrCn[4];
                string _cause = arrCn[5];
                string _value = arrCn[6];
                string _standar_l = arrCn[7];
                string _standar_u = arrCn[8];

                SqlConnection connection = _Conn.Connect();
                string sql = string.Format("EXEC [REPAIR].dbo.[input_repair_nv] '" + model + "','" + line + "','" + defect + "', '" + user + "', '" + _cn + "', '" + _un + "', '" + _line + "', '" + _gen + "', '" + _cause + "', '" + _value + "', '" + _standar_l + "', '" + _standar_u + "', '" + nvpart + "', '" + partnumber + "', '" + countUn + "'");
                connection.Open();
                command = new SqlCommand(sql, connection);
                SqlDataAdapter reader = new SqlDataAdapter(command);
                reader.Fill(hasil);
                connection.Close();
                foreach (DataRow row in hasil.Rows)
                {
                    output = row[0].ToString();
                }
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
                string sql = string.Format("EXEC [REPAIR].dbo.[getStatusNVEngineer_nv]  '" + _cn + "','" + _un + "','" + _line + "','" + _gen + "','" + _cause + "','" + _value + "','" + _standar_l + "','" + _standar_u + "'");
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
                                "FROM [REPAIR].[dbo].[TBL_LOGIN] " +
                                "INNER JOIN [REPAIR].[dbo].[TBL_PROFILE] ON " +
                                "[REPAIR].[dbo].[TBL_LOGIN].[USER_ID] = [REPAIR].[dbo].[TBL_PROFILE].[USER_ID] WHERE [REPAIR].[dbo].[TBL_PROFILE].[P_NAME] like '%" + name + "%' OR [REPAIR].[dbo].[TBL_LOGIN].[USER_GEN] like '%" + name + "%'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();

            return hasil;
        }

        public DataTable getHistory(string _un, string _model)
        {
            DataTable hasil = new DataTable();

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY REPEAT_COUNT) AS NO, [R_ID] AS ID " +
                                ",[R_MODEL] AS MODEL, [R_LINE] AS LINE,[R_CN] AS CN, [R_NV_UN] AS UN, [R_INPUT_DATE] AS 'GR DATE' " +
                                ",[R_DEFECT_NAME] AS DEFECT,[R_CAUSE] AS CAUSE,[R_LOCATION] AS LOCATION,[R_REMARK] AS REMARK " +
                                ",[R_RELEASED_USER] AS 'RELEASE GEN', [P_NAME] AS 'RELEASE NAME',[R_RELEASED_DATE] AS 'RELEASE DATE' " +
                                "FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS] A " +
                                "LEFT JOIN ( " +
                                "SELECT A.USER_GEN, B.[P_NAME] " +
                                "FROM [REPAIR].[dbo].[TBL_LOGIN] A " +
                                "INNER JOIN [REPAIR].[dbo].[TBL_PROFILE] B ON " +
                                "B.[USER_ID] = A.[USER_ID] " +
                                ") B ON B.USER_GEN = A.[R_RELEASED_USER] " +
                                "WHERE R_NV_UN = '" + _un + "' AND R_MODEL = '" + _model + "' AND REPEAT_COUNT>=0 " +
                                "ORDER BY REPEAT_COUNT");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();

            return hasil;
        }

        public string updateGiEngineering(string _nv, string _gen, string _id)
        {
            string output = "ERROR, Simpan data gagal";
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
                string sql = string.Format("EXEC [REPAIR].dbo.[update_gi_engineering_nv] '" + _gen + "', '" + _id + "'");
                connection.Open();
                command = new SqlCommand(sql, connection);
                SqlDataAdapter reader = new SqlDataAdapter(command);
                reader.Fill(hasil);
                connection.Close();
                foreach (DataRow row in hasil.Rows)
                {
                    output = row[0].ToString();
                }
               
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
