<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.Models.Domain.Task>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit task</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.HiddenFor(model=>model.Id) %>
        <fieldset>
            <legend>Fields</legend>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.Title) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Title) %>
                <%: Html.ValidationMessageFor(model => model.Title) %>
            </div>                   
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Description) %>
                <%: Html.ValidationMessageFor(model => model.Description) %>
            </div>
                        
            <div class="editor-label">
                <%: Html.LabelFor(model => model.BeginDate) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.BeginDate, String.Format("{0:g}", Model.BeginDate)) %>
                <%: Html.ValidationMessageFor(model => model.BeginDate) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.EndDate) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.EndDate, String.Format("{0:g}", Model.EndDate)) %>
                <%: Html.ValidationMessageFor(model => model.EndDate) %>
            </div>
                        <div class="editor-label">
                <%: Html.LabelFor(model => model.Status) %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownList("statusId", (IEnumerable<SelectListItem>)ViewData["Statuses"]) %>
                <%: Html.ValidationMessageFor(model => model.Status) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Responsible) %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownList("responsibleId", (IEnumerable<SelectListItem>)ViewData["Users"]) %>
                <%: Html.ValidationMessageFor(model => model.Responsible.Id) %>
            </div>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "Tasks", "Projects", new { id = ViewData["ProjectId"] }, new object())%>
    </div>

</asp:Content>

