<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PendingReplaceDTc.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.PendingReplaceDTc" %>
<script src='<%= ResolveUrl("~/js/jquery-OneEightThree.min.js") %>'></script>

<!-- Latest compiled and minified CSS -->
<script src="https://code.highcharts.com/highcharts.js"></script>
<script src="https://code.highcharts.com/highcharts-3d.js"></script>

    <div style="width: 46.717949%!important"class="span6">
        <div class="widget">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>Pending Repairer<a id="viewstatus" runat="server" onserverclick="view_link_click" >
                              <span style="color: #fff">(View Details..)</span></a></h4>

            </div>
    <div style="margin-left:14px;margin-top:14px" id="divdropdwn" class="control-group"  runat="server">
                <label class="control-label">Store</label>
                <div class="controls">
                    <div class="input-append">
                        <asp:DropDownList ID="cmbDivisions" runat="server" AutoPostBack="false"
onChange="javascript:ddlClick()">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div id="PendingReplacee" style="height: 400px"></div>
        </div>
    </div>

<script>
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "MdDashboard.aspx/ReplaceDtcStatus",
            contentType: "application/json; charset=utf-8",
            data: '{ "strOfficeCode":""}',
            dataType: "json",
            success: function (Result) {
                debugger;
                var serrrr = eval(Result.d[0]);
                GetReplaceDtcChart(serrrr);
            },
            error: function (Result) {
                alert("Error");
            }
        });
    });
    function ddlClick() {
        debugger;
       var officeCode= $('#<%= cmbDivisions.ClientID %>').val();
        $.ajax({
            type: "POST",
            url: "MdDashboard.aspx/ReplaceDtcStatus",
            contentType: "application/json; charset=utf-8",
            data: "{ 'strOfficeCode':'"+officeCode+"'}",
            dataType: "json",
            success: function (Result) {
                debugger;
                var serrrr = eval(Result.d[0]);
                GetReplaceDtcChart(serrrr);
            },
            error: function (Result) {
                alert("Error");
            }
        });
    }
    function GetReplaceDtcChart(serrrr) {
        debugger;
        Highcharts.chart('PendingReplacee', {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 45
                }
            },
            title: {
                style: {
                    color: '#1b4475',
                    font: 'bold 20px "Trebuchet MS", Verdana, sans-serif'
                },
                text: 'Pending with Repairer',
                
            },
           
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    innerSize: 100,
                    depth: 45,
                    
                    showInLegend: true
                },
                
            },
           
            series: [{
                name: 'Repairer Performance',
                data: serrrr
            }],
            

        });
    }
  
</script>


