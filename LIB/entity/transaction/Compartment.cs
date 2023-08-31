using DbExtensions;
using PHD_TAS_LIB.entity.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.transaction
{
    [Table]
    public class Compartment : BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public string orderNumber { set; get; }

        [System.ComponentModel.DataAnnotations.Range(1, 10)]
        [Column]
        public int number { set; get; }

        [System.ComponentModel.DataAnnotations.Range(100, 999999)]
        [Column]
        public int volume { set; get; }

        [Column]
        public int volumeLoaded { set; get; } = 0;

        // TODO: Keep C1 and C2 in an Collection
        [Column]
        public int volumeLoadedC1 { set; get; } = 0;

        [Column]
        public int volumeLoadedC2 { set; get; } = 0;

        [Column]
        public int complement { set; get; } = 0;

        [Column(ConvertTo = typeof(sbyte))]
        public bool online { set; get; } = false;

        [Column]
        public int idProduct { set; get; }

        private Product _product;
        [Association(ThisKey = nameof(idProduct))]
        public Product product
        {
            set
            {
                _product = value;
                idProduct = _product.id;
            }
            get { return _product; }
        }

        [Association(ThisKey = nameof(idProduct))]
        public ProductIndex productIndex { set; get; }

        [Column]
        public int idTransaction { set; get; }

        private LoadTransaction _transaction;
        [Association(ThisKey = nameof(idTransaction))]
        public LoadTransaction transaction
        {
            set
            {
                _transaction = value;
                idTransaction = _transaction.id;
            }
            get { return _transaction; }
        }

        public int volumeComplemented => volume + complement;

        // TODO: Inject Range min-max config
        public bool isVolumeCompletedLoad => volumeLoaded - 5 >= volumeComplemented  || volumeLoaded + 5 >= volumeComplemented;

        // TODO: Inject Loaded min value(10)
        public bool hasBegun => volumeLoaded > 10 && volume > 0;

        public bool hasComplement => complement > 0;

        public int volumeRemaining => (volume + complement) - volumeLoaded;

        public int percentLoad
        {
            get
            {
                if (volumeLoaded > volumeComplemented)
                    return 100;
                if (volumeComplemented <= 0)
                    return 0;
                return Convert.ToInt32(((double)volumeLoaded / (double)volumeComplemented) * 100.0);
            }
        }

        public string status
        {
            get
            {
                if (hasComplement)
                    return $"Complemento({complement})";
                if (isVolumeCompletedLoad)
                    return "Completo";
                return $"{volumeLoaded} / {volume}";

            }
        }

        public override bool Equals(object obj)
        {
            var compartment = obj as Compartment;
            return compartment != null &&
                   base.Equals(obj) &&
                   number == compartment.number;
        }

        public override int GetHashCode()
        {
            var hashCode = 467038368;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + number.GetHashCode();
            return hashCode;
        }

        public void AddComplement(int volumeToComplement)
        {
            if (!online && isVolumeCompletedLoad && volumeToComplement + complement >= 10 && volumeToComplement + complement <= 30)
            {
                complement += volumeToComplement;
            }
        }
    }
}
