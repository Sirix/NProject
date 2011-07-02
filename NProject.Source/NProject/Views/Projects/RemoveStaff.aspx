<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Remove staff
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Removing user from the team</h2>    
    <fieldset>
    Are you sure to remove <b><%: ViewData["UserName"] %></b> from <b>"<%: ViewData["ProjectName"] %>"</b>'s team?
    </fieldset>
     <% using (Html.BeginForm()) { %>
    <%: Html.AntiForgeryToken() %>
    <%: Html.Hidden("id", ViewData["ProjectId"]) %>
    <%: Html.Hidden("userId", ViewData["UserId"]) %>
        <p>
		    <input type="submit" value="Remove" /><br /><br />
		    <%: Html.ActionLink("Cancel", "Team", new {id=ViewData["ProjectId"]}) %>
        </p>
    <% } %>
</asp:Content>
