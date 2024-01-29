<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TcStatus.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.TcStatus" %>


<div class="row-fluid">

    <div class="span12">
        <!-- BEGIN SAMPLE FORMPORTLET-->
      
            <div id="TcStatusfields" runat="server" class="span6">

                  <div class="widget">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>DTR Condition Status</h4>
              
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
            <asp:Chart ID="TcStatusfield" CanResize="true" CssClass="table  table-bordered table-condensed table-responsive" runat="server" Height="300" Width="600">
                <Titles>
                    <asp:Title ShadowOffset="20" Name="Items" />
                </Titles>
                <Series>
                </Series>
                <Legends>
                    <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Legend1"
                        LegendStyle="Row" />
                    <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Legend2"
                        LegendStyle="Row" />
                </Legends>

                  <Titles>
                                        <asp:Title Name="NewTitle">
                                        </asp:Title>
                                    </Titles>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
                </ChartAreas>
            </asp:Chart>
            </div>
                 </div>
            <div id="TcStatusStores" runat="server" class="span6">

                 <div class="widget">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>DTR Condition Status</h4>
              
            </div>
            <div class="widget-body">

                <ul class="breadcrumb" style="font-weight: bold">
                    <li>

                        <a id="viewstatus1" onclick="view_link_click" runat="server">Current Status 
                              <span style="color: Blue">(View Details..)</span></a>
                    </li>
                </ul>
            </div>

            <div id="divdropdwn1" class="control-group" visible="false" runat="server">
                <label class="control-label">Select Division</label>
                <div class="controls">
                    <div class="input-append">
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbDivisions_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <asp:Chart ID="TcStatusStore" CanResize="true" CssClass="table  table-bordered table-condensed table-responsive" runat="server" Height="300" Width="600">
                <Titles>
                    <asp:Title ShadowOffset="20" Name="Items" />
                </Titles>
                <Series>
                </Series>
                <Legends>
                    <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Legend1"
                        LegendStyle="Row" />
                    <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Legend2"
                        LegendStyle="Row" />
                </Legends>
                 <Titles>
                                        <asp:Title Name="NewTitle">
                                        </asp:Title>
                                    </Titles>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
                </ChartAreas>
            </asp:Chart>
                </div>



            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>


        </div>
   
</div>
    </div>
