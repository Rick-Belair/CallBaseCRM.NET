using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;

namespace CallBaseMock
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string lang = "EN";
            if (Session["PageLanguage"] != null)
                lang = Session["PageLanguage"].ToString();
            LanguageDB db = new LanguageDB();
            lblYearDeveloped.Text = "© 2003-"+ DateTime.Today.Year + " " + db.GetLabel("Home", "DevelopedBy", lang); 
        }

    }//class

}//namespace