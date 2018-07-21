<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShipQuery.aspx.cs" Inherits="RLPUR.Web.ShipQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>出货查询</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Page">
            <div class="Title">
                <asp:Label ID="PageTitle" runat="server" Text="出货查询"></asp:Label>
            </div>
            <div class="Operating">
                <ul>
                    <li class="Caption">工令号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="ORDNO" runat="server"></asp:TextBox>
                    </li>
                    <li class="Caption"></li>
                    <li class="Content"></li>
                    <li class="LineFeed"></li>
                    <li class="Caption">客户代码
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="CUSTNO" runat="server" CssClass=""></asp:TextBox><asp:LinkButton ID="selCust" Style="display: inline-block;" runat="server" CssClass="ImageButton ImageButtonDetail" ToolTip="选择客户"></asp:LinkButton>
                    </li>
                    <li class="Caption">客户名称
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="CUSTNAME" runat="server" CssClass=""></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Caption">日期
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="DateFrom" runat="server" CssClass="Date"></asp:TextBox>
                    </li>
                    <li class="Caption">~
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="DateTo" runat="server" CssClass="Date"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Button">
                        <asp:Button ID="OKButton" runat="server" Text="查询"
                            OnClick="OKButton_Click" />
                        <asp:Button ID="CancelButton" runat="server" Text="取消"
                            OnClick="CancelButton_Click" />
                        <asp:Button ID="ExportButton" runat="server" Text="导出"
                            CssClass="Highlighted" OnClick="ExportButton_Click" />
                    </li>
                </ul>
            </div>
            <div class="List">
                <asp:GridView ID="List" runat="server" EmptyDataText="<%$ Resources:iiWeb, EmptyData %>"
                    EnableModelValidation="True" DataKeyNames="shipno" OnRowDataBound="List_RowDataBound" OnPageIndexChanging="List_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="shipno" HeaderText="出货单号" />
                        <asp:BoundField DataField="shipsono" HeaderText="工令号" />
                        <asp:BoundField DataField="shipmno" HeaderText="机种" />
                        <asp:BoundField DataField="shipname" HeaderText="名称" />
                        <asp:BoundField DataField="shipunit" HeaderText="单位" />
                        <asp:BoundField DataField="shipqact" HeaderText="数量" />
                        <asp:BoundField DataField="shipdate" HeaderText="日期" />
                        <asp:BoundField DataField="shipcustno" HeaderText="客户代码" />
                        <asp:BoundField DataField="shipcustname" HeaderText="客户名称" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
