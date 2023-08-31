using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.entity.transaction
{   
    // TODO: Keep this class for futere Transaction details and metadata. For now keep simple putting number in compartment and UnloadTransaction
    public class Order : BaseEntity
    {
        [Column]
        public string number { set; get; }

        // TODO: Aggregate other metadata here, like: client, company, driver, ... 
    }
}
