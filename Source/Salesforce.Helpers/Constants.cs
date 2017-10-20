using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Helpers
{
    

    public class Constants
    {

        public const String NAME = "name";
        public const String LABEL = "label";
        public const String IDENTIFIER= "identifier";
        public const String CASE = "Case";

        public class EncryptionKey
        {
            // Please Update this encryption Key
            public const String PLEASE_UPDATE_THIS_KEY = "1234567890";
        }

        public class SqlCommand
        {
            public const String SELECT = "SELECT ";
            public const String COMMA_SPACE = ", ";
            public const String FROM_CASE_WHERE = " FROM Case WHERE ";
            public const String FROM_USER_WHERE = " FROM User WHERE ";
            public const String FROM_GROUP_WHERE = " FROM Group WHERE ";
            public const String EQUAL = " = ";
            public const String SINGLE_QUOTE = "'";
            public const String START_DATE = " FROM Case WHERE CreatedDate >=  ";
            public const String TIME = "T00:00:00Z";
            public const String CASENUMBER = "CaseNumber";
            public const String NAME = "Name";
        }
        public class ReferenceObject
        {
            public const string OWNERID = "OwnerId";
            public const string LASTMODIFIED_BY_ID = "LastModifiedById";
            public const string PARENTID = "ParentId";
            public const string USER = "User";
            public const string USERGROUP = "UserGroup";
            public const string GROUP = "Group";
            public const string CASE = "Case";
        }
    }
}
