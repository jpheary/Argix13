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
    public enum FreightType { Regular=0, Returns }
    
    public class EnterpriseRGateway {
        //Members
        public const string SQL_CONN = "EnterpriseR";

        public const string USP_ARGIXTERMINALS = "uspRptTerminalsGetList",TBL_ARGIXTERMINALS = "TerminalTable";
        public const string USP_TERMINALS = "uspRptTerminalGetList",TBL_TERMINALS = "TerminalTable";
        public const string USP_CLIENTS = "uspRptClientGetList",TBL_CLIENTS = "ClientTable";
        public const string USP_CLIENTDIVS = "uspRptClientCustomerDivisionGetList",TBL_CLIENTDIVS = "ClientDivisionTable";
        public const string USP_CLIENTTERMINALS = "uspRptTerminalGetListForClient",TBL_CLIENTTERMINALS = "ClientTerminalTable";
        public const string USP_VENDORS = "uspRptVendorGetlistForClient",TBL_VENDORS = "VendorTable";
        public const string USP_AGENTS = "uspRptAgentGetList",TBL_AGENTS = "AgentTable";
        public const string USP_ZONES = "uspRptZoneMainGetList",TBL_ZONES = "ZoneTable";
        public const string USP_REGIONS_DISTRICTS = "uspRptRegionDistrictGetList",TBL_REGIONS = "RegionTable",TBL_DISTRICTS = "DistrictTable";
        public const string USP_EXCEPTIONS = "uspRptDeliveryExceptionGetList",TBL_EXCEPTIONS = "ExceptionTable";
        public const string USP_PICKUPS = "uspRptPickupsGetListFromToDate",TBL_PICKUPS = "PickupViewTable";
        public const string USP_INDIRECTTRIPS = "uspRptIndirectTripGetList",TBL_INDIRECTTRIPS = "IndirectTripTable";
        public const string USP_TLS = "uspRptTLGetListClosedForDateRange",TBL_TLS = "TLListViewTable";
        public const string USP_TLS_FIND = "uspRptTLFindClosedForTLNumber";
        public const string USP_SHIFTS = "uspRptTerminalShiftGetListForDate",TBL_SHIFTS = "ShiftTable";
        public const string USP_DAMAGECODES = "uspRptDamageCodeGetList",TBL_DAMAGECODES = "DamageDetailTable";
        public const string USP_LABELSEQNUMBERS = "uspRptSortedItemGetForCartonNumber",TBL_LABELSEQNUMBERS = "LabelStationTable";
        public const string USP_VENDORLOG = "uspRptManifestGetListForClient",TBL_VENDORLOG = "VendorLogTable";
        public const string USP_INDUCTEDFREIGHT = "uspRptInductedFreightGetList", TBL_INDUCTEDFREIGHT = "FreightTable";

        public const string DAMAGEDESCRIPTON_ALL = "All";
        public const string DAMAGEDESCRIPTON_ALL_EXCEPT_NC = "All, except NON-CONVEYABLE";

        //Interface
        public DataSet GetArgixTerminals() {
            //Get a list of Argix terminals
            DataSet terminals = new DataSet();
            terminals.Tables.Add(TBL_ARGIXTERMINALS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_ARGIXTERMINALS,TBL_ARGIXTERMINALS,new object[] { });
                if(ds != null && ds.Tables[TBL_ARGIXTERMINALS] != null && ds.Tables[TBL_ARGIXTERMINALS].Rows.Count > 0) 
                    terminals.Merge(ds.Tables["TerminalTable"].Select("", "Description ASC"));
            }
            catch(Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return terminals;
        }
        public DataSet GetTerminals() {
            //Get a list of Argix terminals
            DataSet terminals = new DataSet();
            terminals.Tables.Add(TBL_TERMINALS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_TERMINALS,TBL_TERMINALS,new object[] { });
                if (ds != null && ds.Tables[TBL_TERMINALS] != null && ds.Tables[TBL_TERMINALS].Rows.Count > 0) 
                    terminals.Merge(ds.Tables["TerminalTable"].Select("", "Description ASC"));
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return terminals;
        }
        public DataSet GetClientsList(bool activeOnly) {
            //Get a list of clients
            DataSet clients = new DataSet();
            clients.Tables.Add(TBL_CLIENTS);
            try {
                string filter = "DivisionNumber='01'";
                if(activeOnly) {
                    if(filter.Length > 0) filter += " AND ";
                    filter += "Status = 'A'";
                }
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_CLIENTS,TBL_CLIENTS,new object[] { });
                if(ds != null && ds.Tables[TBL_CLIENTS] != null && ds.Tables[TBL_CLIENTS].Rows.Count > 0) 
                    clients.Merge(ds.Tables[TBL_CLIENTS].Select(filter,"ClientName ASC"));
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return clients;
        }
        public DataSet GetClients() { return GetClients(null,null,false); }
        public DataSet GetClients(string number, string division, bool activeOnly) {
            //Get a list of clients filtered for a specific client or division or both
            //Added logic that checks for division 00 as switch to null TerminalCode for TimePeriodCartonCompare report
            DataSet clients = new DataSet();
            clients.Tables.Add(TBL_CLIENTS);
            try {
                string filter = "";
                bool nullTerminalCode = division == "00";
                if(number != null && number.Length > 0) filter = "ClientNumber='" + number + "'";
                if(division != null && division.Length > 0) {
                    if(filter.Length > 0) filter += " AND ";
                    filter += "DivisionNumber='" + (division == "00" ? "01" : division) + "'";
                }
                if(activeOnly) {
                    if(filter.Length > 0) filter += " AND ";
                    filter += "Status = 'A'";
                }
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_CLIENTS,TBL_CLIENTS,new object[] { });
                if(ds != null && ds.Tables[TBL_CLIENTS] != null && ds.Tables[TBL_CLIENTS].Rows.Count > 0)
                    clients.Merge(ds.Tables[TBL_CLIENTS].Select(filter,"ClientNumber ASC, DivisionNumber ASC"));
                if(nullTerminalCode) {
                    for (int i = 0;i < clients.Tables[TBL_CLIENTS].Rows.Count;i++) { clients.Tables[TBL_CLIENTS].Rows[i]["TerminalCode"] = ""; }
                }
            }
            catch(Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return clients;
        }
        public DataSet GetSecureClients(bool activeOnly) {
            //Load a list of client selections
            DataSet clients = new DataSet();
            try {
                clients.Tables.Add(TBL_CLIENTS);
                clients.Tables[TBL_CLIENTS].Columns.Add("ClientNumber");
                clients.Tables[TBL_CLIENTS].Columns.Add("DivisionNumber");
                clients.Tables[TBL_CLIENTS].Columns.Add("ClientName");
                clients.Tables[TBL_CLIENTS].Columns.Add("TerminalCode");
                clients.Tables[TBL_CLIENTS].Columns.Add("Status");
                clients.Tables[TBL_CLIENTS].Rows.Add(new object[] { "","","All","","" });
                DataSet ds = GetClients(null,"01",activeOnly);
                clients.Merge(ds.Tables[TBL_CLIENTS].Select("","ClientName ASC"));
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return clients;
        }
        public DataSet GetClientsForVendor(string vendorID) {
            //
            DataSet clients = new DataSet();
            clients.Tables.Add(TBL_CLIENTS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, "[uspRptClientGetListForVendor]",TBL_CLIENTS,new object[] { vendorID });
                if(ds != null && ds.Tables[TBL_CLIENTS] != null && ds.Tables[TBL_CLIENTS].Rows.Count > 0)
                    clients.Merge(ds.Tables[TBL_CLIENTS].Select("","ClientName ASC"));
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return clients;
        }
        public DataSet GetClientDivisions(string number) {
            //Get a list of client divisions
            DataSet divisions = new DataSet();
            divisions.Tables.Add(TBL_CLIENTDIVS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_CLIENTDIVS,TBL_CLIENTDIVS,new object[] { number });
                if (ds != null && ds.Tables[TBL_CLIENTDIVS] != null && ds.Tables[TBL_CLIENTDIVS].Rows.Count > 0)
                    divisions.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return divisions;
        }
        public DataSet GetClientTerminals(string number) {
            //Get a list of client terminals
            DataSet terminals = new DataSet();
            terminals.Tables.Add(TBL_CLIENTTERMINALS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_CLIENTTERMINALS,TBL_CLIENTTERMINALS,new object[] { number });
                if (ds != null && ds.Tables[TBL_CLIENTTERMINALS] != null && ds.Tables[TBL_CLIENTTERMINALS].Rows.Count > 0)
                    terminals.Merge(ds);
            }
            catch(Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return terminals;
        }
        public DataSet GetClientRegions(string number) {
            //Get a list of client divisions
            DataSet regions = new DataSet();
            regions.Tables.Add(TBL_REGIONS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_REGIONS_DISTRICTS,TBL_REGIONS,new object[] { number });
                Hashtable table = new Hashtable();
                for(int i = 0;i < ds.Tables[TBL_REGIONS].Rows.Count;i++) {
                    string region = ds.Tables[TBL_REGIONS].Rows[i]["Region"].ToString().Trim();
                    if(region.Length == 0)
                        ds.Tables[TBL_REGIONS].Rows[i].Delete();
                    else {
                        if(table.ContainsKey(region))
                            ds.Tables[TBL_REGIONS].Rows[i].Delete();
                        else {
                            table.Add(region,ds.Tables[TBL_REGIONS].Rows[i]["RegionName"].ToString().Trim());
                            ds.Tables[TBL_REGIONS].Rows[i]["Region"] = region;
                            ds.Tables[TBL_REGIONS].Rows[i]["RegionName"] = ds.Tables[TBL_REGIONS].Rows[i]["RegionName"].ToString().Trim();
                        }
                    }
                }
                regions.Merge(ds,true);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return regions;
        }
        public DataSet GetClientDistricts(string number) {
            //Get a list of client divisions
            DataSet districts = new DataSet();
            districts.Tables.Add(TBL_CLIENTTERMINALS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_REGIONS_DISTRICTS,TBL_DISTRICTS,new object[] { number });
                if (ds != null && ds.Tables[TBL_CLIENTTERMINALS] != null && ds.Tables[TBL_CLIENTTERMINALS].Rows.Count > 0)
                    districts.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return districts;
        }
        public DataSet GetVendorsList(string clientNumber,string clientTerminal) {
            //Get a list of vendors
            DataSet vendors = new DataSet();
            vendors.Tables.Add(TBL_VENDORS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_VENDORS,TBL_VENDORS,new object[] { clientNumber,clientTerminal });
                if(ds != null && ds.Tables[TBL_VENDORS] != null && ds.Tables[TBL_VENDORS].Rows.Count > 0)
                    vendors.Merge(ds.Tables[TBL_VENDORS].Select("","VendorName ASC"));
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return vendors;
        }
        public DataSet GetVendors(string clientNumber,string clientTerminal) {
            //Get a list of vendors
            DataSet vendors = new DataSet();
            vendors.Tables.Add(TBL_VENDORS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_VENDORS,TBL_VENDORS,new object[] { clientNumber,clientTerminal });
                if (ds != null && ds.Tables[TBL_VENDORS] != null && ds.Tables[TBL_VENDORS].Rows.Count > 0) {
                    vendors.Merge(ds.Tables[TBL_VENDORS].Select("","VendorNumber ASC"));
                    vendors.Tables[TBL_VENDORS].Columns.Add("VendorSummary");
                    for (int i = 0;i < vendors.Tables[TBL_VENDORS].Rows.Count;i++) {
                        vendors.Tables[TBL_VENDORS].Rows[i]["VendorSummary"] = (!vendors.Tables[TBL_VENDORS].Rows[i].IsNull("VendorNumber") ? vendors.Tables[TBL_VENDORS].Rows[i]["VendorNumber"].ToString() : "     ") + " - " +
                                                                          (!vendors.Tables[TBL_VENDORS].Rows[i].IsNull("VendorName") ? vendors.Tables[TBL_VENDORS].Rows[i]["VendorName"].ToString().Trim() : "");
                    }
                }
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return vendors;
        }
        public DataSet GetParentVendors(string clientNumber,string clientTerminal) {
            //Get a list of parent vendors
            DataSet vendors = new DataSet();
            vendors.Tables.Add(TBL_VENDORS);
            try {
                DataSet ds = GetVendors(clientNumber,clientTerminal);
                if (clientNumber != null && ds.Tables[TBL_VENDORS] != null && ds.Tables[TBL_VENDORS].Rows.Count > 0)
                    vendors.Merge(ds.Tables[TBL_VENDORS].Select("VendorParentNumber = ''"));
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return vendors;
        }
        public DataSet GetVendorLocations(string clientNumber,string clientTerminal,string vendorNumber) {
            //Get a list of vendor locations (child vendors) for the specified client-vendor
            DataSet locs = new DataSet();
            locs.Tables.Add(TBL_VENDORS);
            try {
                DataSet ds = GetVendors(clientNumber,clientTerminal);
                if (clientNumber != null && vendorNumber != null && ds.Tables[TBL_VENDORS] != null && ds.Tables[TBL_VENDORS].Rows.Count > 0)
                    locs.Merge(ds.Tables[TBL_VENDORS].Select("VendorParentNumber = '" + vendorNumber + "'"));
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return locs;
        }
        public DataSet GetAgents(bool mainZoneOnly) {
            //Get a list of agents
            DataSet agents = new DataSet();
            agents.Tables.Add(TBL_AGENTS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_AGENTS,TBL_AGENTS,new object[] { });
                if(ds != null && ds.Tables[TBL_AGENTS] != null && ds.Tables[TBL_AGENTS].Rows.Count != 0) {
                    agents.Merge(ds.Tables[TBL_AGENTS].Select(mainZoneOnly ? "MainZone <> ''" : "","AgentName ASC"));
                    agents.Tables[TBL_AGENTS].Columns.Add("AgentSummary");
                    for (int i = 0;i < agents.Tables[TBL_AGENTS].Rows.Count;i++) {
                        agents.Tables[TBL_AGENTS].Rows[i]["AgentSummary"] = (!agents.Tables[TBL_AGENTS].Rows[i].IsNull("MainZone") ? agents.Tables[TBL_AGENTS].Rows[i]["MainZone"].ToString().PadLeft(2,' ') : "  ") + " - " +
                                                             (!agents.Tables[TBL_AGENTS].Rows[i].IsNull("AgentNumber") ? agents.Tables[TBL_AGENTS].Rows[i]["AgentNumber"].ToString() : "    ") + " - " +
                                                             (!agents.Tables[TBL_AGENTS].Rows[i].IsNull("AgentName") ? agents.Tables[TBL_AGENTS].Rows[i]["AgentName"].ToString().Trim() : "");
                    }
                }
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return agents;
        }
        public DataSet GetParentAgents() {
            //Get a list of parent agent
            DataSet agents = new DataSet();
            agents.Tables.Add(TBL_AGENTS);
            try {
                DataSet ds = GetAgents(false);
                if (ds.Tables[TBL_AGENTS] != null && ds.Tables[TBL_AGENTS].Rows.Count > 0) agents.Merge(ds.Tables[TBL_AGENTS].Select("AgentParentNumber = ''"));
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return agents;
        }
        public DataSet GetAgentLocations(string agent) {
            //Get a list of agents
            DataSet locs = new DataSet();
            locs.Tables.Add(TBL_AGENTS);
            try {
                DataSet ds = GetAgents(false);
                if (agent != null && ds.Tables[TBL_AGENTS] != null && ds.Tables[TBL_AGENTS].Rows.Count > 0) locs.Merge(ds.Tables[TBL_AGENTS].Select("AgentParentNumber = '" + agent + "'"));
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return locs;
        }

        public DataSet GetShippers(FreightType freightType,string clientNumber,string clientTerminal) {
            //Get a list of shippers
            DataSet shippers = new DataSet();
            shippers.Tables.Add("ShipperTable");
            shippers.Tables["ShipperTable"].Columns.Add("ShipperNumber");
            shippers.Tables["ShipperTable"].Columns.Add("ShipperName");
            shippers.Tables["ShipperTable"].Columns.Add("ShipperParentNumber");
            shippers.Tables["ShipperTable"].Columns.Add("ShipperSummary");
            try {
                if (freightType == FreightType.Regular) {
                    DataSet vendors = GetVendors(clientNumber,clientTerminal);
                    for (int i = 0;i < vendors.Tables[TBL_VENDORS].Rows.Count;i++)
                        shippers.Tables["ShipperTable"].Rows.Add(new object[]{vendors.Tables[TBL_VENDORS].Rows[i]["VendorNumber"],vendors.Tables[TBL_VENDORS].Rows[i]["VendorName"],vendors.Tables[TBL_VENDORS].Rows[i]["VendorParentNumber"],vendors.Tables[TBL_VENDORS].Rows[i]["VendorSummary"]});
                }
                else if (freightType == FreightType.Returns) {
                    DataSet agents = GetAgents(false);
                    shippers.Merge(agents.Tables[TBL_AGENTS].Select("","ShipperNumber ASC"));
                    for (int i = 0;i < agents.Tables[TBL_AGENTS].Rows.Count;i++)
                        shippers.Tables["ShipperTable"].Rows.Add(new object[]{agents.Tables[TBL_AGENTS].Rows[i]["AgentNumber"],agents.Tables[TBL_AGENTS].Rows[i]["AgentName"],agents.Tables[TBL_AGENTS].Rows[i]["AgentParentNumber"],agents.Tables[TBL_AGENTS].Rows[i]["AgentSummary"]});
                }
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return shippers;
        }
        public DataSet GetZones() {
            //Get a list of zones
            DataSet zones = new DataSet();
            zones.Tables.Add(TBL_ZONES);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_ZONES,TBL_ZONES,new object[] { });
                if(ds != null && ds.Tables[TBL_ZONES] != null && ds.Tables[TBL_ZONES].Rows.Count > 0) zones.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return zones;
        }

        public DataSet GetPickups(string client,string division,DateTime startDate,DateTime endDate,string vendor) {
            //Get a list of pickups
            DataSet pickups = new DataSet();
            pickups.Tables.Add(TBL_PICKUPS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_PICKUPS,TBL_PICKUPS,new object[] { client,division,startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd"),vendor });
                if (ds != null && ds.Tables[TBL_PICKUPS] != null && ds.Tables[TBL_PICKUPS].Rows.Count > 0) {
                    pickups.Merge(ds);
                    for (int i = 0;i < pickups.Tables[TBL_PICKUPS].Rows.Count;i++) {
                        pickups.Tables[TBL_PICKUPS].Rows[i]["ManifestNumbers"] = pickups.Tables[TBL_PICKUPS].Rows[i]["ManifestNumbers"].ToString().Replace(",",", ");
                        pickups.Tables[TBL_PICKUPS].Rows[i]["TrailerNumbers"] = pickups.Tables[TBL_PICKUPS].Rows[i]["TrailerNumbers"].ToString().Replace(",",", ");
                    }
                }
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return pickups;
        }
        public DataSet GetDeliveryExceptions() {
            //Get a list of delivery exceptions
            DataSet exceptions = new DataSet();
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_EXCEPTIONS,TBL_EXCEPTIONS,new object[] { });
                if(ds != null && ds.Tables[TBL_EXCEPTIONS] != null && ds.Tables[TBL_EXCEPTIONS].Rows.Count > 0) exceptions.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return exceptions;
        }
        public DataSet GetIndirectTrips(string terminal,int daysBack) {
            //Get a list of indirect trips
            DataSet trips = new DataSet();
            trips.Tables.Add(TBL_INDIRECTTRIPS);
            try {
                DateTime startDate = DateTime.Today.AddDays(-daysBack);
                DateTime endDate = DateTime.Today;
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_INDIRECTTRIPS,TBL_INDIRECTTRIPS,new object[] { terminal,startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd") });
                if(ds != null && ds.Tables[TBL_INDIRECTTRIPS] != null && ds.Tables[TBL_INDIRECTTRIPS].Rows.Count != 0) trips.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return trips;
        }
        public DataSet GetTLs(string terminal,int daysBack) {
            //Event handler for change in selected terminal
            DataSet tls = new DataSet();
            tls.Tables.Add(TBL_TLS);
            try {
                string startDate = DateTime.Today.AddDays(-daysBack).ToString("yyyy-MM-dd");
                string endDate = DateTime.Today.ToString("yyyy-MM-dd");
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_TLS,TBL_TLS,new object[] { terminal,startDate,endDate });
                if(ds != null && ds.Tables[TBL_TLS] != null && ds.Tables[TBL_TLS].Rows.Count > 0) tls.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return tls;
        }
        public DataSet GetTLs(string terminal,DateTime startDate,DateTime endDate) {
            //Event handler for change in selected terminal
            DataSet tls = new DataSet();
            tls.Tables.Add(TBL_TLS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_TLS,TBL_TLS,new object[] { terminal,startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd") });
                if (ds != null && ds.Tables[TBL_TLS] != null && ds.Tables[TBL_TLS].Rows.Count > 0) tls.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return tls;
        }
        public DataSet FindTL(string terminal,string TLNumber) {
            //
            DataSet tls = new DataSet();
            tls.Tables.Add(TBL_TLS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_TLS_FIND,TBL_TLS,new object[] { terminal,TLNumber });
                if (ds != null && ds.Tables[TBL_TLS] != null && ds.Tables[TBL_TLS].Rows.Count > 0) tls.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return tls;
        }
        public DataSet GetShifts(string terminal,DateTime date) {
            //Event handler for change in selected terminal
            DataSet shifts = new DataSet();
            shifts.Tables.Add(TBL_SHIFTS);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_SHIFTS,TBL_SHIFTS,new object[] { terminal,date.ToString("yyyy-MM-dd") });
                if(ds != null && ds.Tables[TBL_SHIFTS] != null && ds.Tables[TBL_SHIFTS].Rows.Count > 0) shifts.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return shifts;
        }
        public DataSet GetVendorLog(string client,string clientDivision,DateTime startDate,DateTime endDate) {
            //Event handler for change in selected terminal
            DataSet log = new DataSet();
            log.Tables.Add(TBL_VENDORLOG);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_VENDORLOG,TBL_VENDORLOG,new object[] { client,clientDivision,startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd") });
                if(ds != null && ds.Tables[TBL_VENDORLOG] != null && ds.Tables[TBL_VENDORLOG].Rows.Count != 0) log.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return log;
        }

        public DataSet GetRetailDates(string scope) {
            //Create a list of retail dates
            DataSet ds = new DataSet();
            try {
                ds.Tables.Add("DateRangeTable");
                ds.Tables["DateRangeTable"].Columns.Add("Year",Type.GetType("System.Int32"));
                ds.Tables["DateRangeTable"].Columns.Add("Quarter",Type.GetType("System.Int32"));
                ds.Tables["DateRangeTable"].Columns.Add("Month",Type.GetType("System.Int32"));
                ds.Tables["DateRangeTable"].Columns.Add("Name");
                ds.Tables["DateRangeTable"].Columns.Add("Week",Type.GetType("System.Int32"));
                ds.Tables["DateRangeTable"].Columns.Add("StartDate",Type.GetType("System.DateTime"));
                ds.Tables["DateRangeTable"].Columns.Add("EndDate",Type.GetType("System.DateTime"));
                ds.Tables["DateRangeTable"].Columns.Add("Value");
                DataSet _ds = null;
                string field = "";
                switch(scope.ToLower()) {
                    case "week":
                        field = "Week";
                        _ds = new DataService().FillDataset(SQL_CONN, "uspRptRetailCalendarWeekGetList","DateRangeTable",new object[] { });
                        break;
                    case "month":
                        field = "Month";
                        _ds = new DataService().FillDataset(SQL_CONN, "uspRptRetailCalendarMonthGetList","DateRangeTable",new object[] { });
                        break;
                    case "quarter":
                        field = "Quarter";
                        _ds = new DataService().FillDataset(SQL_CONN, "uspRptRetailCalendarQuarterGetList","DateRangeTable",new object[] { });
                        break;
                    case "ytd":
                        field = "Year";
                        _ds = new DataService().FillDataset(SQL_CONN, "uspRptRetailCalendarYearGetList","DateRangeTable",new object[] { });
                        break;
                }
                for(int i = _ds.Tables["DateRangeTable"].Rows.Count;i > 0;i--)
                    ds.Tables["DateRangeTable"].ImportRow(_ds.Tables["DateRangeTable"].Rows[i - 1]);
                for(int i = 0;i < ds.Tables["DateRangeTable"].Rows.Count;i++) {
                    string val = ds.Tables["DateRangeTable"].Rows[i][field].ToString().Trim();
                    string start = ((DateTime)ds.Tables["DateRangeTable"].Rows[i]["StartDate"]).ToString("yyyy-MM-dd");
                    string end = ((DateTime)ds.Tables["DateRangeTable"].Rows[i]["EndDate"]).ToString("yyyy-MM-dd");
                    ds.Tables["DateRangeTable"].Rows[i]["Value"] = val + ", " + start + " : " + end + "";
                }
                ds.AcceptChanges();
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return ds;
        }
        public DataSet GetSortDates() {
            //Create a list of sort dates
            DataSet ds = new DataSet();
            try {
                ds.Tables.Add("DateRangeTable");
                ds.Tables["DateRangeTable"].Columns.Add("Value");
                int d = (int)DateTime.Today.DayOfWeek;
                DateTime _end = DateTime.Today.AddDays(-d);
                for(int i=-1;i<52;i++) {
                    DateTime end = _end.AddDays(-7 * i);
                    DateTime start = end.AddDays(-6);
                    ds.Tables["DateRangeTable"].Rows.Add(new object[] { start.ToString("yyyy-MM-dd") + " : " + end.ToString("yyyy-MM-dd") });
                }
                ds.AcceptChanges();
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return ds;
        }
        public DataSet GetDamageCodes() {
            //Get a list of Argix damage codes
            DataSet codes = new DataSet();
            try {
                codes.Tables.Add(TBL_DAMAGECODES);
                codes.Tables[TBL_DAMAGECODES].Columns.Add("CODE");
                codes.Tables[TBL_DAMAGECODES].Columns.Add("DESCRIPTION");
                codes.Tables[TBL_DAMAGECODES].Rows.Add(new object[]{"0",DAMAGEDESCRIPTON_ALL});
                codes.Tables[TBL_DAMAGECODES].Rows.Add(new object[]{"00",DAMAGEDESCRIPTON_ALL_EXCEPT_NC});
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_DAMAGECODES,TBL_DAMAGECODES,new object[] { });
                if(ds != null && ds.Tables[TBL_DAMAGECODES] != null && ds.Tables[TBL_DAMAGECODES].Rows.Count != 0) codes.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return codes;
        }
        public DataSet GetCartons(string cartonNumber,string terminalCode,string clientNumber) {
            //Get all cartons that have the specified carton number
            DataSet cartons = new DataSet();
            cartons.Tables.Add(TBL_LABELSEQNUMBERS);
            try {
                if(cartonNumber != null) {
                    DataSet ds = new DataService().FillDataset(SQL_CONN, USP_LABELSEQNUMBERS,TBL_LABELSEQNUMBERS,new object[] { cartonNumber,terminalCode,clientNumber });
                    if(ds != null && ds.Tables[TBL_LABELSEQNUMBERS] != null && ds.Tables[TBL_LABELSEQNUMBERS].Rows.Count > 0) cartons.Merge(ds);
                }
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return cartons;
        }
        public DataSet GetInductedFreight(DateTime startImportedDate,DateTime endImportedDate,string terminalCode) {
            //
            DataSet freight = new DataSet();
            freight.Tables.Add(TBL_INDUCTEDFREIGHT);
            try {
                DataSet ds = new DataService().FillDataset(SQL_CONN, USP_INDUCTEDFREIGHT, TBL_INDUCTEDFREIGHT, new object[] { startImportedDate, endImportedDate, terminalCode });
                if (ds != null && ds.Tables[TBL_INDUCTEDFREIGHT] != null && ds.Tables[TBL_INDUCTEDFREIGHT].Rows.Count > 0) freight.Merge(ds);
            }
            catch (Exception ex) { throw new ApplicationException(ex.Message,ex); }
            return freight;
        }


        public DataSet FillDataset(string spName,string tableName,object[] paramList) { return new DataService().FillDataset(SQL_CONN,spName,tableName,paramList); }
    }
}