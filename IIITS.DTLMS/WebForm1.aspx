<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="IIITS.DTLMS.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="controls">
                <div class="input-append">
                    <asp:FileUpload ID="fupUpload" runat="server" />
                    <asp:Button ID="cmdUpload" class="btn btn-primary" runat="server" Text="Upload" OnClick="cmdUpload_click" />
                </div>
            </div>
    </div>
    </form>
</body>
</html>
