using System;
using System.IO;

namespace AutoInspection.Utils
{
    public   class Logger
    {
        public static void Write(string msg)
        {
            DateTime now = DateTime.Now;
            File.AppendAllText( "Log\\" + now.ToShortDateString() + ".log", now.ToShortTimeString() + "> " + msg + "\r\n");
        }
    }
}
