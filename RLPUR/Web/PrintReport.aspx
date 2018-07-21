<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintReport.aspx.cs" Inherits="RLPUR.Web.PrintReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Dialog">
            <div class="List">
                <rsweb:ReportViewer ID="Viewer" runat="server" ShowBackButton="False" ShowFindControls="False"
                    ShowRefreshButton="False" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt" ShowPageNavigationControls="False" SizeToReportContent="True">
                </rsweb:ReportViewer>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            </div>
        </div>
    </form>
</body>
</html>
