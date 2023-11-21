using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KarateAssignment4.KarateInstructors
{
    public partial class instructorpage : System.Web.UI.Page
    {
        string conn = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\Luke Thompson\\Source\\Repos\\KarateAssignment4_NOV20\\App_Data\\KarateSchool.mdf\";Integrated Security=True;Connect Timeout=30";
        KarateDataContext dbcon;
        protected void Page_Load(object sender, EventArgs e)
        {
            dbcon = new KarateDataContext(conn);
           
            Label1.Text = HttpContext.Current.Session["memberFirst"].ToString() + " " + HttpContext.Current.Session["memberLast"].ToString();
              
            
           
            if (Session.Count != 0)
            {
                if (HttpContext.Current.Session["userType"].ToString().Trim() == "Member")
                {
                    Session.Clear();
                    Session.RemoveAll();
                    Session.Abandon();
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    Response.Redirect("Logon.aspx", true);
                }

            }

            if (HttpContext.Current.Session["sectionName"].ToString() != "")
            {
                Label2.Text = HttpContext.Current.Session["sectionName"].ToString();
                var result = from x in dbcon.Members
                             where x.Member_UserID == Convert.ToUInt32(HttpContext.Current.Session["sectionMemberID"].ToString())
                             select new
                             {
                                 x.MemberFirstName,
                                 x.MemberLastName
                             };
                GridView1.DataSource = result;
                GridView1.DataBind();
            }
            else
            {
                Label2.Text = "No section";
            }
            
            
             
            
            
            




        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}