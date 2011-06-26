<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<NProject.Models.Domain.Project>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: ViewData["TableTitle"] %></h2>
    <% if (Model.Count() > 0)
       {%>
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
            <th>
                Current project status
            </th>
        </tr>

    <%
           foreach (var item in Model)
           {%>
    
        <tr>
            <td>
                <%:Html.ActionLink("Edit", "Edit", new {/* id=item.PrimaryKey */})%> |
                <%:Html.ActionLink("Details", "Details", new { id = item.Id })%> |
                <%:Html.ActionLink("Delete", "Delete", new {/* id=item.PrimaryKey */})%> | 
                <%:Html.ActionLink("View team", "Team", new { id = item.Id })%> | 
                <%:Html.ActionLink("View tasks", "Tasks", new { id = item.Id })%>
            </td>
            <td>
                <%:item.Id%>
            </td>
            <td>
                <%:item.Name%>
            </td>
            <td>
                <%:String.Format("{0:F}", item.TotalCost)%>
            </td>
            <td>
                <%:item.Progress%>
            </td>
            <td>
                <%:String.Format("{0:g}", item.CreationDate)%>
            </td>
            <td>
                <%:String.Format("{0:g}", item.DeliveryDate)%>
            </td>
            <td>
                <%:item.Status%>
            </td>
        </tr>
    
    <%
           }
       }
       else
       {
%>
 <h3>You have no projects yet!</h3>
       <%
       }%>
    </table>

</asp:Content>

