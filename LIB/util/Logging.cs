using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.util
{
    public static class Logging
    {
        public static string LOG_DIR = AppDomain.CurrentDomain.BaseDirectory + @"\log\";

        public static void log(string msg, string contextFileName = "SERVICE", LogSeverity severity = LogSeverity.INFO)
        {
            string file = $"{LOG_DIR}{contextFileName}_{DateTime.Today.ToString("dd-MM-yyyy")}.txt";
            string toWrite = $"\n {severity} | {msg} | {DateTime.Now.ToString()}";
            write(file, toWrite);
        }

        public static void Error(string msg, string contextFileName = "SERVICE")
        {
            string file = $"{LOG_DIR}{contextFileName}_{DateTime.Today.ToString("dd-MM-yyyy")}.txt";
            string toWrite = $"\n {LogSeverity.ERROR} | {msg} | {DateTime.Now.ToString()}";
            write(file, toWrite);
        }

        public static void Warn(string msg, string contextFileName = "SERVICE")
        {
            string file = $"{LOG_DIR}{contextFileName}_{DateTime.Today.ToString("dd-MM-yyyy")}.txt";
            string toWrite = $"\n {LogSeverity.WARN} | {msg} | {DateTime.Now.ToString()}";
            write(file, toWrite);
        }

        public static void Info(string msg, string contextFileName = "SERVICE")
        {
            string file = $"{LOG_DIR}{contextFileName}_{DateTime.Today.ToString("dd-MM-yyyy")}.txt";
            string toWrite = $"\n {LogSeverity.INFO} | {msg} | {DateTime.Now.ToString()}";
            write(file, toWrite);
        }

        public static void Log(string msg, string customFileContext, string info = null)
        {
            string file = $"{LOG_DIR}{customFileContext}.txt";
            info = info == null ? LogSeverity.INFO.ToString() : info;
            string toWrite = $"\n {info} | {msg} | {DateTime.Now.ToString()}";
            write(file, toWrite);
        }

        private static void write(string file, string toWrite)
        {
            StreamWriter log = null;
            try
            {
                log = writeOnStream(file, toWrite);
            }
            catch (Exception e)
            {
                if (!Directory.Exists(LOG_DIR))
                {
                    Console.WriteLine("CREATING LOG_DIR: " + file);
                    Directory.CreateDirectory(LOG_DIR);
                    try { log = writeOnStream(file, toWrite); } catch { }
                }
                else
                {
                    Console.WriteLine("ERROR LOG: " + file);
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }
            }
            finally
            {
                if (log != null)
                    log.Close();
            }
        }

        private static StreamWriter writeOnStream(string file, string toWrite)
        {
            StreamWriter log;
            using (log = File.AppendText(file))
            {
                log.WriteLine(toWrite);
                System.Diagnostics.Debug.WriteLine(toWrite);
                Console.WriteLine(toWrite);
                log.Close();
            }
            return log;
        }
    }

    public enum LogSeverity
    {
        ERROR,
        WARN,
        INFO
    }
}
