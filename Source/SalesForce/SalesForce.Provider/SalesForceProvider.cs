using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using kCura.IntegrationPoints.Contracts.Models;
using kCura.IntegrationPoints.Contracts.Provider;
using Relativity.API;
using Salesforce.Helpers;
using Salesforce.Helpers.Interfaces;
using Newtonsoft.Json;
using Salesforce.Helpers.Models;

namespace SalesForce.Provider
{
    [kCura.IntegrationPoints.Contracts.DataSourceProvider(Constants.Guids.Provider.SALESFORCE_PROVIDER)]
    public class SalesForceProvider : IDataSourceProvider
    {
        public IHelper Helper;
        public IUtility Utility;

        public SalesForceProvider(IHelper helper)
        {
            Helper = helper;
            Utility = new Utility();
        }

        public IEnumerable<FieldEntry> GetFields(string options)
        {
            DataTable dt;
            var jobConfig = Utility.DecryptJobConfiguration(options);

            /* This method runs on the webserver while users are configuring their integration point.
             * It return a list of fields for users to map to Relativity objects*/
            var fieldEntries = new List<FieldEntry>();

            dt = Utility.salesforceBind(jobConfig.SalesforceUserID, jobConfig.SalesforceUserPwd);
            DataTableReader reader = dt.CreateDataReader();
            while (reader.Read())
            {
                    fieldEntries.Add(new FieldEntry { DisplayName = (string)reader[0], FieldIdentifier = (string)reader[1], IsIdentifier = (bool)reader[2] });

            }

            return fieldEntries;
        }

        public IDataReader GetData(IEnumerable<FieldEntry> fields, IEnumerable<string> entryIds, string options)
        {
            /* This method is executed on itegration points agents to import the data into Relativity
             * Query your data source from the IDs provided, format it with the columns entered in GetFields()
             * Lastly return it as a dataReader.*/
            var jobConfig = Utility.DecryptJobConfiguration(options);


            var dataSource = new DataTable();
            dataSource = Utility.salesforceQueryData(fields, entryIds, jobConfig.SalesforceUserID, jobConfig.SalesforceUserPwd, jobConfig.StartDate);

            return dataSource.CreateDataReader();
        }

        public IDataReader GetBatchableIds(FieldEntry identifier, string options)
        {
            /*Return all the IDs of the datasource in a datareader, The IDs will be passed to GetData
             * to allow the data to be processed in batches*/
            var jobConfig = Utility.DecryptJobConfiguration(options);

            var dataSource = new DataTable();
            dataSource = Utility.salesforceQueryID(identifier, jobConfig.SalesforceUserID, jobConfig.SalesforceUserPwd, jobConfig.StartDate);
  

            return dataSource.CreateDataReader();
            
        }

        

    }
}
