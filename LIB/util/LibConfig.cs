using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.util
{
    public static class LibConfig
    {
        public static readonly string App_Data = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data";
        public static string AppDataTo(string file)
        {
            return $@"{App_Data}\{file}";
        }

        public static string tankInterpolatedTableFile { private set; get; } = loadTankInterpolatedTableFile();
        public static string loadTankInterpolatedTableFile()
        {
            string tankInterpolatedTableFile = ConfigurationManager.AppSettings["tankInterpolatedTableFile"];
            if (string.IsNullOrEmpty(tankInterpolatedTableFile))
            {
                Logging.Error("ON LOAD AppSettings[tankInterpolatedTableFile]");
                throw new ConfigurationErrorsException("Erro to Load Configuration");
            }
            return tankInterpolatedTableFile;
        }
    }
}
