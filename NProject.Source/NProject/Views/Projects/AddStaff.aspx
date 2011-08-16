<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	AddStaff
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Add staff to project team "<%:ViewData["ProjectTitle"] %>"</h2>
        <% using (Html.BeginForm()) {%>
        <%: Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(true) %>
        <fieldset>
        <% if (((IEnumerable<SelectListItem>)ViewData["Users"]).Count() > 0) {  %>
            <legend>Select staff and click "Add"</legend>
                        
            <div class="editor-label">
                <%: Html.Label("Add") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownList("userId", (IEnumerable<SelectListItem>)ViewData["Users"]) %>
            </div>
            <p>
                <input type="submit" value="Add" />
            </p>
        
        <% } else {%>
        Unfortunatelly, all programmers already take part in this project.
        <% } %>
        </fieldset>
    <% } %>

    <div>
        <%: Html.ActionLink("Back to team", "Team", new { id = ViewData["ProjectId"] })%>
    </div>
</asp:Content>
