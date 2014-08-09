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

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Login"; // Titlen på siden.
        Label_Errors.Visible = false;
        if (Request.QueryString["action"] == "logud") // Når man trykker på logud knappen på admin siden bliver du sendt her til og du får fjernet session.
        {
            Session.Clear(); // Gør så du ikker logget ind.
            Response.Redirect(ResolveClientUrl("~/Login.aspx"));
        }
        if (Session["bruger_id"] != null) // Hvis du vil prøve og logge ind imens du er logget ind bliver du sendt til admin siden.
        {
            Response.Redirect(ResolveClientUrl("~/Admin/Default.aspx"));
        }
    }
    protected void Button_Login_Click(object sender, EventArgs e)
    {
        var salt = "";
        string kode = "";
        SqlConnection conn = new SqlConnection(MyConnectionString.ConnectionString); // MyConnectionString.ConnectionString er fra en class som gør det lettere og skrive sin connectionstring.
        SqlCommand cmd = new SqlCommand(); // Standard connection til din database.
        cmd.Connection = conn;
        
        cmd.CommandText = @"
            SELECT bruger_id, bruger_navn, bruger_email, bruger_password, bruger_salt, rolle_adgang
            FROM bruger INNER JOIN rolle ON rolle_id = fk_rolle_id
            WHERE 
                bruger_email = @bruger_email";
        cmd.Parameters.AddWithValue("@bruger_email", TextBox_Email.Text);

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            salt = reader["bruger_salt"].ToString(); // Henter salt fra db. Salt bruges til når man skal krypterer koden så sætter man salt bag ved koden og så kryptere man koden.
            // Så hvis der er 2 der har koden 123, så er deres kode ikke ens efter den er krypteret.
            kode = reader["bruger_password"].ToString(); // Henter password fra db så vi kan se om koden er ens.
            var password = TextBox_Password.Text; // Tager koden som er indtastet i texboxen.
            UTF8Encoding encoder = new UTF8Encoding(); // Gør klar så vi encoder koden om til UTF8.
            SHA256Managed sha256hasher = new SHA256Managed(); // Gør klar til at lave kryptering.
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(password + salt)); // Her sætter vi koden (textboxen) og salted (fra db) og laver det til bytes efter, da ComputeHash kun kan bruge bytes.
            StringBuilder hash = new StringBuilder(""); // Laver en string builder så vi kan samle alle bytes til en string.
            for (int i = 0; i < hashedDataBytes.Length; i++) // For hver bytes 
                hash.Append(hashedDataBytes[i].ToString("X2")); // Her laver vi bytesne om til Hexadecimal.
            if (hash.ToString() == kode) // Nu tjekker vi om de koder er ens. Vi blev nød til at kryptere koden (fra texboxen) da koden fra db er kryptere og vi ikke kan dekryptere koden.
            {
                Session["bruger_id"] = reader["bruger_id"]; // Hvis koden er ens laver vi en masse sessions, så vi kan bruge dem hvis vi får brug for dem.
                Session["bruger_navn"] = reader["bruger_navn"];
                Session["bruger_email"] = reader["bruger_email"];
                Session["rolle_adgang"] = reader["rolle_adgang"];
                Session.Remove("besked");

                Response.Redirect(ResolveClientUrl("~/Admin/Default.aspx"));
            }
            else // Hvis koden ikke er ens.
            {
                Label_Errors.Text = "Forkert Email eller Password"; 
                Label_Errors.Visible = true;
            }
        }
        else // Hvis emailen ikke er findes. Af sikkerheds mæssige grunde fortæller vi ikke at det er emailen som er forkert. 
        {
            Label_Errors.Text = "Forkert Email eller Password";
            Label_Errors.Visible = true;
        }
        conn.Close();
    }
}