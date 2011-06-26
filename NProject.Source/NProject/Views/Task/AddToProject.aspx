<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.Models.Domain.Task>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add task to project
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Add task to "<%:ViewData["ProjectTitle"] %>"</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(true) %>

        <fieldset>
            <legend>Fields</legend>
                        
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
                <%: Html.TextBoxFor(model => model.BeginDate) %>
                <%: Html.ValidationMessageFor(model => model.BeginDate) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.EndDate) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.EndDate) %>
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
                <%: Html.DropDownList("userId", (IEnumerable<SelectListItem>)ViewData["Users"]) %>
                <%: Html.ValidationMessageFor(model => model.Responsible) %>
            </div>
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Cancel", "List", "Projects") %>
    </div>

</asp:Content>

