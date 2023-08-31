using DbExtensions;
using PHD_TAS_LIB.entity.online;
using PHD_TAS_LIB.entity.tag;
using PHD_TAS_LIB.modbus;
using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.command
{
    //TODO: consider override Equal and HashCode
    [Table(Name = "unload_plc_command")]
    public class UnloadPLCCommand : BaseCommand
    {
        [Column]
        public int bit { set; get; }

        [Column]
        public int number { set; get; }

        [Column]
        public int sequential { set; get; }

        [Column(ConvertTo = typeof(string))]
        public UnloadInstrumentType type { set; get; }

        // TODO: include productName of command or keep in other table? - the product of XV

        // TODO: create wait time mecanism for temporary commands and other abstractions
        [Column]
        public int priority { set; get; } = 1;

        public bool valueBool => value != 0;

        public override string ToString()
        {
            return $"{type.ToString()}: {number}";
        }

        public virtual void readOn(ModbusDataPackage read)
        {
            value = read.getUShort(tag.register).bitValueOf(bit) ? 1 : 0;
        }

        public virtual void writeOn(Dictionary<int, int> registerToWrite, ModbusDataPackage read)
        {   
            if (registerToWrite.ContainsKey(tag.register))
            {
                int newValue = registerToWrite[tag.register];
                if (valueBool)
                    newValue = newValue.setTrueBitOf(this.bit);
                else
                    newValue = newValue.setFalseBitOf(this.bit);
                registerToWrite[tag.register] = newValue;
            }
            else
            {
                int plcValue = read.getUShort(tag.register);
                if (valueBool)
                    plcValue = plcValue.setTrueBitOf(this.bit);
                else
                    plcValue = plcValue.setFalseBitOf(this.bit);
                registerToWrite.Add(tag.register, plcValue);
            }
        }
    }

    public enum UnloadInstrumentType
    {
        XV,
        XV_LOW_FLOW,
        XV_DEAERATOR,
        PUMP,
        LOGIC // RESET DEFAULT, gauge mode
    }
}
