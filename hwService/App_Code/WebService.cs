using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;

/// <summary>
/// WebService için özet açıklama
/// </summary>
[WebService(Namespace = "http://localhost/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Bu Web Hizmeti'nin, ASP.NET AJAX kullanılarak komut dosyasından çağrılmasına, aşağıdaki satırı açıklamadan kaldırmasına olanak vermek için.
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    string ConnectionName = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=d:\\Users\\Devrim Mert\\source\\repos\\hwService\\hwService\\App_Data\\Database.mdf;Integrated Security=True";

    public WebService()
    {

        //Tasarlanmış bileşenleri kullanıyorsanız şu satırı açıklamadan çıkarın
        //InitializeComponent(); 
    }

    [WebMethod]
    public string addUser(string userName, string userPassword)
    {
        string login = "You need register.";
        
      
        if (userName.Length == 0 || userPassword.Length == 0)
        {
            return "Fill the empty areas";
        }
        else
        {
      
            {
                SqlConnection conn = new SqlConnection(this.ConnectionName);
                string sql = "INSERT INTO users (userName,userPassword) values (@userName,@userPassword)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userName", userName);
                cmd.Parameters.AddWithValue("@userPassword", userPassword);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "Added";
                }
                catch(Exception ex)
                {
                    return ex.ToString();
                }
            }
        }
    }

    [WebMethod(enableSession: true)]
    public string Login(string userName, string userPassword)
    {
        string login = "ERROR";
        Session["login"] = 0;
        SqlConnection conn = new SqlConnection(this.ConnectionName);
        string sql = "SELECT * from users where userName = @userName AND userPassword = @userPassword ";
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@userName", userName);
        cmd.Parameters.AddWithValue("@userPassword", userPassword);
        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            Session["login"] = (Int32)reader["userID"];
            login = "SUCCESS";
        }
        conn.Close();
        reader.Close();
        return login;
    }

    [WebMethod(enableSession: true)]
    public string addEvent(string eventName, string eDate, string eAccommodation, string eTime)
    {

        string login = "You need login for this action.";

        if (Session["login"] == null)
        {
            return login;
        }
        else if (eventName.Length == 0 || eDate.Length == 0 || eAccommodation.Length == 0 || eTime.Length == 0)
        {
            return "Fill all inputs";
        }
        else
        {
            int userID = int.Parse(Session["login"].ToString());
            if (userID == 0)
            {
                return login;
            }
            else
            {
                SqlConnection conn = new SqlConnection(this.ConnectionName);
                string sql = "INSERT INTO Event (eventName,eDate,eAccommodation,eTime,userid) values (@eventName,@eDate,@eAccommodation,@eTime,@userid)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@eventName", eventName);
                cmd.Parameters.AddWithValue("@eDate", eDate);
                cmd.Parameters.AddWithValue("@eAccommodation", eAccommodation);
                cmd.Parameters.AddWithValue("@eTime", eTime);
                cmd.Parameters.AddWithValue("@userid", userID);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return "Added";
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }

            }
        }

    }

    [WebMethod]
    public DataSet getAllEvents()
    {

        SqlConnection conn = new SqlConnection(this.ConnectionName);
        string sql = "SELECT * FROM event ;";
        SqlDataAdapter daEvent = new SqlDataAdapter(sql, conn);

        string sql2 = "SELECT userid,username FROM users ;";
        SqlDataAdapter daUsers = new SqlDataAdapter(sql2, conn);

        DataSet ds = new DataSet();
        conn.Open();

        daEvent.Fill(ds, "events");
        daUsers.Fill(ds, "users");

        conn.Close();



        return ds;
    }
    [WebMethod]
    public string graphPage()
    {
        Context.Response.Redirect("~\\Default.aspx");
        return null;
    }
}

