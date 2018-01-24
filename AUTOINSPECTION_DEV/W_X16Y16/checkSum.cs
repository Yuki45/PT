using System;
using System.Collections.Generic;
using System.Text;

namespace DIO
{
    public class CheckSum
    {
        static public string CRC(string data) // 체크섬
        {
            string data1 = data.Substring(0, 2);
            string data2 = data.Substring(2, 2);
            //  string data3 = data.Substring(4, 2);
            //  string data4 = data.Substring(6, 2);

            int data1_int = Convert.ToInt32(data1, 16);
            int data2_int = Convert.ToInt32(data2, 16);
            //  int data3_int = Convert.ToInt32(data3, 16);
            //  int data4_int = Convert.ToInt32(data4, 16);

            int crc_int = data1_int + data2_int; //+ data3_int + data4_int;

            string crc_st = Convert.ToString(crc_int, 16);

            char[] crc_st_1 = crc_st.ToCharArray();

            string crc_st_2 = "";
            switch (crc_st_1.Length)
            {
                case 1:
                    crc_st_2 += "0" + crc_st_1[0].ToString();
                    break;

                case 2:
                    crc_st_2 += crc_st_1[0].ToString() + crc_st_1[1].ToString();
                    break;

                case 3:
                    crc_st_2 += crc_st_1[1].ToString() + crc_st_1[2].ToString();
                    break;
                case 4:
                    crc_st_2 += crc_st_1[3].ToString() + crc_st_1[4].ToString();
                    break;
            }

            return crc_st_2;

        }





    }
}
