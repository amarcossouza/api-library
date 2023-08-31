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
    [Table(Name = "unload_operation_report")]
    public class UnloadOperationReport : BaseOperationReport
    {
        //TODO: separate series in other table

        [Column]
        public int totalizer { set; get; }
        [Column]
        public int totalizer20 { set; get; }
        [Column]
        public int beforeTotalizer { set; get; }
        [Column]
        public int beforeTotalizer20 { set; get; }

        private Cliente _transaction;
        [Association(ThisKey = nameof(idTransaction))]
        public Cliente transaction
        {
            set
            {
                _transaction = value;
                if (_transaction != null)
                    idTransaction = _transaction.id;
            }
            get { return _transaction; }
        }

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

        public UnloadOperationReport() { }

        public UnloadOperationReport(UnloadPresetOnline unloadPresetOnline) : base(unloadPresetOnline)
        {
            this.series = unloadPresetOnline.series;
            this.braco = unloadPresetOnline.braco;
            this.bay = braco.bay;

            this.avaregeFlowRate = this.series.avaregeFlowRate();

            this.totalizer = unloadPresetOnline.getRunningTotalizer().volume;
            this.totalizer20 = unloadPresetOnline.getRunningTotalizer20().volume;
            this.beforeTotalizer = unloadPresetOnline.beforeTotalizer;
            this.beforeTotalizer20 = unloadPresetOnline.beforeTotalizer20;
        }

        public bool hasTransaction => transaction != null;
    }
}
