using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExtensions;

namespace LIB.entity.transaction
{
    [Table(Name = "config")]
   public  class ConfigSiscoman : BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int ResisEletAramesIntExtMin { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int ResisEletAramesIntExtMax { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int ComprimentoFacesTerminaisL0Min { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int ComprimentoFacesTerminaisL0Max { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int TorcaoConectoresTerminaisMin { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int TorcaoConectoresTerminaisMax { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int ResistEletConectoresTerminaisMin { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int ResistEletConectoresTerminaisMax { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int ResistEletAramesDrenaoMin { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int ResistEletAramesDrenaoMax { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int PercentAlongamentoTempMin { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int PercentAlongamentoTempMax { set; get; }

        public override string ToString()
        {
            return $@"{ResisEletAramesIntExtMin};
                  {ResisEletAramesIntExtMax};
                     {ComprimentoFacesTerminaisL0Min};
                     {ComprimentoFacesTerminaisL0Max};
                {TorcaoConectoresTerminaisMin};
                     {TorcaoConectoresTerminaisMax};
                    {ResistEletConectoresTerminaisMin};
                    {ResistEletConectoresTerminaisMax};
                    {ResistEletAramesDrenaoMin};
                    {ResistEletAramesDrenaoMax};
                    {PercentAlongamentoTempMin};
                    {PercentAlongamentoTempMax};
                    {lastEntry};";
        }

    }
}
