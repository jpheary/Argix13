using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Argix {
    //
    public class EnterpriseGateway {
        //Members
        public const string SQL_CONNID = "Enterprise";
        public const int CMD_TIMEOUT_DEFAULT = 3;
        
        //Interface
        public EnterpriseGateway() { }
    }
}
