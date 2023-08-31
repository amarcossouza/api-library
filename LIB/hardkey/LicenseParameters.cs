using System;
using System.Runtime.InteropServices;

namespace PHD_TAS_LIB.hardkey
{
    [Serializable()]
    public struct LicenseParameters
    {
        public char Enable;
        public char SingleInstance;
        public char Netusers;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] ExpireDate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public char[] Counter;
        public char ID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public char[] Reserved;
    }

    public enum LicenseType
    {
        LOCAL_REDE = 0,
        LOCAL = 1,
        REDE = 2
    }

    public enum ErrorCode
    {
        READ_KEY_ERROR = -1,
        OTP_INVALID = -2801
    }
}