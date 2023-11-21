using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KarateAssignment4
{
    public partial class Logon : System.Web.UI.Page
    {
        string conn = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\Luke Thompson\\Source\\Repos\\KarateAssignment4_NOV20\\App_Data\\KarateSchool.mdf\";Integrated Security=True;Connect Timeout=30";
        KarateDataContext dbcon;
        protected void Page_Load(object sender, EventArgs e)
        {
            dbcon = new KarateDataContext(conn);
            UnobtrusiveValidationMode =
            UnobtrusiveValidationMode.None;
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            string nUserName = Login1.UserName;
            string nPassword = Login1.Password;

            HttpContext.Current.Session["nUserName"] = nUserName;
            HttpContext.Current.Session["uPass"] = nPassword;

            // Search for the current User, validate UserName and Password
            NetUser myUser = (from x in dbcon.NetUsers where x.UserName == HttpContext.Current.Session["nUserName"].ToString() && x.UserPassword == HttpContext.Current.Session["uPass"].ToString() select x).First();

            if (myUser != null)
            {
                // Add UserId and User type to the session
                HttpContext.Current.Session["userID"] = myUser.UserID;
                HttpContext.Current.Session["userType"] = myUser.UserType;
                if (HttpContext.Current.Session["userType"].ToString() == "Member")
                {
                    Member member = (from x in dbcon.Members where x.Member_UserID == myUser.UserID select x).First();
                    HttpContext.Current.Session["memberLast"] = member.MemberLastName;
                    HttpContext.Current.Session["memberFirst"] = member.MemberFirstName;

                }
                if (HttpContext.Current.Session["userType"].ToString() == "Instructor")
                {
                    Instructor member = (from x in dbcon.Instructors where x.InstructorID == myUser.UserID select x).First();
                    HttpContext.Current.Session["memberLast"] = member.InstructorLastName;
                    HttpContext.Current.Session["memberFirst"] = member.InstructorFirstName;
                    Section section = (from x in dbcon.Sections where x.Instructor_ID == myUser.UserID select x).First();
                    HttpContext.Current.Session["sectionName"] = section.SectionName;
                    HttpContext.Current.Session["sectionMemberID"] = section.Member_ID;
                }
            }
            if (myUser != null && HttpContext.Current.Session["userType"].ToString() == "Member")
            {
                FormsAuthentication.RedirectFromLoginPage(HttpContext.Current.Session["nUserName"].ToString(), true);
                Response.Redirect("~/KarateMembers/memberpage.aspx");
            }
            else if (myUser != null && HttpContext.Current.Session["userType"].ToString() == "Instructor")
            {
                FormsAuthentication.RedirectFromLoginPage(HttpContext.Current.Session["nUserName"].ToString(), true);
                Response.Redirect("~/KarateInstructors/instructorpage.aspx");
            }
            else
                Response.Redirect("Logon.aspx", true);
        }
    }
    }
