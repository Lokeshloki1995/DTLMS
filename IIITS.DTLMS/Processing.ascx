<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Processing.ascx.cs" Inherits="IIITS.DTLMS.Processing" %>

<script type="text/javascript">
    // Get the instance of PageRequestManager.
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    // Add initializeRequest and endRequest
    prm.add_initializeRequest(prm_InitializeRequest);
    prm.add_endRequest(prm_EndRequest);

    // Called when async postback begins
    function prm_InitializeRequest(sender, args) {
        // get the divImage and set it to visible
        var panelProg = $get('divImage');
        panelProg.style.display = '';
        // reset label text
                                <%--var lbl = $get('<%= this.lblText.ClientID %>');
                 lbl.innerHTML = '';--%>

                                // Disable button that caused a postback
                                $get(args._postBackElement.id).disabled = true;
                            }

                            // Called when async postback ends
                            function prm_EndRequest(sender, args) {
                                // get the divImage and hide it again
                                var panelProg = $get('divImage');
                                panelProg.style.display = 'none';

                                // Enable button that caused a postback
                                //$get(sender._postBackSettings.sourceElement.id).disabled = false;
                            }
</script>
