using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.tag
{   
    [Table]
    public class Tag : BaseEntity
    {
        [Column]
        public string name { set; get; }

        [Column]
        public int register { set; get; }
        [Column]
        public int sequencial { set; get; } = 1;

        [Column]
        public DeviceType typeDevice { set; get; }

        [Column(ConvertTo = typeof(string))]
        public TagTypeGroup typeGroup { set; get; }

        [Column]
        public string description { set; get; }

        // TODO: abstract all memory register here
        // TODO: add other infos, like: description, lenght, type, local, device....

        //
        //[Column]
        //public int idDevice { set; get; }

        //private Device _device;
        //[Association(ThisKey = nameof(idDevice))]
        //public Device device
        //{
        //    set
        //    {
        //        _device = value;
        //        if (_device != null)
        //            idDevice = _device.id;
        //    }
        //    get { return _device; }
        //}
    }

    public enum TagTypeGroup {

        COMMAND,
        ONLINE_VALUE,
        CONFIG,
        PRESET,
        ONLINE_CONFIG,
        DISABLE
    }

    
}
