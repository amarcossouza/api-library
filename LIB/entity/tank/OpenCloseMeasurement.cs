using PHD_TAS_LIB.entity.report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.tank
{
    public struct OpenCloseMeasurement
    {
        public TankMeasurement open { get; }
        public TankMeasurement close { get; }

        public OpenCloseMeasurement(TankMeasurement open, TankMeasurement close, bool forceCloseOnEnd = true)
        {
            if (open == null && !open.type.isOpenMeasurement())
                throw new ArgumentException("OPEN OpenCloseMeasurement type deve ser de ABERTURA");

            if (close == null && (!close.type.isCloseMeasurement() || forceCloseOnEnd))
                throw new ArgumentException("CLOSE OpenCloseMeasurement type deve ser de FECHAMENTO");

            // TODO: When open and close are in diferente days

            this.open = open;
            this.close = close;
        }

        public int levelDifference()
        {
            return close.level - open.level;
        }

        public int volumeDifference()
        {

            return close.volume - open.volume;
        }

        public int volume20Difference()
        {
            return close.volume20 - open.volume20;
        }

        public bool isPositive() {
            return levelDifference() >= 0;
        }
    }
}
