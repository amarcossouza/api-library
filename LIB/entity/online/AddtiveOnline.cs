using PHD_TAS_LIB.entity.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.online
{
    public class AdditiveOnline
    {
        public string name { set; get; }
        public int volumeLoaded { set; get; }
        public int index { set; get; }

        public string volumeLoadedReal(string delimiter = ",")
        {
            return (volumeLoaded / 1000.0).ToString().Replace(",", delimiter);
        }
    }
}
