$(document).ready()
{
    var url = "";
    function popupCreate(listname, width, height) {

        function init() {
        var url = _spPageContextInfo.siteAbsoluteUrl;
        var pageUrl = url + "/Lists/Kundlista/NewForm.aspx";
        var errandslistname = "renden"; // Ärenden
        var customerlistname = "Kundlista" // Kundlista
        createcustomerLink = url + "/Lists/" + customerlistname + "/NewForm.aspx";
        createErrandLink = url + "/Lists/" + errandslistname + "/NewForm.aspx";

        switch(listname)
        {
            case 'Kundlista':
            pageUrl = url + "/_layouts/15/MosesPraktik/Pages/customerForm.aspx";
            break;

            case 'renden':
            pageUrl = url + "/_layouts/15/MosesPraktik/Pages/caseForm.aspx";
            break;

            default:
            pageUrl = url + "/Lists/" + listname + "/NewForm.aspx";
            break;
        }

        var options = {
            url: pageUrl,
            width: width,
            height: height,
            dialogReturnValueCallback: function (result) {
                if (result == SP.UI.DialogResult.OK) {
                    RefreshOnDialogClose(); // refresh on dialog close
                }
                if (result == SP.UI.DialogResult.cancel) {
                    //do nothing, modal was closed
                }
            }
        };

        SP.SOD.execute
        ('sp.ui.dialog.js',
        'SP.UI.ModalDialog.showModalDialog',
            options);
        }

        SP.SOD.executeOrDelayUntilScriptLoaded(init(), "sp.js");
    }

    function popupCreateCustomer()
    {
        popupCreate('Kundlista', 600, 200);
    }
    function popupCreateErrand()
    {
        popupCreate('renden', 600, 940);
    }
    function popupfromurl(pageUrl)
    {
        var options = {
            url: pageUrl,
            width: 500,
            height: 800,
            dialogReturnValueCallback: function (result) {
            if (result == SP.UI.DialogResult.OK) {
                RefreshOnDialogClose(); // refresh on dialog close
            }
            if (result == SP.UI.DialogResult.cancel) {
                //do nothing, modal was closed
            }
        }

    };

        SP.SOD.execute
        ('sp.ui.dialog.js',
        'SP.UI.ModalDialog.showModalDialog',
            options);
    }
};