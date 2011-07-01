<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<NProject.Models.Domain.Task>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ProjectTasks
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Project tasks</h2>

    <table>
        <tr>
            <th></th>
            <th>
                Description
            </th>
            <th>
                CreationDate
            </th>
            <th>
                BeginDate
            </th>
            <th>
                EndDate
            </th>
            <th>
                Responsible
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: Html.ActionLink("Edit", "Edit", "Task", new {  id=item.Id }, new object()) %> |
                <%: Html.ActionLink("Details", "Details", "Task",new {  id=item.Id }, new object()) %> |
                <%: Html.ActionLink("Delete", "Delete", "Task",new {  id=item.Id }, new object()) %> |
            </td>
            <td>
                <%: item.Description %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.CreationDate) %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.BeginDate) %>
            </td>
            <td>
                <%: String.Format("{0:g}", item.EndDate) %>
            </td>
            <td>
                <%: item.Responsible.Username %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Add new task", "AddToProject", "Task", new { id = ViewData["ProjectId"] }, new object { })%>
    </p>

</asp:Content>

