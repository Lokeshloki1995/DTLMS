<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DTrConditionsStore.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.DTrConditionsStore" %>


<div style="width:-webkit-fill-available!important;    margin-left: 32px;" class="span12">
        <div class="widget">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>DTr Conditions Store Wise<a  runat="server" id="viewstatus" onserverclick="view_link_click" >
                              <span style="color: #fff">(View Details..)</span></a></h4>

            </div>

            <div <%--style="min-width: 310px; height: 400px; margin: 0 auto"--%>id="DTrConditionStatus"></div>
        </div>
    </div>
<script src='<%= ResolveUrl("~/js/jquery-OneEightThree.min.js") %>'></script>
<script src="https://code.highcharts.com/highcharts.js"></script>
<script>
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "MdDashboard.aspx/DTrConditionsStore",
            contentType: "application/json; charset=utf-8",
            data : "{}",
            dataType: "json",
            success: function (Resultssss) {
                debugger;
                DTrConditionStoressss(eval(Resultssss.d[0]), eval(Resultssss.d[1]));
            },
            error: function (Result) {
                alert("Error");
            }
        });
    });
    function DTrConditionStoressss(xCategory, YCategory) {
        Highcharts.chart('DTrConditionStatus', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'DTr Condition Store Wise'
            },
            xAxis: {
                categories: xCategory,
                title: {
                    text: 'Store Name'
                }
            },

            yAxis: {
                min: 0,
                title: {
                    text: 'TC Count'
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
            series: YCategory
        });
    }
</script>
<style>
    text.highcharts-credits{
        display:none;
    }
    tspan {
    font-weight: lighter!important;
    }
</style>