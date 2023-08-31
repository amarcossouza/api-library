using PHD_TAS_LIB.util;
using System;

namespace PHD_TAS_LIB.multiload
{
    public enum MLRegisters
    {
        PRESET_STATE = 4000,

        INPUT_KEY = 2850,
        PROCESSING_MODE = 1024,
        DEVICE_STATE = 7000,
        MODBUS_EXTENDED = 9000,
        FPOWER_UP = 7012,
        HOST_UP = 7013,
        HOST_DOWN = 7014,
        RESET_RCU_DISPLAY = 7017, //reseta a tela da rcu

        PROMPT_READ = 2851,

        VOLUME_PRESETED = 4036,
        VOLUME_LOADED = 4060,
        VOLUME_LOADED_20 = 4084,

        VOLUME_LOADED_COMP = 4372,
        VOLUME_LOADED_COMP_20 = 4564,

        FLOW_RATE = 4180,
        FLOW_RATE_COMP = 4312,

        AVERAGE_DENSITY_COMP = 5140,
        AVERAGE_DENSITY = 4156,

        // +p*16+a
        ADDITIVE_LOADED = 5908,

        TEMP_COMP = 4756,
        TEMP_PRESET = 4108,
        TOTALIZER = 3144,
        TOTALIZER_20 = 3744,

        AUTHORIZE_PRESET = 7500,

        TRANSACTION_VIEW = 7016,

        AUTH_TRANSACTION = 7032,

        END_TRANSACTION = 7044,
        BATCH_COMPLETE = 7045,
        PRESET_BATCH_END = 7057,
        END_BATCH = 7069,

        BIT_ALARM_DEVICE = 7094,
        BIT_ALARM_PRESET = 4024, // mapa de bits de evento de alarme no preset
        BIT_ALARM_METER = 7191,
        BIT_ALARM_COMPONENT = 7095, // mapa de bits de evento de alarme no componente
        BIT_ALARM_ADITIVE = 7251,

        INDEX_ALARM_DEVICE = 2500, // ponteiro de índice para pegar nome de alarme bay
        INDEX_ALARM_PRESET = 2520,
        INDEX_ALARM_METER = 2540,
        INDEX_ALARM_COMPONENTS = 2560,
        INDEX_ALARM_ADITIVE = 2580,

        ASCII_ALARM_DEVICE = 2502,
        ASCII_ALARM_PRESET = 2522,
        ASCII_ALARM_METER = 2542,
        ASCII_ALARM_COMPONENTS = 2562,
        ASCII_ALARM_ADITIVE = 2582,

        PRESET_INDEX_COMPONENTS = 8010,
        PRESET_INDEX_ADDITIVE = 8018,
        PRESET_INDEX_PRODUCTS = 8034,

        PRODUCT_DEFINITION_POINTER = 2200,
        PRODUCT_CODE = 2202,
        PRODUCT_NAME = 2233,
        COMPONENT_INDEX = 2243,
        COMPONENT_PERCENT = 2244,
        ADDITIVE_INDEX = 2259,
        ADDITIVE_PERCENT = 2260,

        PRESET_INDEX_FOR_CONFIGURATIONS =996, // PONTEIRO PARA PEGAR CONFIGURAÇÕES EM UM DETERMINADO PRESET 0-11
        COMPONENT_INDEX_FOR_CONFIGURATIONS =997, // PONTEIRO PARA PEGAR CONFIGURAÇÕES DE UM COMPONENTE EM UM DETERMINADO PRESET 0-7
        METER_INDEX_FOR_CONFIGURATIONS = 998, // PONTEIRO PARA PEGAR CONFIGURAÇÕES DE UM MEDIDOR EM UM DETERMINADO PRESET 0-4
        ADITIVE_INDEX_FOR_CONFIGURATIONS = 999, // PONTEIRO PARA PEGAR CONFIGURAÇÕES DE UM ADITIVO EM UM DETERMINADO PRESET 0-15

        METER_FACTOR_1 = 1970,
        METER_FACTOR_1_RATE = 1972,
        METER_FACTOR_2 = 1974,
        METER_FACTOR_2_RATE = 1976,
        METER_FACTOR_3 = 1978,
        METER_FACTOR_3_RATE = 1980,
        METER_FACTOR_4 = 1982,
        METER_FACTOR_4_RATE = 1984,
        METER_FACTORS_USED = 1986

    }


    public enum MLPresetRegisters
    {

    }

    public static class MultiLoadRegistersExtensions
    {
        //public static int read(this MLRegisters r, ModbusInterface i)
        //{
        //    return i.leituraInt((int)r);
        //}

        //4036 + 2 * p
        public static int preset(this MLRegisters r, int preset)
        {
            return (int)r + 2 * preset;
        }

        // 4312 + p
        public static int statePreset(this MLRegisters r, int preset)
        {
            return (int)r + preset;
        }

        // 4312 + p
        public static int flowRatePreset(this MLRegisters r, int preset = 0)
        {
            return (int)r + preset;
        }

        // 4312 + p
        public static int forPreset(this MLRegisters r, int preset = 0)
        {
            return (int)r + preset;
        }

        public static int forIndex(this MLRegisters r, int comp)
        {
            return (int)r + 2 * comp;
        }

        //2 * (p * 16) + a
        public static int forAdditive(this MLRegisters r, int preset = 0, int additive = 0)
        {
            return (int)r + 2 * (16 * preset + additive);
        }

        // 4312 + 5 * p
        public static int flowRateComponent(this MLRegisters r, int comp, int preset = 0)
        {
            return (int)r + 5 * preset + comp;
        }

        public static int authPreset(this MLRegisters r, int preset = 0)
        {
            return (int)r + 5 * preset;
        }

        // 4372 + 2 * (8 * p) + c
        public static int loadedPreset(this MLRegisters r, int preset, int comp = 0)
        {
            return (int)r + 2 * (8 * preset + comp);
        }

        public static int totalizerPreset(this MLRegisters r, int preset, int comp = 0)
        {
            return (int)r + 2 * (8 * preset + comp);
        }

        public static int temperaturePreset(this MLRegisters r, int preset, int comp = 0)
        {
            return (int)r + 2 * (8 * preset + comp);
        }

        public static int forIndex(this MLRegisters r, int preset = 0, int indexInPreset = 0)
        {
            return (int)r + 67 * preset + indexInPreset;
        }
    }

    public enum MLProcessingMode
    {
        LOCAL = 0,
        REMOTE = 1,
        BAD_READ = -1
    }

    public enum MLInputKey
    {
        NONE = 0,
        EMPTY = ' ',
        NEXT = 'A',
        PREV = 'B',
        EXIT = 'C',
        ENTER = 'D',
        CLEAR = 'E',
        BAD_READ = -1
    }

    public enum MLHostStatus
    {
        IDLE = '0',
        AUTH_BAY = '4',
        MENU_MODE = '9',
        DIAG_MODE = '%',
        AUTHORIZING_LOAD = 'A',
        LOAD_AUTHORIZED = 'B',
        COMPLETING_LOAD = 'C',
        TRANSACTION_DONE = 'D',
        TRANSACTION_CANCEL = 'E',
        PULLING_TRANSACTION = 'P',
        ARCHIVING_TRANSACTION = 'R',
        TRANSACTION_AUTHORIZED = 'T',
        RCU_NOT_CONFIGURED = '?',
        RCU_POWER_UP = '!',
        INITIALIZING = 'I',
        NO_TRANSACTION = 'N',
        REMOTE_AUTH_PRESET1 = 'a',
        REMOTE_AUTH_PRESET2 = 'b',
        REMOTE_AUTH_PRESET3 = 'c',
        REMOTE_AUTH_PRESET4 = 'd',
        REMOTE_AUTH_PRESET5 = 'e',
        REMOTE_AUTH_PRESET6 = 'f',
        REMOTE_AUTH_PRESET7 = 'g',
        REMOTE_AUTH_PRESET8 = 'h',
        REMOTE_AUTH_PRESET9 = 'i',
        REMOTE_AUTH_PRESET10 = 'j',
        REMOTE_AUTH_PRESET11 = 'k',
        REMOTE_AUTH_PRESET12 = 'l',
        REMOTE_AUTH_PRESET13 = 'm',
        REMOTE_AUTH_PRESET14 = 'n',
        NO_COMMUNICATION = 0
    }

    public sealed class MLPresetState
    {
        public static readonly MLPresetState BAD_READ = new MLPresetState(-1, "ERRO DE LEITURA");
        public static readonly MLPresetState IDLE = new MLPresetState(0, "INATIVO");
        public static readonly MLPresetState LOW_FLOW = new MLPresetState(1, "VAZÃO BAIXA");
        public static readonly MLPresetState HIGH_FLOW = new MLPresetState(2, "VAZÃO ALTA");
        public static readonly MLPresetState FIRST_TRIP = new MLPresetState(3, "PRESET 1ST TRIP");
        public static readonly MLPresetState SECOND_TRIP = new MLPresetState(4, "PRESET 2ND TRIP");
        public static readonly MLPresetState FINAL_TRIP = new MLPresetState(5, "PRESET FINAL TRIP");
        public static readonly MLPresetState START = new MLPresetState(6, "INÍCIO");
        public static readonly MLPresetState ALARM = new MLPresetState(7, "EM ALARME");
        public static readonly MLPresetState COMPLETE = new MLPresetState(8, "COMPLETO");
        public static readonly MLPresetState NOT_AUTH = new MLPresetState(9, "NÃO AUTORIZADO");
        public static readonly MLPresetState WAIT_TMS = new MLPresetState(10, "AGUARDANDO TMS");
        public static readonly MLPresetState AUTH = new MLPresetState(11, "AUTORIZADO");
        public static readonly MLPresetState PRESET = new MLPresetState(12, "PRESET");
        public static readonly MLPresetState DISABLED = new MLPresetState(13, "DESABILITADO");
        public static readonly MLPresetState STOP = new MLPresetState(14, "PARADO");
        public static readonly MLPresetState REMOTE_MSG = new MLPresetState(15, "MENSAGEM REMOTA");
        public static readonly MLPresetState END_OF_BATCH = new MLPresetState(16, "FINALIZANDO");
        public static readonly MLPresetState ARCHIVING = new MLPresetState(17, "FINALIZANDO");
        public static readonly MLPresetState CLEARING = new MLPresetState(18, "LIMPANDO DATA");
        public static readonly MLPresetState TRANS_DONE = new MLPresetState(19, "TRANSAÇÃO FINALIZADA");

        private readonly string name;
        private readonly int value;

        private MLPresetState(int value, string name)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }

        public int getOrdinal()
        {
            return value;
        }

        public bool isRunning => value > 0 && value <= 6;

        public bool isInAlarm => value == 7 || value == 14;

        public static MLPresetState of(int ordinal)
        {
            foreach (MLPresetState status in values())
            {
                if (ordinal == status.getOrdinal())
                    return status;
            }
            throw new ArgumentException("O ordinal não representa nenhum MultLoadStatus valido.");
        }

        public static MLPresetState of(string name)
        {
            foreach (MLPresetState status in values())
            {
                if (name == status.ToString())
                    return status;
            }
            throw new ArgumentException("O nome não representa nenhum MultLoadStatus valido.");
        }

        public static MLPresetState[] values()
        {
            return new MLPresetState[] {BAD_READ, IDLE, LOW_FLOW, HIGH_FLOW, FIRST_TRIP,
                SECOND_TRIP, FINAL_TRIP, START, ALARM, COMPLETE,
                NOT_AUTH, WAIT_TMS, AUTH, PRESET, DISABLED,
                STOP, REMOTE_MSG, END_OF_BATCH, ARCHIVING, CLEARING, TRANS_DONE };
        }

        public override bool Equals(object obj)
        {
            var state = obj as MLPresetState;
            return state != null &&
                   value == state.value;
        }

        public override int GetHashCode()
        {
            return value;
        }

        // use enum ao inves de int
        //public enum Values
        //{
        //    IDLE = 0, LOW_FLOW, HIGH_FLOW, FIRST_TRIP,
        //    SECOND_TRIP, FINAL_TRIP, START, ALARM, COMPLETE,
        //    NOT_AUTH, WAIT_TMS, AUTH, PRESET, DISABLED,
        //    STOP, REMOTE_MSG, END_OF_BATCH, ARCHIVING, CLEARING, TRANS_DONE3
        //    public static readonly MultLoadStatus IDLE = new MultLoadStatus(Values.IDLE, "INATIVO");
        //}
    }

    public static class MLPrompt
    {
        public static ushort[] clearScreen { private set; get; } = new ushort[] { 3, 84, 27, 88 };

        public static ushort[] ErroPedido { private set; get; } =
            ASCIIConverter.AsciiWritePromptML(new ushort[] { 22, 84 }, "Pedido Invalido", new ushort[] { 27, 76, 48, 48, 27, 69 });
        public static ushort[] ErroProduto { private set; get; } =
            ASCIIConverter.AsciiWritePromptML(new ushort[] { 23, 84 }, "Produto Invalido", new ushort[] { 27, 76, 48, 48, 27, 69 });

        public static ushort[] DescargaCompleta { private set; get; } =
            ASCIIConverter.AsciiWritePromptML(new ushort[] { 43, 84 }, "Descarga completa.", new ushort[] { 27, 76, 48, 48, 27, 69 });
        public static ushort[] DescargaOnline { private set; get; } =
            ASCIIConverter.AsciiWritePromptML(new ushort[] { 43, 84 }, "Descarga em andamento.", new ushort[] { 27, 76, 48, 48, 27, 69 });

        public static ushort[] CompartimentoOnline { private set; get; } =
            ASCIIConverter.AsciiWritePromptML(new ushort[] { 43, 84 }, "Compartimento sendo utilizado.", new ushort[] { 27, 76, 48, 48, 27, 69 });

        public static ushort[] CompartimentoCompleto { private set; get; } =
            ASCIIConverter.AsciiWritePromptML(new ushort[] { 43, 84 }, "Compartimento carregado.", new ushort[] { 27, 76, 48, 48, 27, 69 });

        public static ushort[] ErroLeituraCodigo { private set; get; } =
           ASCIIConverter.AsciiWritePromptML(new ushort[] { 43, 84 }, "Erro ao inserir dados", new ushort[] { 27, 76, 48, 48, 27, 69 });

        public static ushort[] enterCodigo { private set; get; } =
           ASCIIConverter.AsciiWritePromptML(new ushort[] { 24, 84 }, "Informe o Codigo:", new ushort[] { 27, 76, 48, 57, 27, 69 });

        public static ushort[] ErroCodigo { private set; get; } =
            ASCIIConverter.AsciiWritePromptML(new ushort[] { 22, 84 }, "Codigo Invalido", new ushort[] { 27, 76, 48, 48, 27, 69 });
        public static ushort[] enterCompart { private set; get; } =
            ASCIIConverter.AsciiWritePromptML(new ushort[] { 28, 84 }, "Entre compartimento: ", new ushort[] { 27, 76, 48, 50, 27, 69 });
        public static ushort[] erroCompart { private set; get; } =
            ASCIIConverter.AsciiWritePromptML(new ushort[] { 29, 84 }, "Compartimento Invalido", new ushort[] { 27, 76, 48, 48, 27, 69 });
        public static ushort[] exitPedido { private set; get; } =
            ASCIIConverter.AsciiWritePromptML(new ushort[] { 43, 84 }, "Deseja finalizar agora? 1- SIM 2-NAO", new ushort[] { 27, 76, 48, 49, 27, 69 });
        public static ushort[] clearPrompt { private set; get; } =
            ASCIIConverter.AsciiWritePromptML(new ushort[] { 7, 84 }, "", new ushort[] { 27, 76, 48, 48, 27, 69 });
    }
}
