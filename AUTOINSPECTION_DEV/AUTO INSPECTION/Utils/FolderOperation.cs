using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInspection
{
    class ResultImageLogger
    {
        static string defaultFolderName = "Current\\";
        static string WorkingFolder;
        static string LastFolder;

        public static string GetImageLogFolder()
        {
            return WorkingFolder + defaultFolderName; 
        }
        public static string GetLastLogFolder()
        {
            return  LastFolder;
        }

        // 1. Delete Target Folder and ReCreate 
        // _rootPath : @"D:\LCD_AutoInspection\IMG\"
        public static void ResetDefaultLogFoler(string _rootPath)
        {
            string _s = DateTime.Now.ToString("yyyyMMdd");

            WorkingFolder = _rootPath + _s + "\\";
            DeleteFolder(WorkingFolder + defaultFolderName );
            Directory.CreateDirectory(WorkingFolder + defaultFolderName);
        }

        // 2. Rename Folder : src Fodler  -> newFolder
        //  newFolderName : UN[_Fail]
        //  D:\LcdTest\IMG\ 20170429 \ Currnet -> D:\LcdTest\IMG\ 20170429 \ UN[_Fail]
        public static void RenameImageLogFolder(string newFolderName)
        {
            string _srcFolder = GetImageLogFolder();
            string _newFolder = WorkingFolder + newFolderName;

            DirectoryInfo srcDir = new DirectoryInfo(_srcFolder);
            DirectoryInfo newDir = new DirectoryInfo(_newFolder);

            if (newDir.Exists)
            {
                DeleteFolder(_srcFolder);
            }

            if (srcDir.Exists)
            {
                srcDir.MoveTo(_newFolder);
                LastFolder = _newFolder;
            }
        }

        static void DeleteFolder(string _targetFolder)
        {
            if (Directory.Exists(_targetFolder))
            {
                DirectoryInfo dir = new DirectoryInfo(_targetFolder);
                FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo file in files)
                    file.Attributes = FileAttributes.Normal;

                Directory.Delete(_targetFolder, true);
            }
        }
    }
}
