using DbExtensions;
using Newtonsoft.Json;
using PHD_TAS_LIB.entity.command;
using PHD_TAS_LIB.entity.tag;
using PHD_TAS_LIB.modbus;
using PHD_TAS_LIB.util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.online
{
    [Table(Name = "unload_plc_online")]
    public class UnloadPLCOnline : BaseEntity
    {
        [Column]
        public int current { set; get; }
        [Column]
        public int frequency { set; get; }
        [Column]
        public int density { set; get; }
        [Column]
        public int density20 { set; get; }
        [Column]
        public int rpm { set; get; }
        [Column]
        public int tension { set; get; }

        [Column]
        public int deaeratorLevel { set; get; }

        public int deaeratorLevelPercent
        {
            get
            {
                if (deaeratorLevel > 1000)
                    return 100;
                if (deaeratorLevel < 0)
                    return 0;
                return (int)((deaeratorLevel / 1000.0) * 100);
            }
        }

        [Column]
        public bool permDensityReadError { set; get; }

        [Column]
        public bool permLevelKeyReadError { set; get; }

        [Column]
        public bool permDeaeratorLevel { set; get; }

        [Column]
        public bool permEmergencyBtn { set; get; }

        [Column]
        public bool inverterFailure { set; get; }

        [Column(ConvertTo = typeof(sbyte))]
        public bool biTrain { set; get; }

        [Column]
        public int sequential { set; get; }

        //public List<string> warnings { set; get; } = new List<string>();

        //[Column(Name = "warnings")]
        //protected string _warnings
        //{
        //    set
        //    {
        //        if (string.IsNullOrEmpty(value))
        //        {
        //            warnings = new List<string>();
        //            return;
        //        }
        //        warnings = JsonConvert.DeserializeObject<List<string>>(value);
        //    }
        //    get { return JsonConvert.SerializeObject(warnings); }
        //}

        //public bool hasWarnings() {
        //    return warnings.Count > 0;
        //}

        // valvula collection and pump
        public Dictionary<int, UnloadInstrumentOnline> instruments { protected set; get; } = new Dictionary<int, UnloadInstrumentOnline>();

        public void clearInstruments() => this.instruments.Clear();

        [Column(Name = "instruments")]
        protected string _instruments
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    instruments = new Dictionary<int, UnloadInstrumentOnline>();
                    return;
                }
                instruments = JsonConvert.DeserializeObject<Dictionary<int, UnloadInstrumentOnline>>(value);
            }
            get { return JsonConvert.SerializeObject(instruments); }
        }

        public UnloadInstrumentOnline getInstrumentBy(int number,UnloadInstrumentType type) {
            return instruments.Values.FirstOrDefault(i => i.number == number && i.type == type);
        }

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
        public int idSMPDevice { set; get; }

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

        public TagCollection tags { private set; get; }

        public void setTagCollection(Tag[] tags)
        {
            if (tags == null || tags.Length == 0)
                throw new ArgumentException();

            if (this.tags == null || this.tags.Count > 0)
                this.tags = new TagCollection(tags);
        }

        public void readOnlineValuesOn(ModbusDataPackage read)
        {
            this.current = read.getUShort(tags.getRegisterBy(nameof(current)));
            this.frequency = read.getUShort(tags.getRegisterBy(nameof(frequency)));
            Console.WriteLine(frequency);
            this.density = read.getUShort(tags.getRegisterBy(nameof(density)));
            this.density20 = read.getUShort(tags.getRegisterBy(nameof(density20)));
            this.rpm = read.getUShort(tags.getRegisterBy(nameof(rpm)));
            this.tension = read.getUShort(tags.getRegisterBy(nameof(tension)));
            this.deaeratorLevel = read.getUShort(tags.getRegisterBy(nameof(deaeratorLevel)));

            ushort alarms = read.getUShort(tags.getRegisterBy(nameof(alarms)));
            this.permDensityReadError = alarms.bitValueOf(0);
            this.inverterFailure = alarms.bitValueOf(1);
            this.permLevelKeyReadError = alarms.bitValueOf(2);
            this.permDeaeratorLevel = alarms.bitValueOf(5);
            this.permEmergencyBtn = alarms.bitValueOf(6).invert();

            this.biTrain = read.getUShort(tags.getRegisterBy(nameof(biTrain))).bitValueOf(1);
        }

        public void setInstumentBy(UnloadPLCCommand cmd)
        {
            // TODO: use register tag for find instrument or match number and type?
            if (instruments.ContainsKey(cmd.tag.register))
            {
                var instrument = instruments[cmd.tag.register];
                instrument.setValuesBy(cmd);
                instruments[cmd.tag.register] = instrument;
            }
            else
            {
                var instrument = new UnloadInstrumentOnline()
                {
                    type = cmd.type,
                    number = cmd.number,
                };
                instrument.setValuesBy(cmd);
                instruments.Add(cmd.tag.register, instrument);
            }
        }



        //public void addWarning(string warning, bool status)
        //{
        //    if (status == true)
        //    {
        //        warnings.Add(warning);
        //    }
        //}
    }
}
