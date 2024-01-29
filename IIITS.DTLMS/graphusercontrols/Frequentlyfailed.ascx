<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Frequentlyfailed.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.Frequentlyfailed" %>


        <div style="width:48.0%" class="span6" id="rp">

   
        <!-- BEGIN SAMPLE FORMPORTLET-->
        <div class="widget blue">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>Frequentlyfailed DTC</h4>
                <span class="tools">
                    <a href="javascript:;" class="icon-chevron-down"></a>
                    <a href="javascript:;" class="icon-remove"></a>
                </span>
            </div>
            <div class="widget-body">

                <ul class="breadcrumb" style="font-weight: bold">
                    <li>

                        <a id="viewstatus" onclick="view_link_click" runat="server">Current Status 
                              <span style="color: Blue">(View Details..)</span></a>
                    </li>
                </ul>
            </div>

            <div id="divdropdwn" class="control-group" visible="false" runat="server">
                <label class="control-label">Select Division</label>
                <div class="controls">
                    <div class="input-append">
                        <asp:DropDownList ID="cmbDivisions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbDivisions_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <asp:Chart ID="Frequentlyfailedchart" runat="server" Height="300" Width="900">
                <Titles>
                    <asp:Title ShadowOffset="10" Name="Items" />
                </Titles>
               <Series>
                     <asp:Series Name="" />
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

<style>
    /*img#ContentPlaceHolder1_Frequentlyfailed_Frequentlyfailedchart:hover {
        transition: transform .8s;
        transform: scale(1.1);
    }*/
</style>