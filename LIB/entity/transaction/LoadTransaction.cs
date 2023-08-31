using DbExtensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.transaction
{
    [Table(Name = "load_transaction")]
    public class LoadTransaction : BaseTransaction
    {

        // TODO: add transaction number

        // TODO: check compartments encapsulation integrity
        private List<Compartment> _compartments;
        [Association(OtherKey = nameof(Compartment.idTransaction))]
        public List<Compartment> compartments
        {
            set
            {
                if (value != null)
                    foreach (var c in value)
                    {
                        c.transaction = this;
                    }
                _compartments = value;
            }
            get
            {
                if (_compartments != null)
                    foreach (var c in _compartments)
                    {
                        c.transaction = this;
                    }
                return _compartments;
            }
        }

        public void addCompartiment(Compartment c)
        {
            if (compartments == null)
                compartments = new List<Compartment>();
            if (compartments.Contains(c))
                return;
            compartments.Add(c);
            c.transaction = this;
        }

        public ushort[] createAutorizedPreset(int idDevice, int amountOfPresets)
        {
            ushort[] autorizedPresets = new ushort[amountOfPresets];
            for (int i = 0; i < amountOfPresets; i++)
            {
                autorizedPresets[i] = compartments.Any(c => c.productIndex != null
                                                       && c.productIndex.idDevice == idDevice
                                                       && c.productIndex.hasProductFor(i)) ? (ushort)2 : (ushort)1;
                                                       //&& !c.isVolumeCompletedLoad - liberar compartimento completo - para permitir complemento
                //  0 = Disabled, 1 = Enabled/Not Available, 2 = Available 
            }
            //return new ushort[] { 2, 0, 0, 0, 0, 0 };
            return autorizedPresets;
        }

        public Compartment findByNumber(int compartmentSelected)
        {
            if (compartments == null)
                return null;
            return compartments.Where(c => c.number == compartmentSelected).FirstOrDefault();
        }

        public Compartment findByNumberAndIndexProduct(int compartmentSelected, int indexProduct)
        {
            if (compartments == null)
                return null;
            return compartments.Where(c => c.number == compartmentSelected && c.productIndex != null
                && c.productIndex.indexProduct == indexProduct).FirstOrDefault();
        }

        public string transactionsOrder()
        {
            if (compartments != null && compartments.Count > 0)
            {
                var result = string.Join(",", compartments.GroupBy(c => c.orderNumber).Select(o => o.Key));
                if (!string.IsNullOrEmpty(result))
                    return result;
            }
            return "-";
        }

        public bool hasProductsForDevice(int idDevice)
        {
            if (compartments == null)
                return false;
            return compartments.Any(c => c.productIndex != null
                                    && c.productIndex.idDevice == idDevice);
        }

        public Compartment findByNumberForDevice(int compartmentSelected, int idDevice, int presetZeroBased)
        {
            if (compartments == null)
                return null;
            return compartments.Where(c => c.number == compartmentSelected
                                    && c.productIndex != null
                                    && c.productIndex.idDevice == idDevice
                                    && c.productIndex.hasProductFor(presetZeroBased)).FirstOrDefault();
        }

        public bool isStart()
        {
            if (compartments.Count == 0)
                return false;
            foreach (var c in compartments)
            {
                if (c.online || c.hasBegun)
                    return true;
            }
            return false;
        }

        public string orderNumbers()
        {
            if (compartments == null)
                return "-";
            return string.Join(",", compartments.Select(c => c.orderNumber).Distinct());
        }

        public void loadCompartments(List<Compartment> v)
        {   // TODO: check compartments encapsulation integrity
            this.compartments = v;
        }

    }
}
