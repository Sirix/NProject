<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.Models.Domain.User>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Change user information</h2>

    <% using (Html.BeginForm()) {%>
    <%: Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>
            <legend>Fields</legend>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Username) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Username, new {disabled=true}) %>
                <%: Html.ValidationMessageFor(model => model.Username) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Hash) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Hash) %>
                <%: Html.ValidationMessageFor(model => model.Hash) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Email) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Email) %>
                <%: Html.ValidationMessageFor(model => model.Email) %>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.Role) %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownList("RoleName", (List<SelectListItem>)ViewData["Roles"])%>
                <%: Html.ValidationMessageFor(model => model.Role) %>
            </div>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "List") %>
    </div>

</asp:Content>

