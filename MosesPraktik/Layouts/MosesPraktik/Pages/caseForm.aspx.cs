using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MosesPraktik.Layouts.MosesPraktik.Pages
{
    
    public class edititemObject
    {

    }
    public class radioItemEdit
    {
        private string radioName = "";
        private string radioValue = "";
        private bool ischecked = false;

        public radioItemEdit(string radioname, string radioval, bool theischecked)
        {
            this.radioName = radioname;
            this.radioValue = radioval;
            this.ischecked = theischecked;
        }
        
        public string printRadio()
        {
            return string.Concat("",
            "<div class='radio'>",
            "<label>",
                "<input type='radio' name='" + radioName + "' id='" + radioName + "' value='" + radioValue + "' />",
            "</label>",
            "</div>");
        }

        public string RadioName { get { return radioName; } set { radioName = value;} }
        public string RadioValue { get { return radioValue; } set { radioValue = value; } }
    }
    public class Case
    {
        private string casename = "";
        private string casedescription = "";
        private string casepriority = "";
        private string casestatus = "";
        private string casecategory = "";
        private string casecustomer = "";
        private string caseresponsible = "";
        private DateTime caseenddate = DateTime.Now;

        public string caseName { get { return this.casename; } set { this.casename = value; } }
        public string caseDescription { get { return this.casedescription; } set { this.casedescription = value; } }
        public string casePriority { get { return this.casepriority; } set { this.casepriority = value; } }
        public string caseStatus { get { return this.casestatus; } set { this.casestatus = value; } }
        public string caseCategory { get { return this.casecategory; } set { this.casecategory = value; } }
        public string caseCustomer { get { return this.casecustomer; } set { this.casecustomer = value; } }
        public string caseResponsible { get { return this.caseresponsible; } set { this.caseresponsible = value; } }
        public DateTime caseEndDate { get { return this.caseenddate; } set { this.caseenddate = value; } }

        public string printDate()
        {
            return caseEndDate.ToString("yyyy-MM-dd");
        }
    }
    public partial class caseForm : LayoutsPageBase
    {

        public Case MyCase { get; set; }

        private bool edititem = false;
        private int itemid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            MyCase = new Case();

            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists[ErrandDefinitions.ListName];
            SPListItemCollection listItems = list.Items;
            SPUserCollection userCollection = web.AllUsers;

            if (!IsPostBack)
            {
                // responsible person. get all users with email
                foreach (SPUser spUser in userCollection)
                {
                    if (!string.IsNullOrEmpty(spUser.Email))
                    {
                        dropdownUsers.Items.Add(new ListItem(spUser.Name, spUser.ID.ToString()));
                    }
                }

                dropdownUsers.DataTextField = "Name";
                dropdownUsers.DataValueField = "ID";

                SPFieldChoice spfieldchoice = new SPFieldChoice(list.Fields, ErrandDefinitions.PrioFieldName);

                for (int x = 0; x < spfieldchoice.Choices.Count; x++)
                {
                    priorityradiolist.Items.Add(new ListItem(spfieldchoice.Choices[x], spfieldchoice.Choices[x]));
                }
          
                priorityradiolist.DataBind();

                SPFieldChoice spfieldchoicestatus = new SPFieldChoice(list.Fields, ErrandDefinitions.StatusFieldName);

                for (int x = 0; x < spfieldchoicestatus.Choices.Count; x++)
                {
                    statusradiolist.Items.Add(new ListItem(spfieldchoicestatus.Choices[x], spfieldchoicestatus.Choices[x]));
                }

                
                SPFieldChoice spfieldchoicecat = new SPFieldChoice(list.Fields, ErrandDefinitions.CategoryFieldName);

                for (int x = 0; x < spfieldchoicecat.Choices.Count; x++)
                {
                    categoryradiolist.Items.Add(new ListItem(spfieldchoicecat.Choices[x], spfieldchoicecat.Choices[x]));
                }

                SPListItemCollection customersItems = web.Lists[ErrandDefinitions.CustomerListName].Items;

                foreach(SPListItem customeritem in customersItems)
                {
                    dropdownCustomers.Items.Add(new ListItem(customeritem["Title"].ToString(), customeritem.ID.ToString()));
                }

                //dropdownCustomers.DataSource = customersItems;
  
                dropdownCustomers.DataBind();

                if (!string.IsNullOrEmpty(Request.QueryString["itemID"]))
                {
                    itemid = Convert.ToInt32(Request.QueryString["itemID"]);
                    edititem = true;

                    SPListItem targetEditItem = listItems.GetItemById(itemid);

                    MyCase.caseName = targetEditItem["Title"].ToString();
                    MyCase.caseDescription = targetEditItem[ErrandDefinitions.DescriptionFieldName].ToString();
                    MyCase.casePriority = targetEditItem[ErrandDefinitions.PrioFieldName].ToString();
                    MyCase.caseStatus = targetEditItem[ErrandDefinitions.StatusFieldName].ToString();
                    MyCase.caseCategory = targetEditItem[ErrandDefinitions.CategoryFieldName].ToString();
                    MyCase.caseCustomer = targetEditItem[ErrandDefinitions.CustomerNameLookUpFieldName].ToString();
                    MyCase.caseResponsible = targetEditItem[ErrandDefinitions.ResponsibleFieldName].ToString();
                    MyCase.caseEndDate = DateTime.Parse(targetEditItem[ErrandDefinitions.EndDateFieldName].ToString());

                    SPFieldUserValue spfielduservalue = new SPFieldUserValue(web, MyCase.caseResponsible.ToString());
                    dropdownUsers.SelectedValue = spfielduservalue.User.ID.ToString();

                    SPFieldLookupValue spfieldlookupcustomervalue = new SPFieldLookupValue(MyCase.caseCustomer);

                    dropdownCustomers.SelectedValue = spfieldlookupcustomervalue.LookupId.ToString();

                    for (int x = 0; x < priorityradiolist.Items.Count; x++)
                    {
                        if (IfPriorityChecked(priorityradiolist.Items[x]))
                        {
                            priorityradiolist.Items[x].Selected = true;
                        }
                    }

                    // 
                    for (int x = 0; x < statusradiolist.Items.Count; x++)
                    {
                        if (IfStatusChecked(statusradiolist.Items[x]))
                        {
                            statusradiolist.Items[x].Selected = true;
                        }
                    }
                    for (int x = 0; x < categoryradiolist.Items.Count; x++)
                    {
                        if (ifCatChecked(categoryradiolist.Items[x]))
                        {
                            categoryradiolist.Items[x].Selected = true;
                        }
                    }
                }
            }
        }
        public void submitData(object sender, EventArgs e)
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists[ErrandDefinitions.ListName];
            SPListItemCollection listItems = list.Items;

            MyCase = new Case();

            if (!string.IsNullOrEmpty(Request.QueryString["itemID"]))
            {
                itemid = Convert.ToInt32(Request.QueryString["itemID"]);
                edititem = true;

                SPListItem targetEditItem = listItems.GetItemById(itemid);

                MyCase.caseName = targetEditItem["Title"].ToString();
                MyCase.caseDescription = targetEditItem[ErrandDefinitions.DescriptionFieldName].ToString();
                MyCase.casePriority = targetEditItem[ErrandDefinitions.PrioFieldName].ToString();
                MyCase.caseStatus = targetEditItem[ErrandDefinitions.StatusFieldName].ToString();
                MyCase.caseCategory = targetEditItem[ErrandDefinitions.CategoryFieldName].ToString();
                MyCase.caseCustomer = targetEditItem[ErrandDefinitions.CustomerNameLookUpFieldName].ToString();
                MyCase.caseResponsible = targetEditItem[ErrandDefinitions.ResponsibleFieldName].ToString();
                MyCase.caseEndDate = DateTime.Parse(targetEditItem[ErrandDefinitions.EndDateFieldName].ToString());
            }


            string selvalue = "";
            for (int x = 0; x < priorityradiolist.Items.Count; x++)
            {
                if (priorityradiolist.Items[x].Selected) 
                {
                    selvalue = priorityradiolist.Items[x].Value;
                }
            }

            // if nothing is empty.
            if (
                Request.Form["caseName"] != null &&
                Request.Form["caseDescription"] != null &&
                priorityradiolist.SelectedValue  != null &&
                categoryradiolist.SelectedValue != null &&
                dropdownCustomers.SelectedValue != null &&
                    dropdownUsers.SelectedValue != null
                )
            {

                if (edititem == false)
                {
                    SPListItem item = listItems.Add();
                    string casename = Request.Form["caseName"].ToString();
                    string casedescription = Request.Form["caseDescription"].ToString();
                    string casepriority = priorityradiolist.SelectedValue.ToString();
                    string casestatus = statusradiolist.SelectedValue;
                    string casecategory = categoryradiolist.SelectedValue;

                    SPFieldLookupValue lookupvaluecustomer = new SPFieldLookupValue(Convert.ToInt32(dropdownCustomers.SelectedValue), dropdownCustomers.SelectedItem.Text);
                    string casecustomer = lookupvaluecustomer.ToString();

                    string caseresponsible = dropdownUsers.SelectedValue;
                    DateTime caseenddate = DateTime.Parse(Request.Form["enddate"].ToString());

                    item["Title"] = casename;
                    item[ErrandDefinitions.DescriptionFieldName] = casedescription;
                    item[ErrandDefinitions.PrioFieldName] = casepriority;
                    item[ErrandDefinitions.StatusFieldName] = casestatus;
                    item[ErrandDefinitions.CategoryFieldName] = casecategory;
                    item[ErrandDefinitions.CustomerNameLookUpFieldName] = casecustomer;
                    item[ErrandDefinitions.ResponsibleFieldName] = caseresponsible;
                    item[ErrandDefinitions.EndDateFieldName] = caseenddate;

                    item.Update();

                    // Update page
                    Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    SPListItem item = listItems.GetItemById(itemid);

                    string casename = Request.Form["caseName"].ToString();
                    string casedescription = Request.Form["caseDescription"].ToString();
                    string casepriority = priorityradiolist.SelectedValue;
                    string casestatus = statusradiolist.SelectedValue;
                    string casecategory = categoryradiolist.SelectedValue;
                    SPFieldLookupValue lookupvaluecustomer = new SPFieldLookupValue(Convert.ToInt32(dropdownCustomers.SelectedValue), dropdownCustomers.SelectedItem.Text);
                    string casecustomer = lookupvaluecustomer.ToString();
                    string caseresponsible = dropdownUsers.SelectedValue;
                    DateTime caseenddate = DateTime.Parse(Request.Form["enddate"].ToString());

                    item["Title"] = casename;
                    item[ErrandDefinitions.DescriptionFieldName] = casedescription;
                    item[ErrandDefinitions.PrioFieldName] = casepriority;
                    item[ErrandDefinitions.StatusFieldName] = casestatus;
                    item[ErrandDefinitions.CategoryFieldName] = casecategory;
                    item[ErrandDefinitions.CustomerNameLookUpFieldName] = casecustomer;
                    item[ErrandDefinitions.ResponsibleFieldName] = caseresponsible;
                    item[ErrandDefinitions.EndDateFieldName] = caseenddate;

                    item.Update();

                    // Update page 
                    Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
                    Response.Flush();
                    Response.End();
                }
            }  
        }
        
        public bool IfPriorityChecked(object value)
        {
            string valuestr = value.ToString();
           // return valuestr == MyCase.casePriority ? "checked=\"checked\"" : "";
           return valuestr == MyCase.casePriority ? true : false;
        }
        public bool IfStatusChecked(object value)
        {
            string valuestr = value.ToString();
            // return valuestr == MyCase.casePriority ? "checked=\"checked\"" : "";
            return valuestr == MyCase.caseStatus ? true : false;
        }
        public bool ifCatChecked(object value)
        {
            string valuestr = value.ToString();
            // return valuestr == MyCase.casePriority ? "checked=\"checked\"" : "";
            return valuestr == MyCase.caseCategory ? true : false;
        }
        public string testFunc(object value)
        {
            return "hello" + value.ToString();
        }
    }
}
