using System;
using System.Collections.Generic;
using System.Data;
using kCura.IntegrationPoints.Contracts.Models;
using Salesforce.Helpers.Models;

namespace Salesforce.Helpers.Interfaces
{
    public interface IUtility
    {
               
            DataTable salesforceQueryData(IEnumerable<FieldEntry> fields, IEnumerable<string> entryIds, String sfUserID, String sfUserPwd, String startDate);
        DataTable  salesforceQueryID(FieldEntry identifier, String sfUserID, String sfUserPwd, String startDate);
        DataTable salesforceBind(String sfUserID, String sfUserPwd);
            T DeserializeObjectAsync<T>(String metaData);

            String SerializeObjectAsync<T>(T adminObject);
        string Decrypt(string encryptedText);
        string Encrypt(string text);
        JobConfiguration DecryptJobConfiguration(String options);


    }
}
