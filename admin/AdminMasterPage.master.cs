using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminMasterPage : System.Web.UI.MasterPage
{
    /// <summary>
    /// Her Tjekker du om personen er logged ind og om der er nogen besked der skal vises.
    /// Du har også noget der hedder MenuItem som ser om du har rettigheder til de forskellige menu punkter.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["bruger_id"] == null)
        {
            Response.Redirect(ResolveClientUrl("~/Login.aspx"));
        }
        else // Hvis du ikke vil have at ens navn står i menuen skal du fjerne dette og slette Literal_bruger i din AdminMasterPage.master side.
        {
            Literal_bruger.Text += "<li><a href='#'>" + Session["bruger_navn"] + "</a></li>";
        }

        if (Session["besked"] != null) // Hvis der er en besked som der skal vises viser den det og sletter den så den ikke bliver vist igen.
        {
            Panel_Besked.Visible = true;
            Literal_Besked.Text = Session["besked"].ToString(); // Viser beskeden.
            Session.Remove("besked"); // Sletter beskeden.
        }
    }
    
    /// <summary>
    /// Dette bliver kaldt fra AdminMasterPage.master som tilføjer et menupunkt til menuen.
    /// </summary>
    /// <param name="Target">Link til siden</param>
    /// <param name="Text">Navnet på siden</param>
    /// <param name="Rolle_Adgang">Adgangs krav til siden</param>
    /// <returns>Retunere et menupunkt</returns>
    public string MenuItem(string Target, string Text, int Rolle_Adgang)
    {
        if (Convert.ToInt32(Session["rolle_adgang"]) >= Rolle_Adgang) // Tjekker om personen har rettigheder.
        {
            string active = (Request.RawUrl.Contains(Target) ? " class='active'" : ""); // Tilføjer active class hvis man befinder sig på siden.
            return String.Format("<li{2}><a href='{0}'>{1}</a></li>", Target, Text, active); 
        }
        return string.Format(""); // Hvis man ikke har rettigheder viser den ikke noget.

    }
    /// <summary>
    /// Dette bliver kaldt fra AdminMasterPage.master som tilføjer et menupunkt til menuen.
    /// </summary>
    /// <param name="Target">Link til siden</param>
    /// <param name="Text">Navnet på siden</param>
    /// <param name="Rolle_Adgang">Adgangs krav til siden</param>
    /// <param name="OnClick">Så du kan lave et "er du sikker på at du vil?"</param>
    /// <returns>Retunere et menupunkt</returns>
    public string MenuItem(string Target, string Text, int Rolle_Adgang, string OnClick)
    {
        if (Convert.ToInt32(Session["rolle_adgang"]) >= Rolle_Adgang) // Tjekker om personen har rettigheder.
        {
            string active = (Request.RawUrl.Contains(Target) ? " class='active'" : ""); // Tilføjer active class hvis man befinder sig på siden.
            return String.Format("<li{2}><a href='{0}' onclick='{3}'>{1}</a></li>", Target, Text, active, OnClick); // Printer et menupunkt til menuen som bliver sendt til den.
        }
        return string.Format(""); // Hvis man ikke har rettigheder viser den ikke noget.
    }
}
