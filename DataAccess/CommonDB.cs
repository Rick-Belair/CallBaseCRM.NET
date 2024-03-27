using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
namespace DataAccess
{
    public class CommonDB : DAHelper
    {
        public string GetCurrentTime()
        {
            string strSQL = "select sysdate from dual";
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                return tempDS.Tables[0].Rows[0]["sysdate"].ToString();
            else
                return DateTime.Today.ToString();

        }//GetCurrentTime
    
    }//class

}//namespace
