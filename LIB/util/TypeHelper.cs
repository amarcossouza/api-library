using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LIB.util
{
    public static class TypeHelper
    {
        public static ushort toShort(this Enum r)
        {
            return Convert.ToUInt16(r);
        }

        public static ushort toShort(this string s)
        {
            int result = 0;
            int.TryParse(s, out result);
            return Convert.ToUInt16(result);
        }

        public static ushort toUShort(this int s)
        {
            return Convert.ToUInt16(s);
        }

        public static int toInt(this string s)
        {
            int result = 0;
            int.TryParse(s, out result);
            return result;
        }

        public static double toDouble(this string s)
        {
            double valor = 0.0;
            double.TryParse(s, out valor);
            return valor;
        }

        public static decimal toDecimal(this string s)
        {
            decimal valor = 0M;
            decimal.TryParse(s, out valor);
            return valor;
        }

        public static string toDecimal(this string s, int places)
        {
            decimal valor = 0M;
            decimal.TryParse(s, out valor);
            valor = valor / (decimal)places;
            return Convert.ToString(valor).Replace(',', '.');
        }

        public static int percentToInt(this double d, int multi = 100)
        {   
            return Convert.ToInt32(d * multi);
        }

        public static BitArray getBits(this ushort value)
        {
            return new BitArray(BitConverter.GetBytes(value));
        }

        public static int setTrueBitOf(this int value, int index)
        {
            return value |= 1 << index;
        }

        public static int setFalseBitOf(this int value, int index)
        {
            return value &= ~(1 << index);
        }

        public static int setToggleBitOf(this int value, int index)
        {
            return value ^= 1 << index;
        }

        public static bool bitValueOf(this int value, int index)
        {
            return (value & (1 << index)) != 0;
        }

        public static bool bitValueOf(this ushort value, int index)
        {
            return (value & (1 << index)) != 0;
        }

        public static bool invert(this bool value)
        {
            return !value;
        }

        // verifica se o valor retornado é nulo
        public static string verifNull(string var, string SubstituiNull)
        {
            if (var == null || var == "") // tratamento da leitura de pedido no bombeio
                var = SubstituiNull;
            return var;
        }

        public static string intToDecimal(int varInt, int divide)
        {
            string retorno;
            decimal calcula = varInt;
            calcula = calcula / divide;
            retorno = Convert.ToString(calcula);
            retorno = retorno.Replace(',', '.');
            return retorno;
        }

        public static ushort[] floatWarp(this float v, bool swap = false)
        {
            ushort[] r = new ushort[2];
            byte[] fBytes = BitConverter.GetBytes(v);
            if (swap)
            {
                r[1] = BitConverter.ToUInt16(fBytes, 2);
                r[0] = BitConverter.ToUInt16(fBytes, 0);

            }
            else
            {
                r[0] = BitConverter.ToUInt16(fBytes, 2);
                r[1] = BitConverter.ToUInt16(fBytes, 0);
            }
            return r;
        }

        public static float floatWarp(ushort index0, ushort index1, bool swap = false)
        {
            ushort[] uintData = new ushort[2];
            if (swap)
            {
                uintData[0] = index0;
                uintData[1] = index1;
            }
            else
            {
                uintData[0] = index1;
                uintData[1] = index0;
            }
            byte[] v0 = BitConverter.GetBytes(uintData[0]);
            byte[] v1 = BitConverter.GetBytes(uintData[1]);
            byte[] final = new byte[v0.Length + v1.Length];
            Buffer.BlockCopy(v0, 0, final, 0, v0.Length);
            Buffer.BlockCopy(v1, 0, final, v0.Length, v1.Length);
            return BitConverter.ToSingle(final, 0);
        }

        public static string warp32(uint valueLow, uint valueHigh)
        {
            //x = low, y = high
            return Convert.ToString((valueLow << 16) | valueHigh);
        }

        public static int warp32(int valueLow, int valueHigh)
        {
            //x = low, y = high
            return (valueLow << 16) | valueHigh;
        }

        public static ushort[] warp16(int value)
        {
            ushort[] result = { 0, 0 };
            result[0] = (ushort)value;
            result[1] = (ushort)(value >> 16);
            return result;
        }

        public static ushort warp16u(ushort value)
        {
            byte[] getByte = BitConverter.GetBytes(value);

            string conv = Convert.ToString(256 * getByte[0] + getByte[1]);
            ushort aux = Convert.ToUInt16(conv);
            return aux;
        }


        public static string readModbusFloat32(ushort index0, ushort index1, bool warp)
        {
            string retorno = "";
            ushort[] data = new ushort[2];
            if (warp)
            {
                data = new ushort[2] { (warp16u(index0)), warp16u(index1) };
            }
            else
            {
                data = new ushort[2] { (index0), index1 };
            }
            float[] floatData = new float[data.Length / 2];
            Buffer.BlockCopy(data, 0, floatData, 0, data.Length * 2);
            for (int index = 0; index < floatData.Length; index++)
            {
                if (floatData[index / 2].ToString("0.0000") != "0.0000")
                {
                    retorno = floatData[index / 2].ToString("0.0000");
                }
            }
            retorno = retorno.Replace('.', ',');
            return retorno;
        }

        public static string[] StringValues(System.Data.IDataRecord r)
        {
            string[] records = new string[r.FieldCount];
            for (int i = 0; i < records.Length; i++)
            {
                records[i] = r.IsDBNull(0) ? null : r.GetString(i);
            }
            return records;
        }

        public static int[] IntValues(System.Data.IDataRecord r)
        {
            int[] records = new int[r.FieldCount];
            for (int i = 0; i < records.Length; i++)
            {
                records[i] = r.IsDBNull(i) ? 0 : r.GetInt32(i);
            }
            return records;
        }

        public static string StringValue(System.Data.IDataRecord r)
        {

            return r.IsDBNull(0) ? null : r.GetString(0);
        }

        public static int IntValue(System.Data.IDataRecord r)
        {
            return r.IsDBNull(0) ? 0 : r.GetInt32(0);
        }

        public static int? IntNullableValue(System.Data.IDataRecord r)
        {
            return r.GetInt32(0);
        }

        public static bool BoolValue(System.Data.IDataRecord r)
        {
            return r.IsDBNull(0) ? false : r.GetBoolean(0);
        }

        public static int getword16From32(int word32, int part)
        {
            int[] retorno = new int[2];
            retorno[1] = (word32 >> 16);
            retorno[0] = (word32 & 0xffff);
            return retorno[part];
        }


        public static void put<K, V>(this System.Collections.Generic.Dictionary<K, V> dict, K key, V value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        public static V get<K, V>(this System.Collections.Generic.Dictionary<K, V> dict, K key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                return default(V);
            }
        }

        public static string sha1(this string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
            }
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IEnumerable<T> GetValues<T>(this Enum e)
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static bool HasProperty(ExpandoObject obj, string name)
        {
            if (obj is ExpandoObject)
                return ((IDictionary<string, object>)obj).ContainsKey(name);
            return obj.GetType().GetProperty(name) != null;
        }

        // hashalgoritms exemple
        //const string BaseUrlChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        //private static string ShortUrl
        //{
        //    get
        //    {
        //        const int numberOfCharsToSelect = 5;
        //        int maxNumber = BaseUrlChars.Length;

        //        var rnd = new Random();
        //        var numList = new List<int>();

        //        for (int i = 0; i < numberOfCharsToSelect; i++)
        //            numList.Add(rnd.Next(maxNumber));

        //        return numList.Aggregate(string.Empty, (current, num) => current + BaseUrlChars.Substring(num, 1));
        //    }
        //}
        //const int numberOfNumbersNeeded = 100;
        //    const int numberOfBytesNeeded = 8;
        //private byte[] hashh() {

        //    
        //    var randomGen = RandomNumberGenerator.Create();
        //    for (int i = 0; i < numberOfNumbersNeeded; ++i)
        //    {
        //        var bytes = new Byte[numberOfBytesNeeded];
        //        randomGen.GetBytes(bytes);
        //    }
        //}
    }
}
