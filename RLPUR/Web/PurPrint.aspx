<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurPrint.aspx.cs" Inherits="RLPUR.Web.PurPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>请购单打印</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Page">
            <div class="Title">
                <asp:Label ID="PageTitle" runat="server" Text="请购单打印"></asp:Label>
            </div>
            <div class="Operating">
                <ul>
                    <li class="Caption">请购单号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="PRNoFrom" runat="server"></asp:TextBox>
                    </li>
                    <li class="CaptionSmall">~
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="PRNoTo" runat="server"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Caption">厂商代码
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="VenNo" runat="server"></asp:TextBox>
                    </li>
                    <li class="CaptionSmall"></li>
                    <li class="Content">
                        <asp:RadioButtonList ID="PrType" runat="server" RepeatDirection="Horizontal" Width="246px">
                            <asp:ListItem Selected="True" Value="N">一般请购</asp:ListItem>
                            <asp:ListItem Value="F">委外请购</asp:ListItem>
                            <asp:ListItem Value="S">材料请购</asp:ListItem>
                        </asp:RadioButtonList>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Button">
                        <asp:Button ID="OKButton" runat="server" Text="打印" />
                        <asp:Button ID="CancelButton" runat="server" Text="取消"
                            OnClick="CancelButton_Click" />
                    </li>
                </ul>
            </div>
        </div>
    </form>
</body>
</html>
