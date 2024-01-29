<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="QCApproval.aspx.cs" Inherits="IIITS.DTLMS.Internal.QCApproval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
    function CheckAllEmp(Checkbox) {
        var GridVwHeaderChckbox = document.getElementById("<%=grdEnumerationDetails.ClientID %>");
        for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
            GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
        }
    }

    function UnCheckAllEmp(Checkbox) {
        var GridVwHeaderChckbox = document.getElementById("<%=grdEnumerationDetails.ClientID %>");

        GridVwHeaderChckbox.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked = false;
        
    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>      
         <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                     Enumeration View For QC
                   </h3>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i>ddd </button>
                               </div>
                           </form>
                       </li>
                   </ul>
                   <!-- END PAGE TITLE & BREADCRUMB-->
               </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4><i class="icon-reorder"></i>  Enumeration View For QC </h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">                               
                            <div class="form-horizontal" align="center"> 
                                <div class="span2" style="float:left">                                   
                                    <asp:RadioButtonList ID="rdbPendingQC" runat="server" CssClass="radio">
                                        <asp:ListItem Text="Pending QC" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="QC Done" Value="1"></asp:ListItem>    
                                         <%--<asp:ListItem Text="Pending Clarification" Value="2"></asp:ListItem>--%> 
                                        <asp:ListItem Text="Reject" Value="3"></asp:ListItem>                                       
                                    </asp:RadioButtonList> 
                                </div>

                               <div class="span2" style="float:left">
                                    <asp:RadioButtonList ID="rdbLocationType" runat="server" CssClass="radio">
                                        <asp:ListItem Text="Field" Value="2"></asp:ListItem>          
                                     <%--   <asp:ListItem Text="Store/Bank" Value="1,5"></asp:ListItem>--%>
                                           <asp:ListItem Text="Store" Value="1"></asp:ListItem> 
                                          <asp:ListItem Text="Bank" Value="5"></asp:ListItem>                                                                    
                                        <asp:ListItem Text="Repairer" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="All" Value="4"></asp:ListItem>
                                    </asp:RadioButtonList>     
                               </div>

<%--                               <div class="span2" style="float:left">                                    
                                       <asp:Label ID="lblStatus" runat="server" Text="Operator" Font-Bold="False" 
                                        Font-Size="Medium"></asp:Label>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbOperator" runat="server"  TabIndex="9" Width="150px">                                   
                                                </asp:DropDownList>
                                        <div class="space10"></div>        
                                       <asp:Label ID="Label1" runat="server" Text="Supervisor" Font-Bold="False" 
                                        Font-Size="Medium"></asp:Label>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbSupervisor" runat="server"  TabIndex="9" Width="150px">                                   
                                                </asp:DropDownList>                                                                              
                               </div>--%>

                               <div class="span3" style="float:left">                                    
                                       <asp:Label ID="Label2" runat="server" Text="Division" Font-Bold="False" 
                                        Font-Size="Medium"></asp:Label>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbDivision" runat="server"  Width="150px" TabIndex="9" AutoPostBack= "true"
                                           onselectedindexchanged="cmbDivision_SelectedIndexChanged">                                   
                                                </asp:DropDownList>
                                        <div class="space10"></div>        
                                       <asp:Label ID="Label3" runat="server" Text="SubDivision" Font-Bold="False" 
                                        Font-Size="Medium"></asp:Label>
                                         &nbsp;
                                        <asp:DropDownList ID="cmbSubdivision" runat="server"  Width="150px"  AutoPostBack= "true"
                                           TabIndex="9" onselectedindexchanged="cmbSubdivision_SelectedIndexChanged">                                   
                                                </asp:DropDownList> 
                                        <div class="space10"></div>        
                                       <asp:Label ID="Label4" runat="server" Text="Section" Font-Bold="False"  
                                        Font-Size="Medium"></asp:Label>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbSection" runat="server"  Width="150px" TabIndex="9" >                                   
                                                </asp:DropDownList>                                                                             
                               </div>

                               <div class="span3" style="float:left">   
                                    <asp:Label ID="Label5" runat="server" Text="Feeder" Font-Bold="False" 
                                        Font-Size="Medium"></asp:Label>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbFeeder" runat="server"  Width="150px" TabIndex="9">                                   
                                                </asp:DropDownList>
                                        <div class="space10"></div>  
                                    <asp:Label ID="Label6" runat="server" Text="Store" Font-Bold="False" 
                                        Font-Size="Medium"></asp:Label>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="cmbStore" runat="server"  Width="150px" TabIndex="9">                                   
                                                </asp:DropDownList> 
                                        <div class="space10"></div>        
                                    <asp:Label ID="Label7" runat="server" Text="Repairer" Font-Bold="False" 
                                    Font-Size="Medium"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:DropDownList ID="cmbRepairer" runat="server"  Width="150px" TabIndex="9">                                   
                                            </asp:DropDownList>  

                               </div>
                               <div class="space20"></div>
                               <div class="span10" >
                                   <asp:Button ID="cmdLoad" CssClass="btn btn-primary"  runat="server" Text="Load" 
                                       onclick="cmdLoad_Click" />
                               </div>
                               
                            </div>      
                            <div class="space10"> </div>                                
                                <!-- END FORM-->                          
                        
                            <asp:GridView ID="grdEnumerationDetails" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"
                                AutoGenerateColumns="false"  PageSize="25" ShowFooter="true"
                                CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                runat="server" onrowcommand="grdEnumerationDetails_RowCommand" 
                                onpageindexchanging="grdEnumerationDetails_PageIndexChanging" >
                                <Columns>
                                    
                                    <asp:TemplateField AccessibleHeaderText="ID" HeaderText="ID"  Visible="false">                                
                                        <ItemTemplate>                                      
                                            <asp:Label ID="lblEnumDetailsId" runat="server" Text='<%# Bind("ED_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                  </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

<%--                                   <asp:TemplateField AccessibleHeaderText="TYPE" HeaderText="Select" >    
                                       <HeaderTemplate>
                                             <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAllEmp(this);"  />
                                       </HeaderTemplate>                            
                                        <ItemTemplate>                                      
                                            <asp:CheckBox ID="chkSelect" runat="server" onclick="UnCheckAllEmp(this);"/>
                                        </ItemTemplate>
                                  </asp:TemplateField>--%>

                                  <asp:TemplateField AccessibleHeaderText="TYPE" HeaderText="Enum Type"  Visible="true">                                
                                        <ItemTemplate>                                      
                                            <asp:Label ID="lblPlatenumber" runat="server" Text='<%# Bind("TYPE") %>' style="word-break:break-all" width="100px" ></asp:Label>
                                        </ItemTemplate>
                                  </asp:TemplateField>
                                  
                                  <asp:TemplateField AccessibleHeaderText="DTC CODE" HeaderText="DTC Code" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDTCCode" runat="server" Text='<%# Bind("DTE_DTCCODE") %>' style="word-break:break-all" width="130px" ></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                          <asp:TextBox ID="txtDTCcode" runat="server" placeholder="Enter DTC Code " Width="130px" MaxLength="10" ></asp:TextBox>
                                        </FooterTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="DTC NAME" HeaderText="DTC Name" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblDTCName" runat="server" Text='<%# Bind("DTE_NAME") %>' style="word-break:break-all" width="150px" ></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                          <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name " Width="150px" MaxLength="25" ></asp:TextBox>
                                        </FooterTemplate>
                                  </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="TC Code" HeaderText="DTr Code" >                                
                                    <ItemTemplate>                                       
                                        <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("DTE_TC_CODE") %>' style="word-break:break-all" width="100px"> </asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <%--  <asp:TextBox ID="txtTCcode" runat="server" placeholder="Enter DTr Code " Width="100px" MaxLength="10"  onkeypress="javascript:return OnlyNumber(event);"></asp:TextBox>--%>
                                           <asp:TextBox ID="txtTCcode" runat="server" placeholder="Enter DTr Code " Width="100px" MaxLength="10"></asp:TextBox>
                                      </FooterTemplate>
                                  </asp:TemplateField>

<%--                                     <asp:TemplateField AccessibleHeaderText="OPERATOR1" HeaderText="Operator 1" Visible="true">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="Operator1" runat="server" Text='<%# Bind("OPERATOR1") %>' style="word-break:break-all" width="120px"></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                          <asp:TextBox ID="txtOperator1" runat="server" placeholder="Enter Operator1 " Width="120px" MaxLength="25" ></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateField>--%>

                                     <asp:TemplateField AccessibleHeaderText="TCSLNO" HeaderText="Serial NO" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblTCSLNO" runat="server" Text='<%# Bind("DTE_TC_SLNO") %>' style="word-break:break-all" width="120px" ></asp:Label>
                                        </ItemTemplate>
                                         <FooterTemplate>
                                           <asp:TextBox ID="txtSlNo" runat="server" placeholder="Enter SRL No " Width="120px" MaxLength="25" ></asp:TextBox>
                                       </FooterTemplate>
                                    </asp:TemplateField>  

                                      <asp:TemplateField AccessibleHeaderText="MAKE" HeaderText="Make" >                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblmake" runat="server" Text='<%# Bind("MAKE") %>' style="word-break:break-all" width="120px" ></asp:Label>
                                        </ItemTemplate>
                                          <FooterTemplate>
                                          <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                                        </FooterTemplate>
                                    </asp:TemplateField>                              
                                   
                                    <asp:TemplateField HeaderText="QC">
                                    <ItemTemplate>
                                        <center>
                                            <asp:ImageButton  CommandName="Submit" ID="imgBtnEdit" runat="server" Height="15px" ImageUrl="~/img/Manual/Approve.png" ToolTip="Click for QC"
                                                Width="15px" />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                  <asp:TemplateField AccessibleHeaderText="ED_ENUM_TYPE" HeaderText="ED_LOCTYPE" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblEnumType" runat="server" Text='<%# Bind("ED_LOCTYPE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                       <asp:TemplateField AccessibleHeaderText="ED_ENUM_TYPE" HeaderText="ED_STATUS_FLAG" Visible="false">                                
                                        <ItemTemplate>                                       
                                            <asp:Label ID="lblStatusFlag" runat="server" Text='<%# Bind("ED_STATUS_FLAG") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                
                                </Columns>

                            </asp:GridView>
                        
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                  <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                  <div class="span3"></div>
                  <div class="span3">
                    <%--<asp:Button ID="cmdApprove" CssClass="btn btn-primary"  runat="server" 
                        Text="Approve" onclick="cmdApprove_Click" Visible="false" />--%>
                </div>
                </div>
            </div>         

            <!-- END PAGE CONTENT-->
      </div>
 </div>
</asp:Content>
