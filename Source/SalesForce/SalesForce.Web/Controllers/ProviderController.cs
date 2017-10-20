using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using kCura.Relativity.Client;
using Newtonsoft.Json.Linq;
using Relativity.API;
using Salesforce.Helpers.Interfaces;
using Salesforce.Helpers;
using SalesForce.Web.Models;


namespace SalesForce.Web.Controllers
{
    public class ProviderController : Controller
    {
        public IUtility Utility;

        public ProviderController()
        {
            Utility = new Utility();
        }
        // GET: Provider
        public ActionResult Index()
        {
            // Example demonstrates the creation of the RSAPI client and use of the logger
   
            var logger = Relativity.CustomPages.ConnectionHelper.Helper().GetLoggerFactory().GetLogger();
            //try
            //{
                using (var rsapiClient = Relativity.CustomPages.ConnectionHelper.Helper().GetServicesManager().CreateProxy<IRSAPIClient>(ExecutionIdentity.System))
                {
                    var randomRDOArtifactID = 123123;
                    rsapiClient.APIOptions = new APIOptions()
                    {
                        WorkspaceID = Relativity.CustomPages.ConnectionHelper.Helper().GetActiveCaseID()
                    };
                    var result = rsapiClient.Repositories.RDO.Read(randomRDOArtifactID);
                }

                var model = new ProviderViewModel
                {
                    
                    SalesforceURI =  "",
                    SalesforceUserID = "",
                    SalesforceUserPwd = "",
                    StartDate = "",
                    TicketType = "<all>"
                    
      
                };
            //}
            //catch (Exception ex)
            //{
            //    logger.LogError(ex, "An error occured while loaded the custom page");
            //}

            return View(model);
        }

        [HttpPost]
        public ActionResult GetViewFields()
        {
            /*
             *  The JSON model from Script/myCustomProvider.js is retrieved from the input stream.  Here you have the option
             *  to deserialize the model and add items to the returned KeyValuePair, everything added will be displayed on the Job Status Page
             */
            var logger = Relativity.CustomPages.ConnectionHelper.Helper().GetLoggerFactory().GetLogger();
            var settings = new List<KeyValuePair<string, string>>();
            try
            {

                if (Request.InputStream != null && Request.InputStream.Length > 0)
                {
                    var data = new StreamReader(Request.InputStream).ReadToEnd();
                    var token = JToken.Parse(data);
                    var jobConfiguration = Utility.DecryptJobConfiguration(token.ToString());
                    settings.Add(new KeyValuePair<string, string>("Salesforce URI", WebUtility.HtmlEncode(jobConfiguration.SalesforceURI)));
                    settings.Add(new KeyValuePair<string, string>("Salesforce UserID", WebUtility.HtmlEncode(jobConfiguration.SalesforceUserID)));
                    //settings.Add(new KeyValuePair<string, string>("Salesforce pwd", jobConfiguration.SalesforceUserPwd));
                    settings.Add(new KeyValuePair<string, string>("Start Date", WebUtility.HtmlEncode(jobConfiguration.StartDate)));
//                    settings.Add(new KeyValuePair<string, string>("Ticket Type", WebUtility.HtmlEncode(jobConfiguration.TicketType)));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to parse JSON Input");
            }

            return Json(settings);
        }
    }
}