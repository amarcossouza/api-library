using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.entity.Enraf
{
    public class EnrafDriver
    {

        public byte[] buildZMessage(byte CIUAddress, byte GaugeAddressDec, byte GaugeAddressUnit, byte deviceId, byte requestCommand, byte requestCommand2)
        {
            byte[] message = new byte[10];
            message[1] = CIUAddress;
            message[2] = GaugeAddressDec;
            message[3] = GaugeAddressUnit;
            message[4] = deviceId;
            message[5] = 0x5A;
            message[6] = requestCommand;
            message[7] = requestCommand2;
            message[8] = 0x03;
            message[9] = Checkers.BCCSerialCalculation(message);
            message[0] = 0x02;
            return message;
        }

        public byte[] buildMessage(byte CIUAddress, byte GaugeAddressDec, byte GaugeAddressUnit, byte deviceId, byte requestCommand)
        {
            byte[] message = new byte[8];
            message[1] = CIUAddress;
            message[2] = GaugeAddressDec;
            message[3] = GaugeAddressUnit;
            message[4] = deviceId;
            message[5] = requestCommand;
            message[6] = 0x03;
            message[7] = Checkers.BCCSerialCalculation(message);
            message[0] = 0x02;
            return message;
        }

        ////TODO: change to static class enum field
        ////Pos. 7 : as alarm status  - Trocar por um Enum Selead Class
        //private readonly Dictionary<char, string> AlarmStatus = new Dictionary<char, string>()
        //{
        //    { 'F', "Erro leitura alarmes" },
        //    { 'C', "Limite chave motor" },
        //    { 'B', "'Block/Freeze'ativo" },
        //    { 'H', "Alarme nlv alto" },
        //    { 'L', "Alarme nvl baixo" },
        //    { '-', "OK" }
        //};

        ////TODO: change to static class enum field
        ////Pos. 8 : ls level status - Trocar por um Enum Selead Class
        //private readonly Dictionary<char, string> LevelStatus = new Dictionary<char, string>()
        //{
        //    { 'F', "Invalid level" },
        //    { 'C', "Limite chave motor" },
        //    { 'B', "'Block/Freeze'ativo" },
        //    { 'L', "'Teste/Calib'ativo" },
        //    { 'R', "Leitura de densidade ativa" },
        //    { 'T', "O Gauge está procurando por nível ou está em testes, comandos de 'teste de balanceamento' ou 'frequência de medição estão ativos" },
        //    { 'W', "Nvl de água encontrado" },
        //    { 'D', "Procur água" },
        //    { '-', "OK" }
        //};

        //////Pos. 16 : s temperature sign - Trocar por um Enum Selead Class
        ////private readonly Dictionary<char, string> TemperatureSign = new Dictionary<char, string>()
        ////{
        ////    { 'F', "Invalid temp" },
        ////    { '-', "-" },
        ////    { '+', "+" }
        ////};

        ////TODO: change to static class enum field
        //// Pos. 15 : ts temperature status - Trocar por um Enum Selead Class
        //private readonly Dictionary<char, string> TemperatureStatus = new Dictionary<char, string>()
        //{
        //    { 'F', "Invalid temp" },
        //    { 'T', "Possíveis eventos: 1 - current level below RTD position (TPU) invalid level reading 2 - device not calibrated (MTT only) 3 - out of specified temperature range (MRT or MTT) 4 - exceeding differential temperature range (MTT only) 5 - level below lowest temperature element (MRT or MTT) 6 - alternative element selected (MRT only) 7 - last valid level used (MRT or MTT) 8 - manual level used (MRT or MTT)"},
        //    { '-', "OK" }
        //};
    }

    public static class Checkers
    {
        public static byte BCCSerialCalculation(byte[] msg)
        {
            byte retorna = 0;
            for (int i = 1; i < msg.Length; i++)
            {
                retorna ^= msg[i];
            }
            return retorna;
        }

    }
}
