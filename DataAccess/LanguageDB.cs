using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DataAccess
{
    public class LanguageDB : DAHelper
    {
        public string GetLabel(string pageName, string labelCode, string language)
        {
            string label = "";
            string strSQL = "select lang_" + language + " as labelText from f_html_language join f_html_labels on lang_code = label_langcode ";
            strSQL += "where UPPER(label_pagename) = '" + pageName.ToUpper() + "' and UPPER(lang_code) = '" + labelCode.ToUpper() + "'"; 
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                label = tempDS.Tables[0].Rows[0]["labelText"].ToString();
            return label;

        }//GetLabel

    }//class

}//namespace
