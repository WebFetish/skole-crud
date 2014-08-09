using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// Kontakt
// Kontakt
// kontakt_id
// Kontakt
// Husk at tilføje parameter!

public partial class Admin_Kontakt : System.Web.UI.Page
{
    public static string TableNavn = "Kontakt"; // Dit table navn.
    public static string Navn = "Kontakten"; // Dette er hvad der bliver oprettet/slettet ex. "Personen" blev oprettet.
    public static string TableId = "kontakt_id"; // navnet på dit table id, ex når du skal slettet et item.
    public static string SideNavn = "Kontakt"; // Sidens navn så når man bliver redirected at du kommer på den rigtige side.
    public static int RolleAdgang = 100; // Om brugeren har adgang til at tilgå denne side, du har angivet denne værdi i din db.
    // På linje 72 og 82 skal du tilføje din felter som skal vises på listen som giver et overblik over hvad du kan ændre.
    // På linje 140 skal du tilføje hvilke ting der skal læses fra din db til dine textboxe, så når du ændre siden at den henter teksten.
    // På linje 167 og 173 skal du tilføje til din commandtext hvilke felter du har i din db.
    // På linje 179 skal du tilføje dine parameter du skal bruge i din commandtext.

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
                    Dropdown();
                    break;

                case "edit":
                    Panel_Form.Visible = true; // Viser rediger siden.
                    Dropdown();
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

    private void HentRepeater() // Her henter du hvad der skal ud på selve oversigten af siden.
    {
        try
        {
            Panel_List.Visible = true;
            var cmd = new SqlCommand();
            var conn = new SqlConnection(Class.ConnectionString);
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM " + TableNavn + " INNER JOIN Emne ON emne_id = kontakt_emne_fk ORDER BY " + TableId + " DESC";

            conn.Open();
            var reader = cmd.ExecuteReader();
            // TODO tilføj felterne til din repeater.
            var builder = new StringBuilder();
            builder.Append("<table class='table table-striped table-hover'>").AppendLine();
            builder.Append("<thead>").AppendLine();
            builder.Append("<tr>").AppendLine();
            builder.Append("<th colspan='2'><a href='" + SideNavn + ".aspx?action=add' class='btn btn-success btn-xs'><span class='glyphicon glyphicon-plus'></span>&nbsp;Opret</a></th>").AppendLine();
            builder.Append("<th>Id</th>").AppendLine();
            builder.Append("<th>Navn</th>").AppendLine(); // Her skal du tilføje hvilke "overskrifte" der skal være. du skal bare kopiere denne linje for at lave flere.
            builder.Append("<th>Email</th>").AppendLine();
            builder.Append("<th>Emne</th>").AppendLine();
            builder.Append("</tr>").AppendLine();
            builder.Append("</thead>").AppendLine();
            builder.Append("<tbody>").AppendLine();
            while (reader.Read()) // Dette er det samme som en repeater og bare gentager sig selv for hver række som er i din db.
            {
                builder.Append("<tr>").AppendLine();
                builder.Append("<td style='width: 30px;'><a href='" + SideNavn + ".aspx?action=edit&id=" + reader[TableId] + "' class='btn btn-primary btn-xs'><span class='glyphicon glyphicon-pencil'></span></a></td>").AppendLine();
                builder.Append("<td style='width: 30px;'><a href='" + SideNavn + ".aspx?action=delete&id=" + reader[TableId] + "' class='btn btn-danger btn-xs' onclick=\"return confirm('Er du sikker på du vil slette?')\"><span class='glyphicon glyphicon-trash'></span></a></td>").AppendLine();
                builder.Append("<td style='width: 50px;'>" + reader[TableId] + "</td>").AppendLine();
                builder.Append("<td>" + reader["kontakt_navn"] + "</td>").AppendLine(); // Her skal du så tilføje samme antal som du har af overskrifter, hvor det bare henter det fra db. og du skal også bare kopiere denne linje for flere.
                builder.Append("<td>" + reader["kontakt_email"] + "</td>").AppendLine();
                builder.Append("<td>" + reader["emne_navn"] + "</td>").AppendLine();
                builder.Append("</tr>").AppendLine();
            }
            conn.Close();

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

    private void SletItem(string id) // Her sletter du det id som bliver sendt til den. 
    {
        try
        {
            SqlConnection conn = new SqlConnection(Class.ConnectionString);
            SqlCommand cmd = new SqlCommand("DELETE FROM " + TableNavn + " WHERE " + TableId + " = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            Session["besked"] = Navn + " blev slettet";
            Response.Redirect(ResolveClientUrl("~/Admin/" + SideNavn + ".aspx"));
        }
        catch (Exception ex)
        {
            Panel_Error.Visible = true;
            Label_Error.Text = ex.Message + " <strong>SletItem(), " + SideNavn + ".aspx.cs</strong>";
        }
    }
    private void HentItem(string id) // Her henter du teksten som skal i dine texboxe når du er på rediger siden.
    {
        if (!IsPostBack)
        {
            try
            {
                SqlConnection conn = new SqlConnection(Class.ConnectionString);
                SqlCommand cmd = new SqlCommand(@"SELECT * FROM " + TableNavn + " WHERE " + TableId + " = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // TODO Tilføj alle textboxe!
                    TextBox_Name.Text = reader["kontakt_navn"].ToString(); // Her ændre du Textboxen som skal have indholdet som du henter, og ændre din reader. Du kopirer denne linje for flerer.
                    TextBox_Email.Text = reader["kontakt_email"].ToString();
                    DropDownList_Emne.SelectedValue = reader["kontakt_emne_fk"].ToString();
                    TextBox_Besked.Text = reader["kontakt_besked"].ToString();
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                Panel_Error.Visible = true;
                Label_Error.Text = ex.Message + " <strong>HentItem(), " + SideNavn + ".aspx.cs</strong>";
            }
        }
    }

    private void Dropdown()
    {
        if (!IsPostBack)
        {
            SqlConnection conn = new SqlConnection(Class.ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(@"
                        SELECT * FROM Emne ORDER BY emne_id DESC", conn);
            DataTable items = new DataTable();
            adapter.Fill(items);

            DropDownList_Emne.DataTextField = "emne_navn";
            DropDownList_Emne.DataValueField = "emne_id";

            DropDownList_Emne.DataSource = items;
            DropDownList_Emne.DataBind();
            DropDownList_Emne.Items.Insert(0, new ListItem("Vælg Emne", "0"));
        }
    }

    protected void Button_Save_Click(object sender, EventArgs e) // når du opretter eller redigere kalder du denne, og her indsætter du det i din db.
    {
        try
        {
            SqlConnection conn = new SqlConnection(Class.ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            bool fejl = false;

            if (TextBox_Name.Text == "")
            {
                fejl = true;
                eName.Attributes.Add("class", "form-group has-error");
            }

            if (TextBox_Email.Text == "")
            {
                fejl = true;
                eEmail.Attributes.Add("class", "form-group has-error");
            }
            if (TextBox_Besked.Text == "")
            {
                fejl = true;
                eBesked.Attributes.Add("class", "form-group has-error");
            }

            switch (Request.QueryString["action"])
            {
                // TODO Tilføj de alle dine felter
                case "add":
                    // Sådan skal det se ud med mere end 1.
                    // (Navn, Efternavn) VALUES (@Navn, @Efternavn)
                    cmd.CommandText = "INSERT INTO " + TableNavn + " (kontakt_navn, kontakt_email, kontakt_emne_fk, kontakt_besked, kontakt_dato) VALUES (@1, @2, @3, @4, @5)";
                    break;

                case "edit":
                    // Sådan skal det se ud med mere end 1.
                    // Navn = @Navn, Efternavn = @Efternavn
                    cmd.CommandText = "UPDATE " + TableNavn + " SET kontakt_navn = @1, kontakt_email = @2, kontakt_emne_fk = @3, kontakt_besked = @4, kontakt_dato = @5 WHERE " + TableId + " = @id";
                    cmd.Parameters.AddWithValue("@id", Request.QueryString["id"]);
                    break;
            }
            // Husk og tilføje parameter. Husk du kan bruge samme parameter til add og edit.
            // Jeg bruger AddWithValue fordi så behøver du ikke og fortælle den om det er en int/string.
            cmd.Parameters.AddWithValue("@1", TextBox_Name.Text);
            cmd.Parameters.AddWithValue("@2", TextBox_Email.Text);
            cmd.Parameters.AddWithValue("@3", DropDownList_Emne.SelectedValue);
            cmd.Parameters.AddWithValue("@4", TextBox_Besked.Text);
            cmd.Parameters.AddWithValue("@5", DateTime.Now);

            if (!fejl)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                Session["besked"] = Navn + " blev gemt";
                Response.Redirect(ResolveClientUrl("~/Admin/" + SideNavn + ".aspx"));
            }
        }
        catch (Exception ex)
        {
            Panel_Error.Visible = true;
            Label_Error.Text = ex.Message + " <strong>Button_Save_Click(), " + SideNavn + ".aspx.cs</strong>";
        }
    }
}