using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QuadroEngine
{
    public static class Console
    {
        private static string m_log = " ";

        public static void Log(string text, string value)
        {
            m_log += "\n" + DateTime.Now.TimeOfDay.ToString() + " " + text + ": " + value;
        }

        public static void LogToFile()
        {
            string[] files = Directory.GetFiles("Logs");
            string _path = @"Logs\";
            DirectoryInfo dirInfo = new DirectoryInfo(_path);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            using (FileStream fstream = new FileStream(@"Logs\session " + files.Length.ToString() + " log.txt", FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(m_log);
                fstream.Write(array, 0, array.Length);
            }
        }
    }
}
