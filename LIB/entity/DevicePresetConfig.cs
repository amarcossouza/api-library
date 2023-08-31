using DbExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity
{
    [Table(Name = "device_preset_config")]
    public class DevicePresetConfig : BaseEntity
    {
        [Column(ConvertTo = typeof(sbyte))]
        public bool atualize { set; get; } = false;
        [Column(ConvertTo = typeof(sbyte))]
        public bool forceExit { set; get; } = false;
        [Column]
        public int exitTimeout { set; get; } = 18;

        //  TODO: Change for timeout
        [Column]
        public int maxIdleReads { set; get; } = 20;

        [Column(ConvertTo = typeof(int))]
        public ProcessingMode processingMode { set; get; } = ProcessingMode.LOCAL;

        public bool isInLocal() {
            if (atualize && processingMode == ProcessingMode.LOCAL)
                return false;
            if (atualize && processingMode == ProcessingMode.REMOTE)
                return true;
            return this.processingMode == ProcessingMode.LOCAL;
        }

        // TODO: Keep Reference to Preset Class and table 

        // TODO: insert other configs like: custom prompts, 
        // custom inputs, check product recipe, read components or not, can change processingMode, reboot preset and etc

        [Column]
        public int idDevice { set; get; }

        private Device _device;
        [Association(ThisKey = nameof(idDevice))]
        public Device device
        {
            set
            {
                _device = value;
                if(_device != null)
                    idDevice = _device.id;
            }
            get{ return _device; }
        }
    }

    public enum ProcessingMode
    {
        LOCAL = 0,
        REMOTE = 1
    }
}
