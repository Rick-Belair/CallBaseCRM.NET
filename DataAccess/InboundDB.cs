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
    public class InboundDB : DAHelper
    {
        public DataSet GetInboundDetails(string ticketNumber, string incline)
        {
            string linesAllowed = getLinesAllowed(incline);
            string strSQL = "select S_SEQUENCE, S_REC_NO, S_LANG_E, S_LANG_F," +
                     "S_L_OTHER, S_OLANG_FLAG, S_USER_GRP, S_LOC_REG," +
                     "S_NON_LOC_REG, S_ADVICE, S_RES_LINE, S_PRODUCT," +
                     "S_COMPLAINT, S_REFERRAL, S_REF_TO, S_ADVICE_TO," +
                     "S_RESEARCH_OVER_UNDER_10, S_RESEARCH_EXTRA, S_CORP, S_CORP_INFO_TO, " +
                     "S_VERBAL_INFO, S_RESEARCH_DURATION, S_EMERG, S_NOT_NCC," +
                     "S_OTHER_SUB, S_OSUB_CODE, S_DIR, S_GOVT, to_char(S_DATE_INPUT, 'DD-MON-YYYY HH24:MI') as S_DATE_INPUT," +
                     "S_OPERATOR, S_TELE, S_VISIT, S_WRITTEN, S_FAX, S_INTERNAL," +
                     "S_EMAIL, S_GEN, S_ASS_GRP, S_SURVEY_DATA_ON, S_OCLIENT_FLAG," +
                     "S_CLIENT_TEXT, S_COMMENTS,  S_ELECTED, S_BUSINESS," +
                     "S_SRC_DEF, S_TEL_LONG, S_WWW, S_VOICE_M, S_PROV, S_OP_NO, S_SPLIT," +
                     "S_CL_REC, S_SPLIT_FROM, S_OPEN_SUBJECT, S_SUBJ_STRING, S_REF_DEF, S_SRC_DEF, S_TELE_USED," +
                     "S_CALLERS_NO, S_CUSTOMER_TYPE, NVL(S_GENDER, 0) S_GENDER, S_TONE_START, S_TONE_END," +
                     "S_OPEN_ANSWER, S_OPEN_SUBJECT, S_OPEN_REGION,S_OWNER," +
                     "S_ORDER_STATUS, to_char(S_ORDER_STATUS_DATE, 'DD-MON-YYYY HH24:MI') as S_ORDER_STATUS_DATE," +
                     "to_char(S_DATE_BACK, 'DD-MON-YYYY HH24:MI') as S_DATE_BACK, " +
                     "to_char(S_DATE_TO_CALLBACK, 'DD-MON-YYYY HH24:MI') as S_DATE_TO_CALLBACK, S_DATE_SENT, " +
                     "to_char(S_DATE_EDIT, 'DD-MON-YYYY HH24:MI') as S_DATE_EDIT, S_USER_EDIT, S_ABORIGINAL " +
                     " from FSTATISTICS where S_REC_NO = :ticketNumber and s_tele_used in (" + linesAllowed + ")";



            DbParameter[] parms = new DbParameter[]
            {
                CreateParameter(":ticketNumber", DbType.String, ticketNumber)

            };

            return Execute(strSQL, CommandType.Text, parms);

        }//GetInboundDetails

        public DataSet GetProductDetails(string ticketNumber, string language)
        {
            string strSQL = "select PR_SEQUENCE, PR_ORDER_NUM, PR_INVENTORY_CODE, " +
                        "PR_QUANTITY, PR_DATE_SENT, PR_REC_NO_2, PR_TITLE, nvl(PR_SALE,0) PR_SALE, PR_WH_NUM,  " +
                        "nvl(PR_GST,0) PR_GST, nvl(PR_TOTAL,0) PR_TOTAL, nvl(PR_PST,0) PR_PST, PR_DONE, PR_PROCESS_STATUS, PR_DEDUCT_QTY, PR_DATE_INPUT,  " +
                        "PR_UM, PR_USER_ID, PR_EXTRA_TOTAL, PR_SIZE, PR_STOCK_CHECK, PR_NOTES, PR_COMP_FLAG,  " +
                        "PR_PROD_TYPE, PR_PROD_CAT, PR_PROD_ACCOUNT, PR_WIDTH, PR_DEPTH, PR_HEIGHT,  " +
                        "PR_CALC_FLAG, PR_DUE, PR_PRIORITY, nvl(PR_UNIT_COST,0) PR_UNIT_COST, PR_TOTAL_COST, nvl(PR_UNIT_PRICE,0) PR_UNIT_PRICE,  " +
                        "PR_SUPPLIER, PR_KIT, PR_GST_EXEMPT, PR_PST_EXEMPT, PR_DELIVERY_MODE, PR_SUP_NO,  ";
            if (language.Equals("FR"))
                strSQL += "PROD_STATUS_DEF_FR PROD_STATUS_DEF, ";
            else
                strSQL += "PROD_STATUS_DEF, ";

            //strSQL += "PR_FREIGHT, PR_CUSTOMER from FPROD_REQ JOIN FPROD_STATUS ON PR_STATUS = PROD_STATUS_CD" +
            //    " where PR_ORDER_NUM = '" + ticketNumber + "' Order by PR_INVENTORY_CODE";
            strSQL += "PR_FREIGHT, PR_CUSTOMER, PROD_TYPE from v_prod_req_all JOIN FPROD_STATUS ON PR_STATUS = PROD_STATUS_CD" +
                " where PR_ORDER_NUM = '" + ticketNumber + "' Order by PR_INVENTORY_CODE";


            return Execute(strSQL, CommandType.Text);

        }//GetProductDetails

        public int DeleteProductDetails(string inventoryCode, string ticketNumber)
        {
            string strSQL = "delete from FPROD_REQ where PR_INVENTORY_CODE = '" + inventoryCode + "' and PR_ORDER_NUM = '" + ticketNumber + "'";

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }//DeleteProductDetails

        public int DeleteProductDetailsElectronic(string inventoryCode, string ticketNumber)
        {
            string strSQL = "delete from FPROD_REQ_EMAIL where PR_INVENTORY_CODE = '" + inventoryCode + "' and PR_ORDER_NUM = '" + ticketNumber + "' and PR_DATE_SENT is null";

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }//DeleteProductDetails

        public int UpdateProductDetails(string inventoryCode, string ticketNumber, string QTY)
        {
            string strSQL = "update FPROD_REQ set PR_QUANTITY = " + QTY + " where PR_INVENTORY_CODE = '" + inventoryCode + "' and PR_ORDER_NUM = '" + ticketNumber + "'";

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }//UpdateProductDetails

        public int DeleteProductOrder(string inventoryCode, string ticketNumber)
        {
            string strSQL = "delete from ORDERS_IN_PROCESS where OIP_INV_CODE = '" + inventoryCode + "' and OIP_TICKET_NUM = '" + ticketNumber + "'";

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }//DeleteProductDetails

        public int UpdateProductOrder(string inventoryCode, string ticketNumber, string QTY)
        {
            string strSQL = "update ORDERS_IN_PROCESS set OIP_QTY = " + QTY + " where OIP_INV_CODE = '" + inventoryCode + "' and OIP_TICKET_NUM = '" + ticketNumber + "'";

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }//UpdateProductDetails

        public int InsertProductOrder(string inventoryCode, string ticketNumber, string QTY, string wh, string delivery, int processType)
        {
            string strSQL = "insert into ORDERS_IN_PROCESS(OIP_INV_CODE, OIP_TICKET_NUM, OIP_QTY, OIP_DATE_IN, OIP_WH, OIP_DEL_MODE, OIP_PROCESS_TYPE) values('" + inventoryCode + "', " +
                ticketNumber + ", " + QTY + ", sysdate, " + wh + ", '" + delivery + "', " + processType + ")"; 

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }//InsertProductOrder

        public DataSet GetCustomerDetails(string ticketNumber)
        {
            string strSQL = "select fs_rec_no, fs_fname, fs_lname, fs_company, fs_street, fs_street2, fs_city, fs_prov_code, fs_country, fs_tel, fs_post_code, fs_email, fs_back_order, " +
                "fs_fax, fs_ready_to_ship, fs_delivered_flag, fs_delivery, fs_notes, fs_shipping_details, fs_delivered from fsale where fs_order_num = " + ticketNumber;
            return Execute(strSQL, CommandType.Text);

        }//GetCustomerDetails

        public DataSet SearchCustomers(string condition, bool accountFlag, bool postFlag, bool lastFlag, bool orgFlag)
        {
            string strSQL = "select c_rec_no, c_firstname_intl, c_surname, c_postal_code, c_city, c_prov_code, c_country, c_fax_no, c_telephone, c_organization, c_email, "
            + "c_street, c_address_line_2 from fclient";

            if (accountFlag)
                strSQL += " where c_rec_no = " + condition;

            else if (postFlag)
                strSQL += " where UPPER(c_postal_code) = '" + condition.ToUpper() + "'";

            else if (lastFlag)
                strSQL += " where UPPER(c_surname) = '" + condition.ToUpper() + "'";

            else
                strSQL += " where UPPER(c_organization) = '" + condition.ToUpper() + "'";

            return Execute(strSQL, CommandType.Text);

        }//SearchCustomers

        public DataSet GetSubjects(string language)
        {
            string strSQL = "select subject_code, ";

            if (language.Equals("FR"))
                strSQL += "subj_fr_def as subject";
            else
                strSQL += "subject_def as subject";

            strSQL += " from fsubject where subj_inactive = 0 order by subject";

            return Execute(strSQL, CommandType.Text);

        }//GetSubjects

        public int GetLastRecordNumber()
        {
            string strSQL = "select max(s_rec_no) TicketNo from fstatistics";
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                return int.Parse(tempDS.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
            else return -1;

        }//GetLastRecordNumber

        public int DeleteCustDetails(string ticketNumber)
        {
            string strSQL = "delete from fsale where fs_order_num = " + ticketNumber;

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }//DeleteCustDetails

        public int DeleteEnquiry(string ticketNumber)
        {
            string strSQL = "delete from fstatistics where s_rec_no = " + ticketNumber;

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }//DeleteEnquiry

        public int InsertProductRecord(string ticketNumber, string userid, string inventoryCode, string quantity, string title, string inventoryStatus, string unitCost, string unitPrice,
            string subTotal, string gst, string pst, string total, string totalCost, string stockCheck, string size, string um, string prodType, string prodCat, 
            string poNum, string poNumPrime, string primeUnitCost, string supplier, string primeSupplier, string kit, string supPartNo, string cRecNo, string deliveryMode,
            string whNum, string extraTotal)
        {
            userid = "'" + userid + "'";
            inventoryCode = "'" + inventoryCode + "'";
            title = "'" + title.Replace("'", "''") + "'";
            inventoryStatus = "'" + inventoryStatus + "'";
            stockCheck = "'" + "'";

            if (string.IsNullOrEmpty(size))
                size = "null";
            else
                size = "'" + size + "'";

            if (string.IsNullOrEmpty(um))
                um = "null";
            else
                um = "'" + um + "'";

            if (string.IsNullOrEmpty(prodType))
                prodType = "null";
            else
                prodType = "'" + prodType + "'";

            if (string.IsNullOrEmpty(prodCat))
                prodCat = "null";
            else
                prodCat = "'" + prodCat + "'";

            //if (string.IsNullOrEmpty(prodAccount))
            //    prodAccount = "null";
            //else
            //    prodAccount = "'" + prodAccount + "'";

            string inputDate = "sysdate";

            if (string.IsNullOrEmpty(poNum))
                poNum = "null";

            poNumPrime = poNum;
            primeUnitCost = unitCost;

            if (string.IsNullOrEmpty(supplier))
                supplier = "null";
            else
                supplier = "'" + supplier + "'";

            primeSupplier = supplier;

            if (string.IsNullOrEmpty(supPartNo))
                supPartNo = "null";
            else
                supPartNo = "'" + supPartNo + "'";

            if (string.IsNullOrEmpty(deliveryMode))
                deliveryMode = "null";
            else
                deliveryMode = "'" + deliveryMode + "'";

            if (string.IsNullOrEmpty(cRecNo))
                cRecNo = "null";

            string strSQL = "insert into fprod_req(pr_sequence, pr_order_num, pr_user_id, pr_inventory_code, pr_quantity, pr_title, pr_status, pr_unit_cost, pr_unit_price, pr_subtotal, " +
                "pr_gst, pr_pst, pr_total, pr_total_cost, pr_stock_check, pr_size, pr_um, pr_prod_type, pr_prod_cat, pr_prod_account, pr_date_input, pr_po_num, pr_po_num_prime, " +
                "pr_prime_unit_cost, pr_supplier, " + "pr_prime_supplier, pr_kit, pr_sup_no, pr_customer, pr_delivery_mode, pr_wh_num, pr_extra_total, pr_done, pr_process_status) values("
                + "pr_sequence.nextval, " + ticketNumber + ", " + userid + ", " + inventoryCode + ", " + quantity + ", " + title + ", " + inventoryStatus + ", " + unitCost + ", " + unitPrice
                + ", " + subTotal + ", " + gst + ", " + pst + ", " + total + ", " + totalCost + ", " + stockCheck + ", " + size + ", " + um + ", " + prodType + ", " + prodCat + ", " +
                "null, " + inputDate + ", " + poNum + ", " + poNumPrime + ", " + primeUnitCost + ", " + supplier + ", " + primeSupplier + ", " + kit + ", " + supPartNo + ", " +
                cRecNo + ", " + deliveryMode + ", " + whNum + ", " + extraTotal + ", " + "'NO', 0)"; //NO for done and 0 for process status

            int returnVal = 0;
            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//InsertProductRecord

        public int InsertProductRecordElectronic(string ticketNumber, string userid, string inventoryCode, string quantity, string title, string inventoryStatus, string unitCost, string unitPrice,
            string subTotal, string gst, string pst, string total, string totalCost, string stockCheck, string size, string um, string prodType, string prodCat,
            string poNum, string poNumPrime, string primeUnitCost, string supplier, string primeSupplier, string kit, string supPartNo, string cRecNo, string deliveryMode,
            string whNum, string extraTotal)
        {
            userid = "'" + userid + "'";
            inventoryCode = "'" + inventoryCode + "'";
            title = "'" + title.Replace("'", "''") + "'";
            inventoryStatus = "'" + inventoryStatus + "'";
            stockCheck = "'" + "'";

            size = "null";
            um = "null";
            prodType = "null";
            prodCat = "null";
            string inputDate = "sysdate";
            poNum = "null";
            poNumPrime = poNum;
            primeUnitCost = unitCost;
            supplier = "null";
            primeSupplier = supplier;
            supPartNo = "null";
            deliveryMode = "null";
            cRecNo = "null";

            string strSQL = "insert into fprod_req_email(pr_sequence, pr_order_num, pr_user_id, pr_inventory_code, pr_quantity, pr_title, pr_status, pr_unit_cost, pr_unit_price, pr_subtotal, " +
                "pr_gst, pr_pst, pr_total, pr_total_cost, pr_stock_check, pr_size, pr_um, pr_prod_type, pr_prod_cat, pr_prod_account, pr_date_input, pr_po_num, pr_po_num_prime, " +
                "pr_prime_unit_cost, pr_supplier, " + "pr_prime_supplier, pr_kit, pr_sup_no, pr_customer, pr_delivery_mode, pr_wh_num, pr_extra_total, pr_done, pr_process_status) values("
                + "pr_sequence.nextval, " + ticketNumber + ", " + userid + ", " + inventoryCode + ", " + quantity + ", " + title + ", " + inventoryStatus + ", " + unitCost + ", " + unitPrice
                + ", " + subTotal + ", " + gst + ", " + pst + ", " + total + ", " + totalCost + ", " + stockCheck + ", " + size + ", " + um + ", " + prodType + ", " + prodCat + ", " +
                "null, " + inputDate + ", " + poNum + ", " + poNumPrime + ", " + primeUnitCost + ", " + supplier + ", " + primeSupplier + ", " + kit + ", " + supPartNo + ", " +
                cRecNo + ", " + deliveryMode + ", " + whNum + ", " + extraTotal + ", " + "'NO', 0)"; //NO for done and 0 for process status

            int returnVal = 0;
            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//InsertProductRecord

        public int InsertCustDetails(string ticketNumber, string actNum, string fName, string lName, string company, string street, string street2, string city, string prov, string country,
            string phone, string postalCode, string email, string fax, string readyToShip, string backOrder, string delivery, string shipNotes, string shipDetails)
        {
            if (string.IsNullOrEmpty(fName))
                fName = "null";
            else
                fName = "'" + fName.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(lName))
                lName = "null";
            else
                lName = "'" + lName.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(company))
                company = "null";
            else
                company = "'" + company.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(street))
                street = "null";
            else
                street = "'" + street.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(street2))
                street2 = "null";
            else
                street2 = "'" + street2.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(city))
                city = "null";
            else
                city = "'" + city.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(prov))
                prov = "null";
            else
                prov = "'" + prov + "'";

            if (string.IsNullOrEmpty(country))
                country = "null";
            else
                country = "'" + country.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(phone))
                phone = "null";
            else
                phone = "'" + phone + "'";

            if (string.IsNullOrEmpty(postalCode))
                postalCode = "null";
            else
                postalCode = "'" + postalCode.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(email))
                email = "null";
            else
                email = "'" + email.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(fax))
                fax = "null";
            else
                fax = "'" + fax.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(readyToShip))
                readyToShip = "null";
            else
                readyToShip = "'" + readyToShip + "'";

            if (string.IsNullOrEmpty(backOrder))
                backOrder = "null";
            else
                backOrder = "'" + backOrder + "'";

            if (string.IsNullOrEmpty(delivery))
                delivery = "null";
            else
                delivery = "'" + delivery + "'";

            if (string.IsNullOrEmpty(shipNotes))
                shipNotes = "null";
            else
                shipNotes = "'" + shipNotes.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(shipDetails))
                shipDetails = "null";
            else
                shipDetails = "'" + shipDetails.Replace("'", "''") + "'";

            int returnVal = 0;
            string strSQL = "insert into fsale(fs_seq, fs_order_num, fs_rec_no, fs_fname, fs_lname, fs_company, fs_street, fs_street2, fs_city, fs_prov_code, fs_country, fs_tel, " +
            "fs_post_code, fs_email, fs_fax, fs_ready_to_ship, fs_back_order, fs_delivered_flag, fs_delivery, fs_notes, fs_shipping_details)" +
            "values (fs_seq.nextval, " + ticketNumber + ", " + actNum + ", " + fName + ", " + lName + ", " + company + ", " + street + ", " + street2 + ", " + city + ", " + prov + ", " + 
            country + ", " + phone + ", " + postalCode + ", " + email + ", " + fax + ", " + readyToShip + ", " + backOrder +", null, " + delivery + ", " + shipNotes + ", " + shipDetails +")";

            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//InsertCustDetails

        public int UpdateCustDetails(string ticketNumber, string actNum, string fName, string lName, string company, string street, string street2, string city, string prov, string country,
          string phone, string postalCode, string email, string fax, string delivery, string shipNotes, string shipDetails)
        {
            if (string.IsNullOrEmpty(fName))
                fName = "null";
            else
                fName = "'" + fName.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(lName))
                lName = "null";
            else
                lName = "'" + lName.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(company))
                company = "null";
            else
                company = "'" + company.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(street))
                street = "null";
            else
                street = "'" + street.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(street2))
                street2 = "null";
            else
                street2 = "'" + street2.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(city))
                city = "null";
            else
                city = "'" + city.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(prov))
                prov = "null";
            else
                prov = "'" + prov + "'";

            if (string.IsNullOrEmpty(country))
                country = "null";
            else
                country = "'" + country.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(phone))
                phone = "null";
            else
                phone = "'" + phone.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(postalCode))
                postalCode = "null";
            else
                postalCode = "'" + postalCode.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(email))
                email = "null";
            else
                email = "'" + email.Replace("'", "'") + "'";

            if (string.IsNullOrEmpty(fax))
                fax = "null";
            else
                fax = "'" + fax.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(delivery))
                delivery = "null";
            else
                delivery = "'" + delivery + "'";

            if (string.IsNullOrEmpty(shipNotes))
                shipNotes = "null";
            else
                shipNotes = "'" + shipNotes.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(shipDetails))
                shipDetails = "null";
            else
                shipDetails = "'" + shipDetails.Replace("'", "''") + "'";

            int returnVal = 0;
            string strSQL = "update fsale set fs_rec_no = " + actNum + ", fs_fname = " + fName + ", fs_lname = " + lName + ",  fs_company = " + company + ",  fs_street = " + street + 
                ",  fs_street2 = " + street2 + ",  fs_city = " + city + ",  fs_prov_code = " + prov + ",  fs_country = " + country + ",  fs_tel = " + phone + ",  fs_post_code = " + 
                postalCode + ", fs_email = " + email + ",  fs_fax = " + fax + ",  fs_delivery = " + delivery + ", fs_notes = " + shipNotes + ", fs_shipping_details = " + shipDetails + 
                " where fs_order_num = " + ticketNumber;

            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//UpdateCustDetails

        public int InsertNewEnquiry(string ticketNumber, string openRegion, string otherLang, string incLine, string englishFlag, string frenchFlag, string otherFlag,
           string phoneFlag, string personFlag, string mailFlag, string faxFlag, string exhibitFlag, string emailFlag, string webFlag, string subject, string refTo,
           string adviceTo, string corpTo, string adviceFlag, string corpFlag, string referralFlag, string openSubject, string voiceMailFlag, string gender,
           string issues, string answer, string province, string opNO, string custType, string s_operator, string research10, string researchDuration, string researchExtra, string clRec,
               string userGroup, string owner, string originalOwner, string complaint, string srcDef, string classification, string productFlag, string source, string orderStatus,
               string toneStart, string toneEnd, string dateToCall, string dateBack)
        {
            int returnVal = 0;

            //adjust the variables to form the query
            if (string.IsNullOrEmpty(openRegion))
                openRegion = "null";
            else
                openRegion = "'" + openRegion.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(otherLang))
                otherLang = "null";
            else
                otherLang = "'" + otherLang.Replace("'", "''") + "'";

            incLine = "'" + incLine + "'";

            if (string.IsNullOrEmpty(subject))
                subject = "null";
            else
                subject = "'" + subject.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(refTo))
                refTo = "null";
            else
                refTo = "'" + refTo + "'";

            if (string.IsNullOrEmpty(adviceTo))
                adviceTo = "null";
            else
                adviceTo = "'" + adviceTo + "'";

            if (string.IsNullOrEmpty(corpTo))
                corpTo = "null";
            else
                corpTo = "'" + corpTo + "'";

            if (string.IsNullOrEmpty(openSubject))
                openSubject = "null";
            else
                openSubject = "'" + openSubject.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(issues))
                issues = "null";
            else
                issues = "'" + issues.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(answer))
                answer = "null";
            else
                answer = "'" + answer.Replace("'", "''") + "'";

            province = "'" + province + "'";

            if (string.IsNullOrEmpty(opNO))
                opNO = "null";
            else
                opNO = "'" + opNO + "'";

            custType = "'" + custType + "'";

            s_operator = "'" + s_operator + "'";

            if (string.IsNullOrEmpty(research10))
                research10 = "null";

            if (string.IsNullOrEmpty(researchDuration))
                researchDuration = "null";

            if (string.IsNullOrEmpty(researchExtra))
                researchExtra = "null";

            if (string.IsNullOrEmpty(clRec))
                clRec = "null";

            userGroup = "'" + userGroup + "'";

            owner = "'" + owner + "'";

            originalOwner = "'" + originalOwner + "'";

            if (string.IsNullOrEmpty(srcDef))
                srcDef = "null";
            else
                srcDef = "'" + srcDef + "'";

            if (string.IsNullOrEmpty(source))
                source = "null";
            else
                source = "'" + source + "'";

            if (string.IsNullOrEmpty(orderStatus))
                orderStatus = "null";
            else
                orderStatus = "'" + orderStatus + "'";

            if (string.IsNullOrEmpty(toneStart))
                toneStart = "null";
            else
                toneStart = "'" + toneStart + "'";

            if (string.IsNullOrEmpty(toneEnd))
                toneEnd = "null";
            else
                toneEnd = "'" + toneEnd + "'";

            if (string.IsNullOrEmpty(dateToCall))
                dateToCall = "null";
            else
                dateToCall = "TO_DATE( '" + dateToCall + "','DD-MON-YYYY HH24:MI') ";

            if (string.IsNullOrEmpty(dateBack))
                dateBack = "null";
            else
                dateBack = "TO_DATE( '" + dateBack + "','DD-MON-YYYY HH24:MI') ";

            string strSQL = "insert into FSTATISTICS(S_SEQUENCE, S_REC_NO, S_OPEN_REGION, S_L_OTHER, S_TELE_USED, S_LANG_E, S_LANG_F, S_OLANG_FLAG, " +
                "S_TELE, S_VISIT, S_WRITTEN, S_FAX, S_INTERNAL, S_EMAIL, S_WWW, S_SUBJ_STRING, S_REF_TO, S_ADVICE_TO, S_CORP_INFO_TO, S_ADVICE, S_CORP, S_REFERRAL, S_OPEN_SUBJECT, " +
                "S_VOICE_M, S_GENDER, S_COMMENTS, S_OPEN_ANSWER, S_PROV, S_OP_NO, S_CUSTOMER_TYPE, S_OPERATOR, S_RESEARCH_OVER_UNDER_10, S_RESEARCH_DURATION, S_RESEARCH_EXTRA, " +
                "S_CL_REC, S_DATE_INPUT, S_USER_GRP, S_OWNER, S_ORIGINAL_OWNER, S_COMPLAINT, S_SRC_DEF, S_ABORIGINAL, S_PRODUCT, S_SOURCE, S_ORDER_STATUS, S_TONE_START, S_TONE_END, " +
                "S_DATE_TO_CALLBACK, S_DATE_BACK, S_ORDER_STATUS_DATE) " +
                " values (s_sequence.nextval, " + ticketNumber + ", " + openRegion + ", " + otherLang + ", " + incLine + ", " + englishFlag + ", " + frenchFlag + ", " + otherFlag +
                ", " + phoneFlag + ", " + personFlag + ", " + mailFlag + ", " + faxFlag + ", " + exhibitFlag + ", " + emailFlag + ", " + webFlag + ", " + subject + ", " + refTo +
                ", " + adviceTo + ", " + corpTo + ", " + adviceFlag + ", " + corpFlag + ", " + referralFlag + ", " + openSubject + ", " + voiceMailFlag + ", " + gender +
                ", " + issues + ", " + answer + ", " + province + ", " + opNO + ", " + custType + ", " + s_operator + ", " + research10 + ", " + researchDuration + ", " +
                researchExtra + ", " + clRec + ", sysdate, " + userGroup + ", " + owner + ", " + originalOwner + "," + complaint + ", " + srcDef + ", " + classification + ", "
                + productFlag + ", " + source + ", " + orderStatus + ", " + toneStart + ", " + toneEnd + ", " + dateToCall + ", " + dateBack + ", sysdate)";

            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);

            return returnVal;

        }//InsertNewEnquiry

        public int UpdateEnquiry(string ticketNumber, string openRegion, string otherLang, string incLine, string englishFlag, string frenchFlag, string otherFlag,
           string phoneFlag, string personFlag, string mailFlag, string faxFlag, string exhibitFlag, string emailFlag, string webFlag, string subject, string refTo,
           string adviceTo, string corpTo, string adviceFlag, string corpFlag, string referralFlag, string openSubject, string voiceMailFlag, string gender,
           string issues, string answer, string province, string opNO, string custType, string edit_agent, string research10, string researchDuration, string researchExtra, string clRec,
           string complaint, string srcDef, string classification, string productFlag, string source, string orderStatus, string toneStart, string toneEnd, string dateToCall,
            string dateBack)
        {
            int returnVal = 0;

            //adjust the variables to form the query
            if (string.IsNullOrEmpty(openRegion))
                openRegion = "null";
            else
                openRegion = "'" + openRegion.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(otherLang))
                otherLang = "null";
            else
                otherLang = "'" + otherLang.Replace("'", "''") + "'";

            incLine = "'" + incLine + "'";

            if (string.IsNullOrEmpty(subject))
                subject = "null";
            else
                subject = "'" + subject.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(refTo))
                refTo = "null";
            else
                refTo = "'" + refTo + "'";

            if (string.IsNullOrEmpty(adviceTo))
                adviceTo = "null";
            else
                adviceTo = "'" + adviceTo + "'";

            if (string.IsNullOrEmpty(corpTo))
                corpTo = "null";
            else
                corpTo = "'" + corpTo + "'";

            if (string.IsNullOrEmpty(openSubject))
                openSubject = "null";
            else
                openSubject = "'" + openSubject.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(issues))
                issues = "null";
            else
                issues = "'" + issues.Replace("'", "''") + "'";

            if (string.IsNullOrEmpty(answer))
                answer = "null";
            else
                answer = "'" + answer.Replace("'", "''") + "'";

            province = "'" + province + "'";

            if (string.IsNullOrEmpty(opNO))
                opNO = "null";
            else
                opNO = "'" + opNO + "'";

            custType = "'" + custType + "'";

            edit_agent = "'" + edit_agent + "'";

            if (string.IsNullOrEmpty(research10))
                research10 = "null";

            if (string.IsNullOrEmpty(researchDuration))
                researchDuration = "null";

            if (string.IsNullOrEmpty(researchExtra))
                researchExtra = "null";

            if (string.IsNullOrEmpty(clRec))
                clRec = "null";

            if (string.IsNullOrEmpty(srcDef))
                srcDef = "null";
            else
                srcDef = "'" + srcDef + "'";

            if (string.IsNullOrEmpty(source))
                source = "null";
            else
                source = "'" + source + "'";

            if (string.IsNullOrEmpty(orderStatus))
                orderStatus = "null";
            else
                orderStatus = "'" + orderStatus + "'";

            if (string.IsNullOrEmpty(toneStart))
                toneStart = "null";
            else
                toneStart = "'" + toneStart + "'";

            if (string.IsNullOrEmpty(toneEnd))
                toneEnd = "null";
            else
                toneEnd = "'" + toneEnd + "'";

            if (string.IsNullOrEmpty(dateToCall))
                dateToCall = "null";
            else
                dateToCall = "TO_DATE( '" + dateToCall + "','DD-MON-YYYY HH24:MI') ";

            if (string.IsNullOrEmpty(dateBack))
                dateBack = "null";
            else
                dateBack = "TO_DATE( '" + dateBack + "','DD-MON-YYYY HH24:MI') ";

            string strSQL = "update FSTATISTICS SET S_OPEN_REGION = " + openRegion + ", S_L_OTHER = " + otherLang + ", S_TELE_USED = " + incLine + ", S_LANG_E = " + englishFlag +
                ", S_LANG_F = " + frenchFlag + ", S_OLANG_FLAG = " + otherFlag + ", S_TELE = " + phoneFlag + ", S_VISIT = " + personFlag + ", S_WRITTEN = " + mailFlag + ", S_FAX = " +
                    faxFlag + ", S_INTERNAL = " + exhibitFlag + ", S_EMAIL = " + emailFlag + ", S_WWW = " + webFlag + ", S_SUBJ_STRING = " + subject + ", S_REF_TO = " + refTo +
            ", S_ADVICE_TO = " + adviceTo + ", S_CORP_INFO_TO = " + corpTo + ", S_ADVICE = " + adviceFlag + ", S_CORP = " + corpFlag + ", S_REFERRAL = " + referralFlag +
            ", S_OPEN_SUBJECT = " + openSubject + ", S_VOICE_M = " + voiceMailFlag + ", S_GENDER = " + gender + ", S_COMMENTS = " + issues + ", S_OPEN_ANSWER = " + answer + ", S_PROV = " +
            province + ", S_OP_NO = " + opNO + ", S_CUSTOMER_TYPE = " + custType + ", S_RESEARCH_OVER_UNDER_10 = " + research10 + ", S_RESEARCH_DURATION = " + researchDuration +
            ", S_RESEARCH_EXTRA = " + researchExtra + ", S_CL_REC = " + clRec + ", S_COMPLAINT = " + complaint + ", S_SRC_DEF = " + srcDef + ", S_ABORIGINAL = " + classification +
            ", S_PRODUCT = " + productFlag + ", S_SOURCE = " + source + ", S_ORDER_STATUS =" + orderStatus + ", S_TONE_START = " + toneStart + ", S_TONE_END = " + toneEnd +
            ", S_DATE_TO_CALLBACK = " + dateToCall + ", S_DATE_BACK = " + dateBack + ", S_DATE_EDIT = sysdate, S_USER_EDIT = " + edit_agent +
            " WHERE S_REC_NO = " + ticketNumber;

            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);

            return returnVal;

        }//UpdateEnquiry

        public int UpdateOrderStatusDate(string ticketNumber)
        {
            int returnVal = 0;
            string strSQL = "update FSTATISTICS SET S_ORDER_STATUS_DATE = sysdate where S_REC_NO = " + ticketNumber;
            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//UpdateOrderStatusDate

        public int UpdateOrderStatus(string ticketNumber, string status)
        {
            int returnVal = 0;
            string strSQL = "update FSTATISTICS SET S_ORDER_STATUS = '" + status + "' where S_REC_NO = " + ticketNumber;
            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//UpdateOrderStatus

        public int UpdateOrderStatusHist(string ticketNumber, string status)
        {
            int returnVal = 0;
            string strSQL = "update f_order_status_history SET OSHIST_STATUS_CODE = '" + status + "' where OSHIST_SRECNO = " + ticketNumber;
            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//UpdateOrderStatusHist

        public int InsertOrderStatusHistory(string ticketNumber, string status, string userid)
        {
            int returnVal = 0;
            string strSQL = "insert into f_order_status_history(OSHIST_SEQ, OSHIST_SRECNO, OSHIST_STATUS_CODE, OSHIST_STATUS_U_ID, OSHIST_CHANGE_DATE) " +
                "values(OSHIST_SEQ.nextval, " + ticketNumber + ", '" + status + "', '" + userid + "', sysdate)";
            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//InsertOrderStatusHistory

        public int UpdateAssignedUser(string ticketNum, string userid, string userGroup)
        {
            int returnVal = 0;
            string strSQL = "update fstatistics set s_user_edit = '" + userid + "', s_owner = '" + userGroup + "' where s_rec_no = " + ticketNum;
            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//UpdateAssignedUser

        public int InsertAssignedToRecord(string ticketNum, string userid, string assignedUser)
        {
            int returnVal = 0;
            string strSQL = "insert into comment_audit_history values (" + ticketNum + ", '" + userid + "', sysdate, 'Assigned to/Assigné à " + assignedUser + "')";
            returnVal = ExecuteNonQuery(strSQL, CommandType.Text);
            return returnVal;

        }//InsertAssignedToRecord

        public int GetFirstRecordNumber(string incline)
        {
            string linesAllowed = getLinesAllowed(incline);
            string strSQL = "select min(s_rec_no) TicketNo from fstatistics where s_tele_used in (" + linesAllowed + ")";
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                return int.Parse(tempDS.Tables[0].Rows[0]["TicketNo"].ToString());
            else return -1;

        }//GetFirstRecordNumber

        public bool TicketIsValid(string ticketNumber, string incline)
        {
            string linesAllowed = getLinesAllowed(incline);
            string strSQL = "select (s_rec_no) TicketNo from fstatistics where s_rec_no = " + ticketNumber + " and s_tele_used in (" + linesAllowed + ")";
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;

        }//TicketIsValid

        public int GetMaxRecordNumber(string incline)
        {
            string linesAllowed = getLinesAllowed(incline);
            string strSQL = "select max(s_rec_no) TicketNo from fstatistics where s_tele_used in (" + linesAllowed + ")";
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                return int.Parse(tempDS.Tables[0].Rows[0]["TicketNo"].ToString());
            else return -1;

        }//GetMaxRecordNumber

        public int GenerateNewTicketNumber()
        {
            string strSQL = "select invoice_seq.nextval as TicketNo from dual";
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                return int.Parse(tempDS.Tables[0].Rows[0]["TicketNo"].ToString());
            else return -1;

        }//GenerateNewTicketNumber

        public int GetNextRecordNumber(string currentTicketNumber, string incline)
        {
            string linesAllowed = getLinesAllowed(incline);
            string strSQL = "select min(s_rec_no) TicketNo from fstatistics where s_rec_no > " + currentTicketNumber + " and s_tele_used in (" + linesAllowed + ")";
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                return int.Parse(tempDS.Tables[0].Rows[0]["TicketNo"].ToString());
            else return -1;

        }//GetNextRecordNumber

        public int GetPreviousRecordNumber(string currentTicketNumber, string incline)
        {
            string linesAllowed = getLinesAllowed(incline);
            string strSQL = "select max(s_rec_no) TicketNo from fstatistics where s_rec_no < " + currentTicketNumber + " and s_tele_used in (" + linesAllowed + ")";
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                return int.Parse(tempDS.Tables[0].Rows[0]["TicketNo"].ToString());
            else return -1;

        }//GetPreviousRecordNumber

        public int GetCriticalValue(string productNum)
        {
            int criticalValue = 0;
            productNum = "'" + productNum + "'";
            string strSQL = "select NVL(INVENTORY_CRIT_LEV, 0) as CRIT_VALUE from finventory where inventory_code = " + productNum;
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                criticalValue = int.Parse(tempDS.Tables[0].Rows[0]["CRIT_VALUE"].ToString());
            return criticalValue;

        }//GetCriticalValue

        public int GetWHByDelCode(string delcode)
        {
            delcode = "'" + delcode + "'";
            string strSQL = "select DEL_WH from FDEL_CODES where DEL_CODE = " + delcode;
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                return int.Parse(tempDS.Tables[0].Rows[0]["DEL_WH"].ToString());
            else return -1;

        }//GetWHByDelCode

        public DataSet GetUsers()
        {
            //perhaps this will be filtered by group
            string strSQL = "select u_name, u_id from f_user_id order by UPPER(u_name)";
            return Execute(strSQL, CommandType.Text);

        }//GetUsers

        public string GetUserGroup(string userid)
        {
            string userGroup = "";
            string strSQL = "select u_id_grp from f_user_id where u_id = '" + userid + "'";
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                userGroup = tempDS.Tables[0].Rows[0]["u_id_grp"].ToString();
            return userGroup;

        }//GetUserGroup

        public DataSet GetCommentHistory(string ticketnumber)
        {
            string strSQL = "select cah_comments, to_char(cah_date_edit, 'DD-MON-YYYY HH24:MI') as date_edit, cah_user_edit from comment_audit_history where cah_rec_no = " +
                ticketnumber + " order by cah_date_edit";
            return Execute(strSQL, CommandType.Text);

        }//GetCommentHistory

        public string GetScript(string teleNo, string language)
        {
            string script = "";
            string strSQL;
            if (language.Equals("FR"))
                strSQL = "select TELE_SCRIPT_FR ";
            else
                strSQL = "select TELE_SCRIPT_EN ";

            strSQL += "as TELE_SCRIPT from F_TELE where TELE_NO = '" + teleNo + "'";

            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                script = tempDS.Tables[0].Rows[0]["TELE_SCRIPT"].ToString();
            return script;

        }//GetScript

        public DataSet GetOrderStatusHistory(string ticketnumber, string language)
        {
            string strSQL = "select to_char(oshist_change_date, 'DD-MON-YYYY HH24:MI') as change_date, oshist_status_u_id as userid, ";
            if (language.Equals("FR"))
                strSQL += "ORDER_STATUS_DESC_F ORDER_STATUS_DESC ";
            else
                strSQL += "ORDER_STATUS_DESC_E ORDER_STATUS_DESC ";

            strSQL += "from f_order_status_history join f_order_status on oshist_status_code = order_status_code " +
                "where oshist_srecno = " + ticketnumber + " order by change_date desc";
            return Execute(strSQL, CommandType.Text);

        }//GetOrderStatusHistory

        public DataSet GetIncomingLinesByGroup(string language, int userlevel, string usergroup)
        {
            string strSQL = "";

            if (language.Equals("FR"))
                strSQL = "select TELESEQ,TELE_NO,TELE_DESC_FR as TELE_DESC,TELE_SCRIPT_FR as Script,TELE_SURVEY_DATA, TELE_FR_SURVEY as URL, TELE_REGION_DATA from F_TELE " +
                          "where TELE_NONACTIVE_CAMPAIGN = 0 ";
            else
                strSQL = "select TELESEQ,TELE_NO,TELE_DESC,TELE_SCRIPT_EN as Script,TELE_SURVEY_DATA, TELE_ENG_SURVEY as URL, TELE_REGION_DATA from F_TELE " +
                        "where TELE_NONACTIVE_CAMPAIGN = 0 ";

            if (userlevel > 1)
            {
                string groupsAllowed = "'" + usergroup + "'";
                if (groupsAllowed.Contains(','))
                    groupsAllowed = getGroupsAllowed(usergroup);

                strSQL += " and TELE_GRP_ACCESS in (" + groupsAllowed + ")";

            }// if the user level is greater than 1

            strSQL += " order by upper(TELE_DESC)";
            return Execute(strSQL, CommandType.Text);

        }//GetIncomingLinesByGroup

        public DataSet GetDeliveryMethods(string language, int userlevel, string usergroup)
        {
            string strSQL = "";

            if (language.Equals("FR"))
                strSQL = "select DEL_CODE,DEL_DEFINITION_FR as DEL_DEFINITION,DEL_WH " +
                         "from FDEL_CODES where nvl(DEL_INACTIVE,0) = 0 and DEL_CODE not in ('T1','T2') ";
            else
                strSQL = "select DEL_CODE,DEL_DEFINITION,DEL_WH " +
                         "from FDEL_CODES where nvl(DEL_INACTIVE,0) = 0 and DEL_CODE not in ('T1','T2') ";

            if (userlevel > 1)
            {

                string groupsAllowed = "'" + usergroup + "'";
                if (groupsAllowed.Contains(','))
                groupsAllowed = getGroupsAllowed(usergroup);

                strSQL += " and DEL_WH in (select WH_NUMBER from F_WAREHOUSE where upper(WH_GROUP) in (" + groupsAllowed + "))";

            }// if the user level is greater than 1

            if (language.Equals("FR"))
                strSQL += " order by upper(DEL_DEFINITION_FR)";
            else
                strSQL += " order by upper(DEL_DEFINITION)";

            return Execute(strSQL, CommandType.Text);

        }//GetDeliveryMethods

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

        private string getLinesAllowed(string incline)
        {
            string linesAllowed = "";
            string[] splitLines = incline.Split(',');

            if (splitLines.Length > 2)
            {
                for (int i = 0; i < splitLines.Length - 1; ++i)
                {
                    linesAllowed += "'" + splitLines[i].Trim() + "'";
                    if (i != splitLines.Length - 2)
                        linesAllowed += ", ";

                }// for the number of items seperated by commas, minus 1 since the last will be blank

            }// if there is more than 2 items (unless the field is empty, there is at least one item seperated by a comma, which creates two items)

            return linesAllowed;

        }//getLinesAllowed

        private string getProductsNotAllowed(string productCodes)
        {
            string productsNotAllowed = "";
            string[] splitCodes = productCodes.Split(',');

            if (splitCodes.Length > 2)
            {
                for (int i = 0; i < splitCodes.Length - 1; ++i)
                {
                    productsNotAllowed += "'" + splitCodes[i].Trim() + "'";
                    if (i != splitCodes.Length - 2)
                        productsNotAllowed += ", ";

                }// for the number of items seperated by commas, minus 1 since the last will be blank

            }// if there is more than 2 items (unless the field is empty, there is at least one item seperated by a comma, which creates two items)

            return productsNotAllowed;

        }//getProductsNotAllowed

        public string GetStatusForProduct(string inventoryCode)
        {
            string status = "";
            inventoryCode = "'" + inventoryCode + "'";
            string strSQL = "select INVENTORY_STATUS from FINVENTORY where INVENTORY_CODE = " + inventoryCode;
            DataSet tempDS = Execute(strSQL, CommandType.Text);
            if (tempDS.Tables[0].Rows.Count > 0)
                status = tempDS.Tables[0].Rows[0]["INVENTORY_STATUS"].ToString();
            return status;

        }//GetStatusForProduct

        public DataSet GetInventoryItem(string language, string inventoryCode, string wh)
        {
            string strSQL = "";
            inventoryCode = "'" + inventoryCode + "'";
            if (language.Equals("FR"))
                strSQL = "select INVENTORY_FRENCH_DEFINITION INVENTORY_DEF,";
            else
                strSQL = "select INVENTORY_PROD_DEF INVENTORY_DEF,";

            if (language.Equals("FR"))
                strSQL += "PROD_STATUS_DEF_FR PROD_STATUS_DEF, ";
            else
                strSQL += "PROD_STATUS_DEF, ";

            strSQL += "INVENTORY_STATUS, INVENTORY_REC_NO, nvl(inventory_unit_cost, 0) as UNIT_COST, nvl(inventory_ret_cost, 0) as UNIT_PRICE, INVENTORY_GST_EXEMPT, INVENTORY_PST_EXEMPT, "
                + "INVENTORY_STOCK_CHECK, INVENTORY_SIZE, INVENTORY_UM, INVENTORY_TYPE, INVENTORY_PROD_CAT_DEPT, INVENTORY_ACCOUNT_NO, INVENTORY_PO_NUM, INVENTORY_SUPPLIER, " +
                "NVL(commited, 0) as CMTD, (NVL(WH0, 0) + NVL(WH1, 0) + NVL(WH2, 0) + NVL(WH3, 0) + NVL(WH4, 0) + NVL(WH5, 0) + NVL(WH6, 0) + NVL(WH7,0) + NVL(WH8,0) + NVL(WH9, 0)) as " +
                "TOTAL_QTY, INVENTORY_KIT, INVENTORY_KIT_ADD, INVENTORY_SUP_PART_NO from FINVENTORY join FPROD_STATUS on INVENTORY_STATUS = PROD_STATUS_CD";
            if (!string.IsNullOrEmpty(wh))
                strSQL += " join F_WAREHOUSE_INV on INVENTORY_CODE = WI_INV_CODE";

            strSQL += " full outer join v_inv_warehouses on finventory.inventory_code=v_inv_warehouses.productcode" +
               " full outer join v_inv_commited on finventory.inventory_code=v_inv_commited.productcode" +
            " where INVENTORY_CODE = " + inventoryCode;
            if (!string.IsNullOrEmpty(wh))
                strSQL += " and WI_WH_NUM = " + wh;

            if (language.Equals("FR"))
                strSQL += " order by upper(trim(INVENTORY_FRENCH_DEFINITION))";
            else
                strSQL += " order by upper(trim(INVENTORY_PROD_DEF))";

            return Execute(strSQL, CommandType.Text);

        }//GetInventoryItem

        public DataSet GetKitItems(string recNo)
        {
            string strSQL = "select KIT_PROD_REC, KIT_INV_REC, KIT_PROD, INVENTORY_REC_NO " +
                     "from F_KIT join FINVENTORY on KIT_INV_REC = INVENTORY_REC_NO " +
                     "where INVENTORY_REC_NO = " + recNo;

            return Execute(strSQL, CommandType.Text);

        }//GetKitItems

        public DataSet GetInventory(string language, string usergroup, string condition, string products, string sWH)
        {
            if (string.IsNullOrEmpty(condition))
                condition = "All";
            if (condition.Contains("'"))
                condition = condition.Replace("'", "''");
            if (sWH.Contains("All"))
                sWH = "";

            string groupsAllowed = "'" + usergroup + "'";
            if (groupsAllowed.Contains(','))
                groupsAllowed = getGroupsAllowed(usergroup);

            string sWHQuery = "and " + sWH + " > 0";
            string strSQL;
            if (language.Equals("FR"))
                strSQL = "select distinct INVENTORY_FRENCH_DEFINITION INVENTORY_DEF,";
            else
                strSQL = "select distinct INVENTORY_PROD_DEF INVENTORY_DEF,";

            if (language.Equals("FR"))
                strSQL += "PROD_STATUS_DEF_FR PROD_STATUS_DEF, ";
            else
                strSQL += "PROD_STATUS_DEF, ";

            strSQL += "INVENTORY_REC_NO, INVENTORY_CODE, INVENTORY_BRANCH , INVENTORY_COMPONENT, INVENTORY_STAND_ALONE, INVENTORY_STATUS, " +
                  " NVL(INVENTORY_RET_COST,0) PRICE, NVL(INVENTORY_KIT,0) INVENTORY_KIT, NVL(WH0, 0) WH0, NVL(WH1, 0) WH1, NVL(WH2, 0) WH2, NVL(WH3, 0) WH3, NVL(WH4, 0) WH4, " +
                  "NVL(WH5, 0) WH5, NVL(WH6, 0) WH6, NVL(WH7,0) WH7, NVL(WH8,0) WH8, NVL(WH9, 0) WH9, NVL(commited, 0) as CMTD, PROD_STATUS_CD, " +
                  " (NVL(WH0, 0) + NVL(WH1, 0) + NVL(WH2, 0) + NVL(WH3, 0) + NVL(WH4, 0) + NVL(WH5, 0) + NVL(WH6, 0) + NVL(WH7,0) + NVL(WH8,0) + NVL(WH9, 0)) as TOTAL_QTY" +
                  " From finventory";
            if (!string.IsNullOrEmpty(sWH))
                strSQL += " join F_WAREHOUSE_INV on INVENTORY_CODE = WI_INV_CODE";

            strSQL += " full outer join v_inv_warehouses on finventory.inventory_code=v_inv_warehouses.productcode" +
               " full outer join v_inv_commited on finventory.inventory_code=v_inv_commited.productcode" +
               " join FPROD_STATUS on INVENTORY_STATUS = PROD_STATUS_CD" +
               " where (NVL(INVENTORY_NATIONAL,'TRUE') = 'TRUE' or INVENTORY_GROUP in (" + groupsAllowed + "))" +
               " and INVENTORY_STATUS in ('A','B','C','N', 'E') and (inventory_component = 'NO' or inventory_stand_alone = 'YES')";
            if (!string.IsNullOrEmpty(sWH))
                strSQL += sWHQuery;


            if (!condition.Equals("All") && !condition.Equals("New") && language.Equals("EN"))
                strSQL += "and UPPER(INVENTORY_PROD_DEF) LIKE '%" + condition.ToUpper() + "%'";

            if (!condition.Equals("All") && !condition.Equals("New") && language.Equals("FR"))
                strSQL += "and UPPER(INVENTORY_FRENCH_DEFINITION) LIKE '%" + condition.ToUpper() + "%'";

            if (condition.Equals("New"))
                strSQL += "and INVENTORY_LIST_NO = 1 ";

            if (!string.IsNullOrEmpty(products))
            {
                string productsNotAllowed = getProductsNotAllowed(products);
                strSQL += " and INVENTORY_CODE not in (" + productsNotAllowed + ") ";
            }

            if (language.Equals("FR"))
                strSQL += " order by upper(trim(INVENTORY_FRENCH_DEFINITION))";
            else
                strSQL += " order by upper(trim(INVENTORY_PROD_DEF))";

            return Execute(strSQL, CommandType.Text);

        }//GetInventory

        public DataSet GetInventoryElectronic(string language, string usergroup, string condition, string products, string sWH)
        {
            if (string.IsNullOrEmpty(condition))
                condition = "All";
            if (condition.Contains("'"))
                condition = condition.Replace("'", "''");
            if (sWH.Contains("All"))
                sWH = "";

            string groupsAllowed = "'" + usergroup + "'";
            if (groupsAllowed.Contains(','))
                groupsAllowed = getGroupsAllowed(usergroup);

            string sWHQuery = "and " + sWH + " > 0";
            string strSQL;
            if (language.Equals("FR"))
                strSQL = "select distinct INVENTORY_FRENCH_DEFINITION INVENTORY_DEF,";
            else
                strSQL = "select distinct INVENTORY_PROD_DEF INVENTORY_DEF,";

            if (language.Equals("FR"))
                strSQL += "PROD_STATUS_DEF_FR PROD_STATUS_DEF, ";
            else
                strSQL += "PROD_STATUS_DEF, ";

            strSQL += "INVENTORY_REC_NO, INVENTORY_CODE, INVENTORY_BRANCH , INVENTORY_COMPONENT, INVENTORY_STAND_ALONE, INVENTORY_STATUS, " +
                  " NVL(INVENTORY_RET_COST,0) PRICE, NVL(INVENTORY_KIT,0) INVENTORY_KIT, NVL(WH0, 0) WH0, NVL(WH1, 0) WH1, NVL(WH2, 0) WH2, NVL(WH3, 0) WH3, NVL(WH4, 0) WH4, " +
                  "NVL(WH5, 0) WH5, NVL(WH6, 0) WH6, NVL(WH7,0) WH7, NVL(WH8,0) WH8, NVL(WH9, 0) WH9, NVL(commited, 0) as CMTD, PROD_STATUS_CD, " +
                  " (NVL(WH0, 0) + NVL(WH1, 0) + NVL(WH2, 0) + NVL(WH3, 0) + NVL(WH4, 0) + NVL(WH5, 0) + NVL(WH6, 0) + NVL(WH7,0) + NVL(WH8,0) + NVL(WH9, 0)) as TOTAL_QTY" +
                  " From finventory";
            if (!string.IsNullOrEmpty(sWH))
                strSQL += " join F_WAREHOUSE_INV on INVENTORY_CODE = WI_INV_CODE";

            strSQL += " full outer join v_inv_warehouses on finventory.inventory_code=v_inv_warehouses.productcode" +
               " full outer join v_inv_commited on finventory.inventory_code=v_inv_commited.productcode" +
               " join FPROD_STATUS on INVENTORY_STATUS = PROD_STATUS_CD" +
               " where (NVL(INVENTORY_NATIONAL,'TRUE') = 'TRUE' or INVENTORY_GROUP in (" + groupsAllowed + "))" +
               " and INVENTORY_STATUS in ('A','B','C','N', 'E') and (inventory_component = 'NO' or inventory_stand_alone = 'YES')";
            strSQL += " and inventory_code in (select item from f_inventory_web where (inv_html_brk = 'NO' or inv_html_brk = 'NO') and not (inv_pdf_filename is null and inventory_abstract_eng is null and inv_pdf_filename_fr is null and inventory_abstract_french is null))";

            if (!string.IsNullOrEmpty(sWH))
                strSQL += sWHQuery;


            if (!condition.Equals("All") && !condition.Equals("New") && language.Equals("EN"))
                strSQL += "and UPPER(INVENTORY_PROD_DEF) LIKE '%" + condition.ToUpper() + "%'";

            if (!condition.Equals("All") && !condition.Equals("New") && language.Equals("FR"))
                strSQL += "and UPPER(INVENTORY_FRENCH_DEFINITION) LIKE '%" + condition.ToUpper() + "%'";

            if (condition.Equals("New"))
                strSQL += "and INVENTORY_LIST_NO = 1 ";

            if (!string.IsNullOrEmpty(products))
            {
                string productsNotAllowed = getProductsNotAllowed(products);
                strSQL += " and INVENTORY_CODE not in (" + productsNotAllowed + ") ";
            }

            if (language.Equals("FR"))
                strSQL += " order by upper(trim(INVENTORY_FRENCH_DEFINITION))";
            else
                strSQL += " order by upper(trim(INVENTORY_PROD_DEF))";

            return Execute(strSQL, CommandType.Text);

        }//GetInventoryElectronic

        public DataSet GetOrderStatusRecords(string language, string searchType, string lowVal, string highVal, string status, string group, string selectAllIncompleteFlag)
        {
            string strSQL;
            
            strSQL = "select * from (";
            if (language.Equals("FR"))
                strSQL += "select ORDER_STATUS_DESC_F as ORDER_STATUS_DESC, ";
            else
                strSQL += "select ORDER_STATUS_DESC_E as ORDER_STATUS_DESC, ";

            strSQL += "S_REC_NO, to_char(S_DATE_INPUT, 'DD-MON-YYYY') S_DATE_IN, S_DATE_INPUT, S_OPERATOR, S_OWNER||'-'||S_OPERATOR GRP, decode(S_DATE_SENT,null,'N','Y') SDATESENT, "
            + "decode(S_PRODUCT,1,'Y','N') SPRODUCT, decode(S_COMMENTS,null,'N','Y') SCOMMENTS, decode(S_OPEN_ANSWER,null,'N','Y') SOPENANSWER, S_COMMENTS, S_OPEN_ANSWER, S_DATE_BACK, " +
            "S_PRODUCT, S_DATE_SENT, to_char(S_ORDER_STATUS_DATE, 'DD-MON-YYYY') STATUS_DATE, S_ORDER_STATUS_DATE, to_char(S_DATE_TO_CALLBACK, 'DD-MON-YYYY') DATE_TO_CALLBACK, " +
            "S_DATE_TO_CALLBACK, S_OWNER, S_USER_EDIT, decode(FS_LNAME,null,'',FS_LNAME)||decode(FS_FNAME,null,'',decode(FS_LNAME,null,FS_FNAME,' - '||FS_FNAME)) CNAME, " +
            "UPPER(S_USER_EDIT) EDIT_USER from FSTATISTICS join F_ORDER_STATUS on s_order_status = order_status_code join fsale on s_rec_no = fs_order_num";

            if (searchType.Equals("Incomplete") || selectAllIncompleteFlag.ToUpper().Equals("Y"))
                strSQL += " where s_order_status <> '099'";
            else
            {
                if (searchType.Equals("Date"))
                {
                    if (!string.IsNullOrEmpty(lowVal) && !string.IsNullOrEmpty(highVal))
                        strSQL += " where s_date_input >= '" + lowVal + "' and s_date_input <= TO_DATE('" + highVal + " 23:59','DD-MON-YYYY HH24:MI')";

                    else
                    {
                        if (!string.IsNullOrEmpty(lowVal))
                            strSQL += " where s_date_input >= '" + lowVal + "'";

                        if (!string.IsNullOrEmpty(highVal))
                            strSQL += " where s_date_input <= '" + highVal + "'";
                    }

                }// if it's a date search
                else
                {
                    if (!string.IsNullOrEmpty(lowVal) && !string.IsNullOrEmpty(highVal))
                        strSQL += " where s_rec_no >= " + lowVal + " and s_rec_no <= " + highVal;

                    else
                    {
                        if (!string.IsNullOrEmpty(lowVal))
                            strSQL += " where s_rec_no >= " + lowVal;

                        if (!string.IsNullOrEmpty(highVal))
                            strSQL += " where s_rec_no <= " + highVal;
                    }

                } // if it's a ticket search

                if (!string.IsNullOrEmpty(status))
                    strSQL += " and s_order_status = '" + status + "'";

                if (!string.IsNullOrEmpty(group))
                    strSQL += " and s_owner = '" + group + "'";

                strSQL += " order by s_rec_no desc";

            }//if it's not an incomplete search

            strSQL += ") ";
            strSQL += " where rownum <= 2000 ";


            return Execute(strSQL, CommandType.Text);

        }//GetOrderStatusRecords

        public string GetOrderCNAME(string ticket)
        {
            DataSet TempDS;
            string strSQL;
            string returnVal = "";

            strSQL = "select ";

            strSQL += "decode(FS_LNAME,null,'',FS_LNAME)||decode(FS_FNAME,null,'',decode(FS_LNAME,null,FS_FNAME,' - '||FS_FNAME)) CNAME " +
            " from FSALE where fs_order_num = " + ticket;

            TempDS = Execute(strSQL, CommandType.Text);
            if (TempDS.Tables[0].Rows.Count > 0)
            {
                returnVal = TempDS.Tables[0].Rows[0][0].ToString();
            }
            return returnVal;


        }//GetOrderStatusRecords

        public DataSet GetKnowledgeRecords(string language, string condition)
        {
            string strSQL = "select ref_seq, ref_rec_no, ref_first_name, ref_surname, ref_tele_no, ref_email,";

            if (language.Equals("FR"))
                strSQL += "ref_fr_desc ref_desc, ref_fr_instr ref_subject";
            else
                strSQL += "ref_resp_desc ref_desc, ref_title ref_subject";

            strSQL += " from freference ";

            strSQL += " where REF_ACTIVE = 'YES'";

            if (!string.IsNullOrEmpty(condition))
            {
                if (condition.Contains("'"))
                    condition = condition.Replace("'", "''");

                if (language.Equals("FR"))
                    strSQL += "and (UPPER(ref_fr_desc) LIKE '%" + condition.ToUpper() + "%' or UPPER(ref_fr_instr) LIKE '%" + condition.ToUpper() + "%')";
                else
                    strSQL += "and (UPPER(ref_resp_desc) LIKE '%" + condition.ToUpper() + "%' or UPPER(ref_title) LIKE '%" + condition.ToUpper() + "%')";

            }// if there is a search condition

            if (language.Equals("FR"))
                strSQL += " order by upper(trim(ref_fr_instr))";
            else
                strSQL += " order by upper(trim(ref_title))";

            return Execute(strSQL, CommandType.Text);

        }//GetKnowledgeRecords

        public DataSet getKnowledgeDetails(string seqNumber, string language)
        {
            string strSQL = "";
            if (language.Equals("FR"))
                strSQL += "SELECT TELE_DESC_FR as TELE_DESC, IND_FR_DEF as IND_DEF, BR_FR_DEF as BR_DEF, ";
            else
                strSQL += "SELECT TELE_DESC, IND_GEN_DEF as IND_DEF, BR_DEF, ";

            strSQL += " ref_active, ref_email, ref_url, ref_referral, ref_qa, ref_prog_info, ref_fr_instr, ref_fr_desc, ref_operator, ref_company, ref_campaign, "
            + "TO_CHAR(ref_date_input, 'DD-MON-YYYY') as input_date, TO_CHAR(ref_date_amend, 'DD-MON-YYYY') as edit_date, ref_rec_no, ref_address_1, ref_address_2, ref_resp_desc, "
            +"ref_title, ref_postalcode, ref_fax_no, ref_tele_no, ref_first_name, ref_surname, ref_reg_code, ref_key_subj FROM freference left join f_tele on tele_no = ref_campaign "
            + "left join findustry on ref_key_subj = industry_code left join f_branch on ref_reg_code = br_account_code " +
            "where ref_rec_no = " + seqNumber;

            return Execute(strSQL, CommandType.Text);

        }//getKnowledgeDetails

        public DataSet GetUserWHs(string usergroup, string lang)
        {
            string groupsAllowed = "'" + usergroup + "'";
            if (groupsAllowed.Contains(','))
            groupsAllowed = getGroupsAllowed(usergroup);
            string strSQL = "select WH_NUMBER, ";
            if (lang.Equals("FR"))
                strSQL += "WH_DESC_FR as WH_DESC";
            else
                strSQL += "WH_DESC";

            strSQL += " from F_WAREHOUSE where (WH_GROUP in (" + groupsAllowed + ")) order by wh_number";

            return Execute(strSQL, CommandType.Text);

        }//GetUserWHs

        public DataSet GetOrderStatus(string language)
        {
            string strSQL = "";

            if (language.Equals("FR"))
                strSQL = "select ORDER_STATUS_CODE, ORDER_STATUS_DESC_F as ORDER_STATUS_DESC from F_ORDER_STATUS where (ORDER_STATUS_INACTIVE  <> 1 or ORDER_STATUS_INACTIVE IS NULL) " +
                          "order by ORDER_STATUS_DESC_F ";
            else
                strSQL = "select ORDER_STATUS_CODE, ORDER_STATUS_DESC_E as ORDER_STATUS_DESC from F_ORDER_STATUS where (ORDER_STATUS_INACTIVE  <> 1 or ORDER_STATUS_INACTIVE IS NULL) " +
                          "order by ORDER_STATUS_DESC_E ";

            return Execute(strSQL, CommandType.Text);

        }//GetOrderStatus

        public DataSet GetGroups(string language)
        {
            string strSQL = "";

            if (language.Equals("FR"))
                strSQL = "select own_gen_def_fr as group_def, ";
            else
                strSQL = "select own_gen_def as group_def, ";

            strSQL += "own_code from fowner";

            if (language.Equals("FR"))
                strSQL += " ORDER BY upper(OWN_GEN_DEF_FR)";
            else
                strSQL += " ORDER BY upper(OWN_GEN_DEF)";

            return Execute(strSQL, CommandType.Text);

        }//GetGroups

        public DataSet GetAllOrderStatuses(string language)
        {
            string strSQL = "";

            if (language.Equals("FR"))
                strSQL = "select ORDER_STATUS_DESC_F as ORDER_STATUS_DESC,";
            else
                strSQL = "select ORDER_STATUS_DESC_E as ORDER_STATUS_DESC,";

            strSQL += " ORDER_STATUS_CODE from F_ORDER_STATUS ";

            if (language.Equals("FR"))
                strSQL += "order by ORDER_STATUS_DESC_F";
            else
                strSQL += "order by ORDER_STATUS_DESC_E";

            return Execute(strSQL, CommandType.Text);

        }//GetAllOrderStatuses

        public DataSet GetProvinces(string language)
        {
            string strSQL;
            if (language.Equals("FR"))
                strSQL = "select distinct PROV_CODE,PROV_FDEFINITION as Province, ''''||REG_PROVCODE||'''' as ValidPopups from FPROV_CODES, F_PROV_REGIONS where PROV_CODE = REG_PROVCODE(+) and NVL(REG_INACTIVE(+),0) = 0 order by PROV_CODE";
            else
                strSQL = "select distinct PROV_CODE,PROV_DEFINITION as Province, ''''||REG_PROVCODE||'''' as ValidPopups from FPROV_CODES, F_PROV_REGIONS where PROV_CODE = REG_PROVCODE(+) and NVL(REG_INACTIVE(+),0) = 0 order by PROV_CODE";

            return Execute(strSQL, CommandType.Text);

        }//GetProvinces

        public DataSet GetSources(string language)
        {
            string strSQL;
            if (language.Equals("FR"))
                strSQL = "select PROMOTION_CODE,PROMO_DEF_FR as PROM_DEFINITION from FPROMO_CODES order by PROM_DEFINITION";
            else
                strSQL = "select PROMOTION_CODE,PROM_DEFINITION as PROM_DEFINITION from FPROMO_CODES order by PROM_DEFINITION";

            return Execute(strSQL, CommandType.Text);

        }//GetSources

        public DataSet GetSourcesForSubject(string language, string subjCode)
        {
            string strSQL = "";
            if (language.Equals("FR"))
                strSQL = "select PROMO_DEF_FR as PROM_DEFINITION, ";
            else
                strSQL = "select PROM_DEFINITION, ";

            strSQL += "PROMOTION_CODE from FPROMO_CODES join SUBJ_PROMO_LINKS on PR_CODE = PROMOTION_CODE where SUBJ_CODE = '" + subjCode + "' order by PROM_DEFINITION";

            return Execute(strSQL, CommandType.Text);

        }//GetSourcesForSubject

        public DataSet GetCustTypes(string language)
        {
            string strSQL;
            if (language.Equals("FR"))
                strSQL = "select AFF_CODE,AFF_GEN_DEF_FR as AFF_GEN_DEF from F_AFFILIATION";
            else
                strSQL = "select AFF_CODE,AFF_GEN_DEF from F_AFFILIATION";

            strSQL += " where nvl(aff_inactive,0) = 0 order by upper(AFF_GEN_DEF)";

            return Execute(strSQL, CommandType.Text);

        }//GetCustTypes

        public DataSet GetEmailDetails()
        {
            string strSQL;
            strSQL = "select con_email_server, con_email_orders from fconstant";
            
            return Execute(strSQL, CommandType.Text);
        }

        public DataSet GetWebInventoryDetails(string strCode)
        {
            string strSQL;
            strSQL = "select item, title,           inv_pdf_filename,    inventory_abstract_eng,  ";
            strSQL = strSQL + "    title_alternate, inv_pdf_filename_fr, inventory_abstract_french, ";
            strSQL = strSQL + "    inv_html_brk, inv_pdf_brk ";
            strSQL = strSQL + " from f_inventory_web ";
            strSQL = strSQL + " where item = '" + strCode + "' ";

            return Execute(strSQL, CommandType.Text);
        }

        public DataSet GetUnsentProducts(string ticket)
        {
            string strSQL = "";

            strSQL = "select PR_SEQUENCE, PR_INVENTORY_CODE from FPROD_REQ_EMAIL ";
            strSQL += "where PR_ORDER_NUM = " + ticket + " AND PR_DATE_SENT is NULL ";

            return Execute(strSQL, CommandType.Text);

        }//GetUnsentProducts

        public int UpdateEmailOrders(string prsequence, string note)
        {
            string strSQL = "update FPROD_REQ_EMAIL set PR_DATE_SENT = sysdate, pr_notes = '" + note.Replace("'", "''") + "', pr_delivery_mode = 'E' , pr_process_status = 3 ";
            strSQL = strSQL +  " where PR_SEQUENCE = " + prsequence ;

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }//UpdateEmailOrders

        public int UpdateDeliveredDate(string ticketnumber)
        {
            string strSQL = "update fsale set fs_delivered = sysdate ";
            strSQL = strSQL + " where fs_order_num = " + ticketnumber;

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }//UpdateEmailOrders

        public int UpdateAnswerField(string ticketnumber, string answer)
        {
            string strSQL = "update fstatistics set S_OPEN_ANSWER = '" + answer.Replace("'","''") + "' ";
            strSQL = strSQL + " where s_rec_no = " + ticketnumber;

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }

        public DataSet GetTicketCount(string ticketNumber)
        {
            string strSQL = "select count(*) from fstatistics where s_rec_no = " + ticketNumber;
            return Execute(strSQL, CommandType.Text);
        }

        public DataSet GetOrderedProductCount(string ticketNumber)
        {
            string strSQL = "select count(*) from fprod_req where pr_order_num = " + ticketNumber;
            return Execute(strSQL, CommandType.Text);
    
        }

        public int UpdateDeliveryMethod(string ticketNumber, string strMethod)
        {
            string strSQL = "update fsale set FS_DELIVERY = '" + strMethod + "' ";
            strSQL = strSQL + " where FS_ORDER_NUM = " + ticketNumber;

            return ExecuteNonQuery(strSQL, CommandType.Text);

        }
        public int UpdateDeliveredFlag(string ticketNumber, string strFlagValue)
        {
            string strSQL = "update fsale set FS_DELIVERED_FLAG = '" + strFlagValue + "' ";
            strSQL = strSQL + " where FS_ORDER_NUM = " + ticketNumber;

            return ExecuteNonQuery(strSQL, CommandType.Text);
        }

        public int UpdateStatusFlag(string ticketNumber, string strStatus)
        {
            //string strSQL = "update fsale set FS_DELIVERED_FLAG = '" + strFlagValue + "' ";
            //strSQL = strSQL + " where FS_ORDER_NUM = " + ticketNumber;

            //return ExecuteNonQuery(strSQL, CommandType.Text);
            return 1;
        }

        public DataSet GetControlValue(string parameter)
        {
            string strSQL = "";

            strSQL = "select cntr_description, cntr_value, cntr_comment from f_control ";
            strSQL += "where upper(CNTR_PARAMETER) = '" + parameter.ToUpper() + "'  ";

            return Execute(strSQL, CommandType.Text);

        }//GetControlValue

    }//class

}//namespace
