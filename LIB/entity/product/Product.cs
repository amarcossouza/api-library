using DbExtensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.product
{
    [Table]
    public class Product : BaseEntity
    {
        [Column]
        public string code { set; get; }
        [Column]
        public string name { set; get; }
        [Column]
        public string description { set; get; }

        //[Column]
        //public string color { set; get; }

        //[Column(ConvertTo = typeof(sbyte))]
        //public bool isHydrocarbon { set; get; }

        [Column(ConvertTo = typeof(sbyte))]
        public bool component { set; get; } = false;

        [Association(OtherKey = nameof(ProductIndex.idProduct))]
        public Collection<ProductIndex> indexCollection { protected set; get; }

        [Association(ThisKey = nameof(ProductIndex.idProduct))]
        public ProductIndex index { set; get; }

        public bool hasIndexInDevice(int indexProduct)
        {
            if (indexCollection != null)
                return indexCollection.Any(p => p.indexProduct == indexProduct);
            return false;
        }

        public int getIndexByDevice(int idDevice)
        {
            if (indexCollection != null)
                return indexCollection.FirstOrDefault(p => p.idDevice == idDevice).indexProduct;
            return 0;
        }
    }
}
