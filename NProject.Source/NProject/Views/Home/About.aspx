<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    About Us
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>About NProject</h2>
    <p>
     NProject - is a light-weight, web-based project tracking system.
     With it you have no need to create pieces of paper with individual tasks to each other teammates.
    </p>
    <p>
         It is very simple in use and installation. 
        Just unpack it on your server and that's all!<br />
        <b><i>*You need rights to create database on the server</i></b>
     </p>
     <br /><br />
     <h3>Special thanks for our team, which worked at version 1.0</h3>
     <ul>
        <li>Ivan Manzhos</li>
        <li>Sergey Smirnov</li>
        <li>Vlad Zaycev</li>
        <li>Lera Pedchenko</li>
        <li>Alex Klunnyi</li>
     </ul>
</asp:Content>
