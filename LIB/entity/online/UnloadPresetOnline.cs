using DbExtensions;
using Newtonsoft.Json;
using PHD_TAS_LIB.entity.transaction;
using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;

namespace PHD_TAS_LIB.entity.online
{
    [Table(Name = "unload_preset_online")]
    public class UnloadPresetOnline : BasePresetOnline
    {
        public bool hasTrasaction => idTransaction.HasValue && idTransaction.Value > 0;

        private Cliente _transaction;
        [Association(ThisKey = nameof(idTransaction))]
        public Cliente transaction
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

        public TotalizerOnline[] totalizer { protected set; get; } = new TotalizerOnline[6];
        public TotalizerOnline getTotalizer(int id)
        {
            if (id > 6) return new TotalizerOnline();
            return totalizer[id];
        }
        public void setTotalizer(int id, int value, string name)
        {
            if (id > 6) return;
            totalizer[id] = new TotalizerOnline() { name = name, volume = value };
        }

        public TotalizerOnline[] totalizer20 { protected set; get; } = new TotalizerOnline[6];
        public TotalizerOnline getTotalizer20(int id)
        {
            if (id > 6) return new TotalizerOnline();
            return totalizer20[id];
        }
        public void setTotalizer20(int id, int value, string name)
        {
            if (id > 6) return;
            totalizer20[id] = new TotalizerOnline() { name = name, volume = value };
        }
        public TotalizerOnline getRunningTotalizer()
        {
            if (onlineProduct == null)
                return new TotalizerOnline();
            return getTotalizer(onlineProduct.indexInPreset);
        }
        public TotalizerOnline getRunningTotalizer20()
        {
            if (onlineProduct == null)
                return new TotalizerOnline();
            return getTotalizer20(onlineProduct.indexInPreset);
        }

        [Column(Name = "totalizer")]
        protected string _totalizer
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    totalizer = new TotalizerOnline[6];
                    return;
                }
                totalizer = JsonConvert.DeserializeObject<TotalizerOnline[]>(value);
            }
            get => JsonConvert.SerializeObject(totalizer);
        }

        [Column(Name = "totalizer20")]
        protected string _totalizer20
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    totalizer20 = new TotalizerOnline[6];
                    return;
                }
                totalizer20 = JsonConvert.DeserializeObject<TotalizerOnline[]>(value);
            }
            get => JsonConvert.SerializeObject(totalizer20);
        }

        // TODO: Check if beforeTotalizer belongs to PresetOnlineData
        public int beforeTotalizer { set; get; }
        public int beforeTotalizer20 { set; get; }

        public override void beginsOperation()
        {
            beginOperation = DateTime.Now;
            beforeTotalizer = getRunningTotalizer().volume;
            beforeTotalizer20 = getRunningTotalizer20().volume;
        }

        // TODO: Acopamento com Multiload - revisar
        //public ushort[] createApplyRegister()
        //{
        //    ushort[] typeValue = TypeHelper.warp16(transaction.volumeTransaction);
        //    ushort[] apply = new ushort[5];
        //    apply[0] = Convert.ToUInt16(transaction.productIndex.indexProduct);
        //    apply[1] = typeValue[1];
        //    apply[2] = typeValue[0];
        //    apply[3] = 0;
        //    apply[4] = 1;
        //    return apply;
        //}

        public override void cleanOperation()
        {
            beginOperation = null;
            onlineProduct = null;
            //productName = null;
            transaction = null;

            if (series != null)
                series.clear();
        }

        // TODO: implement generalization for this. In BasePresetOnline
        public bool isOperationClean => !beginOperation.HasValue && onlineProduct == null && !hasTrasaction;

        [Association(ThisKey = nameof(idDevice), OtherKey = nameof(UnloadPLCOnline.idSMPDevice))]
        public UnloadPLCOnline plcOnline { set; get; }

        [Association(ThisKey = nameof(idDevice), OtherKey = nameof(Device.id))]
        public Device device { set; get; }
    }
}
