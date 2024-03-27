using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CallBaseMock
{
    public class Client
    {
        public int c_rec_no { get; set; }
        public string c_firstname_intl { get; set; }
        public string c_surname { get; set; }
        public string c_salutation { get; set; }
        public string c_organization { get; set; }
        public string c_job_title { get; set; }
        public string c_street { get; set; }
        public string c_address_line_2 { get; set; }
        public string c_prov_code { get; set; }
        public string c_province_name { get; set; }
        public string c_city { get; set; }
        public string c_country { get; set; }
        public string c_telephone { get; set; }
        public string c_fax_no { get; set; }
        public string c_email { get; set; }
        public string c_www { get; set; }
        public string c_language { get; set; }
        public int c_status { get; set; }
        public string c_postal_code { get; set; }
        public string c_customer_type { get; set; }
        public string c_delivery_mode { get; set; }
        //C_DATE_INPUT, C_DATE_AMENDED, C_OPERATOR, C_OWNER, C_USER_GRP, C_DATE_USED
        public string c_date_input { get; set; }
        public string c_date_amended { get; set; }
        public string c_operator { get; set; }
        public string c_owner { get; set; }
        public string c_user_grp { get; set; }
        public string c_date_used { get; set; }
    }
}