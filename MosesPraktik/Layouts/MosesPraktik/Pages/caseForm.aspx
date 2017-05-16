<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="caseForm.aspx.cs" Inherits="MosesPraktik.Layouts.MosesPraktik.Pages.caseForm" DynamicMasterPageFile="~masterurl/default.master" %>

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
                            <label for="titleTextInput" class="smallformheader">Ärendenamn</label><br />
                            <input required="true" class="form-control" name="caseName" id="titleTextInput" type="text" value="<%=MyCase.caseName %>">
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label for="dropdownCustomers" class="smallformheader">Ange kund</label><br />
                            <asp:DropDownList class="form-control" runat="server" ID="dropdownCustomers" required="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3">
                        <div id="chooseperson" class="form-group">
                            <label for="dropdownUsers" class="smallformheader">Tilldela ärende till en person</label><br />
                            <asp:DropDownList class="form-control" runat="server" ID="dropdownUsers" required="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group">
                        <div class="col-sm-12">
                            <label for="descriptionTextareaInput" class="smallformheader">Beskriv ärendet</label><br />
                            <textarea required="true" class="form-control" cols="50" rows="6" name="caseDescription" id="descriptionTextareaInput"><%= MyCase.caseDescription %></textarea>
                        </div>
                    </div>
                </div>
                <div class="row midcols">
                    <div class="col-sm-3">
                       <div id="chooseprio" class="form-group">

                        <label for="chooseprio" class="smallformheader">Välj prioritet</label>
                        <asp:RadioButtonList runat="server" ID="priorityradiolist">
                        </asp:RadioButtonList>

                        </div>
                    </div>
                    <div class="col-sm-3">
                        <label for="choosestatus" class="smallformheader">Välj Status</label>
                        <div id="choosestatus" class="form-group">
                        <asp:RadioButtonList runat="server" ID="statusradiolist">
                        </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <label for="choosecategory" class="smallformheader">Ange kategori</label>
                        <div id="choosecategory" class="form-group">
                        <asp:RadioButtonList runat="server" ID="categoryradiolist">
                        </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3">
                        <div class="form-group">
                            <div class="form-group">
                                <label for="datetimepicker1" class="smallformheader">Ange sluttid för ärendet</label>
                                <div class='input-group date' id='datetimepicker1'>
                                    <input type='text' class="form-control" name="enddate" value="<%= MyCase.printDate() %>"/>
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
                        <asp:Button ID="buttonsubmit" class="submitbtn" runat="server" OnClick="submitData" value="Skicka" Text="Skicka" />
                       <!-- <input class="form-control" name="caseSubmit" id="submitCase" type="submit" value="Skicka" /> -->
                    </div>
                </div>
            </fieldset>
        </form>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Lägg till ärende
</asp:Content>

<asp:Content Visible="false" ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Lägg till ärende
</asp:Content>
