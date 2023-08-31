using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.report
{
    [Table(Name = "alarm_report")]
    public class AlarmReport : BaseEntity
    {
        [Column]
        public int? idBay { set; get; }

        private Bay _bay;
        [Association(ThisKey = nameof(idBay))]
        public Bay bay
        {
            set
            {
                _bay = value;
                if (_bay != null)
                    idBay = _bay.id;
            }
            get { return _bay; }
        }

        [Column]
        public string name { set; get; }
        [Column]
        public string description { set; get; }
        [Column]
        public DateTime datetime { set; get; } = DateTime.Now;

        [Column(ConvertTo = typeof(int))]
        public AlarmLevel level { set; get; } = AlarmLevel.DANGER;

        public override string ToString()
        {
            if (bay != null)
                return $"{name} LOCAL: {bay.name} - {datetime.ToString("dd/MM/yy HH:mm")}";
            return $"{name} - {datetime.ToString("dd/MM/yy HH:mm")}";
        }

        //TODO: REMOVE
        public string levelCss()
        {
            switch (level)
            {
                case AlarmLevel.DANGER:
                    return "danger";
                case AlarmLevel.WARN:
                    return "warning";
                case AlarmLevel.INFO:
                    return "info";
                default: return "default";
            }
        }

        //TODO: REMOVE
        public string levelString()
        {
            switch (level)
            {
                case AlarmLevel.DANGER:
                    return "Alerta";
                case AlarmLevel.WARN:
                    return "Aviso";
                case AlarmLevel.INFO:
                    return "Info";
                default: return "Aviso";
            }
        }
    }

    // TODO: Config per AlarmLevel
    public enum AlarmLevel
    {
        DANGER,
        WARN,
        INFO
    }
}
