using Modbus.Device;
using PHD_TAS_LIB.entity;
using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.modbus
{
    public class ModbusInterface
    {
        public Device device { private set; get; }
        private ModbusIpMaster comModbus;
        private TcpClient tcpConnection;
        private byte slaveAddress;

        private static readonly string BAD_RETURN = "BAD";

        public ModbusInterface(Device device)
        {
            this.device = device;
            slaveAddress = Convert.ToByte(device.unitID);
        }

        public bool isConnected()
        {
            if (tcpConnection == null)
                return false;
            return tcpConnection.Connected;
        }

        public bool connect()
        {
            try
            {
                if (device.hasCommunication())
                {
                    if (!isConnected())
                        this.tcpConnection = device.CreateTCPClient();
                    if (comModbus != null)
                    {
                        comModbus.Dispose();
                        comModbus = null;
                    }
                    comModbus = ModbusIpMaster.CreateIp(tcpConnection);
                    comModbus.Transport.Retries = device.retries;
                    return true;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on ModbusInterface.connect!", @"C:\backup", device.ip, device.nome);
                close();
            }
            return false;
        }

        public void close()
        {
            if (tcpConnection != null)
            {
                if (tcpConnection.Client != null)
                    tcpConnection.Client.Disconnect(false);
                tcpConnection.Close();
                tcpConnection = null;
            }
            if (comModbus != null)
            {
                comModbus.Dispose();
                comModbus = null;
            }
        }

        public void escritaSingle(int register, int value, int delay)
        {
            ushort Reg = Convert.ToUInt16(register);
            ushort valor = Convert.ToUInt16(value);
            try
            {
                comModbus.WriteSingleRegister(slaveAddress, Reg, valor);
                Thread.Sleep(delay);
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on escritaSingle for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on escritaSingle[" + value + "] for: " + register + "E:" + e.Message, @"C:\backup", device.ip, device.nome);
            }
        }

        public void escritaSingle(int register, int value)
        {
            ushort Reg = Convert.ToUInt16(register);
            ushort valor = Convert.ToUInt16(value);
            try
            {
                comModbus.WriteSingleRegister(slaveAddress, Reg, valor);
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on escritaSingle for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on escritaSingle[" + value + "] for: " + register + "E:" + e.Message, @"C:\backup", device.ip, device.nome);
            }
        }

        public void escrita32(int register, int data)
        {
            ushort Reg = Convert.ToUInt16(register);
            ushort[] valor = new ushort[2];

            valor[0] = Convert.ToUInt16(TypeHelper.getword16From32(data, 0));
            valor[1] = Convert.ToUInt16(TypeHelper.getword16From32(data, 1));

            try
            {
                comModbus.WriteMultipleRegisters(slaveAddress, Reg, valor);
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on escritaMultiple for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on escritaSingle[" + data + "]  for: " + register + "E:" + e.Message, @"C:\backup", device.ip, device.nome);
            }
        }

        public void escrita32Swap(int register, int data)
        {
            ushort Reg = Convert.ToUInt16(register);
            ushort[] valor = new ushort[2];

            valor[1] = Convert.ToUInt16(TypeHelper.getword16From32(data, 0));
            valor[0] = Convert.ToUInt16(TypeHelper.getword16From32(data, 1));

            try
            {
                comModbus.WriteMultipleRegisters(slaveAddress, Reg, valor);
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on escritaMultiple for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on escritaSingle[" + data + "]  for: " + register + "E:" + e.Message, @"C:\backup", device.ip, device.nome);
            }
        }



        public void escritaMultiple(int register, IList<int> data)
        {

            ushort Reg = Convert.ToUInt16(register);
            ushort[] valor = new ushort[data.Count];
            int j = 0;
            while (j < data.Count)
            {
                valor[j] = Convert.ToUInt16(data[j]);
                j++;
            }

            try
            {
                comModbus.WriteMultipleRegisters(slaveAddress, Reg, valor);
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on escritaMultiple for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on escritaSingle[" + data.Count + "]  for: " + register + "E:" + e.Message, @"C:\backup", device.ip, device.nome);
            }
        }

        public void escritaMultiple(int register, ushort[] data)
        {
            ushort Reg = Convert.ToUInt16(register);
            try
            {
                comModbus.WriteMultipleRegisters(slaveAddress, Reg, data);
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on escritaMultiple for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on escritaMultiple[" + data.Length + "]  for: " + register + "E:" + e.Message, @"C:\backup", device.ip, device.nome);
            }
        }

        public byte[] leituraByte(int register)
        {
            byte[] retorno = { 0 };
            ushort[] retornoLeitura = new ushort[1];
            ushort uregister = Convert.ToUInt16(register);
            try
            {
                retornoLeitura = comModbus.ReadHoldingRegisters(slaveAddress, uregister, 1);
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on leituraByte for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                retornoLeitura = null;
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on leituraByte for: " + register + "E:" + e.Message, @"C:\backup", device.ip, device.nome);
            }

            if (retornoLeitura != null)
            {
                return BitConverter.GetBytes(retornoLeitura[0]);
            }
            return retorno;
        }

        public ushort[] leituraShort(int register, int count)
        {
            ushort[] retornoLeitura = new ushort[count];
            ushort uregister = Convert.ToUInt16(register);
            try
            {
                retornoLeitura = comModbus.ReadHoldingRegisters(slaveAddress, uregister, Convert.ToUInt16(count));  // totalizador do braço 2 COMP2               
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on leituraShort for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                retornoLeitura = null;
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on leituraShort[" + count + "]  for: " + register + " E: " + e.Message, @"C:\backup", device.ip, device.nome);
            }
            return retornoLeitura;
        }

        public ushort leituraShort(int register)
        {
            ushort[] retornoLeitura = new ushort[1];
            ushort uregister = Convert.ToUInt16(register);
            try
            {
                retornoLeitura = comModbus.ReadHoldingRegisters(slaveAddress, uregister, 1);
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on leituraShort for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                retornoLeitura = null;
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on leituraShort for: " + register + "E: " + e.Message, @"C:\backup", device.ip, device.nome);
            }
            if (retornoLeitura != null)
            {
                return retornoLeitura[0];
            }
            else
            {
                return 0;
            }
        }

        public int leituraInt(int register)
        {
            int retorno = -1;
            ushort[] retornoLeitura = new ushort[1];
            ushort uregister = Convert.ToUInt16(register);
            try
            {
                retornoLeitura = comModbus.ReadHoldingRegisters(slaveAddress, uregister, 1);
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on leituraInt for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                retornoLeitura = null;
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on leituraInt for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }

            if (retornoLeitura != null)
            {
                return retornoLeitura[0];
            }
            return retorno;
        }

        public int leituraIntGenericWarp(int register)
        {
            int retorno = 0;
            ushort[] retornoLeitura = new ushort[2];
            ushort uregister = Convert.ToUInt16(register);
            try
            {
                retornoLeitura = comModbus.ReadHoldingRegisters(slaveAddress, uregister, 2);  // totalizador do braço 2 COMP2               
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on leituraIntGenericWarp for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                retornoLeitura = null;
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on leituraIntGenericWarp for: " + register + "E:" + e.Message, @"C:\backup", device.ip, device.nome);
            }

            if (retornoLeitura != null)
            {
                retorno = TypeHelper.warp32(retornoLeitura[0], retornoLeitura[1]);
            }
            return retorno;
        }

        public int[] leituraIntArrayGenericWarp(int register, ushort size)
        {
            int[] arrayRetorno = new int[size];

            ushort[] retornoLeitura = new ushort[2 * size];
            ushort usize = Convert.ToUInt16(size * 2);
            ushort uregister = Convert.ToUInt16(register);

            try
            {
                retornoLeitura = comModbus.ReadHoldingRegisters(slaveAddress, uregister, usize);
            }
            catch (IOException eio)
            {
                retornoLeitura = null;
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on leituraIntGenericWarp for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                retornoLeitura = null;
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on leituraIntGenericWarp for: " + register + "E:" + e.Message, @"C:\backup", device.ip, device.nome);
            }

            if (retornoLeitura != null)
            {
                int indexRetorno = 0;
                for (int i = 0; i < arrayRetorno.Length; i++)
                {
                    indexRetorno = i * 2;
                    arrayRetorno[i] = TypeHelper.warp32(retornoLeitura[indexRetorno], retornoLeitura[indexRetorno + 1]);
                }
            }
            return arrayRetorno;
        }

        public int leitura32(int register)
        {
            ushort[] getLeitura = new ushort[2];
            int retornoLeitura = -1;
            ushort uregister = Convert.ToUInt16(register);
            try
            {
                getLeitura = comModbus.ReadHoldingRegisters(slaveAddress, uregister, 2);
                retornoLeitura = (getLeitura[1] * 65536) + getLeitura[0];

            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on leituraShort for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on leituraShort for: " + register + "E: " + e.Message, @"C:\backup", device.ip, device.nome);
            }

            if (retornoLeitura != -1)
            {
                return retornoLeitura;
            }
            else
            {
                return 0;
            }

        }



        public string leituraGenericWarp(int register)
        {

            string retorno = BAD_RETURN;
            ushort[] retornoLeitura = new ushort[2];
            ushort uregister = Convert.ToUInt16(register);
            try
            {
                retornoLeitura = comModbus.ReadHoldingRegisters(slaveAddress, uregister, 2);
            }
            catch (IOException eio)
            {
                retornoLeitura = null;
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on leituraGenericWarp for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                retornoLeitura = null;
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on leituraGenericWarp for: " + register, @"C:\backup", device.ip, device.nome);
            }

            if (retornoLeitura != null)
            {
                retorno = Convert.ToString(TypeHelper.warp32(retornoLeitura[0], retornoLeitura[1]));
            }

            return retorno;
        }


        public string leitura(int register)
        {

            string retorno = BAD_RETURN;
            ushort[] retornoLeitura = new ushort[1];
            ushort startReg = Convert.ToUInt16(register);
            try
            {
                retornoLeitura = comModbus.ReadHoldingRegisters(slaveAddress, startReg, 1);
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on leitura for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                retornoLeitura = null;
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on leitura for: " + register, @"C:\backup", device.ip, device.nome);
            }

            if (retornoLeitura != null)
            {
                retorno = Convert.ToString(retornoLeitura[0]);
            }

            return retorno;
        }

        public string[] leituraArray(int register, int count)
        {
            string[] retorno = new string[count];
            for (int id = 0; id <= retorno.Length; id++)
                retorno[id] = BAD_RETURN;

            ushort[] retornoLeitura = new ushort[count];
            ushort startReg = Convert.ToUInt16(register);
            ushort quantidade = Convert.ToUInt16(count);
            int i = 0;

            try
            {
                retornoLeitura = comModbus.ReadHoldingRegisters(slaveAddress, startReg, quantidade);  // totalizador do braço 2 COMP2               
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error("IO ERROR on leituraArray for: " + register, @"C:\backup", device.ip, device.nome);
                close();
            }
            catch (Exception e)
            {
                retornoLeitura = null;
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error("ERROR on leituraArray[" + count + "]  for: " + register, @"C:\backup", device.ip, device.nome);
            }

            while (i < count)
            {
                if (retornoLeitura != null)
                {
                    retorno[i] = Convert.ToString(retornoLeitura[i]);
                }
                else
                {
                    retorno[i] = BAD_RETURN;
                }
                i++;
            }
            return retorno;
        }

        public ModbusDataPackage bathRead(int beginRegister, int size, ushort deviceLenght = 125, int startOverlay = 0)
        {
            int numChunks = (int)Math.Ceiling(size / (double)deviceLenght);
            List<ushort> totalRead = new List<ushort>(numChunks * deviceLenght);
            ushort chunckRegister = 0;
            try
            {
                for (int i = 0; i <= numChunks; i++)
                {
                    chunckRegister = (ushort)(beginRegister + (i * deviceLenght) - startOverlay);
                    ushort[] r = comModbus.ReadHoldingRegisters(slaveAddress, chunckRegister, deviceLenght);
                    totalRead.AddRange(r);
                }
            }
            catch (IOException eio)
            {
                System.Diagnostics.Debug.WriteLine(eio.Message);
                System.Diagnostics.Debug.WriteLine(eio.StackTrace);
                //logging.error($"IO ERROR on bathRead for Begin: {beginRegister} SIZE: {size} CHUNK: {chunckRegister} MSG:{eio.Message} EX: {eio.StackTrace}", @"C:\backup", device.ip, device.nome);
                close();
                throw eio;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                //logging.error($"IO ERROR on bathRead for Begin: {beginRegister} SIZE: {size} CHUNK: {chunckRegister} MSG:{e.Message} EX: {e.StackTrace}", @"C:\backup", device.ip, device.nome);
                throw e;
            }
            return new ModbusDataPackage(totalRead.ToArray(), beginRegister);
        }

    }
}
