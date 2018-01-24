using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using repairhhp.Models;

namespace repairhhp.Controls
{
    public class RackGR
    {
        public static Conn _Conn = new Conn();
        public static SqlCommand command;

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

        public string  GenSlipNo()
        {
            SqlConnection connection = _Conn.Connect();
            DataTable hasil = new DataTable();
            string SlipNo = "";
            string sql = string.Format(@"SELECT TOP 1 [item_slip]  FROM [MAIN].[dbo].[materialtrx]
                                         WHERE [MAIN].[dbo].[materialtrx].[item_slip] like '%" + DateTime.Now.ToString("yyyyMMdd") + "%'  Order By [item_slip] desc");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();

            if (hasil.Rows.Count > 0)
            {
                foreach (DataRow row in hasil.Rows)
                {                   
                    int no = Convert.ToInt32(row[0].ToString().Substring(8, 3)) + 1;
                    if (no.ToString().Length == 1)
                    {
                        SlipNo = DateTime.Now.ToString("yyyyMMdd") + "00"+no.ToString();
                    }
                    else
                    if (no.ToString().Length == 2)
                    {
                        SlipNo = DateTime.Now.ToString("yyyyMMdd") + "0" + no.ToString();
                    }
                    else
                    if (no.ToString().Length == 3)
                    {
                        SlipNo = DateTime.Now.ToString("yyyyMMdd") + no.ToString();
                    }
                }

                
            }
            else
            {
                SlipNo = DateTime.Now.ToString("yyyyMMdd")+"001";
            }

            return SlipNo;
        }

        public DataTable getItem()
        {
            SqlConnection connection = _Conn.Connect();
            DataTable hasil = new DataTable();
            string sql = string.Format("SELECT [id] as kode,[item_name]as nama  FROM [MAIN].[dbo].[items]");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public string getItemCode(string kode)
        {
            SqlConnection connection = _Conn.Connect();
            DataTable hasil = new DataTable();
            string kodes = "";
            string sql = string.Format("SELECT [id] as kode  FROM [MAIN].[dbo].[items] Where [item_name] ='" + kode + "'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();

            foreach (DataRow row in hasil.Rows)
            {
                kodes = row[0].ToString();
            }

            return kodes;
        }

        public DataTable getStorage()
        {
            SqlConnection connection = _Conn.Connect();
            DataTable hasil = new DataTable();
            string sql = string.Format("SELECT [strg_code] as code,[strg_name]as nama  FROM [MAIN].[dbo].[storages]");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public string getStorageCode(string kode)
        {
            SqlConnection connection = _Conn.Connect();
            DataTable hasil = new DataTable();
            string kodes="";
            string sql = string.Format("SELECT [strg_code] as code FROM [MAIN].[dbo].[storages] Where [strg_name]='"+kode+"'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();

            foreach(DataRow row in hasil.Rows)
            {
                kodes = row[0].ToString();
            }

            return kodes;
        }

        public DataTable getZone(string kode)
        {
            SqlConnection connection = _Conn.Connect();
            DataTable hasil = new DataTable();
            string sql = string.Format("SELECT [zone_code],[zone_name] FROM [MAIN].[dbo].[zones]  Where [strg_code] ='"+kode+"'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

        public string getZoneCode(string kode, string strg)
        {
            SqlConnection connection = _Conn.Connect();
            DataTable hasil = new DataTable();
            string sql = string.Format("SELECT [zone_code] FROM [MAIN].[dbo].[zones]Where [zone_name]='" + kode + "' And [strg_code] ='"+strg+"'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();

            string kodes = "";
            foreach (DataRow row in hasil.Rows)
            {
                kodes = row[0].ToString();
            }

            return kodes;

        }

        public DataTable getBin(string kodestorage, string kodezone)
        {
            SqlConnection connection = _Conn.Connect();
            DataTable hasil = new DataTable();
            string sql = string.Format("SELECT [bin_code]  FROM [MAIN].[dbo].[bins]  Where [strg_code] ='"+kodestorage+"' And [zone_code] ='"+kodezone+"'");
            connection.Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter reader = new SqlDataAdapter(command);
            reader.Fill(hasil);
            connection.Close();
            return hasil;
        }

    }
}
