<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="IIITS.DTLMS.graphusercontrols.WebForm1" %>

  
<%--<script src="https://code.highcharts.com/highcharts.js"></script>
<script src="https://code.highcharts.com/highcharts-3d.js"></script>
<script src="https://code.highcharts.com/modules/exporting.js"></script>
       <script type="text/javascript" src='<%= ResolveUrl("~/js/jquery-1.8.3.min.js") %>'></script>
<script src="https://code.highcharts.com/modules/export-data.js"></script>--%>
    <%--<script type="text/javascript">
        //graphusercontrols
        $(document).ready(function () {
            debugger;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/WebForm1.aspx/LoadFailureDetails",
                data: "{}",
                // dataType: "json",
                success: function (Result) {
                    Result = Result.d;
                    var data = [];

                    for (var i in Result) {
                        var serie = new Array(Result[i].Age, Result[i].AgeCount);
                        data.push(serie);
                    }

                    DreawChart(data);
                },
                error: function (Result) {
                    alert("Error");
                }
            });
        });
        function DreawChart(series) {

            $('#container').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: 1, //null,
                    plotShadow: false,
                    backgroundColor: {
                        linearGradient: [0, 0, 500, 500],
                        stops: [
                    [0, 'rgb(255, 255, 255)'],
                    [1, 'rgb(200, 200, 255)']
                        ]
                    }
                },
                title: {
                    text: 'fruit  market shares at a specific month, 2014'
                },
                tooltip: {
                    pointFormat: '{series.Age}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.Age}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: 'Fruit share',
                    data: series
                }]
            });
        }
    </script>--%>


