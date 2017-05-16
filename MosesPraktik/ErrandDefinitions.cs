using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosesPraktik
{
    public static class ErrandDefinitions
    {
        // list name and description
        private static string listname = "Ärenden";

        private static string customerlistname = "Kundlista";

        private static string listdescription = "En lista som innehåller ärenden för PrecioFishbone";

        // content type name
        private static string contenttypename = "Ärende";


        // column group
        private static string contenttypegroup = "Custom Content Types";

        // field names
        private static string description_fieldname = "Beskrivning";
        private static string status_fieldname = "Status";
        private static string category_fieldname = "Kategori";
        private static string prio_fieldname = "Prio";
        private static string responsible_fieldname = "Ansvarig";
        private static string customer_fieldname = "Kund";
        private static string enddate_fieldname = "Slutdatum";
        private static string notificationsent_fieldname = "NotificationSent";


        // relationship field
        private static string customernamelookupname = "Kund";

        // title display name
        private static string titledisplaytitle = "Titel";

        // Status choices
        private static string[] status_choices = { "Stängd", "Klar", "Aktiv", "Ny" };
        private static string status_choices_default = status_choices[3];

        // Prio choices
        private static string[] prio_choices = { "Låg", "Normal", "Hög", "Akut" };
        private static string prio_choices_default = prio_choices[1];

        // Category choices
        private static string[] category_choices = { "Support", "Ändring", "Förfrågan" };
        private static string category_choices_default = category_choices[0];


        // list name and description
        public static string ListName { get { return listname; } }
        public static string ListDescription { get { return listdescription; } }

        public static string CustomerListName { get { return customerlistname; } }

        // field names
        public static string DescriptionFieldName { get { return description_fieldname; } }
        public static string StatusFieldName { get { return status_fieldname; } }
        public static string CategoryFieldName { get { return category_fieldname; } }
        public static string PrioFieldName { get { return prio_fieldname; } }
        public static string ResponsibleFieldName { get { return responsible_fieldname; } }
        public static string CustomerFieldName { get { return customer_fieldname; } }
        public static string EndDateFieldName { get { return enddate_fieldname; } }
        public static string NotificationSentFieldName { get { return notificationsent_fieldname; } }

        // column group
        public static string ContentTypeGroup { get { return contenttypegroup; } }

        // display title
        public static string TitleDisplayTitle { get { return titledisplaytitle; } }

        // prio choice
        public static string[] PrioChoices { get { return prio_choices; } }
        public static string PrioChoicesDefault { get { return prio_choices_default; } }

        // status choice
        public static string[] StatusChoices { get { return status_choices; } }
        public static string StatusChoicesDefault { get { return status_choices_default; } }

        // category choice
        public static string[] CategoryChoices { get { return category_choices; } }
        public static string CategoryChoicesDefault { get { return category_choices_default; } }

        // content type name
        public static string ContentTypeName { get { return contenttypename; } }

        // relationship field
        public static string CustomerNameLookUpFieldName
        {
            get
            {
                return customernamelookupname;
            }
        }
    }
}
