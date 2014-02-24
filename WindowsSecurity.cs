using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;

namespace HostsManager
{
    public static class WindowsSecurity
    {
        public static bool HasAdministratorRights()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return pricipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static bool NeedsAdministratorRights()
        {
            var permissionSet = new PermissionSet(PermissionState.None);    
            var writePermission = new FileIOPermission(FileIOPermissionAccess.Write, HostsFileManager.Filename);
            permissionSet.AddPermission(writePermission);

            return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
        }

        public static bool RunElevated()
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.Verb = "runas";
            processInfo.FileName = Assembly.GetExecutingAssembly().Location;

            try
            {
                Process.Start(processInfo);
                return true;
            }
            catch (Win32Exception) { }

            return false;
        }
    }
}
