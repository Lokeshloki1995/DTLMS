<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Expenditure.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.Expenditure" %>


        <!-- BEGIN SAMPLE FORMPORTLET-->
        <div class="widget">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>Expenditure graph</h4>
                
            </div>
            <div class="widget-body">

                <ul class="breadcrumb" style="font-weight: bold">
                    <li>

                        <a id="viewstatus" onclick="view_link_click" runat="server">Current Status 
                              <span style="color: Blue">(View Details..)</span></a>
                    </li>
                </ul>
            </div>

           <div id="divdropdwn" class="control-group container-fluid"  runat="server">
                <label class="control-label">Select Division</label>
                <div class="controls">
                    <div class="input-append">
                        <asp:DropDownList ID="cmbDivisions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbDivisions_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

           <div class="form-horizontal">
                        <div class="row-fluid">
                            <div>

                                <asp:Chart ID="expenditure" runat="server">
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
