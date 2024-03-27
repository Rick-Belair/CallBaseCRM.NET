using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CallBaseMock.partials
{
    public partial class answer_zoom : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Issues"] != null)
                txtIssues.Text = Session["Issues"].ToString();

            if (Session["Answer"] != null)
                txtAnswer.Text = Session["Answer"].ToString();
        }
    }
}