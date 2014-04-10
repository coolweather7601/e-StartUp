<%@ Page Language="C#" MasterPageFile="../Master/MasterPage.master" AutoEventWireup="true" CodeFile="CleanStar.aspx.cs" Inherits="ESC_Web.Alan.CleanStar" Title="洗模機檢查表AUTO CLEANSTAR ENCAPSULATION CHECK LIST" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:UpdatePanel id="up" runat="server">
    <ContentTemplate>
    <div>
        <h2>
            洗 模 機 檢 查 表
            <br />
            AUTO CLEANSTAR ENCAPSULATION CHECK LIST</h2>
        <table id="autoTable">
            <tbody>
                <tr>
                    <td style="text-align: right" class="tdbgt">
                        機台編號 Machine No.：</td>
                    <td>
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMachine" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
                    <td style="text-align: right" class="tdbgt">
                        機台位置 Location.：</td>
                    <td>
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtLocation" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right" class="tdbgt">
                        時間班別 Time / Shift：</td>
                    <td>
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtTimeShift" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
                    <td style="text-align: right" class="tdbgt">
                        操作人員 Operator：</td>
                    <td>
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtOperator" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
                </tr>
            </tbody>
        </table>
        <br />
        <table class="content_table" style="width: 50%">
                <tbody>
                    <tr>
                        <td class="tdbgt">
                            檢查時機<br />
                            Change Occasion
                        </td>
                        <td class="tdCon" colspan="2">
                            <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            日期<br />
                            Date
                        </td>
                        <td class="tdCon" colspan="2">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDate" runat="server" CssClass="input-date" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <!--first title-->
                    <tr>
                        <td class="tdTitle" style="width:30%;">
                            <strong>檢查項目Check Item</strong>
                        </td>
                        <td class="tdTitle" style="width:70%;" colspan="2">
                            <strong>規格 Spec</strong>
                        </td>
                    </tr>                    
                    <tr>
                        <td class="tdbgt">
                            安全門<br />
                            Safety Door</td>
                        <td class="tdbgt2">
                            不漏砂</td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoSafetyDoor_Y" runat="server" Text="Normal 正常" CssClass="radio inline"
                                GroupName="A"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoSafetyDoor_N" runat="server" Text="Abnormal 異常" CssClass="radio inline"
                                GroupName="A"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            氣壓量<br />
                            Air Pressure</td>
                        <td class="tdbgt2">
                            管制線內 (3 +/-0.5 Bar)</td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoAirPressure_Y" runat="server" Text="Normal 正常" CssClass="radio inline"
                                GroupName="B"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoAirPressure_N" runat="server" Text="Abnormal 異常" CssClass="radio inline"
                                GroupName="B"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            砂斗<br />
                            Collecting Box</td>
                        <td class="tdbgt2">
                            管制線內</td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoCollectBox_Y" runat="server" Text="Normal 正常" CssClass="radio inline"
                                GroupName="C"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoCollectBox_N" runat="server" Text="Abnormal 異常" CssClass="radio inline"
                                GroupName="C"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Submit"
                                CssClass="btn btn-primary" OnClientClick="return confirm('確定送出表單?');"></asp:Button>
                            <asp:Button ID="btnUpdateSubmit" OnClick="btnUpdateSubmit_Click" runat="server" Text="Submit"
                                Visible="false" CssClass="btn btn-primary" OnClientClick="return confirm('確定送出表單?');">
                            </asp:Button>
                            <asp:Button ID="btnReset" OnClick="btnReset_Click" runat="server" Text="Reset" CssClass="btn btn-inverse"
                                OnClientClick="return confirm('確定把資料清空?');"></asp:Button>
                            <asp:Button ID="btnSwitch" OnClick="btnSwitch_Click" runat="server" Text="Quick Mode"
                                CssClass="btn btn-success" Visible="false"></asp:Button>
                            <asp:Button ID="btnMail" runat="server" Text="Mail Set" CssClass="btn btn-warning"
                                OnClientClick="open_win();"></asp:Button>

                            <script type="text/javascript">
                    function open_win(){
                        window.open('../others/MailSet.aspx?sheet_categoryID=<%=sheet_categoryID %>', '_blank', 'width=1100,height=700,scrollbars=yes');
                    }
                            </script>

                        </td>
                    </tr>
                </tbody>
            </table>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

