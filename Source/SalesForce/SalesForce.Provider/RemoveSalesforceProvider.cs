using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using kCura.EventHandler;
using kCura.IntegrationPoints.SourceProviderInstaller;

namespace SalesForce.Provider
{
    [kCura.EventHandler.CustomAttributes.Description("Salesforce Provider - Uninstall")]
    [kCura.EventHandler.CustomAttributes.RunOnce(false)]
    [Guid("D2A9E6E3-0343-45DF-8F3E-A73CAD514D9E")]
    public class RemoveSalesforceProvider : kCura.IntegrationPoints.SourceProviderInstaller.IntegrationPointSourceProviderUninstaller
    {
        public RemoveSalesforceProvider()
        {
            // Subscribe to Pre and Post UnInstall Events to cleanup environment after your provider is removed
            RaisePreUninstallPostExecuteEvent += PostUninstall;
            RaisePreUninstallPreExecuteEvent += PreUninstall;
        }

        public void PreUninstall()
        {
            // Execute a command before your provider is Uninstalled
            //this.Helper.GetDBContext(Helper.GetActiveCaseID()).ExecuteNonQuerySQLStatement("DROP TABLE [EDDSDBO].MyScratchTableForSalesforceProvider");
        }

        public void PostUninstall(Boolean isUninstalled, Exception ex)
        {
            // Execute a command after your provider is Uninstalled
            //this.Helper.GetDBContext(Helper.GetActiveCaseID()).ExecuteNonQuerySQLStatement("DROP TABLE [EDDSDBO].MyJobTableForSalesforceProvider");
        }
    }
}
