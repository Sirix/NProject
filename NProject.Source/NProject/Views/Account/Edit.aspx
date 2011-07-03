<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.Models.Domain.User>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Manage role <%: ViewData["Username"] %></h2>

    <% using (Html.BeginForm()) {%>
    <%: Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>
            <legend>Set role</legend>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Role) %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownList("roleId", (List<SelectListItem>)ViewData["Roles"])%>
                <%: Html.ValidationMessageFor(model => model.Role) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.UserState) %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownList("userStateId", (List<SelectListItem>)ViewData["UserStates"])%>
                <%: Html.ValidationMessageFor(model => model.UserState) %>
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

