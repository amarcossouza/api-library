using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.tag
{
    public class TagField
    {
        public TagField(Tag tag, int? bit = null, int size = 1)
        {
            this.tag = tag;
            this.size = size;
            this.bit = bit;
        }

        public Tag tag { protected set; get; }

        public int size { protected set; get; }

        public int? bit { protected set; get; }

        //TODO: keep this class generic relative to value
        public int value { protected set; get; }
    }
}
