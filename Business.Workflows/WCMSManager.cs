using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using DataAccess;

namespace Business.Workflows
{
    public class WCMSManager
    {
        private WCMSDB db = new WCMSDB();

        public DataSet GetPageNames()
        {
            return db.GetPageNames();

        }//GetPageNames

        public DataSet GetLabels(string page, string condition)
        {
            return db.GetLabels(page, condition);

        }//GetLabels

        public int UpdateLabel(string lang_code, string lang_en, string lang_fr)
        {
            return db.UpdateLabel(lang_code.Replace("'", "''"), lang_en.Replace("'", "''"), lang_fr.Replace("'", "''"));

        }//UpdateLabel

    }//class

}//namespace
