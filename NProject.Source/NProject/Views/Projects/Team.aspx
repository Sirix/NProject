<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<NProject.Models.Domain.TeamMate>>" %>

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
        <% if ((bool)ViewData["CanManageTeam"])
           {%>
            <th>
            </th>
            <%
           }%>
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
         <% if ((bool)ViewData["CanManageTeam"])
{%>
            <td>
                <%:Html.ActionLink("Remove from team", "RemoveStaff",
                                      new {id = ViewData["ProjectId"], userId = item.Id})%>
            </td>
            <%
}%>
            <td>
                <%: item.User.Name %>
            </td>
            <td>
                <%: item.User.Email %>
            </td>
            <td>
                <%: item.Role %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
    <%if ((bool)ViewData["CanManageTeam"])  %>
        <%: Html.ActionLink("Add staff", "AddStaff", new { id = ViewData["ProjectId"] })%>
    </p>

</asp:Content>