<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<NProject.Models.Domain.User>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>List</h2>

    <table>
        <tr>
            <th></th>
            <th>
                Id
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
                <%: Html.ActionLink("Edit", "Edit", new {  id=item.Id  }) %> |
                <%: Html.ActionLink("Delete", "Delete", new { id = item.Id })%>
            </td>
            <td>
                <%: item.Id %>
            </td>
            <td>
                <%: item.Username %>
            </td>
            <td>
                <%: item.Email %>
            </td>
            <td>
                <%: item.Role %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Register") %>
    </p>

</asp:Content>

