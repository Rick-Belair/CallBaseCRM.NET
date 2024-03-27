using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;

namespace CallBaseMock.partials
{
    public partial class view_script : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TeleNo"] != null && Session["PageLanguage"] != null)
            {
                InboundDB db = new InboundDB();
                txtScript.Text = db.GetScript(Session["TeleNo"].ToString(), Session["PageLanguage"].ToString());
            }

        }//Page_Load

    }//class

}//namespace