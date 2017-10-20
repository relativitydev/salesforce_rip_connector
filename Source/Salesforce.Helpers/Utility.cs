using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services.Protocols;
using Salesforce.Helpers.sForceService;
using System.Net;
using kCura.Crypto;
using kCura.IntegrationPoints.Contracts.Models;
using Newtonsoft.Json;
using Salesforce.Helpers.Interfaces;
using Salesforce.Helpers.Models;

namespace Salesforce.Helpers
{
    public class Utility : IUtility 
    {
        #region Fields

        public String name { get; set; } = String.Empty;
        public String label { get; set; } = String.Empty;
        public bool identifier { get; set; } = false;

        #endregion
        private Salesforce.Helpers.sForceService.SforceService _binding { get; set; } = null;

        private string sfRefSubOject(Salesforce.Helpers.sForceService.SforceService binding, string fieldname, string fieldvalue, string sfobject)
        {
            string retval = null;
            QueryResult qr;
            string soqlQuery;
            sObject[] records=null;

            if (sfobject == Constants.ReferenceObject.CASE)
            {
                soqlQuery = Constants.SqlCommand.SELECT + Constants.SqlCommand.CASENUMBER + Constants.SqlCommand.FROM_CASE_WHERE + " ID " + Constants.SqlCommand.EQUAL + Constants.SqlCommand.SINGLE_QUOTE + fieldvalue + Constants.SqlCommand.SINGLE_QUOTE;
                qr = binding.query(soqlQuery);
                records = qr.records;

            }
            else if(sfobject == Constants.ReferenceObject.USER)
            {
                soqlQuery = Constants.SqlCommand.SELECT + Constants.SqlCommand.NAME + Constants.SqlCommand.FROM_USER_WHERE + " ID " + Constants.SqlCommand.EQUAL + Constants.SqlCommand.SINGLE_QUOTE + fieldvalue + Constants.SqlCommand.SINGLE_QUOTE;
                qr = binding.query(soqlQuery);
                records = qr.records;

            }
            else if (sfobject == Constants.ReferenceObject.USERGROUP)
            {
                soqlQuery = Constants.SqlCommand.SELECT + Constants.SqlCommand.NAME + Constants.SqlCommand.FROM_USER_WHERE + " ID "  + Constants.SqlCommand.EQUAL + Constants.SqlCommand.SINGLE_QUOTE + fieldvalue + Constants.SqlCommand.SINGLE_QUOTE;
                qr = binding.query(soqlQuery);
                if (qr.records == null)
                {
                    soqlQuery = Constants.SqlCommand.SELECT + Constants.SqlCommand.NAME + Constants.SqlCommand.FROM_GROUP_WHERE + " ID " + Constants.SqlCommand.EQUAL + Constants.SqlCommand.SINGLE_QUOTE + fieldvalue + Constants.SqlCommand.SINGLE_QUOTE;
                    qr = binding.query(soqlQuery);
                }
                records = qr.records;

            }
            if (records != null)
            {
                if ((records[0] != null) && (records[0].Any != null))
                    retval = records[0].Any[0].InnerText;
                else
                    retval = "";
            }
            else
                retval = "";


                return retval;
        }
        private DataTable GenerateDataTable()
        {
            var retVal = new DataTable();
            retVal.Columns.Add(new DataColumn(Constants.NAME) { DataType = name.GetType() });
            retVal.Columns.Add(new DataColumn(Constants.LABEL) { DataType = label.GetType() });
            retVal.Columns.Add(new DataColumn(Constants.IDENTIFIER) { DataType = identifier.GetType() });



            return retVal;
        }

        private DataTable GenerateIDDataTable()
        {
            var retVal = new DataTable();
            retVal.Columns.Add(new DataColumn(Constants.NAME) { DataType = name.GetType() });

            return retVal;
        }
        private Salesforce.Helpers.sForceService.SforceService sfLogin(String sfUserID, String sfUserPwd)
        {

            bool reqlogin = true;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            Salesforce.Helpers.sForceService.SforceService binding;

            // Create a service object if needed
            if (_binding == null)
            {
                binding = new SforceService();
                reqlogin = true;
            }
            else
            {
                reqlogin = false;
                binding = _binding;
                try
                {
                    GetUserInfoResult result = binding.getUserInfo();
                    
                }
                catch (SoapException uef)
                {
   //                 if (uef.ExceptionCode == ExceptionCode.INVALID_SESSION_ID)
                    {
                        reqlogin = true;
                    }
                }


            }
            if (reqlogin)
            {
                LoginResult lr;
                lr = binding.login(sfUserID, sfUserPwd);
                /** 
                    * The login results contain the endpoint of the virtual server instance 
                    * that is servicing your organization. Set the URL of the binding 
                    * to this endpoint.
                    */
                // Save old authentication end point URL
                String authEndPoint = binding.Url;
                // Set returned service endpoint URL
                binding.Url = lr.serverUrl;

                /** Get the session ID from the login result and set it for the 
                   * session header that will be used for all subsequent calls.
                   */
                binding.SessionHeaderValue = new SessionHeader();
                binding.SessionHeaderValue.sessionId = lr.sessionId;
                _binding = binding;
            }
            return (binding);
        }
        public DataTable salesforceQueryData(IEnumerable<FieldEntry> fields, IEnumerable<string> entryIds, String sfUserID, String sfUserPwd, String startDate)
        {
            DataTable dt;
            string controlID = null;
            string fieldvalue = null;
            string fieldname = null;
            dt = new DataTable();

            Salesforce.Helpers.sForceService.SforceService binding = sfLogin( sfUserID, sfUserPwd);



            DataTable dtFields = GenerateIDDataTable();

            try
            {
                QueryResult qr = null;
                binding.QueryOptionsValue = new Salesforce.Helpers.sForceService.QueryOptions();
                binding.QueryOptionsValue.batchSize = 250;
                binding.QueryOptionsValue.batchSizeSpecified = true;

                String soqlQuery = Constants.SqlCommand.SELECT;
                bool firstentry = true;
                int countfields = 0;
                foreach (var field in fields)
                {
                    if(field.IsIdentifier)
                    {
                        controlID = field.FieldIdentifier;
                    }
                    countfields++;
                    if (firstentry)
                    {
                        soqlQuery += field.FieldIdentifier;
                        firstentry = false;
                    }
                    else
                    {
                        soqlQuery += Constants.SqlCommand.COMMA_SPACE + field.FieldIdentifier;

                    }


                    dt.Columns.Add(new DataColumn((String)field.FieldIdentifier));

                }
                soqlQuery += Constants.SqlCommand.FROM_CASE_WHERE;

                String soqlQueryID = soqlQuery;
                bool done = false;
                foreach (var entryID in entryIds)
                {
                    soqlQueryID += controlID + Constants.SqlCommand.EQUAL + Constants.SqlCommand.SINGLE_QUOTE + entryID + Constants.SqlCommand.SINGLE_QUOTE;
                    qr = binding.query(soqlQueryID);


                    done = false;
                    while (!done)
                    {
                        sObject[] records = qr.records;
                        if (records != null)
                        {
                            for (int i = 0; i < records.Length; i++)
                            {
                                var row  = dt.NewRow();

                                Salesforce.Helpers.sForceService.sObject con = qr.records[i];
                                sObject m = records[i];

                                for (int j= 0; j < countfields; j++)
                                {
                                    fieldname = m.Any[j].LocalName;
                                    fieldvalue = m.Any[j].InnerText;
                                    if (fieldname == Constants.ReferenceObject.OWNERID)
                                    {
                                        fieldvalue = sfRefSubOject(binding, fieldname, fieldvalue, Constants.ReferenceObject.USERGROUP);
                                    }
                                    else if(fieldname == Constants.ReferenceObject.PARENTID)
                                    {
                                        fieldvalue = sfRefSubOject(binding, fieldname, fieldvalue, Constants.ReferenceObject.CASE);

                                    }
                                    else if (fieldname == Constants.ReferenceObject.LASTMODIFIED_BY_ID)
                                    {
                                        fieldvalue = sfRefSubOject(binding, fieldname, fieldvalue, Constants.ReferenceObject.USER);

                                    }
                                  
                                    row[fieldname] = fieldvalue;
                                  
                               }


                                dt.Rows.Add(row);

                            }
                            if (qr.done)
                            {
                                done = true;
                                soqlQueryID = soqlQuery;
                            }
                            else
                                qr = binding.queryMore(qr.queryLocator);
                        }
                        else
                            done = true;
                    }

                }
                //          String soqlQuery = "SELECT CaseNumber FROM Case WHERE Date_Opened__c >  '2015-10-08'";



            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " + e.Message +
                    " Stack trace: " + e.StackTrace);
            }




            return dt;

        }

        public DataTable salesforceQueryID(FieldEntry identifier, String sfUserID, String sfUserPwd, String startDate)
        {

            Salesforce.Helpers.sForceService.SforceService binding = sfLogin(sfUserID, sfUserPwd);


 

            DataTable dtFields = GenerateIDDataTable();

            try
            {
                QueryResult qr = null;
                binding.QueryOptionsValue = new Salesforce.Helpers.sForceService.QueryOptions();
                binding.QueryOptionsValue.batchSize = 250;
                binding.QueryOptionsValue.batchSizeSpecified = true;


                //String soqlQuery = "SELECT CaseNumber FROM Case WHERE CreatedDate >  " + startDate + ""T00:00:00Z"";
                String soqlQuery = "SELECT " + identifier.FieldIdentifier + Constants.SqlCommand.START_DATE + startDate + Constants.SqlCommand.TIME;

                // Make the query call and get the query results

                qr = binding.query(soqlQuery);



                bool done = false;

                while (!done)
                {
                    sObject[] records = qr.records;
                    if (records != null)
                    {
                        for (int i = 0; i < records.Length; i++)
                        {

                            Salesforce.Helpers.sForceService.sObject con = qr.records[i];
                            sObject m = records[i];

                            string ticket = m.Any[0].InnerText;
                            dtFields.Rows.Add(ticket);
                        }

                        if (qr.done)
                            done = true;
                        else
                            qr = binding.queryMore(qr.queryLocator);
                    }
                    else
                        done = true;
                }
            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " + e.Message +
                    " Stack trace: " + e.StackTrace);
            }




            return dtFields;

        }
        public DataTable salesforceBind(String sfUserID, String sfUserPwd)
        {
            Salesforce.Helpers.sForceService.SforceService binding = sfLogin(sfUserID, sfUserPwd);

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            
            DataTable dtFields = describeSObjectsSample(binding);
            return dtFields;
        }


        public DataTable describeSObjectsSample(Salesforce.Helpers.sForceService.SforceService binding)
        {
            var dt = GenerateDataTable();

            try
            {
                // Call describeSObjectResults and pass it an array with
                // the names of the objects to describe.
                DescribeSObjectResult[] describeSObjectResults =
                                    binding.describeSObjects(
                                    new string[] { Constants.CASE });

                // Iterate through the list of describe sObject results
                foreach (DescribeSObjectResult describeSObjectResult in describeSObjectResults)
                {
                    // Get the name of the sObject
                    String objectName = describeSObjectResult.name;

                    // For each described sObject, get the fields
                    Field[] fields = describeSObjectResult.fields;


                    // Iterate through the fields to get properties for each field
                    foreach (Field field in fields)
                    {

                        // field.label for CaseNumber is Ticket Number - needs to match GetData table values
                        dt.Rows.Add(field.label, field.name,  field.idLookup);



                    }
                }
            }
            catch (SoapException e)
            {
                Console.WriteLine("An unexpected error has occurred: " + e.Message
                    + "\n" + e.StackTrace);
            }
            return dt;
        }

        public JobConfiguration DecryptJobConfiguration(String options)
        {
            var encryptedModel = JsonConvert.DeserializeObject<SecurityResponse>(options);
            options = Decrypt(encryptedModel.Value);
            return JsonConvert.DeserializeObject<JobConfiguration>(Uri.UnescapeDataString(options));
        }

        public T DeserializeObjectAsync<T>(String metaData)
        {
            return JsonConvert.DeserializeObject<T>(
                value: metaData,
                settings: new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                }
            );
        }

        public String SerializeObjectAsync<T>(T obj)
        {
            return JsonConvert.SerializeObject(
                value: obj,
                formatting: Formatting.None,
                settings: new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
                }
            );
        }

        public string Decrypt(string encryptedText)
        {
            return TripleDESEncryptor.Decrypt(encryptedText, Helpers.Constants.EncryptionKey.PLEASE_UPDATE_THIS_KEY);
        }

        public string Encrypt(string text)
        {
            return TripleDESEncryptor.Encrypt(text, Helpers.Constants.EncryptionKey.PLEASE_UPDATE_THIS_KEY);
        }
    }
}
