using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExtensions;


namespace LIB.entity.transaction
{
    [Table(Name = "relatoriomanager")]
    public class RelatorioManager:BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int revisao { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int seqCliente { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int seqTsPro { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int idRelatorio { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int idCliente { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int idMangueira { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int idUser { set; get; }

    }
}
