<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="UserView.aspx.cs" Inherits="IIITS.DTLMS.MasterForms.UserView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>--%>

    <link type="text/css" href="../assets/jquery.dataTables.css" rel="stylesheet" />
    <script type="text/javascript" src="../assets/jquery.dataTables.min.js"></script>

    <link type="text/css" rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.min.css" />
    <%--    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js"></script>--%>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/chosen/1.8.7/chosen.jquery.min.js"></script>


    <style type="text/css">
        .chosen-container-multi .chosen-choices {
            max-height: 110px !important;
            overflow: auto !important;
        }

            .chosen-container-multi .chosen-choices li.search-choice {
                font-size: 11px;
            }

        #ContentPlaceHolder1_grdUser {
            overflow: scroll !important;
            width: 100% !important;
        }
    </style>
    <style type="text/css">
        .main {
            border: 1px solid #c5c5c5 !important;
        }

        .tabs {
            margin: 0;
            padding: 0.2em 0.2em 0 !important;
            color: #333333 !important;
            font-weight: bold !important;
        }

        .tab-button {
            float: left !important;
            padding: 0.5em 1em !important;
            text-decoration: none !important;
            font-size: 14px !important;
            border: 1px solid #c5c5c5 !important;
        }

        table {
            overflow: scroll;
        }

        td {
            border: 1px solid #ccc;
            text-align: center;
        }

        .table-advance thead tr th {
            background-color: #438eb9 !important;
            color: #fff;
        }

        table.dataTable thead th {
            border-bottom: 1px solid #111;
            font-size: 12px !important;
        }

        table.dataTable tbody th, table.dataTable tbody td {
            padding: 10px 0px !important;
            text-align: center !important;
        }

        .table-advance tr td {
            border-left-width: 1px !important;
            border: 1px solid #d4d4d4;
            font-size: 12px !important;
        }

        th {
            white-space: nowrap;
        }

        table#ContentPlaceHolder1_grdUser {
            overflow: auto;
        }

        td {
            border: none;
            text-align: center;
        }

        .table-advance thead tr th {
            background-color: #438eb9 !important;
            color: #fff;
        }

        th {
            white-space: nowrap;
            text-align: center !important;
        }

        thead {
            text-align: center !important;
        }

        span {
            text-align: center;
        }

        select#ContentPlaceHolder1_cmbZone, select#ContentPlaceHolder1_cmbsubdivision, select#ContentPlaceHolder1_cmbCircle, select#ContentPlaceHolder1_cmbSection, select#ContentPlaceHolder1_cmbDivision {
            width: 216px !important;
        }

        select {
            width: 70px;
        }

        .gvPagerCss span {
            background-color: #f9f9f9;
            font-size: 18px;
        }

        .gvPagerCss td {
            padding-left: 5px;
            padding-right: 5px;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ascending th a {
            background: url(img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        .descending th a {
            background: url(img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .both th a {
            background: url(img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        div.pager {
            text-align: center;
            margin: 1em 0;
        }

        div.pager {
            text-align: center;
            margin: 1em 0;
        }

        div.pg-goto {
            color: #000000;
            font-size: 15px;
            cursor: pointer;
            background: #D0B389;
            padding: 2px 4px 2px 4px;
        }

        div.pg-selected {
            color: #fff;
            font-size: 15px;
            background: #000000;
            padding: 2px 4px 2px 4px;
        }

        div.pg-normal {
            color: #000000;
            font-size: 15px;
            cursor: pointer;
            background: #D0B389;
            padding: 2px 4px 2px 4px;
        }

        .chosen-container-multi .chosen-drop .result-selected {
            display: none !important;
        }

        .chosen-container-multi .chosen-choices li.search-choice span {
            word-wrap: break-word !important;
            font-size: 9px !important;
        }

        .chosen-container .chosen-results li.no-results {
            display: none !important;
        }

        input#ContentPlaceHolder1_cmdexport:focus {
            background: #22c0cb !important;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#ContentPlaceHolder1_grdUser').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "sPaginationType": "full_numbers"
            });
        });
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip(); // added css in font-awesome.min.css line 43 and 405
        });

        function ConfirmStatus(status) {

            var result = confirm('Are you sure,Do you want to ' + status + ' User?');
            if (result) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            debugger;


            //if ($(".chosen-select").val().length != "") {
            //    $(this).prop('disabled', true).trigger("chosen:updated");
            //}
            //else {
            //    $(this).prop('disabled', false).trigger("chosen:updated");
            //}


            var navigationType = performance.navigation.type;

            if (navigationType === 1) {
                // Page has been refreshed
                // Your code here
                $('#hdcmbZone').val('');
                $('#hdcmbCircle').val('');
                $('#hdcmbDivision').val('');
                $('#hdcmbSubDivision').val('');
                $('#hdcmbSection').val('');
                $('#txtlastDropdownSelected').val('');
                localStorage.clear();
                window.location.href = 'UserView.aspx';
            } else if (navigationType === 0) {
                // Page has been loaded
                // Your code here

            }


            var Session_OfficeCode = $('#ContentPlaceHolder1_txtSessionoffcode').val();
            if (Session_OfficeCode != "3") {
                $('#ContentPlaceHolder1_cmbsubdivision').val('').trigger("chosen:updated");
                $('#ContentPlaceHolder1_cmbSection').val('').trigger("chosen:updated");
            }
            else {
                $('#ContentPlaceHolder1_cmbZone').val('').trigger('chosen:updated');
                $('#ContentPlaceHolder1_cmbCircle').val('').trigger("chosen:updated");
                $('#ContentPlaceHolder1_cmbDivision').val('').trigger("chosen:updated");
                $('#ContentPlaceHolder1_cmbsubdivision').val('').trigger("chosen:updated");
                $('#ContentPlaceHolder1_cmbSection').val('').trigger("chosen:updated");
            }

            //localStorage.clear();
            var chosenSelect = $('#ContentPlaceHolder1_cmbZone');
            var chosenSelect1 = $('#ContentPlaceHolder1_cmbCircle');
            var chosenSelect2 = $('#ContentPlaceHolder1_cmbDivision');
            var chosenSelect3 = $('#ContentPlaceHolder1_cmbsubdivision');
            var chosenSelect4 = $('#ContentPlaceHolder1_cmbSection');
            //Get the values from the local storages and assign to chosen select
            //For ZONE values are stoing to the local storage
            var savedSelectedValue = localStorage.getItem('chosenSelectValue');
            if (savedSelectedValue) {
                var parsedSelectedValue = JSON.parse(savedSelectedValue);
                chosenSelect.val(parsedSelectedValue).attr('disabled', true).trigger("chosen:updated");
            }
            //For Circle values are stoing to the local storage
            var savedSelectedValue1 = localStorage.getItem('chosenSelectValue1');
            if (savedSelectedValue1) {
                var parsedSelectedValue1 = JSON.parse(savedSelectedValue1);
                chosenSelect1.val(parsedSelectedValue1).attr('disabled', true).trigger("chosen:updated");
            }
            //For Division values are stoing to the local storage
            var savedSelectedValue2 = localStorage.getItem('chosenSelectValue2');
            if (savedSelectedValue2) {
                var parsedSelectedValue2 = JSON.parse(savedSelectedValue2);
                chosenSelect2.val(parsedSelectedValue2).attr('disabled', true).trigger("chosen:updated");
            }

            //For Sub Division values are stoing to the local storage
            var savedSelectedValue3 = localStorage.getItem('chosenSelectValue3');
            if (savedSelectedValue3) {
                var parsedSelectedValue3 = JSON.parse(savedSelectedValue3);
                chosenSelect3.val(parsedSelectedValue3).attr('disabled', true).trigger("chosen:updated");
            }
            //For Section values are stoing to the local storage
            var savedSelectedValue4 = localStorage.getItem('chosenSelectValue4');
            if (savedSelectedValue4) {
                var parsedSelectedValue4 = JSON.parse(savedSelectedValue4);
                chosenSelect4.val(parsedSelectedValue4).attr('disabled', true).trigger("chosen:updated");
            }



            //Chosen select dropdowns for multiselct
            $('.chosen-select').chosen();

            //Filter the Zone dropdown
            $('#ContentPlaceHolder1_cmbZone').on('change', function (event, params) {
                debugger;

                $('#ContentPlaceHolder1_cmbDivision').trigger('chosen:updated');
                var selectedValues = $(this).val();
                var Zone_val = $('#ContentPlaceHolder1_cmbZone').val();
                $('#ContentPlaceHolder1_hdcmbZone ').val(Zone_val);
                if (Zone_val != "") {
                    $('#ContentPlaceHolder1_txtlastDropdownSelected ').val("1");
                }

                $('#ContentPlaceHolder1_cmbCircle').prop('disabled', false).trigger("chosen:updated");

                if (params.deselected) {

                    debugger;

                    var deselectedValues = params.deselected.charAt(0);
                    var regex = new RegExp('^' + deselectedValues);
                    $('#ContentPlaceHolder1_cmbCircle').each(function () {
                        debugger
                        var selectedValues1 = $(this).val();
                        var filteredValues = selectedValues1.filter(function (value) {
                            return !value.startsWith(deselectedValues);
                        });
                        $('#ContentPlaceHolder1_cmbCircle').val(filteredValues).trigger('chosen:updated');
                    });



                    $('#ContentPlaceHolder1_cmbDivision').each(function () {

                        var selectedValues1 = $(this).val();
                        var filteredValues = selectedValues1.filter(function (value) {
                            return !value.startsWith(deselectedValues);
                        });
                        //if (filteredValues == "") {
                        //    $('#ContentPlaceHolder1_cmbDivision').prop('disabled', true).trigger("chosen:updated");

                        //}
                        $('#ContentPlaceHolder1_cmbDivision').val(filteredValues).trigger('chosen:updated');
                    });








                    $('#ContentPlaceHolder1_cmbsubdivision').each(function () {
                        debugger;
                        var selectedValues1 = $(this).val();

                        var filteredValues = selectedValues1.filter(function (value) {
                            return !value.startsWith(deselectedValues);
                        });
                        //if (filteredValues == "") {
                        //    $('#ContentPlaceHolder1_cmbsubdivision').prop('disabled', true).trigger("chosen:updated");

                        //}
                        $('#ContentPlaceHolder1_cmbsubdivision').val(filteredValues).trigger('chosen:updated');


                    });





                    $('#ContentPlaceHolder1_cmbSection').each(function () {
                        debugger;
                        var selectedValues1 = $(this).val();
                        var filteredValues = selectedValues1.filter(function (value) {
                            return !value.startsWith(deselectedValues);
                        });
                        //if (filteredValues == "") {
                        //    $('#ContentPlaceHolder1_cmbSection').prop('disabled', true).trigger("chosen:updated");

                        //}
                        $('#ContentPlaceHolder1_cmbSection').val(filteredValues).trigger('chosen:updated');

                    });





                    DesableFuntion();
                }

                //Filter the Circle dropdown based on Zone 



                $('#ContentPlaceHolder1_cmbCircle option').each(function () {
                    debugger
                    var optionValue = $(this).val();
                    var firstLetter = optionValue.charAt(0);

                    if (selectedValues && selectedValues.length > 0) {
                        debugger
                        if (selectedValues.includes(firstLetter)) {
                            debugger
                            $(this).show();
                        } else {

                            $(this).hide();
                        }
                    } else {
                        $(this).show();
                    }
                });

                $('#ContentPlaceHolder1_cmbCircle').trigger('chosen:updated');

                var selectedValues = $("#ContentPlaceHolder1_cmbCircle").val();

                $('#ContentPlaceHolder1_cmbDivision option').each(function () {
                    debugger
                    var optionValue = $(this).val();
                    var firstLetter = optionValue.substring(0, 2);

                    if (selectedValues && selectedValues.length > 0) {
                        debugger
                        if (selectedValues.includes(firstLetter)) {
                            debugger
                            $(this).show();
                        } else {

                            $(this).hide();
                        }
                    } else {
                        $(this).show();
                    }
                });

                $('#ContentPlaceHolder1_cmbDivision').trigger('chosen:updated');




                var selectedValues = $("#ContentPlaceHolder1_cmbDivision").val();

                $('#ContentPlaceHolder1_cmbsubdivision option').each(function () {
                    debugger
                    var optionValue = $(this).val();
                    var firstLetter = optionValue.substring(0, 3);

                    if (selectedValues && selectedValues.length > 0) {
                        debugger
                        if (selectedValues.includes(firstLetter)) {
                            debugger
                            $(this).show();
                        } else {

                            $(this).hide();
                        }
                    } else {
                        $(this).show();
                    }
                });

                $('#ContentPlaceHolder1_cmbsubdivision').trigger('chosen:updated');

                var selectedValues = $("#ContentPlaceHolder1_cmbsubdivision").val();

                $('#ContentPlaceHolder1_cmbSection option').each(function () {
                    debugger
                    var optionValue = $(this).val();
                    var firstLetter = optionValue.substring(0, 4);

                    if (selectedValues && selectedValues.length > 0) {
                        debugger
                        if (selectedValues.includes(firstLetter)) {
                            debugger
                            $(this).show();
                        } else {

                            $(this).hide();
                        }
                    } else {
                        $(this).show();
                    }
                });

                $('#ContentPlaceHolder1_cmbSection').trigger('chosen:updated');









            });

            //Filter the Zone dropdown
            $('#ContentPlaceHolder1_cmbCircle').on('change', function (event, params) {
                debugger

                var selectedValues_1 = $(this).val();
                var Circle_val = $('#ContentPlaceHolder1_cmbCircle').val();
                $('#ContentPlaceHolder1_hdcmbCircle ').val(Circle_val);
                if (Circle_val != "") {
                    $('#ContentPlaceHolder1_txtlastDropdownSelected ').val("2");
                }
                $('#ContentPlaceHolder1_cmbDivision').prop('disabled', false).trigger("chosen:updated");

                //if (Circle_val == "") {
                //    $('#ContentPlaceHolder1_cmbDivision').prop('disabled', true).trigger("chosen:updated");
                //}
                //Deselect the  related options in other dropdowns after deselecting the option in Circle
                if (params.deselected) {
                    var deselectedValues = params.deselected;

                    var regex = new RegExp('^' + deselectedValues);

                    $('#ContentPlaceHolder1_cmbDivision').each(function () {

                        var selectedValues1 = $(this).val();
                        var filteredValues = selectedValues1.filter(function (value) {
                            return !value.startsWith(deselectedValues);
                        });

                        $('#ContentPlaceHolder1_cmbDivision').val(filteredValues).trigger('chosen:updated');


                    });



                    $('#ContentPlaceHolder1_cmbsubdivision').each(function () {
                        debugger;
                        var selectedValues1 = $(this).val();
                        var filteredValues = selectedValues1.filter(function (value) {
                            return !value.startsWith(deselectedValues);
                        });
                        //if (filteredValues == "") {
                        //    $('#ContentPlaceHolder1_cmbsubdivision').prop('disabled', true).trigger("chosen:updated");

                        //}
                        $('#ContentPlaceHolder1_cmbsubdivision').val(filteredValues).trigger('chosen:updated');

                    });

                    $('#ContentPlaceHolder1_cmbSection').each(function () {
                        debugger;
                        var selectedValues1 = $(this).val();
                        var filteredValues = selectedValues1.filter(function (value) {
                            return !value.startsWith(deselectedValues);
                        });
                        //if (filteredValues == "") {
                        //    $('#ContentPlaceHolder1_cmbSection').prop('disabled', true).trigger("chosen:updated");

                        //}
                        $('#ContentPlaceHolder1_cmbSection').val(filteredValues).trigger('chosen:updated');
                    });
                    DesableFuntion();
                }


                //Filter the Division dropdown based on Zone 
                $('#ContentPlaceHolder1_cmbDivision option').each(function () {
                    debugger
                    var optionValue1 = $(this).val();
                    var firstLetter1 = optionValue1.substring(0, 2);
                    if (selectedValues_1 && selectedValues_1.length > 0) {
                        debugger
                        if (selectedValues_1.includes(firstLetter1)) {
                            debugger
                            $(this).show();
                        } else {

                            $(this).hide();
                        }
                    } else {
                        $(this).show();
                    }
                });
                $('#ContentPlaceHolder1_cmbDivision').trigger('chosen:updated');







                var selectedValues = $("#ContentPlaceHolder1_cmbDivision").val();

                $('#ContentPlaceHolder1_cmbsubdivision option').each(function () {
                    debugger
                    var optionValue = $(this).val();
                    var firstLetter = optionValue.substring(0, 3);

                    if (selectedValues && selectedValues.length > 0) {
                        debugger
                        if (selectedValues.includes(firstLetter)) {
                            debugger
                            $(this).show();
                        } else {

                            $(this).hide();
                        }
                    } else {
                        $(this).show();
                    }
                });

                $('#ContentPlaceHolder1_cmbsubdivision').trigger('chosen:updated');

                var selectedValues = $("#ContentPlaceHolder1_cmbsubdivision").val();

                $('#ContentPlaceHolder1_cmbSection option').each(function () {
                    debugger
                    var optionValue = $(this).val();
                    var firstLetter = optionValue.substring(0, 4);

                    if (selectedValues && selectedValues.length > 0) {
                        debugger
                        if (selectedValues.includes(firstLetter)) {
                            debugger
                            $(this).show();
                        } else {

                            $(this).hide();
                        }
                    } else {
                        $(this).show();
                    }
                });

                $('#ContentPlaceHolder1_cmbSection').trigger('chosen:updated');
            });

            //Filter the Division dropdown
            $('#ContentPlaceHolder1_cmbDivision').on('change', function (event, params) {
                debugger
                var selectedValues_1 = $(this).val();
                var Division_val = $('#ContentPlaceHolder1_cmbDivision').val();
                $('#ContentPlaceHolder1_hdcmbDivision ').val(Division_val);
                if (Division_val != "") {
                    $('#ContentPlaceHolder1_txtlastDropdownSelected ').val("3");
                }

                $('#ContentPlaceHolder1_cmbsubdivision').prop('disabled', false).trigger("chosen:updated");
                //if (Division_val == "") {
                //    $('#ContentPlaceHolder1_cmbsubdivision').prop('disabled', true).trigger("chosen:updated");
                //}
                //Deselect the related options in other dropdowns after deselecting the option in Division
                if (params.deselected) {
                    var deselectedValues = params.deselected;
                    $('#ContentPlaceHolder1_cmbsubdivision').each(function () {
                        debugger;
                        var selectedValues1 = $(this).val();
                        var filteredValues = selectedValues1.filter(function (value) {
                            return !value.startsWith(deselectedValues);
                        });

                        $('#ContentPlaceHolder1_cmbsubdivision').val(filteredValues).trigger('chosen:updated');

                    });


                    $('#ContentPlaceHolder1_cmbSection').each(function () {
                        debugger;
                        var selectedValues1 = $(this).val();
                        var filteredValues = selectedValues1.filter(function (value) {
                            return !value.startsWith(deselectedValues);
                        });
                        //if (filteredValues == "") {
                        //    $('#ContentPlaceHolder1_cmbSection').prop('disabled', true).trigger("chosen:updated");

                        //}
                        $('#ContentPlaceHolder1_cmbSection').val(filteredValues).trigger('chosen:updated');

                    });

                    DesableFuntion();
                }


                //Filter the Divison dropdown based on Circle 
                $('#ContentPlaceHolder1_cmbsubdivision option').each(function () {
                    debugger
                    var optionValue1 = $(this).val();
                    var firstLetter1 = optionValue1.substring(0, 3);
                    if (selectedValues_1 && selectedValues_1.length > 0) {
                        debugger
                        if (selectedValues_1.includes(firstLetter1)) {
                            debugger
                            $(this).show();
                        } else {

                            $(this).hide();
                        }
                    } else {
                        $(this).show();
                    }
                });
                $('#ContentPlaceHolder1_cmbsubdivision').trigger('chosen:updated');

                var selectedValues = $("#ContentPlaceHolder1_cmbsubdivision").val();

                $('#ContentPlaceHolder1_cmbSection option').each(function () {
                    debugger
                    var optionValue = $(this).val();
                    var firstLetter = optionValue.substring(0, 4);

                    if (selectedValues && selectedValues.length > 0) {
                        debugger
                        if (selectedValues.includes(firstLetter)) {
                            debugger
                            $(this).show();
                        } else {

                            $(this).hide();
                        }
                    } else {
                        $(this).show();
                    }
                });

                $('#ContentPlaceHolder1_cmbSection').trigger('chosen:updated');
            });

            //Filter the SubDivision dropdown
            $('#ContentPlaceHolder1_cmbsubdivision').on('change', function (event, params) {
                debugger
                var selectedValues_1 = $(this).val();
                var SubDivision_val = $('#ContentPlaceHolder1_cmbsubdivision').val();
                $('#ContentPlaceHolder1_hdcmbSubDivision ').val(SubDivision_val);
                if (SubDivision_val != "") {
                    $('#ContentPlaceHolder1_txtlastDropdownSelected ').val("4");
                }

                $('#ContentPlaceHolder1_cmbSection').prop('disabled', false).trigger("chosen:updated");
                //if (SubDivision_val == "") {
                //    $('#ContentPlaceHolder1_cmbSection').prop('disabled', true).trigger("chosen:updated");

                //}
                //Deselect the related options in other dropdowns after deselecting the option in SubDivision
                if (params.deselected) {
                    //debugger
                    var deselectedValues = params.deselected;

                    $('#ContentPlaceHolder1_cmbSection').each(function () {
                        //debugger;

                        var selectedValues1 = $(this).val();

                        var filteredValues = selectedValues1.filter(function (value) {
                            return !value.startsWith(deselectedValues);
                        });

                        $('#ContentPlaceHolder1_cmbSection').val(filteredValues).trigger('chosen:updated');

                    });




                    DesableFuntion();
                }
                //Filter the SubDivision dropdown based on Division 
                $('#ContentPlaceHolder1_cmbSection option').each(function () {
                    //debugger
                    var optionValue1 = $(this).val();
                    var firstLetter1 = optionValue1.substring(0, 4);
                    if (selectedValues_1 && selectedValues_1.length > 0) {
                        //debugger
                        if (selectedValues_1.includes(firstLetter1)) {
                            //debugger
                            $(this).show();
                        } else {

                            $(this).hide();
                        }
                    } else {
                        $(this).show();
                    }
                });
                $('#ContentPlaceHolder1_cmbSection').trigger('chosen:updated');
            });

            // for Section
            $('#ContentPlaceHolder1_cmbSection').on('change', function (event, params) {
                debugger
                var Section_val = $('#ContentPlaceHolder1_cmbSection').val();
                $('#ContentPlaceHolder1_hdcmbSection ').val(Section_val);
                if (Section_val != "") {
                    $('#ContentPlaceHolder1_txtlastDropdownSelected ').val("5");
                }
            });

            //Load click and save the values to localstorage 
            $("#ContentPlaceHolder1_cmdLoad").click(function () {




                var Zone_val = $('#ContentPlaceHolder1_cmbZone').val();
                $('#ContentPlaceHolder1_hdcmbZone ').val(Zone_val);
                localStorage.setItem('chosenSelectValue', JSON.stringify(Zone_val));

                var Circle_val = $('#ContentPlaceHolder1_cmbCircle').val();
                $('#ContentPlaceHolder1_hdcmbCircle ').val(Circle_val);
                localStorage.setItem('chosenSelectValue1', JSON.stringify(Circle_val));

                var Division_val = $('#ContentPlaceHolder1_cmbDivision').val();
                $('#ContentPlaceHolder1_hdcmbDivision ').val(Division_val);
                localStorage.setItem('chosenSelectValue2', JSON.stringify(Division_val));

                var SubDivision_val = $('#ContentPlaceHolder1_cmbsubdivision').val();
                $('#ContentPlaceHolder1_hdcmbSubDivision ').val(SubDivision_val);
                localStorage.setItem('chosenSelectValue3', JSON.stringify(SubDivision_val));

                var Section_val = $('#ContentPlaceHolder1_cmbSection').val();
                $('#ContentPlaceHolder1_hdcmbSection ').val(Section_val);
                localStorage.setItem('chosenSelectValue4', JSON.stringify(Section_val));








                if (Zone_val != "" && Zone_val != null) {
                    $('#ContentPlaceHolder1_txtlastDropdownSelected ').val("1");
                }
                if (Circle_val != "" && Circle_val != null) {
                    $('#ContentPlaceHolder1_txtlastDropdownSelected ').val("2");
                }
                if (Division_val != "" && Division_val != null) {
                    $('#ContentPlaceHolder1_txtlastDropdownSelected ').val("3");
                }
                if (SubDivision_val != "" && SubDivision_val != null ) {
                    $('#ContentPlaceHolder1_txtlastDropdownSelected ').val("4");
                }
                if (Section_val != "" && Section_val != null) {
                    $('#ContentPlaceHolder1_txtlastDropdownSelected ').val("5");
                }




            });

            //Reset the option from chosen select and local storage
            $("#ContentPlaceHolder1_cmdReset").click(function () {
                debugger;
                //var Session_OfficeCode = $('#ContentPlaceHolder1_txtSessionoffcode').val();
                //if (Session_OfficeCode != "3") {
                //    localStorage.clear();
                //    $('#ContentPlaceHolder1_cmbsubdivision').val('').trigger('chosen:updated');
                //    $('#ContentPlaceHolder1_cmbSection').val('').trigger('chosen:updated');

                //    $('#ContentPlaceHolder1_cmbSection').prop('disabled', true).trigger('chosen:updated');
                //} else {
                //    localStorage.clear();
                //    $('#ContentPlaceHolder1_cmbZone').val('').trigger('chosen:updated');
                //    $('#ContentPlaceHolder1_cmbCircle').val('').trigger('chosen:updated');
                //    $('#ContentPlaceHolder1_cmbDivision').val('').trigger('chosen:updated');
                //    $('#ContentPlaceHolder1_cmbsubdivision').val('').trigger('chosen:updated');
                //    $('#ContentPlaceHolder1_cmbSection').val('').trigger('chosen:updated');

                //    //$('#ContentPlaceHolder1_cmbZone').prop('disabled', true).trigger('chosen:updated');
                //    $('#ContentPlaceHolder1_cmbCircle').prop('disabled', true).trigger('chosen:updated');
                //    $('#ContentPlaceHolder1_cmbDivision').prop('disabled', true).trigger('chosen:updated');
                //    $('#ContentPlaceHolder1_cmbsubdivision').prop('disabled', true).trigger('chosen:updated');
                //    $('#ContentPlaceHolder1_cmbSection').prop('disabled', true).trigger('chosen:updated');
                //}
                $('#hdcmbZone').val('');
                $('#hdcmbCircle').val('');
                $('#hdcmbDivision').val('');
                $('#hdcmbSubDivision').val('');
                $('#hdcmbSection').val('');
                $('#txtlastDropdownSelected').val('');
                localStorage.clear();
                window.location.href = 'UserView.aspx';
            });

            DesableFuntion();
        });
        function DesableFuntion() {
            debugger;
            var Zone_val = $('#ContentPlaceHolder1_cmbZone').val();
            var Circle_val = $('#ContentPlaceHolder1_cmbCircle').val();
            var Division_val = $('#ContentPlaceHolder1_cmbDivision').val();
            var SubDivision_val = $('#ContentPlaceHolder1_cmbsubdivision').val();
            var Section_val = $('#ContentPlaceHolder1_cmbSection').val();

            //$('#ContentPlaceHolder1_cmbZone').prop('disabled', true).trigger('chosen:updated');
            //$('#ContentPlaceHolder1_cmbCircle').prop('disabled', false).trigger('chosen:updated');
            //$('#ContentPlaceHolder1_cmbDivision').prop('disabled', false).trigger('chosen:updated');
            //$('#ContentPlaceHolder1_cmbsubdivision').prop('disabled', false).trigger('chosen:updated');
            //$('#ContentPlaceHolder1_cmbSection').prop('disabled', false).trigger('chosen:updated');

            var Session_OfficeCode = $('#ContentPlaceHolder1_txtSessionoffcode').val();

            if (Session_OfficeCode != "3") {


                //$('#ContentPlaceHolder1_cmbZone').prop('disabled', true).trigger('chosen:updated');
                $('#ContentPlaceHolder1_cmbCircle').prop('disabled', true).trigger('chosen:updated');
                $('#ContentPlaceHolder1_cmbDivision').prop('disabled', true).trigger('chosen:updated');

                if (SubDivision_val == "") {
                    $('#ContentPlaceHolder1_cmbSection').prop('disabled', true).trigger('chosen:updated');
                }



            } else {


                if (Zone_val == "") {
                    $('#ContentPlaceHolder1_cmbCircle').prop('disabled', true).trigger('chosen:updated');
                    $('#ContentPlaceHolder1_cmbDivision').prop('disabled', true).trigger('chosen:updated');
                    $('#ContentPlaceHolder1_cmbsubdivision').prop('disabled', true).trigger('chosen:updated');
                    $('#ContentPlaceHolder1_cmbSection').prop('disabled', true).trigger('chosen:updated');
                } else if (Circle_val == "") {
                    $('#ContentPlaceHolder1_cmbDivision').prop('disabled', true).trigger('chosen:updated');
                    $('#ContentPlaceHolder1_cmbsubdivision').prop('disabled', true).trigger('chosen:updated');
                    $('#ContentPlaceHolder1_cmbSection').prop('disabled', true).trigger('chosen:updated');
                } else if (Division_val == "") {
                    $('#ContentPlaceHolder1_cmbsubdivision').prop('disabled', true).trigger('chosen:updated');
                    $('#ContentPlaceHolder1_cmbSection').prop('disabled', true).trigger('chosen:updated');
                } else if (SubDivision_val == "") {
                    $('#ContentPlaceHolder1_cmbSection').prop('disabled', true).trigger('chosen:updated');
                }
            }




        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <%--<div>--%>
    <div class="container-fluid">
        <!-- BEGIN PAGE HEADER-->
        <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                <h3 class="page-title">User View</h3>
                <%--  <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help"><i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>--%>
                <a style="margin-left: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>
                <ul class="breadcrumb" style="display: none">

                    <li class="pull-right search-wrap">
                        <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="appendedInputButton" type="text">
                                <button class="btn" type="button"><i class="icon-search"></i></button>
                            </div>
                        </form>
                    </li>
                </ul>
                <!-- END PAGE TITLE & BREADCRUMB-->
            </div>
        </div>

        <div class="row-fluid">

            <div style="display: none;">
                <asp:TextBox ID="txtlastDropdownSelected" runat="server" Style="display: none" />
                <asp:TextBox ID="txtSessionoffcode" runat="server" Style="display: none" />
            </div>
            <div class="span12">
                <!-- BEGIN SAMPLE FORMPORTLET-->
                <div class="widget blue">
                    <div class="widget-title">
                        <h4><i class="icon-reorder"></i>User View</h4>
                        <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                        </span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-body form">
                            <div style="float: right">
                                <div class="span2">
                                    <asp:Button ID="cmdNew" runat="server" Text="New User"
                                        CssClass="btn btn-primary" OnClick="cmdNew_Click" /><br />
                                </div>
                            </div>

                            <div class="span5">
                                <div class="control-group">
                                    <label class="control-label">Zone</label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:DropDownList ID="cmbZone" runat="server" CssClass="chosen-select" SelectionMode="Multiple" multiple="true">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="hdcmbZone" runat="server" Style="display: none" />
                                        </div>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Circle Name</label>
                                    <div class="controls">
                                        <div class="input-append">

                                            <asp:DropDownList ID="cmbCircle" runat="server" CssClass="chosen-select" multiple="true">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="hdcmbCircle" runat="server" Style="display: none" />
                                        </div>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Division Name</label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:DropDownList ID="cmbDivision" runat="server" CssClass="chosen-select" multiple="true">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="hdcmbDivision" runat="server" Style="display: none" />

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="span5">

                                <div class="control-group">
                                    <label class="control-label">Sub Division Name</label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:DropDownList ID="cmbsubdivision" runat="server" CssClass="chosen-select" multiple="true">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="hdcmbSubDivision" runat="server" Style="display: none" />
                                        </div>
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Section Name</label>
                                    <div class="controls">
                                        <div class="input-append">
                                            <asp:DropDownList ID="cmbSection" runat="server" CssClass="chosen-select" multiple="true">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdfCirle" runat="server" />
                                            <asp:HiddenField ID="hdfDivision" runat="server" />
                                            <asp:HiddenField ID="hdfSubdivision" runat="server" />
                                            <asp:HiddenField ID="hdfSection" runat="server" />
                                        </div>
                                        <asp:TextBox ID="hdcmbSection" runat="server" Style="display: none" />
                                    </div>
                                </div>
                            </div>
                            <div class="span5"></div>


                            <div class="span25">
                                <div class="text-center">

                                    <asp:Button ID="cmdLoad" runat="server" type="button" Text="Load" CssClass="btn btn-primary" OnClick="cmdLoad_Click" />
                                    <asp:Button ID="cmdReset" runat="server" Text="Reset" UseSubmitBehavior="false" TabIndex="11" OnClientClick="return false;" CssClass="btn btn-warning" type="button" />

                                </div>

                            </div>
                            <div class="span25"></div>
                            <div class="widget blue">
                                <div class="widget-body">

                                    <div style="background-color: #d4d4d4; width: 100%; margin-bottom: 20px; margin-left: 0px" class="span25">
                                        <div class="tabs" style="width: 100% !important;">
                                            <div style="float: left!important">
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                                <asp:Button ID="cmdActiveUser" CssClass="btn btn-danger tab-button" Text="Active User" runat="server" OnClick="cmdActiveUser_click" />
                                                <asp:Button ID="cmdInActiveUser" CssClass="btn btn-danger tab-button" Text="Inactive User" runat="server" OnClick="cmdInActiveUser_click" />
                                            </div>
                                            <div style="float: right!important">
                                                <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-info tab-button" OnClick="Export_click" Style="" />
                                                <%--   <asp:Button ID="cmdLoad" runat="server" type="button" Text="Load" CssClass="btn btn-primary" OnClick="cmdLoad_Click" />
                                    <asp:Button ID="cmdReset" runat="server" Text="Reset" UseSubmitBehavior="false" TabIndex="11" OnClientClick="return false;" CssClass="btn btn-warning" type="button" />--%>
                                            </div>
                                        </div>
                                    </div>


                                    <div style="overflow-x: auto!important; width: 100%!important;" class="">

                                        <%--                                <label for="pageSizeDropdown">Page Size:</label>
                                <asp:DropDownList ID="pageSizeDropdown" runat="server" onchange="pageSizeDropdownChanged()">
                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                </asp:DropDownList>

                                <input id="Search" type="text" runat="server" onkeydown="grdUser_RowCommand"  placeholder="Search.." />--%>


                                        <asp:GridView ID="grdUser"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="True"
                                            EmptyDataText="No Records Found"
                                            OnRowDataBound="grdUser_RowDataBound"
                                            OnRowCommand="grdUser_RowCommand"
                                            CssClass="table table-striped table-bordered table-advance table-hover"
                                            runat="server"
                                            ShowFooter="false">

                                            <PagerStyle CssClass="gvPagerCss" />
                                            <HeaderStyle CssClass="both" />


                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="US_ID" HeaderText="ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("US_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="ZONE_NAME" HeaderText="Zone NAME" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblZoneName" runat="server" Text='<%# Bind("zone") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="CM_CIRCLE_NAME" HeaderText="CIRCLE NAME" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCircleName" runat="server" Text='<%# Bind("circle") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="DIV_NAME" HeaderText="DIVISION NAME" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDivisionName" runat="server" Text='<%# Bind("division") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="SD_SUBDIV_NAME" HeaderText="SUBDIVISION NAME" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubDivisionName" runat="server" Text='<%# Bind("subdivision") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="OM_NAME" HeaderText="SECTION NAME" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSectionName" runat="server" Text='<%# Bind("section") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField AccessibleHeaderText="US_FULL_NAME" HeaderText="Name" SortExpression="US_FULL_NAME">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFullName" runat="server" Text='<%# Bind("US_FULL_NAME") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                    <%-- <FooterTemplate>
                                                                <asp:TextBox ID="txtsFullName" runat="server" Width="120px" placeholder="Enter Name" ToolTip="Enter Name to Search"></asp:TextBox>
                                                            </FooterTemplate>--%>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="US_EMAIL" HeaderText="Email Id">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("US_EMAIL") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                    <%-- <FooterTemplate>
                                                                <asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/img/Manual/search.png" Height="25px" ToolTip="Search" CommandName="search" TabIndex="9" />
                                                            </FooterTemplate>--%>
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="US_LG_NAME" HeaderText="User Id">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUSLGName" runat="server" Text='<%# Bind("us_lg_name") %>' Style="word-break: break-all;" Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="US_MOBILE_NO" HeaderText="Mobile No">

                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("US_MOBILE_NO") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="RO_NAME" HeaderText="Role Name" SortExpression="RO_NAME">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRole" runat="server" Text='<%# Bind("RO_NAME") %>' Style="word-break: break-all;" Width="120px"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="US_DESG_ID" HeaderText="Designation" SortExpression="US_DESG_ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Bind("US_DESG_ID") %>' Style="word-break: break-all;" Width="100px"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtsDesignation" runat="server" Width="100px" placeholder="Enter Designation" ToolTip="Enter Designation to Search" Visible="false"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField AccessibleHeaderText="OFF_NAME" HeaderText="Location" SortExpression="OFF_NAME">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOfficeName" runat="server" Text='<%# Bind("OFF_NAME") %>' Style="word-break: break-all;" Width="200px"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtOfficeName" runat="server" Width="150px" placeholder="Enter Designation" ToolTip="Enter Designation to Search" Visible="false"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:ImageButton ID="imgBtnEdit" Title="Click To Edit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png" CommandName="create"
                                                                Width="12px" />
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%# Eval("us_status1") %>'></asp:Label>
                                                        <center>
                                                            <asp:ImageButton Visible="false" ID="imgDeactive" runat="server" ImageUrl="~/img/Manual/Disable.png" CommandName="status"
                                                                ToolTip="Click to Enable User" OnClientClick="return ConfirmStatus('Enable');" Width="10px" />
                                                            <asp:ImageButton Visible="false" ID="imgActive" runat="server" ImageUrl="~/img/Manual/Enable.gif" CommandName="status"
                                                                ToolTip="Click to Disable User" OnClientClick="return ConfirmStatus('Disable');" />
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delete" Visible="false">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:ImageButton ID="imbBtnDelete" runat="server" Height="12px" ImageUrl="~/Styles/images/delete64x64.png"
                                                                Width="12px" OnClientClick="return confirm ('Are you sure, you want to delete');"
                                                                CausesValidation="false" />
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>

                                            <PagerSettings FirstPageText="first" LastPageText="last" Mode="NumericFirstLast" />


                                        </asp:GridView>
                                    </div>
                                </div>

                            </div>

                            <ajax:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnshow" CancelControlID="cmdClose"
                                PopupControlID="pnlControls" BackgroundCssClass="modalBackground" />

                            <div style="width: 100%; vertical-align: middle;" align="center">
                                <div>
                                    <asp:Button ID="btnshow" runat="server" Text="Button" Style="display: none;" />
                                </div>
                                <asp:Panel ID="pnlControls" runat="server" BackColor="White" Height="200px" Width="550px">
                                    <div class="widget blue">
                                        <div class="widget-title">
                                            <h4>Give Reason </h4>
                                            <div class="space20"></div>
                                            <%--<div class="row-fluid">--%>
                                            <div class="span1"></div>
                                            <div class="space20">
                                                <div class="span1"></div>

                                                <div class="span5">

                                                    <div class="control-group" style="font-weight: bold">
                                                        <label class="control-label">Reason<span class="Mandotary"> *</span></label>

                                                        <div class="controls">
                                                            <div class="input-append" align="center">

                                                                <asp:TextBox ID="txtReason" runat="server" MaxLength="500" TabIndex="4" TextMode="MultiLine" Style="resize: none"
                                                                    onkeyup="javascript:ValidateTextlimit(this,100)"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div align="center">
                                                        <div class="control-group" style="font-weight: bold">
                                                            <label class="control-label">Effect From<span class="Mandotary"> *</span></label>
                                                            <div class="controls">
                                                                <div class="input-append" align="center">

                                                                    <asp:TextBox ID="txtEffectFrom" runat="server" MaxLength="10" TabIndex="3"></asp:TextBox>
                                                                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server"
                                                                        CssClass="cal_Theme1" TargetControlID="txtEffectFrom" Format="dd/MM/yyyy">
                                                                    </ajax:CalendarExtender>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="span5">
                                                        <div class="control-group" style="font-weight: bold">

                                                            <div class="controls">
                                                                <div class="input-append">

                                                                    <div class="span10">
                                                                        <asp:Button ID="cmdSubmit" runat="server" CssClass="btn btn-primary"
                                                                            OnClick="cmdSubmit_Click" TabIndex="10" Text="Submit" />
                                                                    </div>
                                                                    <div class="span1">
                                                                        <asp:Button ID="cmdClose" runat="server" CssClass="btn btn-primary"
                                                                            TabIndex="10" Text="Close" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <div class="space20" align="center">

                                                        <div class="form-horizontal" align="center">
                                                            <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
                                                        </div>


                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="space20"></div>
                                        <div class="space20"></div>

                                    </div>
                                </asp:Panel>
                            </div>

                        </div>
                    </div>
                </div>

            </div>
        </div>
        <!-- END PAGE HEADER-->

        <!-- BEGIN PAGE CONTENT-->



        <!-- END PAGE CONTENT-->
    </div>
    <%--    </div>--%>
    <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Web Page Can be used To View Existing User Details and To Add New User
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>New User Can Be Added By Clicking New User Button
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>User Can Be Enabled/Disabled By clicking Status Radio Button
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>User details can be viewed by selecting multiple loactions. 
                    </p>
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>Active and Inactive user details can be filtered by using Active User and Inactive User buttons
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

</asp:Content>
