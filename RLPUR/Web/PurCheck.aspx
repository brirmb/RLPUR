<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurCheck.aspx.cs" Inherits="RLPUR.Web.PurCheck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>请购核准</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Page">
            <div class="Title">
                <asp:Label ID="PageTitle" runat="server" Text="请购核准"></asp:Label>
            </div>
            <div class="Operating">
                <ul>
                    <li class="Button">
                        <asp:Button ID="OKButton" runat="server" Text="查询"
                            OnClick="OKButton_Click" />
                        <asp:Button ID="ApproveButton" runat="server" Text="核准"
                            CssClass="Highlighted" OnClick="ApproveButton_Click" />
                        <asp:Button ID="RejectButton" runat="server" Text="否决"
                            CssClass="Highlighted" OnClick="RejectButton_Click" />
                    </li>
                </ul>
            </div>
            <div class="List">
                <asp:GridView ID="List" runat="server" EmptyDataText="<%$ Resources:iiWeb, EmptyData %>"
                    EnableModelValidation="True" DataKeyNames="PRHNO" OnRowDataBound="List_RowDataBound" OnPageIndexChanging="List_PageIndexChanging" OnSelectedIndexChanging="List_SelectedIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="详情">
                            <ItemTemplate>
                                <asp:LinkButton ID="RowDetailButton" runat="server" CssClass="ImageButton ImageButtonDetail" CommandName="Select"
                                    ToolTip='<%# Eval("PRHNO").ToString().Trim() %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PRHNO" HeaderText="请购单号" />
                        <asp:BoundField DataField="PRHCBY" HeaderText="请购人" />
                        <asp:BoundField DataField="PRHCDTE" HeaderText="日期" />
                        <asp:BoundField DataField="PRHSTAT" HeaderText="状态码" />
                        <asp:BoundField DataField="PRHTYP" HeaderText="类型" />
                        <asp:BoundField DataField="PRHSORD" HeaderText="工令号" />
                        <asp:BoundField DataField="PRHMNO" HeaderText="图号" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="List">
                <%--  <asp:Label ID="DetailLabel" runat="server" Text="请购单明细" Font-Bold="True" Font-Size="15px" ForeColor="Red"></asp:Label>--%>
                <asp:GridView ID="DetailList" runat="server" EmptyDataText="<%$ Resources:iiWeb, EmptyData %>" AllowPaging="false"
                    EnableModelValidation="True" DataKeyNames="prlseq" OnRowDataBound="DetailList_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input type="checkbox" id="RowCheckAll" runat="server" class="CheckAll" title="勾选" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" id="RowCheck" runat="server" class="Check" title="勾选" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="prlno" HeaderText="请购单号" />
                        <asp:BoundField DataField="prlseq" HeaderText="序号" />
                        <asp:BoundField DataField="prltno" HeaderText="工件号" />
                        <asp:BoundField DataField="bomnam" HeaderText="工站" />
                        <asp:BoundField DataField="bommat" HeaderText="数量" />
                        <asp:BoundField DataField="prlrule" HeaderText="单位" />
                        <asp:BoundField DataField="prlsoseq" HeaderText="需求日期" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
