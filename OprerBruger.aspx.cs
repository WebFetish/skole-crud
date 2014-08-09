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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SqlConnection conn = new SqlConnection(MyConnectionString.ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(@"
            SELECT rolle_id, rolle_title
            FROM rolle
            ORDER BY rolle_adgang DESC", conn);
            DataTable items = new DataTable();
            adapter.Fill(items);

            DropDownList_Role.DataTextField = "rolle_title";
            DropDownList_Role.DataValueField = "rolle_id";

            DropDownList_Role.DataSource = items;
            DropDownList_Role.DataBind();
            DropDownList_Role.Items.Insert(0, new ListItem("Vælg Rolle", "0"));
        }
    }

    protected void Button_Save_Click(object sender, EventArgs e)
    {
        var fejl = false;

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
        if (!fejl)
        {
            var salt = Guid.NewGuid().ToString();
            var password = TextBox_Password.Text;
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(password + salt));
            StringBuilder hash = new StringBuilder("");
            for (int i = 0; i < hashedDataBytes.Length; i++)
                hash.Append(hashedDataBytes[i].ToString("X2"));

            var cmd = new SqlCommand();
            var conn = new SqlConnection(MyConnectionString.ConnectionString);
            cmd.Connection = conn;
            cmd.CommandText = cmd.CommandText = "INSERT INTO bruger (bruger_navn, bruger_email, bruger_password, bruger_salt, fk_rolle_id, bruger_dato) VALUES (@1, @2, @3, @4, @5, @6)";
            cmd.Parameters.AddWithValue("@1", TextBox_Name.Text);
            cmd.Parameters.AddWithValue("@2", TextBox_Email.Text);
            cmd.Parameters.AddWithValue("@3", hash.ToString());
            cmd.Parameters.AddWithValue("@4", salt);
            cmd.Parameters.AddWithValue("@5", DropDownList_Role.SelectedValue);
            cmd.Parameters.AddWithValue("@6", DateTime.Now);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            Session["besked"] = "Bruger oprettet.";
            Response.Redirect("OprerBruger.aspx");
        }
        
    }
}