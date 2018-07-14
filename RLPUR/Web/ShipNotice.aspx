<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShipNotice.aspx.cs" Inherits="RLPUR.Web.ShipNotice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>出货通知单</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Page">
            <div class="Title">
                <asp:Label ID="PageTitle" runat="server" Text="出货通知单"></asp:Label>
            </div>
            <div class="Operating">
                <ul>
                    <li class="Caption">工令号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="ORDNO" runat="server"></asp:TextBox>
                    </li>
                    <li class="Caption">需求日期
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="RDate" runat="server" Enabled="False"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Caption">客户代码
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="CustNo" runat="server" Enabled="False"></asp:TextBox>
                    </li>
                    <li class="Caption">客户名称
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="CustName" runat="server" Enabled="False"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Caption">出货单号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="ShipNo" runat="server" Enabled="False"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Button">
                        <asp:Button ID="OKButton" runat="server" Text="查询"
                            OnClick="OKButton_Click" />
                        <asp:Button ID="SaveButton" runat="server" Text="保存"
                            OnClick="SaveButton_Click" />
                        <%--<asp:Button ID="PrintButton" runat="server" Text="打印"
                            CssClass="Highlighted" OnClick="PrintButton_Click" />--%>
                        <asp:Button ID="TransferButton" runat="server" Text="转出货界面"
                            CssClass="Highlighted" OnClick="TransferButton_Click" />
                    </li>
                </ul>
            </div>
            <div class="List">
                <asp:GridView ID="List" runat="server" EmptyDataText="<%$ Resources:iiWeb, EmptyData %>"
                    EnableModelValidation="True" DataKeyNames="seq" OnRowDataBound="List_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input type="checkbox" id="RowCheckAll" runat="server" class="CheckAll" title="勾选" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" id="RowCheck" runat="server" class="Check" title="勾选" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="seq" HeaderText="序号" />
                        <asp:BoundField DataField="drawno" HeaderText="图号" />
                        <asp:BoundField DataField="itemno" HeaderText="名称" />
                        <asp:BoundField DataField="ordqty" HeaderText="需求数量" />
                        <asp:BoundField DataField="um" HeaderText="单位" />
                        <asp:BoundField DataField="" HeaderText="未出货数量" />
                        <asp:TemplateField HeaderText="计划出货数量">
                            <ItemTemplate>
                                <asp:TextBox ID="shipqplan" runat="server" Width="90%" CssClass="Integer"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="出货日期">
                            <ItemTemplate>
                                <asp:TextBox ID="shipdate" runat="server" Width="90%" CssClass="Date"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
