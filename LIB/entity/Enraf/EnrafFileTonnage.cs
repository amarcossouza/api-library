using PHD_TAS_LIB.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.Enraf
{
    public class EnrafFileTonnage : IDisposable
    {
        //Microsoft.Jet.OLEDB.4.0
        private readonly OleDbConnection tonnageFile;

        public EnrafFileTonnage(string tonnageFile)
        {
            this.tonnageFile = new OleDbConnection($@"Provider = Microsoft.ACE.OLEDB.12.0;Data Source = '{tonnageFile}' ;Extended Properties='Excel 12.0 Xml;HDR=YES'");
        }

        public void Dispose()
        {
            this.tonnageFile.Close();
            this.tonnageFile.Dispose();
        }

        //TODO: Create class for InterpolatedTable
        public Dictionary<int, double> GenerateInterpolatedTables(int tankNumber)
        {
            Dictionary<int, double> tonnageTable = null;
            StreamWriter loadLog = null;
            try
            {
                tonnageTable = new Dictionary<int, double>();
                tonnageFile.Open();
                OleDbCommand Cmd = new OleDbCommand();
                Cmd.Connection = tonnageFile;
                Cmd.CommandType = CommandType.Text;
                loadLog = File.CreateText($"{Logging.LOG_DIR}Tonnage_TQ_{tankNumber}_{DateTime.Today.ToString("dd-MM-yyyy")}.txt");

                Cmd.CommandText = "select * from[Planilha" + tankNumber + "$]"; // variação por tanque 
                loadLog.WriteLine($"LOADING TONNAGE TABLE {DateTime.Today.ToString("dd-MM-yyyy")}");
                using (OleDbDataReader Reader = Cmd.ExecuteReader())
                {
                    int key = 1;
                    double valorLinhaAanterior = 0;
                    double valorCmAnterior = 0;
                    while (Reader.Read())
                    {
                        double value = 0;
                        double volumeInicial = 0;

                        if (!Reader.IsDBNull(13))
                            volumeInicial = Convert.ToDouble(Reader.GetValue(13));


                        double[] mmValue = new double[]
                            {
                            (double)Reader.GetValue(4),
                            (double)Reader.GetValue(5),
                            (double)Reader.GetValue(6),
                            (double)Reader.GetValue(7),
                            (double)Reader.GetValue(8),
                            (double)Reader.GetValue(9),
                            (double)Reader.GetValue(10),
                            (double)Reader.GetValue(11),
                            (double)Reader.GetValue(12),
                            (double)Reader.GetValue(3)
                            };

                        double nivelInicial = (double)Reader.GetValue(1);
                        double nivelFinal = (double)Reader.GetValue(2);
                        int i = 0;
                        int vezes = 0;
                        int j = 0;
                        double cm = 0;
                        while (i < (10 * (nivelFinal - nivelInicial)))
                        {
                            if (j == 10)
                            {
                                vezes++;
                                j = 0;
                            }
                            value = volumeInicial + (mmValue[j]) + (mmValue[9] * vezes + valorLinhaAanterior);

                            tonnageTable.Add(key, value);
                            cm = (double)(i + 1) / 10 + valorCmAnterior;
                            loadLog.WriteLine(cm + ";" + value);

                            key++;
                            i++;
                            j++;
                        }
                        valorLinhaAanterior = value;
                        valorCmAnterior = cm;
                    }
                    Reader.Close();
                }
                loadLog.Close();
                tonnageFile.Close();
            }
            catch (Exception e)
            {
                Logging.Error("ON LOAD TONNAGE TABLE E:" + e.Message, "EnrafTonnageFileManager");
                tonnageTable = null;
            }
            finally
            {
                if (loadLog != null)
                    loadLog.Close();
                tonnageFile.Close();
            }
            return tonnageTable;
        }

        public List<dynamic> ShowInterpolatedTable(int tankNumber)
        {
            try
            {
                tonnageFile.Open();
                OleDbCommand Cmd = new OleDbCommand();
                Cmd.Connection = tonnageFile;
                Cmd.CommandType = CommandType.Text;

                Cmd.CommandText = "select * from[Planilha" + tankNumber + "$]";
                List<dynamic> table = new List<dynamic>();
                using (OleDbDataReader Reader = Cmd.ExecuteReader())
                {
                    while (Reader.Read())
                    {

                        dynamic row = new System.Dynamic.ExpandoObject();
                        row.anel = Reader.GetValue(0);
                        row.inicial = Reader.GetValue(1);
                        row.final = Reader.GetValue(2);
                        row.fator = Reader.GetValue(3);
                        row.mm1 = Reader.GetValue(4);
                        row.mm2 = Reader.GetValue(5);
                        row.mm3 = Reader.GetValue(6);
                        row.mm4 = Reader.GetValue(7);
                        row.mm5 = Reader.GetValue(8);
                        row.mm6 = Reader.GetValue(9);
                        row.mm7 = Reader.GetValue(10);
                        row.mm8 = Reader.GetValue(11);
                        row.mm9 = Reader.GetValue(12);
                        table.Add(row);
                    }
                    Reader.Close();
                }
                return table;
            }
            catch (Exception e)
            {
                Logging.Error("ON SHOW TONNAGE TABLE E:" + e.Message, "EnrafTonnageFileManager");
                return null;
            }
            finally
            {
                tonnageFile.Close();
            }
        }
    }
}
