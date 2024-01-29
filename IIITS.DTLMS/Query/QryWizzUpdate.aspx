<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QryWizzUpdate.aspx.cs" Inherits="IIITS.DTLMS.Query.QryWizzUpdate" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update Query Wizard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
  <%--  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>--%>
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet">
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
            height: 400px;
        }
    </style>
    <script type="text/javascript">
        function validate() {
           <%-- if (document.getElementById("<%=txtQuery.ClientID%>").value == "") {
                alert("Please Enter your Query!.");
                document.getElementById("<%=txtQuery.ClientID%>").focus();
                return false;
                e.preventDefault();
            }else
            {
                return true;
            }--%>
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
    <form id="form1" runat="server" name="myform">
        <div class="container-fluid">
            <div class="row">
                <h3 style="text-align: center">Update Query Wizard for (HESCOM)</h3>
                <div class="col-xs-2 table-responsive">
                    <asp:TextBox ID="txtsearchtable" runat="server" placeholder="Search..." onkeyup="searchTableFunction()" Visible="true" autocomplete="off"></asp:TextBox>
                    <asp:GridView ID="TablesName" runat="server" OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="TablesName_SelectedIndexChanged">
                    </asp:GridView>
                </div>
                <div class="col-xs-8 table-responsive">
                    <section class="accordion">
                        <input type="checkbox" name="collapse" id="handle1" checked="checked">
                        <h2 class="handle">
                            <label for="handle1">Query Wizard</label>
                        </h2>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="txtQuery" name="query" runat="server" TextMode="MultiLine" Rows="5" Columns="110"
                                        ondrop="drop(event)" ondragover="allowDrop(event)" Style="border-width: thick;resize:none;width: 100%;"></asp:TextBox>
                                     <br /> </div>
                                    <div class="text-center">
                                        <asp:Button ID="btnLoad" runat="server" CssClass="btn btn-success" OnClick="cmdLoad_Click" Text="Execute" OnClientClick="javascript:validate()" />
                                        <asp:Button ID="btnCommit" runat="server" CssClass="btn btn-primary" OnClick="cmdLoad_Click" Text="Commit " Visible="false" />
                                        <asp:Button ID="btnRollback" runat="server" CssClass="btn btn-success" OnClick="cmdRollback" Text="RollBack " Visible="false" />
                                        <asp:Button ID="btnclear" runat="server" CssClass="btn btn-danger" OnClick="cmdClear" Text="Clear" />
                                    </div>
                                    <div class="col-md-5">
                                        Ticket No<span class="Mandotary" style="color: red"> * </span>
                                        <asp:TextBox ID="txtTicketID" runat="server" MaxLength="15" AutoComplete="off" Style="border-width: medium; padding-top: 7px;" onkeypress="javascript: return onlyAlphabets(event,this);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                    </div>
                                    <div class="col-md-5">
                                        Remarks <span class="Mandotary" style="color: red">* </span>
                                        <asp:TextBox ID="txtRemarks" runat="server" AutoComplete="off" MaxLength="50" Style="border-width: medium; height: 37px; width: 245px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                    </section>
                    <section class="accordion">
                        <input type="checkbox" name="collapse2" id="handle2" checked="checked">
                        <h2 class="handle">
                            <label for="handle2">Result</label>
                        </h2>
                        <div class="content">
                            <div class="clearfix">
                                <div class="pull-right tableTools-container">
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Panel ID="Panel1" runat="server">
                                    <asp:Label ID="Label1" runat="server" Text="Label" Style="color: Red;" Visible="False" ForeColor="Red"></asp:Label>
                                    <asp:GridView ID="GridQuery" runat="server" class="table table-striped  EASyGridLayout" Width="100% " OnPreRender="GridQuery_PreRender">
                                    </asp:GridView>
                                    <asp:LinkButton ID="btnConf" runat="server" OnClick="btnConf_Click" Text="OK" CssClass="btn btn-info" Visible="false"></asp:LinkButton>
                                    <%--<asp:Button ID="btnConf" runat="server" OnClick="btnConf_Click" Text="OK" CssClass="btn btn-info" Visible="false" />--%>
                                    <br />
                                </asp:Panel>
                            </div>
                        </div>
                    </section>
                </div>
                <div class="col-xs-2 table-responsive">
                    <asp:TextBox ID="txtsearchtablecol" runat="server" placeholder="Search..." onkeyup="searchTablecolFunction()" Visible="false" autocomplete="off"></asp:TextBox>
                    <asp:GridView ID="ColName" runat="server">
                    </asp:GridView>
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
    }

    jQuery(function ($) {
        //initiate dataTables plugin
        var myTable =
            $('#<%= GridQuery.ClientID %>')
				.DataTable({
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
    })
</script>
