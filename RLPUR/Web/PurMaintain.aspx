<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurMaintain.aspx.cs" Inherits="RLPUR.Web.PurMaintain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>请购维护</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Page">
            <div class="Title">
                <asp:Label ID="PageTitle" runat="server" Text="请购维护"></asp:Label>
            </div>
            <div class="Operating">
                <ul>
                    <li class="Caption">请购单号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="PRNo" runat="server"></asp:TextBox>
                    </li>
                    <li class="Caption">工令号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="ORDNO" runat="server" Enabled="False"></asp:TextBox>
                    </li>

                    <li class="CaptionSmall">状态
                    </li>
                    <li class="ContentSmall">
                        <asp:TextBox ID="PRStatus" runat="server" Enabled="False"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Caption">请购类型
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="PRType" runat="server" Enabled="False"></asp:TextBox>
                    </li>
                    <li class="Caption">图号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="DRAWNO" runat="server" Enabled="False"></asp:TextBox>
                    </li>
                    <li class="ContentSmall">
                        <asp:CheckBox ID="IsWeight" runat="server" Text="按重量结算" ForeColor="Red" />
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Button">
                        <asp:Button ID="OKButton" runat="server" Text="查询"
                            OnClick="OKButton_Click" />
                        <asp:Button ID="CancelButton" runat="server" Text="取消"
                            OnClick="CancelButton_Click" />
                        <asp:Button ID="SaveButton" runat="server" Text="保存"
                            CssClass="Highlighted" OnClick="SaveButton_Click" />
                        <asp:Button ID="PostButton" runat="server" Text="提交"
                            CssClass="Highlighted" OnClick="PostButton_Click" />
                        <%--         <asp:Button ID="PrintButton" runat="server" Text="打印"
                            OnClick="PrintButton_Click" />--%>
                    </li>
                </ul>
            </div>
            <div class="List">
                <asp:GridView ID="List" runat="server" EmptyDataText="<%$ Resources:iiWeb, EmptyData %>" AllowPaging="false"
                    EnableModelValidation="True" DataKeyNames="prlseq" OnRowDataBound="List_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="急件">
                            <%--       <HeaderTemplate>
                                <input type="checkbox" id="RowCheckAll" runat="server" class="CheckAll" title="<%$ Resources:iiWeb, DeleteButton %>" />
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <input type="checkbox" id="UrgentCheck" runat="server" class="Check" title="急件" disabled="disabled" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input type="checkbox" id="RowCheckAll" runat="server" class="CheckAll" title="勾选" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" id="RowCheck" runat="server" class="Check" title="勾选" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="序号" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Label ID="SEQ" runat="server" Text="<%# Container.DataItemIndex +1%>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="prlseq" HeaderText="序号" />
                        <asp:BoundField DataField="prltno" HeaderText="工件号" />
                        <asp:BoundField DataField="bomnam" HeaderText="工件名称" />
                        <asp:BoundField DataField="bommat" HeaderText="材质" />
                        <asp:BoundField DataField="prlstation" HeaderText="工站" />
                        <asp:BoundField DataField="prlum" HeaderText="单位" />
                        <asp:BoundField DataField="PRLQTY" HeaderText="请购数量" />
                        <asp:BoundField DataField="PRLPDTE" HeaderText="需求日期" />
                        <asp:TemplateField HeaderText="单价">
                            <ItemTemplate>
                                <asp:TextBox ID="prlpacst" runat="server" Text='<%# Eval("prlpacst").ToString().Trim() %>' Width="90%" CssClass="Float"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="厂商代码">
                            <ItemTemplate>
                                <asp:TextBox ID="prlvnd" runat="server" Text='<%# Eval("prlvnd").ToString().Trim() %>' Width="90%" CssClass="venno"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="厂商名称">
                            <ItemTemplate>
                                <asp:TextBox ID="prlvndm" runat="server" Text='<%# Eval("prlvndm").ToString().Trim() %>' Width="90%" CssClass="venname"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="币别">
                            <ItemTemplate>
                                <asp:TextBox ID="prlcur" runat="server" Text='<%# Eval("prlcur").ToString().Trim() %>' Width="90%" CssClass="vencurr"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="" HeaderText="状态" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
