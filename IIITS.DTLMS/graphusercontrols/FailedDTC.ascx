<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FailedDTC.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.FailedDTC" %>
<script src='<%= ResolveUrl("~/js/jquery-OneEightThree.min.js") %>'></script>
<!-- Latest compiled and minified CSS -->
<script src="https://code.highcharts.com/highcharts.js"></script>
<script src="https://code.highcharts.com/highcharts-3d.js"></script>


    <div style="width: 48.0%!important"class="span6">
        <div class="widget">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>Frequently Failed DTC<a id="viewstatus" runat="server" onserverclick="view_link_click">
                              <span style="color: #fff">(View Details..)</span></a></h4>

            </div>
            <div id="FailedDTC" style="height: 400px"></div>
        </div>
    </div>

<script>
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "MdDashboard.aspx/FrequentlyFailedDtc",
            contentType: "application/json; charset=utf-8",
            data: "{}",
            dataType: "json",
            success: function (Resultsss) {
                debugger;
                FailedDTCVal(eval(Resultsss.d[0]));
            },
            error: function (Result) {
                alert("Error");
            }
        });
    });
    function FailedDTCVal(strValues) {
        debugger;
     Highcharts.chart('FailedDTC', {
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
            text: 'Frequently Failed DTC',

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
        name: 'Frequently Failed DTC',
        data: strValues
    }],


    });
    }
   
</script>