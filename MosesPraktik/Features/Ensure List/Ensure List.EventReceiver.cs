using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Collections.Generic;
using Microsoft.SharePoint.Utilities;


namespace MosesPraktik.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("fa58ba7a-796b-465f-8beb-531714ed8390")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {

            var web = properties.Feature.Parent as SPWeb;
            if (web == null) return;

            string listName = ErrandDefinitions.ListName;
            string customerlistName = ErrandDefinitions.CustomerListName;
            var Mainlist = web.Lists.TryGetList(listName);

            var Customerlist = web.Lists.TryGetList(ErrandDefinitions.CustomerListName);
            
            // create lists and site columns
            if (Customerlist == null)
            {
                var listId = web.Lists.Add(customerlistName, "Customer list", SPListTemplateType.GenericList);
                Customerlist = web.Lists[listId];
                Customerlist.OnQuickLaunch = true;
                Customerlist.ContentTypesEnabled = true;
                Customerlist.Update();

                // Title display name
                SPField defaultTitleField = Customerlist.Fields.GetFieldByInternalName("Title");
                defaultTitleField.Title = ErrandDefinitions.TitleDisplayTitle;
                defaultTitleField.Update();
                Customerlist.Update();
            }
            if (Mainlist == null)
            {
                // Create List
                var listId = web.Lists.Add(listName, ErrandDefinitions.ListDescription, SPListTemplateType.GenericList);
                Mainlist = web.Lists[listId];
                Mainlist.OnQuickLaunch = true;
                Mainlist.ContentTypesEnabled = true;
                Mainlist.Update();

                // Title display name
                SPField defaultTitleField = web.Fields.GetFieldByInternalName("Title");
                defaultTitleField.Title = ErrandDefinitions.TitleDisplayTitle;

                // Description Field
                SPFieldMultiLineText DescText = (SPFieldMultiLineText)web.Fields.CreateNewField(
                SPFieldType.Note.ToString(), ErrandDefinitions.DescriptionFieldName);
                DescText.Required = true;
                DescText.RichText = true;
                DescText.AllowHyperlink = true;
                DescText.NumberOfLines = 5;

                web.Fields.Add(DescText);

                // Responsible Field
                // people picker
                SPFieldUser Responsible = (SPFieldUser)web.Fields.CreateNewField(
                SPFieldType.User.ToString(), ErrandDefinitions.ResponsibleFieldName);
                Responsible.Required = true;
                web.Fields.Add(Responsible);

                // Customer Field
                var CustomerLookup = web.Fields.AddLookup(ErrandDefinitions.CustomerNameLookUpFieldName, Customerlist.ID, true);

                // Status Field
                string fieldStatusName = web.Fields.Add(ErrandDefinitions.StatusFieldName, SPFieldType.Choice, true);

                SPFieldChoice status_choice_col = (SPFieldChoice)web.Fields.GetFieldByInternalName(fieldStatusName);

                for (int x = 0; x < ErrandDefinitions.StatusChoices.Length; x++)
                {
                    status_choice_col.AddChoice(ErrandDefinitions.StatusChoices[x]);
                }
                status_choice_col.DefaultValue = ErrandDefinitions.StatusChoicesDefault;
                status_choice_col.Update();

                // Category Field
                web.Fields.Add(ErrandDefinitions.CategoryFieldName, SPFieldType.Choice, true);

                SPFieldChoice category_choice_col = (SPFieldChoice)web.Fields[ErrandDefinitions.CategoryFieldName];
                for (int x = 0; x < ErrandDefinitions.CategoryChoices.Length; x++)
                {
                    category_choice_col.AddChoice(ErrandDefinitions.CategoryChoices[x]);
                }
                category_choice_col.DefaultValue = ErrandDefinitions.CategoryChoicesDefault;
                category_choice_col.Update();

                // Priority Field
                web.Fields.Add(ErrandDefinitions.PrioFieldName, SPFieldType.Choice, true);

                SPFieldChoice prio_choice_col = (SPFieldChoice)web.Fields[ErrandDefinitions.PrioFieldName];
                for (int x = 0; x < ErrandDefinitions.PrioChoices.Length; x++)
                {
                    prio_choice_col.AddChoice(ErrandDefinitions.PrioChoices[x]);
                }

                prio_choice_col.DefaultValue = ErrandDefinitions.PrioChoicesDefault;
                prio_choice_col.Update();

                // Date field
                web.Fields.Add(ErrandDefinitions.EndDateFieldName, SPFieldType.DateTime, false);
                SPFieldDateTime end_date_field = (SPFieldDateTime)web.Fields[ErrandDefinitions.EndDateFieldName];
                end_date_field.DefaultValue = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.AddDays(14)); // now plus 14 days
                end_date_field.Update();

                // Hidden Boolean Field for checking if notification mail has been sent.
                web.Fields.Add(ErrandDefinitions.NotificationSentFieldName, SPFieldType.Boolean, false);
                SPFieldBoolean notificationsent_field = (SPFieldBoolean)web.Fields[ErrandDefinitions.NotificationSentFieldName];
                notificationsent_field.DefaultValue = "0";
                notificationsent_field.ShowInNewForm = false;
                notificationsent_field.ShowInEditForm = false;
                notificationsent_field.ShowInDisplayForm = false;
                notificationsent_field.Update();
            }

            // content type
            SPContentType TheErrandContentType = web.ContentTypes[ErrandDefinitions.ContentTypeName];
            if (TheErrandContentType == null)
            {
                // Parent type item
                SPContentType itemParent = web.AvailableContentTypes[SPBuiltInContentTypeId.Item];

                // Create the content type
                SPContentType errandType = new SPContentType(itemParent, web.ContentTypes, ErrandDefinitions.ContentTypeName);
                web.ContentTypes.Add(errandType);

                // if there is a content type and list, add the content type to the list.
                /* if (TheErrandContentType != null && list != null) */

                // add refs
                SPContentType GetTheErrandContentType = web.ContentTypes[ErrandDefinitions.ContentTypeName];

                SPField beskrivningfield = web.Fields.GetField(ErrandDefinitions.DescriptionFieldName);
                SPFieldLink beskrivningfieldRef = new SPFieldLink(beskrivningfield);

                GetTheErrandContentType.FieldLinks.Add(beskrivningfieldRef);

                SPField prioField = web.Fields.GetField(ErrandDefinitions.PrioFieldName);
                SPFieldLink prioFieldRef = new SPFieldLink(prioField);

                GetTheErrandContentType.FieldLinks.Add(prioFieldRef);

                SPFieldChoice statusField = (SPFieldChoice)web.Fields.GetField(ErrandDefinitions.StatusFieldName);
                SPFieldLink statusFieldRef = new SPFieldLink(statusField);

                GetTheErrandContentType.FieldLinks.Add(statusFieldRef);

                SPField responsibleField = web.Fields.GetField(ErrandDefinitions.ResponsibleFieldName);
                SPFieldLink responsibleFieldRef = new SPFieldLink(responsibleField);

                GetTheErrandContentType.FieldLinks.Add(responsibleFieldRef);

                SPField categoryField = web.Fields.GetField(ErrandDefinitions.CategoryFieldName);
                SPFieldLink categoryFieldRef = new SPFieldLink(categoryField);

                GetTheErrandContentType.FieldLinks.Add(categoryFieldRef);

                // add the lookup field
                SPFieldLookup customernameField = (SPFieldLookup)web.Fields[ErrandDefinitions.CustomerNameLookUpFieldName];
                customernameField.LookupField = Customerlist.Fields["Titel"].InternalName;
                customernameField.Title = ErrandDefinitions.CustomerNameLookUpFieldName;
                customernameField.Update();

                SPFieldLink customerFieldRef = new SPFieldLink(customernameField);
                GetTheErrandContentType.FieldLinks.Add(customerFieldRef);

                // add datetime
                SPFieldDateTime end_datetime = (SPFieldDateTime)web.Fields[ErrandDefinitions.EndDateFieldName];

                SPFieldLink end_datetimeFieldRef = new SPFieldLink(end_datetime);
                GetTheErrandContentType.FieldLinks.Add(end_datetimeFieldRef);

                // add NotificationSent
                SPFieldBoolean notificationsentField = (SPFieldBoolean)web.Fields[ErrandDefinitions.NotificationSentFieldName];

                SPFieldLink notificationsentFieldRef = new SPFieldLink(notificationsentField);
                GetTheErrandContentType.FieldLinks.Add(notificationsentFieldRef);

                GetTheErrandContentType.NewFormUrl = "_layouts/15/MosesPraktik/Pages/caseForm.aspx";

                GetTheErrandContentType.Update();

                // add content type
                Mainlist.ContentTypes.Add(GetTheErrandContentType);

                // List title display name
                SPField ErrandlistTitle = Mainlist.Fields.GetFieldByInternalName("Title");
                ErrandlistTitle.Title = ErrandDefinitions.TitleDisplayTitle;


                // set default list content type


                ErrandlistTitle.Update();
                ReorderContentTypes(web, ErrandDefinitions.ListName, ErrandDefinitions.ContentTypeName);

                Mainlist.Update();
            }

        }
        private void ReorderContentTypes(SPWeb web, string listName, string firstContentTypeName)
        {
            SPList list = web.Lists[listName];

            SPContentType cType = web.AvailableContentTypes[firstContentTypeName];

            List<SPContentType> oldCTypes = new List<SPContentType>();

            for (int i = list.ContentTypes.Count - 1; i >= 0; i--)
            {
                if (!list.ContentTypes[i].Id.IsChildOf(cType.Id))
                {
                    oldCTypes.Add(list.ContentTypes[i]);

                    list.ContentTypes[i].Delete();
                }
            }

            foreach (SPContentType c in oldCTypes)
            {
                list.ContentTypes.Add(c);
            }

            list.Update();
        }
        
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        { 
            
             //base.FeatureDeactivating(properties);
             //var web = properties.Feature.Parent as SPWeb;

             //if (web == null) return;

             //string listName = ErrandDefinitions.ListName;
             //var list = web.Lists.TryGetList(listName);

             //string listcustomerName = ErrandDefinitions.CustomerListName;
             //var listcustomer = web.Lists.TryGetList(listcustomerName);

             //list.Delete();
             //listcustomer.Delete();

             //SPField resField = web.Fields.GetField(ErrandDefinitions.ResponsibleFieldName);
             //SPField beskField = web.Fields.GetField(ErrandDefinitions.DescriptionFieldName);
             //SPField catField = web.Fields.GetField(ErrandDefinitions.CategoryFieldName);
             //SPField statField = web.Fields.GetField(ErrandDefinitions.StatusFieldName);
             //SPField cusField = web.Fields.GetField(ErrandDefinitions.CustomerNameLookUpFieldName);
             //SPField prioField = web.Fields.GetField(ErrandDefinitions.PrioFieldName);
             //SPField enddateField = web.Fields.GetField(ErrandDefinitions.EndDateFieldName);
             //SPField notificationsendField = web.Fields.GetField(ErrandDefinitions.NotificationSentFieldName);


             //// Our content type
             //SPContentType spContentType = web.ContentTypes[ErrandDefinitions.ContentTypeName];

             //// Delete field links for the content type
             //spContentType.FieldLinks.Delete(resField.Id);
             //spContentType.FieldLinks.Delete(catField.Id);
             //spContentType.FieldLinks.Delete(statField.Id);
             //spContentType.FieldLinks.Delete(cusField.Id);
             //spContentType.FieldLinks.Delete(prioField.Id);
             //spContentType.FieldLinks.Delete(beskField.Id);
             //spContentType.FieldLinks.Delete(enddateField.Id);
             //spContentType.FieldLinks.Delete(notificationsendField.Id);

             //spContentType.Update();

             //// delete col
             //if (resField != null)
             //{
             //    resField.Delete();
             //}
             //// delete col
             //if (catField != null)
             //{
             //    catField.Delete();
             //}
             //// delete col
             //if (cusField != null)
             //{
             //    cusField.Delete();
             //}
             //// delete col
             //if (statField != null)
             //{
             //    statField.Delete();
             //}
             //// delete col
             //if (prioField != null)
             //{
             //    prioField.Delete();
             //}
             //// delete col
             //if (beskField != null)
             //{
             //    beskField.Delete();
             //}
             //// delete col
             //if (enddateField != null)
             //{
             //    enddateField.Delete();
             //}
             //// delete col
             //if (notificationsendField != null)
             //{
             //    notificationsendField.Delete();
             //}

             //IList<SPContentTypeUsage> usages = SPContentTypeUsage.GetUsages(spContentType);

             //if (usages.Count > 0)
             //{

             //}
             //else
             //{
             //    web.ContentTypes.Delete(spContentType.Id);
             //}
         }
    }     
}  