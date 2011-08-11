<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.Models.ViewModels.TaskFormViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit task
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit "<%:Model.Task.Title %>"</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.HiddenFor(model => model.ProjectId) %>
        <%: Html.HiddenFor(model => model.Task.Id) %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary() %>

        <fieldset>
            <legend>Fields</legend>
                 
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Task.Title) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Task.Title)%>
                <%: Html.ValidationMessageFor(model => model.Task.Title)%>
            </div>       
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Task.Description)%>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Task.Description)%>
                <%: Html.ValidationMessageFor(model => model.Task.Description)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Task.BeginDate)%>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Task.BeginDate)%>
                <%: Html.ValidationMessageFor(model => model.Task.BeginDate)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Task.EndDate)%>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Task.EndDate)%>
                <%: Html.ValidationMessageFor(model => model.Task.EndDate)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Task.Status) %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.StatusId, Model.Statuses) %>
                <%: Html.ValidationMessageFor(model => model.StatusId) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Task.Responsible)%>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.ResponsibleUserId, Model.Programmers)%>
                <%: Html.ValidationMessageFor(model => model.ResponsibleUserId) %>
            </div>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "Tasks", "Projects", new { id = Model.ProjectId }, new object())%>
    </div>

</asp:Content>

