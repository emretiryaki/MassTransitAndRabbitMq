using System.Configuration.Install;
using System.Reflection;

namespace Core.Container.ServiceInstaller
{
    public static class SelfInstaller
    {
        private static readonly string exePath = Assembly.GetExecutingAssembly().Location;

        public static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { "/u", exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
