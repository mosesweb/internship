using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MosesPraktik.Layouts.MosesPraktik.Pages
{
    public partial class customerForm : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SPWeb web = SPContext.Current.Web;

            if (IsPostBack)
            {
                if (Request.Form["customerName"] != null)
                {
                    string customerName = Request.Form["customerName"].ToString();

                    SPListItemCollection listItems = web.Lists[ErrandDefinitions.CustomerListName].Items;
                    SPListItem item = listItems.Add();
                    item["Title"] = customerName;
                    item.Update();

                    // Update page
                    Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
}
