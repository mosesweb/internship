using System;
using System.Linq;
using System.Security.Permissions;
using System.Collections.Specialized;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;

namespace MosesPraktik.ErrandListEventReceiver
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class ErrandListEventReceiver : SPItemEventReceiver
    {
        StringDictionary headers = new StringDictionary();

        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);

            SPWeb web = properties.Web as SPWeb;
            if (web == null) return;

            bool blnlsEmailServerSet = SPUtility.IsEmailServerSet(web);
            if (blnlsEmailServerSet)
            {
                // old values
                string oldtitle = properties.ListItem["Title"].ToString();
                string olddescription = properties.ListItem[ErrandDefinitions.DescriptionFieldName].ToString();
                string oldcategory = properties.ListItem[ErrandDefinitions.CategoryFieldName].ToString();
                
                string oldcustomer = properties.ListItem[ErrandDefinitions.CustomerFieldName].ToString();
                SPFieldLookupValue oldCustomerValue = new SPFieldLookupValue(oldcustomer);
                string oldCustomerLookupID = oldCustomerValue.LookupId.ToString();

                string oldstatus = properties.ListItem[ErrandDefinitions.StatusFieldName].ToString();
                string oldprio = properties.ListItem[ErrandDefinitions.PrioFieldName].ToString();

                // old responsible field
                string oldresponsible = properties.ListItem[ErrandDefinitions.ResponsibleFieldName].ToString();
                SPFieldUserValue oldresponsibleLookupValue = new SPFieldUserValue(web, oldresponsible);
                string oldResponsibleLookupValueString = oldresponsibleLookupValue.ToString();

                // new values
                string newtitle = properties.AfterProperties["Title"].ToString();
                string newdescription = properties.AfterProperties[ErrandDefinitions.DescriptionFieldName].ToString();
                string newcategory = properties.AfterProperties[ErrandDefinitions.CategoryFieldName].ToString();
                string newstatus = properties.AfterProperties[ErrandDefinitions.StatusFieldName].ToString();
                string newprio = properties.AfterProperties[ErrandDefinitions.PrioFieldName].ToString();
                string newresponsible = properties.AfterProperties[ErrandDefinitions.ResponsibleFieldName].ToString();
                string newcustomer = properties.AfterProperties[ErrandDefinitions.CustomerFieldName].ToString();
                string newenddate = properties.AfterProperties[ErrandDefinitions.EndDateFieldName].ToString();

                // get correct web time zone
                SPTimeZone timeZone = web.RegionalSettings.TimeZone;

                string oldenddate = properties.ListItem[ErrandDefinitions.EndDateFieldName].ToString();

                DateTime dtBefore = DateTime.Parse(oldenddate).ToUniversalTime();
                string thenewenddate = properties.AfterProperties[ErrandDefinitions.EndDateFieldName].ToString();

                DateTime dtNew = DateTime.Parse(thenewenddate).ToUniversalTime();

                dtNew = timeZone.LocalTimeToUTC(dtNew);

                bool changes = false;
                int n;

                if (oldtitle != newtitle)
                    changes = true;
                if (olddescription != newdescription)
                    changes = true;
                if (oldcategory != newcategory)
                     changes = true;
                if (oldCustomerLookupID != newcustomer)
                     changes = true;
                if (oldstatus != newstatus)
                    changes = true;
                if (oldprio != newprio)
                    changes = true;
                if (dtBefore != dtNew)
                    changes = true;

                // email title
                string thetitle = properties.AfterProperties["Title"].ToString();

                // email adress
                string uservaluestring = properties.AfterProperties[ErrandDefinitions.ResponsibleFieldName].ToString();

                // responsible user
                string theNewresponsibleFieldValue = properties.AfterProperties[ErrandDefinitions.ResponsibleFieldName].ToString();
                string responsibleFieldValue = properties.ListItem[ErrandDefinitions.ResponsibleFieldName].ToString();

                bool isNumeric = int.TryParse(theNewresponsibleFieldValue, out n);
                string responsibleemail = "nouser@dev.local";
                string responsiblename = "";
                //SPItem spitemcustomername = properties.List.Fields.GetField(ErrandDefinitions.CustomerFieldName).value
                SPList customerlist = web.Lists[ErrandDefinitions.CustomerListName];
                SPListItem splistitem = customerlist.GetItemById(Convert.ToInt32(newcustomer));
                string valuefromlisttitle = splistitem["Title"].ToString();
                string customerloginname = "";

                if (isNumeric)
                {
                    SPUser spuser = web.SiteUsers.GetByID(Convert.ToInt32(theNewresponsibleFieldValue));
                    responsibleemail = spuser.Email;
                    responsiblename = spuser.LoginName.ToString();
                }
                else 
                {
                    changes = true;
                    SPUser spuser = GetUserObject(theNewresponsibleFieldValue, web);

                     responsibleemail = spuser.Email;
                     responsiblename = spuser.LoginName.ToString();
                }
                string[] parts = responsiblename.Split('|');
                customerloginname = parts[1];

                // update the SPItems if any differences are found
                if (changes)
                {
                    if (string.IsNullOrEmpty(responsibleemail.ToString()))
                    {
                        responsibleemail = "nouser@dev.local";
                    }

                    // email body
                    string body = "<strong>" + thetitle + "</strong> Ärendet har blivit uppdaterat.\n\n" + 
                        "Prioritet: " + 
                        newprio + 
                        "\nStatus: " + 
                        newstatus + 
                        "\nKategori: " + 
                        newcategory + 
                        "\nKund: " +
                        valuefromlisttitle + 
                        "\nAnsvarig: " +
                        customerloginname + 
                        "\n\n" + 
                        properties.AfterProperties[ErrandDefinitions.DescriptionFieldName].ToString();
                
                    headers.Add("to", responsibleemail);
                    headers.Add("from", "noreply@dev.local");
                    headers.Add("subject", thetitle + " ärendet har blivit uppdaterad");

                    SPUtility.SendEmail(web, headers, body);
                }
            }
        }
        public static SPUser GetUserObject(string peoplePickerValue, SPWeb web)
        {
            string strUser = string.Empty;
            SPFieldUser flduser = (SPFieldUser)web.Lists[ErrandDefinitions.ListName].Fields.GetField(ErrandDefinitions.ResponsibleFieldName);
            SPFieldUserValue valResponsible = (SPFieldUserValue)flduser.GetFieldValue(peoplePickerValue);
            SPUser user = web.EnsureUser(valResponsible.LookupValue);
            
            // SPUser usertemp = valResponsible.User; //null
            return user;
        }
    }
}