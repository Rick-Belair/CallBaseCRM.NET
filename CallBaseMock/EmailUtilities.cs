using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Data;
using DataAccess;
using System.Configuration;

namespace CallBaseMock
{
    public class EmailUtilities
    {

        private MailMessage m_mm;
        private SmtpClient m_client;
        private InboundDB m_db;
        private List<string> m_lstAttachments;

        public string GetFileNameFromURL(string strURL)
        {
            int c;
            string strResult = "";

            for (c = 1; c < (strURL.Length - 1); c++)
            {
                if (strURL.Substring(c, 1) == "/" || strURL.Substring(c, 1) == "\\")
                {
                    strResult = strURL.Substring(c + 1);
                }
            }
            return strResult;
        }

        public bool SendEmail(string strCode, string strEmailDestination, string strLang, string strAddedMessage, bool blnIncludeAttachments)
        {
            bool result = true;
            string strEmailTitle;
            string strEmailTitle_e;
            string strEmailMessage;
            string strEmailMessage_e;
            string strEmailMessage1_e;
            string strEmailMessage2_e;
            string strEmailMessage3_e;

            string strEmailTitle_f;
            string strEmailMessage_f;
            string strEmailMessage1_f;
            string strEmailMessage2_f;
            string strEmailMessage3_f;

            string strEmailSignoff_e;
            string strEmailSignoff_f;
            string strHost;
            string strMailSender;

            string strDirectory;
            string strUrlPdf_e;
            string strUrlPdf_f;
            string strUrl_e;
            string strUrl_f;
            string strDocTitle_e;
            string strDocTitle_f;

            bool boolIsPDF_e, boolIsPDF_f;
            strDirectory = TemporaryDirectory();
            InboundDB db = new InboundDB();
            LanguageDB ldb = new LanguageDB();

            strLang = strLang.ToLower();

            // Get the text for the email message
            strEmailTitle_e = ldb.GetLabel("InboundTracking", "MailSubject", "EN");
            strEmailMessage1_e = ldb.GetLabel("InboundTracking", "MailMessage1", "EN");
            strEmailMessage2_e = ldb.GetLabel("InboundTracking", "MailMessage2", "EN");
            strEmailMessage3_e = ldb.GetLabel("InboundTracking", "MailMessage3", "EN");

            strEmailSignoff_e = ldb.GetLabel("InboundTracking", "MailMessageSignoff1", "EN") + "\n" + ldb.GetLabel("InboundTracking", "MailMessageSignoff2", "EN"); 

            strEmailTitle_f = ldb.GetLabel("InboundTracking", "MailSubject", "FR");
            strEmailMessage1_f = ldb.GetLabel("InboundTracking", "MailMessage1", "FR");
            strEmailMessage2_f = ldb.GetLabel("InboundTracking", "MailMessage2", "FR");
            strEmailMessage3_f = ldb.GetLabel("InboundTracking", "MailMessage3", "FR");
            strEmailSignoff_f = ldb.GetLabel("InboundTracking", "MailMessageSignoff1", "FR") + "\n" + ldb.GetLabel("InboundTracking", "MailMessageSignoff2", "FR");

            // Get information about the email server 
            
            DataSet ds = db.GetEmailDetails();
            strHost = ds.Tables[0].Rows[0][0].ToString();
            strMailSender = ds.Tables[0].Rows[0][1].ToString();

            // Get information about the product to be sent

            ds = db.GetWebInventoryDetails(strCode);
            strDocTitle_e = ds.Tables[0].Rows[0]["title"].ToString(); ;
            strDocTitle_f = ds.Tables[0].Rows[0]["title_alternate"].ToString(); ;
            strUrl_e = ds.Tables[0].Rows[0]["inventory_abstract_eng"].ToString();
            strUrl_f = ds.Tables[0].Rows[0]["inventory_abstract_french"].ToString();
            if (ds.Tables[0].Rows[0]["inv_pdf_filename"] == null || ds.Tables[0].Rows[0]["inv_pdf_filename"].ToString().Length == 0)
            {
                boolIsPDF_e = false;
                strUrlPdf_e = "";
            }
            else
            {
                boolIsPDF_e = true;
                strUrlPdf_e = ds.Tables[0].Rows[0]["inv_pdf_filename"].ToString();
            }
            if (ds.Tables[0].Rows[0]["inv_pdf_filename_fr"] == null || ds.Tables[0].Rows[0]["inv_pdf_filename_fr"].ToString().Length == 0)
            {
                strUrlPdf_f = "";
                boolIsPDF_f = false;
            }
            else
            {
                boolIsPDF_f = true;
                strUrlPdf_f = ds.Tables[0].Rows[0]["inv_pdf_filename_fr"].ToString();
            }

            if (strUrl_e.Length == 0)
            {
                if (boolIsPDF_e == true)
                {
                    strUrl_e = strUrlPdf_e;
                }
            }

            if (strUrl_f.Length == 0)
            {
                if (boolIsPDF_f == true)
                {
                    strUrl_f = strUrlPdf_e;
                }
            }

            if (strUrl_e == "")
            {
                strUrl_e = strUrl_f;
            }
            else if (strUrl_f == "")
            {
                strUrl_f = strUrl_e;
            }

            // Build the message

            strEmailMessage3_e = strEmailMessage3_e.Replace("%1", strDocTitle_e);
            strEmailMessage3_e = strEmailMessage3_e.Replace("%2", strUrl_e);
            strEmailMessage_e = strEmailMessage1_e;
            if (boolIsPDF_e == true)
            {
                strEmailMessage_e = strEmailMessage_e + "\n\n" + strEmailMessage2_e;
            }
            strEmailMessage_e = strEmailMessage_e + "\n\n" + strEmailMessage3_e;

            strEmailMessage3_f = strEmailMessage3_f.Replace("%1", strDocTitle_f);
            strEmailMessage3_f = strEmailMessage3_f.Replace("%2", strUrl_f);
            strEmailMessage_f = strEmailMessage1_f;
            strEmailMessage_f = strEmailMessage_f.Replace("%2", strUrl_f);
            if (boolIsPDF_f == true)
            {
                strEmailMessage_f = strEmailMessage_f + "\n\n" + strEmailMessage2_f;
            }
            strEmailMessage_f = strEmailMessage_f + "\n\n" + strEmailMessage3_f;

            if (strLang == "f")
            {
                strEmailTitle = strEmailTitle_f + " / " + strEmailTitle_e;
                strEmailMessage = strEmailMessage_f;
                if (strAddedMessage.Length > 0)
                {
                    strEmailMessage = strEmailMessage + "\n\n" + strAddedMessage;
                }
                strEmailMessage = strEmailMessage + "\n\n" + strEmailSignoff_f +  "\n\n---\n\n" + strEmailMessage_e + "\n\n" + strEmailSignoff_e;
            }
            else {
                strEmailTitle = strEmailTitle_e + " / " + strEmailTitle_f;
                strEmailMessage = strEmailMessage_e;
                if (strAddedMessage.Length > 0)
                {
                    strEmailMessage = strEmailMessage + "\n\n" + strAddedMessage;
                }
                strEmailMessage = strEmailMessage + "\n\n" + strEmailSignoff_e + "\n\n---\n\n" + strEmailMessage_f + "\n\n" + strEmailSignoff_f;
            }

            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = strHost;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;

                MailMessage mm = new MailMessage(strMailSender, strEmailDestination);
                mm.Body = strEmailMessage;
                mm.BodyEncoding = System.Text.Encoding.UTF8;
                mm.Subject = strEmailTitle;
                mm.SubjectEncoding = System.Text.Encoding.UTF8;
                if (blnIncludeAttachments == true)
                {
                    if (boolIsPDF_e == true)
                    {
                        AttachFile(strUrlPdf_e, ref mm);
                        //mm.Attachments.Add(new Attachment(strFullFileName));
                    }
                    if ((boolIsPDF_f) == true && (strUrlPdf_f != strUrlPdf_e))
                    {
                        AttachFile(strUrlPdf_f, ref mm);
                    }
                }
                client.Send(mm);

            }
            catch (System.Exception ex)
            {
                result = false;
            }

            return result;
        }

        public bool SendEmail(List<string> lstCodes, string strEmailDestination, string strLang, string strAddedMessage, string strTicket, bool blnIncludeAttachments)
        {
            bool result = true;
            string strCode;
            string strEmailTitle;
            string strEmailTitle_e;
            string strEmailMessage;
            string strEmailMessage_e;
            string strEmailMessage1_e, strEmailMessage1b_e, strEmailMessage2_e, strEmailMessage3_e;
            string strEmailAdditional_e;
            string strLocatedAt_e;
            string strDocumentList_e;

            string strEmailTitle_f;
            string strEmailMessage_f;
            string strEmailMessage1_f, strEmailMessage1b_f, strEmailMessage2_f, strEmailMessage3_f;
            string strEmailAdditional_f;
            string strLocatedAt_f;
            string strDocumentList_f;

            string strEmailSignoff_e;
            string strEmailSignoff_f;
            string strHost;
            string strMailSender;
            string strDirectory;
            string strUrlPdf_e;
            string strUrlPdf_f;
            string strUrl_e;
            string strUrl_f;
            string strDocTitle_e;
            string strDocTitle_f;
            bool boolIsPDF_e, boolIsPDF_f;
            int c;
            string strTemp;
            bool boolAttachmentsExist;

            strDirectory = TemporaryDirectory();
            m_db = new InboundDB();
            LanguageDB ldb = new LanguageDB();

            strLang = strLang.ToLower();

            try
            {
                // Get information about the email server 

                DataSet ds = m_db.GetEmailDetails();
                strHost = ds.Tables[0].Rows[0][0].ToString();
                strMailSender = ds.Tables[0].Rows[0][1].ToString();

                m_client = new SmtpClient();
                m_client.Port = 25;
                m_client.Host = strHost;
                m_client.Timeout = 10000;
                m_client.DeliveryMethod = SmtpDeliveryMethod.Network;
                m_client.UseDefaultCredentials = true;
                m_mm = new MailMessage(strMailSender, strEmailDestination);

                boolAttachmentsExist = DetermineAttachedDocuments(lstCodes, strLang);

                // Get the text for the email message
                strEmailTitle_e = ldb.GetLabel("InboundTracking", "MailSubject", "EN");
                strEmailMessage1_e = ldb.GetLabel("InboundTracking", "MailMessage1", "EN");
                strEmailMessage1b_e = ldb.GetLabel("InboundTracking", "MailMessage1b", "EN");
                strEmailMessage1b_e = strEmailMessage1b_e.Replace("%1", strTicket);
                strEmailMessage2_e = ldb.GetLabel("InboundTracking", "MailMessage2", "EN");
                strEmailMessage3_e = ldb.GetLabel("InboundTracking", "MailMessage3", "EN");
                strLocatedAt_e = ldb.GetLabel("InboundTracking", "MailMessageSiteLocatedAt", "EN");
                strEmailAdditional_e = ldb.GetLabel("InboundTracking", "MailMessageAdditional", "EN");

                strEmailSignoff_e = ldb.GetLabel("InboundTracking", "MailMessageSignoff1", "EN") + "\n" + ldb.GetLabel("InboundTracking", "MailMessageSignoff2", "EN");

                strEmailTitle_f = ldb.GetLabel("InboundTracking", "MailSubject", "FR");
                strEmailMessage1_f = ldb.GetLabel("InboundTracking", "MailMessage1", "FR");
                strEmailMessage1b_f = ldb.GetLabel("InboundTracking", "MailMessage1b", "FR");
                strEmailMessage1b_f = strEmailMessage1b_f.Replace("%1", strTicket);
                strEmailMessage2_f = ldb.GetLabel("InboundTracking", "MailMessage2", "FR");
                strEmailMessage3_f = ldb.GetLabel("InboundTracking", "MailMessage3", "FR");
                strLocatedAt_f = ldb.GetLabel("InboundTracking", "MailMessageSiteLocatedAt", "FR");
                strEmailAdditional_f = ldb.GetLabel("InboundTracking", "MailMessageAdditional", "FR");
                strEmailSignoff_f = ldb.GetLabel("InboundTracking", "MailMessageSignoff1", "FR") + "\n" + ldb.GetLabel("InboundTracking", "MailMessageSignoff2", "FR");
                
                // Get information about the products to be sent

                strDocumentList_e = "";
                strDocumentList_f = "";
                for (c = 0; c < lstCodes.Count; c++)
                {
                    strCode = lstCodes[c].ToString();

                    ds = m_db.GetWebInventoryDetails(strCode);
                    strDocTitle_e = ds.Tables[0].Rows[0]["title"].ToString(); ;
                    strDocTitle_f = ds.Tables[0].Rows[0]["title_alternate"].ToString(); 
                    strUrl_e = ds.Tables[0].Rows[0]["inventory_abstract_eng"].ToString();
                    strUrl_f = ds.Tables[0].Rows[0]["inventory_abstract_french"].ToString();
                    if ((strUrl_e == null) || (strUrl_e.Length < 1))
                    {
                        strUrl_e = ds.Tables[0].Rows[0]["inv_pdf_filename"].ToString();
                    }
                    if ((strUrl_f == null) || (strUrl_f.Length < 1))
                    {
                        strUrl_f = ds.Tables[0].Rows[0]["inv_pdf_filename"].ToString();
                    }

                    strTemp = strLocatedAt_e;
                    strTemp = strTemp.Replace("%1", (c + 1).ToString());
                    strTemp = strTemp.Replace("%2", strDocTitle_e);
                    strTemp = strTemp.Replace("%3", strUrl_e);

                    strDocumentList_e = strDocumentList_e + strTemp + "\n";

                    strTemp = strLocatedAt_f;
                    strTemp = strTemp.Replace("%1", (c + 1).ToString());
                    strTemp = strTemp.Replace("%2", strDocTitle_f);
                    strTemp = strTemp.Replace("%3", strUrl_f);

                    strDocumentList_f = strDocumentList_f + strTemp + "\n";

                }

                // Build the message

                strEmailMessage_e = strEmailMessage1_e + strEmailMessage1b_e;

                strEmailMessage_e = strEmailMessage_e + "\n\n" + strEmailMessage2_e + "\n";
                strEmailMessage_e = strEmailMessage_e + strDocumentList_e;
                if (boolAttachmentsExist == true)
                {
                    strEmailMessage_e = strEmailMessage_e + "\n\n" + strEmailMessage3_e;
                }

                strEmailMessage_f = strEmailMessage1_f + strEmailMessage1b_f;
                strEmailMessage_f = strEmailMessage_f + "\n\n" + strEmailMessage2_f + "\n";
                strEmailMessage_f = strEmailMessage_f + strDocumentList_f;
                if (boolAttachmentsExist == true)
                {
                    strEmailMessage_f = strEmailMessage_f + "\n\n" + strEmailMessage3_f;
                }

                if (strLang == "f")
                {
                    strEmailTitle = strEmailTitle_f + " / " + strEmailTitle_e;
                    strEmailMessage = strEmailMessage_f;
                    if (strAddedMessage.Length > 0)
                    {
                        strEmailMessage = strEmailMessage + "\n\n" +  strEmailAdditional_f +  "\n" + strAddedMessage;
                    }
                    strEmailMessage = strEmailMessage + "\n\n" + strEmailSignoff_f + "\n\n---\n\n" + strEmailMessage_e + "\n\n" + strEmailSignoff_e;
                }
                else
                {
                    strEmailTitle = strEmailTitle_e + " / " + strEmailTitle_f;
                    strEmailMessage = strEmailMessage_e;
                    if (strAddedMessage.Length > 0)
                    {
                        strEmailMessage = strEmailMessage + "\n\n" + strEmailAdditional_e + "\n" + strAddedMessage;
                    }
                    strEmailMessage = strEmailMessage + "\n\n" + strEmailSignoff_e + "\n\n---\n\n" + strEmailMessage_f + "\n\n" + strEmailSignoff_f;
                }


                m_mm = new MailMessage(strMailSender, strEmailDestination);
                m_mm.Body = strEmailMessage;
                m_mm.BodyEncoding = System.Text.Encoding.UTF8;
                m_mm.Subject = strEmailTitle;
                m_mm.SubjectEncoding = System.Text.Encoding.UTF8;
                if (boolAttachmentsExist == true)
                {
                    if (blnIncludeAttachments == true)
                    {
                        for (c = 0; c < m_lstAttachments.Count; c++)
                        {
                            AttachFile(m_lstAttachments[c].ToString(), ref m_mm);
                        }
                    }
                }
                /*if (boolIsPDF_e == true)
                {
                    AttachFile(strUrlPdf_e, ref m_mm);
                    //mm.Attachments.Add(new Attachment(strFullFileName));
                }*/

                m_client.Send(m_mm);

            }
            catch (System.Exception ex)
            {
                result = false;
            }

            return result;
        }

        public bool DetermineAttachedDocuments(List<string> lstCodes, string strLanguage)
        {
            bool bDocsAttached;
            DataSet ds;
            string strCode;
            string strUrl_e;
            string strUrl_f;
            int c;
            string strUrl;
            string strBroken;

            bDocsAttached = false;
            m_lstAttachments = new List<string>();

            for (c = 0; c < lstCodes.Count; c++)
            {
                strCode = lstCodes[c].ToString();
                ds = m_db.GetWebInventoryDetails(strCode);

                strUrl_e = "";
                strUrl_f = "";
                strBroken = ds.Tables[0].Rows[0]["inv_pdf_brk"].ToString().ToUpper();
                if (ds.Tables[0].Rows[0]["inv_pdf_filename"] != null || ds.Tables[0].Rows[0]["inv_pdf_filename"].ToString().Length == 0)
                {
                    strUrl_e = ds.Tables[0].Rows[0]["inv_pdf_filename"].ToString();
                }
                if (ds.Tables[0].Rows[0]["inv_pdf_filename_fr"] != null)
                {
                    strUrl_f = ds.Tables[0].Rows[0]["inv_pdf_filename_fr"].ToString();
                }
                if (((strUrl_f.Length > 0) && (strLanguage.ToUpper() == "F")) || strUrl_e.Length == 0)
                {
                    strUrl = strUrl_f;
                }
                else
                {
                    strUrl = strUrl_e;
                }

                if ((strUrl.Length > 0) && (strBroken != "YES"))
                {
                    m_lstAttachments.Add(strUrl);
                    //AttachFile(strUrl, ref m_mm);
                    bDocsAttached = true;
                }


            }

            return bDocsAttached;

        }

        public bool AttachFile(string strUrl, ref MailMessage mm)
        {
            bool result = true;
            string strCompleteFileName;

            string strDirectory;

            try
            {

                strDirectory = TemporaryDirectory();
                strCompleteFileName = CompleteName(strUrl, strDirectory);

                System.IO.File.Delete(strCompleteFileName);

                using (WebClient myWebClient = new WebClient())
                {
                    myWebClient.DownloadFile(strUrl, strCompleteFileName);
                }
                mm.Attachments.Add(new Attachment(strCompleteFileName));
            }
            catch (System.Exception ex)
            {
                result = false;
            }

            return result;
        }
        public string CompleteName(string strURL, string strDirectory)
        {
            string strFileName;
            string strResult;

            strResult = "";

            strResult = strDirectory;
            strFileName = GetFileNameFromURL(strURL);
            if (strResult.Substring(strResult.Length - 1, 1) != "\\")
            {
                strResult = strResult + "\\";
            }
            strResult = strResult + strFileName;


            return strResult;
        }

        public string TemporaryDirectory()
        {
            string strReturnValue;

            strReturnValue = ConfigurationManager.AppSettings.Get("tempdir");

            return strReturnValue;
        }
    }
}