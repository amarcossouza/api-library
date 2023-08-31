using DbExtensions;
using PHD_TAS_LIB.entity.tag;
using PHD_TAS_LIB.entity.tank;
using PHD_TAS_LIB.util;
using System;

namespace PHD_TAS_LIB.entity.online
{
    [Table(Name = "tank_online")]
    public class TankOnline : BaseEntity
    {
        [Column]
        public string name { set; get; }
        [Column]
        public int number { set; get; }

        [Column]
        public int capacity { set; get; }

        public int capacityPercent()
        {
            return (int)((level / (double)capacity) * 100);
        }

        [Column]
        public int capacityVolume { set; get; }

        [Column]
        public int gaugeIndex { set; get; }
        [Column]
        public int level { set; get; }
        [Column]
        public int temperature { set; get; }

        public double tempDouble()
        {
            return temperature / 100.0;
        }

        [Column]
        public int volume { set; get; }

        [Column]
        public int volume20 { set; get; }

        [Column]
        public int flowRate { set; get; }

        [Column(ConvertTo = typeof(string))]
        public TankFlowDirection flowDirection { set; get; }

        [Column]
        public string alarmStatus { set; get; }
        public bool isAlarmStatusInWarn() => !string.IsNullOrWhiteSpace(alarmStatus) && alarmStatus != "OK";

        [Column]
        public string levelStatus { set; get; }

        public bool isCapacityWarning()
        {
            return this.level >= capacity - 100;
        }

        public bool isLevelStatusInWarn() => !string.IsNullOrWhiteSpace(levelStatus) && levelStatus != "OK";

        [Column]
        public string temperatureStatus { set; get; }
        public bool isTempStatusInWarn() => !string.IsNullOrWhiteSpace(temperatureStatus) && temperatureStatus != "OK";

        [Column(ConvertTo = typeof(int))]
        public TemperatureSign temperatureSign { set; get; }

        //TODO: use product reference or only string code
        [Column]
        public string product { set; get; }
        [Column(ConvertTo = typeof(sbyte))]
        public bool isHydrocarbon { set; get; }

        [Column]
        public int density { set; get; }
        public double densityDouble(double factor = 100.0)
        {
            return density / factor;
        }

        [Column]
        public int? idDevice { set; get; }
        private Device _device;
        [Association(ThisKey = nameof(idDevice))]
        public Device device
        {
            set
            {
                _device = value;
                if (_device != null)
                    idDevice = _device.id;
                else
                    idDevice = null;
            }
            get { return _device; }
        }

        public bool isAutomatic()
        {
            return idDevice.HasValue && idDevice > 0;
        }

        [Association(ThisKey = nameof(id), OtherKey = nameof(TankConfig.idTank))]
        public TankConfig config { set; get; }

        public void GenerateVolume20(bool isHydrocarbon)
        {
            if (isHydrocarbon)
            {
                volume20 = (int)VolumeConversion.getVolume20ForHydrocarbon(this.volume, this.tempDouble(), this.densityDouble(1000.0));
            }
            else
            {
                volume20 = (int)VolumeConversion.getVolume20ForNonHydrocarbon(this.volume, this.tempDouble());
            }
        }

        public override string ToString()
        {
            return $"{name} {number}[{gaugeIndex}]";
        }

        //TODO: use a serie for more approximated flowRate value
        private int lastVolume = -1;
        private DateTime lastMinute = DateTime.MinValue;

        public void GenerateFlowRate(DateTime now)
        {
            if (lastVolume == -1 || lastMinute == DateTime.MinValue)
            {
                lastMinute = now;
                lastVolume = volume;
                flowRate = 0;
                flowDirection = TankFlowDirection.NONE;
                return;
            }
            var timeDiff = now.Subtract(lastMinute);
            decimal volDif = 0;
            // TODO: inject seconds span config // 20
            if (timeDiff.Seconds >= 15)
            {
                volDif = volume - lastVolume;
                if (volDif < 0)
                {
                    flowDirection = TankFlowDirection.OUT;
                    volDif *= -1;
                }
                else
                {
                    flowDirection = TankFlowDirection.IN;
                }
                //TODO: remove timeDifSeconds
                decimal timeDifSeconds = Convert.ToDecimal(timeDiff.Seconds);
                double vazaoLs = decimal.ToDouble(volDif) / (timeDifSeconds > 0 ? (double)timeDifSeconds : 1.0);
                if (vazaoLs == 0)
                {
                    flowDirection = TankFlowDirection.NONE;
                }
                flowRate = Convert.ToInt32(vazaoLs);
                lastMinute = now;
                lastVolume = volume;
            }
        }

        public TagCollection tags { private set; get; }

        public void setTagCollection(Tag[] tags)
        {
            if (tags == null || tags.Length == 0)
                throw new ArgumentException();

            if (this.tags == null || this.tags.Count > 0)
                this.tags = new TagCollection(tags);
        }

        // exibir se abertura ou fechamento foi realizado no horario esperado
        // carregar a medicao do dia em questao - se for null não houve abertura/fechamento

        [Column]
        public int? idOperation { set; get; }
        private TankOperation _operation;
        [Association(ThisKey = nameof(idOperation))]
        public TankOperation operation
        {
            set
            {
                _operation = value;
                if (_operation != null)
                    idOperation = _operation.id;
                else
                    idOperation = null;
            }
            get { return _operation; }
        }

        public bool hasOperation()
        {
            return idOperation.HasValue;
        }
    }

    public enum TankFlowDirection
    {
        NONE = 0,
        IN = 1,
        OUT = 2,
    }

    public static class TankEnumExtensions
    {
        public static string ToStringValue(this TankFlowDirection flow)
        {
            if (flow == TankFlowDirection.IN)
                return "Entrada";
            if (flow == TankFlowDirection.OUT)
                return "Saída";
            return "Neutro";
        }
    }

    public enum TemperatureSign
    {
        POSITIVE = '+',
        NEGATIVE = '-',
        INVALID = 'F'
    }
}
    