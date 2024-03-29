﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Nyheder : System.Web.UI.Page
{
    public const string TableNavn = "Nyheder"; // Dit table navn.
    public const string Navn = "Nyheden"; // Dette er hvad der bliver oprettet/slettet ex. "Personen" blev oprettet.
    public const string TableId = "NyhederId"; // navnet på dit table id, ex når du skal slettet et item.
    public const string SideNavn = "Nyheder"; // Sidens navn så når man bliver redirected at du kommer på den rigtige side (der skal ikke være .aspx på).
    public const int RolleAdgang = 100; // Om brugeren har adgang til at tilgå denne side, du har angivet denne værdi i din db.

    /// <summary>
    /// Først skal du tilføje dine textboxe som du skal bruge til at tilføje tingen du vil lave crud til.
    /// På linje 72 og 82 skal du tilføje din felter som skal vises på listen som giver et overblik over hvad du kan ændre.
    /// På linje 140 skal du tilføje hvilke ting der skal læses fra din db til dine textboxe, så når du ændre siden at den henter teksten.
    /// På linje 167 og 173 skal du tilføje til din commandtext hvilke felter du har i din db.
    /// På linje 179 skal du tilføje dine parameter du skal bruge i din commandtext.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = SideNavn + " - Adminside"; // Titlen på siden
        sideOverskrift.InnerText = Navn + " Administration"; //Overskriften på siden.
        sideLink.HRef = SideNavn + ".aspx"; // Et link på opret/rediger delen.
        if (Convert.ToInt32(Session["rolle_adgang"]) >= RolleAdgang)
        {
            switch (Request.QueryString["action"])
            {
                case "add":
                    Panel_Form.Visible = true; // Viser tilføj opret siden.
                    break;

                case "edit":
                    Panel_Form.Visible = true; // Viser rediger siden.
                    HentItem(Request.QueryString["id"]); // Kalder på denne så den kan indsætte teksten i dine tekstboxe.
                    break;

                case "delete":
                    SletItem(Request.QueryString["id"]); //Kalder på denne så den sletter siden med det id som er i linket.
                    break;

                default:
                    HentRepeater(); // Hvis der ikke er ?action i linket så vil denne blive kaldt.
                    break;
            }
        }
        else // Hvis personen ikke har adgang til siden.
        {
            Session["besked"] = "Du har ikke adgang til dette!";
            Response.Redirect(ResolveClientUrl("~/Admin/Default.aspx"));
        }
    }

    /// <summary>
    /// Her henter du hvad der skal ud på selve oversigten af siden.
    /// </summary>
    private void HentRepeater()
    {
        try
        {
            Panel_List.Visible = true;
            var cmd = new SqlCommand();
            var conn = new SqlConnection(MyConnectionString.ConnectionString); // MyConnectionString.ConnectionString er fra en class som gør det lettere og skrive sin connectionstring.
            cmd.Connection = conn; // Standard connection til din database.
            cmd.CommandText = "SELECT * FROM " + TableNavn + " ORDER BY " + TableId + " DESC"; // Din CommandText hvor du fortæller hvad den skal loade fra db.

            conn.Open(); // Åbner din connection så så du kan Execute og få din data ud.
            var reader = cmd.ExecuteReader();  // Gør hvad du fortæller den skal gøre som står i din CommandText.
            var builder = new StringBuilder();
            builder.Append("<table class='table table-striped table-hover'>").AppendLine();
            builder.Append("<thead>").AppendLine();
            builder.Append("<tr>").AppendLine();
            builder.Append("<th colspan='2'><a href='" + SideNavn + ".aspx?action=add' class='btn btn-success btn-xs'><span class='glyphicon glyphicon-plus'></span>&nbsp;Opret</a></th>").AppendLine();
            builder.Append("<th>Id</th>").AppendLine();
            // TODO Tilføj alle dine overskifter her. fx Efternavn.
            builder.Append("<th>Overskrift</th>").AppendLine(); // Her skal du tilføje hvilke "overskrifte" der skal være. du skal bare kopiere denne linje for at lave flere.
            builder.Append("<th>dato</th>").AppendLine();
            builder.Append("</tr>").AppendLine();
            builder.Append("</thead>").AppendLine();
            builder.Append("<tbody>").AppendLine();
            while (reader.Read()) // Dette er det samme som en repeater og bare gentager sig selv for hver række som er i din db.
            {
                builder.Append("<tr>").AppendLine();
                builder.Append("<td style='width: 30px;'><a href='" + SideNavn + ".aspx?action=edit&id=" + reader[TableId] + "' class='btn btn-primary btn-xs'><span class='glyphicon glyphicon-pencil'></span></a></td>").AppendLine();
                builder.Append("<td style='width: 30px;'><a href='" + SideNavn + ".aspx?action=delete&id=" + reader[TableId] + "' class='btn btn-danger btn-xs' onclick=\"return confirm('Er du sikker på du vil slette?')\"><span class='glyphicon glyphicon-trash'></span></a></td>").AppendLine();
                builder.Append("<td style='width: 50px;'>" + reader[TableId] + "</td>").AppendLine();
                // TODO Tilføj dine felter som tilhører dine overskrifter. fx reader["Efternavn"].
                builder.Append("<td>" + reader["NyhederOverskrift"] + "</td>").AppendLine(); // Her skal du så tilføje samme antal som du har af overskrifter, hvor det bare henter det fra db. og du skal også bare kopiere denne linje for flere.
                builder.Append("<td>" + reader["NyhederDato"] + "</td>").AppendLine();
                builder.Append("</tr>").AppendLine();
            }
            conn.Close(); // Lukker din connection så den ved at du ikke skal bruge mere data.

            builder.Append("</tbody>").AppendLine();
            builder.Append("<tfoot>").AppendLine();
            builder.Append("<tr>").AppendLine();
            builder.Append("<th colspan='6'><a href='" + SideNavn + ".aspx?action=add' class='btn btn-success btn-xs'><span class='glyphicon glyphicon-plus'></span>&nbsp;Opret</a></th>").AppendLine();
            builder.Append("</tr>").AppendLine();
            builder.Append("</tfoot>").AppendLine();
            builder.Append("</table>").AppendLine();

            LiteralItems.Text = builder.ToString(); // Her tager du alle de linjer du har lavet i din stringbuilder og sætter det ind på siden.
        }
        catch (Exception ex)
        {
            Panel_Error.Visible = true;
            Label_Error.Text = ex.Message + " <strong>HentRepeater(), " + SideNavn + ".aspx.cs</strong>";
        }
    }

    /// <summary>
    /// Her sletter du det id som bliver sendt til den.
    /// </summary>
    /// <param name="id">Det id der skal slettes</param>
    private void SletItem(string id)
    {
        try
        {
            var cmd = new SqlCommand();
            var conn = new SqlConnection(MyConnectionString.ConnectionString); // MyConnectionString.ConnectionString er fra en class som gør det lettere og skrive sin connectionstring.
            cmd.Connection = conn; // Standard connection til din database.
            cmd.CommandText = "DELETE FROM " + TableNavn + " WHERE " + TableId + " = @id"; // Din CommandText hvor du fortæller hvad den skal loade fra db.
            cmd.Parameters.AddWithValue("@id", id); // Dit parameter som laver sikkerhed for sql injection.
            conn.Open(); // Åbner din connection så så du kan Execute og få din data ud.
            cmd.ExecuteNonQuery(); // Gør hvad du fortæller den skal gøre som står i din CommandText.
            conn.Close(); // Lukker din connection så den ved at du ikke skal bruge mere data.

            Session["besked"] = Navn + " blev slettet";
            Response.Redirect(ResolveClientUrl("~/Admin/" + SideNavn + ".aspx"));
        }
        catch (Exception ex)
        {
            Panel_Error.Visible = true;
            Label_Error.Text = ex.Message + " <strong>SletItem(), " + SideNavn + ".aspx.cs</strong>";
        }
    }

    /// <summary>
    /// Her henter du teksten som skal i dine texboxe når du er på rediger siden.
    /// </summary>
    /// <param name="id">Dette er hvilket item som skal hentes</param>
    private void HentItem(string id)
    {
        if (!IsPostBack)
        {
            try
            {
                var cmd = new SqlCommand();
                var conn = new SqlConnection(MyConnectionString.ConnectionString); // MyConnectionString.ConnectionString er fra en class som gør det lettere og skrive sin connectionstring.
                cmd.Connection = conn; // Standard connection til din database.
                cmd.CommandText = "SELECT * FROM " + TableNavn + " WHERE " + TableId + " = @id"; // Din CommandText hvor du fortæller hvad den skal loade fra db.
                cmd.Parameters.AddWithValue("@id", id); // Dit parameter som laver sikkerhed for sql injection.

                conn.Open(); // Åbner din connection så så du kan Execute og få din data ud.
                var reader = cmd.ExecuteReader();  // Gør hvad du fortæller den skal gøre som står i din CommandText.
                if (reader.Read())
                {
                    // TODO Tilføj alle textboxe!
                    TextBoxOverkskrift.Text = reader["NyhederOverskrift"].ToString(); // Her ændre du Textboxen som skal have indholdet som du henter, og ændre din reader. Du kopirer denne linje for flerer.
                    TextBoxTekst.Text = reader["NyhederTekst"].ToString();
                }
                conn.Close(); // Lukker din connection så den ved at du ikke skal bruge mere data.

            }
            catch (Exception ex)
            {
                Panel_Error.Visible = true;
                Label_Error.Text = ex.Message + " <strong>HentItem(), " + SideNavn + ".aspx.cs</strong>";
            }
        }
    }

    /// <summary>
    /// Her er hvis du skal bruge en dropdown menu til at vælge noget.
    /// </summary>
    private void Dropdown()
    {
        if (!IsPostBack)
        {
            //SqlCommand cmd = new SqlCommand();
            //SqlConnection conn = new SqlConnection(MyConnectionString.ConnectionString); // MyConnectionString.ConnectionString er fra en class som gør det lettere og skrive sin connectionstring
            //cmd.Connection = conn; // Standard connection til din database.
            //cmd.CommandText = "SELECT rolle_id, rolle_title FROM rolle ORDER BY rolle_adgang DESC"; // Din CommandText hvor du fortæller hvad den skal loade fra db.
            //conn.Open(); // Åbner din connection så så du kan Execute og få din data ud.
            //var reader = cmd.ExecuteReader(); // Gør hvad du fortæller den skal gøre som står i din CommandText. Her sætter vi noget data i en DataReader.
            //DataTable items = new DataTable(); // Opretter et DataTable som vi kan bruge til at sætte dataen i DropDown menuen.
            //items.Load(reader); // Indsætter dataen i dit DataTable.
            //conn.Close(); // Lukker din connection så den ved at du ikke skal bruge mere data.

            //DropDownList_Role.DataValueField = "rolle_id"; // Fortæller at den skal bruge dens id som value i DropDown menuen.
            //DropDownList_Role.DataTextField = "rolle_title"; // Fortæller at den skal bruge dens title som text i DropDown menuen.
            //DropDownList_Role.DataSource = items; // sætter dens DataSource til at være det DataTable som vi har oprettet til at være dens data.
            //DropDownList_Role.DataBind();
            //DropDownList_Role.Items.Insert(0, new ListItem("Vælg Rolle", "0")); // Sætter dens første punkt i DropDown menuen.
        }
    }

    /// <summary>
    /// når du opretter eller redigere kalder du denne, og her indsætter du det i din db. 
    /// </summary>
    protected void Button_Save_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection conn = new SqlConnection(MyConnectionString.ConnectionString); // MyConnectionString.ConnectionString er fra en class som gør det lettere og skrive sin connectionstring.
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn; // Standard connection til din database.

            switch (Request.QueryString["action"])
            {
                // TODO Tilføj de alle dine felter
                case "add":
                    // Sådan skal det se ud med mere end 1.
                    // (Navn, Efternavn) VALUES (@Navn, @Efternavn)
                    cmd.CommandText = "INSERT INTO " + TableNavn + " (NyhederOverskrift, NyhederTekst, NyhederDato) VALUES (@1, @2, @3)"; // Din CommandText hvor du fortæller hvad den skal loade fra db.
                    break;

                case "edit":
                    // Sådan skal det se ud med mere end 1.
                    // Navn = @Navn, Efternavn = @Efternavn
                    cmd.CommandText = "UPDATE " + TableNavn + " SET NyhederOverskrift = @1, NyhederTekst = @2, NyhederDato = @3 WHERE " + TableId + " = @id"; // Din CommandText hvor du fortæller hvad den skal loade fra db.
                    cmd.Parameters.AddWithValue("@id", Request.QueryString["id"]); // Dit parameter som laver sikkerhed for sql injection.
                    break;
            }
            // Husk og tilføje parameter. Husk du kan bruge samme parameter til add og edit.
            // Jeg bruger AddWithValue fordi så behøver du ikke og fortælle den om det er en int/string.
            cmd.Parameters.AddWithValue("@1", TextBoxOverkskrift.Text); // Dit parameter som laver sikkerhed for sql injection.
            cmd.Parameters.AddWithValue("@2", TextBoxTekst.Text);
            cmd.Parameters.AddWithValue("@3", DateTime.Now);

            conn.Open(); // Åbner din connection så så du kan Execute og få din data ud.
            cmd.ExecuteNonQuery();  // Gør hvad du fortæller den skal gøre som står i din CommandText.
            conn.Close(); // Lukker din connection så den ved at du ikke skal bruge mere data.
            Session["besked"] = Navn + " blev gemt";
            Response.Redirect(ResolveClientUrl("~/Admin/" + SideNavn + ".aspx"));
        }
        catch (Exception ex)
        {
            Panel_Error.Visible = true;
            Label_Error.Text = ex.Message + " <strong>Button_Save_Click(), " + SideNavn + ".aspx.cs</strong>";
        }
    }
}
