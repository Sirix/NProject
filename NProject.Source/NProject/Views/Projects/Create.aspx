<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.Models.Domain.Project>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: ViewData["PageTitle"] %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: ViewData["PageTitle"] %></h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary() %>
        <%: Html.AntiForgeryToken() %>
        <% if (Model != null)  %>
        <%: Html.HiddenFor(model=>model.Id) %>
        <fieldset>
            <legend>Fields</legend>
                        
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.TotalCost) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.TotalCost) %>
                <%: Html.ValidationMessageFor(model => model.TotalCost) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.PriceDiscount) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.PriceDiscount)%>
                <%: Html.ValidationMessageFor(model => model.PriceDiscount)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Progress) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Progress) %>
                <%: Html.ValidationMessageFor(model => model.Progress) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.StartDate) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.StartDate) %>
                <%: Html.ValidationMessageFor(model => model.StartDate) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.DeliveryDate) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.DeliveryDate) %>
                <%: Html.ValidationMessageFor(model => model.DeliveryDate) %>
            </div>
             <div class="editor-label">
                <%: Html.Label("Status") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownList("statusId", (IEnumerable<SelectListItem>)ViewData["Statuses"]) %>
            </div>
            <div class="editor-label">
                <%: Html.Label("Project Manager") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownList("pmId", (IEnumerable<SelectListItem>)ViewData["PM"]) %>
            </div>
            <div class="editor-label">
                <%: Html.Label("Customer") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownList("customerId", (IEnumerable<SelectListItem>)ViewData["Customer"]) %>
            </div>
            <p>
                <input type="submit" value="<%: ViewData["PageTitle"] %>" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "List") %>
    </div>

</asp:Content>

