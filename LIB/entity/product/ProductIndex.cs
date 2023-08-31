using DbExtensions;
using Newtonsoft.Json;
using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.product
{
    [Table(Name = "product_index")]
    public class ProductIndex : BaseEntity
    {
        [Column(Name = "indexInPreset")]
        protected string _indexInPreset
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    indexInPreset = new Dictionary<int, int>();
                    return;
                }
                indexInPreset = JsonConvert.DeserializeObject<Dictionary<int, int>>(value);
            }
            get => JsonConvert.SerializeObject(indexInPreset);
        }

        public Dictionary<int, int> indexInPreset { set; get; } = new Dictionary<int, int>();

        public void addPresetIndex(int preset, int indexInPreset)
        {
            this.indexInPreset.put(preset, indexInPreset);
        }

        [Column]
        public int indexProduct { set; get; }

        public bool hasProductFor(int presetZeroBased)
        {
            return indexInPreset.ContainsKey(presetZeroBased +1);
        }

        public bool hasProductProductIndex(int presetProductIndex)
        {
            return indexInPreset.ContainsValue(presetProductIndex);
        }

        [Column]
        public string code { set; get; }

        // TODO: Enable or disable product
        [Column(ConvertTo = typeof(sbyte))]
        public bool enable { set; get; } = true;

        [Column]
        public int idProduct { set; get; }

        private Product _product;
        [Association(ThisKey = nameof(idProduct))]
        public Product product
        {
            set
            {
                _product = value;
                idProduct = _product.id;
                code = _product.code;
            }
            get { return _product; }
        }

        [Column]
        public int idDevice { set; get; }

        private Device _device;
        [Association(ThisKey = nameof(idDevice))]
        public Device device
        {
            set
            {
                _device = value;
                idDevice = _device.id;
            }
            get { return _device; }
        }

        public string generateCustomCode() => code.Length >= 6 ? "_" + code.Remove(5) : "_" + code;
    }

    // TODO: Realy need this struct? the presetvalues not are in Compartment?
    public struct ProductPreseted
    {

        public int volume { set; get; }
        public int indexProduct { set; get; }
        public bool isCustom { set; get; }
        public bool isComplement { set; get; }
    }
}
