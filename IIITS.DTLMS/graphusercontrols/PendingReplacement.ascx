<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PendingReplacement.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.PendingReplacement" %>
<style>
    div#ContentPlaceHolder1_PendingReplacement_divdropdwn {
                margin-left:16px;
    }
</style>
<div style="width:46%"class="span5" id="pr">
   
     <div class="span12">
    
        <!-- BEGIN SAMPLE FORMPORTLET-->
        <div class="widget">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>Pending Replacement</h4>
                
            </div>
            <div class="widget-body">

                <ul class="breadcrumb" style="font-weight: bold">
                    <li>

                        <a id="viewstatus" onclick="view_link_click" runat="server">Current Status 
                              <span style="color: Blue">(View Details..)</span></a>
                    </li>
                </ul>
            </div>

         <div id="divdropdwn" class="control-group" runat="server">
                <label class="control-label">Select Store</label>
                <div class="controls">
                    <div class="input-append">
                        <asp:DropDownList ID="cmbDivisions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbDivisions_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <asp:Chart ID="PendingReplacements" runat="server" Height="300" Width="600">
                <Titles>
                    <asp:Title ShadowOffset="10" Name="Items" />
                </Titles>
                <Series>
                    <asp:Series Name="1 Day" />
                </Series>
                <Legends>
                    <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Legend1"
                        LegendStyle="Row" />
                    <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Legend2"
                        LegendStyle="Row" />
                </Legends>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
                </ChartAreas>
            </asp:Chart>


            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>


        </div>
   
         </div>
        </div>
    
   
<%--<style>
    img#ContentPlaceHolder1_PendingReplacement_PendingReplacements:hover {
        transition: transform .8s;
        transform: scale(1.1);
    }
</style>--%>

