using DbExtensions;
using PHD_TAS_LIB.entity.online;
using PHD_TAS_LIB.entity.report;
using PHD_TAS_LIB.entity.user;
using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.tank
{
    [Table(Name = "tank_operation")]
    public class TankOperation : BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public string name { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        [Column]
        public int level { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        [Column]
        public int volume { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        [Column]
        public int volume20 { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        [Column]
        public int expectedLevel { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        [Column]
        public int expectedVolume { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        [Column]
        public int expectedVolume20 { set; get; }

        [Column]
        public DateTime dateBegin { set; get; } = DateTime.Now;

        [Column]
        public DateTime? dateEnd { set; get; }

        [Column(ConvertTo = typeof(string))]
        public TankFlowDirection flow { set; get; } = TankFlowDirection.NONE;

        [Column]
        public int idOpenMeasurement { set; get; }
        public TankMeasurement _openMeasurement { set; get; }
        [Association(ThisKey = nameof(idOpenMeasurement))]
        public TankMeasurement openMeasurement
        {
            set
            {
                _openMeasurement = value;
                idOpenMeasurement = _openMeasurement.id;
            }
            get { return _openMeasurement; }
        }

        [Column]
        public int? idEndMeasurement { set; get; }
        public TankMeasurement _endMeasurement { set; get; }
        [Association(ThisKey = nameof(idEndMeasurement))]
        public TankMeasurement endMeasurement
        {
            set
            {
                _endMeasurement = value;
                if (_endMeasurement != null)
                    idEndMeasurement = _endMeasurement.id;
                else
                    idEndMeasurement = null;
            }
            get { return _endMeasurement; }
        }

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
        public int idUser { set; get; }
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
            }
            get { return _user; }
        }

        public bool confirm { set; get; }

        public int levelCompletePercent(int levelNow)
        {
            if (flow == TankFlowDirection.IN)
            {
                if (levelNow > expectedLevel)
                    return 100;
                int value = level - Math.Abs(levelNow - expectedLevel);
                return (int)(((double)value / (double)level) * 100.0);
            }else {
                if (levelNow < expectedLevel)
                    return 100;
                int value = level - Math.Abs(expectedLevel - levelNow);
                return (int)(((double)value / (double)level) * 100.0);
            }
        }
    }
}
