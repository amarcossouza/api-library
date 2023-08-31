using DbExtensions;
using Newtonsoft.Json;
using PHD_TAS_LIB.entity.online;
using PHD_TAS_LIB.entity.product;
using PHD_TAS_LIB.multiload;
using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.online
{
    public abstract class BasePresetOnline : PresetOnlineData
    {
        [Column]
        public int idPreset { set; get; }

        private Preset _braco;

        [Association(ThisKey = nameof(idPreset))]
        public Preset braco
        {
            set
            {
                _braco = value;
                idPreset = _braco.id;
            }
            get { return _braco; }
        }

        [Column]
        public int? idTransaction { set; get; }

        [Column]
        public int idDevice { set; get; }

        [Association(ThisKey = nameof(idDevice), OtherKey = nameof(DevicePresetConfig.idDevice))]
        public DevicePresetConfig config { set; get; }

        [Column]
        public DateTime? beginOperation { protected set; get; }

        private ProductDefinition _onlineProduct;
        public ProductDefinition onlineProduct
        {
            get => _onlineProduct;
            set
            {
                _onlineProduct = value;
                if (_onlineProduct != null)
                    productName = _onlineProduct.code;
            }
        }

        public bool hasOnlineProductDefinition => onlineProduct != null;

        [Column]
        public string productName { protected set; get; }

        // produto online - desnormalizado

        // transacao - pedido - normalizado

        // status preset ?

        // last online values

        // totalizadores

        public PresetOnlineData lastOnlineData { protected set; get; } = new PresetOnlineData();

        public PresetOnlineSeries series { protected set; get; } = new PresetOnlineSeries();

        [Column(Name = "series")]
        public string _series
        {
            protected set
            {
                if (string.IsNullOrEmpty(value))
                {
                    series = null;
                    return;
                }
                series = JsonConvert.DeserializeObject<PresetOnlineSeries>(value);
            }
            get { return JsonConvert.SerializeObject(series); }
        }

        // TODO: keep HostState value

        public MLPresetState presetState { set; get; } = MLPresetState.IDLE;
        public void setPresetState(int v) => presetState = MLPresetState.of(v);

        [Column(Name = "presetState")]
        public int _presetState { set { presetState = MLPresetState.of(value); } get { return presetState.getOrdinal(); } }

        // TODO: test remove presetState == MLPresetState.START in this condition
        public bool isInOperation => presetState.isRunning && (presetState == MLPresetState.START || volumeLoaded > 0);

        public bool operationNotBegins => !beginOperation.HasValue;

        public bool isOperationStartAndStop => volumeLoaded > 0 && (flowRate == 0
                   || presetState == MLPresetState.COMPLETE || presetState == MLPresetState.ALARM
                   || presetState == MLPresetState.END_OF_BATCH || presetState == MLPresetState.ARCHIVING
                   || presetState == MLPresetState.CLEARING || presetState == MLPresetState.TRANS_DONE);

        public bool isOperationStart => volumeLoaded > 0;

        public void storageLastOnlineData()
        {
            if (isOperationStartAndStop)
            {
                this.lastOnlineData = new PresetOnlineData
                {
                    volumePreseted = this.volumePreseted,
                    volumeLoaded = this.volumeLoaded,
                    volumeLoaded20 = this.volumeLoaded20,
                    flowRate = this.flowRate,
                    temperature = this.temperature,
                    avaregeDensity = this.avaregeDensity
                };
            }
        }

        public void incrementSeries()
        {
            // TODO: Test Remove if
            // TODO: Check and Rename this condition, this is necessary and check presetState interval
            // || (presetState.getOrdinal() > 13 && presetState.getOrdinal() < 19)
            if (volumeLoaded > 0 &&
                (presetState.getOrdinal() > 0 && presetState.getOrdinal() < 8 || presetState == MLPresetState.STOP))
            {
                if (series == null)
                    series = new PresetOnlineSeries();
                series.add(this);
                series.name = productName;
            }
        }

        public void clearChart() { if (series != null) series.clear(); }

        public Dictionary<int, AlarmOnline> alarms { private set; get; } = new Dictionary<int, AlarmOnline>();
        public bool isInAlarm(int index) => alarms.Any(v => v.Key == index && v.Value.active);
        public AlarmOnline getAlarm(int index) => alarms.FirstOrDefault(v => v.Key == index).Value;

        public Dictionary<int, AlarmOnline> componentAlarms { private set; get; } = new Dictionary<int, AlarmOnline>();
        public bool isComponentInAlarm(int index) => componentAlarms.Any(v => v.Key == index && v.Value.active);
        public AlarmOnline getComponentAlarm(int index) => componentAlarms.FirstOrDefault(v => v.Key == index).Value;

        public Dictionary<int, AlarmOnline> meterAlarms { private set; get; } = new Dictionary<int, AlarmOnline>();
        public bool isMeterInAlarm(int index) => meterAlarms.Any(v => v.Key == index && v.Value.active);
        public AlarmOnline getMeterAlarm(int index) => meterAlarms.FirstOrDefault(v => v.Key == index).Value;

        public Dictionary<int, AlarmOnline> additiveAlarms { private set; get; } = new Dictionary<int, AlarmOnline>();
        public bool isAdditiveInAlarm(int index) => additiveAlarms.Any(v => v.Key == index && v.Value.active);
        public AlarmOnline getAdditiveAlarm(int index) => additiveAlarms.FirstOrDefault(v => v.Key == index).Value;

        public abstract void beginsOperation();

        public abstract void cleanOperation();

        public bool isPresetStatusInEndOperation()
        {
            return presetState == MLPresetState.COMPLETE
                            || presetState == MLPresetState.END_OF_BATCH
                            || presetState == MLPresetState.ARCHIVING
                            || presetState == MLPresetState.CLEARING
                            || presetState == MLPresetState.TRANS_DONE;
        }
    }
}
