using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Web;

namespace DataAccess
{
    public class ClientDB : DAHelper
    {
        public DataSet GetClientDetails(string accountNumber)
        {
            string strSQL = "select C_REC_NO, C_LANGUAGE, C_SALUTATION, " +
                     "C_FIRSTNAME_INTL, C_SURNAME, C_JOB_TITLE, C_ORGANIZATION, " +
                     "C_STREET, C_ADDRESS_LINE_2, C_PROV_CODE, C_PROVINCE_NAME, " +
                     "C_CITY, C_POSTAL_CODE, C_TELEPHONE, C_FAX_NO, C_EMAIL, " +
                     "C_COUNTRY, C_WWW, C_CUSTOMER_TYPE, C_STATUS, C_POSTAL_CODE, C_DEL_MODE, " +
                     "to_char(C_DATE_INPUT, 'DD-MON-YYYY') as C_DATE_INPUT, to_char(C_DATE_AMENDED, 'DD-MON-YYYY') as C_DATE_AMENDED, " +
                     "C_OPERATOR, C_OWNER, C_USER_GRP, to_char(C_DATE_USED, 'DD-MON-YYYY') as C_DATE_USED" +
                     " from FCLIENT where C_REC_NO = :accountNumber";

            DbParameter[] parms = new DbParameter[]
            {
                CreateParameter(":accountNumber", DbType.String, accountNumber)

            };

            return Execute(strSQL, CommandType.Text, parms);

        }//GetClientDetails

        public DataSet GetNextClientDetails(string accountNumber)
        {
            string strSQL = "select C_REC_NO, C_LANGUAGE, C_SALUTATION, " +
                     "C_FIRSTNAME_INTL, C_SURNAME, C_JOB_TITLE, C_ORGANIZATION, " +
                     "C_STREET, C_ADDRESS_LINE_2, C_PROV_CODE, C_PROVINCE_NAME, " +
                     "C_CITY, C_POSTAL_CODE, C_TELEPHONE, C_FAX_NO, C_EMAIL, " +
                     "C_COUNTRY, C_WWW, C_CUSTOMER_TYPE, C_STATUS, C_POSTAL_CODE, C_DEL_MODE, " +
                     "to_char(C_DATE_INPUT, 'DD-MON-YYYY') as C_DATE_INPUT, to_char(C_DATE_AMENDED, 'DD-MON-YYYY') as C_DATE_AMENDED, " + 
                     "C_OPERATOR, C_OWNER, C_USER_GRP, to_char(C_DATE_USED, 'DD-MON-YYYY') as C_DATE_USED" +
                     " from FCLIENT where C_REC_NO in (select min(C_REC_NO) from FCLIENT where C_REC_NO > :accountNumber)";

            DbParameter[] parms = new DbParameter[]
            {
                CreateParameter(":accountNumber", DbType.String, accountNumber)

            };

            return Execute(strSQL, CommandType.Text, parms);

        }//GetNextClientDetails

        public DataSet GetPreviousClientDetails(string accountNumber)
        {
            string strSQL = "select C_REC_NO, C_LANGUAGE, C_SALUTATION, " +
                     "C_FIRSTNAME_INTL, C_SURNAME, C_JOB_TITLE, C_ORGANIZATION, " +
                     "C_STREET, C_ADDRESS_LINE_2, C_PROV_CODE, C_PROVINCE_NAME, " +
                     "C_CITY, C_POSTAL_CODE, C_TELEPHONE, C_FAX_NO, C_EMAIL, " +
                     "C_COUNTRY, C_WWW, C_CUSTOMER_TYPE, C_STATUS, C_POSTAL_CODE, C_DEL_MODE, " +
                     "to_char(C_DATE_INPUT, 'DD-MON-YYYY') as C_DATE_INPUT, to_char(C_DATE_AMENDED, 'DD-MON-YYYY') as C_DATE_AMENDED, " +
                     "C_OPERATOR, C_OWNER, C_USER_GRP, to_char(C_DATE_USED, 'DD-MON-YYYY') as C_DATE_USED" +
                     " from FCLIENT where C_REC_NO in (select max(C_REC_NO) from FCLIENT where C_REC_NO < :accountNumber)";

            DbParameter[] parms = new DbParameter[]
            {
                CreateParameter(":accountNumber", DbType.String, accountNumber)

            };

            return Execute(strSQL, CommandType.Text, parms);

        }//GetPreviousClientDetails

        public DataSet GetFirstClientDetails()
        {
            string strSQL = "select C_REC_NO, C_LANGUAGE, C_SALUTATION, " +
                     "C_FIRSTNAME_INTL, C_SURNAME, C_JOB_TITLE, C_ORGANIZATION, " +
                     "C_STREET, C_ADDRESS_LINE_2, C_PROV_CODE, C_PROVINCE_NAME, " +
                     "C_CITY, C_POSTAL_CODE, C_TELEPHONE, C_FAX_NO, C_EMAIL, " +
                     "C_COUNTRY, C_WWW, C_CUSTOMER_TYPE, C_STATUS, C_POSTAL_CODE, C_DEL_MODE, " +
                     "to_char(C_DATE_INPUT, 'DD-MON-YYYY') as C_DATE_INPUT, to_char(C_DATE_AMENDED, 'DD-MON-YYYY') as C_DATE_AMENDED, " +
                     "C_OPERATOR, C_OWNER, C_USER_GRP, to_char(C_DATE_USED, 'DD-MON-YYYY') as C_DATE_USED" +
                     " from FCLIENT where C_REC_NO in (select min(C_REC_NO) from FCLIENT)";

            return Execute(strSQL, CommandType.Text);

        }//GetFirstClientDetails

        public DataSet GetLastClientDetails()
        {
            string strSQL = "select C_REC_NO, C_LANGUAGE, C_SALUTATION, " +
                     "C_FIRSTNAME_INTL, C_SURNAME, C_JOB_TITLE, C_ORGANIZATION, " +
                     "C_STREET, C_ADDRESS_LINE_2, C_PROV_CODE, C_PROVINCE_NAME, " +
                     "C_CITY, C_POSTAL_CODE, C_TELEPHONE, C_FAX_NO, C_EMAIL, " +
                     "C_COUNTRY, C_WWW, C_CUSTOMER_TYPE, C_STATUS, C_POSTAL_CODE, C_DEL_MODE, " +
                     "to_char(C_DATE_INPUT, 'DD-MON-YYYY') as C_DATE_INPUT, to_char(C_DATE_AMENDED, 'DD-MON-YYYY') as C_DATE_AMENDED, " +
                     "C_OPERATOR, C_OWNER, C_USER_GRP, to_char(C_DATE_USED, 'DD-MON-YYYY') as C_DATE_USED" +
                     " from FCLIENT where C_REC_NO in (select max(C_REC_NO) from FCLIENT)";

            return Execute(strSQL, CommandType.Text);

        }//GetLastClientDetails

        public bool IsLastClient(string accountNumber)
        {
            bool isLast = false;
            int max_client_id = 0;
            int x = 0;

            string strSQL = "select max(C_REC_NO) as MAX_C_REC_NO from FCLIENT";

            DataSet ds = Execute(strSQL, CommandType.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRowCollection clientRows = ds.Tables[0].Rows;
                max_client_id = Int32.Parse(clientRows[0]["MAX_C_REC_NO"].ToString());
                if (Int32.TryParse(accountNumber, out x))
                {
                    if (x == max_client_id)
                        isLast = true;
                }
            }

            return isLast;

        }//IsLastClient

        public bool IsFirstClient(string accountNumber)
        {
            bool isLast = false;
            int min_client_id = 0;
            int x = 0;

            string strSQL = "select min(C_REC_NO) as MIN_C_REC_NO from FCLIENT";

            DataSet ds = Execute(strSQL, CommandType.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRowCollection clientRows = ds.Tables[0].Rows;
                min_client_id = Int32.Parse(clientRows[0]["MIN_C_REC_NO"].ToString());
                if (Int32.TryParse(accountNumber, out x))
                {
                    if (x == min_client_id)
                        isLast = true;
                }
            }

            return isLast;

        }//IsLastClient

        public int DeleteClient(string accountNumber)
        {
            int affected_rows = 0;
            string strSQL = "delete from FCLIENT where C_REC_NO = :accountNumber";

            DbParameter[] parms = new DbParameter[]
            {
                CreateParameter(":accountNumber", DbType.String, accountNumber)
            };

            affected_rows = ExecuteNonQuery(strSQL, CommandType.Text, parms);

            return affected_rows;

        }//GetPreviousClientDetails


        public DataSet GetPickSalutations(string pageLang)
        {
            string strSQL = "select LANG_CODE, LANG_EN as LANG " +
                         " from F_HTML_LANGUAGE where LANG_CODE in ('Miss', 'Mr', 'Mrs')";

            if (pageLang == "FR")
            {
                strSQL = "select LANG_CODE, LANG_FR as LANG" +
                         " from F_HTML_LANGUAGE where LANG_CODE in ('Miss', 'Mr', 'Mrs')";
            }

            return Execute(strSQL, CommandType.Text);

        }//GetPickSalutations

        public DataSet GetProvinceCode()
        {
            string strSQL = "select PROVSEQUENCE, PROV_CODE " +
                         " from FPROV_CODES where PROV_INACTIVE = 0 order by PROV_CODE";

            return Execute(strSQL, CommandType.Text);

        }//GetProvinceCode

        public DataSet GetProvinceName(string prov_code, string pageLang)
        {

            string strSQL = "select PROV_DEFINITION, PROV_CODE " +
                         " from FPROV_CODES where PROV_CODE = :prov_code";

            if (pageLang == "FR")
            {
                strSQL = "select PROV_FDEFINITION as PROV_DEFINITION, PROV_CODE " +
                             " from FPROV_CODES where PROV_CODE = :prov_code";
            }

            DbParameter[] parms = new DbParameter[]
            {
                CreateParameter(":prov_code", DbType.String, prov_code)
            };

            return Execute(strSQL, CommandType.Text, parms);
        }//GetProvinceName

        public DataSet GetClientType(string pageLang)
        {
            string strSQL = "select AFF_CODE, AFF_GEN_DEF from F_AFFILIATION " +
                                " order by upper(AFF_GEN_DEF)";

            if (pageLang == "FR")
            {
                strSQL = "select AFF_CODE, AFF_GEN_DEF_FR as AFF_GEN_DEF from F_AFFILIATION " +
                            " order by upper(AFF_GEN_DEF_FR)";
            }

            return Execute(strSQL, CommandType.Text);

        }//GetClientType

        public DataSet GetClientStatus(string pageLang)
        {
            string strSQL = "select CL_STAT_SEQ, CL_STAT_DESC from F_CLIENT_STATUS " +
                                "where CL_STAT_INACTIVE  = 0 order by upper(CL_SORT_CODE) ";

            if (pageLang == "FR")
            {
                strSQL = "select CL_STAT_SEQ, CL_STAT_DESC_FR as CL_STAT_DESC from F_CLIENT_STATUS " +
                            " where CL_STAT_INACTIVE  = 0 order by upper(CL_SORT_CODE)";
            }

            return Execute(strSQL, CommandType.Text);

        }//GetClientStatus

        public DataSet GetClientDelMode(string pageLang, string user_access)
        {

            string the_user_access = splitPriv(user_access);

            string strSQL = "select DEL_CODE, DEL_DEFINITION, DEL_WH from FDEL_CODES " +
                                "where DEL_WH in " +
                                " ( select WH_NUMBER from F_WAREHOUSE where upper(WH_GROUP) in (:user_access) ) ";

            if (pageLang == "FR")
            {
                strSQL = "select DEL_CODE, DEL_DEFINITION_FR as DEL_DEFINITION, DEL_WH from FDEL_CODES " +
                                "where DEL_WH in " +
                                " ( select WH_NUMBER from F_WAREHOUSE where upper(WH_GROUP) in (:user_access) ) ";
            }

            DbParameter[] parms = new DbParameter[]
            {
                CreateParameter(":user_access", DbType.String, user_access)
            };

            return Execute(strSQL, CommandType.Text, parms);
        }//GetClientDelMode

        public string GetClientsMinID()
        {
            string the_min_id = "0";
            DataSet ds;

            string strSQL = "select min(C_REC_NO) as C_REC_NO from FCLIENT";

            ds = Execute(strSQL, CommandType.Text);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRowCollection provRows = ds.Tables[0].Rows;
                the_min_id = provRows[0]["C_REC_NO"].ToString();
            }

            return the_min_id;
        }
        /*
         *         
        private string GetProvinceName(string p_code, string page_lang)
        {
            string prov_name = "";

            DataSet ds = clientDB.GetProvinceName(p_code, page_lang);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRowCollection provRows = ds.Tables[0].Rows;
                prov_name = provRows[0]["PROV_DEFINITION"].ToString();
            }

            return prov_name;
        }
         */
        public int UpdateInsertClientClassic(int txtAccount, string txtFirstName, string txtLastName, string txtCompany,
                            string txtAddress1, string txtAddress2, string txtJobTitle, string txtSalutation,
                            string txtPhone, string txtCity, string txtFax, string txtEmail,
                            string txtProvState, string txtWebUrl, string txtPostalCode, string txtCountry,
                            string ddlDeliveryMode, string ddlLanguage,
                            string ddlProvCode, int ddlStatus, string ddlType, 
                            string c_date_amended, string c_operator, string c_owner, string c_user_grp)
        {

            string strSQL;
            int the_client_id = 0;

            txtFirstName = txtFirstName.Replace("'", "''");
            txtLastName = txtLastName.Replace("'", "''");
            txtCompany = txtCompany.Replace("'", "''");
            txtAddress1 = txtAddress1.Replace("'", "''");
            txtAddress2 = txtAddress2.Replace("'", "''");
            txtJobTitle = txtJobTitle.Replace("'", "''");
            txtSalutation = txtSalutation.Replace("'", "''");

            string the_date = "TO_DATE('" + c_date_amended + "', 'DD-MON-YYYY HH24:MI:SS')";
            // new client
            if (txtAccount <= 0)
            {
                int client_id = GetClientID();
                the_client_id = client_id;
                

                strSQL = "insert into FCLIENT(C_SEQUENCE, C_REC_NO, C_LANGUAGE, C_SALUTATION, C_FIRSTNAME_INTL, " +
                           "C_SURNAME, C_JOB_TITLE, C_ORGANIZATION, C_STREET, C_ADDRESS_LINE_2, C_PROV_CODE, " +
                           "C_PROVINCE_NAME, C_CITY, C_POSTAL_CODE, C_TELEPHONE, C_FAX_NO, C_COUNTRY, " +
                           "C_CUSTOMER_TYPE, C_DEL_MODE, C_EMAIL, C_STATUS, C_WWW, " + 
                           "C_DATE_INPUT, C_DATE_AMENDED, C_OPERATOR, C_OWNER, C_USER_GRP) " +
                           " values (C_SEQUENCE.nextval, " + the_client_id + ", '" + ddlLanguage + "', '" + txtSalutation + "' ,'" + txtFirstName + "', '" + txtLastName + 
                           "', '" + txtJobTitle + "', '" +  txtCompany + "', '" + txtAddress1 + "', '" + txtAddress2 + "', '" + ddlProvCode + "', '" + 
                           txtProvState + "', '" + txtCity + "', '" + txtPostalCode + "', '" + txtPhone + "', '" + txtFax + "', '" + txtCountry + "', '" +
                           ddlType + "', '" + ddlDeliveryMode + "', '" + txtEmail + "', " + ddlStatus + ", '" + txtWebUrl +
                           "', " + the_date + ", " + the_date +
                           ", '" + c_operator + "', '" + c_owner + "', '" + c_user_grp + "')";

                ExecuteNonQuery(strSQL, CommandType.Text);
            }
            // update client txtAccount > 0
            else
            {
                the_client_id = txtAccount;

                strSQL = "update FCLIENT set C_LANGUAGE = '" + ddlLanguage + "', C_SALUTATION = '" + txtSalutation + "', C_FIRSTNAME_INTL = '" + txtFirstName + 
                            "', C_SURNAME = '" + txtLastName +
                            "', C_JOB_TITLE = '" + txtJobTitle +
                            "', C_ORGANIZATION = '" + txtCompany +
                            "', C_STREET = '" + txtAddress1 +
                            "', C_ADDRESS_LINE_2 = '" + txtAddress2 +
                            "', C_PROV_CODE = '" + ddlProvCode +
                            "', C_PROVINCE_NAME = '" + txtProvState +
                            "', C_CITY = '" + txtCity +
                            "', C_POSTAL_CODE = '" + txtPostalCode +
                            "', C_TELEPHONE = '" + txtPhone +
                            "', C_FAX_NO = '" + txtFax +
                            "', C_CUSTOMER_TYPE = '" + ddlType +
                            "', C_DEL_MODE = '" + ddlDeliveryMode +
                            "', C_EMAIL = '" + txtEmail +
                            "', C_STATUS = " + ddlStatus +
                            ", C_WWW = '" + txtWebUrl +
                            "', C_DATE_AMENDED = " + the_date +
                            ", C_OPERATOR= '" + c_operator +
                            "' WHERE C_REC_NO = " + txtAccount;
                           
                ExecuteNonQuery(strSQL, CommandType.Text);
            }

            return the_client_id;
        }


        public int UpdateInsertClient(int txtAccount, string txtFirstName, string txtLastName, string txtCompany,
                            string txtAddress1, string txtAddress2, string txtJobTitle, string txtSalutation,
                            string txtPhone, string txtCity, string txtFax, string txtEmail,
                            string txtProvState, string txtWebUrl, string txtPostalCode, string txtCountry,
                            string ddlDeliveryMode, string ddlLanguage,
                            string ddlProvCode, int ddlStatus, string ddlType)
        {

            string strSQL;
            int the_client_id = 0;
            
            // new client
            if (txtAccount <= 0)
            {
                int client_id = GetClientID();
                the_client_id = client_id;

                strSQL = "insert into FCLIENT(C_REC_NO, C_LANGUAGE, C_SALUTATION, C_FISRTNAME_INTL, " +
                           "C_SURNAME, C_JOB_TITLE, C_ORGANIZATION, C_STREET, C_ADDRESS_LINE_2, C_PROV_CODE, " +
                           "C_PROVINCE_NAME, C_CITY, C_POSTAL_CODE, C_TELEPHONE, C_FAX_NO, C_COUNTRY, " +
                           "C_CUSTOMER_TYPE, C_DEL_MODE, C_EMAIL, C_STATUS, C_WWW) " +
                           " values (:c_rec_no, :c_language, :c_salutation, :c_firstname_intl, " + 
                           " :c_surname, :c_job_title, :c_organization, :c_street, :c_address_line_2, :c_prov_code, " +
                           " :c_province_name, :c_city, :c_postal_code, :c_telephone, :c_fax_no, :c_country" +
                           " :c_customer_type, :c_del_mode, :c_email, :c_status, :c_www)";
                DbParameter[] parms = new DbParameter[]
                {
                    CreateParameter(":c_rec_no", DbType.Int32, client_id),
                    CreateParameter(":c_language", DbType.String, ddlLanguage),
                    CreateParameter(":c_salutation", DbType.String, txtSalutation),
                    CreateParameter(":c_firstname_intl", DbType.String, txtFirstName),
                    CreateParameter(":c_surname", DbType.String, txtLastName),
                    CreateParameter(":c_job_title", DbType.String, txtJobTitle),
                    CreateParameter(":c_organization", DbType.String, txtCompany),
                    CreateParameter(":c_street", DbType.String, txtAddress1),
                    CreateParameter(":c_address_line_2", DbType.String, txtAddress2),
                    CreateParameter(":c_prov_code", DbType.String, ddlProvCode),
                    CreateParameter(":c_province_name", DbType.String, txtProvState),
                    CreateParameter(":c_city", DbType.String, txtCity),
                    CreateParameter(":c_postal_code", DbType.String, txtPostalCode),
                    CreateParameter(":c_telephone", DbType.String, txtPhone),
                    CreateParameter(":c_fax_no", DbType.String, txtFax),
                    CreateParameter(":c_country", DbType.String, txtCountry),
                    CreateParameter(":c_customer_type", DbType.String, ddlType),
                    CreateParameter(":c_del_mode", DbType.String, ddlDeliveryMode),
                    CreateParameter(":c_email", DbType.String, txtEmail),
                    CreateParameter(":c_status", DbType.Int32, ddlStatus),
                    CreateParameter(":c_www", DbType.String, txtWebUrl)
                };
                
                ExecuteNonQuery(strSQL, CommandType.Text, parms);
            }
            // update client txtAccount > 0
            else
            {
                the_client_id = txtAccount;

                strSQL = "update FCLIENT set C_LANGUAGE = :c_language, C_SALUTATION = :c_salutation, C_FISRTNAME_INTL = :c_firstname_intl, " +
                           "C_SURNAME = :c_surname, C_JOB_TITLE = :c_job_title, C_ORGANIZATION = :c_organization, C_STREET = :c_street, " +
                           "C_ADDRESS_LINE_2 = :c_address_line_2, C_PROV_CODE = :c_prov_code, C_PROVINCE_NAME = :c_province_name, C_CITY = :c_city, " +
                           "C_POSTAL_CODE = :c_prov_code, C_TELEPHONE = :c_telephone, C_FAX_NO = :c_fax_no, C_COUNTRY = :c_country, " + 
                           "C_CUSTOMER_TYPE = :c_customer_type, C_DEL_MODE = :c_del_mode, C_EMAIL = :c_email, C_STATUS = :c_status, C_WWW = :c_www " +
                           " WHERE C_REC_NO = :c_rec_no"; 

                DbParameter[] parms = new DbParameter[]
                {
                    CreateParameter("c_rec_no", DbType.Int32, txtAccount),
                    CreateParameter("c_language", DbType.String, ddlLanguage),
                    CreateParameter("c_salutation", DbType.String, txtSalutation),
                    CreateParameter("c_firstname_intl", DbType.String, txtFirstName),
                    CreateParameter("c_surname", DbType.String, txtLastName),
                    CreateParameter("c_job_title", DbType.String, txtJobTitle),
                    CreateParameter("c_organization", DbType.String, txtCompany),
                    CreateParameter("c_street", DbType.String, txtAddress1),
                    CreateParameter("c_address_line_2", DbType.String, txtAddress2),
                    CreateParameter("c_prov_code", DbType.String, ddlProvCode),
                    CreateParameter("c_province_name", DbType.String, txtProvState),
                    CreateParameter("c_city", DbType.String, txtCity),
                    CreateParameter("c_postal_code", DbType.String, txtPostalCode),
                    CreateParameter("c_telephone", DbType.String, txtPhone),
                    CreateParameter("c_fax_no", DbType.String, txtFax),
                    CreateParameter("c_country", DbType.String, txtCountry),
                    CreateParameter("c_customer_type", DbType.String, ddlType),
                    CreateParameter("c_del_mode", DbType.String, ddlDeliveryMode),
                    CreateParameter("c_email", DbType.String, txtEmail),
                    CreateParameter("c_status", DbType.Int32, ddlStatus),
                    CreateParameter("c_www", DbType.String, txtWebUrl)
                };
                
                ExecuteNonQuery(strSQL, CommandType.Text, parms);
            }

            return the_client_id;
        }

        public int GetClientID()
        {
            int client_id = 0;
            string client_id_string;
            DataSet ds;

            string strSQL = "select CON_CUSTOMER from FCONSTANT where CONSTSEQ = 1";

            ds = Execute(strSQL, CommandType.Text);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRowCollection provRows = ds.Tables[0].Rows;
                client_id_string = provRows[0]["CON_CUSTOMER"].ToString();

                if (Int32.TryParse(client_id_string, out client_id))
                {
                    client_id = client_id + 1;

                    strSQL = "update FCONSTANT set CON_CUSTOMER = :client_id where CONSTSEQ = 1";

                    DbParameter[] parms = new DbParameter[]
                    {
                        CreateParameter(":client_id", DbType.Int32, client_id)
                    };

                    ExecuteNonQuery(strSQL, CommandType.Text, parms);

                }
            }

            return client_id;

        }

        public string GetProvinceByPostalCode(string postal_code, string page_lang)
        {
            string province = "";
            char first = postal_code[0];

            switch (first)
            {
                case 'A':
                    province = "NL";
                    break;
                case 'B':
                    province = "NS";
                    break;
                case 'C':
                    province = "PE";
                    break;
                case 'D':
                    province = "NB";
                    break;
                case 'H': case 'J': case 'G':
                    province = "QC";
                    break;
                case 'K': case 'L': case 'M': case 'N': case 'P':
                    province = "ON";
                    break;
                case 'R':
                    province = "MB";
                    break;
                case 'S':
                    province = "SK";
                    break;
                case 'T':
                    province = "AB";
                    break;
                case 'U':
                    province = "BC";
                    break;
                case 'X':
                    if (postal_code.Substring(0, 3) == "X0A" ||
                        postal_code.Substring(0, 3) == "X0B" ||
                        postal_code.Substring(0, 3) == "X0C")
                        province = "NU";
                    else
                        province = "NT";
                    break;
                default: 
                    province = "";
                    break;
            }
            return province;
        }

        public DataSet GetContactListAssocDiassocWithClient(bool associatedWithClient, string c_rec_no)
        {
            string strSQL;

            if (associatedWithClient)
                strSQL = "select con.*, c1.CON_CL_SEQ, to_char(c1.CL_DATE_IN, 'DD-Mon-YYYY') as CL_DATE_IN, " + 
                                " to_char(c1.CL_DATE_OUT, 'DD-Mon-YYYY') as CL_DATE_OUT " +
                                " from F_CONTACT con INNER JOIN CON_LINK_CL c1 ON " +
                                " c1.CON_SEQ_NO = con.CON_SEQUENCE " +
                                " where c1.CL_REC_NO = :c_rec_no" +
                                " order by con.CON_CODE";

            else            
                strSQL = "SELECT con.* FROM F_CONTACT con " +
                            " WHERE NOT EXISTS (SELECT c1.CON_CL_SEQ FROM CON_LINK_CL c1 " +
                                    " WHERE con.CON_SEQUENCE = c1.CON_SEQ_NO " +
                                    " AND c1.CL_REC_NO = :c_rec_no) " +
                            " ORDER BY con.CON_CODE";


            DbParameter[] parms = new DbParameter[]
            {
                CreateParameter(":c_rec_no", DbType.Int32, c_rec_no)
            };

            return Execute(strSQL, CommandType.Text, parms);
        }//GetContactListAssocDiassocWithClient


        public void AddContactLinkRecords(string conSeqList, string c_rec_no, string user_id)
        {
            string strSQL;

            strSQL = "INSERT INTO CON_LINK_CL " +
                     "(CON_CL_SEQ, CL_REC_NO, CON_SEQ_NO, CL_CON_CODE, CL_CON_GRP, " +
                     "CL_DESC, CL_DATE_IN, CL_STATUS, CL_USERID) " +
                     " select CON_CL_SEQ.NextVal, " + c_rec_no + ", CON_SEQUENCE, CON_CODE, CON_GRP, " +
                     "(CON_DEF || ' / ' || CON_DEF_FR), SYSDATE, 1, '" + user_id + "' FROM F_CONTACT " +
                     " WHERE CON_SEQUENCE IN (" + conSeqList + ") " +
                     " AND  CON_SEQUENCE NOT IN (SELECT CON_SEQ_NO FROM CON_LINK_CL WHERE CL_REC_NO = " + c_rec_no + ")";

            ExecuteNonQuery(strSQL, CommandType.Text);
        }

        public void DeleteContactLinkRecords(string conLinkSeqIDs)
        {
            string strSQL;

            strSQL = "DELETE FROM CON_LINK_CL WHERE CON_SEQ_NO IN (" + conLinkSeqIDs + ")";

            ExecuteNonQuery(strSQL, CommandType.Text);
        }

        public void UpdateContactLinkLastUsage(string c_rec_no, string contactCode)
        {
            string strSQL;

            strSQL = "UPDATE CON_LINK_CL SET CL_DATE_OUT = SYSDATE " +
                        " WHERE CL_REC_NO = " + c_rec_no + " AND CL_CON_CODE = '" + contactCode + "'";

            ExecuteNonQuery(strSQL, CommandType.Text);
        }

        public DataSet GetClientContacts(string accountNumber)
        {
            string strSQL = "select CA_SEQ, TO_CHAR(CA_DATE, 'DD-MON-YYYY') || ', ' || CA_SUBJECT as SUBJECT, CA_COMM " +
                     " from F_CLIENT_ACTIVITY where CA_CLIENT = :accountNumber and CA_COMM is not null order by CA_COMM";

            DbParameter[] parms = new DbParameter[]
            {
                CreateParameter(":accountNumber", DbType.String, accountNumber)

            };

            return Execute(strSQL, CommandType.Text, parms);

        }//GetClientContacts

        public DataSet GetClientCommType(string pageLang)
        {

            // string the_user_access = splitPriv(user_access);

            string strSQL = "select COMM_SEQ, COMM_TITLE from F_COMM_TYPES";

            if (pageLang == "FR")
            {
                strSQL = "select COMM_SEQ, COMM_TITLE_FR as COMM_TITLE from F_COMM_TYPES";
            }

            return Execute(strSQL, CommandType.Text);
        }//GetClientCommType

        public DataSet GetClientActivityStatus(string pageLang)
        {

            // string the_user_access = splitPriv(user_access);

            string strSQL = "select CL_STAT_SEQ, CL_STAT_DESC from F_CLIENT_STATUS order by CL_STAT_SEQ";

            if (pageLang == "FR")
            {
                strSQL = "select CL_STAT_SEQ, CL_STAT_DESC_FR as CL_STAT_DESC from F_CLIENT_STATUS order by CL_STAT_SEQ";
            }

            return Execute(strSQL, CommandType.Text);
        }//GetClientActivityStatus

        public DataSet GetClientActivitySubjects(string pageLang)
        {

            // string the_user_access = splitPriv(user_access);

            string strSQL = "select ACTION_SEQ, ACTION_TEXT from F_ACTION_TYPES";

            if (pageLang == "FR")
            {
                strSQL = "select ACTION_SEQ, ACTION_TEXT_FR as ACTION_TEXT from F_ACTION_TYPES";
            }

            return Execute(strSQL, CommandType.Text);
        }//GetClientActivityStatus
    }
}
