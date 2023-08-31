using System;
using System.Text;

namespace LIB.util
{
   public static class ASCIIConverter
    {   
        // converte Texto ASCII para Ushort para escrever nas prompts
        public static ushort[] AsciiWritePromptML(ushort[] prefix, string word, ushort[] sufix)
        {
            int i = 0;
            ushort[] retorno = new ushort[prefix[0] + 1];

            foreach (ushort n in prefix)
            {
                retorno[i] = n;
                i++;
            }

            foreach (char n in word)
            {
                retorno[i] = Convert.ToUInt16(n);
                i++;
            }

            foreach (ushort n in sufix)
            {
                retorno[i] = n;
                i++;
            }
            return retorno;
        }

        // converte Ushort para ASCII, prefix e sufix podem nao ser utilizados  // TO PARENT
        public static string ReadASCIIAc20(ushort[] var)
        {
            int j = 0;
            byte[] r = new byte[20];
            string retorno = "";

            while (j <= 10)
            {
                r = BitConverter.GetBytes(var[j]);
                Array.Reverse(r);
                j++;
                retorno = retorno + Encoding.ASCII.GetString(r);
            }
            return retorno;
        }

        // método para escrever ascii em variáveis comuns do multiload, sem ser prompts 
        public static ushort[] AsciiWriteVarML(string word)
        {
            ushort[] retorno = new ushort[word.Length];
            int i = 0;
            foreach (char n in word)
            {
                retorno[i] = Convert.ToUInt16(n);
                i++;
            }
            return retorno;
        }

        // converte Ushort para ASCII, prefix e sufix podem nao ser utilizados
        public static string AsciiReadML(ushort[] value, int size)
        {
            int i = 0;
            byte[] Recebe = new byte[80];
            string retorna;

            foreach (ushort n in value)
            {
                Recebe[i] = Convert.ToByte(value[i]);
                i++;
            }
            retorna = Encoding.ASCII.GetString(Recebe);
            return retorna.Remove(size);
        }

        public static string AsciiRead(ushort[] value)
        {
            int i = 0;
            byte[] Recebe = new byte[value.Length];
            foreach (ushort n in value)
            {
                Recebe[i] = Convert.ToByte(value[i]);
                i++;
            }
            return Encoding.ASCII.GetString(Recebe).Replace("\0", "");
        }

        public static ushort[] AsciiWrite(string word)
        {
            var response = new ushort[word.Length];
            int i = 0;
            foreach (var c in ASCIIEncoding.ASCII.GetBytes(word.ToCharArray()))
            {   
                response[i] = Convert.ToUInt16(c);
                i++;
            }
            return response;
        }
    }
}

