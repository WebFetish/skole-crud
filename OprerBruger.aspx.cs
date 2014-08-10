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

public partial class Admin_OprerBruger : System.Web.UI.Page
{
    /// <summary>
    /// Her henter vi hvilke roller som man kan vælge.
    /// Husk og slette denne side når du er færdig, så man ikke kan oprette brugere uden og have rettigheder.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) // Hvis der sker et postback skal den ikke tilføje det igen.
        {
            SqlCommand cmd = new SqlCommand(); 
            SqlConnection conn = new SqlConnection(MyConnectionString.ConnectionString); // MyConnectionString.ConnectionString er fra en class som gør det lettere og skrive sin connectionstring
            cmd.Connection = conn; // Standard connection til din database.
            cmd.CommandText = "SELECT rolle_id, rolle_title FROM rolle ORDER BY rolle_adgang DESC"; // Din CommandText hvor du fortæller hvad den skal loade fra db.
            conn.Open(); // Åbner din connection så så du kan Execute og få din data ud.
            var reader = cmd.ExecuteReader(); // Gør hvad du fortæller den skal gøre som står i din CommandText. Her sætter vi noget data i en DataReader.
            DataTable items = new DataTable(); // Opretter et DataTable som vi kan bruge til at sætte dataen i DropDown menuen.
            items.Load(reader); // Indsætter dataen i dit DataTable.
            conn.Close(); // Lukker din connection så den ved at du ikke skal bruge mere data.

            DropDownList_Role.DataValueField = "rolle_id"; // Fortæller at den skal bruge dens id som value i DropDown menuen.
            DropDownList_Role.DataTextField = "rolle_title"; // Fortæller at den skal bruge dens title som text i DropDown menuen.
            DropDownList_Role.DataSource = items; // sætter dens DataSource til at være det DataTable som vi har oprettet til at være dens data.
            DropDownList_Role.DataBind();
            DropDownList_Role.Items.Insert(0, new ListItem("Vælg Rolle", "0")); // Sætter dens første punkt i DropDown menuen.
        }
    }

    /// <summary>
    /// Tjekker om man har indtastet al infomation og tilføjer en bruger.
    /// </summary>
    protected void Button_Save_Click(object sender, EventArgs e)
    {
        var fejl = false;

        if (TextBox_Name.Text == "") // Hvis TexBoxen er tom.
        {
            fejl = true; // Fortæller en der er sket en fejl så den ikke prøve og tilføje brugeren.
            eName.Attributes.Add("class", "form-group has-error"); // Gør TexBoxen rød så man kan se at det er den der er problemet.
        }

        if (TextBox_Email.Text == "") // Hvis TexBoxen er tom.
        {
            fejl = true; // Fortæller en der er sket en fejl så den ikke prøve og tilføje brugeren.
            eEmail.Attributes.Add("class", "form-group has-error");// Gør TexBoxen rød så man kan se at det er den der er problemet.
        }
        if (!fejl) // Hvis der ikke sker en fejl så skal den udfører dette.
        {
            var salt = Guid.NewGuid().ToString(); // Opretter et salt som er unique som bruges til at tilføje efter koden.
            var password = TextBox_Password.Text; // Tager koden som er indtastet i texboxen.
            UTF8Encoding encoder = new UTF8Encoding(); // Gør klar så vi encoder koden om til UTF8.
            SHA256Managed sha256hasher = new SHA256Managed(); // Gør klar til at lave kryptering.
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(password + salt));  // Her sætter vi koden (textboxen) og salted (fra db) og laver det til bytes efter, da ComputeHash kun kan bruge bytes.
            StringBuilder hash = new StringBuilder(""); // Laver en string builder så vi kan samle alle bytes til en string.
            for (int i = 0; i < hashedDataBytes.Length; i++) // For hver bytes 
                hash.Append(hashedDataBytes[i].ToString("X2")); // Her laver vi bytesne om til Hexadecimal.

            var cmd = new SqlCommand();
            var conn = new SqlConnection(MyConnectionString.ConnectionString); // MyConnectionString.ConnectionString er fra en class som gør det lettere og skrive sin connectionstring
            cmd.Connection = conn; // Standard connection til din database.
            cmd.CommandText = cmd.CommandText = "INSERT INTO bruger (bruger_navn, bruger_email, bruger_password, bruger_salt, fk_rolle_id, bruger_dato) VALUES (@1, @2, @3, @4, @5, @6)"; // Din CommandText hvor du fortæller hvad den skal loade fra db.
            cmd.Parameters.AddWithValue("@1", TextBox_Name.Text); // Dit parameter som laver sikkerhed for sql injection.
            cmd.Parameters.AddWithValue("@2", TextBox_Email.Text);
            cmd.Parameters.AddWithValue("@3", hash.ToString());
            cmd.Parameters.AddWithValue("@4", salt);
            cmd.Parameters.AddWithValue("@5", DropDownList_Role.SelectedValue);
            cmd.Parameters.AddWithValue("@6", DateTime.Now);
            conn.Open(); // Åbner din connection så så du kan Execute og få din data ud.
            cmd.ExecuteNonQuery(); // Gør hvad du fortæller den skal gøre som står i din CommandText.
            conn.Close(); // Lukker din connection så den ved at du ikke skal bruge mere data.
            Response.Redirect("OprerBruger.aspx"); // Sender personen til siden igen så der ikke er daten som lige er blevet sendt.
        }
        
    }
}