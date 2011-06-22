<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<NProject.Models.Domain.Project>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h1>Hello, <%:ViewData["User"] %></h1>
    <h2>Index</h2>

    <table>
        <tr>
            <th></th>
            <th>
                Id
            </th>
            <th>
                Name
            </th>
            <th>
                TotalCost
            </th>
            <th>
                Progress
            </th>
            <th>
                CreationDate
            </th>
            <th>
                DeliveryDate
            </th>
        </tr>

    <%if(Model!=null)
                     foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */ }) %> |
                <%: Html.ActionLink("Details", "Details", new { /* id=item.PrimaryKey */ })%> |
                <%: Html.ActionLink("Delete", "Delete", new { /* id=item.PrimaryKey */ })%>
            </td>
            <td>
                <%: item.Id %>
            </td>
            <td>
                <%: item.Name %>
            </td>
            <td>
                <%: String.Format("{0:F}", item.TotalCost) %>
            </td>
            <td>
                <%: item.Progress %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.CreationDate) %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.DeliveryDate) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

