using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Security;

namespace Argix {
    //
    public class EnterpriseGateway {
        //Members
        public const string SQL_CONN = "Enterprise";

        //Interface
        public DataSet FillDataset(string spName,string tableName,object[] paramList) { return new DataService().FillDataset(SQL_CONN,spName,tableName,paramList); }
    }
}