using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.util
{
    public static class VolumeConversion
    {

        // TODO: refactor to array
        private static readonly List<double> densidA
            = new List<double>() { 0.4, 0.4981, 0.5181, 0.5391, 0.5591, 0.5791, 0.6001, 0.6151, 0.6351, 0.6551,
                0.6751, 0.6951, 0.7461, 0.7661, 0.7861, 0.8061, 0.8261, 0.8461, 0.8711, 0.8961, 0.9961 };
        private static readonly List<double> densidB
            = new List<double>() { 0.498, 0.518, 0.539, 0.559, 0.579, 0.6, 0.615, 0.635, 0.655, 0.675, 0.695,
                0.746, 0.766, 0.786, 0.806, 0.826, 0.846, 0.871, 0.896, 0.996, 2 };
        private static readonly List<double> a1
            = new List<double>() { -0.002462, -0.002391, -0.002294, -0.002146, -0.00192, -0.002358, -0.001361,
                -0.001237, -0.001077, -0.001011, -0.000977, -0.001005, -0.001238, -0.001084, -0.000965,
                -0.0008435, -0.000719, -0.000617, -0.000512, -0.0003948, -0.0005426 };
        private static readonly List<double> a2
            = new List<double>() { 0.003215000, 0.003074000, 0.002887000, 0.002615000, 0.002214000, 0.002962000,
                0.001300000, 0.001100000, 0.000850000, 0.000750000, 0.000700000, 0.000740000, 0.001050000,
                0.000850000, 0.000700000, 0.000550000, 0.000400000, 0.000280000, 0.000160000, 0.000030000, 0.000177800 };
        private static readonly List<double> b1
            = new List<double>() { -0.000010140, -0.000008410, -0.000000839, -0.000005460, -0.000005510, -0.000012250,
                -0.000000490, -0.000000490, -0.000000490, -0.000000490, -0.000000490, -0.000000490, -0.000000490,
                -0.000000490, -0.000000490, -0.000000490, -0.000000490, -0.000000490, -0.000000490, -0.000000490, 0.000002310 };
        private static readonly List<double> b2
            = new List<double>() { 0.000017380, 0.000013980, 0.000013870, 0.000008550, 0.000008550, 0.000020150,
                0.000000600, 0.000000600, 0.000000600, 0.000000600, 0.000000600, 0.000000600, 0.000000600,
                0.000000600, 0.000000600, 0.000000600, 0.000000600, 0.000000600, 0.000000600, 0.000000600, -0.000002200 };


        // TODO define better place for getVolume20ForNonHydrocarbon and getVolume20ForHydrocarbon
        public static double getVolume20ForNonHydrocarbon(double volume, double temp)
        {
            double valor = 0.00109;
            //temp /= 100;
            double volumeAmb = volume;
            double fatorCorrecAlcool = 1 - (valor * (temp - 20));
            fatorCorrecAlcool = Math.Round(fatorCorrecAlcool, 4);
            double volumeCorrigido = volumeAmb * fatorCorrecAlcool;
            volumeCorrigido = Math.Round(volumeCorrigido);
            return volumeCorrigido;
        }

        public static double getVolume20ForHydrocarbon(double volume, double temp, double density)
        {
            double returno = 0.0;
            try
            {
                int j = 0;
                double Result = 0;
                double Fator = 0;
                double vol20 = 0;
                double p1 = 0;
                double p2 = 0;
                double p3 = 0;
                double p4 = 0;
                double HYC = 0;
                double TemperaturaLida = temp > 0.0 ? temp : 1.0;
                //TemperaturaLida /= 100;
                //  Console.WriteLine(TemperaturaLida);

                foreach (decimal i in densidA)
                {
                    if (density >= densidA.ElementAt(j) && density <= densidB.ElementAt(j))
                    {
                        p1 = (9.0 / 5.0) * 0.999042 * ((a1.ElementAt(j) + (16.0 * b1.ElementAt(j)) - (8.0 * a1.ElementAt(j) + 64.0 * b1.ElementAt(j)) * (a2.ElementAt(j) + 16.0 * b2.ElementAt(j)) / (1.0 + (8.0 * a2.ElementAt(j)) + (64.0 * b2.ElementAt(j)))));
                        p2 = (9.0 / 5.0) * ((a2.ElementAt(j) + 16.0 * b2.ElementAt(j)) / (1.0 + (8.0 * a2.ElementAt(j)) + (64.0 * b2.ElementAt(j))));
                        p3 = (81.0 / 25.0) * 0.999042 * ((b1.ElementAt(j) - (8.0 * a1.ElementAt(j) + 64.0 * b1.ElementAt(j)) * b2.ElementAt(j)) / (1.0 + 8.0 * a2.ElementAt(j) + 64.0 * b2.ElementAt(j)));
                        p4 = (81.0 / 25.0) * (b2.ElementAt(j) / (1.0 + 8.0 * a2.ElementAt(j) + 64.0 * b2.ElementAt(j)));
                        HYC = 1.0 - 0.000023 * (TemperaturaLida - 20.0) - 0.00000002 * (Math.Pow((TemperaturaLida - 20.0), 2.0));
                        Result = (density - p1 * (TemperaturaLida - 20.0) - (p3 * Math.Pow((TemperaturaLida - 20.0), 2.0))) / (1.1 + p2 * (TemperaturaLida - 20.0) + p4 * (Math.Pow((TemperaturaLida - 20.0), 2.0))) * HYC;
                        Fator = 1 + p2 * (TemperaturaLida - 20.0) + p4 * (Math.Pow((TemperaturaLida - 20.0), 2.0)) + ((p1 * (TemperaturaLida - 20.0) + p3 * (Math.Pow((TemperaturaLida - 20.0), 2.0))) / Result);
                        vol20 = volume * Fator;
                        vol20 = Math.Round(vol20);
                        returno = vol20;
                    }
                    j++;
                }
            }
            catch (Exception e)
            {
                Logging.Error("ON getVolume20ForHydrocarbon E:"+e.Message, "VolumeConversion");
            }
            return returno;
        }

    }
}
