using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Web.Services;

namespace CallBaseMock
{
    public partial class ClientActivityForm : System.Web.UI.Page
    {
        public string pageLang = "EN";
        public string userAccess = "";
        public string userGroup = "";
        public int userLevel = 5;
        public string currentUser = "";
        public string client_account = "";
        public string salutation = "";
        public string prov_code = "";
        public string c_language = "EN";
        public int c_status = 0;
        public string c_customer_type = "";
        public string c_delivery_mode = "";
        public string clientName = "";
        public string item = "";

        ClientDB clientDB = new ClientDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "checkIE", "CheckIEVersion();", true);
            setConstants();
            int x = 0;
            if (Session["C_REC_NO"] != null)
                client_account = Session["C_REC_NO"].ToString();

            item = HttpContext.Current.Request["item"];

            tree_view.Nodes.Clear();


            if (Int32.TryParse(client_account, out x) && x > 0)
            {
                PopulateTreeView(client_account);

                DataSet ds = clientDB.GetClientDetails(client_account);

                // List<Client> clients = new List<Client>();

                Client the_client = new Client();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection clientRows = ds.Tables[0].Rows;
                    the_client.c_rec_no = Int32.Parse(clientRows[0]["C_REC_NO"].ToString());

                    //C_DATE_INPUT, C_DATE_AMENDED, C_OPERATOR, C_OWNER, C_USER_GRP, C_DATE_USED
                    /*
                        public string c_date_input { get; set; }
                        public string c_date_amended { get; set; }
                        public string c_operator { get; set; }
                        public string c_owner { get; set; }
                        public string c_user_grp { get; set; }
                        public string c_date_used { get; set; }                     
                    */
                    the_client.c_date_input = clientRows[0]["C_DATE_INPUT"].ToString();
                    the_client.c_date_amended = clientRows[0]["C_DATE_AMENDED"].ToString();
                    the_client.c_operator = clientRows[0]["C_OPERATOR"].ToString();
                    the_client.c_owner = clientRows[0]["C_OWNER"].ToString();
                    the_client.c_user_grp = clientRows[0]["C_USER_GRP"].ToString();
                    the_client.c_date_used = clientRows[0]["C_DATE_USED"].ToString();
                    the_client.c_firstname_intl = clientRows[0]["C_FIRSTNAME_INTL"].ToString();
                    the_client.c_surname = clientRows[0]["C_SURNAME"].ToString();
                    clientName = the_client.c_firstname_intl + " " + the_client.c_surname;

                    if (!IsPostBack)
                    {
                        /*
                        if (c != null && c.ID == "btnFind")
                        {
                        }
                        */
                        txtAccount.Text = the_client.c_rec_no.ToString();

                        txtDateUsed.Text = the_client.c_date_used;
                        txtDateInput.Text = the_client.c_date_input;
                        txtInputBy.Text = the_client.c_operator;
                        txtEditBy.Text = the_client.c_operator;
                        txtAmended.Text = the_client.c_date_amended;
                        txtGroup.Text = the_client.c_user_grp;

                        DateTime today = DateTime.Today;
                        txtDate.Text = today.ToString("dd-MMM-yyyy");
                        int z = 0;
                        if (item != null && item != "" && item.Length >= 1)
                        {                            
                            if (Int32.TryParse(item, out z) && z > 0)
                                txtItem.Text = z.ToString();
                        }

                        loadDDLType(ddlType, "", pageLang);
                        loadDDLPriority(ddlPriority, "", pageLang);
                        loadDDLSTatus(ddlStatus, "", pageLang);
                        loadDDLSubject(ddlSubject, "", pageLang);
                        loadDDLContactProjects(ddlContactsProjects, "", the_client.c_rec_no, true,  pageLang);
                        loadDDLContactProjects(ddlAllContactsProjects, "", the_client.c_rec_no, false, pageLang);
                    }
                }
            }
            setLabel();
        }

        private void setConstants()
        {
            bool sessionActive = true;

            if (Session["PageLanguage"] != null)
                pageLang = Session["PageLanguage"].ToString();
            else
                sessionActive = false;

            if (Session["UserAccess"] != null)
                userAccess = Session["UserAccess"].ToString();
            else
                sessionActive = false;

            if (Session["UserGroup"] != null)
                userGroup = Session["UserGroup"].ToString();
            else
                sessionActive = false;

            if (Session["UserLevel"] != null)
                userLevel = int.Parse(Session["UserLevel"].ToString());
            else
                sessionActive = false;

            if (Session["CurrentUser"] != null)
                currentUser = Session["CurrentUser"].ToString();
            else
                sessionActive = false;

            if (!sessionActive)
                Response.Redirect("login.aspx");

        }//setConstants

        protected void lnkLanguage_Click(object sender, EventArgs e)
        {
            string currLang = "";
            if (Session["PageLanguage"] != null)
                currLang = Session["PageLanguage"].ToString();
            Session["PageLanguage"] = Utility.setLanguage(currLang);
            Response.Redirect(Request.RawUrl);

        }//lnkLanguage_Click

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            setConstants();
            LoginDB db = new LoginDB();
            db.LogUserOut(currentUser);
            Response.Redirect("login.aspx");
        }

        protected void GoToContactManagement(object sender, EventArgs e)
        {
            int y = 0;
            if (Int32.TryParse(client_account, out y) && y > 0)
            {
                Response.Redirect("ContactManagement.aspx?account=" + y);
            }
        }

        protected void btnNewClick(object sender, EventArgs e)
        {
        }

        protected void btnEditClick(object sender, EventArgs e)
        {
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
        }
        
        protected void btnSaveClick(object sender, EventArgs e)
        {
        }//btnSaveClick
        
        protected void btnDelete_Click(object sender, EventArgs e)
        {
        }// btnDelete_Click

        protected string DelMessage
        {
            get
            {
                setConstants();
                LanguageDB db = new LanguageDB();
                return db.GetLabel("ClientEdit", "CEDeleteClient", pageLang);
            }

        }//

        private void PopulateTreeView(string customerAccount)
        {
            DataSet ds = clientDB.GetClientContacts(customerAccount);
            DataTable dtParent = new DataTable();
            DataRowCollection dataRowCollection;
            string the_root_text = "";
            /*
             select CA_SEQ, 
                TO_CHAR(CA_DATE, 'DD-MON-YYYY') || ', ' || CA_SUBJECT as subject, 
                CA_COMM 
                from F_CLIENT_ACTIVITY where CA_CLIENT = 41960 
                order by CA_COMM; 
             */
            string ca_comm = "";
            string url = "ClientActivityForm.aspx";

            int j = 1;
            TreeNode child_parent, child;
            child_parent = new TreeNode();

            if (ds.Tables[0].Rows.Count > 0)
            {
                dataRowCollection = ds.Tables[0].Rows;
                for (int i = 0; i < dataRowCollection.Count; i++)
                {
                    child = new TreeNode
                    {
                        Text = dataRowCollection[i]["SUBJECT"].ToString(),
                        Value = dataRowCollection[i]["CA_SEQ"].ToString(),
                        NavigateUrl = url + "?item=" + dataRowCollection[i]["CA_SEQ"].ToString()
                    };
                    
                    ca_comm = dataRowCollection[i]["CA_COMM"].ToString();
                    ca_comm = GetNodeTitle(ca_comm);

                    if (the_root_text != ca_comm)
                    {
                        child_parent = new TreeNode
                        {
                            Text = ca_comm,
                            Value = j.ToString()
                        };
                        tree_view.Nodes.Add(child_parent);
                        the_root_text = ca_comm;
                        j++;
                    }
                    child_parent.ChildNodes.Add(child);
                    
                }
                tree_view.CollapseAll();
            }
        }

        private string GetNodeTitle(string comm)
        {
            string the_text = "";
            LanguageDB db = new LanguageDB();
            /*
            -- Outbound          TrOutbound
            Mailing List      TrMailings
            Callback Email    TrEmail
            E-Mail            TrEmail
            Phone Call        TrPhone
            Note to File      TrNote
            Letter            TrLetter
            Fax               TrFax
            */
            switch (comm)
            {
                case "Mailing List":
                    the_text = db.GetLabel("ClientTree", "TrMailings", pageLang);
                    break;
                case "Callback Email": 
                    the_text = db.GetLabel("ClientTree", "TrEmail", pageLang);
                    break;
                case "E-Mail":
                    the_text = db.GetLabel("ClientTree", "TrEmail", pageLang);
                    break;
                case "Phone Call":
                    the_text = db.GetLabel("ClientTree", "TrPhone", pageLang);
                    break;
                case "Note to File":
                    the_text = db.GetLabel("ClientTree", "TrNote", pageLang);
                    break;
                case "Letter":
                    the_text = db.GetLabel("ClientTree", "TrLetter", pageLang);
                    break;
                case "Fax":
                    the_text = db.GetLabel("ClientTree", "TrFax", pageLang);
                    break;
            }
            return the_text;
        }

        protected void btnFormLetterClick(object sender, EventArgs e)
        {
        }

        protected void btnProgramInfoClick(object sender, EventArgs e)
        {
        }

        private void loadDDLType(DropDownList theDDLType, string theCommType, string page_lang)
        {
            DataRowCollection dataRowCollection;
            DataTable types = new DataTable();
            DataSet dataSet1 = clientDB.GetClientCommType(page_lang);

            types.Columns.Add("COMM_SEQ", typeof(string));
            types.Columns.Add("COMM_TITLE", typeof(string));

            if (dataSet1.Tables[0].Rows.Count > 0)
            {
                dataRowCollection = dataSet1.Tables[0].Rows;

                for (int i = 0; i < dataRowCollection.Count; ++i)
                {
                    types.Rows.Add(dataRowCollection[i]["COMM_SEQ"].ToString(), dataRowCollection[i]["COMM_TITLE"].ToString());
                }

                theDDLType.DataSource = types;
                theDDLType.DataTextField = "COMM_TITLE";
                theDDLType.DataValueField = "COMM_SEQ";

                theDDLType.DataBind();
                ListItem selectedListItem;
                if (theCommType != "" && theCommType.Length >= 1 )
                    selectedListItem = theDDLType.Items.FindByValue(theCommType);
                else
                    selectedListItem = theDDLType.Items.FindByValue("1");

                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }

        private void loadDDLPriority(DropDownList theDDLPriority, string thePriority, string page_lang)
        {
            DataTable priority_table = new DataTable();
            string local_priority = "0";

            if (thePriority != "" && thePriority.Length == 1)
                local_priority = thePriority;

            LanguageDB db = new LanguageDB();

            string theLow = db.GetLabel("ClientActivity", "CALow", page_lang);
            string theMedium = db.GetLabel("ClientActivity", "CAMedium", page_lang);
            string theHigh = db.GetLabel("ClientActivity", "CAHigh", page_lang);

            priority_table.Columns.Add("priority_id", typeof(string));
            priority_table.Columns.Add("priority", typeof(string));

            priority_table.Rows.Add("1", theLow);
            priority_table.Rows.Add("2", theMedium);
            priority_table.Rows.Add("3", theHigh);

            theDDLPriority.DataSource = priority_table;
            theDDLPriority.DataTextField = "priority";
            theDDLPriority.DataValueField = "priority_id";

            theDDLPriority.DataBind();

            ListItem selectedListItem = theDDLPriority.Items.FindByValue(local_priority);

            if (selectedListItem != null)
            {
                selectedListItem.Selected = true;
            }
        }

        private void loadDDLSTatus(DropDownList theDDLStatus, string theStatusType, string page_lang)
        {
            DataRowCollection dataRowCollection;
            DataTable types = new DataTable();
            DataSet dataSet1 = clientDB.GetClientActivityStatus(page_lang);

            types.Columns.Add("CL_STAT_SEQ", typeof(string));
            types.Columns.Add("CL_STAT_DESC", typeof(string));

            if (dataSet1.Tables[0].Rows.Count > 0)
            {
                dataRowCollection = dataSet1.Tables[0].Rows;

                for (int i = 0; i < dataRowCollection.Count; ++i)
                {
                    types.Rows.Add(dataRowCollection[i]["CL_STAT_SEQ"].ToString(), dataRowCollection[i]["CL_STAT_DESC"].ToString());
                }

                theDDLStatus.DataSource = types;
                theDDLStatus.DataTextField = "CL_STAT_DESC";
                theDDLStatus.DataValueField = "CL_STAT_SEQ";

                theDDLStatus.DataBind();
                ListItem selectedListItem = new ListItem();
                if (theStatusType != "" && theStatusType.Length >= 1)
                    selectedListItem = theDDLStatus.Items.FindByValue(theStatusType);

                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }
        // ACTION_SEQ, ACTION_TEXT
        private void loadDDLSubject(DropDownList theDDLSubject, string theSubject, string page_lang)
        {
            DataRowCollection dataRowCollection;
            DataTable types = new DataTable();
            DataSet dataSet1 = clientDB.GetClientActivitySubjects(page_lang);

            types.Columns.Add("ACTION_SEQ", typeof(string));
            types.Columns.Add("ACTION_TEXT", typeof(string));

            if (dataSet1.Tables[0].Rows.Count > 0)
            {
                dataRowCollection = dataSet1.Tables[0].Rows;

                for (int i = 0; i < dataRowCollection.Count; ++i)
                {
                    types.Rows.Add(dataRowCollection[i]["ACTION_SEQ"].ToString(), dataRowCollection[i]["ACTION_TEXT"].ToString());
                }

                theDDLSubject.DataSource = types;
                theDDLSubject.DataTextField = "ACTION_TEXT";
                theDDLSubject.DataValueField = "ACTION_SEQ";

                theDDLSubject.DataBind();
                ListItem selectedListItem = new ListItem();
                if (theSubject != "" && theSubject.Length >= 1)
                    selectedListItem = theDDLSubject.Items.FindByValue(theSubject);

                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }

        private void loadDDLContactProjects(DropDownList theDDLContactProjects, string theContactProject, int c_rec_no, bool client_associations, string page_lang)
        {
            DataRowCollection dataRowCollection;
            DataTable types = new DataTable();
            DataSet dataSet1 = clientDB.GetContactListAssocDiassocWithClient(client_associations, c_rec_no.ToString());

            types.Columns.Add("CON_SEQUENCE", typeof(string));
            types.Columns.Add("CON_DEF", typeof(string));

            if (dataSet1.Tables[0].Rows.Count > 0)
            {
                dataRowCollection = dataSet1.Tables[0].Rows;

                for (int i = 0; i < dataRowCollection.Count; ++i)
                {
                    types.Rows.Add(dataRowCollection[i]["CON_SEQUENCE"].ToString(), dataRowCollection[i]["CON_CODE"].ToString() + " - " + dataRowCollection[i]["CON_DEF"].ToString());
                }

                theDDLContactProjects.DataSource = types;
                theDDLContactProjects.DataTextField = "CON_DEF";
                theDDLContactProjects.DataValueField = "CON_SEQUENCE";

                theDDLContactProjects.DataBind();
                ListItem selectedListItem = new ListItem();
                if (theContactProject != "" && theContactProject.Length >= 1)
                    selectedListItem = theDDLContactProjects.Items.FindByValue(theContactProject);

                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }



        private void setLabel()
        {
            LanguageDB db = new LanguageDB();

            #region copied_labels
            lblAANDCHeader.Text = db.GetLabel("Home", "PublicEnquiriesINAC", pageLang);
            lnkLogOut.Text = db.GetLabel("Home", "Logout", pageLang);
            lnkLanguage.Text = db.GetLabel("InboundTracking", "LanguageSwitch", pageLang);
            /*
            btnInbound.Text = db.GetLabel("InboundTracking", "btnInboundStats", pageLang);
            btnKnowledge.Text = db.GetLabel("InboundTracking", "btnKnowledgeBase", pageLang);
            btnProduct.Text = db.GetLabel("InboundTracking", "btnProductListing", pageLang);            
            btnOrderStatus.Text = db.GetLabel("InboundTracking", "AssignOrders", pageLang);
            */
            //btnReports.Text = db.GetLabel("SkanList", "Reports2", pageLang);

            //lblCustDetails.Text = db.GetLabel("ClientEdit", "CEContactInfo", pageLang);
            lblCustomerAccount.Text = db.GetLabel("ClientEdit", "CEAccountNo", pageLang);

            /*
            string phone = db.GetLabel("InboundTracking", "Phone", pageLang);
            lblPhone.Text = phone;            
            lblPostZip.Text = db.GetLabel("InboundTracking", "PostZipCode", pageLang);
            lblFax.Text = db.GetLabel("InboundTracking", "Fax", pageLang);
            string organization = db.GetLabel("InboundTracking", "Organization", pageLang);
            lblOrganization.Text = organization;
            string email = db.GetLabel("InboundTracking", "Email", pageLang);
            lblEmail.Text = email;
            string provState = db.GetLabel("InboundTracking", "ProvState", pageLang);
            lblProvState2.Text = provState;
            lblCountry.Text = db.GetLabel("InboundTracking", "Country", pageLang);
            lblCity.Text = db.GetLabel("InboundTracking", "City", pageLang);
            lblAddress.Text = db.GetLabel("InboundTracking", "Address", pageLang);
            */
            string closeText = db.GetLabel("InboundTracking", "Close", pageLang);
            //btnClose.Text = closeText;
            /*
            btnCloseIssues.Text = closeText;
            lblCurrentComment.Text = db.GetLabel("InboundTracking", "CurrentCommentDetails", pageLang) + ":";
            lblCommentHist.Text = db.GetLabel("InboundTracking", "CommentHistoryforTicket#", pageLang) + ":";
            lblMoreDetails.Text = db.GetLabel("InboundTracking", "Clickonlinetoseemoredetails", pageLang) + ".";
            string dateText = db.GetLabel("InboundTracking", "Date", pageLang);
            gvCommentHistory.Columns[1].HeaderText = dateText;
            string userText = db.GetLabel("InboundTracking", "User", pageLang);
            gvCommentHistory.Columns[2].HeaderText = userText;
            gvCommentHistory.Columns[3].HeaderText = db.GetLabel("InboundTracking", "OriginalComment", pageLang);
            lblSelComment.Text = db.GetLabel("InboundTracking", "CommentDetailsfromselectedhistory line", pageLang);
            string noResults = db.GetLabel("RptR19a", "R19aRepNoResult", pageLang);
            //lblDescDetKBD.Text = 
            gvCommentHistory.EmptyDataText = db.GetLabel("InboundTracking", "NoCommentHist", pageLang);
            //lblBreadcrumb.Text = btnInbound.Text;
            
            */
            #endregion copied_labels
            // buttons
            btnReturn.Text = db.GetLabel("ClientEdit", "btnReturn", pageLang);
            /*
             *  --65   Home	mnuContactManagement	Y	16-NOV-09
                --69   Home	mnuInventory	        Y	16-NOV-09
                --46   Home	Reports	                Y	16-NOV-09
                --26   Home	Maintenance	            Y	16-NOV-09
            */
            btnContactManagement.Text = db.GetLabel("Home", "mnuContactManagement", pageLang).ToUpper();
            btnInventory.Text = db.GetLabel("Home", "mnuInventory", pageLang).ToUpper();
            btnReports.Text = db.GetLabel("Home", "Reports", pageLang).ToUpper();
            btnMaintenance.Text = db.GetLabel("Home", "Maintenance", pageLang).ToUpper();

            lblDateUsed.Text = db.GetLabel("ClientEdit", "CEDateUsed", pageLang);
            lblInserted.Text = db.GetLabel("ClientEdit", "CEInserted", pageLang);
            lblAmended.Text = db.GetLabel("ClientEdit", "CEAmended", pageLang);
            lblInputBy.Text = db.GetLabel("ClientEdit", "CEInputBy", pageLang);
            lblEditBy.Text = db.GetLabel("ClientEdit", "CEEditBy", pageLang);
            lblGroup.Text = db.GetLabel("ClientEdit", "CEGroup", pageLang);
            /*
               -- CEAllContactProjectCodes, CEClientContactProjects, Code, DateIn, 
               -- DateLastUsed, Definition, btnAdd, btnRemove
            
                lblCEAllContactProjectCodes.Text = db.GetLabel("ClientEdit", "CEAllContactProjectCodes", pageLang);
                lblCEClientContactProjects.Text = db.GetLabel("ClientEdit", "CEClientContactProjects", pageLang);
                lblCode.Text = db.GetLabel("ClientEdit", "Code", pageLang);
                lblCode1.Text = db.GetLabel("ClientEdit", "Code", pageLang);
                lblDefinition.Text = db.GetLabel("ClientEdit", "Definition", pageLang);
                lblDefinition1.Text = db.GetLabel("ClientEdit", "Definition", pageLang);
                lblDateIn.Text = db.GetLabel("ClientEdit", "DateIn", pageLang);
                lblDateLastUsed.Text = db.GetLabel("ClientEdit", "DateLastUsed", pageLang);
                btnAdd.Text = "<< " + db.GetLabel("ClientEdit", "btnAdd", pageLang) + " <<";
                btnRemove.Text = ">> " + db.GetLabel("ClientEdit", "btnRemove", pageLang) + " >>";
            */
            lblOutboundActivity.Text = db.GetLabel("ClientActivity", "CAOutbound", pageLang);
            lblCommFor.Text = db.GetLabel("ClientActivity", "CACommFor", pageLang) + " " + clientName;
            lblDateDone.Text = db.GetLabel("ClientActivity", "CADateDone", pageLang);
            lblPriority.Text = db.GetLabel("ClientActivity", "CAPriority", pageLang);
            lblStatus.Text = db.GetLabel("ClientActivity", "CAStatus", pageLang);
            lblSubject.Text = db.GetLabel("ClientActivity", "CASubject", pageLang);
            lblContactsProjects.Text = db.GetLabel("ClientActivity", "CAContactProjects", pageLang);
            lblAllContactsProjects.Text = db.GetLabel("ClientActivity", "CAAllContactProjects", pageLang);

            lblDetails.Text = db.GetLabel("ClientActivity", "CADetails", pageLang);
            btnFormLetter.Text = db.GetLabel("ClientActivity", "CAFormLet", pageLang);
            btnProgramInfo.Text = db.GetLabel("ClientActivity", "CAProgramInfo", pageLang);

            lblItem.Text = db.GetLabel("ClientActivity", "CAItem", pageLang);

        }

        private string GetTxtInqNumber(string page_lang)
        {
            string the_text = "Account #";
            if (pageLang == "FR")
                the_text = "Compte #";

            return the_text;
        }
    }
}