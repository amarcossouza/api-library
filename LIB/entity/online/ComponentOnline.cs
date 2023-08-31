using Newtonsoft.Json;
using PHD_TAS_LIB.entity.product;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.online
{
    [Serializable]
    public class ComponentOnline : PresetOnlineData
    {   
        // TODO: Check if is better keep ComponentDefinition
        //public ComponentDefinition ComponentDefinition { set; get; }
        public string name { set; get; }

        public int totalizer { set; get; }
        public int totalizer20 { set; get; }

        public int beforeTotalizer { set; get; }
        public int beforeTotalizer20 { set; get; }

        [JsonIgnore]
        public PresetOnlineSeries series { protected set; get; }

        [JsonIgnore]
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

        public void incrementSerie()
        {
            if (series == null)
                series = new PresetOnlineSeries(name);
            series.add(this);
            series.name = name;
        }

        public void clearSeries()
        {
            if (series != null)
            {
                series.clear();
                _series = null;
            }
        }
    }
}
