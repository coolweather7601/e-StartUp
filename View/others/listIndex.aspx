<%@ Page Language="C#" MasterPageFile="../Master/MasterPage.master" AutoEventWireup="true" CodeFile="listIndex.aspx.cs" Inherits="ESC_Web.Alan.listIndex" Title="List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server"> 
    <asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
    <table>
        <tbody>
            <tr style="text-align: right">
                <td>
                    BatchCard：</td>
                <td style="text-align:left;">
                     <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtSearch" runat="server"></asp:TextBox></td>
                <td>
                    Mold：</td>
                <td style="text-align:left;">
                    <asp:DropDownList ID="ddlMold" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlMold_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>            
            <tr style="text-align: right">
                <td>
                    MachineNo：</td>
                <td style="text-align:left;">
                     <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMachine" runat="server"></asp:TextBox></td>
                <td>
                    Location：</td>
                <td style="text-align:left;">
                     <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtLocation" runat="server"></asp:TextBox></td>
                <td style="text-align:left;">
                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="Serach"
                        CssClass="btn btn-success"></asp:Button></td>
            </tr>
            <tr style="text-align: right">
                <td>
                    Start date：</td>
                <td style="text-align:left;">
                     <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtStart" runat="server"></asp:TextBox><cc1:CalendarExtender ID="CalendarExtender1"
                        runat="server" Format="yyyy/MM/dd" TargetControlID="txtStart">
                    </cc1:CalendarExtender>
                </td>
                <td>
                    End date：</td>
                <td style="text-align:left;">
                     <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtEnd" runat="server"></asp:TextBox><cc1:CalendarExtender ID="CalendarExtender2"
                        runat="server" Format="yyyy/MM/dd" TargetControlID="txtEnd">
                    </cc1:CalendarExtender>
                </td>
            </tr>
        </tbody>
    </table>

        <h3> Sheet：<asp:DropDownList ID="ddlSheet" AutoPostBack="true" runat="server" onselectedindexchanged="ddlSheet_SelectedIndexChanged"></asp:DropDownList></h3>
        <p>
            <asp:GridView ID="GridView1" runat="server" BorderWidth="1px" BorderStyle="Solid" DataKeyNames="sheetID"
                BorderColor="#999999" BackColor="White" OnPageIndexChanging="GridView1_PageIndexChanging"
                AllowPaging="True" OnRowDataBound="GridView1_RowDataBound" OnRowCancelingEdit="GridView1_RowCancelingEdit"
                OnRowUpdating="GridView1_RowUpdating" OnRowEditing="GridView1_RowEditing" OnRowCommand="GridView1_RowCommand"
                GridLines="Vertical" ForeColor="Black" CellPadding="3" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="No">
                        <ItemTemplate>
                            <%# (Container.DataItemIndex+1).ToString()%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BatchCardNo">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='' ID="lblBatch"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="time" HeaderText="LogTime" ReadOnly="True"></asp:BoundField>
                    <asp:TemplateField HeaderText="Change Kind">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='' ID="lblStatus"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="tester" HeaderText="MachineNo" ReadOnly="True"></asp:BoundField>
                    <asp:BoundField DataField="location" HeaderText="Location" ReadOnly="True"></asp:BoundField>
                    <asp:BoundField DataField="fixEngineer" HeaderText="FixEngineer" ReadOnly="True"></asp:BoundField>
                    <asp:BoundField DataField="fixTime" HeaderText="FixTime" ReadOnly="True"></asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Remark">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlDescribe" runat="server">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("remark_desc") %>' ID="Label0"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Log" Visible="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnLog1" CommandName="log" CommandArgument='<%# Eval("sheetID")%>'
                                ImageUrl="../../images/icon_log.png" runat="server" Width="18px" ToolTip="Log" />
                        </ItemTemplate>
                    </asp:TemplateField>  
                                      
                    <asp:TemplateField HeaderText="Edit" Visible="true">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn1" CommandName="view" CommandArgument='<%# Eval("sheetID") %>'
                                ImageUrl="../../images/icon_edit.png" runat="server" Width="18px" ToolTip="Edit" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Remark" Visible="false">
                        <EditItemTemplate>
                            <asp:LinkButton ID="lbUpdate" runat="server" CommandName="Update">更新</asp:LinkButton>
                            |
                            <asp:LinkButton ID="lbCancelUpdate" runat="server" CommandName="Cancel">取消</asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="btn17" CommandName="Edit" CommandArgument='<%# Eval("sheetID") %>'
                                ImageUrl="../../images/icon_fix.ico" runat="server" Width="18px" ToolTip="Remark"/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Delete" Visible="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="btn2" CommandName="del" OnClientClick="return confirm('確定刪除資料?');"
                                CommandArgument='<%# Eval("sheetID") %>' ImageUrl="../../images/icon_delete.png" ToolTip="Delete"
                                runat="server" Width="18px"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
                <FooterStyle BackColor="#CCCCCC"></FooterStyle>
                <PagerTemplate>
                    <div style="text-align: center;" id="page">
                        共<asp:Label ID="lblTotalCount" runat="server" Text=""></asp:Label>筆 │
                        <asp:Label ID="lblPage" runat="server"></asp:Label>
                        /
                        <asp:Label ID="lblTotalPage" runat="server"></asp:Label>頁 │
                        <asp:LinkButton ID="lbnFirst" runat="Server" Text="第一頁" OnClick="lbnFirst_Click"></asp:LinkButton>
                        │
                        <asp:LinkButton ID="lbnPrev" runat="server" Text="上一頁" OnClick="lbnPrev_Click"></asp:LinkButton>
                        │
                        <asp:LinkButton ID="lbnNext" runat="Server" Text="下一頁" OnClick="lbnNext_Click"></asp:LinkButton>
                        │
                        <asp:LinkButton ID="lbnLast" runat="Server" Text="最後頁" OnClick="lbnLast_Click"></asp:LinkButton>
                        │ 到第 <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="inPageNum" Width="20px" runat="server"></asp:TextBox>頁： 每頁 <asp:TextBox onKeyDown="preventTextEnterEvent();"
                            ID="txtSizePage" Width="25px" runat="server"></asp:TextBox>筆
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
                        <br />
                    </div>
                </PagerTemplate>
                <PagerStyle HorizontalAlign="Center" BackColor="#999999" ForeColor="Black"></PagerStyle>
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White"></SelectedRowStyle>
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White"></HeaderStyle>
                <AlternatingRowStyle BackColor="#CCCCCC"></AlternatingRowStyle>
            </asp:GridView>
        </p>
            
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

