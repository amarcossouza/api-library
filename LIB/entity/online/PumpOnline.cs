using DbExtensions;
using Newtonsoft.Json;
using PHD_TAS_LIB.entity.command;
using PHD_TAS_LIB.entity.product;
using PHD_TAS_LIB.entity.tag;
using PHD_TAS_LIB.modbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.online
{   
    [Table(Name ="pump_online")]
    public class PumpOnline : BaseEntity
    {
        [Column]
        public int current { set; get; }
        [Column]
        public int frequency { set; get; }
        [Column]
        public int rpm { set; get; }
        [Column]
        public int tension { set; get; }
        [Column]
        public int tensionEngine { set; get; }
        [Column]
        public int kWh { set; get; }
        [Column]
        public int statusMsg { set; get; }

        //[Column(ConvertTo = typeof(sbyte))]
        //public bool turnOnGenerator { set; get; } = false;

        //[Column(ConvertTo = typeof(sbyte))]
        //public bool withInverter { set; get; } = true;

        [Column]
        public string name { set; get; }
        [Column]
        public int sequential { set; get; }

        public string productName { set; get; }

        //[Association(ThisKey = "sequential", OtherKey = "sequential AND name = 'HAS_GENERATOR'")]
        //public PumpPLCConfig hasGenerator { protected set; get; } = new PumpPLCConfig();

        //// comando online
        //// tags de comando e config...
        //// config de produdo, é partida Direta, config quando é gerador

        public PumpCommandOnline command { set; get; } = new PumpCommandOnline();

        [Column(Name = "command")]
        protected string _command
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    command = new PumpCommandOnline();
                    return;
                }
                command = JsonConvert.DeserializeObject<PumpCommandOnline>(value);
            }
            get { return JsonConvert.SerializeObject(command); }
        }

        public void setOnlineCommand(PumpPLCCommand cmd)
        {
            command.setValuesBy(cmd);
        }


        public TagCollection tags { private set; get; }

        public void setTagCollection(Tag[] tags)
        {

            if (tags == null || tags.Length == 0)
                throw new ArgumentException();

            if (this.tags == null || this.tags.Count > 0)
                this.tags = new TagCollection(tags);
        }

        //[Column]
        //public int idProductConfig { set; get; }

        //private PumpProductConfig _productConfig;
        //[Association(ThisKey = nameof(idProductConfig))]
        //public PumpProductConfig productConfig
        //{
        //    set
        //    {
        //        _productConfig = value;
        //        if (_productConfig != null)
        //            idProductConfig = _productConfig.id;
        //    }
        //    get { return _productConfig; }
        //}

        [Column]
        public int idDevice { set; get; }

        private Device _device;
        [Association(ThisKey = nameof(idDevice))]
        public Device device
        {
            set
            {
                _device = value;
                if (_device != null)
                    idDevice = _device.id;
            }
            get { return _device; }
        }

        [Column]
        public int idBay { set; get; }

        private Bay _bay;

        [Association(ThisKey = nameof(idBay))]
        public Bay bay
        {
            set
            {
                _bay = value;
                if (_bay != null)
                    idBay = _bay.id;
                else
                    idBay = 0;
            }
            get { return _bay; }
        }

        public void readOnlineValuesOn(ModbusDataPackage read)
        {
            this.current = read.getUShort(tags.getRegisterBy("INV_CORRENTE"));
            this.frequency = read.getUShort(tags.getRegisterBy("INV_FREQ"));
            this.tension = read.getUShort(tags.getRegisterBy("INV_TENSAO"));
            this.tensionEngine = read.getUShort(tags.getRegisterBy("INV_TENSAO_MOTOR"));
            this.rpm = read.getUShort(tags.getRegisterBy("INV_RMP"));
            this.kWh = read.getUShort(tags.getRegisterBy("INV_kWh"));
            this.statusMsg = read.getUShort(tags.getRegisterBy("INV_STATUS_MESAGE"));


            //ushort alarms = read.getUShort(tags.getRegisterBy(nameof(alarms)));
            //this.permDensityReadError = alarms.bitValueOf(0);
            //this.inverterFailure = alarms.bitValueOf(1);
            //this.permLevelKeyReadError = alarms.bitValueOf(2);
            //this.permDeaeratorLevel = alarms.bitValueOf(5);
            //this.permEmergencyBtn = alarms.bitValueOf(6).invert();

            //this.biTrain = read.getUShort(tags.getRegisterBy(nameof(biTrain))).bitValueOf(1);

            // TODO: transform warnings in alarms
        }
    }
}
