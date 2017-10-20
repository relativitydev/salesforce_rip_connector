using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Helpers.Models
{
    public class JobConfiguration
    {
        public String SalesforceURI { get; set; }
        public String SalesforceUserID { get; set; }
        public String SalesforceUserPwd { get; set; }
        public String StartDate { get; set; }
        public String TicketType { get; set; }
        public Guid JobIdentifier { get; set; }
        public Int32 WorkspaceArtifactID { get; set; }

    }
}
