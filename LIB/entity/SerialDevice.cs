using DbExtensions;
using Newtonsoft.Json;
using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity
{
    //TODO: too many resposabilities - device and serialInterface
    //TODO: too many try cath
    [Table(Name = "serial_device")]
    public class SerialDevice : BaseEntity
    {
        [Column]
        public int baudRate { set; get; } = 9600;
        [Column]
        public int dataBits { set; get; } = 7;
        [Column]
        public int readTimeOut { set; get; } = 3000;
        [Column]
        public int writeTimeOut { set; get; } = 3000;
        [Column(ConvertTo = typeof(string))]
        public Parity parity { set; get; } = Parity.Odd;
        [Column(ConvertTo = typeof(string))]
        public StopBits stopBits { set; get; } = StopBits.One;
        [Column(ConvertTo = typeof(string))]
        public Handshake handShake { set; get; } = Handshake.None;
        [Column]
        public string portName { set; get; } = "COM3";

        [Column]
        public int idDevice { set; get; }

        private Device _device;
        [Association(ThisKey = nameof(idDevice))]
        public Device device
        {
            set
            {
                _device = value;
                if (_device != null)
                    idDevice = _device.id;
            }
            get { return _device; }
        }

        [Column]
        public int index { set; get; }

        [Column]
        public bool enableModbusServer { get; set; } = false;

        [Column]
        public string serverIP { get; set; } = "127.0.0.1";

        [Column]
        public int serverPort { get; set; } = 502;

        public SerialPort sp { private set; get; }

        public bool isConnected()
        {
            if (sp == null)
                return false;
            return sp.IsOpen;
        }

        public override string ToString()
        {
            return $"{device.type.ToString()} - {portName}";
        }

        public char[] readCmd(byte[] message)
        {
            //TODO: Check lenght of this array
            char[] dataResponse = new char[50];

            if (isConnected())
            {
                sp.DiscardInBuffer();
                sp.DiscardOutBuffer();
                sp.Write(message, 0, message.Length);
                //TODO: change Thread.Sleep(500); to condition
                Thread.Sleep(500);
                sp.Read(dataResponse, 0, dataResponse.Length);
            }
            return dataResponse;
        }

        public void CloseConnect()
        {
            try
            {
                sp.Close();
                sp.Dispose();
                sp = null;
            }
            catch (Exception e)
            {
                Logging.Error($"ON CloseConnect SERIAL SERVICE M:{e.Message} E:{e.StackTrace}", this.ToString());
                sp = null;
            }
        }

        private bool CreateSerialPort()
        {
            bool retorno = false;
            try
            {
                if (sp == null)
                {
                    sp = new SerialPort()
                    {
                        BaudRate = baudRate,
                        DataBits = dataBits,
                        Parity = parity,
                        StopBits = stopBits,
                        ReadTimeout = readTimeOut,
                        WriteTimeout = writeTimeOut,
                        Handshake = handShake,
                        PortName = portName
                    };
                }
                retorno = true;
            }
            catch (Exception e)
            {
                Logging.Error($"ON CreateSerialPort SERIAL SERVICE M:{e.Message} E:{e.StackTrace}", this.ToString());
                retorno = false;
            }
            return retorno;
        }

        public bool connect()
        {
            bool retorno = false;
            if (CreateSerialPort())
            {
                try
                {
                    if (!sp.IsOpen)
                    {
                        sp.Open();
                        sp.DiscardInBuffer();
                        sp.DiscardOutBuffer();
                        retorno = true;
                    }
                }
                catch (Exception e)
                {
                    Logging.Error($"ON CONNECT SERIAL SERVICE M:{e.Message}", this.ToString());
                    retorno = false;
                    CloseConnect();
                }
            }
            return retorno;
        }
    }
}
