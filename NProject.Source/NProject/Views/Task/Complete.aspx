<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.Models.Domain.Task>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Complete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Complete</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>
            <legend>Fields</legend>
                                  
            <div class="editor-label">
                <%: Html.LabelFor(model => model.SpentTime) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.SpentTime) %>
                <%: Html.ValidationMessageFor(model => model.SpentTime) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Status) %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownList("statusId", (IEnumerable<SelectListItem>)ViewData["Statuses"]) %>
                <%: Html.ValidationMessageFor(model => model.Status) %>
            </div>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

