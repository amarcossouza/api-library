using DbExtensions;
using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity
{
    [Table]
    public class Device : BaseEntity
    {
        [Column(ConvertTo = typeof(string))]
        public DeviceType type { set; get; }
        [Column]
        public string ip { set; get; }
        [Column]
        public int timeout { set; get; } = 3000;
        [Column]
        public int retries { set; get; } = 2;
        [Column]
        public int port { set; get; } = 502;
        [Column]
        public int poll { set; get; } = 1000;
        [Column]
        public ushort unitID { set; get; } = 1;
        [Column(ConvertTo = typeof(sbyte))]
        public bool active { set; get; } = false;

        [Column(ConvertTo = typeof(sbyte))]
        public bool online { set; get; } = false;

        public TcpClient CreateTCPClient()
        {
            TcpClient tcpComm = new TcpClient(ip, port)
            {
                SendTimeout = timeout,
                ReceiveTimeout = timeout
            };
            return tcpComm;
        }

        public bool hasCommunication()
        {
            try
            {
                using (Ping p = new Ping())
                {
                    if (p.Send(ip, timeout).Status == IPStatus.Success)
                    {
                        Console.WriteLine("PING SUCCESS:" + ToString());
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("NOT PING DEVICE: " + ToString());
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Error("ON hasCommunication E: " + e.Message, this.ToString());
                return false;
            }
        }

        public override string ToString()
        {
            return type + " - " + ip;
        }

        [Association(ThisKey = nameof(id), OtherKey = nameof(DevicePresetConfig.idDevice))]
        public DevicePresetConfig presetConfig { set; get; }

        [Association(ThisKey = nameof(id), OtherKey = nameof(SerialDevice.idDevice))]
        public SerialDevice serialConfig { set; get; }
    }

    public enum DeviceType
    {
        SMP = 1,
        MULTILOAD = 2,
        PLC_PDAT = 3,
        PLC_MB = 4,
        MULTILOAD_CENTRAL = 5,
        ENRAF_SERIAL = 6,
        ENRAF_MODBUS = 7,
        ACCESS_CONTROL_PANEL = 8,
        MULTILOADSTDALONE = 9,

    }
}
