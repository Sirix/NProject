<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.Models.Domain.Project>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>Fields</legend>
        
        <div class="display-label">Id</div>
        <div class="display-field"><%: Model.Id %></div>
        
        <div class="display-label">Name</div>
        <div class="display-field"><%: Model.Name %></div>
        
        <div class="display-label">TotalCost</div>
        <div class="display-field"><%: String.Format("{0:F}", Model.TotalCost) %></div>
        
        <div class="display-label">Progress</div>
        <div class="display-field"><%: Model.Progress %></div>
        
        <div class="display-label">PriceDiscount</div>
        <div class="display-field"><%: String.Format("{0:F}", Model.PriceDiscount) %></div>
        
        <div class="display-label">CreationDate</div>
        <div class="display-field"><%: String.Format("{0:g}", Model.CreationDate) %></div>
        
        <div class="display-label">StartDate</div>
        <div class="display-field"><%: String.Format("{0:g}", Model.StartDate) %></div>
        
        <div class="display-label">DeliveryDate</div>
        <div class="display-field"><%: String.Format("{0:g}", Model.DeliveryDate) %></div>
        
    </fieldset>
    <p>
        <% if ((bool)ViewData["ShowEditAction"])
           {%>
        <%:Html.ActionLink("Edit", "Edit", new {id = Model.Id})%> |
        <%
           }%>
        <%: Html.ActionLink("Back to List", "List") %>
    </p>

</asp:Content>

