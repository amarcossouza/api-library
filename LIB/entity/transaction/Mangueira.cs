using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbExtensions;



namespace LIB.entity.transaction
{
    [Table(Name = "mangueira")]
    public class Mangueira : BaseEntity
    {

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int idcliente { set; get; }

        [Column]
        public int code { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public string normaConstrutiva { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int tipoMangueira { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int materialBarreiraQuimica { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int MaterialArameInterno { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int MaterialArameExterno { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int TipoConexao1 { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int materialConexao1 { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int TipoConexao2 { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int materialConexao2 { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public bool isEletricamenteContinuo { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int tipoRevestimento { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int diametroNominal { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int comprimentoNominal { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public string fabricante { set; get; }

     //   [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public string status { set; get; }

        [Column]
        public DateTime date { set; get; }

        [Column]
        public DateTime dataFabricacao { set; get; }

        [Column]
        public DateTime dataValidade { set; get; }

        [Column]
        public string localArmazenamento { set; get; }
        
     

        [Column]
        public string tagCliente { set; get; }

        [Column]
        public DateTime dateLastRevision { set; get; }

        [Column]
        public DateTime lastEntry { set; get; } = DateTime.Now;

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public string numPedidoCompraCliente { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public string numPedidoVendaCliente { set; get; }

        [Column]
        public string codComercial { set; get; }

        [System.ComponentModel.DataAnnotations.Required]
        [Column]
        public int aplicacaoPrincipal { set; get; }

        [Column]
        public string notas { set; get; }

        


        //generate unique code 
        public  int generateNewCode(Mangueira[] mangueirasInDb)
        {
            int code = 0;
            Random randNum = new Random();
            foreach (Mangueira i in mangueirasInDb)
            {
                int tryCode = randNum.Next(10000000, 99999999);
                if (!mangueirasInDb.Where(p => p.code == tryCode).Any())
                {
                    code = tryCode;
                    break;
                }
            }
            return code;
        }


        // old
        public int? generateCode()
        {
            if (code > 0)
                return code;           


            int hash = 17;
            hash = hash * 23 + date.GetHashCode();

            if (numPedidoVendaCliente != "")
            {
                hash = hash * 23 + numPedidoVendaCliente.GetHashCode();
                code = hash % 100000000;  // 8 zeros
                if (code < 0) code *= -1;
            }
            return code;
        }


        public override string ToString()
        {
            string getUserData = "";
            getUserData = code + ";";
            getUserData += normaConstrutiva + ";";
            getUserData += tipoMangueira + ";";
            getUserData += materialBarreiraQuimica + ";";
            getUserData += MaterialArameInterno + ";";
            getUserData += MaterialArameExterno + ";";
            getUserData += TipoConexao1 + ";";
            getUserData += materialConexao1 + ";";
            getUserData += TipoConexao2 + ";";
            getUserData += materialConexao2 + ";";
            getUserData += isEletricamenteContinuo + ";";
            getUserData += tipoRevestimento + ";";
            getUserData += diametroNominal + ";";
            getUserData += comprimentoNominal + ";";
            getUserData += fabricante + ";";
            getUserData += dataFabricacao + ";";
            getUserData += date + ";";
            getUserData += lastEntry + ";";
            getUserData += dateLastRevision + ";";
            getUserData += dataValidade + ";";
            getUserData += numPedidoCompraCliente + ";";
            getUserData += numPedidoVendaCliente + ";";
            getUserData += status + ";";
            getUserData += aplicacaoPrincipal + ";";
            getUserData += localArmazenamento + ";";
            getUserData += tagCliente + ";";
            getUserData += codComercial + ";";
            getUserData += notas + ";";


            return getUserData;
        }


    }
}
