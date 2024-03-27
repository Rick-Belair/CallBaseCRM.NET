using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices.AccountManagement;
using System.Data;
using DataAccess;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;


namespace CallBaseMock
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            string lang = "EN";
            if (Session["PageLanguage"] != null)
                lang = Session["PageLanguage"].ToString();

            LanguageDB db = new LanguageDB();
            lnkLanguage.Text = db.GetLabel("InboundTracking", "LanguageSwitch", lang);
            lblAANDCHeader.Text = db.GetLabel("Home", "PublicEnquiriesINAC", lang);
            lblUserName.Text = db.GetLabel("Home", "UserName", lang).Trim() +":";
            lblPassword.Text = db.GetLabel("Home", "Password", lang) + ":";
            btnLogin.Text = db.GetLabel("Home", "Login", lang);
            lnkForgot.Text = db.GetLabel("Home", "ForgotYourPassword", lang);

        }//Page_Load

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string lang = "EN";
            if (Session["PageLanguage"] == null)
                Session["PageLanguage"] = lang;
            else
                lang = Session["PageLanguage"].ToString();

            /* The section immediately following this comment is for testing purposes, you can change the session variables to match whatever user and access you want to test
             * if you uncomment it it will ignore regular login logic and sign you in with the credentials provided

            Session["CurrentUser"] = "jeremy";
            Session["UserLevel"] = 3;
            Session["UserName"] = "Jeremy O'Neill";
            Session["UserAccess"] = "KIO,BC3,";
            Session["UserGroup"] = "KIO";
            Response.Redirect("inbound.aspx");
             */

            /* The section immediately following this comment is the real login logic, it will be ignored if you uncomment the section above */


            bool userExists = false;
            bool ldapLogin = false;
            LoginDB db = new LoginDB();
            LanguageDB langDB = new LanguageDB();

            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                lblError.Text = langDB.GetLabel("Home", "BothFields", lang);
                return;
            }
            else
            {
                ldapLogin = db.LDAPLogin();

                if (ldapLogin)
                {
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName))
                    {
                        // validate the credentials
                        userExists = pc.ValidateCredentials(txtUserName.Text, txtPassword.Text);

                        if (!userExists)
                            lblError.Text = langDB.GetLabel("Home", "UnrecognizedUsernamePassword", lang);
                        else
                            userExists = true;
                    }//using the domain

                }//ldapLogin

            }//else

            if (userExists || !ldapLogin)
            {
                DataSet ds;

                if (ldapLogin)
                    ds = db.GetUserInfoLAN(txtUserName.Text.ToUpper());
                else
                    ds = db.GetUserInfo(txtUserName.Text.ToUpper(), txtPassword.Text, "#dhksymd$w");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string go_to = HttpContext.Current.Request["go"];
                    //element at based on position in query
                    string userID = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                    Session["CurrentUser"] = userID;

                    string userName = ds.Tables[0].Rows[0].ItemArray.ElementAt(1).ToString();
                    Session["UserName"] = userName;

                    string userLevel = ds.Tables[0].Rows[0].ItemArray.ElementAt(2).ToString();
                    Session["UserLevel"] = userLevel;

                    string userGroup = ds.Tables[0].Rows[0].ItemArray.ElementAt(3).ToString();
                    Session["UserGroup"] = userGroup;

                    string userAccess = ds.Tables[0].Rows[0].ItemArray.ElementAt(4).ToString();
                    Session["UserAccess"] = userAccess;

                    string key = Utility.GetKey();
                    string iv = Utility.GetIV();

                    // string filePath = "/bin";
                    Process test = Process.GetCurrentProcess();
                    string filePath = AppDomain.CurrentDomain.BaseDirectory + "bin\\";
                    string text = Utility.Decrypt(System.IO.File.ReadAllText(filePath + "CallBase.df1"), key, iv);

                    int position = text.IndexOf("LIC:");
                    position += 9; // goes past LIC: then a space then NTK-
                    string lic = text.Substring(position, 21);

                    // bool userFound = System.IO.File.ReadAllText(filePath + "Callbase.df1").Contains(userID);
                    db.LogUser(userID);
                    // Response.Redirect("ContactManagement.aspx");
                    if (go_to == "cm")
                        Response.Redirect("ContactManagement.aspx");
                    else 
                        Response.Redirect("inbound.aspx");

                }//if there is a record (user is in db)

                else
                {
                    if (ldapLogin)
                        lblError.Text = langDB.GetLabel("Home", "NoAccess", lang);
                    else
                        lblError.Text = langDB.GetLabel("Home", "UnrecognizedUsernamePassword", lang);
                }

            }//if the user exists in AD or not using AD

        }//btnLogin_Click

        protected void lnkLanguage_Click(object sender, EventArgs e)
        {
            string currLang = "";
            if (Session["PageLanguage"] != null)
                currLang = Session["PageLanguage"].ToString();
            Session["PageLanguage"] = Utility.setLanguage(currLang);
            Response.Redirect(Request.RawUrl);

        }//lnkLanguage_Click

    }//class

}//namespace