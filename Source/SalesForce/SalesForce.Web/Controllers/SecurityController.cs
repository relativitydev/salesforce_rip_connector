using System;
using System.Net;
using System.Web.Mvc;
using Salesforce.Helpers;
using Salesforce.Helpers.Interfaces;

namespace SalesForce.Web.Controllers
{
    public class SecurityController : Controller
    {
        public IUtility Utility;

        public SecurityController()
        {
            Utility = new Utility();
        }

        [HttpPost]
        public JsonResult Encrypt(String value)
        {
            var retVal = new JsonResult();
            try
            {
                retVal.Data = Utility.Encrypt(value);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (Int32)HttpStatusCode.BadRequest;
                retVal.Data = ex.ToString();
            }
            return retVal;
        }

        [HttpPost]
        public JsonResult Decrypt(String value)
        {
            var retVal = new JsonResult();
            try
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    retVal.Data = Utility.Decrypt(value);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (Int32)HttpStatusCode.BadRequest;
                retVal.Data = ex.ToString();
            }
            return retVal;
        }
    }
}