<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DTrConditionsField.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.DTrConditionsField" %>
<script src='<%= ResolveUrl("~/js/jquery-OneEightThree.min.js") %>'></script>
<script src="https://code.highcharts.com/highcharts.js"></script>  

<div style="width:-webkit-fill-available!important" class="span12">
        <div class="widget">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>DTr Conditions Field Wise<a id="viewstatus" runat="server" onserverclick="view_link_click" >
                              <span style="color: #fff">(View Details..)</span></a></h4>

            </div>

            <div style="min-width: 310px; height: 400px; margin: 0 auto"id="DTrConditionFieldStatus"></div>
        </div>
    </div>
<script>
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "MdDashboard.aspx/DTrConditionsField",
            contentType: "application/json; charset=utf-8",
            data : "{}",
            dataType: "json",
            success: function (Resultsss) {
                debugger;
                DTrConditionFieldssss(eval(Resultsss.d[0]), eval(Resultsss.d[1]));
            },
            error: function (Result) {
                alert("Error");
            }
        });
    });
    function DTrConditionFieldssss(xCategorys, YCategorys) {
        Highcharts.chart('DTrConditionFieldStatus', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'DTr Condition Field Wise'
            },
            xAxis: {
                categories: xCategorys,
                title: {
                    text: 'Office Name'
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
            series: YCategorys
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