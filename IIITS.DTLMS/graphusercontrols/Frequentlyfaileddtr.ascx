<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Frequentlyfaileddtr.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.Frequentlyfaileddtr" %>

<style>
    div#ContentPlaceHolder1_PendingReplacement_divdropdwn {
                margin-left:16px;
    }
</style>


<%--<script type="text/javascript">
    function showTooltip(value1, value2, ex) {
        var tooltip = document.getElementById("myToolTip");
        tooltip.style.visibility = "visible";
        var posx = 0;
        var posy = 0;
        if (!e) var e = (window.event) ? event : ex;
        if (e.pageX || e.pageY) {
            posx = e.pageX;
            posy = e.pageY;
            tooltip.style.left = posx + "px";
            tooltip.style.top = (posy - 100) + "px";
        }
        else if (e.clientX || e.clientY) {
            if (e.cancelBubble != null) e.cancelBubble = true;
            //for IE8 and earlier versions event bubbling occurs...
            posx = e.clientX + document.body.scrollLeft
                   + document.documentElement.scrollLeft;
            posy = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;
            tooltip.style.left = posx + "px";
            tooltip.style.top = (posy - 100) + "px";
        }
        document.getElementById("<%=lbl.ClientID%>").innerHTML =
          "X and Y Values : " + "(" + value1 + "," + value2 + ")";
    }
    function hide() {
        var tooltip = document.getElementById("myToolTip");
        tooltip.style.visibility = "hidden";
    }
</script> --%>
<div style="width:46%"class="span5" id="pr">

    <div class="span12">
         
        <!-- BEGIN SAMPLE FORMPORTLET-->
         <div class="widget">
       
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>FrequentlyfailedDTR Status</h4>
                <span class="tools">
                    <a href="javascript:;" class="icon-chevron-down"></a>
                    <a href="javascript:;" class="icon-remove"></a>
                </span>
            </div>
            <div class="widget-body">

                <ul class="breadcrumb" style="font-weight: bold">
                    <li>
                        <a  id="viewstatus" onclick="view_link_click" runat="server">Current Status 
                              <span style="color: Blue">(View Details..)</span></a>
                    </li>
                </ul>
            </div>

            <div id="divdropdwn" class="control-group" visible="false" runat="server">
                <label class="control-label">Select strore</label>
                <div class="controls">
                    <div class="input-append">
                        <asp:DropDownList ID="cmbDivisions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbDivisions_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <asp:Chart ID="Frequentlyfaileddtrchart" runat="server" Height="300" Width="800">
                <Titles>
                    <asp:Title ShadowOffset="10" Name="Items" />
                </Titles>
                <Series>

                    <%--       <asp:Series ChartType="StackedColumn">
                                        </asp:Series>
                                        <asp:Series ChartType="StackedColumn">
                                        </asp:Series> --%>
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

 <%--<div id="myToolTip" style="position: absolute; visibility: hidden; width:10px; height:10px; margin: 9px 0;
            padding: 18px 20px;
            background-color: white; /* easy rounded corners for modern browsers */
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #c5d9e8;
            padding: 17px 19px;">
<div style="position:absolute;float:left;">
<asp:Image ID="Image1" runat="server" ImageUrl="~/exclamation-mark.jpg" />
</div>
<div style="position:absolute;left:70px;top:30px;">
 <b><asp:Label ID="lbl" runat="server"></asp:Label></b>
</div>
</div>--%>

        </div>

       
</div>
    </div>