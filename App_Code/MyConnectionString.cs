using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for MyConnectionString
/// </summary>
public class MyConnectionString
{
    public static string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
}