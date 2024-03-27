using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;

namespace CallBaseMock.partials
{
    public partial class report_19c : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentUser = "";
            Session["AverageCompTime"] = null;
            if (Session["CurrentUser"] != null)
                currentUser = Session["CurrentUser"].ToString();
            lblUser.Text = currentUser;
            string pageLang = "";
            if (Session["PageLanguage"] != null)
                pageLang = Session["PageLanguage"].ToString();
            string userName = "";
            if (Session["UserName"] != null)
                userName = Session["UserName"].ToString();
            lblUserName.Text = userName;
            Session["TotalHours"] = null;
            Session["TotalDays"] = null;
            LanguageDB langDB = new LanguageDB();
            lblRepHeader.Text = langDB.GetLabel("InboundTracking", "RepHeader", pageLang);
            lblReportTitle.Text = langDB.GetLabel("RptR19", "RepR19cName", pageLang);
            lblReportNum.Text = langDB.GetLabel("RptR19c", "R19cRepRptNo", pageLang) + ": 19c";
            lblPrintedBy.Text = langDB.GetLabel("RptR19a", "R19aRepPrintedBy", pageLang) + ":";
            lblProtectedData.Text = langDB.GetLabel("RptR19a", "R19aRepProtData", pageLang) + ":";
            lblCurrUser.Text = langDB.GetLabel("RptR19a", "R19aRepCurrentUser", pageLang) + ":";
            lblDateRangeText.Text = langDB.GetLabel("RepEntry", "DateRange", pageLang) + ":";
            lblCampaigns.Text = langDB.GetLabel("RepEntry", "RepCampaigns", pageLang) + ":";
            lblOrderStatuses.Text = langDB.GetLabel("RepEntry", "OrderStatuses", pageLang) + ":";
            lblGroups.Text = langDB.GetLabel("RepEntry", "Groups", pageLang) + ":";
            string openTicketText = langDB.GetLabel("RptR19", "R19OpenTickets", pageLang);
            lblOpenTicketsText.Text = openTicketText + ":";
            string doneTicketText = langDB.GetLabel("RptR19", "R19ClosedTickets", pageLang);
            lblDoneTicketsText.Text = doneTicketText + ":";
            lblCompTimeText.Text = langDB.GetLabel("RptR19", "AverageCompTime", pageLang) + ":";
            lbl24Clock.Text = "(" + langDB.GetLabel("RptR19", "24HourClock", pageLang) + ")";
            tblReport.Rows[0].Cells[0].Text = langDB.GetLabel("StatsList", "Assignedto", pageLang) + "/<br /> " + langDB.GetLabel("InboundTracking", "EditAgent", pageLang);
            tblReport.Rows[0].Cells[1].Text = langDB.GetLabel("InboundTracking", "InitialAgent", pageLang);
            tblReport.Rows[0].Cells[2].Text = langDB.GetLabel("InboundTracking", "Ticket", pageLang);
            tblReport.Rows[0].Cells[3].Text = langDB.GetLabel("InboundTracking", "DateInput", pageLang).Replace(":", "");
            tblReport.Rows[0].Cells[4].Text = langDB.GetLabel("RptR19", "R19CurrentStatus", pageLang);
            tblReport.Rows[0].Cells[5].Text = langDB.GetLabel("RptR19", "R19DateAssigned", pageLang);
            tblReport.Rows[0].Cells[6].Text = langDB.GetLabel("RptR19", "R19DateToCallback", pageLang);
            tblReport.Rows[0].Cells[7].Text = langDB.GetLabel("RptR19", "DateCompleted", pageLang);
            tblReport.Rows[0].Cells[8].Text = langDB.GetLabel("RptR19", "TotalHours", pageLang);
            tblReport.Rows[0].Cells[9].Text = langDB.GetLabel("RptR19", "TotalDays", pageLang);
            CommonDB commonDB = new CommonDB();
            DateTime today = DateTime.Parse(commonDB.GetCurrentTime());
            lblDate.Text = today.ToString("MM/dd/yyyy HH:mm");
            if (Session["ReportDateRange"] != null)
                lblDateRange.Text = Session["ReportDateRange"].ToString();
            if (Session["OrderDescList"] != null)
                lblSelStatus.Text = Session["OrderDescList"].ToString();
            if (Session["GroupDescList"] != null)
                lblSelGroup.Text = Session["GroupDescList"].ToString();
            if (Session["CampaignDescList"] != null)
                lblCampaign.Text = Session["CampaignDescList"].ToString();
            if (Session["ReportFilter"] != null)
                lblFilter.Text = Session["ReportFilter"].ToString() + ":";
            if (Session["ReportVals"] != null)
                lblVals.Text = Session["ReportVals"].ToString();

            if (Session["ReportDT"] != null)
            {
                DataTable dt = (DataTable)Session["ReportDT"];
                if (dt.Rows.Count > 0)
                {
                    string rowCssClass = "greyBack blackBorder";
                    TableRow openTicketsRow = new TableRow();
                    openTicketsRow.CssClass = rowCssClass;
                    TableCell openTicketCell = new TableCell();
                    openTicketCell.ColumnSpan = 10;
                    openTicketCell.Text = openTicketText;
                    openTicketCell.CssClass = "fontBold";
                    openTicketsRow.Cells.Add(openTicketCell);

                    TableRow doneTicketRow = new TableRow();
                    doneTicketRow.CssClass = rowCssClass;
                    TableCell doneTicketCell = new TableCell();
                    doneTicketCell.ColumnSpan = 10;
                    doneTicketCell.Text = doneTicketText;
                    doneTicketCell.CssClass = "fontBold";
                    doneTicketRow.Cells.Add(doneTicketCell);

                    DataRow[] closedRows = dt.Select("s_order_status = '099'");
                    DataRow[] openRows = dt.Select("s_order_status <> '099'");
                    openRows = openRows.OrderBy(x => x["edit_user"].ToString()).ThenBy(x => x["ORDER_STATUS_DESC"].ToString()).ThenBy(x => x["s_rec_no"].ToString()).ToArray();
                    closedRows = closedRows.OrderBy(x => x["edit_user"].ToString()).ThenBy(x => x["ORDER_STATUS_DESC"].ToString()).ThenBy(x => x["s_rec_no"].ToString()).ToArray();

                    if (openRows.Count() > 0)
                    {
                        tblReport.Rows.Add(openTicketsRow);
                        populateTable(openRows, true, rowCssClass, today, pageLang);

                    }

                    if (closedRows.Count() > 0)
                    {
                        if (openRows.Count() > 0)
                        {
                            TableRow breakRow = new TableRow();
                            breakRow.CssClass = "breakRow";
                            TableCell blankCell = new TableCell();
                            blankCell.ColumnSpan = 10;
                            blankCell.CssClass = "blankCell";
                            breakRow.Cells.Add(blankCell);
                            tblReport.Rows.Add(breakRow);
                        }
                        tblReport.Rows.Add(doneTicketRow);
                        populateTable(closedRows, false, rowCssClass, today, pageLang);

                    }// if there are closedRows

                    TableRow finalBreakRow = new TableRow();
                    finalBreakRow.CssClass = "finalBreakRow";
                    TableCell finalBlankCell = new TableCell();
                    finalBlankCell.ColumnSpan = 10;
                    finalBlankCell.CssClass = "blankCell";
                    finalBreakRow.Cells.Add(finalBlankCell);
                    tblReport.Rows.Add(finalBreakRow);

                    TableRow totalRow = new TableRow();
                    totalRow.CssClass = "fontBold grandTotal";
                    TableCell totalCountCell = new TableCell();
                    totalCountCell.ColumnSpan = 8;
                    int totalCount = openRows.Count() + closedRows.Count();
                    string grandTotal = langDB.GetLabel("RptR10", "GrandTotal", pageLang);
                    totalCountCell.Text = grandTotal + " = " + totalCount;
                    totalRow.Cells.Add(totalCountCell);
                    double totalHours = 0;
                    double totalDays = 0;
                    if (Session["TotalHours"] != null)
                        totalHours = double.Parse(Session["TotalHours"].ToString());
                    if (Session["TotalDays"] != null)
                        totalDays = double.Parse(Session["TotalDays"].ToString());
                    TableCell totalHoursCell = new TableCell();
                    totalHoursCell.Text = totalHours.ToString();
                    totalRow.Cells.Add(totalHoursCell);
                    TableCell totalDaysCell = new TableCell();
                    totalDaysCell.Text = totalDays.ToString();
                    totalRow.Cells.Add(totalDaysCell);
                    tblReport.Rows.Add(totalRow);

                    lblOpenTickets.Text = openRows.Count().ToString();
                    lblDoneTickets.Text = closedRows.Count().ToString();
                    if (Session["AverageCompTime"] != null)
                        lblCompTime.Text = Session["AverageCompTime"].ToString() + " " + langDB.GetLabel("RptR19", "R19Hours", pageLang) + " ";
                    else
                        lblCompTime.Text = "0";

                }//if there are any tickets for the params passed
                else
                {
                    hasRowsDiv.Visible = false;
                }

            }//if the session exists
            else
            {
                hasRowsDiv.Visible = false;
            }

        }//Page_Load

        private void populateTable(DataRow[] ticketRows, bool open, string rowCssClass, DateTime today, string lang)
        {
            LanguageDB langDB = new LanguageDB();
            double averageUserTime = 0;
            double averageCompTime = 0;
            int userCount = 0;
            double totalHours = 0;
            double totalDays = 0;
            string prevUser = "";
            int i = 0;
            bool sameAsPrev = false;
            bool sameAsNext = false;
            foreach (DataRow row in ticketRows)
            {
                TableRow tableRow = new TableRow();
                TableRow detailsRow = new TableRow();
                TableRow answerRow = new TableRow();
                tableRow.CssClass = "blackBorder";
                ++userCount;

                TableCell editAgentCell = new TableCell();
                string editAgent = "";
                if (!string.IsNullOrEmpty(row["edit_user"].ToString()))
                    editAgent = row["edit_user"].ToString();
                if (string.IsNullOrEmpty(editAgent))
                    editAgent = langDB.GetLabel("RptR19", "R19Unassigned", lang);
                editAgentCell.Text = editAgent;

                if (editAgent.Equals(prevUser))
                    sameAsPrev = true;
                else
                    sameAsPrev = false;

                if (!sameAsPrev)
                {
                    TableRow userHeadRow = new TableRow();
                    userHeadRow.CssClass = rowCssClass;
                    TableCell userHeadCell = new TableCell();
                    userHeadCell.ColumnSpan = 10;
                    userHeadCell.CssClass = "fontBold";
                    userHeadRow.Cells.Add(userHeadCell);
                    userHeadCell.Text = langDB.GetLabel("RptR19", "OrdersForAgent", lang) + " ";
                    userHeadCell.Text += editAgent;
                    tblReport.Rows.Add(userHeadRow);

                }// if the user isn't the same as the prev user

                tableRow.Cells.Add(editAgentCell);

                TableCell origAgentCell = new TableCell();
                origAgentCell.Text = row["orig_user"].ToString();
                tableRow.Cells.Add(origAgentCell);

                TableCell ticketNumCell = new TableCell();
                ticketNumCell.Text = row["s_rec_no"].ToString();
                tableRow.Cells.Add(ticketNumCell);

                TableCell dateInputCell = new TableCell();
                string inputDate = row["date_in"].ToString();
                dateInputCell.Text = inputDate;
                tableRow.Cells.Add(dateInputCell);

                TableCell statusCell = new TableCell();
                statusCell.Text = row["ORDER_STATUS_DESC"].ToString();
                tableRow.Cells.Add(statusCell);

                TableCell dateAssignedCell = new TableCell();
                dateAssignedCell.Text = row["date_assigned"].ToString();
                tableRow.Cells.Add(dateAssignedCell);

                TableCell dateBackCell = new TableCell();
                dateBackCell.Text = row["date_back"].ToString();
                tableRow.Cells.Add(dateBackCell);

                TableCell dateCompletedCell = new TableCell();
                tableRow.Cells.Add(dateCompletedCell);

                TableCell totalHoursCell = new TableCell();
                DateTime inputDateTime = DateTime.Parse(inputDate);
                TimeSpan dateDifference;
                if (open)
                {

                    dateDifference = today - inputDateTime;
                }
                else
                {
                    string statusDate = row["status_date"].ToString();
                    DateTime statusDateTime = DateTime.Parse(statusDate);
                    dateCompletedCell.Text = statusDate;
                    dateDifference = statusDateTime - inputDateTime;
                    averageCompTime += dateDifference.TotalHours;
                }

                double userTotalHours = Math.Round((dateDifference).TotalHours, 2);
                totalHours += userTotalHours;
                averageUserTime += userTotalHours;
                totalHoursCell.Text = userTotalHours.ToString();
                tableRow.Cells.Add(totalHoursCell);

                TableCell totalDaysCell = new TableCell();
                double userTotalDays = Math.Round((dateDifference).TotalDays, 2);
                totalDays += userTotalDays;
                totalDaysCell.Text = userTotalDays.ToString();
                tableRow.Cells.Add(totalDaysCell);

                tblReport.Rows.Add(tableRow);

                if (!string.IsNullOrEmpty(row["s_comments"].ToString()))
                {
                    TableCell detailBlank = new TableCell();
                    detailBlank.ColumnSpan = 2;
                    detailsRow.Cells.Add(detailBlank);
                    string details = row["s_comments"].ToString();
                    TableCell detailCell = new TableCell();
                    detailCell.ColumnSpan = 8;
                    detailCell.Text = langDB.GetLabel("Task", "Details", lang) + ": " + details;
                    detailsRow.Cells.Add(detailCell);
                    tblReport.Rows.Add(detailsRow);

                }// if there are comments

                if (!string.IsNullOrEmpty(row["s_open_answer"].ToString()))
                {
                    TableCell answerBlank = new TableCell();
                    answerBlank.ColumnSpan = 2;
                    answerRow.Cells.Add(answerBlank);
                    string answer = row["s_open_answer"].ToString();
                    TableCell answerCell = new TableCell();
                    answerCell.ColumnSpan = 8;
                    answerCell.Text = langDB.GetLabel("RptR1b", "R1bRepAnswer", lang) + ": " + answer;
                    answerRow.Cells.Add(answerCell);
                    tblReport.Rows.Add(answerRow);

                }// if there is an answer

                try
                {
                    string nextUser = ticketRows.ElementAt(i + 1)["edit_user"].ToString();
                    if (string.IsNullOrEmpty(nextUser))
                        nextUser = langDB.GetLabel("RptR19", "R19Unassigned", lang);
                    if (editAgent.Equals(nextUser))
                        sameAsNext = true;
                    else
                        sameAsNext = false;

                }// try to find a next user

                catch (Exception ex)
                {
                    sameAsNext = false;
                }

                if (!sameAsNext)
                {
                    TableRow userSubRow = new TableRow();
                    userSubRow.CssClass = rowCssClass;
                    TableCell userSubCell = new TableCell();
                    userSubCell.CssClass = "fontBold reportUserTotalCell reportUserTotalRow";
                    userSubRow.Cells.Add(userSubCell);
                    userSubCell.Text = langDB.GetLabel("RptR19", "TotalForAgent", lang) + " ";
                    userSubCell.Text += editAgent + " = " + userCount;
                    userSubCell.ColumnSpan = 7;

                    averageUserTime = Math.Round((averageUserTime / userCount), 2);
                    TableCell averageTimeTextCell = new TableCell();
                    averageTimeTextCell.CssClass = "reportUserTotalRow";
                    TableCell averageTimeCell = new TableCell();
                    averageTimeCell.CssClass = "reportUserTotalRow";
                    averageTimeCell.ColumnSpan = 2;
                    averageTimeTextCell.Text = langDB.GetLabel("RptR19", "R19AverageTimeInHours", lang) + ":";
                    averageTimeCell.Text = averageUserTime.ToString();
                    userSubRow.Cells.Add(averageTimeTextCell);
                    userSubRow.Cells.Add(averageTimeCell);

                    tblReport.Rows.Add(userSubRow);
                    averageUserTime = 0;
                    userCount = 0;

                }// if the user isn't the same as the next user (or if there isn't a next user)

                ++i;
                prevUser = editAgent;

            }//foreach ticket

            TableRow subTotalRow = new TableRow();
            subTotalRow.CssClass = rowCssClass;
            TableCell subTotalCell = new TableCell();
            subTotalCell.ColumnSpan = 8;
            subTotalCell.Text = langDB.GetLabel("RptR19a", "R19aSubTotal", lang) + " = " + ticketRows.Count();
            subTotalCell.CssClass = "fontBold";
            subTotalRow.Cells.Add(subTotalCell);

            TableCell subTotalHoursCell = new TableCell();
            subTotalHoursCell.Text = totalHours.ToString();
            subTotalHoursCell.CssClass = "fontBold";
            subTotalRow.Cells.Add(subTotalHoursCell);

            TableCell subTotalDaysCell = new TableCell();
            subTotalDaysCell.Text = totalDays.ToString();
            subTotalDaysCell.CssClass = "fontBold";
            subTotalRow.Cells.Add(subTotalDaysCell);

            tblReport.Rows.Add(subTotalRow);

            if (Session["TotalHours"] != null)
                Session["TotalHours"] = Double.Parse(Session["TotalHours"].ToString()) + totalHours;
            else
                Session["TotalHours"] = totalHours;

            if (Session["TotalDays"] != null)
                Session["TotalDays"] = Double.Parse(Session["TotalDays"].ToString()) + totalDays;
            else
                Session["TotalDays"] = totalDays;

            if (!open)
            {
                averageCompTime = Math.Round(averageCompTime / ticketRows.Count(), 2);
                Session["AverageCompTime"] = averageCompTime;
            }

        }//populateTable

    }//class 

}//namespace
