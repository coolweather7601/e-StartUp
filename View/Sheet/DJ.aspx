<%@ Page Language="C#" MasterPageFile="../Master/MasterPage.master" AutoEventWireup="true" CodeFile="DJ.aspx.cs" Inherits="ESC_Web.Alan.DJ" Title="DJ" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:UpdatePanel id="up" runat="server">
    <ContentTemplate>  
    <h2>DJ (Dejunk) 開機檢查表<br />
        DJ (Dejunk) StartUp Check List</h2>      
    <table id='autoTable' >
        <tr>
            <td style="text-align:right;" class="tdbgt">機台編號 Machine No.：</td>
            <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMachine" Enabled="true" runat="server" CssClass="input-large"></asp:TextBox></td>    
            <td style="text-align:right;" class="tdbgt">機台位置 Location：</td>
            <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtLocation" Enabled="true" runat="server" CssClass="input-large"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="text-align:right;" class="tdbgt">時間班別 Time / Shift：</td>
            <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtTimeShift" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
            <td style="text-align:right;" class="tdbgt">操作人員 Operator：</td>
            <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtOperator" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
        </tr>
    </table>
    <br />    
    <table class="content_table" style="width:70%;">
            <tr>
                <td class="tdbgt">檢查時機<br />Change Occasion </td>
                <td class="tdCon" colspan="3"><asp:DropDownList ID="ddlStatus" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"></asp:DropDownList></td>              
            </tr>
            <tr>
                <td class="tdbgt">日期<br /> date</td>
                <td class="tdCon" colspan="3">
                     <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDate" runat="server"  CssClass="input-date" Enabled="false"/>
                </td>
            </tr>
            <tr>
                <td class="tdbgt">流程卡 <br />Batch Card No.</td>
                <td class="tdCon" colspan="3"> <asp:TextBox onKeyDown="preventTextEnterEvent();" id="txtBatchCardNo" runat="server"  CssClass="input-xlarge" AutoPostBack="True"  OnTextChanged="txtBatchCardNo_TextChanged"/></td>
            </tr>
            <tr>
                <td colspan="4" class="tdTitle"><strong>檢查項目 Check Items</strong></td>
                
            </tr>
            <tr>
                <td rowspan="2" class="tdbgt" style="width:30%;">流程卡內容 <br />Batch Card</td>
                <td class="tdCon" style="width:20%;">正確產品 Correct pkg.<br />
                    <asp:RadioButton ID="rdoPkg_Y" runat="server" Text="V 正確" GroupName="A" 
                        CssClass="radio inline" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoPkg_N" runat="server" Text="X 錯誤" GroupName="A" 
                        CssClass="radio inline" />
                </td>
                <td rowspan="2" class="tdbgt" style="width:30%;">吸塵器 <br />Vacuum Cleaner</td>
                <td rowspan="2" class="tdCon" style="width:20%;">正常 Normal<br />
                    <asp:RadioButton ID="rdoCleaner_Y" runat="server" Text="V 正常" GroupName="C" 
                        CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoCleaner_N" runat="server" Text="X 異常" GroupName="C" 
                        CssClass="radio inline"/>
                </td>
            </tr>
            <tr>
                <td class="tdCon">正確模具 Correct Tool<br />
                    <asp:RadioButton ID="rdoTool_Y" runat="server" Text="V 正常" GroupName="B" 
                        CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoTool_N" runat="server" Text="X 異常" GroupName="B" 
                        CssClass="radio inline"/>
                </td>
                
            </tr>
            <tr>
                <td class="tdbgt">影像辨識<br />CCD Camera</td>
                <td class="tdCon">正常 Normal<br />
                    <asp:RadioButton ID="rdoCCD_Y" runat="server" Text="V 正常" GroupName="X" 
                        CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoCCD_N" runat="server" Text="X 異常" GroupName="X" 
                        CssClass="radio inline"/>
                </td>
                <td class="tdbgt">高壓空氣<br />CDA</td>
                <td class="tdCon">正常 Normal<br />
                    <asp:RadioButton ID="rdoCda_Y" runat="server" Text="V 正常" GroupName="E" 
                        CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoCda_N" runat="server" Text="X 異常" GroupName="E" 
                        CssClass="radio inline"/>
                </td>
                
            </tr>
            <tr>
                <td class="tdbgt">安全門防呆<br />Safe Door</td>
                <td class="tdCon">正常 Normal<br />
                    <asp:RadioButton ID="rdoSafedoor_Y" runat="server" Text="V 正常" GroupName="F" 
                        CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoSafedoor_N" runat="server" Text="X 異常" GroupName="F" 
                        CssClass="radio inline"/>
                </td>
                <td class="tdbgt">工作桌接地線是否連結<br />Ground Wrist Connect With Work Table</td>
                <td class="tdCon">
                    <asp:RadioButton ID="rdoGroundWristConnectTable_Y" runat="server" Text="V 是" 
                        GroupName="G" CssClass="radio inline" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoGroundWristConnectTable_N" runat="server" Text="X 否" 
                        GroupName="G" CssClass="radio inline" />
                </td>
            </tr>
            <tr>
                <td class="tdbgt">機台離子風扇<br />是否有轉動且方向需朝產品方向<br />Electrostatic Discharge (ESD) Check</td>
                <td class="tdCon"><asp:DropDownList runat="server" ID="ddlEsdCheck"/></td>
                <td class="tdbgt">--</td>
                <td class="tdCon">--</td>
            </tr> 
            <tr>
                <td class="tdTitle" colspan="4"><strong>產品製程檢查</strong></td>
                <tr>
                    <td class="tdbgt">
                        餘膠殘留<br />Mold Flash Remain</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoMoldFlashRemain_Y" runat="server" 
                            CssClass="radio inline" GroupName="J" Text="合格" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="rdoMoldFlashRemain_N" runat="server" 
                            CssClass="radio inline" GroupName="J" Text="不合格" />
                    </td>
                    <td class="tdbgt">
                        腳沾污(含可修)<br />Lead Contamination</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoLeadContamination_Y" runat="server" 
                            CssClass="radio inline" GroupName="K" Text="合格" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="rdoLeadContamination_N" runat="server" 
                            CssClass="radio inline" GroupName="K" Text="不合格" />
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        腳變形(含可修)<br />Lead Bend</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoLeadBend_Y" runat="server" CssClass="radio inline" 
                            GroupName="L" Text="合格" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="rdoLeadBend_N" runat="server" CssClass="radio inline" 
                            GroupName="L" Text="不合格" />
                    </td>
                    <td class="tdbgt">
                        腳壓傷/刮傷<br />Lead Damage</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoLendDamage_Y" runat="server" CssClass="radio inline" 
                            GroupName="M" Text="合格" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="rdoLendDamage_N" runat="server" CssClass="radio inline" 
                            GroupName="M" Text="不合格" />
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        膠體斷裂/裂角<br />Body Broken/ Chip</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoBodyBroken_Y" runat="server" CssClass="radio inline" 
                            GroupName="N" Text="合格" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="rdoBodyBroken_N" runat="server" CssClass="radio inline" 
                            GroupName="N" Text="不合格" />
                    </td>
                    <td class="tdbgt">
                        毛邊<br />Burr</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoBurr_Y" runat="server" CssClass="radio inline" 
                            GroupName="O" Text="合格" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="rdoBurr_N" runat="server" CssClass="radio inline" 
                            GroupName="O" Text="不合格" />
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        產品變形<br />L/F Deformation</td>
                    <td class="tdCon">
                        <asp:RadioButton ID="rdoLfDeformation_Y" runat="server" CssClass="radio inline" 
                            GroupName="P" Text="合格" />
                        &nbsp;&nbsp;
                        <asp:RadioButton ID="rdoLfDeformation_N" runat="server" CssClass="radio inline" 
                            GroupName="P" Text="不合格" />
                    </td>
                    <td class="tdbgt">
                        --</td>
                    <td class="tdCon">
                        --</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" 
                            OnClick="btnSubmit_Click" OnClientClick="return confirm('確定送出表單?');" 
                            Text="Submit" />
                        <asp:Button ID="btnUpdateSubmit" runat="server" CssClass="btn btn-primary" 
                            OnClick="btnUpdateSubmit_Click" OnClientClick="return confirm('確定送出表單?');" 
                            Text="Submit" Visible="false" />
                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-inverse" 
                            OnClick="btnReset_Click" OnClientClick="return confirm('確定把資料清空?');" 
                            Text="Reset" />
                        <asp:Button ID="btnSwitch" runat="server" CssClass="btn btn-success" 
                            OnClick="btnSwitch_Click" Text="Quick Mode" visible="false" />
                        <asp:Button ID="btnMail" runat="server" CssClass="btn btn-warning" 
                            OnClientClick="open_win();" Text="Mail Set" />
                        <script type="text/javascript">

                    function open_win(){
                        window.open('../others/MailSet.aspx?sheet_categoryID=<%=sheet_categoryID %>', '_blank', 'width=1100,height=700,scrollbars=yes');
                    }
                    </script>
                    </td>
                </tr>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

