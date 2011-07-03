<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.Models.MeetingsListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Meetings list
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <h2><%: Model.TableTitle %></h2>
    <% if (Model.Meetings.Count() > 0)
       {%>
    <table>
        <tr>
            <th></th>
            <th>
                Title
            </th>
            <th>
                Result
            </th>
            <th>
                Meeting date
            </th>
        </tr>

    <%
           foreach (var item in Model.Meetings)
           {%>
    
        <tr>
            <td>
                <%:Html.ActionLink("Details", "Details", new { id = item.Id })%> | 
                <%:Html.ActionLink("Edit", "Edit", new {/* id=item.PrimaryKey */})%> |
                <% if (false)
{%>
                <%:Html.ActionLink("View team", "Team", new {id = item.Id})%> | 
                <%:Html.ActionLink("View tasks", "Tasks", new {id = item.Id})%> |
                <%
}%>               
                <% if (Model.UserCanCreateAndDeleteMeeting) { %>  
                   <%:Html.ActionLink("Delete", "Delete", new { id=item.Id})%>
                <%  }%>
            </td>
            <td>
                <%:item.Title%>
            </td>
            <td>
                <%:item.Result_status%>
            </td>
            <td>
                <%:String.Format("{0:g}", item.Meeting_date)%>
            </td>
        </tr>
    
    <%
           }
       }
       else
       {
%>
 <h3>You have no meetings</h3>
       <%
       }%>
    </table>
    <br />
    <% if (Model.UserCanCreateAndDeleteMeeting)
       {%>
     <%:Html.ActionLink("Create new meeting", "Create")%>
     <%
       }%>

</asp:Content>
