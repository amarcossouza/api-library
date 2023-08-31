using PHD_TAS_LIB.entity.command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.online
{
    //TODO: Rename to InstrumentOnline
    [Serializable]
    public class UnloadInstrumentOnline
    {
        public CommandStatus statusCmd { set; get; } = new CommandStatus(false);

        public CommandStatus maintenance { set; get; } = new CommandStatus(false);

        public CommandStatus automaticOperation { set; get; } = new CommandStatus(true);

        public bool status { set; get; } = false;

        public string name { set; get; }

        public int number { set; get; } = 1;

        public UnloadInstrumentType type { set; get; }

        public void setValuesBy(UnloadPLCCommand cmd)
        {
            //if (type != cmd.type)
            //    return;
            switch (cmd.name)
            {
                // TODO: use enum for cmd.name in instruments
                case "manut": maintenance = new CommandStatus(cmd.valueBool, cmd.id); break;
                case "auto": automaticOperation = new CommandStatus(cmd.valueBool, cmd.id); break;
                case "statusCmd": statusCmd = new CommandStatus(cmd.valueBool, cmd.id); break;
                case "status": status = cmd.valueBool; break;
                default:
                    break;
            }
        }
    }

    public struct CommandStatus
    {
        public CommandStatus(bool status, int? idCommand = null) : this()
        {
            this.idCommand = idCommand;
            this.status = status;
        }

        public int? idCommand { set; get; }
        public bool status { set; get; }
    }

    [Serializable]
    public class PumpCommandOnline
    {
        public CommandStatus statusCmd { set; get; } = new CommandStatus(false);

        public CommandStatus maintenance { set; get; } = new CommandStatus(false);

        public CommandStatus automaticOperation { set; get; } = new CommandStatus(true);

        // variavel so de status
        public bool status { set; get; } = false;

        // variavel so de status
        public bool startFail { set; get; } = false;

        // variavel de status
        public bool inverterAlarm { set; get; } = false;

        // variavel de config
        public CommandStatus hasInverter { set; get; } = new CommandStatus(false);

        // variavel de config
        public bool inverterInLocal { set; get; } = false;

        // variavel de config
        public bool directStartInLocal { set; get; } = false;

        public bool isInLocal()
        {
            if (hasInverter.status)
                return inverterInLocal;
            else
                return directStartInLocal;
        }

        public void setValuesBy(PumpPLCCommand cmd)
        {
            switch (cmd.name)
            {
                // TODO: use enum for cmd.name in instruments
                case "manut": maintenance = new CommandStatus(cmd.valueBool, cmd.id); break;
                case "auto": automaticOperation = new CommandStatus(cmd.valueBool, cmd.id); break;
                case "statusCmd": statusCmd = new CommandStatus(cmd.valueBool, cmd.id); break;
                case "hasInverter": hasInverter = new CommandStatus(cmd.valueBool, cmd.id); break;  
                case "status": status = cmd.valueBool; break;
                case "startFail": startFail = cmd.valueBool; break;
                case "inverterAlarm": inverterAlarm = cmd.valueBool; break;
                case "inverterInLocal": inverterInLocal = cmd.valueBool; break;
                case "directStartInLocal": directStartInLocal = cmd.valueBool; break;
                default:
                    break;
            }
        }
    }


}
