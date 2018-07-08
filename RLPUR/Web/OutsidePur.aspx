<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutsidePur.aspx.cs" Inherits="RLPUR.Web.OutsidePur" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>委外请购</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Page">
            <div class="Title">
                <asp:Label ID="PageTitle" runat="server" Text="委外请购"></asp:Label>
            </div>
            <div class="Operating">
                <ul>
                    <li class="Caption">工令号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="ORDNO" runat="server"></asp:TextBox>
                    </li>
                    <li class="Caption">图号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="DRAWNO" runat="server" Enabled="False"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Caption">工件类型
                    </li>
                    <li class="Content">
                        <asp:DropDownList ID="BomType" runat="server"></asp:DropDownList>
                    </li>
                    <li class="Caption">请购单号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="PRNo" runat="server"></asp:TextBox>
                    </li>
                    <li class="CaptionSmall">状态
                    </li>
                    <li class="ContentSmall">
                        <asp:TextBox ID="PRStatus" runat="server" Enabled="False"></asp:TextBox>
                    </li>
                    <li class="LineFeed"></li>
                    <li class="Button">
                        <asp:Button ID="OKButton" runat="server" Text="查询"
                            OnClick="OKButton_Click" />
                        <asp:Button ID="CancelButton" runat="server" Text="取消"
                            OnClick="CancelButton_Click" />
                        <asp:Button ID="CreateButton" runat="server" Text="<%$ Resources:iiWeb, CreateButton %>"
                            CssClass="Highlighted" OnClick="CreateButton_Click" />
                        <asp:Button ID="DeleteButton" runat="server" Text="<%$ Resources:iiWeb, DeleteButton %>"
                            CssClass="Highlighted" OnClick="DeleteButton_Click" />
                        <asp:Button ID="SaveButton" runat="server" Text="保存"
                            CssClass="Highlighted" OnClick="SaveButton_Click" />
                    </li>
                </ul>
            </div>

            <div class="List">
                <asp:Label ID="Label1" runat="server" Text="工令BOM明细" Font-Bold="True" Font-Size="15px" ForeColor="Red"></asp:Label>
                <asp:GridView ID="List" runat="server" EmptyDataText="<%$ Resources:iiWeb, EmptyData %>" AllowPaging="false"
                    EnableModelValidation="True" DataKeyNames="bomseq" OnRowDataBound="List_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="勾选">
                            <%--<HeaderTemplate>
                                <input type="checkbox" id="RowCheckAll" runat="server" class="CheckAll" title="勾选" />
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <input type="checkbox" id="RowCheck" runat="server" class="Check" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="<%$ Resources:iiWeb, EditCaption %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="RowEditButton" runat="server" CssClass="ImageButton ImageButtonEdit"
                                    ToolTip='<%# Eval("avend").ToString().Trim() %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="bomseq" HeaderText="序号" />
                        <asp:BoundField DataField="bompno" HeaderText="工件号" />
                        <asp:BoundField DataField="bomnam" HeaderText="名称" />
                        <asp:BoundField DataField="bommat" HeaderText="材料" />
                        <asp:BoundField DataField="bomspc" HeaderText="下料规格" />
                        <asp:BoundField DataField="bomreq" HeaderText="需求数量" />
                        <asp:BoundField DataField="bomuit" HeaderText="单位" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="List">
                <asp:Label ID="DetailLabel" runat="server" Text="请购单明细" Font-Bold="True" Font-Size="15px" ForeColor="Red"></asp:Label>
                <asp:GridView ID="PRList" runat="server" EmptyDataText="<%$ Resources:iiWeb, EmptyData %>" AllowPaging="false"
                    EnableModelValidation="True" DataKeyNames="prlseq" OnRowDataBound="PRList_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="急件">
                            <%--       <HeaderTemplate>
                                <input type="checkbox" id="RowCheckAll" runat="server" class="CheckAll" title="<%$ Resources:iiWeb, DeleteButton %>" />
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <input type="checkbox" id="UrgentCheck" runat="server" class="Check" title="急件" />
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
                        <asp:TemplateField HeaderText="序号" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Label ID="SEQ" runat="server" Text="<%# Container.DataItemIndex +1%>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="prlseq" HeaderText="序号" />--%>
                        <asp:BoundField DataField="prltno" HeaderText="工件号" />
                        <asp:BoundField DataField="bomnam" HeaderText="名称" />
                        <asp:BoundField DataField="bommat" HeaderText="材料" />
                        <asp:BoundField DataField="prlrule" HeaderText="下料规格" />
                        <asp:BoundField DataField="prlsoseq" HeaderText="工令序号" />
                        <asp:BoundField DataField="bomreq" HeaderText="需求数量" />
                        <asp:BoundField DataField="prlum" HeaderText="单位" />
                        <asp:TemplateField HeaderText="工站">
                            <ItemTemplate>
                                <asp:TextBox ID="prlstation" runat="server" Text='<%# Eval("prlstation").ToString().Trim() %>' Width="90%" CssClass=""></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="请购数量">
                            <ItemTemplate>
                                <asp:TextBox ID="PRLQTY" runat="server" Text='<%# Eval("PRLQTY").ToString().Trim() %>' Width="90%" CssClass="Required Integer"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="需求日期">
                            <ItemTemplate>
                                <asp:TextBox ID="PRLPDTE" runat="server" Text='<%# Eval("PRLPDTE").ToString().Trim() %>' Width="90%" CssClass="Required Date"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
