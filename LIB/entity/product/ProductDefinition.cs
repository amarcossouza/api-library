using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.product
{
    // TODO: Transform ProductDefinition in a struct
    public class ProductDefinition : BaseComponentDefinition
    {
        public List<ComponentDefinition> components { private set; get; } = new List<ComponentDefinition>();
        public List<ComponentDefinition> additives { private set; get; } = new List<ComponentDefinition>();

        public void addComponent(ComponentDefinition c)
        {
            components.Add(c);
        }

        public void addAdditive(ComponentDefinition c)
        {
            additives.Add(c);
        }

        public double getPercentForComponente(int c)
        {
            if (components[c] == null)
            {
                if (c == 0)
                    return 100.0;
                else
                    return 0.0;
            }
            return components[c].percent;
        }

        public bool isBlend => components.Count >= 2;

        public override string ToString()
        {
            string retorno = $"{code}[{indexInPreset}][{indexProduct}]";
            retorno += " C: ";
            foreach (var c in components)
            {
                retorno += c;
            }
            retorno += " A: ";
            foreach (var a in additives)
            {
                retorno += " " + a;
            }
            return retorno;
        }

        public double getPercentualFor(int component, double volumeLoaded, double volumeRemaining, double volumeOriginal)
        {
            double valorOriginal = (volumeOriginal * getPercentForComponente(component)) / 100.0;
            double diffFaltante = valorOriginal - volumeLoaded;
            return (diffFaltante / volumeRemaining) * 100;
        }

        public string customName => $"{code}_TEMP";
        public string customCode => $"{code}_TMP";
    }

    public class ComponentDefinition : BaseComponentDefinition
    {
        public double percent { set; get; }

        public override string ToString()
        {
            return $" {code}[{indexInPreset}][{indexProduct}](%{percent}) ";
        }

        public int[] generateWriteArrayForAdditive()
        {
            return new int[] { indexProduct, percent.percentToInt(10000) };
        }

        public int[] generateWriteArray()
        {
            return new int[] { indexProduct, percent.percentToInt() };
        }
    }
}
