using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Scanner_Aging_OCV
{
    public class logfiles
    {
        public static void WriteLogAgent(string strLog, string path)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            string line = "";

            line += strLog;

            string logFilePath = path;
            //logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(line);
            log.Close();
        }
    }
}
