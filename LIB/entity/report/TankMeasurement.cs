using DbExtensions;
using PHD_TAS_LIB.entity.online;
using PHD_TAS_LIB.entity.tank;
using PHD_TAS_LIB.entity.user;
using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.report
{
    [Table(Name = "tank_measurement")]
    public class TankMeasurement : BaseEntity
    {
        public TankMeasurement() { }

        public TankMeasurement(TankOnline tank)
        {
            this.tankOnline = tank;
            this.level = tank.level;
            this.temperature = tank.temperature;
        }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
        [Column]
        public int level { set; get; } = 1;
        [Column]
        public int beforeLevel { set; get; }

        [Column]
        public int temperature { set; get; } = 2000;

        public double tempDouble()
        {
            return temperature / 100.0;
        }

        [Column]
        public int volume { set; get; }
        [Column]
        public int volume20 { set; get; }

        [Column(ConvertTo = typeof(string))]
        public TankFlowDirection flowDirection { set; get; }

        [Column(ConvertTo = typeof(string))]
        public MeasurementType type { set; get; } = MeasurementType.MANUAL;

        //TODO admin users cam change date and time of measurement
        [Column]
        public DateTime date { set; get; } = DateTime.Now;

        public bool confirm { set; get; } = false;

        [Column]
        public int idTank { set; get; }
        private TankOnline _tankOnline;
        [Association(ThisKey = nameof(idTank))]
        public TankOnline tankOnline
        {
            set
            {
                _tankOnline = value;
                idTank = _tankOnline.id;
            }
            get { return _tankOnline; }
        }

        [Column]
        public int? idUser { set; get; }
        private User _user;
        [Association(ThisKey = nameof(idUser))]
        public User user
        {
            set
            {
                _user = value;
                if (_user != null)
                {
                    idUser = _user.id;
                }
                else
                {
                    _user = null;
                    idUser = null;
                }
            }
            get { return _user; }
        }

        [Association(ThisKey = nameof(idTank), OtherKey = nameof(TankConfig.idTank))]
        public TankConfig config { set; get; }

        public void GenerateVolume20(bool isHydrocarbon)
        {
            if (isHydrocarbon)
            {
                volume20 = (int)VolumeConversion.getVolume20ForHydrocarbon(this.volume, this.tempDouble(), tankOnline.densityDouble(1000.0));
            }
            else
            {
                volume20 = (int)VolumeConversion.getVolume20ForNonHydrocarbon(this.volume, this.tempDouble());
            }

        }
    }

    // TODO: separete manual and AUTOMATIC for open close - should be 2 diferent types 
    public enum MeasurementType
    {
        MANUAL = 1,
        AUTOMATIC = 2,
        OPEN_SCHEDULED = 3,
        CLOSE_SCHEDULED = 4,
        OPEN_MANUAL = 5,
        CLOSE_MANUAL = 6,
        //OPEN_OPERATION = 7,
        //CLOSE_OPERATION = 8
    }

    public static class MeasurementTypeExtensions
    {
        public static string StringValue(this MeasurementType type)
        {
            switch (type)
            {
                case MeasurementType.MANUAL:
                    return "Manual";
                case MeasurementType.AUTOMATIC:
                    return "Automático";
                case MeasurementType.OPEN_SCHEDULED:
                    return "Abertura (Auto)";
                case MeasurementType.CLOSE_SCHEDULED:
                    return "Fechamento (Auto)";
                case MeasurementType.OPEN_MANUAL:
                    return "Abertura (Manual)";
                case MeasurementType.CLOSE_MANUAL:
                    return "Fechamento (Manual)";
                default: return "-";
            }
        }

        public static bool isOpenCloseMeasurement(this MeasurementType type)
        {
            return (int)type >= 3 || (int)type >= 6;
        }

        public static bool isOpenMeasurement(this MeasurementType type)
        {
            return type == MeasurementType.OPEN_MANUAL || type == MeasurementType.OPEN_SCHEDULED;
        }

        public static bool isCloseMeasurement(this MeasurementType type)
        {
            return type == MeasurementType.CLOSE_MANUAL || type == MeasurementType.CLOSE_SCHEDULED;
        }

        public static bool isManual(this MeasurementType type)
        {
            return type == MeasurementType.MANUAL || type == MeasurementType.OPEN_MANUAL || type == MeasurementType.CLOSE_MANUAL;
        }

        public static bool isAutomatic(this MeasurementType type)
        {
            return type == MeasurementType.AUTOMATIC || type == MeasurementType.OPEN_SCHEDULED || type == MeasurementType.CLOSE_SCHEDULED;
        }
    }
}
