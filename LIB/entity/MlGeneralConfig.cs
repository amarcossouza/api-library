using DbExtensions;
using PHD_TAS_LIB.modbus;
using System.Collections.Generic;
using System.Linq;
namespace PHD_TAS_LIB.entity
{
    [Table(Name = "ml_general_config")]
    public class MlGeneralConfig : BaseEntity
    {
        [Column]
        public string name { set; get; }

        [Column]
        public int value { set; get; }

        public decimal valueMask { set; get; }

        [Column]
        public int correction { set; get; }



        public decimal valueMaskGet()
        {
            return (decimal)value / correction;
        }


        public decimal valueMaskSet()
        {
            return valueMask * correction;           
        }



        [Column]
        public string description { set; get; }
        [Column(ConvertTo = typeof(sbyte))]
        public bool atualize { set; get; } = false;
        [Column]
        public int idDevice { set; get; }
        [Column]
        public int idBay { set; get; }
        [Column]
        public string category { set; get; }

        public bool hasCategory() => !string.IsNullOrWhiteSpace(category);

        [Column]
        public string unit { set; get; }

        public bool hasUnit() => !string.IsNullOrWhiteSpace(unit);

        [Column]
        public string type { set; get; }
        [Column(ConvertTo = typeof(sbyte))]
        public bool active { set; get; } = false;

      


       



        public Dictionary<string, List<MlGeneralConfig>> configs { set; get; }


    }
}