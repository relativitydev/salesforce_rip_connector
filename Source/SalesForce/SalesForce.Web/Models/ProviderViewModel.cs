using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesForce.Web.Models
{
    public class ProviderViewModel
    {
        public String SalesforceURI { get; set; }
        public String SalesforceUserID { get; set; }
        public String SalesforceUserPwd { get; set; }
        public String StartDate { get; set; }
        public String TicketType { get; set; }

    }
}