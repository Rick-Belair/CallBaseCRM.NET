using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data;

namespace CallBaseMock.partials
{
    public partial class status_report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentUser = "";
            if (Session["CurrentUser"] != null)
                currentUser = Session["CurrentUser"].ToString();
            string lang = "";
            if (Session["PageLanguage"] != null)
                lang = Session["PageLanguage"].ToString();
            lblUser.Text = currentUser;
            string userName = "";
            if (Session["UserName"] != null)
                userName = Session["UserName"].ToString();
            lblUserName.Text = userName;
            setLabels(lang);
            CommonDB commonDB = new CommonDB();
            DateTime today = DateTime.Parse(commonDB.GetCurrentTime());
            lblDate.Text = today.ToString("MM/dd/yyyy");
            lblDate2.Text = lblDate.Text;
            LanguageDB langDB = new LanguageDB();
            lblRepHeader.Text = langDB.GetLabel("InboundTracking", "RepHeader", lang);
            GridView gvOrderStatus;
            string ticketNum;
            string cname;
            InboundDB inboundDB = new InboundDB();

            if (Session["OrderStatusTable"] != null)
            {
                string criteria = "";
                if (Session["SearchCriteria"] != null)
                    criteria = Session["SearchCriteria"].ToString();
                if (criteria.Equals("Incomplete"))
                {
                    groupRow.Visible = false;
                    statusRow.Visible = false;
                    lblCriteria.Text = langDB.GetLabel("StatsList", "StatsListCriteria", lang) + ":";
                    lblVals.Text = langDB.GetLabel("StatsList", "StatlistShowAllWork", lang);
                }
                else
                {
                    lblCriteria.Text = criteria + " " + langDB.GetLabel("RepEntry", "RepRange", lang) + ":"; //need to change this to be one label (date range exists but not ticket) 

                    string lowVal = "";
                    if (Session["LowVal"] != null)
                        lowVal = Session["LowVal"].ToString();
                    string highVal = "";
                    if (Session["HighVal"] != null)
                        highVal = Session["HighVal"].ToString();
                    if (!string.IsNullOrEmpty(lowVal) && !string.IsNullOrEmpty(highVal))
                        lblVals.Text = lowVal + " - " + highVal;
                    else
                    {
                        if (!string.IsNullOrEmpty(lowVal))
                            lblVals.Text = ">= " + lowVal;  //perhaps doing this will be better
                        // lblVals.Text = "greater than or equal to " + lowVal; //need to replace the message with a label
                        if (!string.IsNullOrEmpty(highVal))
                            lblVals.Text = "<= " + highVal;
                        // lblVals.Text = "less than or equal to " + highVal; //need to replace the message with a label

                    }// if one of the vals is not entered

                    if (Session["OrderGroup"] != null)
                        lblGroup.Text = Session["OrderGroup"].ToString();

                    if (Session["Status"] != null)
                        lblStatus.Text = Session["Status"].ToString();

                }// if it's not the incomplete criteria

                gvOrderStatus = (GridView)Session["OrderStatusTable"];
                TableHeaderRow headerRow = new TableHeaderRow();

                foreach (DataControlField header in gvOrderStatus.Columns)
                {
                    TableHeaderCell headerCell = new TableHeaderCell();
                    if (gvOrderStatus.SortExpression.Equals(header.SortExpression))
                    {
                        if (gvOrderStatus.SortDirection == SortDirection.Ascending)
                            headerCell.CssClass = "sortasc";
                        else
                            headerCell.CssClass = "sortdesc";
                    }
                    headerCell.Text = header.HeaderText;
                    headerRow.Cells.Add(headerCell);

                }//foreach gridview header

                tblOrderStatus.Rows.Add(headerRow);

                int rowCount = 0;
                ticketNum = "0";
                foreach (GridViewRow row in gvOrderStatus.Rows)
                {
                    TableRow tableRow = new TableRow();
                    rowCount++;
                    for (int i = 0; i < row.Cells.Count; ++i)
                    {
                        TableCell tableCell = new TableCell();
                        if (i == 0) //this is the checkbox field, in the report it's the row count
                            tableCell.Text = rowCount.ToString();
                        else
                        {
                            if (i == 1)
                            {
                                LinkButton lnkTicketNum = (LinkButton)row.Cells[i].FindControl("lnkTicketNum");
                                tableCell.Text = lnkTicketNum.Text;
                                ticketNum = lnkTicketNum.Text;
                            }
                            else if (i == 4)  // Customer name, handled separately
                            {
                                string strCname = inboundDB.GetOrderCNAME(ticketNum);
                                if (strCname.Length > 12)
                                {
                                    tableCell.Text = strCname.Substring(0, 12) + "...";
                                }
                                else
                                {
                                    tableCell.Text = strCname;
                                }

                            }
                            else
                                tableCell.Text = row.Cells[i].Text;

                        }// if it's not the first cell

                        tableRow.Cells.Add(tableCell);

                    }//for all the cells

                    tblOrderStatus.Rows.Add(tableRow);

                }//foreach row in the gridview

                lblNumRows.Text = rowCount + " " + langDB.GetLabel("StatsList", "RecordsFound", lang);

                Session["OrderStatusTable"] = null;

            }// if the OrderStatusTable session has been set

            else
            {
                noRowsDiv.Visible = true;
                hasRowsDiv.Visible = false;
            }


        }//Page_Load

        private void setLabels(string lang)
        {
            LanguageDB db = new LanguageDB();
            lblStatusHeader.Text = db.GetLabel("StatsList", "StatsListMessage", lang);
            lblReportTitle.Text = db.GetLabel("StatsList", "ListOfStats", lang);
            string protectedData = db.GetLabel("StatsList", "StatsListProtectedData", lang);
            lblProtectedData.Text = protectedData;
            lblProtectedData2.Text = protectedData;
            lblCurrUser.Text = db.GetLabel("StatsList", "StatsListCurrentUser", lang);
            lblGroupText.Text = db.GetLabel("StatsList", "Group", lang) + ":";
            lblStatusText.Text = db.GetLabel("StatsList", "StatslistDisplay", lang) + ":";

        }//setLabels

    }//class

}//namespace