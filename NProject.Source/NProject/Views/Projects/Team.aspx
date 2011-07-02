<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<NProject.Models.Domain.User>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Team
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <%: Html.ActionLink("Back to projects list", "list")%>
    </p>
    <h2>Team</h2>
    <table>
        <tr>
            <th>
            </th>
            <th>
                Username
            </th>
            <th>
                Email
            </th>
            <th>
                Role
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Remove from team", "RemoveStaff", new { id = ViewData["ProjectId"], userId=item.Id})%>
            </td>
            <td>
                <%: item.Username %>
            </td>
            <td>
                <%: item.Email %>
            </td>
            <td>
                <%: item.Role.Name %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Add staff", "AddStaff", new { id = ViewData["ProjectId"] })%>
    </p>

</asp:Content>

