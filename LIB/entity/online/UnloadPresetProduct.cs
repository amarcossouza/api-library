using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.online
{   
    //TODO: Check if is better storage this in json, for totalizer, config and instruments
    [Table(Name ="unload_preset_product")]
    public class UnloadPresetProduct : BaseEntity
    {
        [Column]
        public int idDevice { set; get; }
        [Column]
        public int sequential { set; get; }
        [Column]
        public int number { set; get; }
        [Column]
        public string name { set; get; }

        // TODO: add idProduct
    }
}
