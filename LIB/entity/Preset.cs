using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity
{
    [Table]
    public class Preset : BaseEntity
    {
        [Column]
        public string name { set; get; }

        [Column]
        public int indexDevice { set; get; }

        [Column]
        public int number { set; get; }

        // TODO: add type of preset(load,unload...) for dont make unnecessary joins

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

        public override string ToString()
        {
            if (bay != null)
                return name + " - " + bay.name;
            return name;
        }
    }
}
