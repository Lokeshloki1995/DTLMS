<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TutorialVideos.aspx.cs"   MasterPageFile="~/DTLMS.Master" Inherits="IIITS.DTLMS.DashboardForm.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function openTab(th) {
            window.open(th.name, '_blank');
        }
        </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hdfvideopath" runat="server" />
    <style>
fieldset.scheduler-border {
    border: 1px groove #ddd !important;
    padding: 0 1.4em 1.4em 1.4em !important;
    margin: 0 0 1.5em 0 !important;
    -webkit-box-shadow:  0px 0px 0px 0px #000;
            box-shadow:  0px 0px 0px 0px #000;
}

    legend.scheduler-border {
        font-size: 1.2em !important;
        font-weight: bold !important;
        text-align: left !important;
        width:auto;
        padding:0 10px;
        border-bottom:none;
    }
</style>
    <br /><br />
         <div class="container-fluid">
 <fieldset class="scheduler-border">
    <legend class="scheduler-border">Video Tutorials</legend>
  <!-- Trigger the modal with a button -->
  <a href="#"  data-toggle="modal" data-target="#myModal">Video Tutorial-1 Field Enumeration</a><br />
     <br />
   <%--  <a href="#" data-toggle="modal" data-target="#myModal1">Video Tutorial-2(DTC New Commissioning)</a>--%>


 
 <%-- <asp:LinkButton runat="server" ID="lnkbtnVDO1" text="video_1" OnClick="vdo_click1"></asp:LinkButton>--%>
  <!-- Modal -->
  <div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
       
          <h4 class="modal-title"style="color:#000;font-weight:bold"><center>Field Enumeration Process</center></h4>
               <button type="button" class="close" data-dismiss="modal">&times;</button>
        </div>
        <div class="modal-body">
       
<video id="video1" width="520" visible="false" height="340" controls  > 
  <source ID="source" src="https://bescomdtlms.com/HESCOMDTLMS/VIDEO_TUTORIAL/Presentation1_Hescom_DTLMS.mp4" runat="server" type="video/mp4"> 
 
    
</video> 

        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
        </div>
      </div>
    </div>
  </div>


   <%--   <div class="modal fade" id="myModal1" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
         
          <h4 class="modal-title"style="color:#000;font-weight:bold"><center>DTC New Commissioning</center></h4>
             <button type="button" class="close" data-dismiss="modal">&times;</button>
        </div>
        <div class="modal-body">
       
<video id="video2" width="520" height="340" controls > 
  <source ID="source1" src="https://bescomdtlms.com/VideoTutorial/VideosTutorial/video-2.mp4" runat="server" type="video/mp4"> 
 
    
</video> 

  
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
        </div>
      </div>
    </div>
  </div>--%>
  </fieldset>
</div>

    

      

       <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
 
</asp:Content>
