<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<NProject.Models.Domain.Task>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ProjectTasks
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>"<%:ViewData["ProjectTitle"] %>" tasks</h2>
     <p>
        <%: Html.ActionLink("Back to projects", "List")%>
    </p>
    <table>
        <tr>
            <th></th>
            <th>
                Title
            </th>
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
            <th>
                Status
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
            <% if((bool)ViewData["CanExecuteTask"])
{%>
                <%: Html.ActionLink("Start task", "Take", "Task", new {  id=item.Id }, new object()) %> |
                <%: Html.ActionLink("Complete", "Complete", "Task", new {  id=item.Id }, new object()) %> |
            <%
}%>
            <% if ((bool)ViewData["CanCreateTasks"])
{%>
                <%: Html.ActionLink("Edit", "Edit", "Task", new {  id=item.Id }, new object()) %>
           <!--   |   <%: Html.ActionLink("Delete", "Delete", "Task",new {  id=item.Id }, new object()) %> -->
            <%}%>
            </td>
            <td>
                <%: item.Title %>
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
            <td>
                <%: item.Status %>
            </td>
        </tr>
    
    <% } %>

    </table>
    <%if ((bool)ViewData["CanCreateTasks"])
      {%>
    <p>
        <%:Html.ActionLink("Add new task", "AddToProject", "Task", new {id = ViewData["ProjectId"]},
                                            new object {})%>
    </p>
    <%
      }%>
</asp:Content>

