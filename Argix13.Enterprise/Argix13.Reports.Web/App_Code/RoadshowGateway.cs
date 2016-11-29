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

namespace Argix.Terminals {
    //    
    public class RoadshowGateway {
        //Members
        public const string SQL_CONN = "Roadshow";
        public const string USP_DEPOTS = "uspDepotsGetList",TBL_DEPOTS = "DepotTable";

        //Interface
        public DataSet GetDepots() {
            //Get a list of Argix depots
            DataSet depots = null;
            try {
                depots = new DataSet();
                depots.Tables.Add(TBL_DEPOTS);
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_DEPOTS,TBL_DEPOTS,new object[] { });
                if(ds.Tables[TBL_DEPOTS].Rows.Count != 0) {
                    depots.Merge(ds);
                }
            }
            catch(ApplicationException ex) { throw ex; }
            catch(Exception ex) { throw new ApplicationException("Unexpected exception reading Roadshow depot list.",ex); }
            return depots;
        }

        public DataSet FillDataset(string spName,string tableName,object[] paramList) { return new DataService().FillDataset(SQL_CONN,spName,tableName,paramList); }
    }
}