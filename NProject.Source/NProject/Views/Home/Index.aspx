<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    NProject
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <h1>Hello to NProject</h1>
   Start use NProject right now!
   <% using (Html.BeginForm("SimpleRegistration", "Account"))
      {%>
    <%:Html.AntiForgeryToken()%>
    E-mail: <%:Html.TextBox("Email")%>
    <input type="submit" value="Register!" />
   <% }%>
</asp:Content>
