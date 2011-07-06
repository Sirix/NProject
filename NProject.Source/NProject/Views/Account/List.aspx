<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<NProject.Models.Domain.User>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>User's list</h2>

    <table>
        <tr>
            <th></th>
            <th>
                Username
            </th>
            <th>
                Email
            </th>
            <th>
                Role
            </th>
            <th>
                State
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new {  id=item.Id  }) %>
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
            <td>
                <%: item.UserState %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Register") %>
    </p>

</asp:Content>

