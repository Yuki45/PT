using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;

namespace repairhhp
{
    public class Inifiles
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, int key, string defaultValue, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(int section, string key, string defaultValue, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);
        public static void WriteValue(string path, string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, path);
        }

        public static string[] GetSectionNames(string Names)
        {
            // Verify the file exists
            if (!File.Exists(Names))
                return null;

            // Loop until the buffer has grown enough to fit the value
            for (int maxSize = 500; true; maxSize *= 2)
            {
                byte[] bytes = new byte[maxSize];
                int size = GetPrivateProfileString(0, "", "", bytes, maxSize, Names);

                if (size < maxSize - 2)
                {
                    // Convert the buffer to a string and split it
                    string sections = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
                    if (sections == "")
                        return new string[0];
                    return sections.Split(new char[] { '\0' });
                }
            }
        }

        private void VerifyAndAdjustSection(ref string section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            section = section.Trim();
        }

      /*  public static string GetValue(string path, string Names,string section)
        {

            // Loop until the buffer has grown enough to fit the value
            for (int maxSize = 250; true; maxSize *= 2)
            {
                StringBuilder result = new StringBuilder(maxSize);
                int size = GetPrivateProfileString(Names, section, "", result, maxSize, path);

                if (size < maxSize - 1)
                {
                    if (size == 0 )
                        return null;
                    return result.ToString();
                }
            }
          return ReadValue(path , Names, section );
        }*/
              
        private bool HasSection(string section, string names)
        {
            string[] sections = GetSectionNames(names);

            if (sections == null)
                return false;

            VerifyAndAdjustSection(ref section);
            return false;// Array.IndexOf(sections, section) >= 0;
        }

        public static string[] GetEntryNames(string section, string Names)
        {
            // Verify the section exists
            // if (!HasSection(section,Names))
            //    return null;            
            // Loop until the buffer has grown enough to fit the value
            for (int maxSize = 500; true; maxSize *= 2)
            {
                byte[] bytes = new byte[maxSize];
                int size = GetPrivateProfileString(section, 0, "", bytes, maxSize, Names);

                if (size < maxSize - 2)
                {
                    // Convert the buffer to a string and split it
                    string entries = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
                    if (entries == "")
                        return new string[0];
                    return entries.Split(new char[] { '\0' });
                }
            }
        }

        public static string ReadValue(string path, string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, path);
            return temp.ToString();
        }

        public static void AddApplicationToStartup(string name, string path)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue(name, path);
            }
        }

        public static void RemoveApplicationFromStartup(string name)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue(name, false);
            }
        }

        public static void WriteLog(string strLog, string path)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            string line = DateTime.Now.ToString() + " | ";

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

        public static void WriteLogAgent(string strLog, string path)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            string line = "";//DateTime.Now.ToString() + " | ";

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

        public static String left(String input, int len)
        {
            return input.Substring(0, len);
        }

        public static String right(String input, int len)
        {
            return input.Substring(input.Length - len);
        }

        public static String mid(String input, int index, int len)
        {
            return input.Substring(index - 1, index + len - 1);
        }

        public static String mid(String input, int index)
        {
            return input.Substring(index - 1);
        }

    }
}
