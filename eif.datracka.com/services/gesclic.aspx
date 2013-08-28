<%@ Page Language="VB" AutoEventWireup="false" CodeFile="gesclic.aspx.vb" Inherits="gestionclick" %>
<% 
    System.Web.HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache")
System.Web.HttpContext.Current.Response.Expires = 0
    System.Web.HttpContext.Current.Response.Cache.SetNoStore()
System.Web.HttpContext.Current.Response.AddHeader( "Pragma", "no-cache")%>
