using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.entity.transaction
{
    [Table(Name = "cliente")]
    public class Cliente : BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(2)]
        [Column]
        public string empresa { set; get; }
               
        //  [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int code { set; get; }

       

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(11)]
        [Column]
        public string cnpj { set; get; }

        //  [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public DateTime date {  set; get; }

        [System.ComponentModel.DataAnnotations.MaxLength(128)]
        [Column]
        public string notas { set; get; }

       


        public int? generateCode()
        {
            if (code > 0)
                return code;

            int hash = 17;
            hash = hash * 23 + empresa.GetHashCode();
            if (cnpj != "")
            {
                hash = hash * 23 + cnpj.GetHashCode();
                code = hash % 100000000;  // 8 zeros
                if (code < 0) code *= -1;
            }
            return code;
        }

        public override string ToString()
        {
            return  $@"{id};
                  {empresa};
                     {cnpj};
                     {code};
                {lastEntry};
                     {date};
                    {notas};";  
        }





        //[System.ComponentModel.DataAnnotations.Required]
        //[Column]
        //public int data_cad { set; get; }




        //[Column(ConvertTo = typeof(sbyte))]
        //public bool online { set; get; } = false;

        //[System.ComponentModel.DataAnnotations.Required]
        //[Column]
        //public int idProduct { set; get; }

        //[Association(ThisKey = nameof(idProduct))]
        //public ProductIndex productIndex { set; get; }

        //private Product _product;
        //[Association(ThisKey = nameof(idProduct))]
        //public Product product
        //{
        //    set
        //    {
        //        _product = value;
        //        idProduct = _product.id;
        //    }
        //    get { return _product; }
        //}
        // TODO: inject volume range, volume min and adicional unload plus(1000) config
        //public int volumeRemaining => volume - volumeLoaded;
        //public int volumeTransaction => volumeRemaining + 1000;
        //public bool isReachedVolume => volumeLoaded > volume + 1000;

        //public bool isVolumeCompleted => volumeLoaded - 10 >= volume || volumeLoaded + 10 >= volume;

        //public int percentLoad
        //{
        //    get
        //    {
        //        if (volumeLoaded > volume)
        //            return 100;
        //        if (volume <= 0)
        //            return 0;
        //        return Convert.ToInt32(((double)volumeLoaded / (double)volume) * 100.0);
        //    }
        //}

        //public bool isStart()
        //{
        //    if (volumeLoaded > 10 && volume > 0)
        //        return true;
        //    return false;
        //}
    }
}
