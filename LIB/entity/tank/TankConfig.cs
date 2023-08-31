using DbExtensions;
using PHD_TAS_LIB.entity.online;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity
{
    //TODO: Rename to TankScheduler ?
    [Table(Name = "tank_config")]
    public class TankConfig : BaseEntity
    {
        [Column]
        public TimeSpan openTime { set; get; } = TimeSpan.Parse("06:00");
        [Column]
        public TimeSpan closeTime { set; get; } = TimeSpan.Parse("23:00");

        // TODO: this fields are online state values - check the tankConfig resposability
        [Column(ConvertTo = typeof(sbyte))]
        public bool isOpenChecked { protected set; get; } = false;
        [Column(ConvertTo = typeof(sbyte))]
        public bool isClosedChecked { protected set; get; } = false;

        [Column]
        public int limitTimeForMeasurement { set; get; } = 20;

        [Column]
        public DateTime checkDay { protected set; get; } = DateTime.MinValue;

        //TODO: check this name
        [Column(ConvertTo = typeof(int))]
        public TankOpenCloseInput openCloseInput { set; get; } = TankOpenCloseInput.AUTOMATIC;

        [Column]
        public char ciuModel { set; get; } = 'B'; // (char)0x42;

        //[Column(ConvertTo = typeof(sbyte))]
        //public bool enableCheck { set; get; } = false;

        // configuração de fechamento:
        // por tanque
        // medição deve ser manual, automatica, ou ambas
        // a medição (automatica) será feita independente da configuração de fechamento
        // para o caso de ambas: TODO
        // Deve ser possivel deixar um tank com leitura automatica com medição de fechamento manual? 
        //  desabilitar a medicao automatica de abertura/fechamento?

        // add outras configurações
        // gaugeIndex
        // arquivo da tabela de interpolação
        // produto

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

        public bool checkOpen(DateTime now, bool byPassLimit = false)
        {
            if (!isOpenChecked || checkDay.Date != now.Date)
            {
                var diff = now.TimeOfDay.Subtract(openTime);
                if (diff.TotalMinutes > 0 && (byPassLimit || diff.TotalMinutes < limitTimeForMeasurement))
                {
                    checkDay = now;
                    isOpenChecked = true;
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool checkClose(DateTime now, bool byPassLimit = false)
        {
            if (!isClosedChecked || checkDay.Date != now.Date)
            {
                var diff = now.TimeOfDay.Subtract(closeTime);
                if (diff.TotalMinutes > 0 && (byPassLimit || diff.TotalMinutes < limitTimeForMeasurement))
                {
                    checkDay = now;
                    isClosedChecked = true;
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool isClosedCheckedOnTime()
        {
            DateTime now = DateTime.Now;
            if (!isClosedChecked || checkDay.Date != now.Date)
            {
                var diff = DateTime.Now.TimeOfDay.Subtract(closeTime);
                if (diff.TotalMinutes > 4)
                    return false;
                return true;
            }
            return true;
        }
        public bool isOpenCheckedOnTime()
        {
            DateTime now = DateTime.Now;
            if (!isOpenChecked || checkDay.Date != now.Date)
            {
                var diff = DateTime.Now.TimeOfDay.Subtract(openTime);
                if (diff.TotalMinutes > 4)
                    return false;
                return true;
            }
            return true;
        }

        public void resetCheck()
        {
            checkDay = DateTime.MinValue;
            isClosedChecked = false;
            isOpenChecked = false;
        }
    }

    public enum TankOpenCloseInput
    {
        MANUAL = 1,
        AUTOMATIC = 2,
        BOTH = 3
    }
}
