using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.WebPartPages;

namespace MosesPraktik.Features.Pages
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("0eb5f54b-ee96-471a-bfa7-89e9ad3bcf5a")]
    public class PagesEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var web = properties.Feature.Parent as SPWeb;
            PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(web);

            string pageName = "ClosedErrands.aspx";
            PageLayout[] pageLayouts = publishingWeb.GetAvailablePageLayouts();
            PageLayout PagelayoutToUse = pageLayouts[11]; //welcome splash
            PublishingPageCollection pages = publishingWeb.GetPublishingPages();
            bool page_exists = false;
            foreach (PublishingPage page in pages)
            {
                if (page.Name.Equals(pageName))
                {
                    page_exists = true;
                }
            }
            if (!page_exists)
            {
                PublishingPage newPage = pages.Add(pageName, PagelayoutToUse);
                newPage.ListItem[FieldId.PublishingPageContent] = "This is my content";

                newPage.ListItem.Update();
                newPage.Update();
                newPage.ListItem.File.CheckIn("checkin");
                newPage.ListItem.File.Publish("publish page");

                // Now add web part
                string url = String.Format(web.Url + "/Pages/" + pageName);
                SPFile pageFile = web.GetFile(url);
                pageFile.CheckOut();

                SPLimitedWebPartManager wpManager = pageFile.GetLimitedWebPartManager(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared);
                SPLimitedWebPartCollection webpartCollection = wpManager.WebParts;
                VisualWebPart.VisualWebPart wp = new VisualWebPart.VisualWebPart();
                wp.Title = ErrandDefinitions.ListName;
                web.AllowUnsafeUpdates = true;
                wp._ErrandPresentationDropDown = VisualWebPart.VisualWebPart.ErrandPresentationEnum.Closed;
       
                wpManager.AddWebPart(wp, "Header", 0);
                web.Update();
               
                pageFile.CheckIn("web part add");
                pageFile.Publish("web part add");
            }
        }
    }
}

