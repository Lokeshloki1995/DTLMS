<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pograph.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.Pograph" %>


<!-- BEGIN SAMPLE FORMPORTLET-->
<div class="widget">
    <div class="widget-title">
        <h4><i class="icon-reorder"></i>Po graph</h4>

    </div>
    <div class="widget-body">

        <ul class="breadcrumb" style="font-weight: bold">
            <li>

                <a id="viewstatus" onclick="view_link_click" runat="server">Current Status 
                              <span style="color: Blue">(View Details..)</span></a>
            </li>
        </ul>
    </div>



    <div class="form-horizontal">
        <div class="row-fluid">
            <div>

                

                <asp:Chart ID="pograph" runat="server">
                                    <Legends>
                                        <asp:Legend Name="Legend1">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title Name="NewTitle">
                                        </asp:Title>
                                    </Titles>
                                    <Series>
                                        <asp:Series ChartType="Column">
                                        </asp:Series>
                                        <asp:Series ChartType="Column">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1">
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
            </div>

            <div class="span3">
            </div>
        </div>

    </div>



    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>



</div>
