using System;
using DbExtensions;
using PHD_TAS_LIB.entity.tag;
using PHD_TAS_LIB.modbus;

namespace PHD_TAS_LIB.entity.command
{   
    // TODO: alter this class to permit bool reads/writes
    [Table(Name = "pump_plc_config")]
    public class PumpPLCConfig : BaseCommand
    {
        [Column]
        public int sequential { set; get; }

        // TODO: create wait time mecanism for temporary commands and other abstractions
        [Column]
        public int priority { set; get; } = 1;

        public bool valueBool => Convert.ToBoolean(value);

        public void readOn(ModbusDataPackage read)
        {
            value = read.getUShort(tag.register);
        }

        public void writeOn(ModbusInterface modbus)
        {
            // TODO: modbus sould be return bool
            modbus.escritaSingle(tag.register, value);
        }
    }
}
