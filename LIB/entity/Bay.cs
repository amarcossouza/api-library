using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity
{
    // TODO: Change to AREA
    [Table]
    public class Bay : BaseEntity
    {
        //TODO: Define type of Bay or area

        [Column]
        public string name { set; get; }

        [Column]
        public int number { set; get; }

        [Column]
        public int? idPlataform { set; get; }

        private Plataform _plataform;
        [Association(ThisKey = nameof(idPlataform))]
        public Plataform plataform
        {
            set
            {
                _plataform = value;
                if (_plataform != null)
                    idPlataform = _plataform.id;
            }
            get { return _plataform; }
        }

        public List<Device> devices { set; get; }

        public void addDevice(Device d) {
            if (devices == null)
                devices = new List<Device>();
            devices.Add(d);
        }

        public override string ToString()
        {
            return $"{name} ({number})";
        }

        public void clearDevices()
        {
            if (devices != null)
                devices.Clear();
        }
    }
}
