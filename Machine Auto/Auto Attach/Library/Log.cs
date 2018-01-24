
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Attach.Library
{
    public class Log
    {
        public static MainForm mainForm;
        static string fileName;
        static string modelName;
        const string topCode = "TOPD69";
        const string portNum = "01";
        static DateTime dtStart;
        static public Queue<string> logQueue;
		static public Queue<string> logPmQueue;

        static public Queue<string> failListQueue;

        public Log(MainForm _mainForm,string path,string _modelName)
        {
            mainForm = _mainForm;
            modelName = _modelName;
            logQueue = new Queue<string>();
			logPmQueue = new Queue<string>();

			
            failListQueue = new Queue<string>();

            fileName = path + "\\" + modelName + "_"+ topCode + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + portNum + ".csv" ;
            if (!File.Exists(fileName))
            {
                FileStream fs = new FileStream(fileName,FileMode.OpenOrCreate,FileAccess.ReadWrite);
            }
        }

        public static void AddLog(string msg)
        {
            string logTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss : ");
            if( mainForm != null )
                mainForm.DisplayLog(logTime + msg);

            if(logQueue!=null)
                logQueue.Enqueue(logTime + msg);
        }

        public static void SaveLog()
        {
           if (logQueue.Count == 0) return;

            string saveTime = DateTime.Now.ToString("yyyy-MM-dd");
            if (!Directory.Exists("Log\\")) Directory.CreateDirectory("Log");
            
            StreamWriter sw = new StreamWriter("Log\\" + saveTime + ".log", true);

            while (logQueue.Count > 0)
            {
                string msg = logQueue.Dequeue();
                sw.WriteLine(msg);
            }

            sw.Flush();
            sw.Close();
        }

        public static void AddPmLog(string msg)
        {
            string logTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss : ");

            if (logPmQueue != null)
                logPmQueue.Enqueue(logTime + msg);
        }

        public static void SavePmLog()
        {
            if (logPmQueue.Count == 0) return;

            string saveTime = DateTime.Now.ToString("yyyy-MM-dd");
            StreamWriter sw = new StreamWriter("Log\\" + saveTime + ".Pm.log", true);

            while (logPmQueue.Count > 0)
            {
                string msg = logPmQueue.Dequeue();
                sw.WriteLine(msg);
            }

            sw.Flush();
            sw.Close();
        }

        public static void AddHeadInfo(string pn, string swver)
        {
            try
            {
                TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Read));
                dtStart = DateTime.Now;
                tw.WriteLine("\n#INIT");
                tw.WriteLine("MODEL : " + modelName);
                tw.WriteLine("P/N : " + pn);
                tw.WriteLine("S/W : " + swver);
                tw.WriteLine("DATE : " + dtStart.ToString("yyyy'/'MM'/'dd"));
                tw.WriteLine("TIME : " + dtStart.ToString("HH:mm:ss"));
                tw.WriteLine("INSTRUMENT : ");
                tw.WriteLine("FIRMWARE : ");
                tw.WriteLine("JIG : 1");
                tw.WriteLine("PROGRAM : LCD Inspection");
                tw.WriteLine("INIFILE : ");
                tw.WriteLine("TESTCODE : TOPD69");
                tw.WriteLine("LOGVERSION : V1.0");
                tw.WriteLine("HW : ");
                tw.WriteLine("\n#TEST");
                tw.WriteLine("/*================================================================================");
                tw.WriteLine("Test Conditions,Measured Value,Lower Limit,Upper Limit,P/F,Sec,Code,Code Lsl,Code Usl");
                tw.WriteLine("================================================================================*/");
                tw.Flush();
                tw.Close();
            }
            catch (Exception ex)
            {
                Log.AddLog("AddHeadInfo() in Log exception " + ex.ToString());
                Log.AddPmLog("AddHeadInfo() in Log exception " + ex.ToString());
            }
        }
        public static void AddInfo(string item, string value, string lowspec, string highspec, bool passfail, int sec)
        {
            string result = passfail ? "P" : "F";

            if (lowspec == "-") lowspec = "-1";
            if (highspec == "-") highspec = "-1";

            try
            {
                TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Read));
                tw.WriteLine(item + "," + value + "," + lowspec + "," + highspec + "," + result +  "," + sec + ",,");
                tw.Flush();
                tw.Close();
                if (result == "F")
                    failListQueue.Enqueue(item);
            }
            catch (Exception ex)
            {
                Log.AddLog("AddInfo() in Log exception " + ex.ToString());
                Log.AddPmLog("AddInfo() in Log exception " + ex.ToString());
            }
        }

        public static void AddEndInfo(bool result, int sum, int fail)
        {
            try
            {
                string r = result ? "PASS" : "FAIL";
                TimeSpan t = DateTime.Now - dtStart;
                int testTime = t.Seconds;
                //string testTime = t.TotalSeconds.ToString().Substring(0, 4);
                string failItem = "";
                while (failListQueue.Count > 0)
                {
                    if (failListQueue.Count > 1)
                        failItem += failListQueue.Dequeue() + ",";
                    else
                        failItem += failListQueue.Dequeue();
                }

                TextWriter tw = new StreamWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Read));
                tw.WriteLine("\n#END");
                tw.WriteLine("RESULT : " + r);
                tw.WriteLine("TEST-TIME : " + testTime);
                tw.WriteLine("FAILITEM : " + failItem);
                tw.WriteLine("//Total : " + sum + " Pass : " + (sum - fail) + " Fail : " + fail);
                tw.WriteLine("//FAIL_RATE : " + String.Format("{0:0.00}", fail * 100 / sum) + "%");
                tw.WriteLine("\n" + Convert.ToChar(12));
                tw.Flush();
                tw.Close();
            }
            catch (Exception ex)
            {
                Log.AddLog("AddEndInfo() in Log exception " + ex.ToString());
                Log.AddPmLog("AddEndInfo() in Log exception " + ex.ToString());
            }
        }


        /*
		public string FolderCheckAndMake(string path)
        {
            string[] spath = path.Split('\\');
            string tpath = "";

            foreach (string name in spath)
            {
                if (name.Trim() != "")
                {
                    tpath += (name + "\\");

                    if (Directory.Exists(tpath) == false)
                    {
                        Directory.CreateDirectory(tpath);
                    }
                }
            }
            return path;
        }
*/

    }
}
