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
    public partial class issue_history : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Issues"] != null)
                txtCurrentComment.Text = Session["Issues"].ToString();
            if (Session["TicketNumber"] != null)
            {
                string ticketnumber = Session["TicketNumber"].ToString();
                lblTicket.Text = ticketnumber;
             //   InboundDB db = new InboundDB();
              //  DataSet commentDS = db.GetCommentHistory(ticketnumber);
             //   loadCommentHistory(commentDS);
            }

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
            

        }//Page_Load

        protected void gvCommentHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvCommentHistory.SelectedRow;
            string issue = row.Cells[3].Text;
            if (!issue.ToLower().Equals("&nbsp;"))
                txtSelComment.Text = row.Cells[3].Text;
            else
                txtSelComment.Text = "";
        }

        //private void loadCommentHistory(DataSet ds)
        //{
        //    DataRowCollection commentRows = ds.Tables[0].Rows;

        //    foreach (DataRow row in commentRows)
        //    {
        //        TableRow tableRow = new TableRow();
        //        TableCell dateCell = new TableCell();
        //        TableCell userCell = new TableCell();
        //        TableCell commentCell = new TableCell();

        //        LinkButton dateLink = new LinkButton();
        //        LinkButton userLink = new LinkButton();
        //        LinkButton commentLink = new LinkButton();

        //        if (row.Table.Columns.Contains("cah_date_edit"))
        //        {
        //            dateLink.Text = row["cah_date_edit"].ToString();
        //            dateLink.Click += comment_Click;
        //            dateCell.Controls.Add(dateLink);
        //        }

        //        tableRow.Cells.Add(dateCell);

        //        if (row.Table.Columns.Contains("cah_user_edit"))
        //        {
        //            userLink.Text = row["cah_user_edit"].ToString();
        //            userLink.Click += comment_Click;
        //            userCell.Controls.Add(userLink);
        //        }

        //        tableRow.Cells.Add(userCell);

        //        if (row.Table.Columns.Contains("cah_comments"))
        //        {
        //            commentLink.Text = row["cah_comments"].ToString();
        //            commentLink.Click += comment_Click;
        //            commentCell.Controls.Add(commentLink);
        //        }

        //        tableRow.Cells.Add(commentCell);

        //        tblCommentHistory.Rows.Add(tableRow);


        //    }//foreach row returned

        //}//loadCommentHistory

        //protected void comment_Click(object sender, EventArgs e)
        //{
        //    bool found = false;
        //    for (int i = 1; i < tblCommentHistory.Rows.Count; ++i )
        //    {
        //        if (found) break;
        //        foreach (TableCell cell in tblCommentHistory.Rows[i].Cells)
        //        {
        //            LinkButton lnkBtn = (LinkButton)cell.Controls[0];
        //            LinkButton senderBtn = (LinkButton)sender;
        //            if (senderBtn.Text.ToString().Contains(lnkBtn.Text))
        //            {
        //                LinkButton commentBtn = (LinkButton)tblCommentHistory.Rows[i].Cells[2].Controls[0];
        //                txtSelComment.Text = commentBtn.Text.ToString();
        //                found = true;
        //                break;
        //            }// if the button that was clicked matches the current one in the loop

        //        }//for each cell in the row

        //    }// for each row other than the header row

        //}//comment_Click

    }//class

}//namespace