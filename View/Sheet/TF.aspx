<%@ Page Language="C#" MasterPageFile="../Master/MasterPage.master" AutoEventWireup="true" CodeFile="TF.aspx.cs" Inherits="ESC_Web.Alan.TF" Title="TF" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

    <asp:UpdatePanel id="up" runat="server">
        <ContentTemplate>
    <h2>Tri-Form(TF) 開機檢查表<br />
        Tri-From(TF) StartUp Check List</h2>
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
                    時間班別 Time / shift ：</td>
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
    <table class="content_table" style="width: 70%">
            <tbody>
                <tr>
                    <td class="tdbgt">
                        檢查時機<br />
                        Change Occasion
                    </td>
                    <td class="tdCon" colspan="3">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input-large" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        日期<br />
                        Date
                    </td>
                    <td class="tdCon" colspan="3">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDate" runat="server" CssClass="input-date" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        流程卡 <br />Batch Card No.</td>
                    <td class="tdCon" colspan="3">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBatchCardNo" runat="server" CssClass="input-xlarge"  AutoPostBack="True" OnTextChanged="txtBatchCardNo_TextChanged"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle" colspan="4">
                        <strong>檢查項目  Check Items</strong>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" class="tdbgt" style="width:30%;">
                        流程卡內容 <br />Batch Card</td>
                    <td class="tdCon" style="width:20%;">
                        正確產品 Correct pkg.<br />
                        <asp:RadioButton ID="rdoPkg_Y" runat="server" Text="V 正確" CssClass="radio inline"
                            GroupName="A"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoPkg_N" runat="server" Text="X 錯誤" 
                            CssClass="radio inline" GroupName="A">
                        </asp:RadioButton>
                    </td>  
                    
                    <td class="tdbgt" rowspan="2" style="width:30%;">
                        吸塵器<br />
                        Vacuum Cleaner</td>
                    <td class="tdCon" rowspan="2" style="width:20%;">
                        正常 Normal<br />
                        <asp:RadioButton ID="rdoCleaner_Y" runat="server" Text="V 正常" CssClass="radio inline"
                            GroupName="C"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoCleaner_N" runat="server" Text="X 異常" CssClass="radio inline"
                            GroupName="C"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdCon">
                        正確模具 Correct Tool<br />
                        <asp:RadioButton ID="rdoTool_Y" runat="server" Text="V 正確" CssClass="radio inline"
                            GroupName="B"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoTool_N" runat="server" Text="X 錯誤" CssClass="radio inline"
                            GroupName="B"></asp:RadioButton>
                    </td>  
                    
                </tr>
                <tr>
                    <td class="tdbgt">
                        安全門防呆
                        <br />
                        Safe Door</td>
                    <td class="tdCon">
                        正常 Normal<br />
                        <asp:RadioButton ID="rdoSafedoor_Y" runat="server" Text="V 正常" CssClass="radio inline"
                            GroupName="F"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoSafedoor_N" runat="server" Text="X 異常" CssClass="radio inline"
                            GroupName="F"></asp:RadioButton>
                    </td>
                    <td class="tdbgt">
                        高壓空氣<br />
                        CDA</td>
                    <td class="tdCon">
                        正常 Normal<br />
                        <asp:RadioButton ID="rdoCda_Y" runat="server" Text="V 正常" CssClass="radio inline"
                            GroupName="E"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoCda_N" runat="server" Text="X 異常" 
                            CssClass="radio inline" GroupName="E">
                        </asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">機台離子風扇<br />是否有轉動且方向需朝產品方向<br />Electrostatic Discharge (ESD) Check</td>
                    <td class="tdCon"><asp:DropDownList runat="server" ID="ddlEsdCheck"/></td>
                    <td class="tdbgt">
                        工作桌接地線是否連結<br />
                        Ground Wrist Connect with Work Table</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoGroundWristConnectTable_Y" runat="server" Text="V 是" CssClass="radio inline"
                            GroupName="G"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoGroundWristConnectTable_N" runat="server" Text="X 否" CssClass="radio inline"
                            GroupName="G"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        冷卻水<br />
                        Cooling Water</td>
                    <td class="tdCon">
                        開 ON<br/>
                        <asp:DropDownList ID="ddlCoolwater" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="tdbgt">--</td>
                    <td class="tdCon">--</td>
                </tr>                        
                <tr>
                    <td class="tdTitle" colspan="6">
                        <strong>產品製程檢查</strong>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        餘膠殘留<br />
                        Text
                        Mold flash Remain</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoMoldFlashRemain_Y" runat="server" Text="合格" CssClass="radio inline"
                            GroupName="J"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoMoldFlashRemain_N" runat="server" Text="不合格" CssClass="radio inline"
                            GroupName="J"></asp:RadioButton>
                    </td>
                    <td class="tdbgt">
                        腳沾污(含可修)<br />
                        Lead Contamination</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoLeadContamination_Y" runat="server" Text="合格" CssClass="radio inline"
                            GroupName="K"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoLeadContamination_N" runat="server" Text="不合格" CssClass="radio inline"
                            GroupName="K"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        腳變形(含可修)<br />
                        Lead Bend</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoLeadBend_Y" runat="server" Text="合格" CssClass="radio inline"
                            GroupName="L"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoLeadBend_N" runat="server" Text="不合格" CssClass="radio inline"
                            GroupName="L"></asp:RadioButton>
                    </td>
                    <td class="tdbgt">
                        腳壓傷/刮傷<br />
                        Lead Damage</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoLendDamage_Y" runat="server" Text="合格" CssClass="radio inline"
                            GroupName="M"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoLendDamage_N" runat="server" Text="不合格" CssClass="radio inline"
                            GroupName="M"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        膠體斷裂/裂角<br />
                        Body Broken / Chip</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoBodyBroken_Y" runat="server" Text="合格" CssClass="radio inline"
                            GroupName="N"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoBodyBroken_N" runat="server" Text="不合格" CssClass="radio inline"
                            GroupName="N"></asp:RadioButton>
                    </td>
                    <td class="tdbgt">
                        毛邊
                        <br />
                        Burr</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoBurr_Y" runat="server" Text="合格" CssClass="radio inline" GroupName="O">
                        </asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoBurr_N" runat="server" Text="不合格" CssClass="radio inline" GroupName="O">
                        </asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        產品變形<br />
                        L/F Deformation</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoLfDeformation_Y" runat="server" Text="合格" CssClass="radio inline"
                            GroupName="P"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoLfDeformation_N" runat="server" Text="不合格" CssClass="radio inline"
                            GroupName="P"></asp:RadioButton>
                    </td>
                    <td class="tdbgt">
                        腳平面度及站立高度<br />
                        Co-Planarity / Stand-off Height </td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoICOS_Y" runat="server" Text="Pass" CssClass="radio inline"
                            GroupName="Q"></asp:RadioButton>&nbsp;
                        <asp:RadioButton ID="rdoICOS_R" runat="server" Text="Rework" CssClass="radio inline"
                            GroupName="Q"></asp:RadioButton>&nbsp;
                        <asp:RadioButton ID="rdoICOS_N" runat="server" Text="Fail" CssClass="radio inline"
                            GroupName="Q"></asp:RadioButton>
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
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
