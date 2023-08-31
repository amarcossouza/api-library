using DbExtensions;
using PHD_TAS_LIB.entity.online;
using PHD_TAS_LIB.multiload;
using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.online
{
    public class PresetOnlineData : BaseEntity
    {
        [Column]
        public int volumePreseted { set; get; }
        [Column]
        public int volumeLoaded { set; get; }
        [Column]
        public int volumeLoaded20 { set; get; }
        [Column]
        public int flowRate { set; get; }
        [Column]
        public int temperature { set; get; }
        [Column]
        public int avaregeDensity { set; get; }

        public string temperatureReal(string delimiter = ",")
        {
            return (temperature / 100.0).ToString().Replace(",", delimiter);
        }

        public string avaregeDensityReal(string delimiter = ",")
        {
            return (avaregeDensity / 10.0).ToString().Replace(",", delimiter);
        }

        public int volumeOperationPercent()
        {
            if (volumeLoaded > volumePreseted)
                return 100;
            if (volumePreseted <= 0)
                return 0;
            return (int)(((double)volumeLoaded / (double)volumePreseted) * 100.0);
        }

        public int volumeOperationPercent(int volumeTotal)
        {
            if (volumeLoaded > volumeTotal)
                return 100;
            if (volumeTotal <= 0)
                return 0;
            int i = Convert.ToInt32(((double)volumeLoaded / (double)volumeTotal) * 100.0);
            return (int)(((double)volumeLoaded / (double)volumeTotal) * 100.0);
        }
    }
}
