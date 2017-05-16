using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

namespace MosesPraktik.Features.Clear
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("cbcafd20-49ed-4d84-bbf2-d19ff10c30fc")]
    public class ClearEventReceiver : SPFeatureReceiver
    {
        private bool customVisualWebPartExists = false;

        // Method for removing all web parts except our custom VisualWebPart
        public void RemoveAllWebParts(SPFeatureReceiverProperties properties)
        {
            var web = properties.Feature.Parent as SPWeb;
            
            web.AllowUnsafeUpdates = true;
            string url = String.Format(web.Url + "/Pages/default.aspx");
            SPFile pageFile = web.GetFile(url);
            SPFileLevel fileLevel = pageFile.Level;
            
            using (SPLimitedWebPartManager wpManager = pageFile.GetLimitedWebPartManager(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
            {
                List<WebPart> webParts = new List<WebPart>();
                SPLimitedWebPartCollection webpartCollection = wpManager.WebParts;
                int amountofWebParts = webpartCollection.Count;

                for (int x = 0; x < amountofWebParts; x++)
                {
                    if (webpartCollection[x] != null)
                    {
                        // Delete all web parts except Visual Web Part if found
                        Object WebPartType = webpartCollection[x].GetType();
                        if (WebPartType != typeof(VisualWebPart.VisualWebPart))
                        {
                            wpManager.DeleteWebPart(webpartCollection[x]);
                            x--;
                            amountofWebParts--;
                        }
                        else 
                        {
                            customVisualWebPartExists = true;
                        }
                    }
                }
            }
            web.Update();
        }
        public void addOurVisualWebPart()
        {
 
        }

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
           
            var web = properties.Feature.Parent as SPWeb;
            string url = String.Format(web.Url + "/Pages/default.aspx");
            SPFile webPartPage = web.GetFile(url);
            SPLimitedWebPartManager spLimitedWPM = webPartPage.GetLimitedWebPartManager(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared);
            
            webPartPage.CheckOut();
            RemoveAllWebParts(properties);

            // Add web part if it doesnt exist
            if (customVisualWebPartExists == false)
            {
                VisualWebPart.VisualWebPart webpart = new VisualWebPart.VisualWebPart();
                SPFile file = web.GetFile("http://sp/Pages/default.aspx");

                SPLimitedWebPartManager wpm = file.GetLimitedWebPartManager(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared);
                VisualWebPart.VisualWebPart wp = new VisualWebPart.VisualWebPart();
                wp.Title = ErrandDefinitions.ListName;
                wp._ErrandPresentationDropDown = VisualWebPart.VisualWebPart.ErrandPresentationEnum.Open;
                wp.ChromeType = System.Web.UI.WebControls.WebParts.PartChromeType.None;
                wpm.AddWebPart(wp, "Header", 0);
            }
            // test action
            webPartPage.CheckIn("adding visual web part and removes other web parts");
            webPartPage.Publish("");
        }
    }
}

