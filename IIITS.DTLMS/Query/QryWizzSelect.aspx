<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QryWizzSelect.aspx.cs" Inherits="IIITS.DTLMS.Query.QryWizzSelect" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select QryWizard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>--%>
        <link type="text/css" rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" >
    <style>
        /* CSS for the main interaction*/
        .accordion > input[type="checkbox"] {
            position: absolute;
            left: -100vw;
        }

        .accordion .content {
            overflow-y: hidden;
            height: 0;
            transition: height 0.3s ease;
        }

        .accordion .textbox {
            overflow-y: hidden;
            height: 0;
            transition: height 0.3s ease;
        }

        .accordion > input[type="checkbox"]:checked ~ .content {
            height: auto;
            overflow: visible;
        }

        .accordion label {
            display: block;
        }
        /* Styling*/
        body {
            font: 16px/1.5em "Overpass", "Open Sans", Helvetica, sans-serif;
            color: #333;
            font-weight: 300;
        }

        .accordion {
            margin-bottom: 1em;
        }

            .accordion > input[type="checkbox"]:checked ~ .content {
                padding: 15px;
                border: 1px solid #e8e8e8;
                border-top: 0;
            }

            .accordion label {
                color: #333;
                cursor: pointer;
                font-size: 20px;
                padding: 15px;
                background: #4A8BC2;
            }

                .accordion label:hover,
                .accordion label:focus {
                    background: #337ab7;
                }

            .accordion .handle label:before {
                font-family: 'fontawesome';
                content: "\f054";
                display: inline-block;
                margin-right: 10px;
                font-size: .58em;
                line-height: 1.556em;
                vertical-align: middle;
            }

            .accordion > input[type="checkbox"]:checked ~ .handle label:before {
                content: "\f078";
            }


        /* Demo purposes only*/
        *,
        *:before,
        *:after {
            box-sizing: border-box;
        }

        body {
            padding: 40px;
        }

        .accordion {
            max-width: 100%;
        }

            .accordion p:last-child {
                margin-bottom: 0;
            }


        th {
            color: #045af7;
            font-size: large;
            font-weight: bold;
            text-align: center;
        }

        .col-xs-2.table-responsive {
            overflow-y: scroll;
            height: 300px;
        }
    </style>
    <script type="text/javascript">
        function validate() {
            if (document.getElementById("<%=txtQuery.ClientID%>").value == "") {
                alert("Please Enter your Query!.");
                //document.getElementById("Label1").innerHTML = "Please Enter Your Query!.";
                document.getElementById("<%=txtQuery.ClientID%>").focus();
                return false;
            }
            
            return true;
            
        }

        function searchFunction() {
            var input = document.getElementById("txtsearch");
            var filter = input.value.toUpperCase();
            var table = document.getElementById("GridQuery");
            var trs = table.getElementsByTagName("tr");
            for (var i = 1; i < trs.length; i++) {
                var tds = trs[i].getElementsByTagName("td");
                trs[i].style.display = "none";
                for (var j = 0; j < tds.length; j++) {
                    if (tds[j].innerHTML.toUpperCase().indexOf(filter) > -1) {
                        trs[i].style.display = "";
                        continue;
                    }
                }
            }
        }
        function searchTableFunction() {
            var input = document.getElementById("txtsearchtable");
            var filter = input.value.toUpperCase();
            var table = document.getElementById("TablesName");
            var trs = table.getElementsByTagName("tr");
            for (var i = 1; i < trs.length; i++) {
                var tds = trs[i].getElementsByTagName("td");
                trs[i].style.display = "none";
                for (var j = 0; j < tds.length; j++) {
                    if (tds[j].innerHTML.toUpperCase().indexOf(filter) > -1) {
                        trs[i].style.display = "";
                        continue;
                    }
                }
            }
        }
        function searchTablecolFunction() {
            var input = document.getElementById("txtsearchtablecol");
            var filter = input.value.toUpperCase();
            var table = document.getElementById("ColName");
            var trs = table.getElementsByTagName("tr");
            for (var i = 1; i < trs.length; i++) {
                var tds = trs[i].getElementsByTagName("td");
                trs[i].style.display = "none";
                for (var j = 0; j < tds.length; j++) {
                    if (tds[j].innerHTML.toUpperCase().indexOf(filter) > -1) {
                        trs[i].style.display = "";
                        continue;
                    }
                }
            }
        }
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row">
                <h3 style="text-align: center">Select Query Wizard for (HESCOM)</h3>
                <div class="col-xs-2 table-responsive">
                    <asp:TextBox ID="txtsearchtable" runat="server"  placeholder="Search..."  onkeyup="searchTableFunction()" Visible="true" autocomplete="off"></asp:TextBox>
                    <asp:GridView ID="TablesName" runat="server" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="TablesName_SelectedIndexChanged">
                    </asp:GridView>
                </div>
                <div class="col-xs-8">
                    <section class="accordion">
                        <input type="checkbox" name="collapse" id="handle1" checked="checked">
                        <h2 class="handle">
                            <label for="handle1">Query Wizard</label>
                        </h2>
                        <div class="content">
                            <div class="row">
                                <%--<div class="col-sm-1">
                                </div>--%>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtQuery" name="query" runat="server" TextMode="MultiLine" Rows="5" Columns="110"
                                        ondrop="drop(event)" ondragover="allowDrop(event)" Style="border-width: thick;max-width:150%"></asp:TextBox>
                                    <br />
                                    <asp:Button ID="btnLoad" runat="server" CssClass="btn btn-success" OnClick="cmdLoad_Click" Text="Load " OnClientClick="javascript:validate()" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <%--<asp:Button ID="btnCommit" runat="server" CssClass="btn btn-primary" OnClick="cmdLoad_Click" Text="Commit " Visible="false" />
                                    <asp:Button ID="btnRoleback" runat="server" CssClass="btn btn-success" OnClick="cmdRollback" Text="RollBack " Visible="false" />--%>
                                    <asp:Button ID="btnclear" runat="server" CssClass="btn btn-danger" OnClick="cmdClear" Text="Clear" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary" Visible="false" OnClick="cmdExport_Click" />
                                    <asp:HiddenField ID="hdfOffCode" runat="server" />
                                </div>
                                <%--<div class="col-sm-1">
                                </div>--%>
                            </div>
                        </div>
                    </section>
                     
                 </div>
                <div class="col-xs-2 table-responsive"">
                     <asp:TextBox ID="txtsearchtablecol" runat="server"  placeholder="Search..."  onkeyup="searchTablecolFunction()" Visible="false" autocomplete="off"></asp:TextBox>
                    <asp:GridView ID="ColName" runat="server">
                    </asp:GridView>
                </div>
                 <div class="col-xs-12">
                    <section class="accordion">
                        <input type="checkbox" name="collapse2" id="handle2" checked="checked">
                        <h2 class="handle">
                            <label for="handle2">Result</label>
                        </h2>
                        <div class="content" visible="false" ID="resultdiv"  runat="server" >
                            <div class="clearfix">
                                <div class="pull-right tableTools-container">
                                </div>
                            </div>
                            </div>
                            <div class="form-group">
                                <%--<asp:Label ID="lblsearch" runat="server" Visible="false">Search: &nbsp;</asp:Label>
                                <asp:TextBox ID="txtsearch" runat="server"  placeholder="Search..."  onkeyup="searchFunction()" Visible="false" autocomplete="off"></asp:TextBox>
                                 <asp:Label ID="lblCount" runat="server"   style="color:green" >  </asp:Label>    overflow-y: scroll;
                                <br />--%>
                                <br />
                                <asp:Panel ID="Panel1" runat="server" class="table-responsive" Style="height: 800px;">
                                    <asp:Label ID="Label1" runat="server" Text="Label" Style="color: Red;" Visible="False" ForeColor="Red"></asp:Label>
                                    <asp:GridView ID="GridQuery" runat="server" class="table table-striped  EASyGridLayout" Width="100% " OnPreRender="GridQuery_PreRender" OnPageIndexChanging="grdComplete_PageIndexChanging">
                                         <%--OnPreRender="GridQuery_PreRender" --%>
                                       
                                       
                                        <%-- <AlternatingRowStyle BackColor="#CCCCCC" />
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#808080" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#383838" />--%>
                                    </asp:GridView>
                                    <%--  <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnConf" runat="server" OnClick="btnConf_Click" Text="OK" CssClass="btn btn-info" Visible="false" ></asp:LinkButton>
                                                </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    <%-- <asp:LinkButton ID="btnConf" runat="server" OnClick="btnConf_Click" Text="OK" CssClass="btn btn-info" Visible="false"></asp:LinkButton>--%>
                                    <%--<asp:Button ID="btnConf" runat="server" OnClick="btnConf_Click" Text="OK" CssClass="btn btn-info" Visible="false" />--%>
                                    <br />
                                </asp:Panel>
                            </div>
                        <%--</div>--%>
                    </section>
            </div>
                 
            </div>
            </div>
    </form>
</body>
</html>


<script src="../dist/js/jquery.js"></script>
<script src="../dist/js/jquery.min.js"></script>
<script src="../components/datatables/media/js/jquery.dataTables.min.js"></script>
<script src="../components/_mod/datatables/jquery.dataTables.bootstrap.min.js"></script>
<script src="../components/datatables.net-buttons/js/dataTables.buttons.min.js"></script>
<script src="../components/datatables.net-buttons/js/buttons.flash.min.js"></script>
<script src="../components/datatables.net-buttons/js/buttons.html5.min.js"></script>
<script src="../components/datatables.net-buttons/js/buttons.print.min.js"></script>
<script src="../components/datatables.net-buttons/js/buttons.colVis.min.js"></script>
<script src="../components/datatables.net-select/js/dataTables.select.min.js"></script>
<script src="../dist/js/ace-elements.min.js"></script>
<script src="../dist/js/ace.min.js"></script>
<script src="../components/datatables/media/js/dataTables.tableTools.min.js"></script>
<script src="../components/datatables/media/js/jszip.min.js"></script>
<script src="../components/datatables/media/js/pdfmake.min.js"></script>
<script src="../components/datatables/media/js/vfs_fonts.js"></script>
<link href="/dist/css/bootstrap.css" rel="stylesheet" />
<link href="/dist/css/font-awesome.css" rel="stylesheet" />
<link href="/dist/css/fonts-family.css" rel="stylesheet" />
<link href="/dist/css/ace-skins.css" rel="stylesheet" />
<link href="/dist/css/ace-rtl.css" rel="stylesheet" />
<link href="/dist/css/EASy-1.3.3.css" rel="stylesheet" />
<link href="/components/_mod/jquery-ui/jquery-ui.css" rel="stylesheet" />
<link href="/components/_mod/jquery-ui.custom/jquery-ui.custom.css" rel="stylesheet" />
<link href="/components/chosen/chosen.css" rel="stylesheet" />
<link rel="stylesheet" href="/dist/css/ace.min.css" class="ace-main-stylesheet" />
<link href="/dist/css/sweetalert.css" rel="stylesheet" />


<script type="text/javascript">

    function asdfasdfsd(quote) {
        $('#txtQuery').val(function (i, text) {
            return text + quote;
        });
    }

    function allowDrop(ev) {
        ev.preventDefault();
    }

    function drag(ev) {

        ev.dataTransfer.setData("text", ev.target.id);
    }

    function drop(ev) {

        ev.preventDefault();
        var data = ev.dataTransfer.getData("text");
        var data1 = $('#' + data + '').html();
        data1 = data1.replace("<td>", "").replace("</td>", "").trim();
        asdfasdfsd(data1);
        //alert(data);
        //ev.target.append(data);
        //$("#Textquery").append(data).trigger('create');
        //$("#Textquery").after(data).trigger('create');
    }

    jQuery(function ($) {
        //initiate dataTables plugin
        var myTable =
            $('#<%= GridQuery.ClientID %>')
            //.wrap("<div class='dataTables_borderWrap' />")   //if you are applying horizontal scrolling (sScrollX)
				.DataTable({
				    //"paging": false,
				    "lengthMenu": [[10, 20, 30, 40, -1], [10, 20, 30, 40, "All"]],
				    "aaSorting": [],
				    "columnDefs": [
                        {
                            "targets": [0],
                            "visible": true,
                            "searchable": false,
                            "orderable": false
                        }
				    ]
				});

        //$.fn.dataTable.Buttons.defaults.dom.container.className = 'dt-buttons btn-overlap btn-group btn-overlap';

        //new $.fn.dataTable.Buttons(myTable, {
        //    buttons: [
        //          {
        //              "extend": "colvis",
        //              "text": "<i class='fa fa-search bigger-110 blue'></i> <span class='hidden'>Show/hide columns</span>",
        //              "className": "btn btn-white btn-primary btn-bold",
        //              "columns": ':not(:first)'
        //          },
        //          {
        //              "extend": "copy",
        //              "text": "<i class='fa fa-copy bigger-110 pink'></i> <span class='hidden'>Copy to clipboard</span>",
        //              "className": "btn btn-white btn-primary btn-bold",
        //              "exportOptions": {
        //                  "columns": ':visible'
        //              }
        //          },
        //          {
        //              "extend": "csv",
        //              "filename": "Query Wizard",
        //              "text": "<i class='fa fa-database bigger-110 orange'></i> <span class='hidden'>Export to CSV</span>",
        //              "className": "btn btn-white btn-primary btn-bold",
        //              "exportOptions": {
        //                  "columns": ':visible'
        //              }
        //          },
        //          {
        //              "extend": "excel",
        //              "filename": "Query Wizard",
        //              "text": "<i class='fa fa-file-excel-o bigger-110 green'></i> <span class='hidden'>Export to Excel</span>",
        //              "className": "btn btn-white btn-primary btn-bold",
        //              "exportOptions": {
        //                  "columns": ':visible'
        //              }
        //          },
        //          {
        //              "extend": "pdf",
        //              "filename": "Query Wizard",
        //              "text": "<i class='fa fa-file-pdf-o bigger-110 red'></i> <span class='hidden'>Export to PDF</span>",
        //              "className": "btn btn-white btn-primary btn-bold",
        //              "exportOptions": {
        //                  "columns": ':visible'
        //              }
        //          },
        //          {
        //              "extend": "print",
        //              "text": "<i class='fa fa-print bigger-110 grey'></i> <span class='hidden'>Print</span>",
        //              "className": "btn btn-white btn-primary btn-bold",
        //              "autoPrint": false,
        //              "message": 'List of all users',
        //              "exportOptions": {
        //                  "columns": ':visible'
        //              }
        //          }
        //    ]
        //});
        myTable.buttons().container().appendTo($('.tableTools-container'));

        //style the message box
        var defaultCopyAction = myTable.button(1).action();
        myTable.button(1).action(function (e, dt, button, config) {
            defaultCopyAction(e, dt, button, config);
            $('.dt-button-info').addClass('gritter-item-wrapper gritter-info gritter-center white');
        });


        var defaultColvisAction = myTable.button(0).action();
        myTable.button(0).action(function (e, dt, button, config) {

            defaultColvisAction(e, dt, button, config);


            if ($('.dt-button-collection > .dropdown-menu').length == 0) {
                $('.dt-button-collection')
                    .wrapInner('<ul class="dropdown-menu dropdown-light dropdown-caret dropdown-caret" />')
                    .find('a').attr('href', '#').wrap("<li />")
            }
            $('.dt-button-collection').appendTo('.tableTools-container .dt-buttons')
        });

        ////

        setTimeout(function () {
            $($('.tableTools-container')).find('a.dt-button').each(function () {
                var div = $(this).find(' > div').first();
                if (div.length == 1) div.tooltip({ container: 'body', title: div.parent().text() });
                else $(this).tooltip({ container: 'body', title: $(this).text() });
            });
        }, 500);

        $(document).on('click', '#dynamic-table .dropdown-toggle', function (e) {
            e.stopImmediatePropagation();
            e.stopPropagation();
            e.preventDefault();
        });

        /********************************/
        //add tooltip for small view action buttons in dropdown menu
        $('[data-rel="tooltip"]').tooltip({ placement: tooltip_placement });

        //tooltip placement on right or left
        function tooltip_placement(context, source) {
            var $source = $(source);
            var $parent = $source.closest('table')
            var off1 = $parent.offset();
            var w1 = $parent.width();

            var off2 = $source.offset();
            //var w2 = $source.width();

            if (parseInt(off2.left) < parseInt(off1.left) + parseInt(w1 / 2)) return 'right';
            return 'left';
        }

        /***************/
        $('.show-details-btn').on('click', function (e) {
            e.preventDefault();
            $(this).closest('tr').next().toggleClass('open');
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
        });
        /***************/
    })
</script>
