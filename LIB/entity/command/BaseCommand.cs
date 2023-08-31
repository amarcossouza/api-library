using DbExtensions;
using PHD_TAS_LIB.entity.tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.command
{
    public abstract class BaseCommand : BaseEntity
    {   
        //TODO: Change to tag_name
        [Column]
        public string name { set; get; }

        // TODO: keep only bytes on database?
        // TODO: Using a generic class for defined type of value
        [Column]
        public int value { set; get; }

        [Column(ConvertTo = typeof(sbyte))]
        public bool atualize { set; get; } = false;

        // TODO: one command can be associated with many tags
        // TODO: one command can be composed by many command - like a chain of resposability pattern
        // TODO: Create a commun interface, well defined, contract for commands
        // TODO: Add Enable or disable field
        [Column]
        public int idTag { set; get; }

        private Tag _tag;
        [Association(ThisKey = nameof(idTag))]
        public Tag tag
        {
            set
            {
                _tag = value;
                if (_tag != null)
                    idTag = _tag.id;
            }
            get { return _tag; }
        }

        [Column]
        public int idDevice { set; get; }

        private Device _device;
        [Association(ThisKey = nameof(idDevice))]
        public Device device
        {
            set
            {
                _device = value;
                if (_device != null)
                    idDevice = _device.id;
            }
            get { return _device; }
        }

        public override string ToString()
        {
            return $"{name}[{value}] device:{idDevice}";
        }
    }
}
