using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class Released
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;

        public SqlDataAdapter getStockRepair()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, " +
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE'," +
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE'," +
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV]," +
            "[R_CAUSE] AS 'CAUSE',[R_LOCATION] AS 'LOCATION',[R_REMARK] AS 'REMARK', [R_ID] AS ID, [R_NV_PART] AS PART, [R_PART_NUMBER] AS 'PART NUMBER' " +
            "FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE R_STS_GI_TO_ENG IS NOT NULL AND (ISNULL(R_STATUS,'') = '' OR R_STATUS = 'GR PE')");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getStockRepairCN(string _cn)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, " +
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE'," +
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE'," +
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV]," +
            "[R_CAUSE] AS 'CAUSE',[R_LOCATION] AS 'LOCATION',[R_REMARK] AS 'REMARK', [R_ID] AS ID, [R_NV_PART] AS PART, [R_PART_NUMBER] AS 'PART NUMBER' " +
            "FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE R_STS_GI_TO_ENG IS NOT NULL  AND (ISNULL(R_STATUS,'') = '' OR R_STATUS = 'GR PE') AND [R_CN] LIKE '%" + _cn + "%'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getRepairStock()
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, " +
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE'," +
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE'," +
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV]," +
            "[R_CAUSE] AS 'CAUSE',[R_LOCATION] AS 'LOCATION',[R_REMARK] AS 'REMARK', [R_ID], [R_STS_GI_TO_ENG] AS 'ENG GEN', " +
            "(SELECT P_NAME FROM [REPAIR].[dbo].[TBL_LOGIN] A INNER JOIN [REPAIR].[dbo].[TBL_PROFILE] B ON B.USER_ID = A.USER_ID AND A.USER_GEN = [REPAIR].[dbo].[TBL_REPAIR_STATUS]. [R_STS_GI_TO_ENG]) AS 'ENG NAME', [R_STS_GI_TO_PE] AS 'PE GEN', " +
            "(SELECT P_NAME FROM [REPAIR].[dbo].[TBL_LOGIN] A INNER JOIN [REPAIR].[dbo].[TBL_PROFILE] B ON B.USER_ID = A.USER_ID AND A.USER_GEN = [REPAIR].[dbo].[TBL_REPAIR_STATUS]. [R_STS_GI_TO_PE]) AS 'PE NAME'" +
            "FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE ISNULL(R_STS_GI_TO_ENG,'') != '' AND ((R_STATUS != 'REPAIRED' AND R_STATUS != 'ABOLISH' AND R_STATUS != 'RMA') OR ISNULL(R_STATUS,'') = '')");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            connection.Close();
            return reader;
        }

        public SqlDataAdapter getRepairStockCN(string _cn)
        {
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY R_INPUT_DATE DESC) AS NO, " +
            "[R_MODEL] AS MODEL,[R_LINE] AS LINE,[R_CN] AS CN,[R_NV_UN] AS UN," +
            "[R_NV_PROCESS] AS PROCESS, [R_NV_SYMPTOM_CODE] AS 'DEFECT CODE'," +
            "[R_DEFECT_NAME] AS 'DEFECT NAME',[R_INPUT_DATE] AS 'INPUT DATE'," +
            "'|'+[R_CN]+'|'+[R_NV_UN]+'|'+[R_NV_PROCESS]+'|'+[R_NV_SYMPTOM_CODE]+'|'+[R_NV_TEST_ITEM]+'|'+[R_NV_VALUE]+'|'+[R_NV_STANDAR_L]+'|'+[R_NV_STANDAR_U]+'|---|'+[R_LINE]+'|'+[R_MODEL]+'|' AS [NV]," +
            "[R_CAUSE] AS 'CAUSE',[R_LOCATION] AS 'LOCATION',[R_REMARK] AS 'REMARK', [R_ID], [R_STS_GI_TO_ENG] AS 'ENG GEN', " +
            "(SELECT P_NAME FROM [REPAIR].[dbo].[TBL_LOGIN] A INNER JOIN [REPAIR].[dbo].[TBL_PROFILE] B ON B.USER_ID = A.USER_ID AND A.USER_GEN = [REPAIR].[dbo].[TBL_REPAIR_STATUS]. [R_STS_GI_TO_ENG]) AS 'ENG NAME', [R_STS_GI_TO_PE] AS 'PE GEN', " +
            "(SELECT P_NAME FROM [REPAIR].[dbo].[TBL_LOGIN] A INNER JOIN [REPAIR].[dbo].[TBL_PROFILE] B ON B.USER_ID = A.USER_ID AND A.USER_GEN = [REPAIR].[dbo].[TBL_REPAIR_STATUS]. [R_STS_GI_TO_PE]) AS 'PE NAME'" +
            "FROM [REPAIR].[dbo].[TBL_REPAIR_STATUS]" +
            "WHERE ISNULL(R_STS_GI_TO_ENG,'') != '' AND ((R_STATUS != 'REPAIRED' AND R_STATUS != 'ABOLISH' AND R_STATUS != 'RMA') OR ISNULL(R_STATUS,'') = '') AND [R_CN] LIKE '%" + _cn + "%'");
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

        public bool EditFieldNV(string _nv, string cause, string location, string status, string user, string cn, string remarkDec, string absDec, string _r_status, string _check_un, string _defectDec, string _partnumber, string _id)
        {
            bool output = false;
            DataTable hasil = new DataTable();
            var count = _nv.Count(x => x == '|');
            if (count != 12)
            {
                output = false;
            }
            else
            {
                string[] arrCn = _nv.Split('|');
                string _cn = arrCn[1];
                string _nv_un = arrCn[2];
                string _un = "";
                if (string.IsNullOrWhiteSpace(_nv_un))
                {
                    _un = _check_un;
                }
                else
                {
                    _un = _nv_un;
                }
                string _line = arrCn[3];
                string _gen = arrCn[4];
                string _cause = arrCn[5];
                string _value = arrCn[6];
                string _standar_l = arrCn[7];
                string _standar_u = arrCn[8];

                SqlConnection connection = _Conn.Connect();
                string sql = string.Format("EXEC dbo.[Released_repair_nv] " +
                                "'" + cause + "'" +
                                ",'" + location + "'" +
                                ", '" + status + "'" +
                                ", '" + user + "'" +
                                ", '" + remarkDec + "'" +
                                ", '" + _cn + "'" +
                                ", '" + _nv_un + "'" +
                                ", '" + _line + "'" +
                                ", '" + _gen + "'" +
                                ", '" + _cause + "'" +
                                ", '" + _value + "'" +
                                ", '" + _standar_l + "'" +
                                ", '" + _standar_u + "'" +
                                ", '" + _r_status + "'" +
                                ", '" + absDec + "'" +
                                ", '" + _un + "'" +
                                ", '" + _defectDec + "'" +
                                ", '" + _partnumber + "'" +
                                ", '" + _id + "'");
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

        public string updateGenPIC(string _cn, string _un, string _process, string _defect, string _genEng, string _genPE, string _id)
        {
            string output = "ERROR, Update data PIC failed";
            DataTable hasil = new DataTable();

            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("EXEC [REPAIR].dbo.[update_gen_pic] '" + _cn + "', '" + _un + "', '" + _process + "', '" + _defect + "', '" + _genEng + "', '" + _genPE + "', '" + _id + "'");
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

        public List<string> GetCause()
        {
            List<string> output = new List<string>();

            DataTable hasil = new DataTable();
            SqlConnection connection = _Conn.Connect();
            string sql = string.Format("SELECT CAUSE_DESC FROM [REPAIR].[dbo].[TBL_MDM_CAUSE]");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            output = hasil.AsEnumerable().Select(x => x[0].ToString()).ToList();

            return output;
        }
    }
}
