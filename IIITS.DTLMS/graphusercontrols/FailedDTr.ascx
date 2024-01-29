<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FailedDTr.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.FailedDTr" %>
<script src='<%= ResolveUrl("~/js/jquery-OneEightThree.min.js") %>'></script>
<!-- Latest compiled and minified CSS -->
<script src="https://code.highcharts.com/highcharts.js"></script>
<script src="https://code.highcharts.com/highcharts-3d.js"></script>


    <div style="width: 46.717949%!important"class="span6">
        <div class="widget">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>Frequently Failed DTr<a id="viewstatus" runat="server" onserverclick="view_link_click">
                              <span style="color: #fff">(View Details..)</span></a></h4>

            </div>
            <div id="FailedDTr" style="height: 400px"></div>
        </div>
    </div>
<script>
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "MdDashboard.aspx/FrequentlyFailedDtr",
            contentType: "application/json; charset=utf-8",
            data: "{}",
            dataType: "json",
            success: function (Resultsss) {
                debugger;
                FailedDTrVal(eval(Resultsss.d[0]), eval(Resultsss.d[1]));
            },
            error: function (Result) {
                alert("Error");
            }
        });
    });
    function FailedDTrVal(xCategorys,yCategorys) {
        Highcharts.chart('FailedDTr', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'Failed DTr Capacity'
            },
            xAxis: {
                categories: xCategorys,
                title: {
                    text: 'Off Name'
                }
            },

            yAxis: {
                min: 0,
                title: {
                    text: 'DTr Count'
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                }
            },
            legend: {
                align: 'right',
                x: -30,
                verticalAlign: 'top',
                y: 25,
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                borderColor: '#CCC',
                borderWidth: 1,
                shadow: false
            },
            tooltip: {
                headerFormat: '<b>{point.x}</b><br/>',
                pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                    }
                }
            },
            series: yCategorys
        });
    }

</script>