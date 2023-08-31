using DbExtensions;
using Newtonsoft.Json;
using PHD_TAS_LIB.entity.tag;
using PHD_TAS_LIB.modbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.product
{
    [Table(Name = "pump_product_config")]
    public class PumpProductConfig : BaseEntity
    {
        //TOOD: composition with product class or only keep name? keep product reference?
        [Column]
        public string name { set; get; }

        // TODO: Create enum
        [Column]
        public int productNumber { set; get; } = 0;

        [Column]
        public int horimetre { set; get; }

        [Column]
        public int sequential { set; get; }

        [Column(ConvertTo = typeof(sbyte))]
        public bool atualize { set; get; } = false;

        //conjunto de min(20) com bracos, bombas e frequencia

        public List<RequestPumpConfig> config { set; get; } = createConfig();

        [Column(Name = "config")]
        public string _config
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    config = createConfig();
                    return;
                }
                config = JsonConvert.DeserializeObject<List<RequestPumpConfig>>(value);
            }
            get { return JsonConvert.SerializeObject(config); }
        }

        public TagCollection tags { private set; get; }

        public void setTagCollection(Tag[] tags)
        {
            if (tags == null || tags.Length == 0)
                throw new ArgumentException();

            if (this.tags == null || this.tags.Count > 0)
                this.tags = new TagCollection(tags);
        }

        public void readOnlineValuesOn(ModbusDataPackage read, ModbusInterface modbus)
        {
            int horimetreRegister = tags.getRegisterBy(sequential,"horimetre");
            int horimetreInPLC = read.getUShort(horimetreRegister);
            if (atualize && this.horimetre != horimetreInPLC)
            {
                modbus.escritaSingle(horimetreRegister, horimetre);
            }
            else
            {
                this.horimetre = horimetreInPLC;
            }

            foreach (var c in config)
            {
                int pumpRegister = tags.getRegisterBy("PRODUCT_PUMPS_" + c.number);
                int pumpsInPLC = read.getUShort(pumpRegister);
                if (atualize && c.pumps != pumpsInPLC)
                {
                    modbus.escritaSingle(pumpRegister, c.pumps);
                }
                else
                {
                    c.pumps = pumpsInPLC;
                }

                int freqRegister = tags.getRegisterBy("PRODUCT_FREQ_" + c.number);
                int freqInPLC = read.getUShort(freqRegister);
                if (atualize && c.frequency != freqInPLC)
                {
                    modbus.escritaSingle(freqRegister, c.frequency);
                }
                else
                {
                    c.frequency = freqInPLC;
                }
            }
        }

        private static List<RequestPumpConfig> createConfig()
        {
            var config = new List<RequestPumpConfig>(20);
            for (int i = 0; i < 20; i++)
            {
                config.Add(new RequestPumpConfig() { number = i + 1 });
            };
            return config;
        }
    }

    //TODO: create struct
    public class RequestPumpConfig
    {
        public int number { set; get; } = 1;
        public int pumps { set; get; } = 0;
        public int frequency { set; get; } = 0;
    }
}
