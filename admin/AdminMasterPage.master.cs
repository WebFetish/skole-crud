using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["bruger_id"] == null)
        {
            Response.Redirect(ResolveClientUrl("~/Login.aspx"));
        }
        else
        {
            Literal_bruger.Text += "<li class='nav navbar-nav navbar-right'>" + Session["bruger_navn"] + "</li>";
        }

        if (Session["besked"] != null)
        {
            Panel_Besked.Visible = true;
            Literal_Besked.Text = Session["besked"].ToString();
            Session.Remove("besked");
        }
    }
    public string MenuItem(string Target, string Text, int Rolle_Adgang)
    {
        if (Convert.ToInt32(Session["rolle_adgang"]) >= Rolle_Adgang)
        {
            string active = (Request.RawUrl.Contains(Target) ? " class='active'" : "");
            return String.Format("<li{2}><a href='{0}'>{1}</a></li>", Target, Text, active);
        }
        return string.Format("");

    }
    public string MenuItem(string Target, string Text, int Rolle_Adgang, string OnClick)
    {
        if (Convert.ToInt32(Session["rolle_adgang"]) >= Rolle_Adgang)
        {
            string active = (Request.RawUrl.Contains(Target) ? " class='active'" : "");
            return String.Format("<li{2}><a href='{0}' onclick='{3}'>{1}</a></li>", Target, Text, active, OnClick);
        }
        return string.Format("");
    }
}
