using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Workflows;
using System.Data;

namespace CallBaseMock
{
    public partial class translation : System.Web.UI.Page
    {
        protected WCMSManager manager;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                manager = new WCMSManager();
                DataSet ds = manager.GetPageNames();
                ddlPage.DataSource = ds;
                ddlPage.DataTextField = "label_pagename";
                ddlPage.DataValueField = "label_pagename";
                ddlPage.DataBind();
                ddlPage.Items.Insert(0, new ListItem("All", ""));
            }
        }

    }//class

}//namespace