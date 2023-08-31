using DbExtensions;
using PHD_TAS_LIB.entity.report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.online
{
    [Table(Name = "alarm_online")]
    public class AlarmOnline : BaseEntity
    {
        [Column]
        public int? idDevice { set; get; }

        // TODO: idBay should be local, generic id or name for local, like: preset, bay, tank, platform...
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
                else
                    idBay = null;
            }
            get { return _bay; }
        }

        [Column]
        public string name { set; get; }
        [Column]
        public string description { set; get; }

        [Column(ConvertTo = typeof(sbyte))]
        public bool active { set; get; } = false;
        [Column]
        public DateTime startTime { set; get; } = DateTime.Now;

        [Column]
        public int indexHash { protected set; get; }

        [Column(ConvertTo = typeof(sbyte))]
        public bool canClosed { set; get; } = false;
        [Column]
        public DateTime? closedTime { set; get; }

        [Column(ConvertTo = typeof(int))]
        public AlarmLevel level { set; get; } = AlarmLevel.DANGER;

        public AlarmReport getAlarmForReport()
        {
            return new AlarmReport()
            {
                name = this.name,
                idBay = this.idBay,
                bay = this.bay,
                description = this.description,
                datetime = this.startTime,
                level = this.level
            };
        }

        public void createIndexHash(params int[] index)
        {
            indexHash = 17;
            foreach (var i in index)
            {
                indexHash = indexHash * (23 + i);
            }
            indexHash = indexHash * (idBay.HasValue ? idBay.Value : 1);
            //TODO: Remove name.GetHashCode() keep responsability in outside call 
            indexHash += name.GetHashCode();
        }

        public override string ToString()
        {
            return $"{name} - {indexHash}";
        }

        // TODO: add alarm config
        // Closed timeout
        // sound alarm
        // comfirm msg
        // notification - group, email, sms
        // normal aberto-fechado
        // Value - the value that cause the alarm
    }
}
