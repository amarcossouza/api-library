using DbExtensions;
using PHD_TAS_LIB.entity.online;
using PHD_TAS_LIB.entity.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.report
{
    public abstract class BaseOperationReport : BaseEntity
    {
        public BaseOperationReport() { }

        public BaseOperationReport(BasePresetOnline presetOnline)
        {
            this.avaregeDensity = presetOnline.lastOnlineData.avaregeDensity;
            this.avaregeTemperature = presetOnline.lastOnlineData.temperature;
            this.volumePreseted = presetOnline.lastOnlineData.volumePreseted;
            this.volumeLoaded = presetOnline.lastOnlineData.volumeLoaded;
            this.volumeLoaded20 = presetOnline.lastOnlineData.volumeLoaded20;
            this.dateStart = presetOnline.beginOperation.Value;
            this.productName = presetOnline.productName;
            this.idDevice = presetOnline.idDevice;
            this.idPreset = presetOnline.idPreset;
        }

        //[System.ComponentModel.DataAnnotations.Required]
        //[System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        [Column]
        public int volumePreseted { set; get; }
        [Column]
        public int volumeLoaded { set; get; }
        [Column]
        public int volumeLoaded20 { set; get; }
        [Column]
        public int avaregeFlowRate { set; get; }

        public double avaregeFlowRateInDouble => avaregeTemperature / 100.0;

        [Column]
        public int avaregeTemperature { set; get; }

        public double avaregeTemperatureInDouble => avaregeTemperature / 100.0;

        [Column]
        public int avaregeDensity { set; get; }

        public double avaregeDensityInDouble => avaregeDensity / 10.0;

        [Column]
        public DateTime dateStart { set; get; }
        [Column]
        public DateTime dateEnd { set; get; } = DateTime.Now;

        [Column]
        public int idPreset { set; get; }
        [Column]
        public string presetName { set; get; } // TODO: consider remove. keep only in load operations
        [Column]
        public int idBay { set; get; }
        [Column]
        public string bayName { set; get; }
        [Column]
        public int idDevice { set; get; }


        [Column]
        public int? idProduct { set; get; }
        [Column]
        public int? idTransaction { set; get; }

        // TODO: add all alarms id in JSON field

        // report
        //Desnormalizado: code, placa, c.numero, c.produto, volumes...series...etc..., complemento , JSON{p.numero, p.compania, p.cliente}
        //Normalizado: idCompartimento, idProduto, idPedido?, idTransacao?

        [Column]
        public string productName { set; get; }

        private Product _product;
        [Association(ThisKey = nameof(idProduct))]
        public Product product
        {
            set
            {
                _product = value;
                idProduct = _product.id;
                productName = _product.name;
            }
            get { return _product; }
        }

        private Preset _braco;
        [Association(ThisKey = nameof(idPreset))]
        public Preset braco
        {
            set
            {
                _braco = value;
                idPreset = _braco.id;
                presetName = _braco.name;
                idBay = _braco.idBay;
                bayName = _braco.bay.name;
                // TODO: Set bay from preset, but when preset.bay is NULL?
            }
            get { return _braco; }
        }

        private Bay _bay;
        [Association(ThisKey = nameof(idBay))]
        public Bay bay
        {
            set
            {
                _bay = value;
                idBay = _bay.id;
                bayName = _bay.name;
            }
            get { return _bay; }
        }
    }
}
