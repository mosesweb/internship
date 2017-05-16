using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;

namespace MosesPraktik
{
    class ErrandJobDefinition : SPJobDefinition
    {
        public const string JobName = "TimerJob";

        public ErrandJobDefinition() : base() { }
        
        public ErrandJobDefinition(SPWebApplication webApp) : base (JobName, webApp, null, SPJobLockType.Job) 
        {
            Title = "Timer Job Definition";
        }

        public override void Execute(Guid targetInstanceId)
        {
            SPWebApplication webApp = this.Parent as SPWebApplication;
     
            sendEmail(webApp);
        }
        public void sendEmail(SPWebApplication webApp)
        {
            SPWeb spweb = webApp.Sites[0].RootWeb;

            SPList errand_list = webApp.Sites[0].RootWeb.Lists[ErrandDefinitions.ListName];
            SPListItem item;
            SPListItemCollection listitemcollection;

            // without <Query></Query> and it works!
            SPQuery spquery = new SPQuery();

            // måste kolla om slutdatumvärdet har mindre än 1 vecka kvar. jämför mot NOW
            // enddate >= Created Date 
            // måste bara skicka en gång.
            
            string strquery = 
            "   <Where>" +
            "      <Or>" +
            "         <Eq>" +
            "            <FieldRef Name='Status' />" +
            "            <Value Type='Choice'>Aktiv</Value>" +
            "         </Eq>" +
            "         <Eq>" +
            "            <FieldRef Name='Status' />" +
            "            <Value Type='Choice'>Ny</Value>" +
            "         </Eq>" +
            "      </Or>" +
            "   </Where>";

            // Also where SendNotification column is Eq to 0?
            // Need to add a "hidden" column perhaps?
            string strqueryFilterDate = 
            "   <Where>" +
            "      <And>" +
            "      <And>" +
            "         <Leq>" +
            "            <FieldRef Name='Slutdatum' />" +
            "            <Value IncludeTimeValue='False' Type='DateTime'><Today OffsetDays='7'/></Value>" +
            "         </Leq>" +
            "         <Neq>" +
            "            <FieldRef Name='NotificationSent' />" +
            "            <Value Type='Boolean'>1</Value>" +
            "         </Neq>" +
            "      </And>" +
            "         <Or>" +
            "            <Eq>" +
            "               <FieldRef Name='Status' />" +
            "               <Value Type='Choice'>Ny</Value>" +
            "            </Eq>" +
            "            <Eq>" +
            "               <FieldRef Name='Status' />" +
            "               <Value Type='Choice'>Aktiv</Value>" +
            "            </Eq>" +
            "         </Or>"+
            "      </And>" +
            "   </Where>";

            //<Value Type='DateTime'>[Now+7Day(s)]</Value>
           // "<Where> <Leq> <FieldRef Name='Slutdatum' /> <Value Type='DateTime'>[Now+7Day(s)]</Value> </Leq> </Where>";

            // select all active and new errands
            spquery.Query = strqueryFilterDate;

            listitemcollection = errand_list.GetItems(spquery);

            foreach (SPListItem anitem in listitemcollection)
            {
                // Fields
                string itemTitle = anitem.Title.ToString();
                string itemDescription = anitem[ErrandDefinitions.DescriptionFieldName].ToString();
                string priofield = anitem[ErrandDefinitions.PrioFieldName].ToString();
                string statusfield = anitem[ErrandDefinitions.StatusFieldName].ToString();
                string categoryfield = anitem[ErrandDefinitions.CategoryFieldName].ToString();
                string customerlogin = anitem[ErrandDefinitions.CustomerFieldName].ToString();
                string userfielduservaluestring = anitem[ErrandDefinitions.ResponsibleFieldName].ToString();
                string descriptionfield = anitem[ErrandDefinitions.DescriptionFieldName].ToString();
                string enddate;

                SPFieldLookupValue customerspfieldlookup = new SPFieldLookupValue(anitem[ErrandDefinitions.CustomerNameLookUpFieldName].ToString());
                string realcustomername = customerspfieldlookup.LookupValue;

                
                if (anitem[ErrandDefinitions.EndDateFieldName] != null)
                {
                    enddate = anitem[ErrandDefinitions.EndDateFieldName].ToString();
                }
                else
                {
                    enddate = "Inget slutdatum";
                }

                SPFieldUserValue spfielduservalue = new SPFieldUserValue(spweb, userfielduservaluestring);
                SPUser spuser = spfielduservalue.User;

                string responsiblename = spuser.LoginName.ToString();
                string[] parts = responsiblename.Split('|');
                string responsibleloginname = parts[1];

                // email body
                string body = "<strong>" + itemTitle + "</strong> Ärendet har endast en vecka kvar tills det ska vara klart.\n\n" +
                    "Prioritet: " +
                    priofield +
                    "\nStatus: " +
                    statusfield +
                    "\nKategori: " +
                    categoryfield +
                    "\nKund: " +
                    realcustomername +
                    "\nAnsvarig: " +
                    responsibleloginname +
                    "\nSluttid: " +
                    enddate +
                    "\n\n" +
                    descriptionfield;

                // if user has no email
                string useremail = "";
                if (string.IsNullOrEmpty(spuser.Email))
                {
                    useremail = "noemail@dev.local";
                }
                else
                {
                    useremail = spuser.Email;
                }
                StringDictionary headers = new StringDictionary();
                headers.Add("to", useremail);
                headers.Add("from", "noreply@dev.local");
                // \"{0}\" not looking nice for some reason..
                headers.Add("subject", String.Format("Endast en vecka kvar på {0} ärendet", itemTitle));
                SPUtility.SendEmail(webApp.Sites[0].RootWeb, headers, body);
                anitem[ErrandDefinitions.NotificationSentFieldName] = "1";
                anitem.Update();
            }
        }
    }
}