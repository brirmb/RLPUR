<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurQuery.aspx.cs" Inherits="RLPUR.Web.PurQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>请购查询</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Page">
            <div class="Title">
                <asp:Label ID="PageTitle" runat="server" Text="请购查询"></asp:Label>
            </div>
            <div class="Operating">
                <ul>
                    <li class="Caption">请购单号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="PRNo" runat="server"></asp:TextBox>
                    </li>
                    <li class="Caption">~
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="ORDNO" runat="server"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Caption">工令号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="PRStatus" runat="server"></asp:TextBox>
                    </li>
                    <li class="Caption">~
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="PRType" runat="server"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Caption">请购日期
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
                    <li class="Caption">厂商
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </li>
                    <li class="Caption">~
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Caption">图号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    </li>
                    <li class="Caption">~
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Caption">工件号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                    </li>
                    <li class="Caption">工件名
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Caption">总金额
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="TextBox7" runat="server" Enabled="False"></asp:TextBox>(RMB/元)
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Button">
                        <asp:Button ID="OKButton" runat="server" Text="查询"
                            OnClick="OKButton_Click" />
                        <asp:Button ID="CancelButton" runat="server" Text="取消"
                            OnClick="CancelButton_Click" />
                        <asp:Button ID="ExcelButton" runat="server" Text="导出"
                            CssClass="Highlighted" />
                    </li>
                </ul>
            </div>
            <div class="List">
                <asp:GridView ID="List" runat="server" EmptyDataText="<%$ Resources:iiWeb, EmptyData %>"
                    EnableModelValidation="True" DataKeyNames="prlseq" OnRowDataBound="List_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="prlseq" HeaderText="请购单号" />
                        <asp:BoundField DataField="prlseq" HeaderText="序号" />
                        <asp:BoundField DataField="prlseq" HeaderText="工令号" />
                        <asp:BoundField DataField="prlseq" HeaderText="图号" />
                        <asp:BoundField DataField="prltno" HeaderText="工件号" />
                        <asp:BoundField DataField="bomnam" HeaderText="工件名称" />
                        <asp:BoundField DataField="bommat" HeaderText="材质" />
                        <asp:BoundField DataField="prlstation" HeaderText="工站" />
                        <asp:BoundField DataField="PRLQTY" HeaderText="请购数量" />
                        <asp:BoundField DataField="prlvnd" HeaderText="厂商代码" />
                        <asp:BoundField DataField="prlvndm" HeaderText="厂商名称" />
                        <asp:BoundField DataField="prlpacst" HeaderText="单价" />
                        <asp:BoundField DataField="prlum" HeaderText="单位" />
                        <asp:BoundField DataField="prlum" HeaderText="金额" />
                        <asp:BoundField DataField="prlcur" HeaderText="币别" />
                        <asp:BoundField DataField="prlum" HeaderText="请购日期" />
                        <asp:BoundField DataField="PRLPDTE" HeaderText="交货期限" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
