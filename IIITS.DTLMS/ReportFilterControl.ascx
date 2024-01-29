<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportFilterControl.ascx.cs" Inherits="IIITS.DTLMS.ReportFilterControl" %>

 <div class="widget-body form">
                                <!-- BEGIN FORM-->
                                <div class="form-horizontal">
                                    <div class="row-fluid">
                                    <div class="span1"></div>
                                        <div class="span5">
                                       <%-- <div class="control-group">
                                                <label class="control-label">Corp Office<span class="Mandotary"> *</span></label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCorpOffice" AutoPostBack="true" runat="server" 
                                                            onselectedindexchanged="cmbCorpOffice_SelectedIndexChanged" >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>--%>
                                             <div class="control-group">
                                                <label class="control-label">Zone</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbZone" AutoPostBack="true" runat="server" 
                                                            onselectedindexchanged="cmbZone_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        
                                        <div class="control-group">
                                                <label class="control-label">Circle</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbCircle" AutoPostBack="true" runat="server" 
                                                            onselectedindexchanged="cmbCircle_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        
                                         <div class="control-group">
                                                <label class="control-label">Division</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbDivision" AutoPostBack="true" runat="server" 
                                                            onselectedindexchanged="cmbDivision_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                       

                                        </div>
                                        <div class="span5">
                                        
                                                  <div class="control-group">
                                                <label class="control-label">SubDiv Code</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                        <asp:DropDownList ID="cmbSubDiv" AutoPostBack="true" runat="server" 
                                                            onselectedindexchanged="cmbSubDiv_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>                                   
                                            <div class="control-group">
                                                <label class="control-label">O&M Unit</label>
                                                <div class="controls">
                                                    <div class="input-append">
                                                         <asp:DropDownList ID="cmbAccUnit" AutoPostBack="true" runat="server" 
                                                            onselectedindexchanged="cmbAccUnit_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                                                            

                                            </div>
  <%-- <div class="span1"></div>--%>
                                        </div>
                                       
                                        
                                    
                                    </div>
                                </div>