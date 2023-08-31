using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.online
{
    [Serializable]
    public class PresetOnlineSeries
    {
        public string name { set; get; }
        public int volumePreseted { set; get; }
        public List<int> volumeLoaded { protected set; get; } = new List<int>();
        public List<int> volumeLoaded20 { protected set; get; } = new List<int>();
        public List<int> flowRate { protected set; get; } = new List<int>();
        public List<int> temperature { protected set; get; } = new List<int>();
        public List<int> avaregeDensity { protected set; get; } = new List<int>();
        public List<DateTime> datetime { protected set; get; } = new List<DateTime>();
        public int size() { return volumeLoaded.Count; }

        public PresetOnlineSeries() { }

        public PresetOnlineSeries(string name)
        {
            this.name = name;
        }

        public void add(PresetOnlineData onlineData)
        {
            if (volumeLoaded.Count > 65000)
            {
                return;
            }

            volumePreseted = onlineData.volumePreseted;
            volumeLoaded.Add(onlineData.volumeLoaded);
            volumeLoaded20.Add(onlineData.volumeLoaded20);
            flowRate.Add(onlineData.flowRate);
            temperature.Add(onlineData.temperature);
            avaregeDensity.Add(onlineData.avaregeDensity);
            datetime.Add(DateTime.Now);
        }

        // TODO: create get last for each serie
        public PresetOnlineSeries getLast(int itens)
        {
            //TODO: fill serie with 0 when size < itens
            if (size() < itens)
                itens = size();
            if (size() - itens <= 0)
                return new PresetOnlineSeries() { volumePreseted = volumePreseted, datetime = datetime };
            return new PresetOnlineSeries(name)
            {
                volumePreseted = volumePreseted,
                volumeLoaded = volumeLoaded.GetRange(size() - itens - 1, itens),
                volumeLoaded20 = volumeLoaded20.GetRange(size() - itens - 1, itens),
                flowRate = flowRate.GetRange(size() - itens - 1, itens),
                temperature = temperature.GetRange(size() - itens - 1, itens),
                avaregeDensity = avaregeDensity.GetRange(size() - itens - 1, itens),
                datetime = datetime.GetRange(size() - itens - 1, itens)
            };
        }

        public string flowRateLast(int itens, string delimiter = ",")
        {
            //TODO: fill serie with 0 when size < itens
            if (size() < itens)
                itens = size();
            if (itens <= 0)
                return "0";
            return string.Join(delimiter, flowRate.GetRange(size() - itens, itens));
        }

        public int avaregeFlowRate()
        {
            // TODO: transform in int and keep fraction part as int
            return (int)flowRate.Average();
        }

        public float[] temperatureFloat()
        {
            return temperature.Select(t => (float)(t / 100.0)).ToArray();
        }

        public float[] avaregeDensityFloat()
        {
            return avaregeDensity.Select(d => (float)(d / 100.0)).ToArray();
        }

        public void clear()
        {
            volumePreseted = 0;
            volumeLoaded.Clear();
            volumeLoaded20.Clear();
            flowRate.Clear();
            temperature.Clear();
            avaregeDensity.Clear();
            datetime.Clear();
        }
    }
}
