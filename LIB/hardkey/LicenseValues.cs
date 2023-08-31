using System;

namespace PHD_TAS_LIB.hardkey
{
    [Serializable()]
    public struct LicenseValues
    {
        public bool Enable;
        public bool SingleInstance;
        public bool Netusers;
        public DateTime ExpireDate;
        public int Counter;
        public char ID;

        public DateTime lastCheck;
        public bool active;

        public LicenseType type;
        public int lastCode;

        public bool isActive()
        {
            if (!active)
            {
                return false;
            }
            else
            {
                if (DateTime.Now.Subtract(lastCheck).TotalMinutes > 60) {
                    return false;
                };
                return active;
            }
        }
    }

}