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
        SqlConnection conn = new SqlConnection(Class.ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        
        cmd.CommandText = @"
            SELECT bruger_id, bruger_navn, bruger_email, bruger_password, bruger_salt, rolle_adgang
            FROM bruger INNER JOIN rolle ON rolle_id = fk_rolle_id
            WHERE 
                bruger_email = @bruger_email";
        cmd.Parameters.Add("@bruger_email", SqlDbType.VarChar).Value = TextBox_Email.Text;

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            salt = reader["bruger_salt"].ToString();
            kode = reader["bruger_password"].ToString();
            var password = TextBox_Password.Text;
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(password + salt));
            StringBuilder hash = new StringBuilder("");
            for (int i = 0; i < hashedDataBytes.Length; i++)
            hash.Append(hashedDataBytes[i].ToString("X2"));
            if (hash.ToString() == kode)
            {
                Session["bruger_id"] = reader["bruger_id"];
                Session["bruger_navn"] = reader["bruger_navn"];
                Session["bruger_email"] = reader["bruger_email"];
                Session["rolle_adgang"] = reader["rolle_adgang"];
                Session.Remove("besked");

                Response.Redirect(ResolveClientUrl("~/Admin/Default.aspx"));
            }
            else
            {
                Label_Errors.Text = "Forkert Email eller Password";
                Label_Errors.Visible = true;
            }
        }
        else
        {
            Label_Errors.Text = "Forkert Email eller Password";
            Label_Errors.Visible = true;
        }
        conn.Close();
    }
}