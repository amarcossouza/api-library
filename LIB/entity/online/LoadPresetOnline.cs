using DbExtensions;
using Newtonsoft.Json;
using PHD_TAS_LIB.entity.online;
using PHD_TAS_LIB.entity.product;
using PHD_TAS_LIB.entity.transaction;
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
    // TODO: Use PresetOnline like a table instread SQL and try persist in Batch
    [Table(Name = "load_preset_online")]
    public class LoadPresetOnline : BasePresetOnline
    {
        // TODO: For more than 2 components use an Collection
        //public Collection<ComponentOnline> components { private set; get; }
        [Column(Name = "component1")]
        protected string _component1
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    component1 = new ComponentOnline();
                    return;
                }
                component1 = JsonConvert.DeserializeObject<ComponentOnline>(value);
            }
            get => JsonConvert.SerializeObject(component1);
        }

        [Column(Name = "component2")]
        protected string _component2
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    component2 = new ComponentOnline();
                    return;
                }
                component2 = JsonConvert.DeserializeObject<ComponentOnline>(value);
            }
            get => JsonConvert.SerializeObject(component2);
        }

        [Column(Name = "seriesC1")]
        protected string _seriesC1
        {
            get
            {
                if (component1 == null)
                    return null;
                return component1._series;
            }
            set
            {
                if (component1 == null)
                    return;
                component1._series = value;
            }
        }

        [Column(Name = "seriesC2")]
        protected string _seriesC2
        {
            get
            {
                if (component2 == null)
                    return null;
                return component2._series;
            }
            set
            {
                if (component2 == null)
                    return;
                component2._series = value;
            }
        }

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
                additives = JsonConvert.DeserializeObject<Dictionary<int, AdditiveOnline>>(value);
            }
            get => JsonConvert.SerializeObject(additives);
        }

        // TODO: keep components data nullable
        public ComponentOnline component1 { protected set; get; }

        public ComponentOnline component2 { protected set; get; }

        public ComponentOnline[] components()
        {
            if (component1 == null && component2 == null)
                return new ComponentOnline[] { new ComponentOnline() };
            if (component1 == null)
                return new ComponentOnline[] { component2 };
            if (component2 == null)
                return new ComponentOnline[] { component1 };
            return new ComponentOnline[] { component1, component2 };
        }

        public Dictionary<int, AdditiveOnline> additives { protected set; get; } = new Dictionary<int, AdditiveOnline>(1);

        public bool hasAdditives => additives != null && additives.Count > 0;

        // TODO: Check fragmentetion of this code
        public void incrementComponentSeries()
        {
            // TODO: Test Remove if
            // TODO: Check and Rename this condition, this is necessary
            if (volumeLoaded > 0 &&
                (presetState.getOrdinal() > 0 && presetState.getOrdinal() < 8 || presetState == MLPresetState.STOP))
            {
                component1.incrementSerie();
                component2.incrementSerie();
            }

        }

        public override void beginsOperation()
        {
            beginOperation = DateTime.Now;
            component1.volumePreseted = volumePreseted;
            component2.volumePreseted = volumePreseted;
            component1.beforeTotalizer = component1.totalizer;
            component2.beforeTotalizer = component2.totalizer;
            component1.beforeTotalizer20 = component1.totalizer20;
            component2.beforeTotalizer20 = component2.totalizer20;

            productName = null;
            onlineProduct = null;

            _series = null;
            _seriesC1 = null;
            _seriesC2 = null;
            if (series != null)
                series.clear();
            if (additives != null)
                additives.Clear();
        }

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
                    if (_compartment.productIndex != null)
                        productName = _compartment.productIndex.code;
                }
                else
                {
                    idCompartment = null;
                }
            }
            get { return _compartment; }
        }

        public bool hasCompartment => idCompartment.HasValue && idCompartment > 0;

        public bool hasTransaction => idTransaction.HasValue && idTransaction.Value > 0;

        private LoadTransaction _transaction;
        [Association(ThisKey = nameof(idTransaction))]
        public LoadTransaction transaction
        {
            set
            {
                _transaction = value;
                if (_transaction != null)
                    idTransaction = _transaction.id;
                else
                    idTransaction = null;

            }
            get { return _transaction; }
        }

        // TODO: create ProductPreseted or alter compartiment product and index?
        public ProductPreseted? productPreseted { private set; get; } = null;

        public ushort[] createApplyRegister()
        {
            if (productPreseted.HasValue)
            {
                ushort[] typeValue = TypeHelper.warp16(productPreseted.Value.volume);
                ushort[] apply = new ushort[5];
                apply[0] = Convert.ToUInt16(productPreseted.Value.indexProduct);
                apply[1] = typeValue[1];
                apply[2] = typeValue[0];
                apply[3] = 0;
                apply[4] = 1;
                return apply;
            }
            return new ushort[] { 0, 0, 0, 0, 0 };
        }

        public void presetCustomProduct(Compartment c, int volumeTransaction, int indexProduct)
        {
            this.compartment = c;
            // TODO: Use product code ?
            productPreseted = new ProductPreseted()
            {
                volume = volumeTransaction,
                indexProduct = indexProduct,
                isCustom = true,
                isComplement = false
            };
        }

        public void presetComplementProduct(Compartment c, int volumeTransaction, int indexProduct)
        {
            this.compartment = c;
            // TODO: Use product code ?
            productPreseted = new ProductPreseted()
            {
                volume = volumeTransaction,
                indexProduct = indexProduct,
                isCustom = false,
                isComplement = true
            };
        }

        public void presetCompartimentProduct(Compartment c)
        {
            this.compartment = c;

            productPreseted = new ProductPreseted()
            {
                volume = compartment.volumeRemaining,
                indexProduct = compartment.productIndex.indexProduct,
                isCustom = false,
                isComplement = false
            };
        }

        public override void cleanOperation()
        {

            // Clean on completed 
            beginOperation = null;
            productPreseted = null;
            idCompartment = null;
            compartment = null;

            // Clean on exit and before
            //productName = null;
            onlineProduct = null;

            component1 = new ComponentOnline();
            component2 = new ComponentOnline();
            _component1 = null;
            _component2 = null;
            //_series = null;
            _seriesC1 = null;
            _seriesC2 = null;
            if (series != null)
                series.clear();
            if (additives != null)
                additives.Clear();

        }

        public bool isOperationClean => !beginOperation.HasValue && !productPreseted.HasValue && onlineProduct == null && !hasCompartment;

        public AdditiveOnline getAdditive(int index)
        {
            if (additives == null)
            {
                additives = new Dictionary<int, AdditiveOnline>(1);
                return null;
            }
            AdditiveOnline a = null;
            additives.TryGetValue(index, out a);
            return a;
        }
    }
}
