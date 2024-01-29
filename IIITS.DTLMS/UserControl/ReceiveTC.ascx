<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="ReceiveTC.ascx.cs" Inherits="IIITS.DTLMS.UserControl.ReceiveTC" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReceiveTC.ascx.cs" Inherits="IIITS.DTLMS.UserControl.ReceiveTC" %>

 <div class="metro-nav-block nav-block-blue" runat="server">
                     <asp:LinkButton ID="LinkButton" runat="server" onclick="Tcfailed_Click" > <i class="icon-inbox"></i><div class="info"><asp:Label ID="LabelTcfailed" runat="server" ></asp:Label></div><div class="status">Repaired and Good dtr</div></asp:LinkButton>
                    </div>