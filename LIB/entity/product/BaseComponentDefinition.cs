using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.product
{
    public abstract class BaseComponentDefinition
    {
        public int indexPreset { set; get; }
        public int indexInPreset { set; get; }
        public int indexProduct { set; get; }

        public string code { set; get; }
    }
}
