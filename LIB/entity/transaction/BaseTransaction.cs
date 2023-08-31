using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.entity.transaction
{
    public abstract class BaseTransaction : BaseEntity
    {   
        [Column]
        public int? code { set; get; }

        [System.ComponentModel.DataAnnotations.StringLength(7)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[a-zA-Z]{3}[0-9]{4}$")] // TODO: for include(-): \-\
        [Column]
        public string plate { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column] 
        public DateTime date { set; get; } = DateTime.Today;

        // TODO: Check if register when transaction was stated is usefull
        // TODO: Check if this is usefull
        [Column]
        public DateTime lastEntry { protected set; get; } = DateTime.Now;

        public int? generateCode()
        {
            if (code > 0)
                return code;

            int hash = 17;
            hash = hash * 23 + date.GetHashCode();
            if (plate != null)
            {
                hash = hash * 23 + plate.GetHashCode();
                code = hash % 100000000;  // 8 zeros
                if (code < 0) code *= -1;
            }
            return code;
        }

        public bool hashCode() => code.HasValue;

        public static SqlBuilder IncludeProductIndex(string table)
        {
            return SQL.SELECT("t.*")
                            ._("p.id AS productIndex$id, p.code AS productIndex$code, p.enable AS productIndex$enable")
                            ._("p.indexInPreset AS productIndex$_indexInPreset, p.indexProduct AS productIndex$indexProduct")
                            ._("p.idDevice AS productIndex$idDevice, p.idProduct AS productIndex$idProduct")
                            .FROM($"{table} t")
                            .LEFT_JOIN("product_index p ON p.idProduct = t.idProduct");
        }
    }
}
