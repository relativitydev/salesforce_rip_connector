using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using kCura.IntegrationPoints.SourceProviderInstaller;

namespace SalesForce.Provider
{
    [kCura.EventHandler.CustomAttributes.Description("Salesforce Provider - Installer")]
    [kCura.EventHandler.CustomAttributes.RunOnce(false)]
    [Guid("4C15D5D9-973C-4FB6-BD37-0EAA93CAB03E")]
    public class RegisterSalesforceProvider : kCura.IntegrationPoints.SourceProviderInstaller.IntegrationPointSourceProviderInstaller
    {
        public override IDictionary<Guid, SourceProvider> GetSourceProviders()
        {
            var sourceProviders = new Dictionary<Guid, kCura.IntegrationPoints.SourceProviderInstaller.SourceProvider>();

            // Register the name, custom page location and configuration location of your provider
            var salesforceProvider = new kCura.IntegrationPoints.SourceProviderInstaller.SourceProvider()
            {
                Name = "Salesforce Provider",
                Url = $"/%applicationpath%/CustomPages/{Constants.Guids.Application.SFP_RELATIVITY_APPLICATION}/Provider/Index/",
                ViewDataUrl = $"/%applicationpath%/CustomPages/{Constants.Guids.Application.SFP_RELATIVITY_APPLICATION}/Provider/GetViewFields/"
            };

            sourceProviders.Add(new Guid(Constants.Guids.Provider.SALESFORCE_PROVIDER), salesforceProvider);

            return sourceProviders;
        }

        public void PreInstall()
        {
            // Execute a command before your provider is Uninstalled
            //this.Helper.GetDBContext(Helper.GetActiveCaseID()).ExecuteNonQuerySQLStatement("CREATE TABLE [EDDSDBO].MyScratchTableForMyCustomProvider ([ID] INT)");
            //this.Helper.GetDBContext(Helper.GetActiveCaseID()).ExecuteNonQuerySQLStatement("CREATE TABLE [EDDSDBO].MyJobTableForMyCustomProvider ([ID] INT)");
        }

        public void PostInstall(Boolean isUninstalled, Exception ex)
        {
            // Execute a command after your provider is Uninstalled
            //this.Helper.GetDBContext(Helper.GetActiveCaseID()).ExecuteNonQuerySQLStatement("INSERT INTO [EDDSDBO].MyScratchTableForMyCustomProvider(ID)VALUES(1)");
        }
    }
}
