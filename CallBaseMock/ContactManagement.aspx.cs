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
    public partial class ContactManagement : System.Web.UI.Page
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

            if (Request.UrlReferrer != null)
            {
                string previousPageUrl = Request.UrlReferrer.AbsoluteUri;
                string previousPageName = System.IO.Path.GetFileName(Request.UrlReferrer.AbsolutePath);
                if (previousPageName == "inbound.aspx")
                {
                    btnReturn.Visible = true;
                    btnNew.Visible = false;

                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    //btnCancel.Enabled = false;
                    btnFind.Enabled = false;
                    btnSave.Enabled = false;
                    //btnReturn.Enabled = false;
                    btnClientActivity.Enabled = false;
                    btnInboundOrders.Enabled = false;
                    btnContactProjects.Enabled = false;
                    btnContactManagement.Enabled = false;
                    btnInventory.Enabled = false;
                    btnReports.Enabled = false;
                    btnMaintenance.Enabled = false;

                    btnFirst.Enabled = false;
                    btnFirst.CssClass = "movementbutton_disabeld";
                    btnNext.Enabled = false;
                    btnNext.CssClass = "movementbutton_disabeld";
                    btnPrev.Enabled = false;
                    btnPrev.CssClass = "movementbutton_disabeld";
                    btnLast.Enabled = false;
                    btnLast.CssClass = "movementbutton_disabeld";

                }
                else
                {
                    btnReturn.Visible = false;
                    btnNew.Visible = true;

                    btnContactManagement.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    //btnCancel.Enabled = true;
                    btnFind.Enabled = true;
                    btnSave.Enabled = true;
                    //btnReturn.Enabled = true;
                    btnClientActivity.Enabled = true;
                    btnInboundOrders.Enabled = true;
                    btnContactProjects.Enabled = true;
                    btnInventory.Enabled = true;
                    btnReports.Enabled = true;
                    btnMaintenance.Enabled = true;

                    btnFirst.Enabled = true;
                    btnFirst.CssClass = "movementbutton";
                    btnNext.Enabled = true;
                    btnNext.CssClass = "movementbutton";
                    btnPrev.Enabled = true;
                    btnPrev.CssClass = "movementbutton";
                    btnLast.Enabled = true;
                    btnLast.CssClass = "movementbutton";
                }
            }
            else
            {
                btnReturn.Visible = false;
                btnNew.Visible = true;

                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                //btnCancel.Enabled = true;
                btnFind.Enabled = true;
                btnSave.Enabled = true;
                //btnReturn.Enabled = true;
                btnClientActivity.Enabled = true;
                btnInboundOrders.Enabled = true;
                btnContactProjects.Enabled = true;

                btnFirst.Enabled = true;
                btnFirst.CssClass = "movementbutton";
                btnNext.Enabled = true;
                btnNext.CssClass = "movementbutton";
                btnPrev.Enabled = true;
                btnPrev.CssClass = "movementbutton";
                btnLast.Enabled = true;
                btnLast.CssClass = "movementbutton";

            }

            /*
            string if_onmouseup = "if(this.value = '') this.value = '" + GetTxtInqNumber(pageLang) + "';";
            txtInqNumber.Attributes.Add("onmouseup", if_onmouseup);
            */
            //string user_access = Session["UserAccess"].ToString();
            if (!IsPostBack)
            {
                int w = 0;
                client_account = HttpContext.Current.Request["account"];

                if (!Int32.TryParse(client_account, out w) || w == 0)
                {
                    client_account = clientDB.GetClientsMinID(); 
                }
                txtInqNumber.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + GetTxtInqNumber(pageLang) + "';}");
                txtInqNumber.Attributes.Add("onfocus", "if (this.value == '" + GetTxtInqNumber(pageLang) + "') {this.value = '';}");
                txtInqNumber.Text = GetTxtInqNumber(pageLang);
                /*
                 *          378                     onblur="if (this.value == '') {this.value = 'YYYY-MM-DD'; this.style.color='#C0C0C0'}"
                379                     onfocus="if (this.value == 'YYYY-MM-DD') {this.value = ''; this.style.color='#000000' }"     
                 */

            }

            int x = 0;

            // Button clickedButton = sender as Button;

            Control c = GetPostBackControl(this.Page);

            if (c != null)
            {
                if ((c.ID != "btnEdit") && (c.ID != "btnNew"))
                {
                    txtAddress1.Enabled = false;
                    txtAddress2.Enabled = false;
                    txtCity.Enabled = false;
                    txtCompany.Enabled = false;
                    txtCountry.Enabled = false;
                    txtEmail.Enabled = false;
                    txtFax.Enabled = false;
                    txtFirstName.Enabled = false;
                    txtJobTilte.Enabled = false;
                    txtLastName.Enabled = false;
                    txtPhone.Enabled = false;
                    txtPostalCode.Enabled = false;
                    txtProvState.Enabled = false;
                    txtSalutation.Enabled = false;
                    txtWebURL.Enabled = false;

                    ddlDeliveryMode.Enabled = false;
                    ddlLanguage.Enabled = false;
                    ddlPick.Enabled = false;
                    ddlProvCode.Enabled = false;
                    ddlStatus.Enabled = false;
                    ddlType.Enabled = false;
                }

                if (c.ID != "btnNew")
                {
                    txtAccount.Visible = true;
                    lblCustomerAccount.Visible = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }

                if (c.ID == "btnFind")
                {
                    client_account = txtInqNumber.Text;
                    txtAccount.Text = "";
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtCompany.Text = "";
                    txtAddress1.Text = "";
                    txtJobTilte.Text = "";
                    txtAddress2.Text = "";
                    txtSalutation.Text = "";
                    txtPhone.Text = "";
                    txtCity.Text = "";
                    txtFax.Text = "";
                    txtEmail.Text = "";
                    txtProvState.Text = "";
                    txtWebURL.Text = "";
                    txtPostalCode.Text = "";
                    txtCountry.Text = "";

                    txtDateUsed.Text = "";
                    txtDateInput.Text = "";
                    txtInputBy.Text = "";
                    txtEditBy.Text = "";
                    txtAmended.Text = "";
                    txtGroup.Text = "";

                    var firstitem = ddlDeliveryMode.Items[0];
                    ddlDeliveryMode.Items.Clear();
                    ddlDeliveryMode.Items.Add(firstitem);
                    ddlDeliveryMode.ClearSelection();
                    firstitem = ddlLanguage.Items[0];
                    ddlLanguage.Items.Clear();
                    ddlLanguage.Items.Add(firstitem);
                    ddlLanguage.ClearSelection();
                    firstitem = ddlPick.Items[0];
                    ddlPick.Items.Clear();
                    ddlPick.Items.Add(firstitem);
                    ddlPick.ClearSelection();
                    firstitem = ddlProvCode.Items[0];
                    ddlProvCode.Items.Clear();
                    ddlProvCode.Items.Add(firstitem);
                    ddlProvCode.ClearSelection();
                    firstitem = ddlStatus.Items[0];
                    ddlStatus.Items.Clear();
                    ddlStatus.Items.Add(firstitem);
                    ddlStatus.ClearSelection();
                    firstitem = ddlType.Items[0];
                    ddlType.Items.Clear();
                    ddlType.Items.Add(firstitem);
                    ddlType.ClearSelection();
                }                    
            }

            if (Int32.TryParse(client_account, out x))
            {

                DataSet ds = clientDB.GetClientDetails(client_account);

                // List<Client> clients = new List<Client>();

                Client the_client = new Client();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection clientRows = ds.Tables[0].Rows;
                    the_client.c_rec_no = Int32.Parse(clientRows[0]["C_REC_NO"].ToString());
                    the_client.c_firstname_intl = clientRows[0]["C_FIRSTNAME_INTL"].ToString();
                    the_client.c_surname = clientRows[0]["C_SURNAME"].ToString();

                    the_client.c_salutation = clientRows[0]["C_SALUTATION"].ToString();
                    the_client.c_organization = clientRows[0]["C_ORGANIZATION"].ToString();
                    the_client.c_street = clientRows[0]["C_STREET"].ToString();
                    the_client.c_address_line_2 = clientRows[0]["C_ADDRESS_LINE_2"].ToString();
                    the_client.c_job_title = clientRows[0]["C_JOB_TITLE"].ToString();
                    the_client.c_telephone = clientRows[0]["C_TELEPHONE"].ToString();
                    the_client.c_city = clientRows[0]["C_CITY"].ToString();
                    the_client.c_fax_no = clientRows[0]["C_FAX_NO"].ToString();
                    the_client.c_prov_code = clientRows[0]["C_PROV_CODE"].ToString();
                    the_client.c_email = clientRows[0]["C_EMAIL"].ToString();
                    the_client.c_province_name = clientRows[0]["C_PROVINCE_NAME"].ToString();
                    the_client.c_www = clientRows[0]["C_WWW"].ToString();
                    the_client.c_postal_code = clientRows[0]["C_POSTAL_CODE"].ToString();
                    the_client.c_language = clientRows[0]["C_LANGUAGE"].ToString();
                    the_client.c_country = clientRows[0]["C_COUNTRY"].ToString();
                    the_client.c_customer_type = clientRows[0]["C_CUSTOMER_TYPE"].ToString();
                    if (Int32.TryParse(clientRows[0]["C_STATUS"].ToString(), out c_status))
                        the_client.c_status = c_status;
                    else the_client.c_status = 0;
                    the_client.c_delivery_mode = clientRows[0]["C_DEL_MODE"].ToString();
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

                    salutation = the_client.c_salutation;
                    prov_code = the_client.c_prov_code;
                    c_language = the_client.c_language;
                    c_status = the_client.c_status;
                    c_customer_type = the_client.c_customer_type;
                    c_delivery_mode = the_client.c_delivery_mode;
                    if (!IsPostBack || (c != null && c.ID == "btnFind"))
                    {
                        /*
                        if (c != null && c.ID == "btnFind")
                        {
                        }
                        */
                        txtAccount.Text = the_client.c_rec_no.ToString();
                        txtFirstName.Text = the_client.c_firstname_intl;
                        txtLastName.Text = the_client.c_surname;
                        txtCompany.Text = the_client.c_organization;
                        txtAddress1.Text = the_client.c_street;
                        txtJobTilte.Text = the_client.c_job_title;
                        txtAddress2.Text = the_client.c_address_line_2;
                        txtSalutation.Text = the_client.c_salutation;
                        txtPhone.Text = the_client.c_telephone;
                        txtCity.Text = the_client.c_city;
                        txtFax.Text = the_client.c_fax_no;
                        txtEmail.Text = the_client.c_email;
                        txtProvState.Text = GetProvinceName(prov_code, pageLang);
                        txtWebURL.Text = the_client.c_www;
                        txtPostalCode.Text = the_client.c_postal_code;
                        txtCountry.Text = the_client.c_country;

                        txtDateUsed.Text = the_client.c_date_used;
                        txtDateInput.Text = the_client.c_date_input;
                        txtInputBy.Text = the_client.c_operator;
                        txtEditBy.Text = the_client.c_operator;
                        txtAmended.Text = the_client.c_date_amended;
                        txtGroup.Text = the_client.c_user_grp;

                        loadDDLPick(ddlPick, salutation, pageLang);
                        loadDDLProvinceCode(ddlProvCode, prov_code);
                        loadDDLType(ddlType, c_customer_type, pageLang);
                        loadDDLStatus(ddlStatus, c_status, pageLang);
                        loadDDLDeliveryMode(ddlDeliveryMode, c_delivery_mode, userAccess, pageLang);
                        loadDDLLanguage(ddlLanguage, c_language, pageLang);

                        if (Request.UrlReferrer != null)
                        {
                            string previousPageUrl = Request.UrlReferrer.AbsoluteUri;
                            string previousPageName = System.IO.Path.GetFileName(Request.UrlReferrer.AbsolutePath);
                            if (previousPageName != "inbound.aspx")
                            {
                                NextFirstDisableEnable(txtAccount.Text);
                            }
                        }
                    }
                }
            }
            setLabel();
        }

        #region old_code
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //disableFields("cancel");
            //enableFields("cancel");

            if (Session["Action"] != null)
            {
                if (Session["Action"].ToString().Equals("New"))
                {
                    setConstants();
                    InboundDB db = new InboundDB();
                    //ddlLine.Items.Clear();
                    DataSet incomDS = db.GetIncomingLinesByGroup(pageLang, 0, userAccess); //0 for the user level so that you see all lines
                    //loadIncLines(incomDS);

                }// if the action is new

                Session["Action"] = null;
                Session["TempProducts"] = null;
                Session["TempEProducts"] = null;
                Session["TempKB"] = null;
                Session["TempIssues"] = null;
                Session["TempAnswer"] = null;
            }//if there was an action
            Response.Redirect(Request.RawUrl);

        }//btnCancel_Click

        protected void btnFind_Click(object sender, EventArgs e)
        {
            //client_account = txtInqNumber.Text;
            //txtAccount.Text = client_account;
            //NextFirstDisableEnable(txtAccount.Text);
        }

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            //client_account = txtInqNumber.Text;
            txtAccount.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtCompany.Text = "";
            txtAddress1.Text = "";
            txtJobTilte.Text = "";
            txtAddress2.Text = "";
            txtSalutation.Text = "";
            txtPhone.Text = "";
            txtCity.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            txtProvState.Text = "";
            txtWebURL.Text = "";
            txtPostalCode.Text = "";
            txtCountry.Text = "";

            txtDateUsed.Text = "";
            txtDateInput.Text = "";
            txtInputBy.Text = "";
            txtEditBy.Text = "";
            txtAmended.Text = "";
            txtGroup.Text = "";

            var firstitem = ddlDeliveryMode.Items[0];
            ddlDeliveryMode.Items.Clear();
            ddlDeliveryMode.Items.Add(firstitem);
            ddlDeliveryMode.ClearSelection();
            firstitem = ddlLanguage.Items[0];
            ddlLanguage.Items.Clear();
            ddlLanguage.Items.Add(firstitem);
            ddlLanguage.ClearSelection();
            firstitem = ddlPick.Items[0];
            ddlPick.Items.Clear();
            ddlPick.Items.Add(firstitem);
            ddlPick.ClearSelection();
            firstitem = ddlProvCode.Items[0];
            ddlProvCode.Items.Clear();
            ddlProvCode.Items.Add(firstitem);
            ddlProvCode.ClearSelection();
            firstitem = ddlStatus.Items[0];
            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(firstitem);
            ddlStatus.ClearSelection();
            firstitem = ddlType.Items[0];
            ddlType.Items.Clear();
            ddlType.Items.Add(firstitem);
            ddlType.ClearSelection();
            int x = 0;

            Client the_client = new Client();

            DataSet ds = clientDB.GetFirstClientDetails();

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRowCollection clientRows = ds.Tables[0].Rows;
                the_client.c_rec_no = Int32.Parse(clientRows[0]["C_REC_NO"].ToString());
                the_client.c_firstname_intl = clientRows[0]["C_FIRSTNAME_INTL"].ToString();
                the_client.c_surname = clientRows[0]["C_SURNAME"].ToString();

                the_client.c_salutation = clientRows[0]["C_SALUTATION"].ToString();
                the_client.c_organization = clientRows[0]["C_ORGANIZATION"].ToString();
                the_client.c_street = clientRows[0]["C_STREET"].ToString();
                the_client.c_address_line_2 = clientRows[0]["C_ADDRESS_LINE_2"].ToString();
                the_client.c_job_title = clientRows[0]["C_JOB_TITLE"].ToString();
                the_client.c_telephone = clientRows[0]["C_TELEPHONE"].ToString();
                the_client.c_city = clientRows[0]["C_CITY"].ToString();
                the_client.c_fax_no = clientRows[0]["C_FAX_NO"].ToString();
                the_client.c_prov_code = clientRows[0]["C_PROV_CODE"].ToString();
                the_client.c_email = clientRows[0]["C_EMAIL"].ToString();
                the_client.c_province_name = clientRows[0]["C_PROVINCE_NAME"].ToString();
                the_client.c_www = clientRows[0]["C_WWW"].ToString();
                the_client.c_postal_code = clientRows[0]["C_POSTAL_CODE"].ToString();
                the_client.c_language = clientRows[0]["C_LANGUAGE"].ToString();
                the_client.c_country = clientRows[0]["C_COUNTRY"].ToString();
                the_client.c_customer_type = clientRows[0]["C_CUSTOMER_TYPE"].ToString();
                if (Int32.TryParse(clientRows[0]["C_STATUS"].ToString(), out c_status))
                    the_client.c_status = c_status;
                else the_client.c_status = 0;
                the_client.c_delivery_mode = clientRows[0]["C_DEL_MODE"].ToString();

                the_client.c_date_input = clientRows[0]["C_DATE_INPUT"].ToString();
                the_client.c_date_amended = clientRows[0]["C_DATE_AMENDED"].ToString();
                the_client.c_operator = clientRows[0]["C_OPERATOR"].ToString();
                the_client.c_owner = clientRows[0]["C_OWNER"].ToString();
                the_client.c_user_grp = clientRows[0]["C_USER_GRP"].ToString();
                the_client.c_date_used = clientRows[0]["C_DATE_USED"].ToString();

                salutation = the_client.c_salutation;
                prov_code = the_client.c_prov_code;
                c_language = the_client.c_language;
                c_status = the_client.c_status;
                c_customer_type = the_client.c_customer_type;
                c_delivery_mode = the_client.c_delivery_mode;

                txtAccount.Text = the_client.c_rec_no.ToString();
                txtFirstName.Text = the_client.c_firstname_intl;
                txtLastName.Text = the_client.c_surname;
                txtCompany.Text = the_client.c_organization;
                txtAddress1.Text = the_client.c_street;
                txtJobTilte.Text = the_client.c_job_title;
                txtAddress2.Text = the_client.c_address_line_2;
                txtSalutation.Text = the_client.c_salutation;
                txtPhone.Text = the_client.c_telephone;
                txtCity.Text = the_client.c_city;
                txtFax.Text = the_client.c_fax_no;
                txtEmail.Text = the_client.c_email;
                txtProvState.Text = GetProvinceName(prov_code, pageLang);
                txtWebURL.Text = the_client.c_www;
                txtPostalCode.Text = the_client.c_postal_code;
                txtCountry.Text = the_client.c_country;

                txtDateUsed.Text = the_client.c_date_used;
                txtDateInput.Text = the_client.c_date_input;
                txtInputBy.Text = the_client.c_operator;
                txtEditBy.Text = the_client.c_operator;
                txtAmended.Text = the_client.c_date_amended;
                txtGroup.Text = the_client.c_user_grp;

                loadDDLPick(ddlPick, salutation, pageLang);
                loadDDLProvinceCode(ddlProvCode, prov_code);
                loadDDLType(ddlType, c_customer_type, pageLang);
                loadDDLStatus(ddlStatus, c_status, pageLang);
                loadDDLDeliveryMode(ddlDeliveryMode, c_delivery_mode, userAccess, pageLang);
                loadDDLLanguage(ddlLanguage, c_language, pageLang);

                NextFirstDisableEnable(txtAccount.Text);
            }            
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            client_account = txtAccount.Text;
            txtAccount.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtCompany.Text = "";
            txtAddress1.Text = "";
            txtJobTilte.Text = "";
            txtAddress2.Text = "";
            txtSalutation.Text = "";
            txtPhone.Text = "";
            txtCity.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            txtProvState.Text = "";
            txtWebURL.Text = "";
            txtPostalCode.Text = "";
            txtCountry.Text = "";

            txtDateUsed.Text = "";
            txtDateInput.Text = "";
            txtInputBy.Text = "";
            txtEditBy.Text = "";
            txtAmended.Text = "";
            txtGroup.Text = "";

            var firstitem = ddlDeliveryMode.Items[0];
            ddlDeliveryMode.Items.Clear();
            ddlDeliveryMode.Items.Add(firstitem);
            ddlDeliveryMode.ClearSelection();
            firstitem = ddlLanguage.Items[0];
            ddlLanguage.Items.Clear();
            ddlLanguage.Items.Add(firstitem);
            ddlLanguage.ClearSelection();
            firstitem = ddlPick.Items[0];
            ddlPick.Items.Clear();
            ddlPick.Items.Add(firstitem);
            ddlPick.ClearSelection();
            firstitem = ddlProvCode.Items[0];
            ddlProvCode.Items.Clear();
            ddlProvCode.Items.Add(firstitem);
            ddlProvCode.ClearSelection();
            firstitem = ddlStatus.Items[0];
            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(firstitem);
            ddlStatus.ClearSelection();
            firstitem = ddlType.Items[0];
            ddlType.Items.Clear();
            ddlType.Items.Add(firstitem);
            ddlType.ClearSelection();
            int x = 0;

            Client the_client = new Client();

            if (Int32.TryParse(client_account, out x))
            {
                DataSet ds = clientDB.GetPreviousClientDetails(client_account);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection clientRows = ds.Tables[0].Rows;
                    the_client.c_rec_no = Int32.Parse(clientRows[0]["C_REC_NO"].ToString());
                    the_client.c_firstname_intl = clientRows[0]["C_FIRSTNAME_INTL"].ToString();
                    the_client.c_surname = clientRows[0]["C_SURNAME"].ToString();

                    the_client.c_salutation = clientRows[0]["C_SALUTATION"].ToString();
                    the_client.c_organization = clientRows[0]["C_ORGANIZATION"].ToString();
                    the_client.c_street = clientRows[0]["C_STREET"].ToString();
                    the_client.c_address_line_2 = clientRows[0]["C_ADDRESS_LINE_2"].ToString();
                    the_client.c_job_title = clientRows[0]["C_JOB_TITLE"].ToString();
                    the_client.c_telephone = clientRows[0]["C_TELEPHONE"].ToString();
                    the_client.c_city = clientRows[0]["C_CITY"].ToString();
                    the_client.c_fax_no = clientRows[0]["C_FAX_NO"].ToString();
                    the_client.c_prov_code = clientRows[0]["C_PROV_CODE"].ToString();
                    the_client.c_email = clientRows[0]["C_EMAIL"].ToString();
                    the_client.c_province_name = clientRows[0]["C_PROVINCE_NAME"].ToString();
                    the_client.c_www = clientRows[0]["C_WWW"].ToString();
                    the_client.c_postal_code = clientRows[0]["C_POSTAL_CODE"].ToString();
                    the_client.c_language = clientRows[0]["C_LANGUAGE"].ToString();
                    the_client.c_country = clientRows[0]["C_COUNTRY"].ToString();
                    the_client.c_customer_type = clientRows[0]["C_CUSTOMER_TYPE"].ToString();
                    if (Int32.TryParse(clientRows[0]["C_STATUS"].ToString(), out c_status))
                        the_client.c_status = c_status;
                    else the_client.c_status = 0;
                    the_client.c_delivery_mode = clientRows[0]["C_DEL_MODE"].ToString();

                    the_client.c_date_input = clientRows[0]["C_DATE_INPUT"].ToString();
                    the_client.c_date_amended = clientRows[0]["C_DATE_AMENDED"].ToString();
                    the_client.c_operator = clientRows[0]["C_OPERATOR"].ToString();
                    the_client.c_owner = clientRows[0]["C_OWNER"].ToString();
                    the_client.c_user_grp = clientRows[0]["C_USER_GRP"].ToString();
                    the_client.c_date_used = clientRows[0]["C_DATE_USED"].ToString();

                    salutation = the_client.c_salutation;
                    prov_code = the_client.c_prov_code;
                    c_language = the_client.c_language;
                    c_status = the_client.c_status;
                    c_customer_type = the_client.c_customer_type;
                    c_delivery_mode = the_client.c_delivery_mode;

                    txtAccount.Text = the_client.c_rec_no.ToString();
                    txtFirstName.Text = the_client.c_firstname_intl;
                    txtLastName.Text = the_client.c_surname;
                    txtCompany.Text = the_client.c_organization;
                    txtAddress1.Text = the_client.c_street;
                    txtJobTilte.Text = the_client.c_job_title;
                    txtAddress2.Text = the_client.c_address_line_2;
                    txtSalutation.Text = the_client.c_salutation;
                    txtPhone.Text = the_client.c_telephone;
                    txtCity.Text = the_client.c_city;
                    txtFax.Text = the_client.c_fax_no;
                    txtEmail.Text = the_client.c_email;
                    txtProvState.Text = GetProvinceName(prov_code, pageLang);
                    txtWebURL.Text = the_client.c_www;
                    txtPostalCode.Text = the_client.c_postal_code;
                    txtCountry.Text = the_client.c_country;

                    txtDateUsed.Text = the_client.c_date_used;
                    txtDateInput.Text = the_client.c_date_input;
                    txtInputBy.Text = the_client.c_operator;
                    txtEditBy.Text = the_client.c_operator;
                    txtAmended.Text = the_client.c_date_amended;
                    txtGroup.Text = the_client.c_user_grp;

                    loadDDLPick(ddlPick, salutation, pageLang);
                    loadDDLProvinceCode(ddlProvCode, prov_code);
                    loadDDLType(ddlType, c_customer_type, pageLang);
                    loadDDLStatus(ddlStatus, c_status, pageLang);
                    loadDDLDeliveryMode(ddlDeliveryMode, c_delivery_mode, userAccess, pageLang);
                    loadDDLLanguage(ddlLanguage, c_language, pageLang);

                    NextFirstDisableEnable(txtAccount.Text);

                }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            client_account = txtAccount.Text;
            txtAccount.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtCompany.Text = "";
            txtAddress1.Text = "";
            txtJobTilte.Text = "";
            txtAddress2.Text = "";
            txtSalutation.Text = "";
            txtPhone.Text = "";
            txtCity.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            txtProvState.Text = "";
            txtWebURL.Text = "";
            txtPostalCode.Text = "";
            txtCountry.Text = "";

            txtDateUsed.Text = "";
            txtDateInput.Text = "";
            txtInputBy.Text = "";
            txtEditBy.Text = "";
            txtAmended.Text = "";
            txtGroup.Text = "";

            var firstitem = ddlDeliveryMode.Items[0];
            ddlDeliveryMode.Items.Clear();
            ddlDeliveryMode.Items.Add(firstitem);
            ddlDeliveryMode.ClearSelection();
            firstitem = ddlLanguage.Items[0];
            ddlLanguage.Items.Clear();
            ddlLanguage.Items.Add(firstitem);
            ddlLanguage.ClearSelection();
            firstitem = ddlPick.Items[0];
            ddlPick.Items.Clear();
            ddlPick.Items.Add(firstitem);
            ddlPick.ClearSelection();
            firstitem = ddlProvCode.Items[0];
            ddlProvCode.Items.Clear();
            ddlProvCode.Items.Add(firstitem);
            ddlProvCode.ClearSelection();
            firstitem = ddlStatus.Items[0];
            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(firstitem);
            ddlStatus.ClearSelection();
            firstitem = ddlType.Items[0];
            ddlType.Items.Clear();
            ddlType.Items.Add(firstitem);
            ddlType.ClearSelection();
            int x = 0;

            // List<Client> clients = new List<Client>();

            Client the_client = new Client();

            if (Int32.TryParse(client_account, out x))
            {
                DataSet ds = clientDB.GetNextClientDetails(client_account);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection clientRows = ds.Tables[0].Rows;
                    the_client.c_rec_no = Int32.Parse(clientRows[0]["C_REC_NO"].ToString());
                    the_client.c_firstname_intl = clientRows[0]["C_FIRSTNAME_INTL"].ToString();
                    the_client.c_surname = clientRows[0]["C_SURNAME"].ToString();

                    the_client.c_salutation = clientRows[0]["C_SALUTATION"].ToString();
                    the_client.c_organization = clientRows[0]["C_ORGANIZATION"].ToString();
                    the_client.c_street = clientRows[0]["C_STREET"].ToString();
                    the_client.c_address_line_2 = clientRows[0]["C_ADDRESS_LINE_2"].ToString();
                    the_client.c_job_title = clientRows[0]["C_JOB_TITLE"].ToString();
                    the_client.c_telephone = clientRows[0]["C_TELEPHONE"].ToString();
                    the_client.c_city = clientRows[0]["C_CITY"].ToString();
                    the_client.c_fax_no = clientRows[0]["C_FAX_NO"].ToString();
                    the_client.c_prov_code = clientRows[0]["C_PROV_CODE"].ToString();
                    the_client.c_email = clientRows[0]["C_EMAIL"].ToString();
                    the_client.c_province_name = clientRows[0]["C_PROVINCE_NAME"].ToString();
                    the_client.c_www = clientRows[0]["C_WWW"].ToString();
                    the_client.c_postal_code = clientRows[0]["C_POSTAL_CODE"].ToString();
                    the_client.c_language = clientRows[0]["C_LANGUAGE"].ToString();
                    the_client.c_country = clientRows[0]["C_COUNTRY"].ToString();
                    the_client.c_customer_type = clientRows[0]["C_CUSTOMER_TYPE"].ToString();
                    if (Int32.TryParse(clientRows[0]["C_STATUS"].ToString(), out c_status))
                        the_client.c_status = c_status;
                    else the_client.c_status = 0;
                    the_client.c_delivery_mode = clientRows[0]["C_DEL_MODE"].ToString();

                    the_client.c_date_input = clientRows[0]["C_DATE_INPUT"].ToString();
                    the_client.c_date_amended = clientRows[0]["C_DATE_AMENDED"].ToString();
                    the_client.c_operator = clientRows[0]["C_OPERATOR"].ToString();
                    the_client.c_owner = clientRows[0]["C_OWNER"].ToString();
                    the_client.c_user_grp = clientRows[0]["C_USER_GRP"].ToString();
                    the_client.c_date_used = clientRows[0]["C_DATE_USED"].ToString();

                    salutation = the_client.c_salutation;
                    prov_code = the_client.c_prov_code;
                    c_language = the_client.c_language;
                    c_status = the_client.c_status;
                    c_customer_type = the_client.c_customer_type;
                    c_delivery_mode = the_client.c_delivery_mode;


                    txtAccount.Text = the_client.c_rec_no.ToString();
                    txtFirstName.Text = the_client.c_firstname_intl;
                    txtLastName.Text = the_client.c_surname;
                    txtCompany.Text = the_client.c_organization;
                    txtAddress1.Text = the_client.c_street;
                    txtJobTilte.Text = the_client.c_job_title;
                    txtAddress2.Text = the_client.c_address_line_2;
                    txtSalutation.Text = the_client.c_salutation;
                    txtPhone.Text = the_client.c_telephone;
                    txtCity.Text = the_client.c_city;
                    txtFax.Text = the_client.c_fax_no;
                    txtEmail.Text = the_client.c_email;
                    txtProvState.Text = GetProvinceName(prov_code, pageLang);
                    txtWebURL.Text = the_client.c_www;
                    txtPostalCode.Text = the_client.c_postal_code;
                    txtCountry.Text = the_client.c_country;

                    txtDateUsed.Text = the_client.c_date_used;
                    txtDateInput.Text = the_client.c_date_input;
                    txtInputBy.Text = the_client.c_operator;
                    txtEditBy.Text = the_client.c_operator;
                    txtAmended.Text = the_client.c_date_amended;
                    txtGroup.Text = the_client.c_user_grp;

                    loadDDLPick(ddlPick, salutation, pageLang);
                    loadDDLProvinceCode(ddlProvCode, prov_code);
                    loadDDLType(ddlType, c_customer_type, pageLang);
                    loadDDLStatus(ddlStatus, c_status, pageLang);
                    loadDDLDeliveryMode(ddlDeliveryMode, c_delivery_mode, userAccess, pageLang);
                    loadDDLLanguage(ddlLanguage, c_language, pageLang);

                    NextFirstDisableEnable(txtAccount.Text);

                }
            }
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            //client_account = txtInqNumber.Text;
            txtAccount.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtCompany.Text = "";
            txtAddress1.Text = "";
            txtJobTilte.Text = "";
            txtAddress2.Text = "";
            txtSalutation.Text = "";
            txtPhone.Text = "";
            txtCity.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            txtProvState.Text = "";
            txtWebURL.Text = "";
            txtPostalCode.Text = "";
            txtCountry.Text = "";

            txtDateUsed.Text = "";
            txtDateInput.Text = "";
            txtInputBy.Text = "";
            txtEditBy.Text = "";
            txtAmended.Text = "";
            txtGroup.Text = "";

            var firstitem = ddlDeliveryMode.Items[0];
            ddlDeliveryMode.Items.Clear();
            ddlDeliveryMode.Items.Add(firstitem);
            ddlDeliveryMode.ClearSelection();
            firstitem = ddlLanguage.Items[0];
            ddlLanguage.Items.Clear();
            ddlLanguage.Items.Add(firstitem);
            ddlLanguage.ClearSelection();
            firstitem = ddlPick.Items[0];
            ddlPick.Items.Clear();
            ddlPick.Items.Add(firstitem);
            ddlPick.ClearSelection();
            firstitem = ddlProvCode.Items[0];
            ddlProvCode.Items.Clear();
            ddlProvCode.Items.Add(firstitem);
            ddlProvCode.ClearSelection();
            firstitem = ddlStatus.Items[0];
            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(firstitem);
            ddlStatus.ClearSelection();
            firstitem = ddlType.Items[0];
            ddlType.Items.Clear();
            ddlType.Items.Add(firstitem);
            ddlType.ClearSelection();
            int x = 0;

            DataSet ds = clientDB.GetLastClientDetails();

            // List<Client> clients = new List<Client>();

            Client the_client = new Client();

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRowCollection clientRows = ds.Tables[0].Rows;
                the_client.c_rec_no = Int32.Parse(clientRows[0]["C_REC_NO"].ToString());
                the_client.c_firstname_intl = clientRows[0]["C_FIRSTNAME_INTL"].ToString();
                the_client.c_surname = clientRows[0]["C_SURNAME"].ToString();

                the_client.c_salutation = clientRows[0]["C_SALUTATION"].ToString();
                the_client.c_organization = clientRows[0]["C_ORGANIZATION"].ToString();
                the_client.c_street = clientRows[0]["C_STREET"].ToString();
                the_client.c_address_line_2 = clientRows[0]["C_ADDRESS_LINE_2"].ToString();
                the_client.c_job_title = clientRows[0]["C_JOB_TITLE"].ToString();
                the_client.c_telephone = clientRows[0]["C_TELEPHONE"].ToString();
                the_client.c_city = clientRows[0]["C_CITY"].ToString();
                the_client.c_fax_no = clientRows[0]["C_FAX_NO"].ToString();
                the_client.c_prov_code = clientRows[0]["C_PROV_CODE"].ToString();
                the_client.c_email = clientRows[0]["C_EMAIL"].ToString();
                the_client.c_province_name = clientRows[0]["C_PROVINCE_NAME"].ToString();
                the_client.c_www = clientRows[0]["C_WWW"].ToString();
                the_client.c_postal_code = clientRows[0]["C_POSTAL_CODE"].ToString();
                the_client.c_language = clientRows[0]["C_LANGUAGE"].ToString();
                the_client.c_country = clientRows[0]["C_COUNTRY"].ToString();
                the_client.c_customer_type = clientRows[0]["C_CUSTOMER_TYPE"].ToString();
                if (Int32.TryParse(clientRows[0]["C_STATUS"].ToString(), out c_status))
                    the_client.c_status = c_status;
                else the_client.c_status = 0;
                the_client.c_delivery_mode = clientRows[0]["C_DEL_MODE"].ToString();

                the_client.c_date_input = clientRows[0]["C_DATE_INPUT"].ToString();
                the_client.c_date_amended = clientRows[0]["C_DATE_AMENDED"].ToString();
                the_client.c_operator = clientRows[0]["C_OPERATOR"].ToString();
                the_client.c_owner = clientRows[0]["C_OWNER"].ToString();
                the_client.c_user_grp = clientRows[0]["C_USER_GRP"].ToString();
                the_client.c_date_used = clientRows[0]["C_DATE_USED"].ToString();

                salutation = the_client.c_salutation;
                prov_code = the_client.c_prov_code;
                c_language = the_client.c_language;
                c_status = the_client.c_status;
                c_customer_type = the_client.c_customer_type;
                c_delivery_mode = the_client.c_delivery_mode;


                txtAccount.Text = the_client.c_rec_no.ToString();
                txtFirstName.Text = the_client.c_firstname_intl;
                txtLastName.Text = the_client.c_surname;
                txtCompany.Text = the_client.c_organization;
                txtAddress1.Text = the_client.c_street;
                txtJobTilte.Text = the_client.c_job_title;
                txtAddress2.Text = the_client.c_address_line_2;
                txtSalutation.Text = the_client.c_salutation;
                txtPhone.Text = the_client.c_telephone;
                txtCity.Text = the_client.c_city;
                txtFax.Text = the_client.c_fax_no;
                txtEmail.Text = the_client.c_email;
                txtProvState.Text = GetProvinceName(prov_code, pageLang);
                txtWebURL.Text = the_client.c_www;
                txtPostalCode.Text = the_client.c_postal_code;
                txtCountry.Text = the_client.c_country;

                txtDateUsed.Text = the_client.c_date_used;
                txtDateInput.Text = the_client.c_date_input;
                txtInputBy.Text = the_client.c_operator;
                txtEditBy.Text = the_client.c_operator;
                txtAmended.Text = the_client.c_date_amended;
                txtGroup.Text = the_client.c_user_grp;

                loadDDLPick(ddlPick, salutation, pageLang);
                loadDDLProvinceCode(ddlProvCode, prov_code);
                loadDDLType(ddlType, c_customer_type, pageLang);
                loadDDLStatus(ddlStatus, c_status, pageLang);
                loadDDLDeliveryMode(ddlDeliveryMode, c_delivery_mode, userAccess, pageLang);
                loadDDLLanguage(ddlLanguage, c_language, pageLang);

                NextFirstDisableEnable(txtAccount.Text);

            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string client_to_be_deleted = txtAccount.Text;
            int x = 0;
            if (Int32.TryParse(client_to_be_deleted, out x))
            {
                bool is_last = clientDB.IsLastClient(client_to_be_deleted);
                int affected_rows = clientDB.DeleteClient(client_to_be_deleted);
                if (affected_rows > 0)
                {
                    txtAccount.Text = "";
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtCompany.Text = "";
                    txtAddress1.Text = "";
                    txtJobTilte.Text = "";
                    txtAddress2.Text = "";
                    txtSalutation.Text = "";
                    txtPhone.Text = "";
                    txtCity.Text = "";
                    txtFax.Text = "";
                    txtEmail.Text = "";
                    txtProvState.Text = "";
                    txtWebURL.Text = "";
                    txtPostalCode.Text = "";
                    txtCountry.Text = "";

                    txtDateUsed.Text = "";
                    txtDateInput.Text = "";
                    txtInputBy.Text = "";
                    txtEditBy.Text = "";
                    txtAmended.Text = "";
                    txtGroup.Text = "";

                    var firstitem = ddlDeliveryMode.Items[0];
                    ddlDeliveryMode.Items.Clear();
                    ddlDeliveryMode.Items.Add(firstitem);
                    ddlDeliveryMode.ClearSelection();
                    firstitem = ddlLanguage.Items[0];
                    ddlLanguage.Items.Clear();
                    ddlLanguage.Items.Add(firstitem);
                    ddlLanguage.ClearSelection();
                    firstitem = ddlPick.Items[0];
                    ddlPick.Items.Clear();
                    ddlPick.Items.Add(firstitem);
                    ddlPick.ClearSelection();
                    firstitem = ddlProvCode.Items[0];
                    ddlProvCode.Items.Clear();
                    ddlProvCode.Items.Add(firstitem);
                    ddlProvCode.ClearSelection();
                    firstitem = ddlStatus.Items[0];
                    ddlStatus.Items.Clear();
                    ddlStatus.Items.Add(firstitem);
                    ddlStatus.ClearSelection();
                    firstitem = ddlType.Items[0];
                    ddlType.Items.Clear();
                    ddlType.Items.Add(firstitem);
                    ddlType.ClearSelection();

                    DataSet ds;
                    Client the_client = new Client();
                    if (is_last)
                        ds = clientDB.GetLastClientDetails();
                    else
                        ds = clientDB.GetNextClientDetails(client_to_be_deleted);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRowCollection clientRows = ds.Tables[0].Rows;
                        the_client.c_rec_no = Int32.Parse(clientRows[0]["C_REC_NO"].ToString());
                        the_client.c_firstname_intl = clientRows[0]["C_FIRSTNAME_INTL"].ToString();
                        the_client.c_surname = clientRows[0]["C_SURNAME"].ToString();

                        the_client.c_salutation = clientRows[0]["C_SALUTATION"].ToString();
                        the_client.c_organization = clientRows[0]["C_ORGANIZATION"].ToString();
                        the_client.c_street = clientRows[0]["C_STREET"].ToString();
                        the_client.c_address_line_2 = clientRows[0]["C_ADDRESS_LINE_2"].ToString();
                        the_client.c_job_title = clientRows[0]["C_JOB_TITLE"].ToString();
                        the_client.c_telephone = clientRows[0]["C_TELEPHONE"].ToString();
                        the_client.c_city = clientRows[0]["C_CITY"].ToString();
                        the_client.c_fax_no = clientRows[0]["C_FAX_NO"].ToString();
                        the_client.c_prov_code = clientRows[0]["C_PROV_CODE"].ToString();
                        the_client.c_email = clientRows[0]["C_EMAIL"].ToString();
                        the_client.c_province_name = clientRows[0]["C_PROVINCE_NAME"].ToString();
                        the_client.c_www = clientRows[0]["C_WWW"].ToString();
                        the_client.c_postal_code = clientRows[0]["C_POSTAL_CODE"].ToString();
                        the_client.c_language = clientRows[0]["C_LANGUAGE"].ToString();
                        the_client.c_country = clientRows[0]["C_COUNTRY"].ToString();
                        the_client.c_customer_type = clientRows[0]["C_CUSTOMER_TYPE"].ToString();
                        if (Int32.TryParse(clientRows[0]["C_STATUS"].ToString(), out c_status))
                            the_client.c_status = c_status;
                        else the_client.c_status = 0;
                        the_client.c_delivery_mode = clientRows[0]["C_DEL_MODE"].ToString();

                        the_client.c_date_input = clientRows[0]["C_DATE_INPUT"].ToString();
                        the_client.c_date_amended = clientRows[0]["C_DATE_AMENDED"].ToString();
                        the_client.c_operator = clientRows[0]["C_OPERATOR"].ToString();
                        the_client.c_owner = clientRows[0]["C_OWNER"].ToString();
                        the_client.c_user_grp = clientRows[0]["C_USER_GRP"].ToString();
                        the_client.c_date_used = clientRows[0]["C_DATE_USED"].ToString();

                        salutation = the_client.c_salutation;
                        prov_code = the_client.c_prov_code;
                        c_language = the_client.c_language;
                        c_status = the_client.c_status;
                        c_customer_type = the_client.c_customer_type;
                        c_delivery_mode = the_client.c_delivery_mode;

                        txtAccount.Text = the_client.c_rec_no.ToString();
                        txtFirstName.Text = the_client.c_firstname_intl;
                        txtLastName.Text = the_client.c_surname;
                        txtCompany.Text = the_client.c_organization;
                        txtAddress1.Text = the_client.c_street;
                        txtJobTilte.Text = the_client.c_job_title;
                        txtAddress2.Text = the_client.c_address_line_2;
                        txtSalutation.Text = the_client.c_salutation;
                        txtPhone.Text = the_client.c_telephone;
                        txtCity.Text = the_client.c_city;
                        txtFax.Text = the_client.c_fax_no;
                        txtEmail.Text = the_client.c_email;
                        txtProvState.Text = GetProvinceName(prov_code, pageLang);
                        txtWebURL.Text = the_client.c_www;
                        txtPostalCode.Text = the_client.c_postal_code;
                        txtCountry.Text = the_client.c_country;

                        txtDateUsed.Text = the_client.c_date_used;
                        txtDateInput.Text = the_client.c_date_input;
                        txtInputBy.Text = the_client.c_operator;
                        txtEditBy.Text = the_client.c_operator;
                        txtAmended.Text = the_client.c_date_amended;
                        txtGroup.Text = the_client.c_user_grp;

                        loadDDLPick(ddlPick, salutation, pageLang);
                        loadDDLProvinceCode(ddlProvCode, prov_code);
                        loadDDLType(ddlType, c_customer_type, pageLang);
                        loadDDLStatus(ddlStatus, c_status, pageLang);
                        loadDDLDeliveryMode(ddlDeliveryMode, c_delivery_mode, userAccess, pageLang);
                        loadDDLLanguage(ddlLanguage, c_language, pageLang);

                        NextFirstDisableEnable(txtAccount.Text);
                    }
                }
            }
        }

        private void NextFirstDisableEnable(string accountNumber)
        {
            if (clientDB.IsFirstClient(accountNumber))
            {
                btnFirst.Enabled = false;
                btnFirst.CssClass = "movementbutton_disabeld";
                //btnFirst.Visible = false;
                btnPrev.Enabled = false;
                btnPrev.CssClass = "movementbutton_disabeld";
                //btnPrev.Visible = false;
            }
            else
            {
                btnFirst.Enabled = true;
                btnFirst.CssClass = "movementbutton";
                //btnFirst.Visible = true;
                btnPrev.Enabled = true;
                btnPrev.CssClass = "movementbutton";
                //btnPrev.Visible = true;
            }

            if (clientDB.IsLastClient(accountNumber))
            {
                btnLast.Enabled = false;
                btnLast.CssClass = "movementbutton_disabeld";
                //btnLast.Visible = false;
                btnNext.Enabled = false;
                btnNext.CssClass = "movementbutton_disabeld";
                //btnNext.Visible = false;
            }
            else
            {
                btnLast.Enabled = true;
                btnLast.CssClass = "movementbutton";
                //btnLast.Visible = true;
                btnNext.Enabled = true;
                btnNext.CssClass = "movementbutton";
                //btnNext.Visible = true;
            }

        }

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
        #endregion cp

        protected void btnSaveClick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            // string the_date = String.Format("{dd-MMM-yyyy HH:mm:ss}", time).ToUpper();
            string the_date = time.ToString("dd-MMM-yyyy HH:mm:ss").ToUpper();
            int y = 0;
            // here we update the clinet
            if (Int32.TryParse(txtAccount.Text, out y))
            {
                if (y > 0)
                {
                    string the_salutation = "";
                    /*
                    txtAddress1.Enabled = false;
                    txtAddress2.Enabled = false;
                    txtCity.Enabled = false;
                    txtCompany.Enabled = false;
                    txtCountry.Enabled = false;
                    txtEmail.Enabled = false;
                    txtFax.Enabled = false;
                    txtFirstName.Enabled = false;
                    txtJobTilte.Enabled = false;
                    txtLastName.Enabled = false;
                    txtPhone.Enabled = false;
                    txtPostalCode.Enabled = false;
                    txtProvState.Enabled = false;
                    txtSalutation.Enabled = false;
                    txtWebURL.Enabled = false;

                    ddlDeliveryMode.Enabled = false;
                    ddlLanguage.Enabled = false;
                    ddlPick.Enabled = false;
                    ddlProvCode.Enabled = false;
                    ddlStatus.Enabled = false;
                    ddlType.Enabled = false;
                    */
                    string ddlDM = ddlDeliveryMode.SelectedValue;
                    string ddlL = ddlLanguage.SelectedValue;
                    ListItem ddlLanguageList = ddlLanguage.Items.FindByValue(ddlLanguage.SelectedValue.ToString());
                    string ddlL1 = ddlLanguageList.Value;

                    string ddlP = ddlPick.SelectedValue;
                    ListItem ddlPList = ddlPick.Items.FindByValue(ddlPick.SelectedValue.ToString());
                    string ddlP1 = ddlPList.Text;

                    string ddlPC = ddlProvCode.SelectedValue;
                    ListItem ddlPCList = ddlProvCode.Items.FindByValue(ddlProvCode.SelectedValue.ToString());
                    string ddlPC1 = ddlPCList.Text;

                    string ddlS = ddlStatus.SelectedValue;
                    int ddlSt = 2;
                    int z = 0;
                    if (Int32.TryParse(ddlS, out z))
                        ddlSt = z;

                    string ddlT = ddlType.SelectedValue;

                    if (txtSalutation.Text.Length == 0 || String.Compare(ddlP1, txtSalutation.Text, true) == 1)
                        the_salutation = ddlP1;
                    else the_salutation = txtSalutation.Text;

                    int updated_client_id = clientDB.UpdateInsertClientClassic(y, txtFirstName.Text, txtLastName.Text, txtCompany.Text, txtAddress1.Text,
                                                    txtAddress2.Text, txtJobTilte.Text, the_salutation, txtPhone.Text, txtCity.Text, txtFax.Text,
                                                    txtEmail.Text, txtProvState.Text, txtWebURL.Text, txtPostalCode.Text, txtCountry.Text,
                                                    ddlDM, ddlL1, ddlPC1, ddlSt, ddlT, the_date, currentUser, userGroup, userGroup);

                    Response.Redirect("ContactManagement.aspx?account=" + y);
                }
                // new Client
                else
                {                    
                    string the_salutation = "";
                    string ddlDM = ddlDeliveryMode.SelectedValue;
                    string ddlL = ddlLanguage.SelectedValue;
                    ListItem ddlLanguageList = ddlLanguage.Items.FindByValue(ddlLanguage.SelectedValue.ToString());
                    string ddlL1 = ddlLanguageList.Value;

                    string ddlP = ddlPick.SelectedValue;
                    ListItem ddlPList = ddlPick.Items.FindByValue(ddlPick.SelectedValue.ToString());
                    string ddlP1 = ddlPList.Text;

                    string ddlPC = ddlProvCode.SelectedValue;
                    ListItem ddlPCList = ddlProvCode.Items.FindByValue(ddlProvCode.SelectedValue.ToString());
                    string ddlPC1 = ddlPCList.Text;

                    string ddlS = ddlStatus.SelectedValue;
                    int ddlSt = 2;
                    int z = 0;
                    if (Int32.TryParse(ddlS, out z))
                        ddlSt = z;

                    string ddlT = ddlType.SelectedValue;

                    if (txtSalutation.Text.Length == 0 || String.Compare(ddlP1, txtSalutation.Text, true) == 1)
                        the_salutation = ddlP1;
                    else the_salutation = txtSalutation.Text;
                    

                    int updated_client_id = clientDB.UpdateInsertClientClassic(0, txtFirstName.Text, txtLastName.Text, txtCompany.Text, txtAddress1.Text,
                                                    txtAddress2.Text, txtJobTilte.Text, the_salutation, txtPhone.Text, txtCity.Text, txtFax.Text,
                                                    txtEmail.Text, txtProvState.Text, txtWebURL.Text, txtPostalCode.Text, txtCountry.Text,
                                                    ddlDM, ddlL1, ddlPC1, ddlSt, ddlT, the_date, currentUser, userGroup, userGroup);

                    Response.Redirect("ContactManagement.aspx?account=" + updated_client_id);
                }
            }            

        }

        protected void btnEditClick(object sender, EventArgs e)
        {
            int y = 0;

            if (Int32.TryParse(txtAccount.Text, out y))
            {
                if (y > 0)
                {
                    txtAddress1.Enabled = true;
                    txtAddress2.Enabled = true;
                    txtCity.Enabled = true;
                    txtCompany.Enabled = true;
                    txtCountry.Enabled = true;
                    txtEmail.Enabled = true;
                    txtFax.Enabled = true;
                    txtFirstName.Enabled = true;
                    txtJobTilte.Enabled = true;
                    txtLastName.Enabled = true;
                    txtPhone.Enabled = true;
                    txtPostalCode.Enabled = true;
                    txtProvState.Enabled = true;
                    txtSalutation.Enabled = true;
                    txtWebURL.Enabled = true;

                    ddlDeliveryMode.Enabled = true;
                    ddlLanguage.Enabled = true;
                    ddlPick.Enabled = true;
                    ddlProvCode.Enabled = true;
                    ddlStatus.Enabled = true;
                    ddlType.Enabled = true;
                }

            }
        }

        protected void btnNewClick(object sender, EventArgs e)
        {
            txtAccount.Visible = false;
            lblCustomerAccount.Visible = false;

            txtAddress1.Enabled = true;
            txtAddress2.Enabled = true;
            txtCity.Enabled = true;
            txtCompany.Enabled = true;
            txtCountry.Enabled = true;
            txtEmail.Enabled = true;
            txtFax.Enabled = true;
            txtFirstName.Enabled = true;
            txtJobTilte.Enabled = true;
            txtLastName.Enabled = true;
            txtPhone.Enabled = true;
            txtPostalCode.Enabled = true;
            txtProvState.Enabled = true;
            txtSalutation.Enabled = true;
            txtWebURL.Enabled = true;

            ddlDeliveryMode.Enabled = true;
            ddlLanguage.Enabled = true;
            ddlPick.Enabled = true;
            ddlProvCode.Enabled = true;
            ddlStatus.Enabled = true;
            ddlType.Enabled = true;

            txtAccount.Text = "0";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtCompany.Text = "";
            txtAddress1.Text = "";
            txtJobTilte.Text = "";
            txtAddress2.Text = "";
            txtSalutation.Text = "";
            txtPhone.Text = "";
            txtCity.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            txtProvState.Text = "";
            txtWebURL.Text = "";
            txtPostalCode.Text = "";
            txtCountry.Text = "";

            txtDateUsed.Text = "";
            txtDateInput.Text = "";
            txtInputBy.Text = "";
            txtEditBy.Text = "";
            txtAmended.Text = "";
            txtGroup.Text = "";


            ddlDeliveryMode.Text = "";
            ddlLanguage.Text = "";
            ddlPick.Text = "";
            ddlProvCode.Text = "";
            ddlStatus.Text = "";
            ddlType.Text = "";

            btnEdit.Enabled = false;
            btnDelete.Enabled = false;

            NextFirstDisableEnable(txtAccount.Text);
        }

        protected void btnClientActivityClick(object sender, EventArgs e)
        {
            GoToClientActivity();
        }

        protected void btnInboundOrdersClick(object sender, EventArgs e)
        {
        }

        protected void btnContactProjectsClick(object sender, EventArgs e)
        {
            GoToClientProjects();
        }

        protected string DelMessage
        {
            get
            {
                setConstants();
                LanguageDB db = new LanguageDB();
                return db.GetLabel("ClientEdit", "CEDeleteClient", pageLang);
            }

        }//

        protected void GoToContactManagement(object sender, EventArgs e)
        {
            Response.Redirect("ContactManagement.aspx");
        }

        private void GoToClientProjects()
        {                         
            string the_client_id = txtAccount.Text;
            int x = 0;
            if (Int32.TryParse(the_client_id, out x) && x > 0)
            {
                Session["C_REC_NO"] = the_client_id;
                Response.Redirect("ClientProjects.aspx");
            }
        }

        private void GoToClientActivity()
        {
            string the_client_id = txtAccount.Text;
            int x = 0;
            if (Int32.TryParse(the_client_id, out x) && x > 0)
            {
                Session["C_REC_NO"] = the_client_id;
                Response.Redirect("ClientActivityForm.aspx");
            }
        }
        /*
        protected void btnCloseClick(object sender, EventArgs e)
        {
            Response.Redirect("inbound.aspx");
        }
        */
        private void loadDDLPick(DropDownList theDDLPick, string theSalutation, string page_lang)
        {
            //DataTable subjects = new DataTable();
            DataRowCollection dataRowCollection;
            DataTable sals = new DataTable();
            DataSet dataSet1 = clientDB.GetPickSalutations(page_lang);

            sals.Columns.Add("LANG_CODE", typeof(string));
            sals.Columns.Add("LANG", typeof(string));

            if (dataSet1.Tables[0].Rows.Count > 0)
            {
                dataRowCollection = dataSet1.Tables[0].Rows;

                for (int i = 0; i < dataRowCollection.Count; ++i)
                {
                    sals.Rows.Add(dataRowCollection[i]["LANG_CODE"].ToString(), dataRowCollection[i]["LANG"].ToString());
                }

                theDDLPick.DataSource = sals;
                theDDLPick.DataTextField = "LANG";
                theDDLPick.DataValueField = "LANG_CODE";
                /*
                ListItem selectedListItem = ddlPick.Items.FindByValue(salutation);

                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                };                
                */
                theDDLPick.DataBind();

                ListItem selectedListItem = theDDLPick.Items.FindByText(theSalutation);

                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }

        private void loadDDLProvinceCode(DropDownList theDDLProvCode, string theProv_Code)
        {
            DataRowCollection dataRowCollection;
            DataTable provs = new DataTable();
            DataSet dataSet1 = clientDB.GetProvinceCode();

            provs.Columns.Add("PROVSEQUENCE", typeof(string));
            provs.Columns.Add("PROV_CODE", typeof(string));

            if (dataSet1.Tables[0].Rows.Count > 0)
            {
                dataRowCollection = dataSet1.Tables[0].Rows;

                for (int i = 0; i < dataRowCollection.Count; ++i)
                {
                    provs.Rows.Add(dataRowCollection[i]["PROVSEQUENCE"].ToString(), dataRowCollection[i]["PROV_CODE"].ToString());
                }

                theDDLProvCode.DataSource = provs;
                theDDLProvCode.DataTextField = "PROV_CODE";
                theDDLProvCode.DataValueField = "PROVSEQUENCE";

                theDDLProvCode.DataBind();

                ListItem selectedListItem = theDDLProvCode.Items.FindByText(theProv_Code);

                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }

        private void loadDDLType(DropDownList theDDLType, string theCustomerType, string page_lang)
        {
            DataRowCollection dataRowCollection;
            DataTable types = new DataTable();
            DataSet dataSet1 = clientDB.GetClientType(page_lang);

            types.Columns.Add("AFF_CODE", typeof(string));
            types.Columns.Add("AFF_GEN_DEF", typeof(string));

            if (dataSet1.Tables[0].Rows.Count > 0)
            {
                dataRowCollection = dataSet1.Tables[0].Rows;

                for (int i = 0; i < dataRowCollection.Count; ++i)
                {
                    types.Rows.Add(dataRowCollection[i]["AFF_CODE"].ToString(), dataRowCollection[i]["AFF_GEN_DEF"].ToString());
                }

                theDDLType.DataSource = types;
                theDDLType.DataTextField = "AFF_GEN_DEF";
                theDDLType.DataValueField = "AFF_CODE";

                theDDLType.DataBind();

                ListItem selectedListItem = theDDLType.Items.FindByValue(theCustomerType);

                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }

        private void loadDDLStatus(DropDownList theDDLStatus, int theClientStatus, string page_lang)
        {
            DataRowCollection dataRowCollection;
            DataTable types = new DataTable();
            DataSet dataSet1 = clientDB.GetClientStatus(page_lang);

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

                ListItem selectedListItem = theDDLStatus.Items.FindByValue(theClientStatus.ToString());

                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }

        private void loadDDLDeliveryMode(DropDownList theDDLDelivery, string theDeliveryMode, string theUserAccess, string page_lang)
        {
            DataRowCollection dataRowCollection;
            DataTable types = new DataTable();
            DataSet dataSet1 = clientDB.GetClientDelMode(page_lang, theUserAccess);

            types.Columns.Add("DEL_CODE", typeof(string));
            types.Columns.Add("DEL_DEFINITION", typeof(string));

            if (dataSet1.Tables[0].Rows.Count > 0)
            {
                dataRowCollection = dataSet1.Tables[0].Rows;

                for (int i = 0; i < dataRowCollection.Count; ++i)
                {
                    types.Rows.Add(dataRowCollection[i]["DEL_CODE"].ToString(), dataRowCollection[i]["DEL_DEFINITION"].ToString());
                }

                theDDLDelivery.DataSource = types;
                theDDLDelivery.DataTextField = "DEL_DEFINITION";
                theDDLDelivery.DataValueField = "DEL_CODE";

                theDDLDelivery.DataBind();

                ListItem selectedListItem = theDDLDelivery.Items.FindByValue(theDeliveryMode);

                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
            }
        }

        private void loadDDLLanguage(DropDownList theDDLLanguage, string theLanguage, string page_lang)
        {
            DataTable languages = new DataTable();
            string local_language = "E";

            if (theLanguage == "F")
                local_language = "F";

            languages.Columns.Add("lang_code", typeof(string));
            languages.Columns.Add("language", typeof(string));

            if (page_lang == "FR")
            {
                languages.Rows.Add("E", "Anglais");
                languages.Rows.Add("F", "Français");
            }
            else
            {
                languages.Rows.Add("E", "English");
                languages.Rows.Add("F", "French");
            }

            theDDLLanguage.DataSource = languages;
            theDDLLanguage.DataTextField = "language";
            theDDLLanguage.DataValueField = "lang_code";

            theDDLLanguage.DataBind();

            ListItem selectedListItem = theDDLLanguage.Items.FindByValue(local_language);

            if (selectedListItem != null)
            {
                selectedListItem.Selected = true;
            }


        }

        private string GetProvinceName(string p_code, string page_lang)
        {
            string prov_name = "";

            DataSet ds = clientDB.GetProvinceName(p_code, page_lang);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRowCollection provRows = ds.Tables[0].Rows;
                prov_name = provRows[0]["PROV_DEFINITION"].ToString();
            }

            return prov_name;
        }

        private void setLabel()
        {
            setConstants();
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

            lblCustDetails.Text = db.GetLabel("ClientEdit", "CEContactInfo", pageLang);
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
            #endregion copied_labels

            // buttons
            btnNew.Text = db.GetLabel("ClientEdit", "btnNew", pageLang);
            btnEdit.Text = db.GetLabel("ClientEdit", "btnEdit", pageLang);
            string deleteText = db.GetLabel("ClientEdit", "btnDelete", pageLang);
            btnDelete.Text = deleteText;
            string cancelText = db.GetLabel("ClientEdit", "btnCancel", pageLang);
            btnCancel.Text = cancelText;
            btnFind.Text = db.GetLabel("ClientEdit", "btnFind", pageLang);
            btnSave.Text = db.GetLabel("ClientEdit", "btnOK", pageLang);
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

            string firstName = db.GetLabel("ClientEdit", "CEFirstName", pageLang);
            lblFirstName.Text = firstName;
            string surName = db.GetLabel("ClientEdit", "CELastName", pageLang);
            lblSurname.Text = surName;
            string pick_sal = db.GetLabel("ClientEdit", "CEPickSal", pageLang);
            lblPickSal.Text = pick_sal;
            string salutation = db.GetLabel("ClientEdit", "CESalutation", pageLang);
            lblSalutation.Text = salutation;
            string company = db.GetLabel("ClientEdit", "CECompany", pageLang);
            lblCompany.Text = company;
            string address1 = db.GetLabel("ClientEdit", "CEAddress", pageLang);
            lblAdress1.Text = address1;
            string jobTitle = db.GetLabel("ClientEdit", "CEJobTitle", pageLang);
            lblJobTilte.Text = jobTitle;
            string address2 = "";
            lblAddress2.Text = address2;
            lblPhone.Text = db.GetLabel("ClientEdit", "CEPhone", pageLang);
            lblCity.Text = db.GetLabel("ClientEdit", "CECity", pageLang);
            lblFax.Text = db.GetLabel("ClientEdit", "CEFax", pageLang);
            // lblProvCode.Text = db.GetLabel("ClientEdit", "CEProvState", pageLang);
            lblProvCode.Text = "Prov Code";
            lblProvState.Text = db.GetLabel("ClientEdit", "CEProvState", pageLang);
            lblEmail.Text = db.GetLabel("ClientEdit", "CEEmail", pageLang);
            lblWebURL.Text = db.GetLabel("ClientEdit", "CEWebURL", pageLang);
            lblPostalCode.Text = db.GetLabel("ClientEdit", "CEPostZip", pageLang);
            lblCountry.Text = db.GetLabel("ClientEdit", "CECountry", pageLang);
            lblLanguage.Text = db.GetLabel("ClientEdit", "CELanguage", pageLang);
            lblType.Text = db.GetLabel("ClientEdit", "CEType", pageLang);
            lblStatus.Text = db.GetLabel("ClientEdit", "CEStatus", pageLang);
            lblDeliveryMode.Text = db.GetLabel("ClientEdit", "CEDelivery", pageLang);

            lblDateUsed.Text = db.GetLabel("ClientEdit", "CEDateUsed", pageLang);
            lblInserted.Text = db.GetLabel("ClientEdit", "CEInserted", pageLang);
            lblAmended.Text = db.GetLabel("ClientEdit", "CEAmended", pageLang);
            lblInputBy.Text = db.GetLabel("ClientEdit", "CEInputBy", pageLang);
            lblEditBy.Text = db.GetLabel("ClientEdit", "CEEditBy", pageLang);
            lblGroup.Text = db.GetLabel("ClientEdit", "CEGroup", pageLang);
            //CEClientActivity
            lblAddInfo.Text = db.GetLabel("ClientEdit", "CEAddInfo", pageLang);
            btnClientActivity.Text = db.GetLabel("ClientEdit", "CEClientActivity", pageLang);
            btnInboundOrders.Text = db.GetLabel("ClientEdit", "CEInboundOrders", pageLang);
            btnContactProjects.Text = db.GetLabel("ClientEdit", "CEContactProjects", pageLang);
        }

        private string GetTxtInqNumber(string page_lang)
        {
            string the_text = "Account #";
            if (pageLang == "FR")
                the_text = "Compte #";

            return the_text;
        }

        private static Control GetPostBackControl(Page page)
        {
            Control control = null;
            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != String.Empty)
            {
                control = page.FindControl(ctrlname);

            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button)
                    {
                        control = c;
                        break;
                    }
                }

            }
            return control;
        } 
    }
}