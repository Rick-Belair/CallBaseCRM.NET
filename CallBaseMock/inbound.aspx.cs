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
    public partial class inbound : System.Web.UI.Page
    {
        public string pageLang = "EN";
        public string userAccess = "";
        public string userGroup = "";
        public int userLevel = 5;
        public string currentUser = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "checkIE", "CheckIEVersion();", true);
            InboundDB db = new InboundDB();
            setConstants();

            ConfigureEmailControls();

            if (!IsPostBack)
            {
                if (userLevel > 3)
                {
                    btnReports.Visible = false;
                    btnOrderStatus.Visible = false;
                }
                lblProductCount.Text = "";
                lblDisplayInfo.Text = "";
                lblKBCount.Text = "";
                lblOrderStatusRecords.Text = "";
                Session["KBKeys"] = null;
                Session["Action"] = null;
                DataSet incomDS = db.GetIncomingLinesByGroup(pageLang, 0, userAccess); //  user level 0 on load so they can see all incoming lines
                DataSet provDS = db.GetProvinces(pageLang);
                DataSet sourceDS = db.GetSources(pageLang);
                DataSet custTypeDS = db.GetCustTypes(pageLang);
                DataSet orderStatusDS = db.GetOrderStatus(pageLang);
                DataSet allOrderStatusDS = db.GetAllOrderStatuses(pageLang);
                DataSet deliveryDS = db.GetDeliveryMethods(pageLang, userLevel, userAccess);
                DataSet whDS = db.GetUserWHs(userAccess, pageLang);
                ListItem listItem;

                loadIncLines(incomDS);

                if (provDS.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection provRows = provDS.Tables[0].Rows;
                    for (int i = 0; i < provRows.Count; ++i)
                    {
                        string val = provRows[i]["PROV_CODE"].ToString();
                        string text = provRows[i]["Province"].ToString();
                        listItem = new ListItem(text, val);
                        ListItem listItem2 = new ListItem(text, val);
                        ddlProvState.Items.Add(listItem);
                        ddlProvState2.Items.Add(listItem2);

                    }//for all the returned rows

                }//if there are provs/states in the db

                if (sourceDS.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection sourceRows = sourceDS.Tables[0].Rows;
                    for (int i = 0; i < sourceRows.Count; ++i)
                    {
                        string val = sourceRows[i]["PROMOTION_CODE"].ToString();
                        string text = sourceRows[i]["PROM_DEFINITION"].ToString();
                        listItem = new ListItem(text, val);
                        ddlSource.Items.Add(listItem);

                    }//for all the returned rows

                }//if there are promotion/sources in the db

                if (custTypeDS.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection custTypeRows = custTypeDS.Tables[0].Rows;
                    for (int i = 0; i < custTypeRows.Count; ++i)
                    {
                        string val = custTypeRows[i]["AFF_CODE"].ToString().Trim();
                        string text = custTypeRows[i]["AFF_GEN_DEF"].ToString();
                        listItem = new ListItem(text, val);
                        ddlCust.Items.Add(listItem);

                    }//for all the returned rows

                }//if there are customer types in the db

                if (orderStatusDS.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection orderStatusRows = orderStatusDS.Tables[0].Rows;
                    for (int i = 0; i < orderStatusRows.Count; ++i)
                    {
                        string val = orderStatusRows[i]["ORDER_STATUS_CODE"].ToString();

                        //second element will be the description, regardless of language
                        string text = orderStatusRows[i].ItemArray.ElementAt(1).ToString();

                        listItem = new ListItem(text, val);
                        ddlOrderStatus.Items.Add(listItem);

                    }//for all the returned rows

                }//if there are order statuses in the db

                if (allOrderStatusDS.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection orderStatusRows = allOrderStatusDS.Tables[0].Rows;
                    for (int i = 0; i < orderStatusRows.Count; ++i)
                    {
                        string val = orderStatusRows[i]["ORDER_STATUS_CODE"].ToString();
                        string text = orderStatusRows[i]["ORDER_STATUS_DESC"].ToString();

                        listItem = new ListItem(text, val);
                        ddlStatus.Items.Add(listItem);

                    }//for all the returned rows

                }//if there are order statuses in the db

                if (deliveryDS.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection deliveryRows = deliveryDS.Tables[0].Rows;
                    for (int i = 0; i < deliveryRows.Count; ++i)
                    {
                        string val = deliveryRows[i]["DEL_CODE"].ToString();
                        string text = deliveryRows[i]["DEL_DEFINITION"].ToString();
                        listItem = new ListItem(text, val);
                        ddlDelivery.Items.Add(listItem);

                    }//for all the returned rows

                }//if there are order statuses in the db

                if (whDS.Tables[0].Rows.Count > 0)
                {
                    LanguageDB langDB = new LanguageDB();
                    DataRowCollection whRows = whDS.Tables[0].Rows;
                    foreach (DataRow row in whRows)
                    {
                        string whQTY = "WH" + row["WH_NUMBER"].ToString() + "Qty";
                        string whText = langDB.GetLabel("InboundTracking", whQTY, pageLang);
                        string val = "WH" + row["WH_NUMBER"].ToString();
                        string text = "[" + whText + "] - " + row["WH_DESC"];
                        listItem = new ListItem(text, val);
                        ddlWH.Items.Add(listItem);

                    }//for each row returned

                }//if there are whs in the db

                string recordNum;
                if (Session["TicketNumber"] == null)
                {
                    recordNum = db.GetMaxRecordNumber(createIncLineVar()).ToString();
                    Session["TicketNumber"] = recordNum;
                }
                else
                    recordNum = Session["TicketNumber"].ToString();
                txtInqNumber.Text = recordNum;
                DataSet EnquiryDS = db.GetInboundDetails(recordNum, createIncLineVar());
                loadEnquiry(EnquiryDS);
                DataSet CustomerDS = db.GetCustomerDetails(recordNum);
                loadCustomerDetails(CustomerDS);
                setLabels();
                enableMailButtons();


            }//if !isPostBack

            if (validateTicketNumber(txtInqNumber.Text))
            {
                DataSet ProductDS = db.GetProductDetails(txtInqNumber.Text, pageLang);
                if (Session["TempProducts"] != null)
                {
                    DataTable tempProducts = (DataTable)Session["TempProducts"];

                    foreach (DataRow row in tempProducts.Rows)
                        ProductDS.Tables[0].ImportRow(row);

                }// if TempProducts exists; in other words there are rows from inventory that haven't been saved

                if (Session["TempEProducts"] != null)
                {
                    DataTable tempEProducts = (DataTable)Session["TempEProducts"];

                    foreach (DataRow row in tempEProducts.Rows)
                        ProductDS.Tables[0].ImportRow(row);

                }// if TempProducts exists; in other words there are rows from inventory that haven't been saved

                loadProducts(ProductDS);

            }//if the ticket is valid

            if (Session["NewEnq"] != null)
                updateOrderStatus();
            else
            {
                orderDetailsDiv.Visible = true;
                shippingDiv.Visible = true;
                confirmStatusDiv.Visible = false;
            }

            string message = "";
            if (Session["AnswerToLong"] != null){
                LanguageDB langDB = new LanguageDB();
                message = langDB.GetLabel("InboundTracking", "MsgAnswerToLong", pageLang) + " ";
               // ScriptManager.RegisterStartupScript(this, GetType(), "answerToLongAlert", "alert('" + message + "');", true);
            }

            if (Session["IssuesToLong"] != null)
            {
                LanguageDB langDB = new LanguageDB();
                message = message + langDB.GetLabel("InboundTracking", "MsgIssueToLong", pageLang);
                //ScriptManager.RegisterStartupScript(this, GetType(), "issuesToLongAlert", "alert('" + message + "');", true);
            }

            if (message != ""){
                ScriptManager.RegisterStartupScript(this, GetType(), "txtToLongAlert", "alert('" + message + "');", true);
            }

            Session["AnswerToLong"] = null;
            Session["IssuesToLong"] = null;
        }//Page_Load

        private void ConfigureEmailControls()
        {
            InboundDB db = new InboundDB();
            DataSet ds = db.GetControlValue("INBND_EMAIL");

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CNTR_VALUE"].ToString().ToUpper() == "NO" || ds.Tables[0].Rows[0]["CNTR_VALUE"].ToString().ToUpper() == "FALSE")
                {
                    btnAddElectronic.Visible = false;
                    btnSendEmail.Visible = false;
                    //btnShipNotes.Style["width"] = Convert.ToString(btnShipNotes.Width.Value * 3 / 2);
  
                }
            }
        }
        protected void lnkLanguage_Click(object sender, EventArgs e)
        {
            string currLang = "";
            if (Session["PageLanguage"] != null)
                currLang = Session["PageLanguage"].ToString();
            Session["PageLanguage"] = Utility.setLanguage(currLang);
            Response.Redirect(Request.RawUrl);

        }//lnkLanguage_Click

        private void setLabels()
        {
            setConstants();
            LanguageDB db = new LanguageDB();
            lblAANDCHeader.Text = db.GetLabel("Home", "PublicEnquiriesINAC", pageLang);
            lnkLogOut.Text = db.GetLabel("Home", "Logout", pageLang);
            lnkLanguage.Text = db.GetLabel("InboundTracking", "LanguageSwitch", pageLang);
            btnInbound.Text = db.GetLabel("InboundTracking", "btnInboundStats", pageLang);
            btnKnowledge.Text = db.GetLabel("InboundTracking", "btnKnowledgeBase", pageLang);
            btnProduct.Text = db.GetLabel("InboundTracking", "btnProductListing", pageLang);
            btnOrderStatus.Text = db.GetLabel("InboundTracking", "AssignOrders", pageLang);
            lblShipNotes.Text = db.GetLabel("InboundTracking", "EnterNotes", pageLang);
            lblShipDetails.Text = db.GetLabel("InboundTracking", "EnterDetails", pageLang);
            btnReports.Text = db.GetLabel("SkanList", "Reports2", pageLang);
            btnNew.Text = db.GetLabel("InboundTracking", "NewEnquiry", pageLang);
            btnEdit.Text = db.GetLabel("InboundTracking", "Edit", pageLang);
            string deleteText = db.GetLabel("InboundTracking", "Delete2", pageLang);
            btnDelete.Text = deleteText;
            string cancelText = db.GetLabel("InboundTracking", "Cancel", pageLang);
            btnCancel.Text = cancelText;
            btnFind.Text = db.GetLabel("InboundTracking", "btnFind", pageLang);
            btnSave.Text = db.GetLabel("InboundTracking", "btnOkSave", pageLang);
            lblIncLine.Text = db.GetLabel("InboundTracking", "IncomingLine", pageLang);
            lblScript.Text = db.GetLabel("InboundTracking", "Script", pageLang);
            lblLanguage.Text = db.GetLabel("InboundTracking", "Language", pageLang);
            rblLanguage.Items[0].Text = db.GetLabel("InboundTracking", "English", pageLang);
            rblLanguage.Items[1].Text = db.GetLabel("InboundTracking", "French", pageLang);
            string otherText = db.GetLabel("InboundTracking", "Other", pageLang);
            string email = db.GetLabel("InboundTracking", "Email", pageLang);
            rblLanguage.Items[2].Text = otherText;
            lblCommunication.Text = db.GetLabel("InboundTracking", "CommunicationMethod", pageLang);
            rblCommunication.Items[0].Text = db.GetLabel("InboundTracking", "Phone", pageLang);
            rblCommunication.Items[1].Text = db.GetLabel("InboundTracking", "InPerson", pageLang);
            rblCommunication.Items[2].Text = db.GetLabel("InboundTracking", "Mail", pageLang);
            rblCommunication.Items[3].Text = db.GetLabel("InboundTracking", "Fax", pageLang);
            rblCommunication.Items[4].Text = db.GetLabel("InboundTracking", "VoiceMail", pageLang);
            rblCommunication.Items[5].Text = email;
            rblCommunication.Items[6].Text = db.GetLabel("InboundTracking", "Exhibits", pageLang);
            rblCommunication.Items[7].Text = db.GetLabel("InboundTracking", "Web", pageLang);
            lblGender.Text = db.GetLabel("InboundTracking", "Gender", pageLang);
            rblGender.Items[0].Text = db.GetLabel("InboundTracking", "Male", pageLang);
            rblGender.Items[1].Text = db.GetLabel("InboundTracking", "Unknown", pageLang);
            rblGender.Items[2].Text = db.GetLabel("InboundTracking", "Female", pageLang);
            string owner = db.GetLabel("InboundTracking", "Owner", pageLang);
            lblOwner.Text = owner;
            string inputDate = db.GetLabel("InboundTracking", "InputDate", pageLang);
            lblInputDate.Text = inputDate;
            lblInitialAgent.Text = db.GetLabel("InboundTracking", "InitialAgent", pageLang);
            lblEditAgent.Text = db.GetLabel("InboundTracking", "EditAgent", pageLang);
            lblEditDate.Text = db.GetLabel("InboundTracking", "EditDate", pageLang);
            string provState = db.GetLabel("InboundTracking", "ProvState", pageLang);
            lblProvState.Text = provState;
            lblSource.Text = db.GetLabel("InboundTracking", "SourceOfCall", pageLang);
            lblCustomer.Text = db.GetLabel("InboundTracking", "CustomerType", pageLang);
            lblAboriginal.Text = db.GetLabel("InboundTracking", "AboriginalStatus", pageLang);
            ddlClass.Items[1].Text = db.GetLabel("InboundTracking", "Aboriginal", pageLang);
            ddlClass.Items[2].Text = db.GetLabel("InboundTracking", "NonAboriginal", pageLang);
            ddlClass.Items[3].Text = db.GetLabel("InboundTracking", "NoReply", pageLang);
            ddlClass.Items[4].Text = db.GetLabel("InboundTracking", "Unknown", pageLang);
            //need to figure out what we're going to do with this button because of the line break 
            string qaText = db.GetLabel("InboundTracking", "btnQaProgInfoRes", pageLang).Replace("<br>", " ");
            btnQA.Text = qaText;
            if (Session["KBKeys"] != null)
                btnQA.Text += " " + checkMarkText.InnerText;
            string subject = db.GetLabel("InboundTracking", "Subject", pageLang);
            btnSubject.Text = subject;
            if (Session["SubCodes"] != null)
                btnSubject.Text += " " + checkMarkText.InnerText;
            lblMandatory.Text = db.GetLabel("InboundTracking", "Mandatory", pageLang);
            string issuesText = db.GetLabel("InboundTracking", "Issues", pageLang);
            lblIssues.Text = issuesText;
            string orderStatus = db.GetLabel("StatsList", "OrderStatus", pageLang);
            lblOrderStatus.Text = orderStatus;
            string callBack = db.GetLabel("InboundTracking", "DateTimeToCallback", pageLang);
            lblCallBack.Text = callBack;
            string answer = db.GetLabel("InboundTracking", "Answer", pageLang);
            lblAnswer.Text = answer;
            lblCustDetails.Text = db.GetLabel("InboundTracking", "CustomerShippingDetails", pageLang);
            lblCustomerAccount.Text = db.GetLabel("InboundTracking", "CustomerAccount", pageLang);
            lblPostZip.Text = db.GetLabel("InboundTracking", "PostZipCode", pageLang);
            string phone = db.GetLabel("InboundTracking", "Phone", pageLang);
            lblPhone.Text = phone;
            string firstName = db.GetLabel("InboundTracking", "FirstName", pageLang);
            lblFirstName.Text = firstName;
            lblFax.Text = db.GetLabel("InboundTracking", "Fax", pageLang);
            string surName = db.GetLabel("InboundTracking", "Surname", pageLang);
            lblSurname.Text = surName;
            string organization = db.GetLabel("InboundTracking", "Organization", pageLang);
            lblOrganization.Text = organization;
            lblDelivered.Text = db.GetLabel("InboundTracking", "Delivered", pageLang);
            lblBackOrder.Text = db.GetLabel("InboundTracking", "BackOrder2", pageLang);
            lblEmail.Text = email;
            lblProvState2.Text = provState;
            lblCountry.Text = db.GetLabel("InboundTracking", "Country", pageLang);
            lblCity.Text = db.GetLabel("InboundTracking", "City", pageLang);
            lblAddress.Text = db.GetLabel("InboundTracking", "Address", pageLang);
            lblOrderDelivery.Text = db.GetLabel("InboundTracking", "OrderDeliveryDetails", pageLang);
            lblDeliveryMode.Text = db.GetLabel("InboundTracking", "DeliveryMode", pageLang);
            string shippingNotes = db.GetLabel("ProductOrder", "ShippingNotes", pageLang);
            btnShipNotes.Text = shippingNotes;
            btnAddItem.Text = db.GetLabel("InboundTracking", "btnAddItem", pageLang);
            btnAddElectronic.Text = db.GetLabel("InboundTracking", "btnAddElectronic", pageLang);
            btnSendEmail.Text = db.GetLabel("InboundTracking", "btnSendEmail", pageLang);
            string productNo = db.GetLabel("InboundTracking", "ProductNo", pageLang);
            thcProduct.Text = productNo;
            string description = db.GetLabel("InboundTracking", "Description", pageLang);
            thcDesc.Text = description;
            thcQTY.Text = db.GetLabel("InboundTracking", "Qty", pageLang);
            thcProcess.Text = db.GetLabel("InboundTracking", "ProcessStatus", pageLang);
            thcDate.Text = db.GetLabel("InboundTracking", "DateSent", pageLang);
            thcSale.Text = db.GetLabel("InboundTracking", "Sale", pageLang);
            thcGST.Text = db.GetLabel("InboundTracking", "GST", pageLang);
            thcPST.Text = db.GetLabel("InboundTracking", "PST", pageLang);
            thcTotal.Text = db.GetLabel("InboundTracking", "ItemTotal", pageLang);
            thcKit.Text = db.GetLabel("InboundTracking", "KitStatus", pageLang);
            thcWH.Text = db.GetLabel("InboundTracking", "WhNo", pageLang);
            thcWHStatus.Text = db.GetLabel("InboundTracking", "WarehouseStatus", pageLang);
            thcNotes.Text = db.GetLabel("InboundTracking", "Notes", pageLang);
            thcFormat.Text = db.GetLabel("InboundTracking", "Format", pageLang);
            lblProductLines.Text = db.GetLabel("InboundTracking", "ProductLines", pageLang);
            lblTotalQty.Text = db.GetLabel("InboundTracking", "TotalQty", pageLang);
            lblTotalPrice.Text = db.GetLabel("InboundTracking", "TotalPrice", pageLang);
            string client = db.GetLabel("InboundTracking", "Client", pageLang);
            lblClientKB.Text = client;
            string ticketNumText = db.GetLabel("InboundTracking", "Ticket", pageLang);
            lblTicketNumKB.Text = ticketNumText;
            string campaign = db.GetLabel("InboundTracking", "Campaign", pageLang);
            lblCampaignKB.Text = campaign;
            lblSearchForKB.Text = db.GetLabel("InboundTracking", "PubSrchFor", pageLang).Replace(":", "");
            thcSubjKB.Text = subject;
            thcDescKB.Text = description;
            thcEmailKB.Text = email;
            thcFirstKB.Text = firstName;
            thcLastKB.Text = surName;
            string phoneKB = db.GetLabel("InboundTracking", "IMPTelePhone", pageLang);
            thcPhoneKB.Text = phoneKB;
            thcSubjKB.Text = subject;
            gvKB.Columns[3].HeaderText = subject;
            gvKB.Columns[4].HeaderText = firstName;
            gvKB.Columns[5].HeaderText = surName;
            gvKB.Columns[6].HeaderText = phoneKB;
            gvKB.Columns[7].HeaderText = email;
            gvKB.Columns[8].HeaderText = description;
            lblIssuesKB.Text = issuesText;
            lblAnswerKB.Text = answer;
            lblDateReceivedKB.Text = db.GetLabel("InboundTracking", "DateRecv", pageLang);
            lblCallbackKB.Text = callBack;
            lblOrderStatusKB.Text = orderStatus;
            lblResponseTimeKB.Text = db.GetLabel("InboundTracking", "DateTimeOfResponse", pageLang);
            lblResearchKB.Text = db.GetLabel("InboundTracking", "ResearchTime", pageLang);
            lblOtherRegion.Text = db.GetLabel("InboundTracking", "OtherRegion", pageLang);
            string search = db.GetLabel("InboundTracking", "Search2", pageLang);
            btnSearchKB.Text = search;
            btnProductSearch.Text = search;
            string listAll = db.GetLabel("InboundTracking", "ListAll", pageLang);
            btnListAll.Text = listAll;
            btnDisplayAll.Text = listAll;
            string addText = db.GetLabel("InboundTracking", "Add", pageLang);
            btnAddToOrder.Text = addText;
            btnShowSelected.Text = db.GetLabel("InboundTracking", "ShowSelected", pageLang);
            btnHighlightSearch.Text = db.GetLabel("InboundTracking", "SearchHighlightedText", pageLang);
            lblCampaignKBD.Text = campaign;
            lblUsageKBD.Text = db.GetLabel("KnowledgeBaseDetails", "OpUsageCoding", pageLang);
            chkActive.Text = db.GetLabel("KnowledgeBaseDetails", "Active", pageLang);
            lblDateEditKBD.Text = db.GetLabel("KnowledgeBaseDetails", "DateEdit", pageLang);
            lblDateInputKBDText.Text = db.GetLabel("KnowledgeBaseDetails", "DateInput", pageLang);
            lblOperatorKBD.Text = db.GetLabel("KnowledgeBaseDetails", "Operator", pageLang);
            lblReferralKBD.Text = db.GetLabel("KnowledgeBaseDetails", "ReferalNo", pageLang);
            lblSubjKBD.Text = db.GetLabel("KnowledgeBaseDetails", "SubjectQuestionEnglishText", pageLang);
            lblSubjFrKBD.Text = db.GetLabel("KnowledgeBaseDetails", "SubjectQuestionFrenchText", pageLang);
            lblDescKBD.Text = db.GetLabel("KnowledgeBaseDetails", "DescriptionAnswerEnglish", pageLang);
            lblTypeKBD.Text = db.GetLabel("KnowledgeBaseDetails", "ListCodeType", pageLang);
            chkReferral.Text = " " + db.GetLabel("KnowledgeBaseDetails", "Referral", pageLang);
            chkQA.Text = " " + db.GetLabel("KnowledgeBaseDetails", "QnAFAQ", pageLang);
            chkProgram.Text = " " + db.GetLabel("KnowledgeBaseDetails", "ProgramInfo", pageLang);
            lblDivisionKBD.Text = db.GetLabel("KnowledgeBaseDetails", "DivisionBranch", pageLang);
            lblKeywordKBD.Text = db.GetLabel("KnowledgeBaseDetails", "ddKeyword", pageLang);
            lblFirstKBD.Text = db.GetLabel("KnowledgeBaseDetails", "ContactFirstName2", pageLang);
            lblLastKBD.Text = db.GetLabel("KnowledgeBaseDetails", "Surname", pageLang);
            lblCompanyKBD.Text = db.GetLabel("KnowledgeBaseDetails", "Company", pageLang);
            lblPhoneKBD.Text = db.GetLabel("KnowledgeBaseDetails", "Phone", pageLang);
            lblFaxKBD.Text = db.GetLabel("KnowledgeBaseDetails", "Fax", pageLang);
            lblAddressKBD.Text = db.GetLabel("KnowledgeBaseDetails", "Address", pageLang);
            lblCityProvKBD.Text = db.GetLabel("KnowledgeBaseDetails", "CityProvince2", pageLang);
            lblPostalKBD.Text = db.GetLabel("KnowledgeBaseDetails", "SrchPostal", pageLang);
            lblEmailKBD.Text = email;
            string webSite = db.GetLabel("KnowledgeBaseDetails", "WebSiteURL", pageLang);
            lblWebKBD.Text = webSite;
            lblTicketNumKBD.Text = ticketNumText;
            lblSubjQuesKBD.Text = db.GetLabel("KnowledgeBaseDetails", "SubjectsQuestions2", pageLang);
            lblDescDetKBD.Text = db.GetLabel("KnowledgeBaseDetails", "DescriptionDetails", pageLang);
            lblClientProd.Text = client;
            lblTicketNumProd.Text = ticketNumText;
            lblSearchProd.Text = db.GetLabel("InboundTracking", "SearchOnTitle", pageLang);
            gvInventory.Columns[1].HeaderText = productNo;
            gvInventory.Columns[2].HeaderText = db.GetLabel("InboundTracking", "Title", pageLang);
            gvInventory.Columns[3].HeaderText = db.GetLabel("InboundTracking", "WH0Qty", pageLang);
            gvInventory.Columns[4].HeaderText = db.GetLabel("InboundTracking", "WH1Qty", pageLang);
            gvInventory.Columns[5].HeaderText = db.GetLabel("InboundTracking", "WH2Qty", pageLang);
            gvInventory.Columns[6].HeaderText = db.GetLabel("InboundTracking", "WH3Qty", pageLang);
            gvInventory.Columns[7].HeaderText = db.GetLabel("InboundTracking", "WH4Qty", pageLang);
            gvInventory.Columns[8].HeaderText = db.GetLabel("InboundTracking", "WH5Qty", pageLang);
            gvInventory.Columns[9].HeaderText = db.GetLabel("InboundTracking", "WH6Qty", pageLang);
            gvInventory.Columns[10].HeaderText = db.GetLabel("InboundTracking", "WH7Qty", pageLang);
            gvInventory.Columns[11].HeaderText = db.GetLabel("InboundTracking", "WH8Qty", pageLang);
            gvInventory.Columns[12].HeaderText = db.GetLabel("InboundTracking", "WH9Qty", pageLang);
            gvInventory.Columns[13].HeaderText = db.GetLabel("InboundTracking", "OtherWH", pageLang);
            gvInventory.Columns[14].HeaderText = db.GetLabel("SkanQty", "QtyOnHandSkn", pageLang);
            gvInventory.Columns[15].HeaderText = db.GetLabel("InboundTracking", "Cmtd", pageLang);
            string statusText = db.GetLabel("InboundTracking", "Status", pageLang);
            gvInventory.Columns[16].HeaderText = statusText;
            gvInventory.Columns[17].HeaderText = db.GetLabel("InboundTracking", "Branch", pageLang);
            gvInventory.Columns[18].HeaderText = db.GetLabel("InboundTracking", "Price", pageLang);
            gvInventory.Columns[19].HeaderText = db.GetLabel("InboundTracking", "KitStatus", pageLang);
            btnHot.Text = db.GetLabel("InventorySelList", "InvSrchHotNew", pageLang);
            lblSearchExp.Text = db.GetLabel("StatsList", "SearchExplaination", pageLang);
            lblOrderSearch.Text = db.GetLabel("StatsList", "SearchOn", pageLang);
            string dateText = db.GetLabel("InboundTracking", "Date", pageLang);
            ddlCriteria.Items[1].Text = dateText;
            ddlCriteria.Items[2].Text = ticketNumText;
            btnOrderSearch.Text = db.GetLabel("StatsList", "Submit", pageLang);
            btnShowIncomplete.Text = db.GetLabel("StatsList", "StatlistShowAllWork", pageLang);
            lblOrderGroup.Text = db.GetLabel("StatsList", "Group", pageLang);
            lblStatusOrder.Text = statusText;
            lblLowValue.Text = db.GetLabel("StatsList", "LowValue", pageLang);
            lblHighVal.Text = db.GetLabel("StatsList", "HighValue", pageLang);
            gvOrderStatus.Columns[1].HeaderText = ticketNumText;
            gvOrderStatus.Columns[2].HeaderText = db.GetLabel("StatsList", "DateIn", pageLang);
            gvOrderStatus.Columns[3].HeaderText = db.GetLabel("StatsList", "GroupUser", pageLang);
            gvOrderStatus.Columns[4].HeaderText = db.GetLabel("StatsList", "CustomerName", pageLang);

            string assignedTo = db.GetLabel("StatsList", "Assignedto", pageLang);
            gvOrderStatus.Columns[5].HeaderText = assignedTo;
            gvOrderStatus.Columns[6].HeaderText = orderStatus;

            gvOrderStatus.Columns[7].HeaderText = db.GetLabel("StatsList", "DatetoCallBack", pageLang);
            
            gvOrderStatus.Columns[8].HeaderText = db.GetLabel("StatsList", "StatusDate", pageLang);

            gvOrderStatus.Columns[9].HeaderText = db.GetLabel("StatsList", "ProductsOrdered", pageLang);
            gvOrderStatus.Columns[10].HeaderText = db.GetLabel("StatsList", "Shipped", pageLang);
            gvOrderStatus.Columns[11].HeaderText = db.GetLabel("StatsList", "Question", pageLang);
            gvOrderStatus.Columns[12].HeaderText = answer;
            lblOrderAssignTo.Text = db.GetLabel("StatsList", "AssignTo", pageLang);
            btnAssign.Text = db.GetLabel("StatsList", "Assign", pageLang);
            btnPrintReport.Text = db.GetLabel("RepEntry", "PrintRep", pageLang);
            lblSubjClient.Text = client;
            lblSubjMessage.Text = db.GetLabel("InboundTracking", "SelectSubjectsContinue", pageLang);
            lblSubjTicketNum.Text = ticketNumText;
            lblSubjList.Text = db.GetLabel("InboundTracking", "SubjectList", pageLang);
            gvSubjects.Columns[1].HeaderText = subject;
            string codeText = db.GetLabel("InboundTracking", "Code", pageLang);
            gvSubjects.Columns[2].HeaderText = codeText;
            lblOtherSubj.Text = otherText;
            btnContinue.Text = db.GetLabel("InboundTracking", "Continue", pageLang);
            ddlWH.Items[0].Text = db.GetLabel("InboundTracking", "AllWH", pageLang);
            string closeText = db.GetLabel("InboundTracking", "Close", pageLang);
            btnClose.Text = closeText;
            btnCloseIssues.Text = closeText;
            btnCloseShipping.Text = closeText;
            lblCurrentComment.Text = db.GetLabel("InboundTracking", "CurrentCommentDetails", pageLang) + ":";
            lblCommentHist.Text = db.GetLabel("InboundTracking", "CommentHistoryforTicket#", pageLang) + ":";
            lblMoreDetails.Text = db.GetLabel("InboundTracking", "Clickonlinetoseemoredetails", pageLang) + ".";
            gvCommentHistory.Columns[1].HeaderText = dateText;
            string userText = db.GetLabel("InboundTracking", "User", pageLang);
            gvCommentHistory.Columns[2].HeaderText = userText;
            gvCommentHistory.Columns[3].HeaderText = db.GetLabel("InboundTracking", "OriginalComment", pageLang);
            lblSelComment.Text = db.GetLabel("InboundTracking", "CommentDetailsfromselectedhistory line", pageLang);
            lblIssuesAZ.Text = issuesText;
            lblAnswerAZ.Text = answer;
            lblSelClientMessage.InnerText = db.GetLabel("InboundTracking", "SelectClient", pageLang);
            string noResults = db.GetLabel("RptR19a", "R19aRepNoResult", pageLang);
            //lblDescDetKBD.Text = 
            gvClientSearch.EmptyDataText = noResults;
            gvCommentHistory.EmptyDataText = db.GetLabel("InboundTracking", "NoCommentHist", pageLang);
            lblSelPromo.InnerText = db.GetLabel("InboundTracking", "SelectPromo", pageLang);
            gvPromotions.Columns[1].HeaderText = db.GetLabel("InboundTracking", "Promotion", pageLang); //need to change this label, maybe only in test? looks ok in dev
            gvPromotions.Columns[2].HeaderText = codeText;
            lblBreadcrumb.Text = btnInbound.Text;

        }//setLabels

        private string createIncLineVar()
        {
            string incline = "";
            for (int i = 1; i < ddlLine.Items.Count; ++i)
            {
                incline += ddlLine.Items[i].Value.Split('|').ElementAt(0).ToString() + ", ";
            }
            return incline;

        }//createIncLineVar

        private void loadIncLines(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ListItem listItem;
                DataRowCollection incomRows = ds.Tables[0].Rows;
                for (int i = 0; i < incomRows.Count; ++i)
                {
                    string val = incomRows[i]["TELE_NO"].ToString() + "|" + incomRows[i]["Script"].ToString() + "|" +
                        incomRows[i]["TELE_SURVEY_DATA"].ToString() + "|" + incomRows[i]["URL"].ToString() + "|" +
                        incomRows[i]["TELE_REGION_DATA"].ToString();

                    //second element will be the description, regardless of language
                    string text = incomRows[i].ItemArray.ElementAt(2).ToString();

                    listItem = new ListItem(text, val);
                    ddlLine.Items.Add(listItem);

                }//for all the returned rows

            }//if there are incoming lines in the db

        }//loadIncLines

        private void loadEnquiry(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["KBKeys"] = null;
                Session["SubCodes"] = null;
                Session["PromoSelected"] = null;
                Session["OrderStatusDate"] = null;
                DataRow enquiryRow = ds.Tables[0].Rows[0];
                bool englishFlag = Convert.ToBoolean(int.Parse(enquiryRow["S_LANG_E"].ToString()));
                bool frenchFlag = Convert.ToBoolean(int.Parse(enquiryRow["S_LANG_F"].ToString()));
                bool otherFlag = Convert.ToBoolean(int.Parse(enquiryRow["S_OLANG_FLAG"].ToString()));

                if (englishFlag)
                    rblLanguage.SelectedIndex = 0;

                if (frenchFlag)
                    rblLanguage.SelectedIndex = 1;

                if (otherFlag)
                {
                    string otherLang = enquiryRow["S_L_OTHER"].ToString();
                    txtLanguage.Text = otherLang;
                    rblLanguage.SelectedIndex = 2;

                }

                Session["Language"] = rblLanguage.SelectedValue;
                Session["OtherLanguage"] = txtLanguage.Text;

                if (!string.IsNullOrEmpty(enquiryRow["S_DATE_INPUT"].ToString()))
                {
                    string[] inputDateTime = enquiryRow["S_DATE_INPUT"].ToString().Split(' ');
                    string inputDate = inputDateTime.ElementAt(0);
                    string inputTime = inputDateTime.ElementAt(1);
                    txtInputDate.Text = inputDate;
                    Session["InputDate"] = inputDate;
                    Session["InputTime"] = inputTime;

                }// if the row contains date input

                if (!string.IsNullOrEmpty(enquiryRow["S_RESEARCH_OVER_UNDER_10"].ToString()))
                {
                    string overUnder = enquiryRow["S_RESEARCH_OVER_UNDER_10"].ToString();
                    rblResearch.SelectedValue = overUnder;
                    Session["OverUnder"] = overUnder;

                }

                if (!string.IsNullOrEmpty(enquiryRow["S_RESEARCH_DURATION"].ToString()))
                {
                    string researchDuration = enquiryRow["S_RESEARCH_DURATION"].ToString();
                    txtResearchTime.Text = researchDuration;
                }
                Session["ResearchDuration"] = txtResearchTime.Text;

                string initialAgent = enquiryRow["S_OPERATOR"].ToString();
                txtInitialAgent.Text = initialAgent;
                Session["InitialAgent"] = initialAgent;

                bool phoneFlag = Convert.ToBoolean(int.Parse(enquiryRow["S_TELE"].ToString()));
                bool inPersonFlag = Convert.ToBoolean(int.Parse(enquiryRow["S_VISIT"].ToString()));
                bool faxFlag = Convert.ToBoolean(int.Parse(enquiryRow["S_FAX"].ToString()));
                bool emailFlag = Convert.ToBoolean(int.Parse(enquiryRow["S_EMAIL"].ToString()));
                bool webFlag = Convert.ToBoolean(int.Parse(enquiryRow["S_WWW"].ToString()));
                bool voiceMailFlag = Convert.ToBoolean(int.Parse(enquiryRow["S_VOICE_M"].ToString()));
                bool mailFlag = Convert.ToBoolean(int.Parse(enquiryRow["S_WRITTEN"].ToString()));
                bool exhibitsFlag = Convert.ToBoolean(int.Parse(enquiryRow["S_INTERNAL"].ToString()));

                if (phoneFlag)
                    rblCommunication.SelectedIndex = 0;

                if (inPersonFlag)
                    rblCommunication.SelectedIndex = 1;

                if (faxFlag)
                    rblCommunication.SelectedIndex = 3;

                if (emailFlag)
                    rblCommunication.SelectedIndex = 5;

                if (webFlag)
                    rblCommunication.SelectedIndex = 7;

                if (voiceMailFlag)
                    rblCommunication.SelectedIndex = 4;

                if (mailFlag)
                    rblCommunication.SelectedIndex = 2;

                if (exhibitsFlag)
                    rblCommunication.SelectedIndex = 6;

                Session["Communication"] = rblCommunication.SelectedIndex;

                string issues = enquiryRow["S_COMMENTS"].ToString();
                txtIssues.Text = issues;
                Session["Issues"] = issues;

                bool promoSelected = false;

                if (!string.IsNullOrEmpty(enquiryRow["S_SRC_DEF"].ToString()) || !string.IsNullOrEmpty(enquiryRow["S_OP_NO"].ToString()))
                {
                    string source = "";
                    if (!string.IsNullOrEmpty(enquiryRow["S_OP_NO"].ToString()))
                    {

                        source = enquiryRow["S_OP_NO"].ToString();
                        try
                        {
                            ddlSource.SelectedValue = source;
                            promoSelected = true;
                        }
                        catch (Exception ex)
                        {
                            ddlSource.SelectedValue = "";
                        }

                    }//if there is an op_no

                    else
                    {
                        source = enquiryRow["S_SRC_DEF"].ToString();
                        ListItem itemToSelect = ddlSource.Items.FindByText(source);
                        try
                        {
                            ddlSource.SelectedValue = itemToSelect.Value;
                            promoSelected = true;
                        }
                        catch (Exception ex)
                        {
                            ddlSource.SelectedValue = "";
                        }

                    }//if theres a source in the row

                }// if there is a source def or a op_no

                Session["Source"] = ddlSource.SelectedValue;
                Session["PromoSelected"] = promoSelected;

                string prov = enquiryRow["S_PROV"].ToString();
                try
                {
                    ddlProvState.SelectedValue = prov;
                }
                catch (Exception ex)
                {
                    ddlProvState.SelectedValue = "";
                }
                Session["Province"] = ddlProvState.SelectedValue;

                string custType = enquiryRow["S_CUSTOMER_TYPE"].ToString();
                try
                {
                    ddlCust.SelectedValue = custType;
                }
                catch
                {
                    ddlCust.SelectedValue = "";
                }
                Session["CustomerType"] = ddlCust.SelectedValue;

                int gender = int.Parse(enquiryRow["S_GENDER"].ToString());
                if (gender == 0)
                    rblGender.SelectedIndex = 1;
                else if (gender == 1)
                    rblGender.SelectedIndex = 0;
                else
                    rblGender.SelectedIndex = 2;

                Session["Gender"] = rblGender.SelectedValue;

                string owner = enquiryRow["S_OWNER"].ToString();
                txtOwner.Text = owner;
                Session["Owner"] = owner;

                string orderStatus = enquiryRow["S_ORDER_STATUS"].ToString();
                try
                {
                    ddlOrderStatus.SelectedValue = orderStatus;
                }
                catch (Exception ex)
                {
                    ddlOrderStatus.SelectedValue = "010";
                }
                Session["OrderStatus"] = ddlOrderStatus.SelectedValue;

                if (!string.IsNullOrEmpty(enquiryRow["S_ORDER_STATUS_DATE"].ToString()))
                    Session["OrderStatusDate"] = enquiryRow["S_ORDER_STATUS_DATE"].ToString();

                if (!string.IsNullOrEmpty(enquiryRow["S_DATE_TO_CALLBACK"].ToString()))
                {
                    string[] callbackDateTime = enquiryRow["S_DATE_TO_CALLBACK"].ToString().Split(' ');
                    string callbackDate = callbackDateTime.ElementAt(0);
                    string callbackTime = callbackDateTime.ElementAt(1);
                    txtDate.Text = callbackDate;
                    txtTime.Text = callbackTime;

                }// if date to call back is in the row

                Session["CallbackDate"] = txtDate.Text;
                Session["CallbackTime"] = txtTime.Text;

                if (!string.IsNullOrEmpty(enquiryRow["S_DATE_BACK"].ToString()))
                {
                    string[] dateBack = enquiryRow["S_DATE_BACK"].ToString().Split(' ');
                    string dateBackDate = dateBack.ElementAt(0);
                    string dateBackTime = dateBack.ElementAt(1);
                    txtResponseDate.Text = dateBackDate;
                    txtResponseTime.Text = dateBackTime;

                }// if date back is in the row

                Session["DatebackDate"] = txtResponseDate.Text;
                Session["DatebackTime"] = txtResponseTime.Text;

                if (!string.IsNullOrEmpty(enquiryRow["S_DATE_EDIT"].ToString()))
                {
                    string[] EditDateTime = enquiryRow["S_DATE_EDIT"].ToString().Split(' ');
                    string editDate = EditDateTime.ElementAt(0);
                    string editTime = EditDateTime.ElementAt(1);
                    txtEditDate.Text = editDate;
                    Session["EditDate"] = editDate;
                    Session["EditTime"] = editTime;
                }//if date edit is in the row

                string editUser = enquiryRow["S_USER_EDIT"].ToString();
                txtEditAgent.Text = editUser;
                Session["EditAgent"] = editUser;

                string classification = enquiryRow["S_ABORIGINAL"].ToString();
                try
                {
                    ddlClass.SelectedValue = classification;
                }
                catch (Exception ex)
                {
                    ddlClass.SelectedValue = "0";
                }
                Session["Classification"] = ddlClass.SelectedValue;

                string teleused = enquiryRow["S_TELE_USED"].ToString();

                ddlLine.ClearSelection();
                foreach (ListItem item in ddlLine.Items)
                {
                    if (item.Value.Contains(teleused))
                    {
                        item.Selected = true;
                        break;
                    }//if the item contains the teleused value

                }// check the list for the incoming line
                Session["LineUsed"] = ddlLine.SelectedValue;

                string answerText = enquiryRow["S_OPEN_ANSWER"].ToString();
                txtAnswer.Text = answerText;
                Session["Answer"] = answerText;

                string referralFlag = enquiryRow["S_REFERRAL"].ToString();
                if (!string.IsNullOrEmpty(referralFlag) && int.Parse(referralFlag) == 1)
                {
                    if (Session["KBKeys"] != null)
                        Session["KBKeys"] += enquiryRow["S_REF_TO"].ToString();
                    else
                        Session["KBKeys"] = enquiryRow["S_REF_TO"].ToString();

                }//if the referral flag is set to 1

                string adivceFlag = enquiryRow["S_ADVICE"].ToString();
                if (!string.IsNullOrEmpty(adivceFlag) && int.Parse(adivceFlag) == 1)
                {
                    if (Session["KBKeys"] != null)
                        Session["KBKeys"] += enquiryRow["S_ADVICE_TO"].ToString();
                    else
                        Session["KBKeys"] = enquiryRow["S_ADVICE_TO"].ToString();

                }//if the advice flag is set to 1

                string corpFlag = enquiryRow["S_CORP"].ToString();
                if (!string.IsNullOrEmpty(corpFlag) && int.Parse(corpFlag) == 1)
                {
                    if (Session["KBKeys"] != null)
                        Session["KBKeys"] += enquiryRow["S_CORP_INFO_TO"].ToString();
                    else
                        Session["KBKeys"] = enquiryRow["S_CORP_INFO_TO"].ToString();

                }//if the corp flag is set to 1

                string subject = enquiryRow["S_SUBJ_STRING"].ToString();
                if (!string.IsNullOrEmpty(subject))
                    Session["SubCodes"] = subject;

                string openSubject = enquiryRow["S_OPEN_SUBJECT"].ToString();
                txtOtherSubject.Text = openSubject;

                string otherRegion = enquiryRow["S_OPEN_REGION"].ToString();
                txtOther.Text = otherRegion;

            }//if there is a row returned

        }//loadEnquiry

        private void loadCustomerDetails(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow customerRow = ds.Tables[0].Rows[0];

                string accountNumber = customerRow["fs_rec_no"].ToString();
                if (accountNumber.Equals("0"))
                    accountNumber = "";
                txtAccount.Text = accountNumber;
                Session["Account"] = accountNumber;

                string firstName = customerRow["fs_fname"].ToString();
                txtFirst.Text = firstName;
                Session["FirstName"] = firstName;

                string lastName = customerRow["fs_lname"].ToString();
                txtLast.Text = lastName;
                Session["LastName"] = lastName;

                string company = customerRow["fs_company"].ToString();
                txtOrganization.Text = company;
                Session["Organization"] = company;

                string street = customerRow["fs_street"].ToString();
                txtAddress.Text = street;
                Session["Address"] = street;

                string street2 = customerRow["fs_street2"].ToString();
                txtAddress2.Text = street2;
                Session["Address2"] = street2;

                string city = customerRow["fs_city"].ToString();
                txtCity.Text = city;
                Session["City"] = city;

                string provCode = customerRow["fs_prov_code"].ToString();
                try
                {
                    ddlProvState2.SelectedValue = provCode;
                }
                catch (Exception ex)
                {
                    ddlProvState2.SelectedValue = "";
                }
                Session["Province2"] = provCode;

                string country = customerRow["fs_country"].ToString();
                txtCountry.Text = country;
                Session["Country"] = country;

                string phone = customerRow["fs_tel"].ToString();
                txtPhone.Text = phone;
                Session["Phone"] = phone;

                string fax = customerRow["fs_fax"].ToString();
                txtFax.Text = fax;
                Session["Fax"] = fax;

                string postalCode = customerRow["fs_post_code"].ToString();
                txtPostZip.Text = postalCode;
                Session["PostalCode"] = postalCode;

                string email = customerRow["fs_email"].ToString();
                txtEmail.Text = email;
                Session["Email"] = email;

                string backOrder = customerRow["fs_back_order"].ToString();
                txtBackOrder.Text = backOrder;
                Session["BackOrder"] = backOrder;

                //string readyToShip = customerRow["fs_ready_to_ship"].ToString();
                //txtReady.Text = readyToShip;
                //Session["ReadyToShip"] = readyToShip;

                string deliveredFlag = customerRow["fs_delivered_flag"].ToString();
                txtDelivered.Text = deliveredFlag;
                Session["DeliveredFlag"] = deliveredFlag;

                string deliveredDate = customerRow["fs_delivered"].ToString();
                txtDelivered.Text = deliveredDate;
                Session["DeliveredDate"] = deliveredDate;

                string delivery = customerRow["fs_delivery"].ToString();
                try
                {
                    ddlDelivery.SelectedValue = delivery;
                }
                catch (Exception ex)
                {
                    ddlDelivery.SelectedValue = "";
                }
                Session["Delivery"] = ddlDelivery.SelectedValue;

                string shipNotes = customerRow["fs_notes"].ToString();
                txtShipNotes.Text = shipNotes;
                Session["ShipNotes"] = shipNotes;

                string shipDetails = customerRow["fs_shipping_details"].ToString();
                txtShipDetails.Text = shipDetails;
                Session["ShipDetails"] = shipDetails;

            }//if a row is returned

        }//loadCustomerDetails

        private void loadProducts(DataSet ds)
        {
            int count = 0;
            int totalQTY = 0;
            double totalPrice = 0;
            int i = 1;
            string strHardcopy, strElectronic;

            LanguageDB db = new LanguageDB();
            strHardcopy = db.GetLabel("InboundTracking", "Hardcopy", pageLang);
            strElectronic = db.GetLabel("InboundTracking", "Electronic", pageLang); 

            DataRowCollection productRows = ds.Tables[0].Rows;
            Session["InventoryCodes"] = null;
            foreach (DataRow row in productRows)
            {
                ++count;
                TableRow tableRow = new TableRow();
                TableCell delCell = new TableCell();
                delCell.ID = "delCell" + i;
                ImageButton imgbtnDelProduct = new ImageButton();
                imgbtnDelProduct.ID = "imgbtnDelProduct" + count.ToString();
                imgbtnDelProduct.ImageUrl = "images/delete.gif";
                imgbtnDelProduct.AlternateText = "Delete button";
                imgbtnDelProduct.Click += imgbtnDelete_Click;
                imgbtnDelProduct.OnClientClick = "return confirmDelete()";

                if (row["PROD_TYPE"].ToString().ToUpper() == "E")
                {
                    if (row.Table.Columns.Contains("PR_DATE_SENT"))
                    {
                        if (row["PR_DATE_SENT"].ToString().Length > 0)
                        {
                            imgbtnDelProduct.Visible = false;
                        }
                    }
                }

                delCell.Controls.Add(imgbtnDelProduct);
                tableRow.Cells.Add(delCell);
                string dCell = "bodyContent_" + delCell.ID.ToString();
                string delBtn = "bodyContent_" + imgbtnDelProduct.ID.ToString();
                //ALLAN: remove registers because it messes things up
                //ScriptManager.RegisterStartupScript(this, GetType(), "addDelLabel", "addLabelFor(" + dCell +
                 //   ", " + delBtn + ");", true);

                TableCell prodNumberCell = new TableCell();
                if (row.Table.Columns.Contains("PR_INVENTORY_CODE"))
                {
                    prodNumberCell.Text = row["PR_INVENTORY_CODE"].ToString();
                    if (Session["InventoryCodes"] != null)
                        Session["InventoryCodes"] += prodNumberCell.Text + ", ";
                    else
                        Session["InventoryCode"] = prodNumberCell.Text + ", ";
                }
                tableRow.Cells.Add(prodNumberCell);


                TableCell descriptionCell = new TableCell();
                if (row.Table.Columns.Contains("PR_TITLE"))
                    descriptionCell.Text = shortenLabel(row["PR_TITLE"].ToString());
                tableRow.Cells.Add(descriptionCell);

                TableCell quantityCell = new TableCell();
                quantityCell.ID = "quantCell" + i;
                TextBox txtQuantity = new TextBox();
                if (row.Table.Columns.Contains("PR_QUANTITY"))
                {
                    txtQuantity.Text = row["PR_QUANTITY"].ToString();
                    txtQuantity.ID = "txtQuantity" + i;
                    totalQTY += int.Parse(row["PR_QUANTITY"].ToString());
                    txtQuantity.Width = 30;
                    string quantCell = "bodyContent_" + quantityCell.ID.ToString();
                    string qtyTxt = "bodyContent_" + txtQuantity.ID.ToString();

                    // ALLAN: Remove this because its messing up

                    //ScriptManager.RegisterStartupScript(this, GetType(), "addQtyLabel", "addLabelFor(" + quantCell +
                    //    ", " + qtyTxt + ");", true);

                    quantityCell.Controls.Add(txtQuantity);
                    if (Session["Action"] != null && (Session["Action"].ToString().Equals("Edit") || (Session["Action"].ToString().Equals("New"))))
                        txtQuantity.Enabled = true;
                    else
                        txtQuantity.Enabled = false;
                    ++i;

                }// if pr_quantity is in the table
                tableRow.Cells.Add(quantityCell);

                TableCell statusCell = new TableCell();
                if (row.Table.Columns.Contains("PR_PROCESS_STATUS"))
                    statusCell.Text = setProcStatus(int.Parse(row["PR_PROCESS_STATUS"].ToString()));
                tableRow.Cells.Add(statusCell);

                TableCell dateCell = new TableCell();
                if (row.Table.Columns.Contains("PR_DATE_SENT"))
                    dateCell.Text = row["PR_DATE_SENT"].ToString();
                tableRow.Cells.Add(dateCell);

                TableCell salePriceCell = new TableCell();
                if (row.Table.Columns.Contains("PR_SALE"))
                    salePriceCell.Text = row["PR_SALE"].ToString();
                tableRow.Cells.Add(salePriceCell);

                TableCell GstCell = new TableCell();
                if (row.Table.Columns.Contains("PR_GST"))
                    GstCell.Text = row["PR_GST"].ToString();
                tableRow.Cells.Add(GstCell);

                TableCell PstCell = new TableCell();
                if (row.Table.Columns.Contains("PR_PST"))
                    PstCell.Text = row["PR_PST"].ToString();
                tableRow.Cells.Add(PstCell);

                TableCell totalCell = new TableCell();
                if (row.Table.Columns.Contains("PR_TOTAL"))
                {
                    totalCell.Text = row["PR_TOTAL"].ToString();
                    totalPrice += double.Parse(row["PR_TOTAL"].ToString());
                }
                tableRow.Cells.Add(totalCell);

                TableCell kitCell = new TableCell();
                if (row.Table.Columns.Contains("PR_KIT"))
                    kitCell.Text = row["PR_KIT"].ToString();
                tableRow.Cells.Add(kitCell);

                TableCell WhNumberCell = new TableCell();
                if (row.Table.Columns.Contains("PR_WH_NUM"))
                    WhNumberCell.Text = row["PR_WH_NUM"].ToString();
                tableRow.Cells.Add(WhNumberCell);

                TableCell WhStatusCell = new TableCell();
                if (row.Table.Columns.Contains("PROD_STATUS_DEF"))
                    WhStatusCell.Text = row["PROD_STATUS_DEF"].ToString();
                tableRow.Cells.Add(WhStatusCell);

                TableCell NotesCell = new TableCell();
                if (row.Table.Columns.Contains("PR_NOTES"))
                    NotesCell.Text = row["PR_NOTES"].ToString();
                tableRow.Cells.Add(NotesCell);

                TableCell FormatCell = new TableCell();
                if (row["PROD_TYPE"].ToString().ToUpper() == "E")
                    FormatCell.Text = strElectronic;
                else
                    FormatCell.Text = strHardcopy;
                tableRow.Cells.Add(FormatCell);

                tblProducts.Rows.Add(tableRow);

            }//for each product

            txtProductLines.Text = count.ToString();
            txtTotalQTY.Text = totalQTY.ToString();
            txtTotalPrice.Text = totalPrice.ToString();

        }//loadProducts

        private void disableFields(string action)
        {
            if (action.Equals("edit") || action.Equals("new"))
            {
                txtInqNumber.Enabled = false;
                btnFirst.Enabled = false;
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
                btnNew.Enabled = false;
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
                btnFind.Enabled = false;
                btnOrderStatus.Enabled = false;
                btnReports.Enabled = false;

            }// if edit or new

            if (action.Equals("cancel") || action.Equals("save"))
            {
                txtAccount.Enabled = false;
                txtAddress.Enabled = false;
                txtAddress2.Enabled = false;
                txtAnswer.Enabled = false;
                txtBackOrder.Enabled = false;
                txtCity.Enabled = false;
                txtCountry.Enabled = false;
                txtDelivered.Enabled = false;
                txtEditDate.Enabled = false;
                txtEmail.Enabled = false;
                txtFax.Enabled = false;
                txtFirst.Enabled = false;
                txtInputDate.Enabled = false;
                txtIssues.Enabled = false;
                txtLanguage.Enabled = false;
                txtLast.Enabled = false;
                txtOrganization.Enabled = false;
                txtPhone.Enabled = false;
                txtPostZip.Enabled = false;
                txtProductLines.Enabled = false;
                txtTime.Enabled = false;
                txtTotalPrice.Enabled = false;
                txtTotalQTY.Enabled = false;
                ddlClass.Enabled = false;
                ddlCust.Enabled = false;
                ddlDelivery.Enabled = false;
                ddlLine.Enabled = false;
                ddlOrderStatus.Enabled = false;
                ddlProvState.Enabled = false;
                ddlProvState2.Enabled = false;
                ddlSource.Enabled = false;
                rblCommunication.Enabled = false;
                rblGender.Enabled = false;
                rblLanguage.Enabled = false;
                btnAddItem.Enabled = false;
                btnCancel.Enabled = false;
                btnSave.Enabled = false;
                btnShipNotes.Enabled = false;
                btnSubject.Enabled = false;
                btnQA.Enabled = false;
                imgbtnCalendar.Enabled = false;
                imgbtnCal2.Enabled = false;
                imgbtnCal3.Enabled = false;
                txtResearchTime.Enabled = false;
                txtResponseDate.Enabled = false;
                txtResponseTime.Enabled = false;
                txtIssuesKB.Enabled = false;
                txtAnswerKB.Enabled = false;
                ddlOrderKB.Enabled = false;
                rblResearch.Enabled = false;
                txtTimeToKB.Enabled = false;
                chkUsage.Enabled = false;
                btnAddToOrder.Enabled = false;
                txtIssuesZ.Enabled = false;
                txtAnswerZ.Enabled = false;
                txtCurrentComment.Enabled = false;
                imgbtnZoomAccount.Enabled = false;
                imgbtnZoomLast.Enabled = false;
                imgbtnZoomOrganization.Enabled = false;
                imgbtnZoomPost.Enabled = false;

            } // if cancel or save

        }//disableFields

        private void enableFields(string action)
        {
            if (action.Equals("cancel") || action.Equals("save"))
            {
                txtInqNumber.Enabled = true;
                btnFirst.Enabled = true;
                btnLast.Enabled = true;
                btnNext.Enabled = true;
                btnPrev.Enabled = true;
                btnNew.Enabled = true;
                btnDelete.Enabled = true;
                btnEdit.Enabled = true;
                btnFind.Enabled = true;
                btnOrderStatus.Enabled = true;
                btnReports.Enabled = true;

                btnAddElectronic.Enabled = false;
                // btnSendEmail.Enabled = false;
                btnSendEmail.Enabled = true;

            }// if cancel or save

            if (action.Equals("edit") || action.Equals("new"))
            {
                txtAddress.Enabled = true;
                txtAddress2.Enabled = true;
                txtAnswer.Enabled = true;
                txtCity.Enabled = true;
                txtLanguage.Enabled = true;
                txtCountry.Enabled = true;
                txtEmail.Enabled = true;
                txtFax.Enabled = true;
                txtFirst.Enabled = true;
                txtIssues.Enabled = true;
                txtLast.Enabled = true;
                txtOrganization.Enabled = true;
                txtPhone.Enabled = true;
                txtPostZip.Enabled = true;
                txtTime.Enabled = true;
                ddlClass.Enabled = true;
                ddlCust.Enabled = true;
                ddlDelivery.Enabled = true;
                ddlLine.Enabled = true;
                ddlOrderStatus.Enabled = true;
                ddlProvState.Enabled = true;
                ddlProvState2.Enabled = true;
                ddlSource.Enabled = true;
                rblCommunication.Enabled = true;
                rblGender.Enabled = true;
                rblLanguage.Enabled = true;
                btnAddItem.Enabled = true;
                btnAddElectronic.Enabled = true;
                btnSendEmail.Enabled = true;
                btnCancel.Enabled = true;
                btnSave.Enabled = true;
                btnShipNotes.Enabled = true;
                btnSubject.Enabled = true;
                btnQA.Enabled = true;
                imgbtnCalendar.Enabled = true;
                txtIssuesKB.Enabled = true;
                txtAnswerKB.Enabled = true;
                txtTimeToKB.Enabled = true;
                ddlOrderKB.Enabled = true;
                txtResponseTime.Enabled = true;
                txtResearchTime.Enabled = true;
                rblResearch.Enabled = true;
                imgbtnCal2.Enabled = true;
                imgbtnCal3.Enabled = true;
                chkUsage.Enabled = true;
                btnAddToOrder.Enabled = true;
                txtIssuesZ.Enabled = true;
                txtAnswerZ.Enabled = true;
                txtCurrentComment.Enabled = true;
                imgbtnZoomAccount.Enabled = true;
                imgbtnZoomLast.Enabled = true;
                imgbtnZoomOrganization.Enabled = true;
                imgbtnZoomPost.Enabled = true;
                txtOther.Enabled = true;

                if (action.Equals("edit"))
                {
                    for (int i = 1; i < tblProducts.Rows.Count; ++i)
                    {
                        TextBox txtQuantity = (TextBox)tblProducts.Rows[i].FindControl("txtQuantity" + i);
                        txtQuantity.Enabled = true;
                    }

                }//if edit

            } // if edit or new

            enableMailButtons();

        }//enableFields

        private void enableMailButtons()
        {
            InboundDB db = new InboundDB();
            DataSet ds;
            bool boolShowMailButtons;

            boolShowMailButtons = false;
            ds = db.GetControlValue("INBND_EMAIL");

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CNTR_VALUE"] != null)
                {
                    if (ds.Tables[0].Rows[0]["CNTR_VALUE"].ToString().ToUpper() == "YES" || ds.Tables[0].Rows[0]["CNTR_VALUE"].ToString().ToUpper() == "TRUE")
                    {
                        boolShowMailButtons = true;
                    }
                }

            }
            if (boolShowMailButtons == true)
            {
                btnAddElectronic.Visible = true;
                btnSendEmail.Visible = true;
            }
            else
            {
                btnAddElectronic.Visible = false;
                btnSendEmail.Visible = false;

            }

        }

        private void clearFields()
        {
            rblCommunication.ClearSelection();
            rblGender.ClearSelection();
            rblLanguage.ClearSelection();
            ddlClass.ClearSelection();
            ddlCust.ClearSelection();
            ddlDelivery.ClearSelection();
            ddlLine.ClearSelection();
            ddlOrderStatus.ClearSelection();
            ddlProvState.ClearSelection();
            ddlProvState2.ClearSelection();
            ddlSource.ClearSelection();
            rblResearch.ClearSelection();
            txtAccount.Text = "";
            txtAddress.Text = "";
            txtAddress2.Text = "";
            txtAnswer.Text = "";
            txtBackOrder.Text = "";
            txtCity.Text = "";
            txtCountry.Text = "";
            txtDate.Text = "";
            txtDelivered.Text = "";
            txtEditAgent.Text = "";
            txtEditDate.Text = "";
            txtEmail.Text = "";
            txtFax.Text = "";
            txtFirst.Text = "";
            txtInitialAgent.Text = "";
            txtInputDate.Text = "";
            txtIssues.Text = "";
            txtLanguage.Text = "";
            txtLast.Text = "";
            txtOrganization.Text = "";
            txtOwner.Text = "";
            txtPhone.Text = "";
            txtPostZip.Text = "";
            txtProductLines.Text = "";
            //txtReady.Text = "";
            txtTime.Text = "";
            txtTotalPrice.Text = "";
            txtTotalQTY.Text = "";
            txtResponseDate.Text = "";
            txtResponseTime.Text = "";
            txtResearchTime.Text = "";
            txtShipNotes.Text = "";
            txtShipDetails.Text = "";
            txtOther.Text = "";
            txtOtherSubject.Text = "";

            while (tblProducts.Rows.Count > 1) // just keep the header row
            {
                tblProducts.Rows.RemoveAt(1);
            }

        }//clearFields

        private void SaveEnquiry()
        {
            InboundDB db = new InboundDB();
            setConstants();
            string ticketNumber = txtInqNumber.Text;
            string openRegion = txtOther.Text;
            string otherLang = null;
            string incLine = ddlLine.SelectedValue.Split('|').ElementAt(0).ToString();
            string orderStatus = ddlOrderStatus.SelectedValue;
            string englishFlag = "0";
            string frenchFlag = "0";
            string otherFlag = "0";
            string phoneFlag = "0";
            string personFlag = "0";
            string mailFlag = "0";
            string faxFlag = "0";
            string exhibitFlag = "0";
            string emailFlag = "0";
            string webFlag = "0";
            string subject = null; //subject code from subject page
            string refTo = null;
            string adviceTo = null;
            string corpTo = null;
            string adviceFlag = "0";
            string corpFlag = "0";
            string referralFlag = "0";
            string openSubject = null; //subject text from subject page
            string voiceMailFlag = "0";
            string gender;
            string issues = null;
            string answer = null;
            string province = ddlProvState.SelectedValue;
            string opNO = null;
            string custType = ddlCust.SelectedValue;
            string s_operator = currentUser;
            string research10 = null;
            string researchDuration = null;
            string researchExtra = null; //not used
            string clRec = "0";
            string owner = userGroup;
            string originalOwner = owner;
            string complaint = "0"; //not used
            string srcDef = null;
            string classification = ddlClass.SelectedValue;
            string productFlag = "0";
            string source = null; //not used
            string toneStart = null; //not used
            string toneEnd = null; //not used
            string dateToCall = null;
            string dateBack = null;
            int c;
            string s = "";

            if (Session["KBKeys"] != null && !string.IsNullOrEmpty(Session["KBKeys"].ToString()))
            {
                string[] tempKB = Session["KBKeys"].ToString().Split('|');
                foreach (string key in tempKB)
                {
                    if (!string.IsNullOrEmpty(key.Trim()))
                    {
                        DataRow row = db.getKnowledgeDetails(key, pageLang).Tables[0].Rows[0];
                        if (row["ref_referral"] != null && row["ref_referral"].ToString().Equals("YES"))
                        {
                            referralFlag = "1";
                            if (string.IsNullOrEmpty(refTo))
                                refTo = "|" + key + "|";
                            else
                                refTo += "|" + key + "|";

                        }//if the KB record is a referral/refTo record

                        else if (row["ref_qa"] != null && row["ref_qa"].ToString().Equals("YES"))
                        {
                            adviceFlag = "1";
                            if (string.IsNullOrEmpty(adviceTo))
                                adviceTo = "|" + key + "|";
                            else
                                adviceTo += "|" + key + "|";

                        }//if the KB record is a qa/adviceTo record

                        else if (row["ref_prog_info"] != null && row["ref_prog_info"].ToString().Equals("YES"))
                        {
                            corpFlag = "1";
                            if (string.IsNullOrEmpty(corpTo))
                                corpTo = "|" + key + "|";
                            else
                                corpTo += "|" + key + "|";

                        }//if the KB record is a proginfo/corpTo record

                    }// if the key is not null or empty

                }//foreach key in tempKB

            }// if there has been KB records selected

            if (Session["SubCodes"] != null && !string.IsNullOrEmpty(Session["SubCodes"].ToString().Trim()))
                subject = Session["SubCodes"].ToString();

            if (!string.IsNullOrEmpty(txtOtherSubject.Text))
                openSubject = txtOtherSubject.Text;

            if (!string.IsNullOrEmpty(txtIssues.Text)){
                if (txtIssues.Text.Length > 4000)
                {
                    issues = txtIssues.Text.Substring(0, 4000);
                }
                else
                {
                    issues = txtIssues.Text;
                }
            }

            if (!string.IsNullOrEmpty(txtAnswer.Text))
            {
                if (txtAnswer.Text.Length > 4000)
                {
                    answer = txtAnswer.Text.Substring(0, 4000);
                }
                else
                {
                    answer = txtAnswer.Text;
                }
            }

            if (ddlSource.SelectedIndex != 0)
            {
                srcDef = ddlSource.SelectedItem.Text.Replace("'","''");
                opNO = ddlSource.SelectedValue;
            }

            if (!string.IsNullOrEmpty(txtDate.Text) && !string.IsNullOrEmpty(txtTime.Text))
                dateToCall = txtDate.Text + " " + txtTime.Text;

            if (!string.IsNullOrEmpty(txtResponseDate.Text) && !string.IsNullOrEmpty(txtResponseTime.Text))
                dateBack = txtResponseDate.Text + " " + txtResponseTime.Text;

            if (rblLanguage.SelectedIndex == 0)
                englishFlag = "1";
            else if (rblLanguage.SelectedIndex == 1)
                frenchFlag = "1";
            else
            {
                otherFlag = "1";
                otherLang = txtLanguage.Text;
            }

            if (rblCommunication.SelectedIndex == 0)
                phoneFlag = "1";
            else if (rblCommunication.SelectedIndex == 4)
                voiceMailFlag = "1";
            else if (rblCommunication.SelectedIndex == 1)
                personFlag = "1";
            else if (rblCommunication.SelectedIndex == 5)
                emailFlag = "1";
            else if (rblCommunication.SelectedIndex == 2)
                mailFlag = "1";
            else if (rblCommunication.SelectedIndex == 6)
                exhibitFlag = "1";
            else if (rblCommunication.SelectedIndex == 3)
                faxFlag = "1";
            else
                webFlag = "1";

            if (rblGender.SelectedIndex == 0)
                gender = "1";
            else if (rblGender.SelectedIndex == 1)
                gender = "0";
            else
                gender = "2";

            if (rblResearch.SelectedIndex != -1)
                research10 = rblResearch.SelectedValue;
            if (!string.IsNullOrEmpty(txtResearchTime.Text))
                researchDuration = txtResearchTime.Text;

            if (tblProducts.Rows.Count > 1)
                productFlag = "1";

            if (!string.IsNullOrEmpty(txtAccount.Text))
                clRec = txtAccount.Text;

            int returnVal = 0;

            if (Session["Action"] != null && Session["Action"].ToString().Equals("New"))
                returnVal = db.InsertNewEnquiry(ticketNumber, openRegion, otherLang, incLine, englishFlag, frenchFlag, otherFlag,
                  phoneFlag, personFlag, mailFlag, faxFlag, exhibitFlag, emailFlag, webFlag, subject, refTo,
                  adviceTo, corpTo, adviceFlag, corpFlag, referralFlag, openSubject, voiceMailFlag, gender,
                  issues, answer, province, opNO, custType, s_operator, research10, researchDuration, researchExtra, clRec,
                      userGroup, owner, originalOwner, complaint, srcDef, classification, productFlag, source, orderStatus, toneStart, toneEnd, dateToCall, dateBack);
            else

                s = "";
                if (!string.IsNullOrEmpty(issues))
                {
                    for (c = 0; c < issues.Length; c++)
                    {
                        s = s + char.ConvertToUtf32(issues, c).ToString() + "_";
                    }
                }
                returnVal = db.UpdateEnquiry(ticketNumber, openRegion, otherLang, incLine, englishFlag, frenchFlag, otherFlag,
                  phoneFlag, personFlag, mailFlag, faxFlag, exhibitFlag, emailFlag, webFlag, subject, refTo,
                  adviceTo, corpTo, adviceFlag, corpFlag, referralFlag, openSubject, voiceMailFlag, gender,
                  issues, answer, province, opNO, custType, s_operator, research10, researchDuration, researchExtra, clRec,
                  complaint, srcDef, classification, productFlag, source, orderStatus, toneStart, toneEnd, dateToCall, dateBack);

            if (returnVal != 1)
            {
                //didn't work
            }
            else
                Session["TicketNumber"] = txtInqNumber.Text;

        }//SaveNewEnquiry

        private void SaveCustomerDetails()
        {
            InboundDB db = new InboundDB();

            string ticketNumber = txtInqNumber.Text;
            string fName = txtFirst.Text;
            string lName = txtLast.Text;
            string company = txtOrganization.Text;
            string street = txtAddress.Text;
            string street2 = txtAddress2.Text;
            string city = txtCity.Text;
            string prov = ddlProvState2.SelectedValue;
            string country = txtCountry.Text;
            string phone = txtPhone.Text;
            string postalCode = txtPostZip.Text;
            string email = txtEmail.Text;
            string fax = txtFax.Text;
            string readyToShip = null;
            string delivery = ddlDelivery.SelectedValue;
            string shipNotes = txtShipNotes.Text;
            string shipDetails = txtShipDetails.Text;
            string backOrder = "NO";
            string actNum = "0";

            if (!string.IsNullOrEmpty(txtBackOrder.Text))
                backOrder = txtBackOrder.Text;

            if (!string.IsNullOrEmpty(txtAccount.Text))
                actNum = txtAccount.Text;

            if (tblProducts.Rows.Count > 1) //more than 1 because of the header row
                readyToShip = "YES";
            else
                readyToShip = "NO";

            int returnVal = 0;
            if (Session["Action"] != null && Session["Action"].ToString().Equals("New"))
                returnVal = db.InsertCustDetails(ticketNumber, actNum, fName, lName, company, street, street2, city, prov, country,
                 phone, postalCode, email, fax, readyToShip, backOrder, delivery, shipNotes, shipDetails);

            else
                returnVal = db.UpdateCustDetails(ticketNumber, actNum, fName, lName, company, street, street2, city, prov, country,
             phone, postalCode, email, fax, delivery, shipNotes, shipDetails);

            //if (returnVal != 1)
            //{
            //    //didn't work
            //}

        }//SaveCustomerDetails

        private void SaveProduct(DataRow row)
        {
            setConstants();

            InboundDB db = new InboundDB();
            string ticketNumber = txtInqNumber.Text;
            string inventoryCode = row["PR_INVENTORY_CODE"].ToString();
            string quantity = row["PR_QUANTITY"].ToString();
            DataRow productInfo = db.GetInventoryItem(pageLang, inventoryCode, null).Tables[0].Rows[0];

            string title = productInfo["INVENTORY_DEF"].ToString();
            string inventoryStatus = productInfo["INVENTORY_STATUS"].ToString();
            string unitCost = productInfo["UNIT_COST"].ToString();
            string unitPrice = productInfo["UNIT_PRICE"].ToString();
            string subTotal = row["PR_SUBTOTAL"].ToString();
            string gst = row["PR_GST"].ToString();
            string pst = row["PR_PST"].ToString();
            string total = row["PR_TOTAL"].ToString();
            string totalCost = row["PR_TOTAL_COST"].ToString();
            string stockCheck = productInfo["INVENTORY_STOCK_CHECK"].ToString();
            string size = productInfo["INVENTORY_SIZE"].ToString();
            string um = productInfo["INVENTORY_UM"].ToString();
            string prodType = productInfo["INVENTORY_TYPE"].ToString();
            string prodCat = productInfo["INVENTORY_PROD_CAT_DEPT"].ToString();
            // string prodAccount = productInfo["INVENTORY_ACCOUNT_NO"].ToString();
            string poNum = productInfo["INVENTORY_PO_NUM"].ToString();
            string poNumPrime = productInfo["INVENTORY_PO_NUM"].ToString();
            string primeUnitCost = productInfo["UNIT_COST"].ToString();
            string supplier = productInfo["INVENTORY_SUPPLIER"].ToString();
            string primeSupplier = productInfo["INVENTORY_SUPPLIER"].ToString();
            string kit = productInfo["INVENTORY_KIT"].ToString();
            string supPartNo = productInfo["INVENTORY_SUP_PART_NO"].ToString();
            string cRecNo = "0";
            string deliveryMode = ddlDelivery.SelectedValue;
            string whNum = row["PR_WH_NUM"].ToString();
            string extraTotal = row["PR_QUANTITY"].ToString();
            int processStatus = 1;

            if (txtBackOrder.Text.Equals("YES"))
                processStatus = 2;

            if (!string.IsNullOrEmpty(txtAccount.Text))
                cRecNo = txtAccount.Text;

            int returnVal = 0;

            returnVal = db.InsertProductRecord(ticketNumber, currentUser, inventoryCode, quantity, title, inventoryStatus, unitCost, unitPrice, subTotal, gst, pst, total, totalCost,
                stockCheck, size, um, prodType, prodCat, poNum, poNumPrime, primeUnitCost, supplier, primeSupplier, kit, supPartNo, cRecNo, deliveryMode, whNum, extraTotal);

            returnVal = db.InsertProductOrder(inventoryCode, ticketNumber, quantity, whNum, deliveryMode, processStatus);

        }//SaveProduct

        private void SaveElectronicProduct(DataRow row)
        {
            setConstants();

            InboundDB db = new InboundDB();
            string ticketNumber = txtInqNumber.Text;
            string inventoryCode = row["PR_INVENTORY_CODE"].ToString();
            string quantity = row["PR_QUANTITY"].ToString();
            DataRow productInfo = db.GetInventoryItem(pageLang, inventoryCode, null).Tables[0].Rows[0];

            string title = productInfo["INVENTORY_DEF"].ToString();
            string inventoryStatus = productInfo["INVENTORY_STATUS"].ToString();
            string unitCost = productInfo["UNIT_COST"].ToString();
            string unitPrice = productInfo["UNIT_PRICE"].ToString();
            string subTotal = row["PR_SUBTOTAL"].ToString();
            string gst = row["PR_GST"].ToString();
            string pst = row["PR_PST"].ToString();
            string total = row["PR_TOTAL"].ToString();
            string totalCost = row["PR_TOTAL_COST"].ToString();
            string stockCheck = productInfo["INVENTORY_STOCK_CHECK"].ToString();
            string size = productInfo["INVENTORY_SIZE"].ToString();
            string um = productInfo["INVENTORY_UM"].ToString();
            string prodType = productInfo["INVENTORY_TYPE"].ToString();
            string prodCat = productInfo["INVENTORY_PROD_CAT_DEPT"].ToString();
            // string prodAccount = productInfo["INVENTORY_ACCOUNT_NO"].ToString();
            string poNum = productInfo["INVENTORY_PO_NUM"].ToString();
            string poNumPrime = productInfo["INVENTORY_PO_NUM"].ToString();
            string primeUnitCost = productInfo["UNIT_COST"].ToString();
            string supplier = productInfo["INVENTORY_SUPPLIER"].ToString();
            string primeSupplier = productInfo["INVENTORY_SUPPLIER"].ToString();
            string kit = productInfo["INVENTORY_KIT"].ToString();
            string supPartNo = productInfo["INVENTORY_SUP_PART_NO"].ToString();
            string cRecNo = "0";
            string deliveryMode = ddlDelivery.SelectedValue;
            string whNum = row["PR_WH_NUM"].ToString();
            string extraTotal = row["PR_QUANTITY"].ToString();
            int returnVal = 0;

            returnVal = db.InsertProductRecordElectronic(ticketNumber, currentUser, inventoryCode, quantity, title, inventoryStatus, unitCost, unitPrice, subTotal, gst, pst, total, totalCost,
                stockCheck, size, um, prodType, prodCat, poNum, poNumPrime, primeUnitCost, supplier, primeSupplier, kit, supPartNo, cRecNo, deliveryMode, whNum, extraTotal);


        }//SaveProduct
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

        private string setProcStatus(int procStatus)
        {
            LanguageDB ldb = new LanguageDB();

            string status = "UNKNOWN";

            if (procStatus == 0)
                status = ldb.GetLabel("InboundTracking", "btnNew", pageLang);

            else if (procStatus == 1)
                status = ldb.GetLabel("InboundTracking", "Label", pageLang);

            else if (procStatus == 2)
                status = ldb.GetLabel("InboundTracking", "Receipt2", pageLang);

            else if (procStatus == 3)
                status = ldb.GetLabel("InboundTracking", "Done", pageLang);

            else if (procStatus == 4)
                status = ldb.GetLabel("InboundTracking", "Hold", pageLang);

            else if (procStatus == 5)
                status = ldb.GetLabel("InboundTracking", "SentCP", pageLang);

            return status;

        }//setProcStatus

        protected void btnNew_Click(object sender, EventArgs e)
        {
            setConstants();
            Session["KBKeys"] = null;
            Session["SubCodes"] = null;
            InboundDB db = new InboundDB();
            ddlLine.Items.Clear();
            DataSet incomDS = db.GetIncomingLinesByGroup(pageLang, userLevel, userAccess);
            loadIncLines(incomDS);
            Session["Action"] = "New";
            disableFields("new");
            enableFields("new");
            clearFields();
            btnQA.Text = btnQA.Text.Replace(checkMarkText.InnerText, "");
            btnSubject.Text = btnSubject.Text.Replace(checkMarkText.InnerText, "");
            ddlDelivery.SelectedValue = "DM";
            ddlOrderStatus.SelectedValue = "020";
            int newRecNum = db.GenerateNewTicketNumber();
            txtInqNumber.Text = newRecNum.ToString();

        }//btnNew_Click

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            setConstants();
            InboundDB db = new InboundDB();
            DataSet incomDS = db.GetIncomingLinesByGroup(pageLang, userLevel, userAccess);
            bool canEdit = false;
            ListItem selItem = ddlLine.SelectedItem;
            if (incomDS.Tables[0].Rows.Count > 0)
            {
                DataRowCollection incomRows = incomDS.Tables[0].Rows;
                foreach (DataRow row in incomRows)
                {
                    if (row["TELE_DESC"].ToString().Equals(selItem.Text))
                    {
                        canEdit = true;
                        break;
                    }

                }//for each row returned

            }// if incoming lines are returned

            ddlLine.Items.Clear();
            loadIncLines(incomDS);
            ddlLine.SelectedValue = selItem.Value;

            if (canEdit)
            {
                disableFields("edit");
                enableFields("edit");
                Session["Action"] = "Edit";
            }
            else
            {
                LanguageDB langDB = new LanguageDB();
                string alertMessage = langDB.GetLabel("InboundTracking", "SufficientEditRights", pageLang);
                ScriptManager.RegisterStartupScript(this, GetType(), "editAlert", "alert('" + alertMessage + "');", true);
            }

        }//btnEdit_Click

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            disableFields("cancel");
            enableFields("cancel");

            if (Session["Action"] != null)
            {
                if (Session["Action"].ToString().Equals("New"))
                {
                    setConstants();
                    InboundDB db = new InboundDB();
                    ddlLine.Items.Clear();
                    DataSet incomDS = db.GetIncomingLinesByGroup(pageLang, 0, userAccess); //0 for the user level so that you see all lines
                    loadIncLines(incomDS);

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            bool customerValid = true;
            setConstants();
            LanguageDB langDB = new LanguageDB();

            if (!string.IsNullOrEmpty(txtDate.Text))
            {
                if (string.IsNullOrEmpty(txtTime.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "enterTimeAlert", "alert('" + langDB.GetLabel("InboundTracking", "MsgEnterTime", pageLang) + "');", true);
                    return;
                }
                else
                {
                    Regex digitsOnly = new Regex(@"[^\d]");
                    string time = digitsOnly.Replace(txtTime.Text, "");
                    if (time.Length != 4)
                    {
                        string message = langDB.GetLabel("InboundTracking", "MsgTimeFormat", pageLang) + "\\nEx: 10:30";
                        ScriptManager.RegisterStartupScript(this, GetType(), "24hrAlert", "alert('" + message + "');", true);
                        return;
                    }

                }//if something is entered in the callback time field

            }// if a callback date has been entered

            if (IsValid)
            {
                if (ddlProvState2.SelectedIndex == 0)
                {
                    ddlProvState2.SelectedValue = ddlProvState.SelectedValue;
                }

                if ((tblProducts.Rows.Count > 1) && (Session["TempProducts"] != null))
                {
                    if (string.IsNullOrEmpty(txtCity.Text) || string.IsNullOrEmpty(txtAddress.Text) || (string.IsNullOrEmpty(txtLast.Text) && string.IsNullOrEmpty(txtOrganization.Text)))
                        customerValid = false;
                    if ((!ddlProvState2.SelectedValue.Equals("XX") && !ddlProvState2.SelectedValue.Equals("US")) || txtCountry.Text.ToUpper().Equals("CANADA"))
                    {
                        if (string.IsNullOrEmpty(txtPostZip.Text))
                            customerValid = false;
                    }//if the provstate ddl is a canadian prov code or the country box contains Canada
                }
                if (customerValid)
                {
                    InboundDB db = new InboundDB();
                    setConstants();
                    if (Session["Action"] != null && Session["Action"].ToString().Equals("Edit"))
                    {

                        string ticketNum = txtInqNumber.Text;
                        for (int i = 1; i < tblProducts.Rows.Count; ++i)
                        {
                            string productCode = tblProducts.Rows[i].Cells[1].Text;
                            TextBox txtQuant = (TextBox)tblProducts.Rows[i].Cells[3].FindControl("txtQuantity" + i);
                            db.UpdateProductDetails(productCode, ticketNum, txtQuant.Text);
                            db.UpdateProductOrder(productCode, ticketNum, txtQuant.Text);
                        }

                        if (Session["OrderStatus"] != null && Session["OrderStatusDate"] != null)
                        {
                            string orderStatus = Session["OrderStatus"].ToString();
                            if (!orderStatus.Equals(ddlOrderStatus.SelectedValue))
                            {
                                string user;
                                if (!string.IsNullOrEmpty(txtEditAgent.Text))
                                    user = txtEditAgent.Text;
                                else
                                    user = txtInitialAgent.Text;
                                db.InsertOrderStatusHistory(txtInqNumber.Text, orderStatus, user);
                                db.UpdateOrderStatusDate(txtInqNumber.Text);
                            }

                        }//if there is already an orderstatus

                    }//if it's an edit

                    else
                    {
                        db.InsertOrderStatusHistory(txtInqNumber.Text, ddlOrderStatus.SelectedValue, currentUser);
                        Session["NewEnq"] = "YES";
                    }

                    SaveCustomerDetails();

                    if (!string.IsNullOrEmpty(txtAnswer.Text))
                    {
                        if (txtAnswer.Text.Length > 4000)
                            Session["AnswerToLong"] = true;
                    }

                    if (!string.IsNullOrEmpty(txtIssues.Text))
                    {
                        if (txtIssues.Text.Length > 4000)
                            Session["IssuesToLong"] = true;
                    }

                    SaveEnquiry();
                    if (Session["TempProducts"] != null)
                    {
                        DataTable tempProducts = (DataTable)Session["TempProducts"];
                        foreach (DataRow row in tempProducts.Rows)
                            SaveProduct(row);
                    }

                    if (Session["TempEProducts"] != null)
                    {
                        DataTable tempEProducts = (DataTable)Session["TempEProducts"];
                        foreach (DataRow row in tempEProducts.Rows)
                            SaveElectronicProduct(row);
                    }

                    disableFields("save");
                    enableFields("save");
                    Session["TempProducts"] = null;
                    Session["TempEProducts"] = null;
                    Session["Action"] = null;
                    Response.Redirect(Request.RawUrl);

                }//if the customervalidation passes (automatically passes if there are no products)
                else
                {
                    string validationMessage = langDB.GetLabel("InboundTracking", "MsgMissingCustomerFields", pageLang).Replace("\n", "\\n") + "\\n" +
                        langDB.GetLabel("InboundTracking", "SurnameOrOrganization", pageLang) + "\\n" + langDB.GetLabel("InboundTracking", "1stAddress", pageLang) +
                        "\\n" + langDB.GetLabel("InboundTracking", "City", pageLang) + "\\n" + langDB.GetLabel("InboundTracking", "IfInCanada", pageLang);
                    ScriptManager.RegisterStartupScript(this, GetType(), "enterFieldsAlert", "alert('" + validationMessage + "');", true);
                }
            }//if the enq validation passes
            else
                ScriptManager.RegisterStartupScript(this, GetType(), "requiredFieldsAlert", "alert('" + langDB.GetLabel("InboundTracking", "MsgRequiredFields", pageLang) + "');", true);

        }//btnSave_Click

        private void updateOrderStatus()
        {
            setConstants();
            Session["NewEnq"] = null;
            orderDetailsDiv.Visible = false;
            shippingDiv.Visible = false;
            confirmStatusDiv.Visible = true;
            LanguageDB db = new LanguageDB();
            lblConfirmStatus.Text = db.GetLabel("InboundTracking", "TicketStatusConfimation", pageLang);
            lblCurrentStatusMessage.Text = db.GetLabel("InboundTracking", "CurrentStatusForTicket", pageLang).Replace("[ticketnumber]", txtInqNumber.Text) + ": ";
            lblCurStatus.Text = ddlOrderStatus.SelectedItem.Text;
            rblFinalStatus.Items.Clear();
            if (ddlOrderStatus.SelectedValue.Equals("020") || ddlOrderStatus.SelectedValue.Equals("099"))
            {
                lblSelStatusMessage.Text = db.GetLabel("InboundTracking", "SelectTicketStatus", pageLang);
                if (tblProducts.Rows.Count > 1) //includes header row
                    rblFinalStatus.Items.Add(new ListItem(db.GetLabel("InboundTracking", "SaveAsReadyForShipping", pageLang), "010"));
                else
                    rblFinalStatus.Items.Add(new ListItem(db.GetLabel("InboundTracking", "SaveAsDone", pageLang), "099"));
                rblFinalStatus.Items.Add(new ListItem(db.GetLabel("InboundTracking", "SaveAsIncomplete", pageLang), "020"));
                rblFinalStatus.SelectedIndex = 0;
                btnSaveStatus.Text = db.GetLabel("InboundTracking", "btnOkSave", pageLang);
            }
            else
                btnSaveStatus.Text = db.GetLabel("ProductCatalogue", "Return", pageLang);

        }//updateOrderStatus

        protected void imgbtnCalendar_Click(object sender, ImageClickEventArgs e)
        {
            if (calCallbackDate.Visible)
                calCallbackDate.Visible = false;
            else
                calCallbackDate.Visible = true;

        }//imgbtnCalendar_Click

        protected void calCallbackDate_SelectionChanged(object sender, EventArgs e)
        {
            txtDate.Text = Utility.getDate(calCallbackDate.SelectedDate.ToString("dd-MMM-yyyy"));
            if (txtTime.Text.Length < 1)
            {
                txtTime.Text = "09:00";
            }
            calCallbackDate.Visible = false;

        }//calCallbackDate_SelectionChanged

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            setConstants();
            InboundDB db = new InboundDB();
            DataSet incomDS = db.GetIncomingLinesByGroup(pageLang, userLevel, userAccess);
            bool canDelete = false;
            ListItem selItem = ddlLine.SelectedItem;
            if (incomDS.Tables[0].Rows.Count > 0)
            {
                DataRowCollection incomRows = incomDS.Tables[0].Rows;
                foreach (DataRow row in incomRows)
                {
                    if (row["TELE_DESC"].ToString().Equals(selItem.Text))
                    {
                        canDelete = true;
                        break;
                    }

                }//for each row returned

            }// if incoming lines are returned

            if (canDelete)
            {
                db.DeleteCustDetails(txtInqNumber.Text);
                db.DeleteEnquiry(txtInqNumber.Text);
                Session["TicketNumber"] = db.GetMaxRecordNumber(createIncLineVar());
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                LanguageDB langDB = new LanguageDB();
                string alertMessage = langDB.GetLabel("InboundTracking", "Sufficientrights", pageLang);
                ScriptManager.RegisterStartupScript(this, GetType(), "deleteAlert", "alert('" + alertMessage + "');", true);
            }

        }//btnDelete_Click

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            setConstants();
            clearFields();
            InboundDB db = new InboundDB();
            string ticketNumber = db.GetFirstRecordNumber(createIncLineVar()).ToString();
            txtInqNumber.Text = ticketNumber;
            Session["TicketNumber"] = ticketNumber;
            //loadEnquiry(db.GetInboundDetails(ticketNumber, createIncLineVar()));
            //loadCustomerDetails(db.GetCustomerDetails(ticketNumber));
            //loadProducts(db.GetProductDetails(ticketNumber, pageLang));
            Response.Redirect(Request.RawUrl);

        }//btnFirst_Click

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            clearFields();
            setConstants();
            InboundDB db = new InboundDB();
            string currTicketNumber = txtInqNumber.Text;
            string ticketNumber;
            if (currTicketNumber.Equals(db.GetFirstRecordNumber(createIncLineVar()).ToString()))
                ticketNumber = db.GetMaxRecordNumber(createIncLineVar()).ToString();
            else
                ticketNumber = db.GetPreviousRecordNumber(txtInqNumber.Text, createIncLineVar()).ToString();
            txtInqNumber.Text = ticketNumber;
            Session["TicketNumber"] = ticketNumber;
            //loadEnquiry(db.GetInboundDetails(ticketNumber, createIncLineVar()));
            //loadCustomerDetails(db.GetCustomerDetails(ticketNumber));
            //loadProducts(db.GetProductDetails(ticketNumber, pageLang));
            Response.Redirect(Request.RawUrl);

        }//btnPrev_Click

        protected void btnNext_Click(object sender, EventArgs e)
        {
            clearFields();
            setConstants();
            InboundDB db = new InboundDB();
            string currTicketNumber = txtInqNumber.Text;
            string ticketNumber;
            if (currTicketNumber.Equals(db.GetMaxRecordNumber(createIncLineVar()).ToString()))
                ticketNumber = db.GetFirstRecordNumber(createIncLineVar()).ToString();
            else
                ticketNumber = db.GetNextRecordNumber(txtInqNumber.Text, createIncLineVar()).ToString();

            txtInqNumber.Text = ticketNumber;
            Session["TicketNumber"] = ticketNumber;
            //loadEnquiry(db.GetInboundDetails(ticketNumber, createIncLineVar()));
            //loadCustomerDetails(db.GetCustomerDetails(ticketNumber));
            //loadProducts(db.GetProductDetails(ticketNumber, pageLang));
            Response.Redirect(Request.RawUrl);

        }//btnNext_Click

        protected void btnLast_Click(object sender, EventArgs e)
        {
            clearFields();
            setConstants();
            InboundDB db = new InboundDB();
            string ticketNumber = db.GetMaxRecordNumber(createIncLineVar()).ToString();
            txtInqNumber.Text = ticketNumber;
            Session["TicketNumber"] = ticketNumber;
            //loadEnquiry(db.GetInboundDetails(ticketNumber, createIncLineVar()));
            //loadCustomerDetails(db.GetCustomerDetails(ticketNumber));
            //loadProducts(db.GetProductDetails(ticketNumber, pageLang));
            Response.Redirect(Request.RawUrl);

        }//btnLast_Click

        protected void btnInbound_Click(object sender, EventArgs e)
        {
            lblBreadcrumb.Text = btnInbound.Text;
            if (knowledgeSection.Visible || kbDetailsSection.Visible)
            {
                txtAnswer.Text = txtAnswerKB.Text;
                txtIssues.Text = txtIssuesKB.Text;
            }

            inboundDiv.Visible = true;
            productSectionDiv.Visible = false;
            knowledgeSection.Visible = false;
            subjectSection.Visible = false;
            orderStatusSection.Visible = false;
            kbDetailsSection.Visible = false;
            btnClose.Visible = false;

        }//btnInbound_Click

        protected void btnKnowledge_Click(object sender, EventArgs e)
        {
            lblBreadcrumb.Text = btnKnowledge.Text;
            inboundDiv.Visible = false;
            tblKB.Visible = true;
            productSectionDiv.Visible = false;
            knowledgeSection.Visible = true;
            kbDetailsSection.Visible = false;
            subjectSection.Visible = false;
            orderStatusSection.Visible = false;
            kbdTop.Visible = true;
            kbdBottom.Visible = true;
            kbdRef.Visible = true;
            kbdTicket.Visible = false;
            kbdZoomTable.Visible = false;
            txtTicketNumKB.Text = txtInqNumber.Text;
            txtClientInfoKB.Text = txtFirst.Text + " " + txtLast.Text + " " + txtOrganization.Text + " " + ddlProvState2.SelectedValue;
            ddlLineKB.ClearSelection();
            ddlLineKB.Items.Add(ddlLine.SelectedItem);
            btnClose.Visible = true;
            txtIssuesKB.Text = txtIssues.Text;
            txtAnswerKB.Text = txtAnswer.Text;
            ddlOrderKB.Items.Clear();
            txtDateReceived.Text = txtInputDate.Text;
            txtDateToKB.Text = txtDate.Text;
            txtTimeToKB.Text = txtTime.Text;
            foreach (ListItem item in ddlOrderStatus.Items)
            {
                ddlOrderKB.Items.Add(item);
            }
            txtSearchKB.Text = "";
            lblKBCount.Text = "";
            btnShowSelected_Click(sender, e);

        }//btnKnowledge_Click

        private void addWHColumns(DataSet ds)
        {
            DataRowCollection whRows = ds.Tables[0].Rows;
            int placeToAdd = 3;
            string whList = "";
            int i = 0;
            foreach (DataRow whRow in whRows)
            {
                if (whRow.Table.Columns.Contains("WH_NUMBER"))
                {
                    ++i;
                    bool existsFlag = false;
                    BoundField gridColumn = new BoundField();
                    whList += whRow["WH_NUMBER"].ToString() + ", ";
                    gridColumn.HeaderText = "WH " + whRow["WH_NUMBER"].ToString() + " QTY";
                    gridColumn.DataField = "WH" + whRow["WH_NUMBER"].ToString();

                    foreach (DataControlField gridCol in gvInventory.Columns)
                    {
                        if (gridCol.HeaderText.Equals(gridColumn.HeaderText))
                        {
                            existsFlag = true;
                            break;
                        }
                    }//foreach column field in gvInventory

                    if (!existsFlag)
                        gvInventory.Columns.Insert(placeToAdd, gridColumn);
                    ++placeToAdd;

                }//if there is a wh number

            }// for each row returned

            Session["WHCount"] = i;
            Session["WHList"] = whList;

        }//addWHColumns

        protected void btnProduct_Click(object sender, EventArgs e)
        {
            lblBreadcrumb.Text = btnProduct.Text;
            productSectionDiv.Visible = true;
            inboundDiv.Visible = false;
            knowledgeSection.Visible = false;
            kbDetailsSection.Visible = false;
            subjectSection.Visible = false;
            orderStatusSection.Visible = false;
            btnClose.Visible = true;
            txtClientInfo.Text = txtFirst.Text + " " + txtLast.Text + " " + txtOrganization.Text + " " + ddlProvState2.SelectedValue;
            txtTicketNum.Text = txtInqNumber.Text;
            lblDisplayInfo.Text = "";
            lblProductCount.Text = "";
            txtSearch.Text = "";
            //gvInventory.Visible = false;
            gvInventory.Visible = true;
            gvInventoryElectronic.Visible = false;
            gvInventory.DataSourceID = null;
            LanguageDB langDB = new LanguageDB();
            string newProduct = langDB.GetLabel("InboundTracking", "RepNew", pageLang);

            ddlWH.Visible = true;
            btnDisplayAll.Visible = true;
            btnHot.Visible = true;

            Session["NewProdText"] = newProduct;

        }//btnProduct_Click

        protected void btnOrderStatus_Click(object sender, EventArgs e)
        {
            InboundDB db = new InboundDB();
            setConstants();
            DataSet groupDS = db.GetGroups(pageLang);
            foreach (DataRow row in groupDS.Tables[0].Rows)
            {
                ListItem item = new ListItem();
                item.Value = row["own_code"].ToString();
                item.Text = row["group_def"].ToString();
                ddlGroup.Items.Add(item);
            }
            orderStatusSection.Visible = true;
            kbDetailsSection.Visible = false;
            knowledgeSection.Visible = false;
            subjectSection.Visible = false;
            productSectionDiv.Visible = false;
            inboundDiv.Visible = false;
            gvOrderStatus.Visible = false;
            btnClose.Visible = true;
            CommonDB commonDB = new CommonDB();
            DateTime today = DateTime.Parse(commonDB.GetCurrentTime());
            DateTime lastMonth = today.AddMonths(-1);
            txtHighVal.Text = today.ToString("dd-MMM-yyyy");
            txtLowVal.Text = lastMonth.ToString("dd-MMM-yyyy");
            ddlCriteria.SelectedIndex = 1;
            lblOrderStatusRecords.Text = "";
            imgbtnCal4.Visible = true;
            imgbtnCal5.Visible = true;
            txtLowVal.Enabled = false;
            txtHighVal.Enabled = false;

            if (Session["OrderSearchTypeRun"] != null)
            {
                if (Session["OrderSearchCriteria"] != null)
                {
                    ddlCriteria.SelectedIndex = Int32.Parse(Session["OrderSearchCriteria"].ToString());
                    txtHighVal.Text = Session["OrderSearchHighVal"].ToString();
                    txtLowVal.Text = Session["OrderSearchLowVal"].ToString();
                    ddlGroup.SelectedIndex = Int32.Parse(Session["OrderSearchGroup"].ToString());
                    ddlStatus.SelectedIndex = Int32.Parse(Session["OrderSearchStatus"].ToString());
                    txtSelectAllHiddenFlag.Text = "N";
                }
                if (Session["OrderSearchTypeRun"].ToString() == "allincomplete")
                {
                    txtSelectAllHiddenFlag.Text = "Y";
                }
                gvOrderStatus.Visible = true;
                gvOrderStatus.Sort("S_REC_NO", SortDirection.Descending);
            }
        }//btnOrderStatus_Click

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (kbDetailsSection.Visible)
            {
                if (kbdZoomTable.Visible)
                {
                    kbdTop.Visible = true;
                    kbdBottom.Visible = true;
                    kbdRef.Visible = true;
                    kbdTicket.Visible = false;
                    kbdZoomTable.Visible = false;
                }
                else
                {
                    kbDetailsSection.Visible = false;
                    knowledgeSection.Visible = true;
                }

            }//if kbdetails section is visible

            else
            {
                if (knowledgeSection.Visible)
                {
                    if (Session["Action"] != null && (Session["Action"].ToString().Equals("Edit") || Session["Action"].ToString().Equals("New")))
                    {
                        if (!string.IsNullOrEmpty(txtDateToKB.Text) || !string.IsNullOrEmpty(txtResponseDate.Text))
                        {
                            string message = ""; //need to replace the words in this message with a label
                            bool timeBlank = false;
                            bool timeValid = true;
                            if (!string.IsNullOrEmpty(txtDateToKB.Text))
                            {
                                if (string.IsNullOrEmpty(txtTimeToKB.Text))
                                {
                                    message = "You have entered a date to call back, please also enter a time\\n";
                                    timeBlank = true;
                                }

                                else
                                {
                                    Regex digitsOnly = new Regex(@"[^\d]");
                                    string time = digitsOnly.Replace(txtTimeToKB.Text, "");
                                    if (time.Length != 4)
                                    {
                                        message = "Please enter a 4 digit time based on a 24hr clock for callback time\\nEx: 10:30\\n\\n";
                                        timeValid = false;
                                    }

                                }// if the callback time isn't blank

                            }// if the callbackdate isn't blank
                            if (!string.IsNullOrEmpty(txtResponseDate.Text))
                            {
                                if (string.IsNullOrEmpty(txtResponseTime.Text))
                                {
                                    message += "You have entered a response date, please also enter the response time";
                                    timeBlank = true;
                                }
                                else
                                {
                                    Regex digitsOnly = new Regex(@"[^\d]");
                                    string time = digitsOnly.Replace(txtResponseTime.Text, "");
                                    if (time.Length != 4)
                                    {
                                        message += "Please enter a 4 digit time based on a 24hr clock for response time\\nEx: 10:30";
                                        timeValid = false;
                                    }

                                }// if the response time isn't blank

                            }// if the response date isn't blank

                            if (timeBlank || !timeValid)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "blankDateAlert", "alert('" + message + "');", true);
                                return;
                            }

                        } // if the callbackdate or the response date is blank

                    }// if edit or new

                    txtIssues.Text = txtIssuesKB.Text;
                    txtAnswer.Text = txtAnswerKB.Text;
                    ddlOrderStatus.SelectedIndex = ddlOrderKB.SelectedIndex;
                    txtDate.Text = txtDateToKB.Text;
                    txtTime.Text = txtTimeToKB.Text;

                }// if the kb section is visible

                if (subjectSection.Visible)
                    subjectSection.Visible = false;
                orderStatusSection.Visible = false;
                btnClose.Visible = false;
                productSectionDiv.Visible = false;
                knowledgeSection.Visible = false;
                inboundDiv.Visible = true;

            }// if the kb details section isn't visible

        }//btnClose_Click

        private void inventorySearch(string condition)
        {
            string inventoryCodes = "";
            if (Session["InventoryCodes"] != null)
                inventoryCodes = Session["InventoryCodes"].ToString();

            gvInventory.DataSourceID = odsInventory.ID;
            gvInventory.Visible = true;
            displayWH();

        }//inventorySearch

        private void inventorySearchElectronic(string condition)
        {
            string inventoryCodes = "";
            if (Session["InventoryCodes"] != null)
                inventoryCodes = Session["InventoryCodes"].ToString();

            gvInventoryElectronic.DataSourceID = odsInventoryElectronic.ID;
            gvInventoryElectronic.Visible = true;
        }

        private void displayWH()
        {
            setConstants();
            InboundDB db = new InboundDB();
            DataSet whDS = db.GetUserWHs(userAccess, pageLang);
            DataRowCollection whRows = whDS.Tables[0].Rows;

            foreach (DataRow whRow in whRows)
            {
                int wh = int.Parse(whRow["WH_NUMBER"].ToString());
                if (wh == 0)
                    gvInventory.Columns[3].Visible = true;
                else if (wh == 1)
                    gvInventory.Columns[4].Visible = true;
                else if (wh == 2)
                    gvInventory.Columns[5].Visible = true;
                else if (wh == 3)
                    gvInventory.Columns[6].Visible = true;
                else if (wh == 4)
                    gvInventory.Columns[7].Visible = true;
                else if (wh == 5)
                    gvInventory.Columns[8].Visible = true;
                else if (wh == 6)
                    gvInventory.Columns[9].Visible = true;
                else if (wh == 7)
                    gvInventory.Columns[10].Visible = true;
                else if (wh == 8)
                    gvInventory.Columns[11].Visible = true;
                else
                    gvInventory.Columns[12].Visible = true;

            }//foreach userwh

        }//displayWH

        protected void gvInventory_DataBound(object sender, EventArgs e)
        {
            LanguageDB db = new LanguageDB();
            setConstants();
            if (gvInventory.Visible)
                lblProductCount.Text = gvInventory.Rows.Count + " " + db.GetLabel("InboundTracking", "Items", pageLang);

        }//gvInventory_DataBound

        protected void gvInventory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            InboundDB db = new InboundDB();
            setConstants();

            if (e.Row.DataItemIndex != -1)
            {
                Label lblOtherWH = (Label)e.Row.FindControl("lblOtherWH");
                int otherWH = 0;

                Label lblWH0 = (Label)e.Row.FindControl("lblWH0");
                if (!gvInventory.Columns[3].Visible)
                    otherWH += int.Parse(lblWH0.Text);

                Label lblWH1 = (Label)e.Row.FindControl("lblWH1");
                if (!gvInventory.Columns[4].Visible)
                    otherWH += int.Parse(lblWH1.Text);

                Label lblWH2 = (Label)e.Row.FindControl("lblWH2");
                if (!gvInventory.Columns[5].Visible)
                    otherWH += int.Parse(lblWH2.Text);

                Label lblWH3 = (Label)e.Row.FindControl("lblWH3");
                if (!gvInventory.Columns[6].Visible)
                    otherWH += int.Parse(lblWH3.Text);

                Label lblWH4 = (Label)e.Row.FindControl("lblWH4");
                if (!gvInventory.Columns[7].Visible)
                    otherWH += int.Parse(lblWH4.Text);

                Label lblWH5 = (Label)e.Row.FindControl("lblWH5");
                if (!gvInventory.Columns[8].Visible)
                    otherWH += int.Parse(lblWH5.Text);

                Label lblWH6 = (Label)e.Row.FindControl("lblWH6");
                if (!gvInventory.Columns[9].Visible)
                    otherWH += int.Parse(lblWH6.Text);

                Label lblWH7 = (Label)e.Row.FindControl("lblWH7");
                if (!gvInventory.Columns[10].Visible)
                    otherWH += int.Parse(lblWH7.Text);

                Label lblWH8 = (Label)e.Row.FindControl("lblWH8");
                if (!gvInventory.Columns[11].Visible)
                    otherWH += int.Parse(lblWH8.Text);

                Label lblWH9 = (Label)e.Row.FindControl("lblWH9");
                if (!gvInventory.Columns[12].Visible)
                    otherWH += int.Parse(lblWH9.Text);

                lblOtherWH.Text = otherWH.ToString();

                TextBox txtQuant = (TextBox)e.Row.FindControl("txtProdQuantity");
                if (Session["NewProdText"] != null)
                {
                    string newProduct = Session["NewProdText"].ToString();
                    string status = e.Row.Cells[16].Text;
                    if (status.Contains(newProduct)) //need to decide if this is what we want to do, the code that's commented below is too slow for it to be an option I believe
                        txtQuant.Visible = false; //need to update the translations for the statuses, until then this will only work in english
                }
                // string invCode = e.Row.Cells[1].Text;
                //if (db.GetInventoryItem(pageLang, invCode, null).Tables[0].Rows.Count > 0)
                //{
                //    DataRow row = db.GetInventoryItem(pageLang, invCode, null).Tables[0].Rows[0];
                //    string status = row["INVENTORY_STATUS"].ToString();
                //    if (!status.Equals("A") && !status.Equals("B") && !status.Equals("C") && !status.Equals("E"))
                //    txtQuant.Visible = false;
                //}
                if (Session["Action"] != null)
                {
                    if (Session["Action"].Equals("Edit") || Session["Action"].Equals("New"))
                        txtQuant.Enabled = true;
                }

            }// if there are dataitems

        }//gvInventory_RowDataBound

        protected void gvInventoryElectronic_DataBound(object sender, EventArgs e)
        {
            LanguageDB db = new LanguageDB();
            setConstants();
            if (gvInventory.Visible)
                lblProductCount.Text = gvInventory.Rows.Count + " " + db.GetLabel("InboundTracking", "Items", pageLang);

        }//gvInventory_DataBound

        protected void gvInventoryElectronic_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            InboundDB db = new InboundDB();
            setConstants();

        }//gvInventory_RowDataBound

        protected string kitLabel(int kit)
        {
            setConstants();
            LanguageDB db = new LanguageDB();
            string kitLabel = "";
            if (kit == 1)
                kitLabel = db.GetLabel("InboundTracking", "Kit", pageLang);
            else if (kit == 2)
                kitLabel = db.GetLabel("InboundTracking", "Component", pageLang);
            else
                kitLabel = db.GetLabel("InboundTracking", "NonKit", pageLang);

            return kitLabel;

        }//kitLabel


        protected void btnProductSearch_Click(object sender, EventArgs e)
        {
            setConstants();
            LanguageDB db = new LanguageDB();
            string condition = "All";
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                condition = txtSearch.Text;
                lblDisplayInfo.Text = db.GetLabel("InboundTracking", "SearchProductsDisplayMsg", pageLang);
                Session["ProductAction"] = "Search";
            }
            else
            {
                lblDisplayInfo.Text = db.GetLabel("InboundTracking", "AllProductsDisplayMsg", pageLang);
                Session["ProductAction"] = "All";
            }

            if (ddlWH.SelectedIndex != 0)
                lblDisplayInfo.Text += " with quantity available in " + ddlWH.SelectedValue + ": ";
            else
                lblDisplayInfo.Text += ": ";

            if (ddlWH.Visible == true)
            {
                inventorySearch(condition);
            }
            else
            {
                inventorySearchElectronic(condition);
            }

        }//btnProductSearch_Click

        protected void btnDisplayAll_Click(object sender, EventArgs e)
        {
            setConstants();
            LanguageDB db = new LanguageDB();
            lblDisplayInfo.Text = db.GetLabel("InboundTracking", "AllProductsDisplayMsg", pageLang);
            if (ddlWH.SelectedIndex != 0)
                lblDisplayInfo.Text += " with quantity available in " + ddlWH.SelectedValue + ": ";
            else
                lblDisplayInfo.Text += ": ";
            txtSearch.Text = "";
            inventorySearch("All");
            Session["ProductAction"] = "All";

        }//btnDisplayAll_Click

        protected void btnHot_Click(object sender, EventArgs e)
        {
            setConstants();
            LanguageDB db = new LanguageDB();
            lblDisplayInfo.Text = db.GetLabel("InboundTracking", "HotProductsDisplayMsg", pageLang);
            if (ddlWH.SelectedIndex != 0)
                lblDisplayInfo.Text += " with quantity available in " + ddlWH.SelectedValue + ": ";
            else
                lblDisplayInfo.Text += ": ";

            txtSearch.Text = "New";
            inventorySearch("New");
            Session["ProductAction"] = "Hot/New";

        }//btnHot_Click

        private bool validateTicketNumber(string ticketNumber)
        {
            bool valid = true;
            Regex digitsOnly = new Regex(@"[^\d]");
            ticketNumber = digitsOnly.Replace(ticketNumber, "");
            if (string.IsNullOrEmpty(ticketNumber))
                valid = false;

            return valid;

        }//validateTicketNumber

        protected void btnFind_Click(object sender, EventArgs e)
        {
            InboundDB db = new InboundDB();
            string ticketNumber = txtInqNumber.Text;
            bool valid = true;
            valid = validateTicketNumber(ticketNumber);
            if (valid)
                valid = db.GetInboundDetails(ticketNumber, createIncLineVar()).Tables[0].Rows.Count > 0;

            setConstants();

            if (valid)
            {
                clearFields();
                loadEnquiry(db.GetInboundDetails(ticketNumber, createIncLineVar()));
                loadCustomerDetails(db.GetCustomerDetails(ticketNumber));
                loadProducts(db.GetProductDetails(ticketNumber, pageLang));
                Session["TicketNumber"] = ticketNumber;

            }//if the ticket entered is valid

            if (!valid)
            {
                ticketNumber = Session["ticketNumber"].ToString();
                DataSet ProductDS = db.GetProductDetails(ticketNumber, pageLang);
                loadProducts(ProductDS);
                txtInqNumber.Text = ticketNumber;

            }//if the ticket entered is invalid

        }//btnFind_Click

        protected void imgbtnIssueHistory_Click(object sender, ImageClickEventArgs e)
        {
            gvCommentHistory.DataBind();
            issueZoomDiv.Visible = true;
            ToggleDropDownListVisibility(this.Form.Controls, false);
            if (inboundDiv.Visible)
                txtCurrentComment.Text = txtIssues.Text;
            else
                txtCurrentComment.Text = txtIssuesKB.Text;
            if (Session["EditDate"] != null)
            {
                lblCommentDate.Text = Session["EditDate"].ToString() + " ";

                if (Session["EditTime"] != null)
                    lblCommentDate.Text += Session["EditTime"].ToString();
            }
            else
            {
                if (Session["InputDate"] != null)
                    lblCommentDate.Text = Session["InputDate"].ToString() + " ";

                if (Session["InputTime"] != null)
                    lblCommentDate.Text += Session["InputTime"].ToString();
            }
            lblTicket.Text = txtInqNumber.Text;

        }//imgbtnIssueHistory_Click

        protected void imgbtnAnswer_Click(object sender, ImageClickEventArgs e)
        {
            answerZoomDiv.Visible = true;
            ToggleDropDownListVisibility(this.Form.Controls, false);
            if (inboundDiv.Visible)
            {
                txtIssuesZ.Text = txtIssues.Text;
                txtAnswerZ.Text = txtAnswer.Text;
            }
            else
            {
                txtIssuesZ.Text = txtIssuesKB.Text;
                txtAnswerZ.Text = txtAnswerKB.Text;
            }

        }//imgbtnAnswer_Click

        protected void gvCommentHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvCommentHistory.SelectedRow;
            string issue = row.Cells[3].Text;
            if (!issue.ToLower().Equals("&nbsp;"))
                txtSelComment.Text = row.Cells[3].Text;
            else
                txtSelComment.Text = "";

        }//gvCommentHistory_SelectedIndexChanged

        protected void imgbtnScript_Click(object sender, ImageClickEventArgs e)
        {
            Session["TeleNo"] = ddlLine.SelectedValue.Split('|').ElementAt(0).ToString();
            ScriptManager.RegisterStartupScript(this, GetType(), "ViewScript", "ShowScript();", true);
        }

        protected void imgbtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btnSent = (ImageButton)sender;
            string btnId = btnSent.ID;
            for (int i = 1; i < tblProducts.Rows.Count; ++i)
            {
                ImageButton btnFound = (ImageButton)tblProducts.Rows[i].Cells[0].FindControl("imgbtnDelProduct" + i);
                if (btnId.Equals(btnFound.ID))
                {
                    string productCode = tblProducts.Rows[i].Cells[1].Text;
                    InboundDB db = new InboundDB();
                    if (Session["TempProducts"] != null)
                    {
                        DataTable tempProducts = (DataTable)Session["TempProducts"];
                        foreach (DataRow row in tempProducts.Rows)
                        {
                            if (row["PR_INVENTORY_CODE"].Equals(productCode))
                            {
                                row.Delete();
                                break;
                            }
                        }
                    }
                    if (Session["TempEProducts"] != null)
                    {
                        DataTable tempEProducts = (DataTable)Session["TempEProducts"];
                        foreach (DataRow row in tempEProducts.Rows)
                        {
                            if (row["PR_INVENTORY_CODE"].Equals(productCode))
                            {
                                row.Delete();
                                break;
                            }
                        }
                    }

                    string ticketNum = txtInqNumber.Text;
                    db.DeleteProductDetails(productCode, ticketNum);
                    db.DeleteProductDetailsElectronic(productCode, ticketNum);
                    db.DeleteProductOrder(productCode, ticketNum);
                    Response.Redirect(Request.RawUrl);
                    break;

                }//if the button is found

            }//for each row in the tables

        }//imgbtnDelete_Click

        protected void gvKB_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string refSeq = e.CommandArgument.ToString();
            Session["RefSeq"] = refSeq;

            int rowIndex = 0;
            foreach (DataKey key in gvKB.DataKeys)
            {
                if (key.Value.ToString().Equals(refSeq))
                {
                    CheckBox chkSelected = (CheckBox)gvKB.Rows[rowIndex].FindControl("chkSelected");
                    chkUsage.Checked = chkSelected.Checked;
                    break;
                }
                ++rowIndex;

            }//for each key

            knowledgeSection.Visible = false;
            kbDetailsSection.Visible = true;
            loadKBDetails(refSeq);

        }//gvKB_RowCommand

        protected void gvKB_DataBound(object sender, EventArgs e)
        {
            if (Session["KBKeys"] != null)
            {
                string keys = "";
                if (Session["KBKeys"] != null)
                    keys = Session["KBKeys"].ToString();

                int rowIndex = 0;
                if (!string.IsNullOrEmpty(keys))
                {
                    foreach (DataKey key in gvKB.DataKeys)
                    {
                        if (!string.IsNullOrEmpty(keys))
                        {
                            string[] selKeys = keys.Split('|');
                            for (int i = 1; i < selKeys.Length - 1; ++i) //first and last will be blank, can't avoid the other blanks however
                            {
                                if (key.Value.ToString().Equals(selKeys[i]))
                                {
                                    CheckBox chkSelected = (CheckBox)gvKB.Rows[rowIndex].FindControl("chkSelected");
                                    chkSelected.Checked = true;
                                }

                            }//for the keys

                        }//if there are keys

                        ++rowIndex;

                    }//for each key

                }//if there are keys from the db or keys previously selected

            }//if there was some KB records selected

        }//gvKB_DataBound

        protected void gvSubjects_DataBound(object sender, EventArgs e)
        {
            if (Session["SubCodes"] != null)
            {
                string codes = "";
                if (Session["SubCodes"] != null)
                    codes = Session["SubCodes"].ToString();


                if (!string.IsNullOrEmpty(codes))
                {
                    string[] selCodes = codes.Split('|');
                    for (int i = 1; i < selCodes.Length - 1; ++i)//first and last will be blank, can't avoid the other blanks however
                    {
                        foreach (GridViewRow row in gvSubjects.Rows)
                        {
                            string code = row.Cells[2].Text;
                            if (selCodes[i].ToString().Trim().Equals(code.Trim()))
                            {
                                CheckBox chkSelectedSubj = (CheckBox)row.FindControl("chkSelectedSubj");
                                chkSelectedSubj.Checked = true;
                                break;

                            }//if the code from the array is equal to the code from the gv row

                        }//foreach row in the gv

                    }//for the selcodes except the first and last

                }//if there are codes

            }//if the subject session is populated

        }//gvSubjects_DataBound

        private void loadKBDetails(string refSeq)
        {
            InboundDB db = new InboundDB();
            LanguageDB langdb = new LanguageDB();
            setConstants();
            DataSet ds = db.getKnowledgeDetails(refSeq, pageLang);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                string teleNo = "";
                if (row.Table.Columns.Contains("ref_campaign"))
                    teleNo = row["ref_campaign"].ToString();
                txtTele.Text = teleNo;

                string campaign = "";
                if (row.Table.Columns.Contains("TELE_DESC"))
                    campaign = row["TELE_DESC"].ToString();
                txtCampaign.Text = campaign;

                if (row.Table.Columns.Contains("ref_active") && row["ref_active"].ToString().ToUpper().Equals("YES"))
                    chkActive.Checked = true;
                else
                    chkActive.Checked = false;

                string dateEdit = "";
                if (row.Table.Columns.Contains("edit_date"))
                    dateEdit = row["edit_date"].ToString();
                lblEditDateKBD.Text = dateEdit;

                string dateInput = "";
                if (row.Table.Columns.Contains("input_date"))
                    dateInput = row["input_date"].ToString();
                lblInputDateKBDVal.Text = dateInput;

                string refOperator = "";
                if (row.Table.Columns.Contains("ref_operator"))
                    refOperator = row["ref_operator"].ToString();
                lblOperator.Text = refOperator;

                string referralNum = "";
                if (row.Table.Columns.Contains("ref_rec_no"))
                    referralNum = row["ref_rec_no"].ToString();
                lblReferral.Text = referralNum;

                string subj = "";
                if (row.Table.Columns.Contains("ref_title"))
                    subj = row["ref_title"].ToString();
                txtKBSubject.Text = subj;

                string subjFR = "";
                if (row.Table.Columns.Contains("ref_fr_instr"))
                    subjFR = row["ref_fr_instr"].ToString();
                txtKBSubjectFR.Text = subjFR;

                string desc = "";
                if (row.Table.Columns.Contains("ref_resp_desc"))
                    desc = row["ref_resp_desc"].ToString();
                txtKBDesc.Text = desc;

                if (row.Table.Columns.Contains("ref_fr_desc"))
                    Session["KBDFrenchDesc"] = row["ref_fr_desc"].ToString();

                if (row.Table.Columns.Contains("ref_referral") && row["ref_referral"].ToString().ToUpper().Equals("YES"))
                    chkReferral.Checked = true;
                else
                    chkReferral.Checked = false;

                if (row.Table.Columns.Contains("ref_qa") && row["ref_qa"].ToString().ToUpper().Equals("YES"))
                    chkQA.Checked = true;
                else
                    chkQA.Checked = false;

                if (row.Table.Columns.Contains("ref_prog_info") && row["ref_prog_info"].ToString().ToUpper().Equals("YES"))
                    chkProgram.Checked = true;
                else
                    chkProgram.Checked = false;

                string firstName = "";
                if (row.Table.Columns.Contains("ref_first_name"))
                    firstName = row["ref_first_name"].ToString();
                txtKBFirst.Text = firstName;

                string lastName = "";
                if (row.Table.Columns.Contains("ref_surname"))
                    lastName = row["ref_surname"].ToString();
                txtKBLast.Text = lastName;

                string company = "";
                if (row.Table.Columns.Contains("ref_company"))
                    company = row["ref_company"].ToString();
                txtKBCompany.Text = company;

                string phone = "";
                if (row.Table.Columns.Contains("ref_tele_no"))
                    phone = row["ref_tele_no"].ToString();
                txtKBPhone.Text = phone;

                string fax = "";
                if (row.Table.Columns.Contains("ref_fax_no"))
                    fax = row["ref_fax_no"].ToString();
                txtKBFax.Text = fax;

                string address = "";
                if (row.Table.Columns.Contains("ref_address_1"))
                    address = row["ref_address_1"].ToString();
                txtKBAddress.Text = address;

                ListItem branchItem = new ListItem();
                if (row.Table.Columns.Contains("BR_DEF") && !string.IsNullOrEmpty(row["BR_DEF"].ToString()))
                {
                    txtKBDivision.Text = row["BR_DEF"].ToString();
                }


                if (row.Table.Columns.Contains("IND_DEF") && !string.IsNullOrEmpty(row["IND_DEF"].ToString()))
                {
                    txtKBKeyword.Text = row["IND_DEF"].ToString();
                }

                string cityProv = "";
                if (row.Table.Columns.Contains("ref_address_2"))
                    cityProv = row["ref_address_2"].ToString();
                txtKBCityProv.Text = cityProv;

                string postCode = "";
                if (row.Table.Columns.Contains("ref_postalcode"))
                    postCode = row["ref_postalcode"].ToString();
                txtKBPostalCode.Text = postCode;

                string email = "";
                if (row.Table.Columns.Contains("ref_email"))
                    email = row["ref_email"].ToString();
                txtKBEmail.Text = email;

                string webSite = "";
                if (row.Table.Columns.Contains("ref_url") && row["ref_url"].ToString().Length > 0)
                {
                    webSite = row["ref_url"].ToString();
                    lnkWebSite.HRef = row["ref_url"].ToString();
                    lnkWebSite.Attributes.Add("target", "_blank");
                    lnkWebSite.InnerText = langdb.GetLabel("KnowledgeBaseDetails", "GoToWebPage", pageLang);
                }
                else
                {
                    lnkWebSite.InnerText = langdb.GetLabel("KnowledgeBaseDetails", "NoWebSite", pageLang);
                    lnkWebSite.Attributes.Remove("target");
                    lnkWebSite.HRef = "";
                }
                txtKBWeb.Text = webSite;

            }//if there is a row returned

        }//loadKBDetails

        protected void btnSearchKB_Click(object sender, EventArgs e)
        {
            setConstants();
            LanguageDB db = new LanguageDB();
            string condition = "";
            string records = " " + db.GetLabel("InboundTracking", "Records", pageLang);
            searchKB(condition);
            if (!string.IsNullOrWhiteSpace(txtSearchKB.Text))
            {
                condition = txtSearchKB.Text;
                string message = db.GetLabel("InboundTracking", "SearchResultsDisplayMsg", pageLang);
                lblKBCount.Text = message + " " + gvKB.Rows.Count + records;
            }
            else
            {
                string message = db.GetLabel("InboundTracking", "KBActiveRecordsDisplayMsg", pageLang);
                lblKBCount.Text = message + " " + gvKB.Rows.Count + records;
            }

        }//btnSearchKB_Click

        protected string shortenLabel(string label)
        {
            if (label.Length > 35)
                label = Utility.shortenLabel(label, 35);
            return label;

        }//shortenLabel

        protected string shortenLabel2(string label)
        {
            if (label.Length > 75)
                label = Utility.shortenLabel(label, 75);
            return label;

        }//shortenLabel2

        protected string DelMessage
        {
            get
            {
                setConstants();
                LanguageDB db = new LanguageDB();
                return db.GetLabel("InboundTracking", "DeleteFstat", pageLang);
            }

        }//

        protected string AnsToManyCharsMessage
        {
            get
            {
                setConstants();
                LanguageDB db = new LanguageDB();
                return db.GetLabel("InboundTracking", "MsgAnswerToManyChars", pageLang);
            }

        }//

        protected string IssToManyCharsMessage
        {
            get
            {
                setConstants();
                LanguageDB db = new LanguageDB();
                return db.GetLabel("InboundTracking", "MsgIssueToManyChars", pageLang);
            }

        }//

        protected string setSelectText()
        {
            setConstants();
            LanguageDB db = new LanguageDB();
            return db.GetLabel("InboundTracking", "Select", pageLang);

        }//setSelectText

        private void searchKB(string condition)
        {
            gvKB.DataBind();
            if (gvKB.Rows.Count > 0)
            {
                gvKB.Visible = true;

                if (Session["Action"] != null && (Session["Action"].ToString().Equals("Edit") || Session["Action"].ToString().Equals("New")))
                {
                    foreach (GridViewRow row in gvKB.Rows)
                    {
                        CheckBox chkSelected = (CheckBox)row.FindControl("chkSelected");
                        chkSelected.Enabled = true;
                    }

                }//if it's edit or new

                tblKB.Visible = false;

            }//if there are any rows
            else
            {
                tblKB.Visible = true;
                gvKB.Visible = false;
            }

        }//searchKB

        protected void btnListAll_Click(object sender, EventArgs e)
        {
            setConstants();
            LanguageDB db = new LanguageDB();
            string records = " " + db.GetLabel("InboundTracking", "Records", pageLang);
            string message = db.GetLabel("InboundTracking", "KBActiveRecordsDisplayMsg", pageLang);
            txtSearchKB.Text = "";
            searchKB("");
            lblKBCount.Text = message + " " + gvKB.Rows.Count + records;

        }//btnListAll_Click

        protected void btnShowSelected_Click(object sender, EventArgs e)
        {
            setConstants();
            LanguageDB db = new LanguageDB();
            string records = " " + db.GetLabel("InboundTracking", "Records", pageLang);
            txtSearchKB.Text = "";
            searchKB("");
            int count = 0;
            foreach (GridViewRow row in gvKB.Rows)
            {
                CheckBox chkSelected = (CheckBox)row.FindControl("chkSelected");
                if (!chkSelected.Checked)
                    row.Visible = false;
                else
                    ++count;
            }
            //need to decide if we want this message, I think the ticket # part is redundant 
            string message = db.GetLabel("InboundTracking", "KnowledgeBaseEntriesDisplayMsg", pageLang) + ": ";
            //string message = "Displaying knowledgebase items associated with the current enquiry: ";
            lblKBCount.Text = message + count + records;

        }//btnShowSelected_Click

        protected void chkUsage_CheckedChanged(object sender, EventArgs e)
        {
            string refSeq = "";
            if (Session["RefSeq"] != null)
                refSeq = Session["RefSeq"].ToString();

            if (!string.IsNullOrEmpty(refSeq))
            {
                int rowIndex = 0;
                foreach (DataKey key in gvKB.DataKeys)
                {
                    if (key.Value.ToString().Equals(refSeq))
                    {
                        CheckBox chkSelected = (CheckBox)gvKB.Rows[rowIndex].FindControl("chkSelected");
                        chkSelected.Checked = chkUsage.Checked;
                        break;
                    }//if the key is the same as the one that was sent
                    ++rowIndex;

                }//for each datakey

            }// if there is a key stored

        }//chkUsage_CheckedChanged

        protected void chkSelected_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSelected = (CheckBox)sender;
            string key = "|" + chkSelected.Text + "|";
            if (chkSelected.Checked)
            {
                if (Session["KBKeys"] != null && !Session["KBKeys"].ToString().Contains(key))
                    Session["KBKeys"] += key;
                else
                    Session["KBKeys"] = key;

            }//if the checkbox is selected

            else
            {
                if (Session["KBKeys"] != null)
                {
                    string tempKB = Session["KBKeys"].ToString();
                    tempKB = tempKB.Replace(key.Trim(), "");
                    Session["KBKeys"] = tempKB;
                }

            }// if the checkbox gets deselected

        }//chkSelected_CheckedChanged



        protected void btnSubject_Click(object sender, EventArgs e)
        {
            inboundDiv.Visible = false;
            txtClientInfoSubj.Text = txtFirst.Text + " " + txtLast.Text + " " + txtOrganization.Text + " " + ddlProvState2.SelectedValue;
            txtTicketNumSubj.Text = txtInqNumber.Text;
            subjectSection.Visible = true;
            btnClose.Visible = true;
            gvSubjects.DataBind();
            foreach (GridViewRow row in gvSubjects.Rows)
            {
                CheckBox chkSelectedSubj = (CheckBox)row.FindControl("chkSelectedSubj");
                if (Session["Action"] != null && (Session["Action"].ToString().Equals("Edit") || Session["Action"].ToString().Equals("New")))
                    chkSelectedSubj.Enabled = true;
                else
                    chkSelectedSubj.Enabled = false;
            }

        }//btnSubject_Click

        protected void imgbtnCal2_Click(object sender, ImageClickEventArgs e)
        {
            if (calDateToKB.Visible)
                calDateToKB.Visible = false;
            else
                calDateToKB.Visible = true;

        }//imgbtnCal2_Click

        protected void calDateToKB_SelectionChanged(object sender, EventArgs e)
        {
            txtDateToKB.Text = Utility.getDate(calDateToKB.SelectedDate.ToString("dd-MMM-yyyy"));
            calDateToKB.Visible = false;

        }//calDateToKB_SelectionChanged

        protected void imgbtnCal3_Click(object sender, ImageClickEventArgs e)
        {
            if (calResponseDate.Visible)
                calResponseDate.Visible = false;
            else
                calResponseDate.Visible = true;

        }//imgbtnCal3_Click

        protected void calResponseDate_SelectionChanged(object sender, EventArgs e)
        {
            txtResponseDate.Text = Utility.getDate(calResponseDate.SelectedDate.ToString("dd-MMM-yyyy"));
            
            calResponseDate.Visible = false;

        }//calResponseDate_SelectionChanged

        protected void imgbtnCal4_Click(object sender, ImageClickEventArgs e)
        {
            if (calLowVal.Visible)
                calLowVal.Visible = false;
            else
                calLowVal.Visible = true;

        }//imgbtnCal4_Click

        protected void calLowVal_SelectionChanged(object sender, EventArgs e)
        {
            txtLowVal.Text = Utility.getDate(calLowVal.SelectedDate.ToString("dd-MMM-yyyy"));
            calLowVal.Visible = false;

        }//calLowVal_SelectionChanged

        protected void imgbtnCal5_Click(object sender, ImageClickEventArgs e)
        {
            if (calHighVal.Visible)
                calHighVal.Visible = false;
            else
                calHighVal.Visible = true;

        }//imgbtnCal5_Click

        protected void calHighVal_SelectionChanged(object sender, EventArgs e)
        {
            txtHighVal.Text = Utility.getDate(calHighVal.SelectedDate.ToString("dd-MMM-yyyy"));
            calHighVal.Visible = false;

        }//calHighVal_SelectionChanged

        private void populateTempProducts(string inventoryCode, int quant, int committed, int totalQTY, int maxAllowed, string title, int gst, int pst, int unitPrice,
                        int unitCost, string prodStatus, int kitStatus, int wh, bool whHasQTY, DataTable tempDT)
        {
            int subTotal = 0;
            int total = 0;
            int totalCost = 0;
            subTotal = unitPrice * quant;
            //need to add logic to calc the gst and pst by multiplying subtotal by each if it is required by future clients
            total = subTotal + gst + pst;
            totalCost = unitCost * quant;
            LanguageDB db = new LanguageDB();
            setConstants();

            if (!whHasQTY || (quant > maxAllowed))
            {

                if (quant > maxAllowed)
                {
                    txtBackOrder.Text = "YES";
                    string alertMessage = db.GetLabel("InboundTracking", "ProductNumber", pageLang) + " " + inventoryCode + " " + db.GetLabel("InboundTracking", "LimitedQuantities", pageLang)
                        + " " + totalQTY + " " + db.GetLabel("InboundTracking", "Committed", pageLang) + " " + committed +
                        db.GetLabel("InboundTracking", "ChangeTheQuantity", pageLang) + "\\n\\n";

                    if (Session["ProductAlert"] != null)
                        Session["ProductAlert"] += alertMessage;
                    else
                        Session["ProductAlert"] = alertMessage;

                }//if the quantity is more than the max allowed

                if (!whHasQTY)
                {
                    string alertMessage = db.GetLabel("InboundTracking", "ProductNumber", pageLang) + inventoryCode + " " +
                        db.GetLabel("InboundTracking", "MsgNotAvailAtWH", pageLang).Replace("[whNum]", wh.ToString());

                    if (Session["ProductAlert"] != null)
                        Session["ProductAlert"] += alertMessage;
                    else
                        Session["ProductAlert"] = alertMessage;
                }

            }// if the wh does not have enough or the quantity is more than max allowed

            else
            {
                string alertMessage = db.GetLabel("InboundTracking", "ProductNumber", pageLang) + inventoryCode + " " +
                        db.GetLabel("InboundTracking", "AddedSuccessfully", pageLang);

                if (Session["ProductAlert"] != null)
                    Session["ProductAlert"] += alertMessage;
                else
                    Session["ProductAlert"] = alertMessage;

            }// if there are no problems with the order

            string whNum = "";
            if (wh != -1)
                whNum = wh.ToString();
            tempDT.Rows.Add(inventoryCode, quant, title, subTotal, totalCost, total, 0, gst, pst, kitStatus, whNum, prodStatus, null, "H");
            //0 is process status, the null is for notes

        }//populateTempProducts


        private void populateTempProductsElectronic(string inventoryCode, int quant, int committed, int totalQTY, int maxAllowed, string title, int gst, int pst, int unitPrice,
                int unitCost, string prodStatus, int kitStatus, int wh, bool whHasQTY, DataTable tempDT)
        {
            int subTotal = 0;
            int total = 0;
            int totalCost = 0;
            subTotal = unitPrice * quant;
            //need to add logic to calc the gst and pst by multiplying subtotal by each if it is required by future clients
            total = subTotal + gst + pst;
            totalCost = unitCost * quant;
            LanguageDB db = new LanguageDB();
            setConstants();


            string alertMessage = db.GetLabel("InboundTracking", "ProductNumber", pageLang) + inventoryCode + " " +
                    db.GetLabel("InboundTracking", "AddedSuccessfully", pageLang);

            if (Session["ProductAlert"] != null)
                Session["ProductAlert"] += alertMessage;
            else
                Session["ProductAlert"] = alertMessage;


            string whNum = "0";

            tempDT.Rows.Add(inventoryCode, quant, title, subTotal, totalCost, total, 0, gst, pst, kitStatus, whNum, prodStatus, null, "E");
            //0 is process status, the null is for notes

        }//populateTempProductsElectronic

        private bool getStatusForProduct(string inventoryCode)
        {
            InboundDB db = new InboundDB();
            string status = db.GetStatusForProduct(inventoryCode);
            status = status.Trim();
            if (status.Equals("B"))
            {
                txtBackOrder.Text = "YES";
                return true;
            }
            else if (status.Equals("E") || status.Equals("A") || status.Equals("C"))
                return true;
            else
                return false;

        }//getStatusForProduct

        protected void btnAddToOrder_Click(object sender, EventArgs e)
        {
            if (gvInventory.Visible == true)
            {
                AddHardcopyProducts();
            }
            if (gvInventoryElectronic.Visible == true)
            {
                AddElectronicProducts();
            }
        }//btnAddToOrder_Click

        protected void AddHardcopyProducts()
        {
            Session["ProductAlert"] = null;
            setConstants();
            InboundDB db = new InboundDB();
            LanguageDB langDB = new LanguageDB();
            DataTable tempDT = new DataTable();
            tempDT.Columns.Add("PR_INVENTORY_CODE");
            tempDT.Columns.Add("PR_QUANTITY");
            tempDT.Columns.Add("PR_TITLE");
            tempDT.Columns.Add("PR_SUBTOTAL");
            tempDT.Columns.Add("PR_TOTAL_COST");
            tempDT.Columns.Add("PR_TOTAL");
            tempDT.Columns.Add("PR_PROCESS_STATUS");
            tempDT.Columns.Add("PR_GST");
            tempDT.Columns.Add("PR_PST");
            tempDT.Columns.Add("PR_KIT");
            tempDT.Columns.Add("PR_WH_NUM");
            tempDT.Columns.Add("PROD_STATUS_DEF");
            tempDT.Columns.Add("PR_NOTES");
            tempDT.Columns.Add("PROD_TYPE");

            int newCount = gvInventory.Rows.Count;
            for (int i = 0; i < gvInventory.Rows.Count; ++i)
            {
                GridViewRow row = gvInventory.Rows[i];
                TextBox qty = (TextBox)row.FindControl("txtProdQuantity");
                if (qty != null)
                {
                    Regex digitsOnly = new Regex(@"[^\d]");
                    string quantString = digitsOnly.Replace(qty.Text, "");
                    int quant = 0;
                    bool whHasQTY = true;
                    if (!string.IsNullOrEmpty(quantString))
                        quant = int.Parse(quantString);
                    if (quant > 0)
                    {
                        int wh = db.GetWHByDelCode(ddlDelivery.SelectedValue);
                        DataRow productInfo;
                        if (db.GetInventoryItem(pageLang, row.Cells[1].Text, wh.ToString()).Tables[0].Rows.Count > 0)
                            productInfo = db.GetInventoryItem(pageLang, row.Cells[1].Text, wh.ToString()).Tables[0].Rows[0];
                        else
                        {
                            productInfo = db.GetInventoryItem(pageLang, row.Cells[1].Text, null).Tables[0].Rows[0];
                            //null for the WH value so that it still gets the product despite not being listed at the selected WH
                            whHasQTY = false;
                        }

                        bool addHeader = false;
                        if (productInfo["INVENTORY_KIT_ADD"].ToString().Equals("YES"))
                            addHeader = true;

                        string kit = productInfo["INVENTORY_KIT"].ToString();

                        int committed = 0;
                        int totalQTY = 0;
                        int criticalValue = 0;
                        int maxAllowed = 0;
                        string title = "";
                        int gst = 0;
                        int pst = 0;
                        int unitPrice = 0;
                        int unitCost = 0;
                        int kitStatus = 0;
                        if (addHeader || string.IsNullOrEmpty(kit) || int.Parse(kit) != 1)
                        {
                            committed = int.Parse(row.Cells[15].Text);
                            totalQTY = int.Parse(row.Cells[14].Text);
                            criticalValue = db.GetCriticalValue(row.Cells[1].Text);
                            maxAllowed = totalQTY - committed - criticalValue;


                            Label lblTitle = (Label)row.FindControl("lblTitle");

                            if (!string.IsNullOrEmpty(lblTitle.Text))
                                title = lblTitle.Text;

                            //need to check the gst exempt, pst exempt then set gst, pst accordingly

                            if (productInfo.Table.Columns.Contains("UNIT_PRICE"))
                                unitPrice = int.Parse(productInfo["UNIT_PRICE"].ToString());

                            if (productInfo.Table.Columns.Contains("UNIT_COST"))
                                unitCost = int.Parse(productInfo["UNIT_COST"].ToString());

                            if (productInfo.Table.Columns.Contains("INVENTORY_KIT"))
                                kitStatus = int.Parse(productInfo["INVENTORY_KIT"].ToString());

                            if (getStatusForProduct(row.Cells[1].Text))
                            {
                                row.Visible = false;
                                populateTempProducts(row.Cells[1].Text, quant, committed, totalQTY, maxAllowed, title, gst, pst, unitPrice, unitCost, row.Cells[16].Text,
                                    kitStatus, wh, whHasQTY, tempDT); //cell 1 is the invcode, cell16 is prodstatus

                                --newCount;
                            }// if the status is still A, B, C or E

                            else
                            {
                                string alertMessage = langDB.GetLabel("InboundTracking", "MsgStatusChanged", pageLang).Replace("[pNum]", row.Cells[1].Text);

                                if (Session["ProductAlert"] != null)
                                    Session["ProductAlert"] += alertMessage;
                                else
                                    Session["ProductAlert"] = alertMessage;

                            }// if the status has changed since the list was built

                        }// if it's not a kit or it's a kit where you add the header

                        if (!string.IsNullOrEmpty(kit) && int.Parse(kit) == 1)
                        {
                            whHasQTY = true;
                            string invRecNo = productInfo["INVENTORY_REC_NO"].ToString();
                            DataSet kitDS = db.GetKitItems(invRecNo);
                            DataRowCollection kitRows = kitDS.Tables[0].Rows;

                            foreach (DataRow kitRow in kitRows)
                            {
                                string invCode = kitRow["KIT_PROD"].ToString();
                                DataRow kitProd;
                                if (db.GetInventoryItem(pageLang, invCode, wh.ToString()).Tables[0].Rows.Count > 0)
                                    kitProd = db.GetInventoryItem(pageLang, invCode, wh.ToString()).Tables[0].Rows[0];
                                else
                                {
                                    kitProd = db.GetInventoryItem(pageLang, invCode, null).Tables[0].Rows[0];
                                    //null for the WH value so that it still gets the product despite not being listed at the selected WH
                                    whHasQTY = false;
                                }

                                committed = int.Parse(kitProd["CMTD"].ToString());
                                totalQTY = int.Parse(kitProd["TOTAL_QTY"].ToString());
                                criticalValue = db.GetCriticalValue(invCode);
                                maxAllowed = totalQTY - committed - criticalValue;
                                title = kitProd["INVENTORY_DEF"].ToString();
                                //need to check the gst exempt, pst exempt then set gst, pst accordingly
                                unitPrice = int.Parse(kitProd["UNIT_PRICE"].ToString());
                                unitCost = int.Parse(kitProd["UNIT_COST"].ToString());
                                string prodStatus = kitProd["PROD_STATUS_DEF"].ToString();
                                kitStatus = int.Parse(kitProd["INVENTORY_KIT"].ToString());
                                populateTempProducts(invCode, quant, committed, totalQTY, maxAllowed, title, gst, pst, unitPrice, unitCost, prodStatus, kitStatus, wh, whHasQTY, tempDT);

                            }//for each kit component

                        }// if it's a kit

                    }// if there is a quantity

                }// if the quantity textbox exists in the row

            }//for all the rows

            if (Session["ProductAlert"] != null)
                ScriptManager.RegisterStartupScript(this, GetType(), "productAlert", "alert('" + Session["ProductAlert"].ToString() + "');", true);

            if (tempDT.Rows.Count > 0)
                Session["TempProducts"] = tempDT;
            lblProductCount.Text = newCount + " " + langDB.GetLabel("InboundTracking", "Items", pageLang);

        }

        protected void AddElectronicProducts()
        {
            Session["ProductAlert"] = null;
            setConstants();
            InboundDB db = new InboundDB();
            LanguageDB langDB = new LanguageDB();
            DataTable tempDT = new DataTable();
            string availText = langDB.GetLabel("InboundTracking", "QtyAva", pageLang);
            tempDT.Columns.Add("PR_INVENTORY_CODE");
            tempDT.Columns.Add("PR_QUANTITY");
            tempDT.Columns.Add("PR_TITLE");
            tempDT.Columns.Add("PR_SUBTOTAL");
            tempDT.Columns.Add("PR_TOTAL_COST");
            tempDT.Columns.Add("PR_TOTAL");
            tempDT.Columns.Add("PR_PROCESS_STATUS");
            tempDT.Columns.Add("PR_GST");
            tempDT.Columns.Add("PR_PST");
            tempDT.Columns.Add("PR_KIT");
            tempDT.Columns.Add("PR_WH_NUM");
            tempDT.Columns.Add("PROD_STATUS_DEF");
            tempDT.Columns.Add("PR_NOTES");
            tempDT.Columns.Add("PROD_TYPE");

            int newCount = gvInventoryElectronic.Rows.Count;
            for (int i = 0; i < gvInventoryElectronic.Rows.Count; ++i)
            {
                GridViewRow row = gvInventoryElectronic.Rows[i];
                CheckBox qty = (CheckBox)row.FindControl("chkProdSelectE");
                if (qty.Checked == true)
                {
                    int quant = 1;

                    if (quant > 0)
                    {
                        int wh = db.GetWHByDelCode(ddlDelivery.SelectedValue);
                        DataRow productInfo;

                        productInfo = db.GetInventoryItem(pageLang, row.Cells[1].Text, null).Tables[0].Rows[0];
                        bool addHeader = false;

                        string kit = productInfo["INVENTORY_KIT"].ToString();

                        int committed = 0;
                        int totalQTY = 0;
                        int criticalValue = 0;
                        int maxAllowed = 0;
                        string title = "";
                        int gst = 0;
                        int pst = 0;
                        int unitPrice = 0;
                        int unitCost = 0;
                        int kitStatus = 0;
                        if (addHeader || string.IsNullOrEmpty(kit) || int.Parse(kit) != 1)
                        {
                            committed = 0;
                            totalQTY = 0;
                            criticalValue = 0;
                            maxAllowed = 0;

                            Label lblTitle = (Label)row.FindControl("lblTitleE");

                            if (!string.IsNullOrEmpty(lblTitle.Text))
                                title = lblTitle.Text;

                            unitPrice = 0;
                            unitCost = 0;
                            kitStatus = 0;

                            row.Visible = false;
                            populateTempProductsElectronic(row.Cells[1].Text, quant, committed, totalQTY, maxAllowed, title, gst, pst, unitPrice, unitCost, availText,
                                kitStatus, wh, true, tempDT); //cell 1 is the invcode, cell16 is prodstatus

                            --newCount;


                        }// if it's not a kit or it's a kit where you add the header

                    }// if there is a quantity

                }// if the quantity textbox exists in the row

            }//for all the rows

            if (Session["ProductAlert"] != null)
                ScriptManager.RegisterStartupScript(this, GetType(), "productAlert", "alert('" + Session["ProductAlert"].ToString() + "');", true);

            if (tempDT.Rows.Count > 0)
                Session["TempEProducts"] = tempDT;
            lblProductCount.Text = newCount + " " + langDB.GetLabel("InboundTracking", "Items", pageLang);

        

        }

        protected void btnAnswerClose_Click(object sender, EventArgs e)
        {
            answerZoomDiv.Visible = false;
            ToggleDropDownListVisibility(this.Form.Controls, true);
            txtIssues.Text = txtIssuesZ.Text;
            txtAnswer.Text = txtAnswerZ.Text;
            txtIssuesKB.Text = txtIssuesZ.Text;
            txtAnswerKB.Text = txtAnswerZ.Text;

        }//btnAnswerClose_Click

        protected void btnCloseIssues_Click(object sender, EventArgs e)
        {
            issueZoomDiv.Visible = false;
            ToggleDropDownListVisibility(this.Form.Controls, true);
            gvCommentHistory.SelectedIndex = -1;
            txtSelComment.Text = "";
            txtIssues.Text = txtCurrentComment.Text;
            txtIssuesKB.Text = txtCurrentComment.Text;

        }//btnCloseIssues_Click

        protected void imgBtnZoom_Click(object sender, ImageClickEventArgs e)
        {
            txtKBDTicketNum.Text = txtInqNumber.Text;
            kbdTop.Visible = false;
            kbdBottom.Visible = false;
            kbdRef.Visible = false;
            kbdTicket.Visible = true;
            kbdZoomTable.Visible = true;
            txtKBDZoomWeb.Text = txtKBWeb.Text;
            lnkZoomWeb.InnerText = lnkWebSite.InnerText;
            lnkZoomWeb.HRef = lnkWebSite.HRef;
            txtKBDZoomSubject.Text = txtKBSubject.Text;
            txtKBDZoomDetails.Text = txtKBDesc.Text;
            setConstants();
            LanguageDB db = new LanguageDB();
            btnShowLang.Text = db.GetLabel("KnowledgeBaseDetails", "ShowFrench", pageLang);

        }//imgBtnZoom_Click

        protected void btnShowLang_Click(object sender, EventArgs e)
        {
            LanguageDB db = new LanguageDB();
            setConstants();
            if (btnShowLang.Text.Contains("French") || btnShowLang.Text.Contains("fran"))
            {
                btnShowLang.Text = db.GetLabel("KnowledgeBaseDetails", "ShowEnglish", pageLang);
                txtKBDZoomSubject.Text = txtKBSubjectFR.Text;
                if (Session["KBDFrenchDesc"] != null)
                    txtKBDZoomDetails.Text = Session["KBDFrenchDesc"].ToString();
            }
            else
            {
                btnShowLang.Text = db.GetLabel("KnowledgeBaseDetails", "ShowFrench", pageLang);
                txtKBDZoomSubject.Text = txtKBSubject.Text;
                txtKBDZoomDetails.Text = txtKBDesc.Text;
            }

        }//btnShowLang_Click

        protected void btnContinue_Click(object sender, EventArgs e)
        {

            bool promoFlag = false;
            setConstants();
            InboundDB db = new InboundDB();
            foreach (GridViewRow row in gvSubjects.Rows)
            {
                string code = "|" + row.Cells[2].Text + "|";

                CheckBox chkSelectedSubj = (CheckBox)row.FindControl("chkSelectedSubj");
                if (chkSelectedSubj.Checked)
                {
                    if (Session["SubCodes"] != null && !Session["SubCodes"].ToString().Contains(code))
                        Session["SubCodes"] += code;
                    else
                        Session["SubCodes"] = code;

                    string subjCode = code.Replace("|", "").Trim();

                    if (Session["PromoSelected"] == null || !bool.Parse(Session["PromoSelected"].ToString()) || ddlSource.SelectedIndex == 0)
                    {
                        int promoCount = db.GetSourcesForSubject(pageLang, subjCode).Tables[0].Rows.Count;
                        if (promoCount > 0)
                            promoFlag = true;
                    }
                }//if the checkbox is selected

                else
                {
                    if (Session["SubCodes"] != null)
                    {
                        string tempSub = Session["SubCodes"].ToString();
                        tempSub = tempSub.Replace(code.Trim(), "");
                        Session["SubCodes"] = tempSub;

                    }// if there are already subcodes, see if the current code is in the list and remove it

                }//if the checkbox is not selected

            }//foreach row in the gv
            if (Session["PromoSelected"] == null || !bool.Parse(Session["PromoSelected"].ToString()) || ddlSource.SelectedIndex == 0)
            {

                if (promoFlag)
                    promotionDiv.Visible = true;
                else
                {
                    subjectSection.Visible = false;
                    btnClose.Visible = false;
                    inboundDiv.Visible = true;
                }

            }// if there hasn't already been a promotion selected

            else
            {
                subjectSection.Visible = false;
                btnClose.Visible = false;
                inboundDiv.Visible = true;

            }// if there is a promotion selected

        }//btnContinue_Click

        protected void btnOrderSearch_Click(object sender, EventArgs e)
        {
            txtSelectAllHiddenFlag.Text = "N";
            if (ddlCriteria.SelectedIndex == 1)
            {
                if (!string.IsNullOrEmpty(txtLowVal.Text) || !string.IsNullOrEmpty(txtHighVal.Text))
                    gvOrderStatus.Visible = true;
            }
            else
            {
                string lowVal = txtLowVal.Text;
                bool validLow = true;
                validLow = validateTicketNumber(lowVal);
                if (!validLow)
                    txtLowVal.Text = "";
                string highVal = txtHighVal.Text;
                bool validHigh = true;
                validHigh = validateTicketNumber(highVal);
                if (!validHigh)
                    txtHighVal.Text = "";
                if (validLow || validHigh)
                    gvOrderStatus.Visible = true;
                else
                {
                    lblOrderStatusRecords.Text = "";
                    return;
                }
            }
            lblOrderStatusRecords.Text = "";
            gvOrderStatus.Sort("S_REC_NO", SortDirection.Descending);
            txtHighVal.Visible = true;
            txtLowVal.Visible = true;

            Session["OrderSearchCriteria"] = ddlCriteria.SelectedIndex;
            Session["OrderSearchHighVal"] = txtHighVal.Text;
            Session["OrderSearchLowVal"] = txtLowVal.Text;
            Session["OrderSearchGroup"] = ddlGroup.SelectedIndex;
            Session["OrderSearchStatus"] = ddlStatus.SelectedIndex;
            Session["OrderSearchTypeRun"] = "standard";

        }//btnOrderSearch_Click

        protected void btnShowIncomplete_Click(object sender, EventArgs e)
        {
            gvOrderStatus.Visible = true;
            //ddlCriteria.SelectedIndex = 0;
            txtSelectAllHiddenFlag.Text = "Y";
            lblOrderStatusRecords.Text = "";
            gvOrderStatus.Sort("S_REC_NO", SortDirection.Descending);
            //txtHighVal.Text = "";
            //txtLowVal.Text = "";
            txtHighVal.Visible = true;
            txtLowVal.Visible = true;
            Session["OrderSearchTypeRun"] = "allincomplete";


        }//btnShowIncomplete_Click

        protected void ddlCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCriteria.SelectedIndex == 0 || ddlCriteria.SelectedIndex == 2)
            {
                imgbtnCal4.Visible = false;
                imgbtnCal5.Visible = false;
                txtLowVal.Enabled = true;
                txtHighVal.Enabled = true;
            }
            else
            {
                imgbtnCal4.Visible = true;
                imgbtnCal5.Visible = true;
                txtLowVal.Enabled = false;
                txtHighVal.Enabled = false;
            }

            txtLowVal.Text = "";
            txtHighVal.Text = "";
            gvOrderStatus.Visible = false;
            lblOrderStatusRecords.Text = "";

        }//ddlCriteria_SelectedIndexChanged

        protected void gvOrderStatus_DataBound(object sender, EventArgs e)
        {
            LanguageDB db = new LanguageDB();
            lblOrderStatusRecords.Text = gvOrderStatus.Rows.Count + " " + db.GetLabel("StatsList", "RecordsFound", pageLang);
            if (gvOrderStatus.Rows.Count >= 1999)
            {
                lblOrderStatusRecords.Text = lblOrderStatusRecords.Text + " " + db.GetLabel("StatsList", "ListTruncated", pageLang);
            }

        }//gvOrderStatus_DataBound

        protected void btnPrintReport_Click(object sender, EventArgs e)
        {
            setConstants();
            if (gvOrderStatus.Rows.Count > 0 && gvOrderStatus.Visible)
                Session["OrderStatusTable"] = gvOrderStatus;
            Session["SearchCriteria"] = ddlCriteria.SelectedItem.Text;
            if (ddlCriteria.SelectedIndex != 0)
            {
                if (!string.IsNullOrEmpty(txtLowVal.Text))
                    Session["LowVal"] = txtLowVal.Text;
                if (!string.IsNullOrEmpty(txtHighVal.Text))
                    Session["HighVal"] = txtHighVal.Text;
                Session["OrderGroup"] = ddlGroup.SelectedItem.Text;
                Session["Status"] = ddlStatus.SelectedItem.Text;

            }// if it's not incomplete
            ScriptManager.RegisterStartupScript(this, GetType(), "displayStatusReport", "ShowStatusReport();", true);

        }//btnPrintReport_Click

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            setConstants();
            if (gvOrderStatus.Visible)
            {
                bool ticketSelected = false;
                InboundDB db = new InboundDB();

                foreach (GridViewRow row in gvOrderStatus.Rows)
                {
                    CheckBox chkTicket = (CheckBox)row.FindControl("chkTicket");
                    if (chkTicket.Checked)
                    {
                        LinkButton lnkTicketNum = (LinkButton)row.FindControl("lnkTicketNum");
                        string ticketNum = lnkTicketNum.Text;
                        string user = ddlUser.SelectedValue;
                        ticketSelected = true;
                        db.UpdateAssignedUser(ticketNum, user, db.GetUserGroup(user));
                        db.InsertAssignedToRecord(ticketNum, currentUser, user);
                        gvOrderStatus.DataBind(); //refresh it to show updated value

                    }// if the ticket is checked

                }//foreach row in the gridview
                LanguageDB langDB = new LanguageDB();

                if (!ticketSelected)
                    ScriptManager.RegisterStartupScript(this, GetType(), "assignAlert", "alert('" + langDB.GetLabel("InboundTracking", "MsgSelectTicket", pageLang) + "');", true);

            }//if the gridview is visible

        }//btnAssign_Click

        private void setClientGV(string condition, bool accountFlag, bool postFlag, bool lastFlag, bool orgFlag)
        {
            Session["ClientSearch"] = condition;
            Session["AccountFlag"] = accountFlag;
            Session["PostFlag"] = postFlag;
            Session["LastFlag"] = lastFlag;
            Session["OrgFlag"] = orgFlag;
            gvClientSearch.DataBind();
            if (gvClientSearch.Rows.Count > 0)
                lblSelClientMessage.Visible = true;
            else
                lblSelClientMessage.Visible = false;

        }//setClientGV

        private void showClientDiv(bool show)
        {
            gvClientSearch.SelectedIndex = -1;
            clientSearchDiv.Visible = show;

        }//showClientDiv

        protected void imgbtnZoomLast_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLast.Text.Trim()))
            {
                showClientDiv(true);
                string last = txtLast.Text;
                last = last.Replace("'", "''");
                setClientGV(last, false, false, true, false);
            }

        }//imgbtnZoomLast_Click

        protected void imgbtnZoomOrganization_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOrganization.Text.Trim()))
            {
                showClientDiv(true);
                string organization = txtOrganization.Text;
                organization = organization.Replace("'", "''");
                setClientGV(organization, false, false, false, true);
            }

        }//imgbtnZoomOrganization_Click

        protected void imgbtnZoomPost_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPostZip.Text.Trim()))
            {
                showClientDiv(true);
                setClientGV(txtPostZip.Text, false, true, false, false);
            }

        }//imgbtnZoomPost_Click

        protected void btnClientOK_Click(object sender, EventArgs e)
        {
            if (gvClientSearch.SelectedIndex > -1)
            {
                string acctNum = gvClientSearch.SelectedDataKey.Value.ToString();
                InboundDB db = new InboundDB();
                DataRow custRow = db.SearchCustomers(acctNum, true, false, false, false).Tables[0].Rows[0];
                txtAccount.Text = acctNum;
                txtFirst.Text = custRow["c_firstname_intl"].ToString();
                txtLast.Text = custRow["c_surname"].ToString();
                txtPostZip.Text = custRow["c_postal_code"].ToString();
                txtPhone.Text = custRow["c_telephone"].ToString();
                txtFax.Text = custRow["c_fax_no"].ToString();
                txtOrganization.Text = custRow["c_organization"].ToString();
                txtEmail.Text = custRow["c_email"].ToString();
                try
                {
                    ddlProvState2.SelectedValue = custRow["c_prov_code"].ToString();
                }
                catch (Exception ex)
                {
                    ddlProvState2.SelectedValue = "";
                }
                txtCity.Text = custRow["c_city"].ToString();
                txtCountry.Text = custRow["c_country"].ToString();
                txtAddress.Text = custRow["c_street"].ToString();
                txtAddress2.Text = custRow["c_address_line_2"].ToString();

            }// if there is a selected row in the gridview

            showClientDiv(false);

        }//btnClientOK_Click

        protected void btnClientCancel_Click(object sender, EventArgs e)
        {
            showClientDiv(false);

        }//btnClientCancel_Click

        protected void lnkTicketNum_Click(object sender, EventArgs e)
        {
            LinkButton lnkTicket = (LinkButton)sender;
            Session["TicketNumber"] = lnkTicket.Text;
            Response.Redirect(Request.RawUrl);

        }//lnkTicketNum_Click

        protected void btnPromoCancel_Click(object sender, EventArgs e)
        {
            promotionDiv.Visible = false;

        }//btnPromoCancel_Click

        protected void btnPromoOK_Click(object sender, EventArgs e)
        {
            string promoCode = "";
            if (gvPromotions.SelectedIndex != -1)
            {
                promoCode = gvPromotions.SelectedDataKey.Value.ToString();
                Session["PromoSelected"] = true;
                ddlSource.SelectedValue = promoCode;
            }

            promotionDiv.Visible = false;

        }//btnPromoOK_Click

        protected void btnReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("report.aspx");

        }//btnReports_Click

        protected void btnShipNotes_Click(object sender, EventArgs e)
        {
            if (Session["Action"] != null && (Session["Action"].ToString().Equals("Edit") || (Session["Action"].ToString().Equals("New"))))
            {
                txtShipNotes.Enabled = true;
                txtShipDetails.Enabled = true;
            }
            else
            {
                txtShipNotes.Enabled = false;
                txtShipDetails.Enabled = false;
            }
            ToggleDropDownListVisibility(this.Form.Controls, false);
            shippingNotesDiv.Visible = true;

        }//btnShipNotes_Click

        protected void btnCloseShipping_Click(object sender, EventArgs e)
        {
            ToggleDropDownListVisibility(this.Form.Controls, true);
            shippingNotesDiv.Visible = false;

        }//btnCloseShipping_Click

        protected void btnSaveStatus_Click(object sender, EventArgs e)
        {
            if (rblFinalStatus.SelectedIndex == 0)
            {
                InboundDB db = new InboundDB();
                db.UpdateOrderStatus(txtInqNumber.Text, rblFinalStatus.SelectedValue);
                db.UpdateOrderStatusHist(txtInqNumber.Text, rblFinalStatus.SelectedValue);
                Response.Redirect(Request.RawUrl);
            }

        }//btnSaveStatus_Click

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            setConstants();
            LoginDB db = new LoginDB();
            db.LogUserOut(currentUser);
            Response.Redirect("login.aspx");

        }

        protected void btnAddElectronic_Click(object sender, EventArgs e)
        {
            lblBreadcrumb.Text = btnProduct.Text;
            productSectionDiv.Visible = true;
            inboundDiv.Visible = false;
            knowledgeSection.Visible = false;
            kbDetailsSection.Visible = false;
            subjectSection.Visible = false;
            orderStatusSection.Visible = false;
            btnClose.Visible = true;
            txtClientInfo.Text = txtFirst.Text + " " + txtLast.Text + " " + txtOrganization.Text + " " + ddlProvState2.SelectedValue;
            txtTicketNum.Text = txtInqNumber.Text;
            lblDisplayInfo.Text = "";
            lblProductCount.Text = "";
            txtSearch.Text = "";
            gvInventory.Visible = false;
            gvInventoryElectronic.Visible = true;
            gvInventory.DataSourceID = null;
            LanguageDB langDB = new LanguageDB();
            string newProduct = langDB.GetLabel("InboundTracking", "RepNew", pageLang);

            ddlWH.Visible = false;
            btnDisplayAll.Visible = false;
            btnHot.Visible = false;

            Session["NewProdText"] = newProduct;
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            EmailUtilities eu = new EmailUtilities();
            bool result;
            string strLang;
            string strEmail;
            string strAnswer;
            string strTicket;
            string strProdCode;
            string strPrSeq;
            string strUpdatedAnswerField;
            string strPopUpMessage;
            bool boolProductSent;
            int intRecordCount;
            List<string> lstCodes;
            bool blnAllowAttachments;
            DataSet ds;

            LanguageDB ldb = new LanguageDB();
            InboundDB db = new InboundDB();
            string strNote;
            int c;

            strTicket = txtInqNumber.Text;
            DataSet dsRecCount = db.GetTicketCount(strTicket);
            intRecordCount = Int32.Parse(dsRecCount.Tables[0].Rows[0][0].ToString());


            ds = db.GetControlValue("INBND_EMAIL_AT");
            blnAllowAttachments = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CNTR_VALUE"] != null)
                {
                    if (ds.Tables[0].Rows[0]["CNTR_VALUE"].ToString().ToUpper() == "YES" || ds.Tables[0].Rows[0]["CNTR_VALUE"].ToString().ToUpper() == "TRUE")
                    {
                        blnAllowAttachments = true;
                    }
                }

            }

            if (txtEmail.Text.Trim() == "")
            {
                strPopUpMessage = ldb.GetLabel("InboundTracking", "MessageMissingEmail", pageLang);
                strPopUpMessage = strPopUpMessage.Replace("'", "`");
                ScriptManager.RegisterStartupScript(this, GetType(), "productAlert", "alert('" + strPopUpMessage + "');", true);
            }
            else if ((intRecordCount == 0) || (btnNew.Enabled == false))
            {
                strPopUpMessage = ldb.GetLabel("InboundTracking", "MessageRecordNotSavedBeforeEmail", pageLang);
                strPopUpMessage = strPopUpMessage.Replace("'", "`");
                ScriptManager.RegisterStartupScript(this, GetType(), "productAlert", "alert('" + strPopUpMessage + "');", true);
            }
            else
            {
                strEmail = txtEmail.Text.Replace(" ", "");

                // First, save all messages, if required

                if (Session["TempEProducts"] != null)
                {
                    DataTable tempEProducts = (DataTable)Session["TempEProducts"];
                    foreach (DataRow row in tempEProducts.Rows)
                        SaveElectronicProduct(row);
                }
                Session["TempEProducts"] = null;

                // Get details for sending message

                if (rblLanguage.SelectedIndex == 1)
                {
                    strLang = "f";
                }
                else
                {
                    strLang = "e";
                }
                strAnswer = txtAnswer.Text;


                strNote = ldb.GetLabel("InboundTracking", "EmailSentTo", "EN") + "/" + ldb.GetLabel("InboundTracking", "EmailSentTo", "FR");
                strNote = strNote + ":" + strEmail;

                // Send email

                boolProductSent = false;
                ds = db.GetUnsentProducts(strTicket);
                lstCodes = new List<string>();

                for (c = 0; c < ds.Tables[0].Rows.Count; c++)
                {
                    strProdCode = ds.Tables[0].Rows[c]["PR_INVENTORY_CODE"].ToString();
                    strPrSeq = ds.Tables[0].Rows[c]["PR_SEQUENCE"].ToString();
                    //result = eu.SendEmail(strProdCode, strEmail, strLang, strAnswer);
                    //db.UpdateEmailOrders(strPrSeq, strNote);
                    boolProductSent = true;
                    lstCodes.Add(strProdCode);
                }
                result = eu.SendEmail(lstCodes, strEmail, strLang, strAnswer, strTicket, blnAllowAttachments);

                // Add the email address to the 'sent' field
                strUpdatedAnswerField = txtAnswer.Text;
                if (strUpdatedAnswerField.Length > 0)
                {
                    strUpdatedAnswerField = strUpdatedAnswerField + "\n";
                }
                strUpdatedAnswerField = strUpdatedAnswerField + strNote;
                if (strUpdatedAnswerField.Length > 3999)
                {
                    strUpdatedAnswerField = strUpdatedAnswerField.Substring(0, 3999);
                }
                txtAnswer.Text = strUpdatedAnswerField;
                db.UpdateAnswerField(txtInqNumber.Text, strUpdatedAnswerField);

                if (boolProductSent == true)
                {
                    strPopUpMessage = ldb.GetLabel("InboundTracking", "MessageSentEmail", pageLang);
                    strPopUpMessage = strPopUpMessage.Replace("'", "`");
                    ScriptManager.RegisterStartupScript(this, GetType(), "productAlert", "alert('" + strPopUpMessage + "');", true);
                    if (this.HardcopyProductsOnOrder() == false)
                    {
                        SetDeliveryMethodToEmail();
                        SetEmailOrderStatusToDone();
                        db.UpdateDeliveredDate(txtInqNumber.Text);
                    }
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    strPopUpMessage = ldb.GetLabel("InboundTracking", "MessageNoElectronicProducts", pageLang);
                    strPopUpMessage = strPopUpMessage.Replace("'", "`");
                    ScriptManager.RegisterStartupScript(this, GetType(), "productAlert", "alert('" + strPopUpMessage + "');", true);
                }

            }
        }//btnSendEmail

        private void SetDeliveryMethodToEmail()
        {
            bool boolNonEmailProducts;
            string deliveryMode = ddlDelivery.SelectedValue;
            InboundDB db = new InboundDB();

            // Must first check to see if its only electronic items
            boolNonEmailProducts = HardcopyProductsOnOrder();

            // Set the delivery mode
            if (boolNonEmailProducts == false)
            {
                ddlDelivery.SelectedValue = "EM";
                Session["Delivery"] = ddlDelivery.SelectedValue;

                // Now must save record if its not in edit mode... i.e. the record has already been saved
                if (btnNew.Enabled == true)
                {
                    db.UpdateDeliveryMethod(txtInqNumber.Text, "EM");
                    db.UpdateDeliveredFlag(txtInqNumber.Text, "YES");
                }

            }
        } // SetDeliverMethodToEmail

        private void SetEmailOrderStatusToDone()
        {
            InboundDB db = new InboundDB();
            db.UpdateOrderStatus(txtInqNumber.Text, "099");
            db.UpdateOrderStatusHist(txtInqNumber.Text,"099");
            ddlStatus.SelectedValue = "099";
        } // SetEmailOrderStatusToDone


        private bool HardcopyProductsOnOrder()
        {
   
            int intProdOrderedSaved;
            bool boolNonEmailProducts;
            string deliveryMode = ddlDelivery.SelectedValue;
            InboundDB db = new InboundDB();

            // Must first check to see if its only electronic items
            boolNonEmailProducts = false;
            if (Session["TempProducts"] != null)
            {
                boolNonEmailProducts = true;
            }
            DataSet ProductDS = db.GetOrderedProductCount (txtInqNumber.Text);
            intProdOrderedSaved = Convert.ToInt16(ProductDS.Tables[0].Rows[0][0].ToString());
            if (intProdOrderedSaved > 0)
            {
                boolNonEmailProducts = true;
            }

            return boolNonEmailProducts;

        }// HardcopyProductsOnOrder

        protected void goToClient(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string accounId = btn.CommandArgument;

            Response.Redirect("ContactManagement.aspx?account=" + accounId);
        }

        protected void ddlProvState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strValue;
            strValue = "x";
            ddlProvState2.SelectedIndex = ddlProvState.SelectedIndex;


        }


        private void ToggleDropDownListVisibility(ControlCollection ctls, bool isVisible)
        {
            ddlLine.Visible = isVisible;
            ddlProvState.Visible = isVisible;
            ddlSource.Visible = isVisible;
            ddlCust.Visible = isVisible;
            ddlClass.Visible = isVisible;
            ddlOrderStatus.Visible = isVisible;
            ddlProvState2.Visible = isVisible;
            ddlDelivery.Visible = isVisible;
            ddlWH.Visible = isVisible;
            ddlLineKB.Visible = isVisible;
            ddlOrderKB.Visible = isVisible;
            ddlCriteria.Visible = isVisible;
            ddlGroup.Visible = isVisible;
            ddlStatus.Visible = isVisible;
            ddlUser.Visible = isVisible;
        }

    }//class

}//namespace