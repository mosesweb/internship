<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="caseEdit.aspx.cs" Inherits="MosesPraktik.Layouts.MosesPraktik.Pages.caseEdit" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />

    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous" />

    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>

    <script type="text/javascript" src="/_layouts/15/MosesPraktik/Scripts/moment-with-locales.js"></script>
    <script type="text/javascript" src="/_layouts/15/MosesPraktik/Scripts/bootstrap-datetimepicker.js"></script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
     <div class="container">
        <form action="caseForm.aspx" method="post">
            <fieldset id="caseFormMainGroup">
                <div class="row">
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label for="titleTextInput">Ärendenamn</label><br />
                            <input required="true" class="form-control" name="caseName" id="titleTextInput" type="text" />
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label for="dropdownCustomers">Ange kund</label><br />
                            <asp:DropDownList class="form-control" runat="server" ID="dropdownCustomers" required="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3">
                        <div id="chooseperson" class="form-group">
                            <label for="dropdownUsers">Tilldela ärende till en person</label><br />
                            <asp:DropDownList class="form-control" runat="server" ID="dropdownUsers" required="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group">
                        <div class="col-sm-12">
                            <label for="descriptionTextareaInput">Beskriv ärendet</label><br />
                            <textarea required="true" class="form-control" cols="50" rows="6" name="caseDescription" id="descriptionTextareaInput"></textarea>
                        </div>
                    </div>
                </div>
                <div class="row midcols">
                    <div class="col-sm-3">
                        <label for="chooseprio">Välj prioritet</label>
                        <div id="chooseprio" class="form-group">
                            <div class="radio">
                                <label>
                                    <input type="radio" name="casePriority" id="caseRadioPrioEmergency" value="Akut" />Akut</label><br />
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="casePriority" id="caseRadioPrioHigh" value="Hög" />Hög</label><br />
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="casePriority" id="caseRadioPrioNormal" value="Normal" checked="checked" />Normal</label><br />
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="casePriority" id="caseRadioPrioLow" value="Låg" />Låg</label>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <label for="choosestatus">Välj Status</label>
                        <div id="choosestatus" class="form-group">
                            <div class="radio">
                                <label>
                                    <input type="radio" name="caseStatus" id="caseRadioStatusNew" value="Ny" checked="checked" />Ny</label>
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="caseStatus" id="caseRadioStatusActive" value="Aktiv" />Aktiv</label>
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="caseStatus" id="caseRadioStatusFinish" value="Klar" />Klar</label>
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="caseStatus" id="caseRadioStatusClosed" value="Stängd" />Stängd</label>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <label for="choosecategory">Ange kategori</label>
                        <div id="choosecategory" class="form-group">
                            <div class="radio">
                                <label>
                                    <input type="radio" name="caseCategory" id="CaseCategoryRadioSupport" value="Support" checked="checked" />Support</label>
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="caseCategory" id="CaseCategoryRadioChange" value="Ändring" />Ändring</label>
                            </div>
                            <div class="radio">
                                <label>
                                    <input type="radio" name="caseCategory" id="CaseCategoryRadioRequest" value="Förfrågan" />Förfrågan</label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3">
                        <div class="form-group">
                            <div class="form-group">
                                <label for="datetimepicker1">Ange sluttid för ärendet</label>
                                <div class='input-group date' id='datetimepicker1'>
                                    <input type='text' class="form-control" name="enddate" />
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <script type="text/javascript">
                            $(function () {
                                $('#datetimepicker1').datetimepicker(
                                {
                                    locale: 'sv',
                                    format: 'YYYY-MM-DD'
                                });
                            });
                        </script>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3">
                        <input class="form-control" name="caseSubmit" id="submitCase" type="submit" value="Skicka" />
                    </div>
                </div>
            </fieldset>
        </form>
    </div>
    <script type="text/javascript">

        var clientContext;

        $(document).ready(function () {
            SP.SOD.executeFunc('sp.js', 'SP.ClientContext', execOperation);
        });

        function execOperation() {
            
            function GetListItemsFromSPList(listname) {
                var siteUrl = _spPageContextInfo.webAbsoluteUrl;

                $.ajax({
                    url: siteUrl + "/_api/web/lists/getbytitle('" + listname + "')/items",
                    method: "GET",
                    headers: { "Accept": "application/json; odata=verbose" },
                    success: function (data) {
                        // Returning the results
                        var listItems = data.d.results;
                        listItems.forEach(function (entry) {
                            // Do something with list item which is in 'entry' object
                            alert("hello" + entry.Title)
                        });
                    },
                    error: function (data) {
                        alert("Error: " + data)
                    }
                });
            }

            GetListItemsFromSPList('renden');


        }
        
       

    </script>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
