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
//using System.Data.OracleClient;

namespace CallBaseMock
{

    public partial class ClientProjects : System.Web.UI.Page
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

        ClientDB clientDB = new ClientDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "checkIE", "CheckIEVersion();", true);
            setConstants();

            // clear table tblClientContactProjects
            for (int i = 1; i < tblClientContactProjects.Rows.Count; i++)
            {
                tblClientContactProjects.Rows.RemoveAt(i);
            }
            // clear table tblClientAllContactProjects.
            for (int i = 1; i < tblClientAllContactProjects.Rows.Count; i++)
            {
                tblClientAllContactProjects.Rows.RemoveAt(i);
            }

            int x = 0;
            if (Session["C_REC_NO"] != null)
                client_account = Session["C_REC_NO"].ToString();
            
            if (Int32.TryParse(client_account, out x) && x > 0)
            {

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
                    }
                }

                ds = clientDB.GetContactListAssocDiassocWithClient(true, client_account);
                //DataSet ds1 = clientDB.GetContactListAssocDiassocWithClient(false, client_account);
                // CON_SEQUENCE, CON_CODE, CON_DEF, CL_DATE_IN, CL_DATE_OUT
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection clientContactRows = ds.Tables[0].Rows;

                    for (int i = 0; i < clientContactRows.Count; ++i)
                    {
                        TableRow row = new TableRow();
                        TableCell cell = new TableCell();

                        CheckBox cb = new CheckBox();
                        cb.ID = clientContactRows[i]["CON_SEQUENCE"].ToString();
                        cb.Attributes.Add("onClick", "colorRow(this);"); // = "javascript:colorRow(this);";

                        cell.Controls.Add(cb);
                        row.Cells.Add(cell);

                        TableCell cell1 = new TableCell();
                        TableCell cell2 = new TableCell();
                        TableCell cell3 = new TableCell();
                        TableCell cell4 = new TableCell();

                        cell1.Text = clientContactRows[i]["CON_CODE"].ToString();
                        row.Cells.Add(cell1);

                        cell2.Text = clientContactRows[i]["CON_DEF"].ToString();
                        row.Cells.Add(cell2);

                        cell3.Text = clientContactRows[i]["CL_DATE_IN"].ToString();
                        row.Cells.Add(cell3);

                        cell4.Text = clientContactRows[i]["CL_DATE_OUT"].ToString();
                        row.Cells.Add(cell4);

                        tblClientContactProjects.Rows.Add(row);
                    }
                }

                ds = clientDB.GetContactListAssocDiassocWithClient(false, client_account);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection clientContactRows = ds.Tables[0].Rows;

                    for (int i = 0; i < clientContactRows.Count; ++i)
                    {
                        TableRow row = new TableRow();
                        TableCell cell = new TableCell();

                        TableCell cell1 = new TableCell();
                        TableCell cell2 = new TableCell();
                        CheckBox cb = new CheckBox();
                        cb.ID = clientContactRows[i]["CON_SEQUENCE"].ToString();
                        cb.Attributes.Add("onClick", "colorRow(this);"); // = "javascript:colorRow(this);";

                        cell.Controls.Add(cb);
                        row.Cells.Add(cell);

                        cell1.Text = clientContactRows[i]["CON_CODE"].ToString();
                        row.Cells.Add(cell1);

                        cell2.Text = clientContactRows[i]["CON_DEF"].ToString();
                        row.Cells.Add(cell2);

                        tblClientAllContactProjects.Rows.Add(row);
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
        /*
        protected string setSelectText()
        {
            setConstants();
            LanguageDB db = new LanguageDB();
            return db.GetLabel("InboundTracking", "Select", pageLang);

        }//setSelectText

        protected void gvCommentHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvCommentHistory.SelectedRow;
            string issue = row.Cells[3].Text;
            if (!issue.ToLower().Equals("&nbsp;"))
                txtSelComment.Text = row.Cells[3].Text;
            else
                txtSelComment.Text = "";

        }//gvCommentHistory_SelectedIndexChanged
        */

        protected void btnAddClick(object sender, EventArgs e)
        {
            string seqList = "";

            foreach (TableRow row in tblClientAllContactProjects.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    foreach (Control ctrl in cell.Controls)
                    {
                        if (ctrl is CheckBox)
                        {
                            CheckBox cb = (CheckBox)ctrl;
                            if (cb.Checked)
                            {
                                if (!seqList.Equals("") && seqList.Length > 0)
                                    seqList = seqList + ",";

                                seqList = seqList + cb.ID;
                              
                            }
                        }
                    }
                }
            }

            clientDB.AddContactLinkRecords(seqList, Session["C_REC_NO"].ToString(), currentUser);
            Response.Redirect("ClientProjects.aspx");
        }

        protected void btnRemoveClick(object sender, EventArgs e)
        {
            string seqList = "";

            foreach (TableRow row in tblClientContactProjects.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    foreach (Control ctrl in cell.Controls)
                    {
                        if (ctrl is CheckBox)
                        {
                            CheckBox cb = (CheckBox)ctrl;
                            if (cb.Checked)
                            {
                                if (!seqList.Equals("") && seqList.Length > 0)
                                    seqList = seqList + ",";

                                seqList = seqList + cb.ID;

                            }
                        }
                    }
                }
            }

            clientDB.DeleteContactLinkRecords(seqList);
            Response.Redirect("ClientProjects.aspx");
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
            */
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