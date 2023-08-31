using PHD_TAS_LIB.util;
using System;

namespace PHD_TAS_LIB.modbus
{
    public interface ModbusBatchDataPackage
    {
        byte[] getByte(int register);

        int getInt(int register);

        int TryGetInt(int register);

        string TryGet(int register);

        string get(int register);

        int getIntGenericWarp(int register);

        string getGenericWarp(int register);

        ushort getUShort(int register);

        float getFloat(int register, bool swap = false);

        ushort[] getWord(int register, int size);
    }

    public class ModbusDataPackage : ModbusBatchDataPackage
    {
        protected ushort[] bytes;
        public int startIndex { get; } = 0;

        public int size { get { return bytes.Length; } }

        public ModbusDataPackage(ushort[] bytes, int beginRegister)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentNullException();
            this.bytes = bytes;
            this.startIndex = beginRegister;
            //this.lenght = deviceLenght;
            
        }

        public int getInt(int register)
        {
            int finalIndex = register - startIndex;
            if (finalIndex < 0 || finalIndex >= bytes.Length)
                throw new ArgumentException();
            return bytes[finalIndex];
        }

        public int TryGetInt(int register)
        {
            int finalIndex = register - startIndex;
            if (finalIndex < 0 || finalIndex >= bytes.Length)
                return 0;
            return bytes[finalIndex];
        }

        public string get(int register)
        {
            int finalIndex = register - startIndex;
            if (finalIndex < 0 || finalIndex >= bytes.Length)
                throw new ArgumentException();

            return Convert.ToString(bytes[finalIndex]);
        }

        public string TryGet(int register)
        {
            int finalIndex = register - startIndex;
            if (finalIndex < 0 || finalIndex >= bytes.Length)
                return "###";
            return Convert.ToString(bytes[finalIndex]);
        }

        public int getIntGenericWarp(int register)
        {
            int finalIndex = register - startIndex;
            if (finalIndex < 0 || finalIndex >= bytes.Length && finalIndex + 1 >= bytes.Length)
                throw new ArgumentException();

            return TypeHelper.warp32(bytes[finalIndex], bytes[finalIndex + 1]);
        }

        public string getGenericWarp(int register)
        {
            int finalIndex = register - startIndex;
            if (finalIndex < 0 || finalIndex >= bytes.Length && finalIndex + 1 >= bytes.Length)
                throw new ArgumentException();

            return Convert.ToString(TypeHelper.warp32(bytes[finalIndex], bytes[finalIndex + 1]));
        }

        public ushort getUShort(int register)
        {
            int finalIndex = register - startIndex;
            if (finalIndex < 0 || finalIndex >= bytes.Length)
                throw new ArgumentException();

            return bytes[finalIndex];
        }

        public float getFloat(int register, bool swap = false)
        {
            int finalIndex = register - startIndex;
            if (finalIndex < 0 || finalIndex >= bytes.Length && finalIndex + 1 >= bytes.Length)
                throw new ArgumentException();

            return TypeHelper.floatWarp(bytes[finalIndex], bytes[finalIndex + 1], swap);
        }

        public ushort[] getWord(int register, int size)
        {
            int finalIndex = register - startIndex;
            if (finalIndex < 0 || finalIndex >= bytes.Length && finalIndex + size >= bytes.Length)
                throw new ArgumentException();

            ushort[] r = new ushort[size];
            //Buffer.BlockCopy(bytes, finalIndex+size, r, 0, size);
            for (int i = 0; i < size; i++)
            {
                r[i] = bytes[i + finalIndex];
            }
            return r;
        }

        public byte[] getByte(int register)
        {
            int finalIndex = register - startIndex;
            if (finalIndex < 0 || finalIndex >= bytes.Length)
                throw new ArgumentException();

            return BitConverter.GetBytes(bytes[finalIndex]);
        }

    }

    public class ModbusErroDataPackage : ModbusBatchDataPackage
    {
        public int getInt(int register)
        {
            return 0;
        }

        public int TryGetInt(int register)
        {
            return 0;
        }

        public string get(int register)
        {
            return "###";
        }

        public string TryGet(int register)
        {
            return "###";
        }

        public int getIntGenericWarp(int register)
        {
            return 0;
        }

        public string getGenericWarp(int register)
        {
            return "###";
        }

        public ushort getUShort(int register)
        {
            return 0;
        }

        public float getFloat(int register, bool swap = false)
        {
            return 0;
        }

        public ushort[] getWord(int register, int size)
        {
            ushort[] r = new ushort[size];
            for (int i = 0; i < size; i++)
            {
                r[i] = 0;
            }
            return r;
        }

        public byte[] getByte(int register)
        {
            return BitConverter.GetBytes(0);
        }

    }
}
