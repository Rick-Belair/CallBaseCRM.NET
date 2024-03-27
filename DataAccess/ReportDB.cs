using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DataAccess
{
    public class ReportDB : DAHelper
    {
        public DataSet GetCampaigns(string language)
        {
            string strSQL = "select tele_no, ";
            if (language.Equals("FR"))
                strSQL += "tele_desc_fr as tele_desc";
            else
                strSQL += "tele_desc";

            strSQL += " from f_tele where tele_nonactive_campaign = 0 order by tele_desc";

            return Execute(strSQL, CommandType.Text);

        }//GetCampaigns

        public DataSet GetKBList(string language, bool refTo, bool progTo, bool adviceTo)
        {
            string strSQL = "select ref_seq, ";
            if (language.Equals("FR"))
                strSQL += "ref_fr_instr as title ";
            else
                strSQL += "ref_title as title ";

            strSQL += "from freference ";

            if (refTo)
                strSQL += "where ref_referral = 'YES'";
            if (progTo)
                strSQL += "where ref_prog_info = 'YES'";
            if (adviceTo)
                strSQL += "where ref_qa = 'YES'";

            strSQL += " order by UPPER(title)";

            return Execute(strSQL, CommandType.Text);

        }//GetKBList

        public DataSet GetReport19(string language, string dateFrom, string dateTo, string orderStatusList, string groupList, string kbList, string refList, string adviceList,
            string corpList, string subjList, string campaignList, int userLevel, string userAccess, string userGroup)
        {
            string strSQL = "SELECT UPPER(s_user_edit) as edit_user, UPPER(s_operator) as orig_user, s_rec_no, to_char(S_DATE_INPUT, 'DD-MON-YYYY HH24:MI') as date_in, " +
            "to_char(s_order_status_date, 'DD-MON-YYYY HH24:MI') as status_date, s_order_status, s_advice_to, s_corp_info_to, s_ref_to, s_subj_string, " +
            " (SELECT TO_CHAR(MAX (cah_date_edit), 'DD-MON-YYYY HH24:MI') FROM comment_audit_history WHERE cah_rec_no = s_rec_no and UPPER(cah_comments) like '%ASSIGNED TO%') as " +
            "date_assigned, to_char(S_DATE_TO_CALLBACK, 'DD-MON-YYYY HH24:MI') as date_back, s_comments, s_open_answer, ";
            if (language.Equals("FR"))
                strSQL += "ORDER_STATUS_DESC_F as ORDER_STATUS_DESC ";
            else
                strSQL += "ORDER_STATUS_DESC_E as ORDER_STATUS_DESC ";

            strSQL += " FROM fstatistics join f_order_status on s_order_status = order_status_code " +
            "WHERE s_date_input IS NOT NULL and s_order_status is not null and s_order_status_date is not null";

            strSQL += " and s_date_input >= TO_DATE('" + dateFrom + "','DD-MON-YYYY') and s_date_input <= TO_DATE('" + dateTo + " 23:59','DD-MON-YYYY HH24:MI')";

            if (!string.IsNullOrEmpty(orderStatusList))
                strSQL += " and (s_order_status in(" + orderStatusList + ") or (SELECT COUNT(oshist_srecno) FROM f_order_status_history WHERE oshist_status_code IN(" + orderStatusList
            + ") and oshist_srecno = s_rec_no) >0)";

            if (!string.IsNullOrEmpty(groupList))
                strSQL += " and s_owner in (" + groupList + ")";
            else
            {
                if (userAccess.Contains(','))
                    userAccess = getGroupsAllowed(userAccess);
                else
                    userAccess = "'" + userAccess + "'";

                if (userLevel == 2)
                    strSQL += " and s_owner = '" + userGroup + "'";
                else
                    strSQL += " and s_owner in (" + userAccess + ")";
            }

            if (!string.IsNullOrEmpty(campaignList))
                strSQL += " and s_tele_used in (" + campaignList + ")";

            strSQL += "order by edit_user desc, s_order_status, s_date_input";

            DataSet Report19 = Execute(strSQL, CommandType.Text);

            if (!string.IsNullOrEmpty(kbList) || !string.IsNullOrEmpty(refList) || !string.IsNullOrEmpty(adviceList) || !string.IsNullOrEmpty(corpList) || !string.IsNullOrEmpty(subjList))
            {
                DataTable myTable = Report19.Tables[0].Clone();
                myTable.Rows.Clear();
                if (!string.IsNullOrEmpty(kbList))
                {
                    string[] kbArray = kbList.Split(',');
                    foreach (string kbRec in kbArray)
                    {
                        var myquery = Report19.Tables[0].AsEnumerable().Where(dr => dr["s_ref_to"].ToString().Contains(kbRec.Trim()) || dr["s_advice_to"].ToString().Contains(kbRec.Trim()) ||
                            dr["s_corp_info_to"].ToString().Contains(kbRec.Trim()));
                        if (myquery.Count() > 0)
                        {
                            DataTable tempTable = myquery.CopyToDataTable();
                            foreach (DataRow row in tempTable.Rows)
                                myTable.ImportRow(row);
                        }
                    }

                }// if there's a kblist

                if (!string.IsNullOrEmpty(refList))
                {
                    string[] refArray = refList.Split(',');
                    foreach (string refRec in refArray)
                    {
                        var myquery = Report19.Tables[0].AsEnumerable().Where(dr => dr["s_ref_to"].ToString().Contains(refRec.Trim()));
                        if (myquery.Count() > 0)
                        {
                            DataTable tempTable = myquery.CopyToDataTable();
                            foreach (DataRow row in tempTable.Rows)
                                myTable.ImportRow(row);
                        }
                    }

                }// if there's a refList

                if (!string.IsNullOrEmpty(adviceList))
                {
                    string[] adviceArray = adviceList.Split(',');
                    foreach (string adviceRec in adviceArray)
                    {
                        var myquery = Report19.Tables[0].AsEnumerable().Where(dr => dr["s_advice_to"].ToString().Contains(adviceRec.Trim()));
                        if (myquery.Count() > 0)
                        {
                            DataTable tempTable = myquery.CopyToDataTable();
                            foreach (DataRow row in tempTable.Rows)
                                myTable.ImportRow(row);
                        }
                    }

                }// if there's an adviceList

                if (!string.IsNullOrEmpty(corpList))
                {
                    string[] corpArray = corpList.Split(',');
                    foreach (string corpRec in corpArray)
                    {
                        var myquery = Report19.Tables[0].AsEnumerable().Where(dr => dr["s_corp_info_to"].ToString().Contains(corpRec.Trim()));
                        if (myquery.Count() > 0)
                        {
                            DataTable tempTable = myquery.CopyToDataTable();
                            foreach (DataRow row in tempTable.Rows)
                                myTable.ImportRow(row);
                        }
                    }

                }// if there's a corpList

                if (!string.IsNullOrEmpty(subjList))
                {
                    string[] subjArray = subjList.Split(',');
                    foreach (string subjRec in subjArray)
                    {
                        var myquery = Report19.Tables[0].AsEnumerable().Where(dr => dr["s_subj_string"].ToString().Contains(subjRec.Trim()));
                        if (myquery.Count() > 0)
                        {
                            DataTable tempTable = myquery.CopyToDataTable();
                            foreach (DataRow row in tempTable.Rows)
                                myTable.ImportRow(row);
                        }
                    }

                }// if there's a subjList

                Report19.Tables.Clear();
                Report19.Tables.Add(myTable);

            }// if any of kblist, reflist, advicelist, corplist, subjlist are not empty

            return Report19;

        }//GetReport19

        private string getGroupsAllowed(string userAccess)
        {
            string groupsAllowed = "";
            string[] splitGroups = userAccess.Split(',');

            if (splitGroups.Length > 2)
            {
                for (int i = 0; i < splitGroups.Length - 1; ++i)
                {
                    groupsAllowed += "'" + splitGroups[i].Trim() + "'";
                    if (i != splitGroups.Length - 2)
                        groupsAllowed += ", ";

                }// for the number of items seperated by commas, minus 1 since the last will be blank

            }// if there is more than 2 items (unless the field is empty, there is at least one item seperated by a comma, which creates two items)

            return groupsAllowed;

        }//getGroupsAllowed

    }//class

}//namespace
