using DbExtensions;
using Newtonsoft.Json;
using PHD_TAS_LIB.entity.online;
using PHD_TAS_LIB.entity.product;
using PHD_TAS_LIB.entity.transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.report
{
    [Table(Name = "load_operation_report")]
    public class LoadOperationReport : BaseOperationReport
    {
        //TODO: separate series in other table

        // addtives
        // complement
        // customRecipe
        // all preset data for C1 and C2

        [Column]
        public int totalizerC1 { set; get; }
        [Column]
        public int totalizer20C1 { set; get; }
        [Column]
        public int beforeTotalizerC1 { set; get; }
        [Column]
        public int beforeTotalizer20C1 { set; get; }

        [Column]
        public int totalizerC2 { set; get; }
        [Column]
        public int totalizer20C2 { set; get; }
        [Column]
        public int beforeTotalizerC2 { set; get; }
        [Column]
        public int beforeTotalizer20C2 { set; get; }

        //[Column(ConvertTo = typeof(sbyte))]
        //public bool complement { set; get; }

        //[Column(ConvertTo = typeof(sbyte))]
        //public bool customRecipe { set; get; }

        public LoadOperationReport() { }

        public LoadOperationReport(LoadPresetOnline loadPresetOnline) : base(loadPresetOnline)
        {
            this.series = loadPresetOnline.series;
            this.braco = loadPresetOnline.braco;
            this.bay = braco.bay;

            this.avaregeFlowRate = this.series.avaregeFlowRate();

            if (compartment == null)
                if (loadPresetOnline.hasCompartment)
                    idCompartment = loadPresetOnline.idCompartment.Value;
                else
                    this.compartment = loadPresetOnline.compartment;

            this.totalizerC1 = loadPresetOnline.component1.totalizer;
            this.totalizer20C1 = loadPresetOnline.component1.totalizer20;
            this.beforeTotalizerC1 = loadPresetOnline.component1.beforeTotalizer;
            this.beforeTotalizer20C1 = loadPresetOnline.component1.beforeTotalizer20;

            this.totalizerC2 = loadPresetOnline.component2.totalizer;
            this.totalizer20C2 = loadPresetOnline.component2.totalizer20;
            this.beforeTotalizerC2 = loadPresetOnline.component2.beforeTotalizer;
            this.beforeTotalizer20C2 = loadPresetOnline.component2.beforeTotalizer20;

            if (loadPresetOnline.component1.series != null)
                this.seriesC1 = loadPresetOnline.component1.series;
            if (loadPresetOnline.component2.series != null)
                this.seriesC2 = loadPresetOnline.component2.series;

            addComponent(loadPresetOnline.component1);
            addComponent(loadPresetOnline.component2);
            if(additives != null && additives.Count > 0)
                additives = loadPresetOnline.additives.Values.ToList();
        }

        public List<ComponentOnline> components { protected set; get; }

        public void addComponent(ComponentOnline c)
        {
            if (components == null)
                components = new List<ComponentOnline>(2);

            components.Add(c);
        }

        [Column(Name = "components")]
        protected string _components
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    components = null;
                    return;
                }
                components = JsonConvert.DeserializeObject<List<ComponentOnline>>(value);
            }
            get => JsonConvert.SerializeObject(components);
        }

        public List<AdditiveOnline> additives { protected set; get; }

        public bool hasAdditives => additives != null && additives.Count > 0;

        [Column(Name = "additives")]
        protected string _additives
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    additives = null;
                    return;
                }
                additives = JsonConvert.DeserializeObject<List<AdditiveOnline>>(value);
            }
            get => JsonConvert.SerializeObject(additives);
        }

        private LoadTransaction _transaction;

        [Association(ThisKey = nameof(idTransaction))]
        public LoadTransaction transaction
        {
            set
            {
                _transaction = value;
                if (_transaction != null)
                    idTransaction = _transaction.id;
            }
            get { return _transaction; }
        }

        public bool hasTransaction => transaction != null;

        [Column]
        public int? numberCompartment { set; get; }  // TODO: consider remove. keep only in object compartment
        [Column]
        public int? idCompartment { set; get; }
        private Compartment _compartment;

        [Association(ThisKey = nameof(idCompartment))]
        public Compartment compartment
        {
            set
            {
                _compartment = value;
                if (_compartment != null)
                {
                    idCompartment = _compartment.id;
                    idTransaction = _compartment.idTransaction;
                    idProduct = _compartment.idProduct;
                    numberCompartment = _compartment.number;
                }
                else
                {
                    idTransaction = null;
                    idProduct = null;
                    numberCompartment = null;
                }
            }
            get { return _compartment; }
        }

        public bool hasCompartment => compartment != null;

        public PresetOnlineSeries series { set; get; }

        [Column(Name = "series")]
        public string _series
        {
            set
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

        public PresetOnlineSeries seriesC1 { set; get; }
        [Column(Name = "seriesC1")]
        public string _seriesC1
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    seriesC1 = null;
                    return;
                }
                seriesC1 = JsonConvert.DeserializeObject<PresetOnlineSeries>(value);
            }
            get { return JsonConvert.SerializeObject(seriesC1); }
        }

        public PresetOnlineSeries seriesC2 { set; get; }
        [Column(Name = "seriesC2")]
        public string _seriesC2
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    seriesC2 = null;
                    return;
                }
                seriesC2 = JsonConvert.DeserializeObject<PresetOnlineSeries>(value);
            }
            get { return JsonConvert.SerializeObject(seriesC2); }
        }
    }
}
