<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<NProject.ViewModels.Account.SimpleRegistration>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	NProject | Registration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


<% using (Html.BeginForm("SimpleRegistrationFinish", "Account"))
   {%>
   	<%:Html.ValidationSummary()%>
    <% Html.EnableClientValidation(); %>
	<%:Html.AntiForgeryToken()%>
	<%:Html.LabelFor(m => m.Email)%>:  <%:Html.EditorFor(m => m.Email)%> <%:Html.ValidationMessageFor(m=>m.Email) %><br />
	<%:Html.LabelFor(m => m.Name)%>:  <%:Html.EditorFor(m => m.Name)%> <%:Html.ValidationMessageFor(m=>m.Name) %><br />
	<%:Html.LabelFor(m => m.Password)%>:  <%:Html.EditorFor(m => m.Password)%> <%:Html.ValidationMessageFor(m=>m.Password) %><br />
	<%:Html.HiddenFor(m => m.TimeShiftFromUtc)%>
	<input type="submit" value="Register!" />
<% }%>
</asp:Content>
