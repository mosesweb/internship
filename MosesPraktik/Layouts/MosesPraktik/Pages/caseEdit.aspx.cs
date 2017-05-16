using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MosesPraktik.Layouts.MosesPraktik.Pages
{
    public partial class caseEdit : LayoutsPageBase
    {
        SPWeb web = SPContext.Current.Web;

        protected void Page_Load(object sender, EventArgs e)
        {
            string ListId = Request.QueryString["ListId"];
            string ItemId = Request.QueryString["ItemId"];

            //SPList list = web.Lists[new Guid(ListId)];
           // SPListItem item = list.Items.GetItemById(Convert.ToInt32(ItemId));

            // query for information about list and list item
          //  string ListTitle = list.Title;
           // string ItemTitle = item.Title;

            // define fields...

        }
    }
}
