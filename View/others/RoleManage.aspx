﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleManage.aspx.cs" Inherits="ESC_Web.Alan.RoleManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>權限控管</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnImport" runat="server" Text="匯入人員" OnClick="btnImport_Click" />
        Dept：<asp:DropDownList ID="ddlDept" runat="server" AutoPostBack="false"></asp:DropDownList>
        Name：<asp:TextBox ID="txtName" runat="server"></asp:TextBox>
        Account：<asp:TextBox ID="txtAccount" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
        
        <asp:GridView ID="GridView1" runat="server" DataKeyNames="UsersID" AutoGenerateColumns="False" 
                      CellPadding="3" ForeColor="Black" GridLines="Vertical" AllowSorting="True" AllowPaging="True"
            OnSorting="GridView1_Sorting" 
            OnRowCancelingEdit="GridView1_RowCancelingEdit" 
            OnRowEditing="GridView1_RowEditing" 
            OnRowUpdating="GridView1_RowUpdating" 
            OnRowDataBound="GridView1_RowDataBound"  
            OnPageIndexChanging="GridView1_PageIndexChanging" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px">
            <Columns>                
                <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# (Container.DataItemIndex +1).ToString()%>
                        </ItemTemplate>
                    </asp:TemplateField>
                <asp:TemplateField HeaderText="describe" SortExpression="Role_desc">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlDescribe" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Role_desc") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:BoundField DataField="account" HeaderText="Account" SortExpression="account" ReadOnly="True"></asp:BoundField>
                <asp:BoundField DataField="password" HeaderText="Password" SortExpression="password" ReadOnly="True"></asp:BoundField>
                <asp:BoundField DataField="name" HeaderText="Name" SortExpression="name" ReadOnly="True"></asp:BoundField>
                <asp:BoundField DataField="dept" HeaderText="Dept." SortExpression="dept" ReadOnly="True"></asp:BoundField>
                <asp:TemplateField HeaderText="Action">
                    <EditItemTemplate>
                        <asp:LinkButton ID="lbUpdate" runat="server" CommandName="Update">更新</asp:LinkButton>
                        |
                        <asp:LinkButton ID="lbCancelUpdate" runat="server" CommandName="Cancel">取消</asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Edit">Edit</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                
            </Columns>
            <PagerTemplate>
                <div style="text-align:center;" id="page">
                共<asp:Label ID="lblTotalCount" runat="server" Text=""></asp:Label>筆 │ 
                <asp:Label ID="lblPage" runat="server" ></asp:Label> / <asp:Label ID="lblTotalPage" runat="server" ></asp:Label>頁 │ 
                <asp:LinkButton ID="lbnFirst" runat="Server" Text="第一頁" onclick="lbnFirst_Click" ></asp:LinkButton> │ 
                <asp:LinkButton ID="lbnPrev" runat="server" Text="上一頁" onclick="lbnPrev_Click" ></asp:LinkButton> │ 
                <asp:LinkButton ID="lbnNext" runat="Server" Text="下一頁" onclick="lbnNext_Click"></asp:LinkButton> │ 
                <asp:LinkButton ID="lbnLast" runat="Server" Text="最後頁" onclick="lbnLast_Click" ></asp:LinkButton> │ 
                到第<asp:TextBox ID="inPageNum" Width="20px" runat="server"></asp:TextBox>頁： 
                每頁<asp:TextBox ID="txtSizePage" Width="25px" runat="server"></asp:TextBox>筆
                <asp:Button ID="btnGo" runat="server" Text="Go" onclick="btnGo_Click"/>
                <br />
                </div>
            </PagerTemplate>
            <FooterStyle BackColor="#CCCCCC" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="#CCCCCC" />  
        </asp:GridView>                        
    </div>        
    </form>
</body>
</html>
