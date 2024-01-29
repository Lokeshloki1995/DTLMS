<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl1.ascx.cs" Inherits="IIITS.DTLMS.graphusercontrols.WebUserControl1" %>
<header>
    <script src="http://code.highcharts.com/highcharts.js"></script>
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>--%>
</header>

<div class="row-fluid">

    <div class="span12">
        <!-- BEGIN SAMPLE FORMPORTLET-->
        <div class="widget blue">
            <div class="widget-title">
                <h4><i class="icon-reorder"></i>sdf sdf</h4>
                <span class="tools">
                    <a href="javascript:;" class="icon-chevron-down"></a>
                    <a href="javascript:;" class="icon-remove"></a>
                </span>
            </div>
            <div class="widget-body">

                <ul class="breadcrumb" style="font-weight: bold">
                    <li>
 <a href="ViewDetailsgrid.ascx?RefId=true&Gridid=FailureDetails" onclick="" target="_blank">Current Status</a>

    <%--                  <a  id="viewstatus" runat="server">Current Status 
                              <span style="color: Blue">(View Details..)</span></a>--%>
                    </li>
                </ul>
            </div>

           

          <div id="container"></div>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>
</div>


<%--<script type="text/javascript" src='<%= ResolveUrl("~/js/jquery-1.8.3.min.js") %>'></script>--%>
<%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>--%>
<%--<script src="http://code.highcharts.com/highcharts.js"></script>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>--%>

<%--<script src="../assets/highcharts.js"></script>
<script src="../high/highcharts_3d.js"></script>
<script src="../high/exporting.js"></script>
<script src="../high/export-data.js"></script>--%>


 <%--   <script src="../js/jquery-1.8.2.min.js"></script>--%>
<%--     <script type="text/javascript" src='<%= ResolveUrl("~/js/jquery-1.8.3.min.js") %>'></script>--%>
<%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>--%>


<%--<script type="text/javascript" src="https://code.highcharts.com/highcharts-3d.js"></script>
<script type="text/javascript" src="https://code.highcharts.com/modules/exporting.js"></script>
<script type="text/javascript" src="https://code.highcharts.com/modules/export-data.js"></script>--%>

   <script type="text/javascript">

       //graphusercontrols
       $(document).ready(function () {
           debugger;
           $.ajax({
               type: "POST",
               contentType: "application/json; charset=utf-8",
               url: "/graphusercontrols/WebForm1.aspx/GetChartData",
               data: "",
               dataType: "json",
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

           var chart = new Highcharts.Chart({
               chart: {
                   //plotBackgroundColor: null,
                   // plotBorderWidth: 1,//null,
                   renderTo: 'container',
                   // plotShadow: false,
                   //backgroundColor: {
                   //     linearGradient: [0, 0, 500, 500],
                   //     stops: [
                   // [0, 'rgb(255, 255, 255)'],
                   // [1, 'rgb(200, 200, 255)']
                   //     ]
                   // }
               },
               title: {
                   text: ''
               },
               tooltip: {
                   pointFormat: '{series.Age}:series.AgeCount'
               },
               //plotOptions: {
               //    pie: {
               //        allowPointSelect: true,
               //        cursor: 'pointer',
               //        dataLabels: {
               //            enabled: true,
               //            format: '',
               //            style: {
               //                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
               //            }
               //        }
               //    }
               //},
               series: [{
                   type: 'pie',
                   name: 'Fruit share',
                   data: series
               }]
           });
       }
    </script>
   
 


<%--<b>{point.Age}</b>: {point.percentage:.1f} %--%>