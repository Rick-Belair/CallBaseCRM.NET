using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CallBaseMock
{
    public partial class LegacyIE : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // System.Web.HttpBrowserCapabilities browser = Request.Browser;
            int browserVersion = Request.Browser.MajorVersion;
            string lang = "EN";
            if (Session["PageLanguage"] != null)
                lang = Session["PageLanguage"].ToString();

            if (browserVersion < 9)
            {
                if (lang.Equals("EN"))
                    message.InnerHtml = "WARNING: You are using an older version of the Internet Explorer browser which this application was not designed for. " +
                        "The application will not run properly. <br/><br/>" +
                        "You can either update your browser or use the older version of CallBase, contact the system Administrator for assistance. You could also use Chrome or FireFox.";
                else
                {
                    message.InnerHtml = "ATTENTION: Vous utilisez une ancienne version du navigateur Internet Explorer lequel ne fonctionne pas correctement avec cette application." +
                    "<br/><br/>" +
                    "Vous pouvez soit mettre à jour votre navigateur ou utiliser l'ancienne version de CallBase, contactez l'administrateur du système pour assistance. "
                    + "Vous pouvez également utiliser Chrome ou Firefox.";
                    message.Attributes.Add("class", "smallerText");
                }

            }// IE < 9
            else
            {
                if (lang.Equals("EN"))
                    message.InnerHtml = "WARNING: Your Internet Explorer browser is set to Compatibility mode for IE 7 or 8. " +
                        "This will cause problems with the new version of the CallBase application you are trying to run.  You should remove the Compatibility mode " +
                        "and restart the application, or contact your IT department to change this setting so you can properly use the application. <br/><br/>" +
                        "An older version of CallBase is available, contact the system Administrator for assistance. You could also use Chrome or FireFox.";

                else
                {
                    message.InnerHtml = "ATTENTION: Votre navigateur Internet Explorer est configuré en mode de compatibilité pour IE 7 ou 8. " +
                    "Cela ne fonctionnera pas correctement avec la nouvelle version de l'application CallBase que vous essayez d'exécuter. Vous devriez enlever le mode de compatibilité" +
                    " et redémarrer l'application, ou contactez votre service informatique pour modifier ce paramètre pour que vous puissiez utiliser correctement l'application." +
                    "<br/><br/>" +
                    "Une ancienne version de CallBase est disponible, contactez l'administrateur du système pour assistance. Vous pouvez également utiliser Chrome ou Firefox.";
                    message.Attributes.Add("class", "smallerText");
                }

            }// IE >= 9 but in compat

        }//Page_Load

    }//class

}//namespace