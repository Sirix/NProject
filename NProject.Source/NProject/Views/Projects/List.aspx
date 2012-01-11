<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.ViewModels.ProjectListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Model.TableTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: Model.TableTitle %></h2>
    <% if (Model.Projects.Count() > 0)
       {%>
    <table>
        <tr>
            <th></th>
            <th>
                Name
            </th>
            <th>
                CreationDate
            </th>
            <th>
                DeliveryDate
            </th>
            <th>
                Project status
            </th>
            <% if (Model.UserCanCreateAndDeleteProject) { %>  
                  <th>Responsible</th>
                <%  }%>
        </tr>

    <%
           foreach (var item in Model.Projects)
           {%>
    
        <tr>
            <td>
                <%:Html.ActionLink("Details", "Details", new { id = item.Id })%> | 
                
                <% if (!Model.UserIsCustomer)
{%>
                <%:Html.ActionLink("View team", "Team", new {id = item.Id})%> | 
                <%:Html.ActionLink("View tasks", "AtProject", "Task", new { id = item.Id }, new object())%> |
                <%
}%>                <% if (Model.UserCanManageMeetings) { %> 
               <!--  | <%:Html.ActionLink("Meetings", "List", "Meeting", new {id = item.Id}, new object())%> -->
                <%
}%>
                <% if (Model.UserCanCreateAndDeleteProject) { %>  
                    <%:Html.ActionLink("Edit", "Edit", new { id=item.Id })%>
                <%  }%>
            </td>
            <td>
                <%:item.Name%>
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
            <% if (Model.UserCanCreateAndDeleteProject) { %>  
                  <td><%: item.Team.First(i=>i.Role == NProject.Models.Domain.UserRole.Manager).User.Name%></td>
                <%  }%>
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
    <% if (Model.UserCanCreateAndDeleteProject)
       {%>
     <%:Html.ActionLink("Create new project", "Create")%>
     <%
       }%>
</asp:Content>

