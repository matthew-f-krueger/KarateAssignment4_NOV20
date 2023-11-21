using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KarateAssignment4.KarateMembers
{
    public partial class memberpage : System.Web.UI.Page
    {
        string conn = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\Luke Thompson\\Source\\Repos\\KarateAssignment4_NOV20\\App_Data\\KarateSchool.mdf\";Integrated Security=True;Connect Timeout=30";
        KarateDataContext dbcon;
        protected void Page_Load(object sender, EventArgs e)
        {
            dbcon = new KarateDataContext(conn);
            Label1.Text = HttpContext.Current.Session["memberFirst"].ToString() + " " + HttpContext.Current.Session["memberLast"].ToString();
            
            if (HttpContext.Current.Session["userType"].ToString().Trim() == "Instructor")
            {
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("Logon.aspx", true);
            }
            if (HttpContext.Current.Session["instructorID"].ToString() != "")
            {
                var result1 = from x in dbcon.Instructors
                              where x.InstructorID == Convert.ToInt32(HttpContext.Current.Session["instructorID"].ToString())
                              select new
                              {
                                  x.InstructorFirstName,
                                  x.InstructorLastName
                              };
                GridView1.DataSource = result1;
                GridView1.DataBind();

                var result2 = from x in dbcon.Sections
                              where x.Member_ID == Convert.ToInt32(HttpContext.Current.Session["userID"].ToString())
                              select new
                              {
                                  x.SectionName,
                                  x.SectionFee
                              };
                GridView2.DataSource = result2;
                GridView2.DataBind();
            }
            
        }
    }
}