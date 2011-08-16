<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.Models.Domain.Project>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delete</h2>

    <h3>Are you sure you want to delete this?</h3>
    <fieldset>
        <legend>Project</legend>
        
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
    <% using (Html.BeginForm()) { %>
    <%: Html.AntiForgeryToken() %>
        <p>
		    <input type="submit" value="Delete" /><br /><br />
		    <%: Html.ActionLink("Back to List", "List") %>
        </p>
    <% } %>

</asp:Content>

