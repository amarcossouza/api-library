using DbExtensions;
using System.Collections.ObjectModel;

namespace PHD_TAS_LIB.entity
{
    [Table]
    public class Plataform : BaseEntity
    {
        [Column]
        public string name { set; get; }

        //TODO: Define type of plataform
        //[Column]
        //public int number { set; get; }

        [Association(OtherKey = nameof(Bay.idPlataform))]
        public Collection<Bay> bays { protected set; get; }
    }
}
