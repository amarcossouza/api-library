using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PHD_TAS_LIB.util
{
    public static class ServiceStatus
    {
        public static readonly string SERVICE_NAME = "PHD TAS Service";
        private static ServiceController serviceController;

        private static ServiceController GetController()
        {
            if (serviceController == null)
                serviceController = new ServiceController(SERVICE_NAME);
            else
                serviceController.Refresh();
            return serviceController;
        }

        public static ServiceControllerStatus Get()
        {
            return GetController().Status;
        }

        public static bool IsRunning() {

            return Get() == ServiceControllerStatus.Running;
        }

        public static void Stop()
        {
            ServiceController sc = GetController();
            if (sc.Status != ServiceControllerStatus.Stopped
                && sc.Status != ServiceControllerStatus.StopPending)
            {
                sc.Stop();
            }
        }

        public static void Start()
        {
            ServiceController sc = GetController();
            if (sc.Status != ServiceControllerStatus.Running
                && sc.Status != ServiceControllerStatus.StartPending)
            {
                sc.Start();
            }
        }
    }
}
