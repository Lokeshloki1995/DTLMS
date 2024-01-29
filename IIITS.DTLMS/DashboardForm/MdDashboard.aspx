<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MdDashboard.aspx.cs" Inherits="IIITS.DTLMS.DashboardForm.MdDashboard" MasterPageFile="~/DTLMS.Master" %>



<%--<%@ Register Src="/graphusercontrols/FailureDetails.ascx" TagName="FailureDetails" TagPrefix="uc1" %>--%>

<%@ Register Src="/graphusercontrols/FailureStagesGraph.ascx" TagName="FailureDetails" TagPrefix="uc1" %>
<%@ Register Src="/graphusercontrols/DTrConditionsStore.ascx" TagName="TcStatus" TagPrefix="uc3" %>
<%@ Register Src="/graphusercontrols/DTrConditionsField.ascx" TagName="DTrConditionsField" TagPrefix="uc4" %>
<%@ Register Src="/graphusercontrols/RepairerStatus.ascx" TagName="RepairerPerformance" TagPrefix="uc5" %>

<%@ Register Src="/graphusercontrols/PendingReplaceDTc.ascx" TagName="PendingReplacement" TagPrefix="uc6" %>

<%@ Register Src="/graphusercontrols/RIstatus.ascx" TagName="RIstatus" TagPrefix="uc7" %>

<%--<%@ Register Src="/graphusercontrols/WebUserControl1.ascx" TagName="WebUserControl1" TagPrefix="uc8" %>--%>

<%@ Register Src="/graphusercontrols/FailedDTC.ascx" TagName="Frequentlyfailed" TagPrefix="uc9" %>

<%@ Register Src="/graphusercontrols/FailedDTr.ascx" TagName="Frequentlyfaileddtr" TagPrefix="uc10" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

  <div id="FailureDetail" runat="server">
    <uc1:FailureDetails ID="FailureDetails" runat="server" />
  </div>
    
    <div id="RepairerPerformanc" runat="server">
    <uc5:RepairerPerformance ID="RepairerPerformance" runat="server" />
        </div>
    <div id="PendingReplacemen" runat="server" >
    <uc6:PendingReplacement ID="PendingReplacement" runat="server" />
        </div>

    <div id="TcStatu" runat="server">
    <uc3:TcStatus ID="TcStatus" runat="server" />
        </div>

       <div id="DTrConditionsF" runat="server">
    <uc4:DTrConditionsField ID="DTrConditionsFiel" runat="server" />
        </div>

      <div id="RIstatuses" runat="server">
    <uc7:RIstatus ID="RIstatus" runat="server" />
        </div>
    <div id="Div1" runat="server">
    <uc9:Frequentlyfailed ID="Frequentlyfailed" runat="server" />
        </div>
    <div id="Div2" runat="server">
    <uc10:Frequentlyfaileddtr ID="Frequentlyfaileddtr" runat="server" />
        </div>


</asp:Content>



