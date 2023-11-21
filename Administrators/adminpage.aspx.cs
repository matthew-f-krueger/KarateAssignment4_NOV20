using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KarateAssignment4.Administrators
{
    
    public partial class adminpage : System.Web.UI.Page
    {
        string conn = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\Luke Thompson\\Source\\Repos\\KarateAssignment4_NOV20\\App_Data\\KarateSchool.mdf\";Integrated Security=True;Connect Timeout=30";
        KarateDataContext dbcon;
        protected void Page_Load(object sender, EventArgs e)
        {
            dbcon = new KarateDataContext(conn);
            var query = from x in dbcon.Members
                        select new
                        {
                            x.MemberFirstName
                ,
                            x.MemberLastName
                ,
                            x.MemberDateJoined
                ,
                            x.MemberPhoneNumber
                        };
            GridView1.DataSource = query;
            GridView1.DataBind();

            var query2 = from x in dbcon.Instructors
                         select new
                         {
                             x.InstructorFirstName,
                             x.InstructorLastName
                         };
            GridView2.DataSource = query2;
            GridView2.DataBind();
        }

        protected void addInstructorBtn_Click(object sender, EventArgs e)
        {
            string userName = instructorUserNameTxtBx.Text;
            string userPassword = instructorPasswordTxtBx.Text;
            string fName = instructorFNameTxtBx.Text;
            string lNAme = instructorLNameTxtBx.Text;
            string phoneNum = instructorPhoneNumTxtBx.Text;
            NetUser user = new NetUser();
            user.UserName = userName;
            user.UserPassword = userPassword;
            user.UserType = "Instructor";
            
            dbcon.NetUsers.InsertOnSubmit(user);
            dbcon.SubmitChanges();
            int id = user.UserID;


            Instructor add = new Instructor();
            add.InstructorID = id;
            add.InstructorFirstName = fName;
            add.InstructorLastName = lNAme;
            add.InstructorPhoneNumber = phoneNum;

            dbcon.Instructors.InsertOnSubmit(add);
            dbcon.SubmitChanges();
            var query2 = from x in dbcon.Instructors
                         select new
                         {
                             x.InstructorFirstName,
                             x.InstructorLastName
                         };
            GridView2.DataSource = query2;
            GridView2.DataBind();

        }

        protected void addMemberBtn_Click(object sender, EventArgs e)
        {

            string fName = memberFNameTxtBx.Text;
            string lName = memberLNameTxtBx.Text;
            DateTime date = Convert.ToDateTime(memberDateTxtBx.Text);
            string phoneNum = memberPhoneNumTxtBx.Text;
            string email = memberEmailTxtBx.Text;
            NetUser user = new NetUser();
            user.UserName = memberUserNameTxtBx.Text;
            user.UserPassword = memberPasswordTxtBx.Text;
            user.UserType = "Member";
            dbcon.NetUsers.InsertOnSubmit(user);
            dbcon.SubmitChanges();
            int id = user.UserID;

            Member add = new Member();
            add.MemberFirstName = fName;
            add.MemberLastName = lName;
            add.MemberPhoneNumber = phoneNum;
            add.MemberEmail = email;
            add.MemberDateJoined = date;
            add.Member_UserID = id;

            dbcon.Members.InsertOnSubmit(add);
            dbcon.SubmitChanges();

            var query = from x in dbcon.Members
                        select new
                        {
                            x.MemberFirstName
                ,
                            x.MemberLastName
                ,
                            x.MemberDateJoined
                ,
                            x.MemberPhoneNumber
                        };
            GridView1.DataSource = query;
            GridView1.DataBind();
        }

        protected void deleteMemberBtn_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(memberUserIDTxtBx.Text);
            NetUser netUser = (from x in dbcon.NetUsers where x.UserID == id select x).First();
            dbcon.NetUsers.DeleteOnSubmit(netUser);
            dbcon.SubmitChanges();
            Member user = (from x in dbcon.Members where x.Member_UserID == id select x).First();

            dbcon.Members.DeleteOnSubmit(user);
            dbcon.SubmitChanges();
        }

        protected void deleteInstructorBtn_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TextBox2.Text);
            NetUser netUser = (from x in dbcon.NetUsers where x.UserID == id select x).First();
            dbcon.NetUsers.DeleteOnSubmit(netUser);
            dbcon.SubmitChanges();
            Instructor user = (from x in dbcon.Instructors where x.InstructorID == id select x).First();
            dbcon.Instructors.DeleteOnSubmit(user);
            dbcon.SubmitChanges();
        }
    }
}