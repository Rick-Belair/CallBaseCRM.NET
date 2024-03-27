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
    public partial class order_history : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string lang = "EN";
            if (Session["PageLanguage"] != null)
                lang = Session["PageLanguage"].ToString();
            InboundDB db = new InboundDB();
            LanguageDB langDB = new LanguageDB();
            tblOrderHistory.Rows[0].Cells[0].Text = langDB.GetLabel("StatsList", "OrderStatus", lang);
            tblOrderHistory.Rows[0].Cells[1].Text = langDB.GetLabel("InboundTracking", "Date", lang);
            tblOrderHistory.Rows[0].Cells[2].Text = langDB.GetLabel("InboundTracking", "User", lang);

            DataSet orderDS = db.GetOrderStatusHistory(Session["TicketNumber"].ToString(), lang);
            foreach (DataRow row in orderDS.Tables[0].Rows)
            {
                TableRow tableRow = new TableRow();
                TableCell orderCell = new TableCell();
                TableCell dateCell = new TableCell();
                TableCell userCell = new TableCell();
                if (row.Table.Columns.Contains("ORDER_STATUS_DESC"))
                    orderCell.Text = row["ORDER_STATUS_DESC"].ToString();
                if (row.Table.Columns.Contains("change_date"))
                    dateCell.Text = row["change_date"].ToString();

                if (row.Table.Columns.Contains("userid"))
                    userCell.Text = row["userid"].ToString();

                tableRow.Cells.Add(orderCell);
                tableRow.Cells.Add(dateCell);
                tableRow.Cells.Add(userCell);

                tblOrderHistory.Rows.Add(tableRow);

            }//foreach row returned

        }//Page_Load

    }//class

}//namespace