using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace PHD_TAS_LIB.hardkey
{
    public static class Hardkey
    {
        [DllImport("ClientAPI64.dll",
          EntryPoint = "NeoKeyCall",
          SetLastError = true,
          CallingConvention = CallingConvention.StdCall,
          ExactSpelling = true)]
        public static extern short NeoKeyCall(int cmd, byte[] param1, int size1, byte[] param2, int size2, byte[] param3, int size3, ref LicenseParameters param);

        [DllImport("ClientAPI64.dll",
          EntryPoint = "NeoKey_Login",
          SetLastError = true,
          CallingConvention = CallingConvention.StdCall,
          ExactSpelling = true)]
        public static extern short NeoKey_Login(byte[] message, int size, int licenseID, int Type, byte[] HardwareID);
        //unsigned char *message, unsigned int size,unsigned int licenseID, unsigned int Type, unsigned char* HardwareID

        [DllImport("ClientAPI64.dll",
          EntryPoint = "NeoKey_LoginOTP",
          SetLastError = true,
          CallingConvention = CallingConvention.StdCall,
          ExactSpelling = true)]
        public static extern short NeoKey_LoginOTP(char[] message, uint size, uint licenseID, uint Type, byte[] HardwareID);
        //unsigned char *message, unsigned int size,unsigned int licenseID, unsigned int Type, unsigned char* HardwareID

        [DllImport("ClientAPI64.dll",
          EntryPoint = "NeoKey_SincOTP",
          SetLastError = true,
          CallingConvention = CallingConvention.StdCall,
          ExactSpelling = true)]
        public static extern short NeoKey_SincOTP(byte[] Sinc, uint Type);
        //NeoKey_SincOTP(unsigned char *Sinc, unsigned int Type)

        [DllImport("ClientAPI64.dll",
          EntryPoint = "NeoKey_Logout",
          SetLastError = true,
          CallingConvention = CallingConvention.StdCall,
          ExactSpelling = true)]
        public static extern short NeoKey_Logout();

        [DllImport("ClientAPI64.dll",
          EntryPoint = "NeoKey_GetLicense",
          SetLastError = true,
          CallingConvention = CallingConvention.StdCall,
          ExactSpelling = true)]
        public static extern int NeoKey_GetLicense(char ID, byte[] Enable, byte[] SingleInstance, byte[] Netusers, byte[] ExpireDate, byte[] Counter);

        //public static LicenseParameters GetLicense() {


        //}

        public static int Login_OTP(int licenceID = 0, LicenseType type = LicenseType.LOCAL_REDE)
        {
            int neoKeyReturn = -1;
            byte[] syncbytes = new byte[8];
            byte[] hardwareID = new byte[8];
            neoKeyReturn = NeoKey_SincOTP(syncbytes, (uint)type);
            if (neoKeyReturn != 0)
            {
                return neoKeyReturn;
            }

            byte[] dta = new byte[28];
            Array.Copy(OTP.Key1, dta, 20);
            Array.Copy(syncbytes, 0, dta, 20, 8);

            byte[] dtc = new byte[28];
            Array.Copy(OTP.Key2, dtc, 20);
            Array.Copy(syncbytes, 0, dtc, 20, 8);

            System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            sha1.Initialize();

            byte[] passOTP = sha1.ComputeHash(dta);
            //neoKeyReturn = NeoKey_LoginOTP(passOTP, (uint)passOTP.Length, (uint)licenceID, (uint)type, hardwareID);
            LicenseParameters dev = default(LicenseParameters);
            neoKeyReturn = NeoKeyCall(51, passOTP, passOTP.Length, null, 0, null, 0, ref dev);
            if (neoKeyReturn != 0)
            {
                return neoKeyReturn;
            }

            byte[] checkOTP = sha1.ComputeHash(dtc);
            string a1 = string.Join("", checkOTP.Select(b => b.ToString("X2")).Take(16).ToArray());
            string a2 = string.Join("", passOTP.Select(b => b.ToString("X2")).Take(16).ToArray());
            if (!(a1).Equals(a2))
            {
                return -2801;
            }
            return neoKeyReturn;
        }
    }
}
