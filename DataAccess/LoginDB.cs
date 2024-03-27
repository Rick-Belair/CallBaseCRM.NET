using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DataAccess
{
    public class LoginDB : DAHelper
    {
        private const string appVer10 = "rt%u*$d23^y";
        private const string appVerBld10 = ")(OHSa3!qw@";

        public bool LDAPLogin()
        {
            string ldapLogin = "";
            string strSQL = "select CON_LDAP_LOGON from FCONSTANT";
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                ldapLogin = tempDS.Tables[0].Rows[0]["CON_LDAP_LOGON"].ToString();

            if (ldapLogin.Equals("YES"))
                return true;
            else
                return false;

        }//LDAPLogin

        public DataSet GetUserInfoLAN(string userid)
        {
            string strSQL =
               "select U_ID, U_NAME, U_LEVEL,U_ID_GRP,U_GRP_ACCESS from F_USER_ID where UPPER(U_LAN_UID) = :userid ";


            DbParameter[] parms = new DbParameter[]
            {
                CreateParameter(":userid", DbType.String, userid)
            };

            return Execute(strSQL, CommandType.Text, parms);

        }//GetUserInfoLAN

        public DataSet GetUserInfo(string userid, string pass, string appBldNum10)
        {
            string strSQL =
               "select U_ID, U_NAME, U_LEVEL,U_ID_GRP,U_GRP_ACCESS from F_USER_ID where UPPER(U_ID) = :userid " +
               "and dec_cb_pswd(U_PASSWORD, '" + appVer10 + appVerBld10 + appBldNum10 + "') = :pass";


            DbParameter[] parms = new DbParameter[]
            {
                CreateParameter(":userid", DbType.String, userid),
                CreateParameter(":pass", DbType.String, pass)
            };

            return Execute(strSQL, CommandType.Text, parms);

        }//GetUserInfo

        public int LogUser(string userid)
        {
            int returnVal = 0;
            string strSQL = "insert into f_whos_on(WHO_SEQ, WHO_TIME, WHO_PLATFORM, WHO_USERID) " +
                "values(WHO_SEQ.nextval, sysdate, 'WEB 5.0', '" + userid + "')";
            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//LogUser

        public int LogUserOut(string userid)
        {
            int returnVal = 0;
            string strSQL = "delete from f_whos_on where who_userid ='" + userid + "' and who_seq = (select min (who_seq) from f_whos_on where who_userid ='" + userid + "')";
            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//LogUserOut

    }//class

}//namespace
