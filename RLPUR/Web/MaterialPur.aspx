<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialPur.aspx.cs" Inherits="RLPUR.Web.MaterialPur" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>材料请购</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Page">
            <div class="Title">
                <asp:Label ID="PageTitle" runat="server" Text="材料请购"></asp:Label>
            </div>
            <div class="Operating">
                <ul>
                    <li class="Caption">请购单号
                    </li>
                    <li class="Content">
                        <asp:TextBox ID="PRNo" runat="server"></asp:TextBox>
                    </li>
                    <li class="CaptionSmall">仓库
                    </li>
                    <li class="ContentSmall">
                        <asp:TextBox ID="prlwhs" runat="server" Enabled="False" Text="LM" OnTextChanged="prlwhs_TextChanged"></asp:TextBox>
                    </li>
                    <li class="ContentSmall">
                        <asp:Label ID="Label1" runat="server" Text="材料库"></asp:Label>
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
                        <%--<asp:Button ID="CancelButton" runat="server" Text="取消"
                            OnClick="CancelButton_Click" />--%>
                        <asp:Button ID="DeleteButton" runat="server" Text="<%$ Resources:iiWeb, DeleteButton %>"
                            CssClass="Highlighted" OnClick="DeleteButton_Click" />
                        <asp:Button ID="SaveButton" runat="server" Text="保存"
                            CssClass="Highlighted" OnClick="SaveButton_Click" />
                    </li>
                </ul>
            </div>
            <div class="List">
                <div style="">
                    <asp:Button ID="CreateRow" runat="server" Text="添加行" OnClick="CreateRow_Click" />
                    <asp:Button ID="DeleteRow" runat="server" Text="删除行" OnClick="DeleteRow_Click" />
                </div>
                <asp:GridView ID="List" runat="server" EmptyDataText="<%$ Resources:iiWeb, EmptyData %>"
                    EnableModelValidation="True" DataKeyNames="prlseq" PageSize="100" AllowPaging="false" OnRowDataBound="List_RowDataBound" ShowFooter="true" OnPreRender="List_PreRender" OnLoad="List_Load">
                    <Columns>
                        <asp:TemplateField HeaderText="删除" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <input type="checkbox" id="RowCheck" runat="server" class="Check" title="<%$ Resources:iiWeb, DeleteButton %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="序号" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Label ID="SEQ" runat="server" Text="<%# Container.DataItemIndex +1%>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="料号" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%#Eval("prltno") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="prltno" runat="server" Width="90%" CssClass="Required"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="名称" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%#Eval("prloutno") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="prloutno" runat="server" Width="90%" CssClass="Required"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="规格" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <%#Eval("prlrule") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="prlrule" runat="server" Width="90%" CssClass="Required"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="材质" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <%#Eval("prlpicno") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="prlpicno" runat="server" Width="90%" CssClass="Required"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="单位" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <%#Eval("prlum") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="prlum" runat="server" Width="90%" CssClass="Required"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="数量" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <%#Eval("prlqty") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="prlqty" runat="server" Width="90%" CssClass="Required Integer"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="需求日期" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <%#Eval("prlpdte") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="prlpdte" runat="server" Width="90%" CssClass="Required Date"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="用途" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <%#Eval("prlstation") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="prlstation" runat="server" Width="90%" CssClass="Required"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="备注" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <%#Eval("prlmrk") %>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="prlmrk" runat="server" Width="90%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
