using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DataAccess
{
    public class WCMSDB : DAHelper
    {
        public DataSet GetPageNames()
        {
            string strSQL = "select distinct label_pagename from f_html_labels order by label_pagename";

            return Execute(strSQL, CommandType.Text);

        }//GetPageNames

        public DataSet GetLabels(string page, string condition)
        {

            string strSQL = "select distinct lang_code, lang_en, lang_fr from f_html_language join f_html_labels on label_langcode = lang_code";
            string conditionSearch = "(lower(lang_en) like '%" + condition + "%' or lower(lang_code) like '%" + condition + "%' or lower(lang_fr) like '%" + condition + "%')";
            if (!string.IsNullOrEmpty(page))
            {
                strSQL += " where label_pagename = '" + page + "'";

                if (!string.IsNullOrEmpty(condition))
                    strSQL += " and " + conditionSearch;
            }

            else
            {
                if (!string.IsNullOrEmpty(condition))
                    strSQL += " where " + conditionSearch;
            }

            strSQL += " order by lang_code";

            return Execute(strSQL, CommandType.Text);

        }//GetLabels

        public int UpdateLabel(string code, string langEN, string langFR)
        {
            string strSQL = "update f_html_language set lang_en ='" + langEN + "', lang_fr ='" + langFR +
                "' where lang_code = '" + code + "'";

            return ExecuteNonQuery(strSQL, CommandType.Text);
        }

    }//class

}//namespace
