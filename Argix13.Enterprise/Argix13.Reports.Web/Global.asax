<%@ Application Language="C#" %>
<%@ Import Namespace="Argix" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        RouteConfig.RegisterRoutes(RouteTable.Routes);
        BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
    void Application_Error(object sender,EventArgs e) {
        //Code that runs when an unhandled error occurs
        //NOTES:
        //  When transferring control to an error page, use Transfer()()() method. This preserves 
        //  the current context so that error information from GetLastError()()() is available.
        //  After handling an error, clear it by calling ClearError().
        Exception exc = Server.GetLastError();
        if (exc == null) {
            Server.ClearError();
        }
        else if (exc != null && exc.GetType() == typeof(System.Web.HttpException)) {
            //Http processing exception
            Server.Transfer("~/Error.aspx");
        }
        else if (exc != null && exc.GetType() == typeof(System.Web.HttpCompileException)) {
            //Compiler exception
            Server.Transfer("~/Error.aspx");
        }
        else if (exc != null && exc.GetType() == typeof(HttpUnhandledException)) {
            Response.Write("<h2>Oops!</h2>\n");
            if (exc != null) {
                Response.Write("<p>" + exc.Message + "</p>\n");
                if (exc.InnerException != null) {
                    Response.Write("<p>" + exc.InnerException.Message + "</p>\n");
                    if (exc.InnerException.InnerException != null) {
                        Response.Write("<p>" + exc.InnerException.InnerException.Message + "</p>\n");
                        if (exc.InnerException.InnerException.InnerException != null) {
                            Response.Write("<p>" + exc.InnerException.InnerException.InnerException.Message + "</p>\n");
                        }
                    }
                }
            }
            else
                Response.Write("<p>Unknown error</p>\n");
            Response.Write("<br /><br />");
            Response.Write("Return to the <a href='../Default.aspx'>" + "Reports Explorer</a>\n");
            Server.ClearError();
        }
        else {
            //ExceptionUtility.LogException(exc, "DefaultPage");
            //ExceptionUtility.NotifySystemOps(exc);
            Server.Transfer("~/Error.aspx");
        }
    }

</script>
