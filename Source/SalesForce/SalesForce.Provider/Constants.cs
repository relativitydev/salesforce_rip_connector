using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForce.Provider
{
    public class Constants
    {
        public class Guids
        {
            public class Provider
            {
                public const String SALESFORCE_PROVIDER = "CC68F52A-FE80-4DE6-A8F0-B2CFFA70E392";
            }

            public class Application
            {
                public static Guid SFP_RELATIVITY_APPLICATION = new Guid("FE38D634-A413-4EB1-A486-FC00D2B0F17C");
            }
        }
    }
}
