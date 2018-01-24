using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Kitting_EPASS.DB
{
    public class DB
    {
        public SqlConnection con;
        public SqlDataAdapter dAdapter;
        public DataSet dSet;
        public string conString = @"Server=107.102.47.105;Database=PROD;user id=sa;password=seinadminhhp";

        /*-------------------------------------------------------------------------
       * public method  Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\Gawean\Project\MPMS\DatabaseAccessWithADONET\PersonDatabase.mdb;Persist Security Info=True
       * Overloaded		: no
       * Parameters		: no
       * Return value		: bool (true or false)
       * Purpose			: creates a connection to the database,a DataAdapter,
        *						  a DataSet and returns true if all ok otherwise false
        *-------------------------------------------------------------------------*/

        public bool testConnection()
        {
            bool hasil = false;

            try
            {
                con = new SqlConnection(conString);
                con.Open();
                hasil = true;
                con.Close();
            }
            catch
            {
                hasil = false;
            }

            return hasil;
        }

        public DataTable getData(string sql)
        {
            DataTable hasil = new DataTable();
            try
            {
                con = new SqlConnection(conString);
                con.Open();
                SqlCommand command = new SqlCommand(sql, con);
                SqlDataAdapter reader = new SqlDataAdapter(command);
                reader.Fill(hasil);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
                //connectection failed
            }//try-catch	   
            //connection ok!
            con.Close();
            return hasil;
        }

        public bool setData(string sql)
        {
            bool hasil = false;
            try
            {
                con = new SqlConnection(conString);
                SqlCommand command = new SqlCommand(sql, con);
                command.ExecuteNonQuery();
                hasil = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }//try-catch	   
            //connection ok!
            con.Close();
            return hasil;
        }

        public bool setData2(string sql)
        {
            bool hasil = false;
            try
            {
                con = new SqlConnection(conString);
                con.Open();
                SqlCommand command = new SqlCommand(sql, con);
                command.ExecuteScalar();
                hasil = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }//try-catch	   
            //connection ok!
            con.Close();
            return hasil;
        }
    }
}
