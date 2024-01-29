<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FailureStagesGraph.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.FailureStagesGraph" %>
<br />
<div style="width:-webkit-fill-available!important"class="span12">
<div class="widget">
            <div class="widget-title">
     
                <h4><i class="icon-reorder"></i>DTr Failure Status
                     <a id="viewstatus" onserverclick="view_link_click" runat="server"><span style="color:#fff">(View Details..)</span></a>
               </h4>
                   <span class="tools">
                    <a href="javascript:;" class="icon-chevron-down"></a>
                    <a href="javascript:;" class="icon-remove"></a>
                </span>
            </div>
            
 
            <div id="failuredata"></div>
           
       
    </div>
    </div>

<script src='<%= ResolveUrl("~/js/jquery-OneEightThree.min.js") %>'></script>
<script src="https://code.highcharts.com/highcharts.js"></script>
<script>
    function DreawChart(values) {
        debugger;
        Highcharts.chart('failuredata', {
            chart: {
                type: 'column'
            },
            title: {
                text: 'DTr Failure Status'
            },
            xAxis: {
               
                categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'June', 'July', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec'],
                title: {
                    text: 'Month'
                }
            },

            yAxis: {
                min: 0,
                title: {
                    text: 'No of DTr Failures'
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
            series: values
        });
    }
</script>
<script>
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "MdDashboard.aspx/FailureAnalysis",
            contentType: "application/json; charset=utf-8",
            data : "{}",
            dataType: "json",
            success: function (Result) {
                debugger;
                var serrrr= eval(Result.d[0]);
                DreawChart(serrrr);
            },
            error: function (Result) {
               // alert("Error");
            }
        });
    });
</script>
<style>
    text.highcharts-credits{
        display:none;
    }
    tspan {
    font-weight: lighter!important;
    }
</style>