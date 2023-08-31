using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExtensions;
using LIB.util;

namespace LIB.entity.transaction
{
    [Table(Name = "relatorio")]
    public class Relatorio : BaseEntity
    {
        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool isFabricacao { set; get; }

        //[System.ComponentModel.DataAnnotations.Required]
        //[Column]
        //public bool isRevisao { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool corpoLivreAmassosFissuras { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool corpoLivreTorcoesDobramentos { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool capaLivreCortesAbrasoes { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool aramesContinuosEspacoUniforme { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool aramesLivresCorrosaoDanif { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool terminaisLivresCorrosao { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool isPressaor07Bar2MinutosOk { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool isPressaor17Bar10MinutosOk { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int resisEletAramesIntExt { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int comprimentoFacesTerminaisL0 { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int comprimentoFacesTerminaisL1 { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool isComprimentoFacesTerminaisL1ok { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int torcaoConectoresTerminais { set; get; }

        [Column]
        public bool isDiametroInternoTerminaisOk { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public decimal diametroInternoTerminais { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int resistEletConectoresTerminais { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int resistEletAramesDrenao { set; get; }

        //[System.ComponentModel.DataAnnotations.Required]
        //[Column]
        //public int percentAlongamentoTemp { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool aprovado { set; get; }

        [Column]
        public DateTime date { set; get; } = DateTime.Now;

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
        public int idCliente { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int idMangueira { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int idUser { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool isResisEletAramesIntExtOK { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool isComprimentoFacesTerminaisL0ok { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool isTorcaoConectoresTerminaisOK { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool isResistEletConectoresTerminaisOK { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool isResistEletAramesDrenaoOK { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool isPercentAlongamentoTempOK { set; get; }

        [Column]
        public string instrumentos { set; get; }

        public List<ReportInstruments> reportInstruments { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int reportType { set; get; }

        [Column]
        public string notas { set; get; }

      

        public override string ToString()
        {
            string getUserData = "";
            getUserData = isFabricacao + ";";
            getUserData += corpoLivreAmassosFissuras + ";";
            getUserData += corpoLivreTorcoesDobramentos + ";";
            getUserData += capaLivreCortesAbrasoes + ";";
            getUserData += aramesContinuosEspacoUniforme + ";";
            getUserData += aramesLivresCorrosaoDanif + ";";
            getUserData += terminaisLivresCorrosao + ";";
            getUserData += isPressaor07Bar2MinutosOk + ";";
            getUserData += isPressaor17Bar10MinutosOk + ";";
            getUserData += resisEletAramesIntExt + ";";
            getUserData += isResisEletAramesIntExtOK + ";";
            getUserData += comprimentoFacesTerminaisL0 + ";";
            getUserData += isComprimentoFacesTerminaisL0ok + ";";
            getUserData += comprimentoFacesTerminaisL1 + ";";
            getUserData += isComprimentoFacesTerminaisL1ok + ";";
            getUserData += torcaoConectoresTerminais + ";";
            getUserData += isTorcaoConectoresTerminaisOK + ";";
            getUserData += resistEletConectoresTerminais + ";";
            getUserData += isResistEletConectoresTerminaisOK + ";";
            getUserData += resistEletAramesDrenao + ";";
            getUserData += isResistEletAramesDrenaoOK + ";";
            getUserData += isPercentAlongamentoTempOK + ";";
            getUserData += instrumentos + ";";
            getUserData += aprovado + ";";
            getUserData += date + ";";
            getUserData += revisao + ";";
            getUserData += seqCliente + ";";
            getUserData += idUser + ";";
            getUserData += idMangueira + ";";
            getUserData += diametroInternoTerminais + ";";
            getUserData += isDiametroInternoTerminaisOk + ";";
            getUserData += reportType + ";";
            getUserData += notas + ";";           
            return getUserData;

        }
    }
}
