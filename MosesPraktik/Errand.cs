using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace MosesPraktik
{
    class Errand
    {
        private string id;
        private string title = "";
        private string description = "";
        private string priority = "";
        private bool show_priority_icon = false;
        private string status = "";
        private SPUser responsible;
        private SPUser createdBy;
        private string category = "";
        private string customer = "";
        private string enddate = "";
        private string itemurl = "";
        private int order_weight = 0;
        private int order_prio = 0;
        private int order_prio_sort = 0;

        private int order_status = 0;
        private bool closed = false;
        private string print_priority_icon_adress = "/_layouts/15/MosesPraktik/Images/flag_2_left_red_256.png";
        private SPFieldLookupValue lookupvalue;

        public Errand(
        string theid, 
        string thetitle, 
        string thedescription, 
        string thepriority, 
        string thestatus,
        SPUser theresponsible, 
        string thecategory, 
        string thecustomer,
        SPUser thecreatedby,
        string theitemurl,
        SPFieldLookupValue thelookupvalue,
        string theenddate)
        {
            this.ID = theid;
            this.Title = thetitle;
            this.description = thedescription;
            this.priority = thepriority;
            this.status = thestatus;
            this.responsible = theresponsible;
            this.category = thecategory;
            this.customer = thecustomer;
            this.createdBy = thecreatedby;
            this.itemurl = theitemurl;
            this.lookupvalue = thelookupvalue;
            this.enddate = theenddate;

            switch(Priority)
            {
                case "Akut":
                OrderWeight += 0;
                OrderPrioSort = 0;
                ShowPriorityIcon = true;
                break;

                case "Hög":
                OrderWeight += 1;
                OrderPrioSort = 1;
                OrderPrio = 1;
                break;

                case "Normal":
                OrderWeight += 2;
                OrderPrioSort = 2;
                OrderPrio = 2;
                break;

                case "Låg":
                OrderWeight += 3;
                OrderPrioSort = 3;
                OrderPrio = 3;
                break;
            }

            switch (Status)
            {
                case "Ny":
                OrderWeight += 0;
                OrderStatus = 0;
                break;

                case "Aktiv":
                OrderWeight += 1;
                OrderStatus = 1;
                break;

                case "Klar":
                OrderWeight += 3;
                OrderStatus = 2;
                break;

                case "Stängd":
                OrderWeight += 3;
                OrderStatus = 3;
                Closed = true;
                break;
            }
        }
        
        public string ID { 
            get { return this.id; } 
            set { this.id = value; } 
        }
        public string Title 
        {
            get { return this.title; }
            set { this.title = value; } 
        }
        public string Description 
        {
            get { return this.description; }
            set { this.description = value; }
        }
        public string Priority 
        {
            get { return this.priority; }
            set { this.priority = value; }
        }
        public string Status 
        {
            get { return this.status; }
            set { this.status = value; }
        }
        public SPUser Responsible 
        {
            get { return this.responsible; }
            set { this.responsible = value; }
        }
        public SPUser CreatedBy
        {
            get { return this.createdBy; }
            set { this.createdBy = value; }
        }
        public string Category {
            get { return this.category; }
            set { this.category = value; }
        }
        public string Customer 
        {
            get { return this.customer; }
            set { this.customer = value; }
        }
        public string EndDate
        {
            get { return this.enddate; }
            set { this.enddate = value; }
        }
        public int OrderWeight
        {
            get { return this.order_weight; }
            set { this.order_weight = value; }
        }
        public int OrderPrioSort
        {
            get { return this.order_prio_sort; }
            set { this.order_prio_sort = value; }
        }
        public int OrderStatus
        {
            get { return this.order_status; }
            set { this.order_status = value; }
        }
        public int OrderPrio 
        {
            get { return this.order_prio; }
            set { this.order_prio = value; }
        }
        public int ResponsibleID {
            get {
                return this.Responsible.ID;
            }     
        }
        public string ResponsibleName
        {
            get
            {
                return this.Responsible.Name;
            }
        }
        public string CreatedByName
        {
            get
            {
                return this.createdBy.Name;
            }
        }
        public string ItemURL
        {
            set
            {
                itemurl = value;
            }
            get
            {
                return this.itemurl;
            }
        }

        public bool Closed
        {
            set
            {
                this.closed = value;
            }
            get
            {
                return this.closed;
            }
        }
        public bool ShowPriorityIcon
        {
            set
            {
                this.show_priority_icon = value;
            }
            get
            {
                return this.show_priority_icon;
            }
        }
        public string PrintPriorityIconAdress
        {
            set
            {
                this.print_priority_icon_adress = value;
            }
            get
            {
                return this.print_priority_icon_adress;
            }
        }
        public SPFieldLookupValue LookupValue
        {
            set
            {
                this.lookupvalue = value;
            }
            get
            {
                return this.lookupvalue;
            }
        }
        public string LookupValueIDString
        {
            get
            {
                return LookupValue.LookupId.ToString();
            }
        }
        public string LookupValueCustomerValue
        {
            get
            {
                return LookupValue.LookupValue;
            }
        }
    }
}
