using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;

namespace CallBaseMock
{
    public partial class report : System.Web.UI.Page
    {
        public string pageLang = "EN";
        public string userAccess = "";
        public string userGroup = "";
        public int userLevel = 5;
        public string currentUser = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            setConstants();

            if (!IsPostBack)
            {
                if (userLevel > 1)
                    groupSection.Visible = false;
                CommonDB commonDB = new CommonDB();
                DateTime today = DateTime.Parse(commonDB.GetCurrentTime());
                DateTime lastMonth = today.AddMonths(-1);
                txtDateTo.Text = today.ToString("dd-MMM-yyyy");
                txtDateFrom.Text = lastMonth.ToString("dd-MMM-yyyy");

                ReportDB db = new ReportDB();
                InboundDB inbDB = new InboundDB();


                LanguageDB langDB = new LanguageDB();
                setLabels(pageLang);
                string allText = langDB.GetLabel("RepEntry", "All", pageLang);

                DataTable campaignDS = db.GetCampaigns(pageLang).Tables[0];
                DataRow dr = campaignDS.NewRow();
                dr["tele_desc"] = allText;
                campaignDS.Rows.InsertAt(dr, 0);
                gvCampaign.DataSource = campaignDS;
                gvCampaign.DataBind();

                DataTable statusDS = inbDB.GetOrderStatus(pageLang).Tables[0];
                dr = statusDS.NewRow();
                dr["ORDER_STATUS_DESC"] = allText;
                statusDS.Rows.InsertAt(dr, 0);
                gvStatus.DataSource = statusDS;
                gvStatus.DataBind();

                DataTable knowledgeDS = db.GetKBList(pageLang, false, false, false).Tables[0];
                dr = knowledgeDS.NewRow();
                dr["title"] = allText;
                knowledgeDS.Rows.InsertAt(dr, 0);
                gvKB.DataSource = knowledgeDS;
                gvKB.DataBind();

                DataTable refDS = db.GetKBList(pageLang, true, false, false).Tables[0];
                dr = refDS.NewRow();
                dr["title"] = allText;
                refDS.Rows.InsertAt(dr, 0);
                gvRef.DataSource = refDS;
                gvRef.DataBind();

                DataTable adviceDS = db.GetKBList(pageLang, false, false, true).Tables[0];
                dr = adviceDS.NewRow();
                dr["title"] = allText;
                adviceDS.Rows.InsertAt(dr, 0);
                gvQA.DataSource = adviceDS;
                gvQA.DataBind();

                DataTable progDS = db.GetKBList(pageLang, false, true, false).Tables[0];
                dr = progDS.NewRow();
                dr["title"] = allText;
                progDS.Rows.InsertAt(dr, 0);
                gvProg.DataSource = progDS;
                gvProg.DataBind();

                DataTable subjDS = inbDB.GetSubjects(pageLang).Tables[0];
                dr = subjDS.NewRow();
                dr["subject"] = allText;
                subjDS.Rows.InsertAt(dr, 0);
                gvSubjects.DataSource = subjDS;
                gvSubjects.DataBind();

                DataTable groupDS = inbDB.GetGroups(pageLang).Tables[0];
                dr = groupDS.NewRow();
                dr["group_def"] = allText;
                groupDS.Rows.InsertAt(dr, 0);
                gvGroup.DataSource = groupDS;
                gvGroup.DataBind();

            }//IsPostBack

        }//Page_Load

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

        private void setLabels(string lang)
        {
            LanguageDB db = new LanguageDB();
            btnInbound.Text = db.GetLabel("InboundTracking", "btnInboundStats", pageLang);
            lblAANDCHeader.Text = db.GetLabel("Home", "PublicEnquiriesINAC", pageLang);
            lnkLogOut.Text = db.GetLabel("Home", "Logout", pageLang);
            lnkLanguage.Text = db.GetLabel("InboundTracking", "LanguageSwitch", pageLang);
            lblReports.Text = db.GetLabel("SkanList", "Reports2", pageLang);
            lblRepNum.Text = ddlReport.SelectedItem.Text;
            if (ddlReport.SelectedValue.Equals("a"))
                lblRepName.Text = db.GetLabel("RptR19", "R19aName", pageLang);
            else if (ddlReport.SelectedValue.Equals("b"))
                lblRepName.Text = db.GetLabel("RptR19", "R19bName", pageLang);
            else
                lblRepName.Text = db.GetLabel("RptR19", "RepR19cName", pageLang);
            lblReport.Text = db.GetLabel("RepEntry", "Report", lang);
            lblDateRange.Text = db.GetLabel("RepEntry", "DateRange", lang);
            lblFrom.Text = db.GetLabel("RepEntry", "From", lang);
            lblTo.Text = db.GetLabel("RepEntry", "To", lang);
            lblReportLayout.Text = db.GetLabel("RepEntry", "RepLayout", lang);
            rbDetailed.Text = db.GetLabel("RepEntry", "RepDetailed", lang);
            rbSummary.Text = db.GetLabel("RepEntry", "RepSummary", lang);
            lblFilterBy.Text = db.GetLabel("RepEntry", "FilterBy", lang);
            lblCampaignList.Text = db.GetLabel("RepEntry", "CampaignList", lang);
            lblGroupList.Text = db.GetLabel("RepEntry", "GroupList", lang);
            string kbListText = db.GetLabel("RepEntry", "AllKnowledgebaseList", lang);
            ddlFilter.Items[0].Text = kbListText;
            lblKBList.Text = kbListText;
            string progListText = db.GetLabel("RepEntry", "ProgramInfoList", lang);
            ddlFilter.Items[1].Text = progListText;
            lblProgInfoList.Text = progListText;
            string qaListText = db.GetLabel("RepEntry", "QnAList", lang);
            ddlFilter.Items[2].Text = qaListText;
            lblQAList.Text = qaListText;
            string refListText = db.GetLabel("RepEntry", "ReferalList", lang);
            ddlFilter.Items[3].Text = refListText;
            lblRefList.Text = refListText;
            string subjListText = db.GetLabel("RepEntry", "SubjectList", lang);
            ddlFilter.Items[4].Text = subjListText;
            lblSubjList.Text = subjListText;
            lblOrderStatusList.Text = db.GetLabel("RepEntry", "OrderStatusList", lang);
            btnPrintReport.Text = db.GetLabel("RepEntry", "PrintRep", lang);
            btnPrintReport2.Text = db.GetLabel("RepEntry", "PrintRep", lang);
        }//setLabels

        protected string shortenLabel(string label)
        {
            if (label.Length > 80)
                label = Utility.shortenLabel(label, 80);
            return label;

        }//shortenLabel

        protected void btnInbound_Click(object sender, EventArgs e)
        {
            Response.Redirect("inbound.aspx");

        }//btnInbound_Click

        protected void imgbtnCalFrom_Click(object sender, ImageClickEventArgs e)
        {
            if (calDateFrom.Visible)
                calDateFrom.Visible = false;
            else
                calDateFrom.Visible = true;

        }//imgbtnCalFrom_Click

        protected void imgbtnCalTo_Click(object sender, ImageClickEventArgs e)
        {
            if (calDateTo.Visible)
                calDateTo.Visible = false;
            else
                calDateTo.Visible = true;

        }//imgbtnCalTo_Click

        protected void calDateFrom_SelectionChanged(object sender, EventArgs e)
        {
            txtDateFrom.Text = Utility.getDate(calDateFrom.SelectedDate.ToString("dd-MMM-yyyy"));
            calDateFrom.Visible = false;

        }//calDateFrom_SelectionChanged

        protected void calDateTo_SelectionChanged(object sender, EventArgs e)
        {
            txtDateTo.Text = Utility.getDate(calDateTo.SelectedDate.ToString("dd-MMM-yyyy"));
            calDateTo.Visible = false;

        }//calDateTo_SelectionChanged

        protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            setConstants();
            LanguageDB db = new LanguageDB();
            if (ddlReport.SelectedValue.Equals("a"))
            {
                rbSummary.Checked = true;
                rbDetailed.Checked = false;
                lblRepName.Text = db.GetLabel("RptR19", "R19aName", pageLang);
            }
            else if (ddlReport.SelectedValue.Equals("b"))
            {
                rbSummary.Checked = true;
                rbDetailed.Checked = false;
                lblRepName.Text = db.GetLabel("RptR19", "R19bName", pageLang);
            }
            else
            {
                rbDetailed.Checked = true;
                rbSummary.Checked = false;
                lblRepName.Text = db.GetLabel("RptR19", "RepR19cName", pageLang);
            }

            lblRepNum.Text = ddlReport.SelectedItem.Text;

        }//ddlReport_SelectedIndexChanged

        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFilter.SelectedValue.Equals("Ref"))
            {
                knowledgeList.Visible = false;
                refList.Visible = true;
                qaList.Visible = false;
                progList.Visible = false;
                subjList.Visible = false;
            }

            else if (ddlFilter.SelectedValue.Equals("QA"))
            {
                knowledgeList.Visible = false;
                refList.Visible = false;
                qaList.Visible = true;
                progList.Visible = false;
                subjList.Visible = false;
            }

            else if (ddlFilter.SelectedValue.Equals("KB"))
            {
                knowledgeList.Visible = true;
                refList.Visible = false;
                qaList.Visible = false;
                progList.Visible = false;
                subjList.Visible = false;
            }

            else if (ddlFilter.SelectedValue.Equals("Prog"))
            {
                knowledgeList.Visible = false;
                refList.Visible = false;
                qaList.Visible = false;
                progList.Visible = true;
                subjList.Visible = false;
            }

            else
            {
                knowledgeList.Visible = false;
                refList.Visible = false;
                qaList.Visible = false;
                progList.Visible = false;
                subjList.Visible = true;
            }

        }//ddlFilter_SelectedIndexChanged

        protected void btnPrintReport_Click(object sender, EventArgs e)
        {
            ReportDB db = new ReportDB();
            LanguageDB langDB = new LanguageDB();
            setConstants();

            string dateFrom = txtDateFrom.Text;
            string dateTo = txtDateTo.Text;

            string orderStatusList = "";
            string orderDescList = "";
            string groupList = "";
            string groupDescList = "";
            string kbList = "";
            string kbDescList = "";
            string refList = "";
            string refDescList = "";
            string adviceList = "";
            string adviceDescList = "";
            string corpList = "";
            string corpDescList = "";
            string subjList = "";
            string subjDescList = "";
            string campaignList = "";
            string campaignDescList = "";

            for (int i = 0; i < gvStatus.Rows.Count; ++i)
            {
                CheckBox chkStatus = (CheckBox)gvStatus.Rows[i].FindControl("chkStatus");
                if (i == 0 && chkStatus.Checked)
                    break;
                if (chkStatus.Checked)
                {
                    if (!string.IsNullOrEmpty(orderStatusList))
                        orderStatusList += ", ";
                    if (!string.IsNullOrEmpty(orderDescList))
                        orderDescList += ", ";
                    orderStatusList += "'" + gvStatus.DataKeys[i].Value.ToString() + "'";
                    orderDescList += gvStatus.Rows[i].Cells[1].Text;
                }

            }//for the rows in gvStatus

            for (int i = 0; i < gvCampaign.Rows.Count; ++i)
            {
                CheckBox chkCampaign = (CheckBox)gvCampaign.Rows[i].FindControl("chkCampaign");
                if (i == 0 && chkCampaign.Checked)
                    break;
                if (chkCampaign.Checked)
                {
                    if (!string.IsNullOrEmpty(campaignList))
                        campaignList += ", ";
                    if (!string.IsNullOrEmpty(campaignDescList))
                        campaignDescList += ", ";
                    campaignList += "'" + gvCampaign.DataKeys[i].Value.ToString() + "'";
                    campaignDescList += gvCampaign.Rows[i].Cells[1].Text;
                }

            }//for the rows in gvCampaign



            for (int i = 0; i < gvGroup.Rows.Count; ++i)
            {
                CheckBox chkGroup = (CheckBox)gvGroup.Rows[i].FindControl("chkGroup");
                if (i == 0 && chkGroup.Checked)
                    break;
                if (chkGroup.Checked)
                {
                    if (!string.IsNullOrEmpty(groupList))
                        groupList += ", ";
                    if (!string.IsNullOrEmpty(groupDescList))
                        groupDescList += ", ";
                    groupList += "'" + gvGroup.DataKeys[i].Value.ToString() + "'";
                    groupDescList += gvGroup.Rows[i].Cells[1].Text;
                }

            }//for the rows in gvGroup

            if (gvKB.Visible)
            {
                for (int i = 0; i < gvKB.Rows.Count; ++i)
                {
                    CheckBox chkKnowledge = (CheckBox)gvKB.Rows[i].FindControl("chkKnowledge");
                    if (i == 0 && chkKnowledge.Checked)
                        break;
                    if (chkKnowledge.Checked)
                    {
                        if (!string.IsNullOrEmpty(kbList))
                            kbList += ", ";
                        if (!string.IsNullOrEmpty(kbDescList))
                            kbDescList += ", ";
                        kbList += gvKB.DataKeys[i].Value.ToString();
                        Label lblSubject = (Label)gvKB.Rows[i].FindControl("lblSubject");
                        kbDescList += lblSubject.Text;
                    }

                }//for the rows in gvKB

                Session["ReportFilter"] = lblKBList.Text;
                if (string.IsNullOrEmpty(kbDescList))
                    kbDescList = langDB.GetLabel("RepEntry", "All", pageLang);
                Session["ReportVals"] = kbDescList;

            }// if gvKB is visible

            if (gvRef.Visible)
            {
                for (int i = 0; i < gvRef.Rows.Count; i++)
                {
                    CheckBox chkRef = (CheckBox)gvRef.Rows[i].FindControl("chkRef");
                    if (i == 0 && chkRef.Checked)
                        break;
                    if (chkRef.Checked)
                    {
                        if (!string.IsNullOrEmpty(refList))
                            refList += ", ";
                        if (!string.IsNullOrEmpty(refDescList))
                            refDescList += ", ";
                        refList += gvRef.DataKeys[i].Value.ToString();
                        Label lblSubject = (Label)gvRef.Rows[i].FindControl("lblSubject");
                        refDescList += lblSubject.Text;
                    }

                }// for the rows in gvRef

                Session["ReportFilter"] = lblRefList.Text;
                if (string.IsNullOrEmpty(refDescList))
                    refDescList = langDB.GetLabel("RepEntry", "All", pageLang);
                Session["ReportVals"] = refDescList;

            }// if gvRef is visible

            if (gvQA.Visible)
            {
                for (int i = 0; i < gvQA.Rows.Count; i++)
                {
                    CheckBox chkQA = (CheckBox)gvQA.Rows[i].FindControl("chkQA");
                    if (i == 0 && chkQA.Checked)
                        break;
                    if (chkQA.Checked)
                    {
                        if (!string.IsNullOrEmpty(adviceList))
                            adviceList += ", ";
                        if (!string.IsNullOrEmpty(adviceDescList))
                            adviceDescList += ", ";
                        adviceList += gvQA.DataKeys[i].Value.ToString();
                        Label lblSubject = (Label)gvQA.Rows[i].FindControl("lblSubject");
                        adviceDescList += lblSubject.Text;
                    }

                }// for the rows in gvQA

                Session["ReportFilter"] = lblQAList.Text;
                if (string.IsNullOrEmpty(adviceDescList))
                    adviceDescList = langDB.GetLabel("RepEntry", "All", pageLang);
                Session["ReportVals"] = adviceDescList;

            }// if gvQA is visible

            if (gvProg.Visible)
            {
                for (int i = 0; i < gvProg.Rows.Count; i++)
                {
                    CheckBox chkProg = (CheckBox)gvProg.Rows[i].FindControl("chkProg");
                    if (i == 0 && chkProg.Checked)
                        break;
                    if (chkProg.Checked)
                    {
                        if (!string.IsNullOrEmpty(corpList))
                            corpList += ", ";
                        if (!string.IsNullOrEmpty(corpDescList))
                            corpDescList += ", ";
                        corpList += gvProg.DataKeys[i].Value.ToString();
                        Label lblSubject = (Label)gvProg.Rows[i].FindControl("lblSubject");
                        corpDescList += lblSubject.Text;
                    }

                }// for the rows in gvProg

                Session["ReportFilter"] = lblProgInfoList.Text;
                if (string.IsNullOrEmpty(corpDescList))
                    corpDescList = langDB.GetLabel("RepEntry", "All", pageLang);
                Session["ReportVals"] = corpDescList;

            }// if gvProg is visible

            if (gvSubjects.Visible)
            {
                for (int i = 0; i < gvSubjects.Rows.Count; i++)
                {
                    CheckBox chkSubj = (CheckBox)gvSubjects.Rows[i].FindControl("chkSubj");
                    if (i == 0 && chkSubj.Checked)
                        break;
                    if (chkSubj.Checked)
                    {
                        if (!string.IsNullOrEmpty(subjList))
                            subjList += ", ";
                        if (!string.IsNullOrEmpty(subjDescList))
                            subjDescList += ", ";
                        subjList += gvSubjects.DataKeys[i].Value.ToString();
                        Label lblSubject = (Label)gvSubjects.Rows[i].FindControl("lblSubject");
                        subjDescList += lblSubject.Text;
                    }

                }// for the rows in gvSubjects

                Session["ReportFilter"] = lblSubjList.Text;
                if (string.IsNullOrEmpty(subjDescList))
                    subjDescList = langDB.GetLabel("RepEntry", "All", pageLang);
                Session["ReportVals"] = subjDescList;

            }// if gvSubjects is visible

            if (string.IsNullOrEmpty(orderDescList))
                orderDescList = langDB.GetLabel("RepEntry", "All", pageLang);
            if (string.IsNullOrEmpty(groupDescList))
                groupDescList = langDB.GetLabel("RepEntry", "All", pageLang);
            if (string.IsNullOrEmpty(campaignDescList))
                campaignDescList = langDB.GetLabel("RepEntry", "All", pageLang);



            Session["ReportDateRange"] = dateFrom + " - " + dateTo;
            Session["OrderDescList"] = orderDescList;
            Session["GroupDescList"] = groupDescList;
            Session["CampaignDescList"] = campaignDescList;

            Session["ReportDT"] = db.GetReport19(pageLang, dateFrom, dateTo, orderStatusList, groupList, kbList, refList, adviceList, corpList, subjList, campaignList, userLevel,
                userAccess, userGroup).Tables[0];

            if (ddlReport.SelectedValue.Equals("a"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayreport19a", "ShowReport19A();", true);

            }// if it's report a

            else if (ddlReport.SelectedValue.Equals("b"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayreport19b", "ShowReport19B();", true);

            }//if it's report b

            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayreport19c", "ShowReport19C();", true);

            }// need to change this if we add more reports, but right now this would only hit when it's report c

        }//btnPrintReport_Click

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

    }//class

}//namespace