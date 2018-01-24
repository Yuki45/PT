using AutoInspection.Utils;
using AutoInspection_GUMI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoInspection
{
    public class HeadInfoData
    {
        private string _Model = string.Empty;
        private string _Nature = string.Empty;
        private string _CustomerCode = string.Empty;
        private string _Date = string.Empty;
        private string _Charger = string.Empty;
        private string _Version = string.Empty;
        private string _Checksum = string.Empty;
        private string _Crcsum = string.Empty;
        private string _UniqueNo = string.Empty;
        private string _MemName = string.Empty;
        private string _SecCode = string.Empty;

        public bool DataOK { get; set; }

        public HeadInfoData() 
        {
            this.DataOK = true;
            
        }

        public string ModelName { get { return this._Model; } }
        public string Country { get { return this._Nature; } }
        public string DateRelease { get { return this._Date; } }
        public string Charger { get { return this._Charger; } }
        public string Version { get { return this._Version; } }
        public string CheckSum { get { return this._Checksum; } }
        public string Crcsum { get { return this._Crcsum; } }
        public string UniqueNo { get { return this._UniqueNo; } }
        public string MemoryName { get { return this._MemName; } }
        public string SecCode { get { return this._SecCode; } }

        /// <summary>
        /// Get header information from data
        /// </summary>
        /// <param name="data"></param>
        public void ProcessData(string data)
        {
            if (data.Length < 420)
            {
                DataOK = false;
                return;
            }
            else
            {
                try
                {
                    this._Model = ConvertHexToData(data.Substring(8, 40));
                    this._Nature = ConvertHexToData(data.Substring(48, 80));
                    this._CustomerCode = ConvertHexToData(data.Substring(128, 16));
                    this._Date = ConvertHexToData(data.Substring(144, 28));
                    this._Charger = ConvertHexToData(data.Substring(172, 48));
                    this._Version = ConvertHexToData(data.Substring(220, 40));
                    this._Checksum = ConvertHexToData(data.Substring(260, 20));
                    this._Crcsum = ConvertHexToData(data.Substring(280, 20));
                    this._UniqueNo = ConvertHexToData(data.Substring(300, 40));
                    this._MemName = ConvertHexToData(data.Substring(340, 40));
                    this._SecCode = ConvertHexToData(data.Substring(380, 40));
                }
                catch (Exception e)
                {
                    Log.AddLog("ProcessData() exception " + e.ToString());
                    // Logger.Write("ProcessData() exception " + e.ToString());
                }
            }
        }
        /// <summary>
        /// Convert hex data to string data
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private string ConvertHexToData(string hex)
        {
            string result = string.Empty;
            for (int i = 0; i < hex.Length; i += 2)
            {
                string h = hex.Substring(i, 2);
                if (h.Equals("00"))
                    break;
                result += (char)(int.Parse(h, NumberStyles.HexNumber));
            }
            return result;
        }
    }
}
