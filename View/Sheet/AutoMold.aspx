<%@ Page Language="C#" MasterPageFile="../Master/MasterPage.master" AutoEventWireup="true" CodeFile="AutoMold.aspx.cs" Inherits="ESC_Web.Alan.AutoMold" Title="自動模壓鑄檢查表 AUTO MOLD ENCAPSULATION CHECK LIST" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

<asp:UpdatePanel id="up" runat="server"> 
    <ContentTemplate>
    <div>
        <h2>
            自 動 模 壓 鑄 檢 查 表<br />
            AUTO MOLD ENCAPSULATION CHECK LIST
        </h2>
        <table id="autoTable">
            <tbody>
                <tr>
                    <td style="text-align: right" class="tdbgt">
                        機台編號 Machine No.：</td>
                    <td>
                        <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtMachine" runat="server"
                            CssClass="input-large" Enabled="true"></asp:TextBox></td>
                    <td style="text-align: right" class="tdbgt">
                        機台位置 Location.：</td>
                    <td>
                        <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtLocation" runat="server"
                            CssClass="input-large" Enabled="true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right" class="tdbgt">
                        時間班別 Time / Shift：</td>
                    <td>
                        <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtTimeShift" runat="server"
                            CssClass="input-large" Enabled="true"></asp:TextBox></td>
                    <td style="text-align: right" class="tdbgt">
                        操作人員 Operator：</td>
                    <td>
                        <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtOperator" runat="server"
                            CssClass="input-large" Enabled="true"></asp:TextBox></td>
                </tr>
            </tbody>
        </table>
        <br />
        <table class="content_table" style="width: 75%">
                <tbody>
                    <tr>
                        <td style="vertical-align: middle;" class="tdbgt">
                            檢查時機<br />
                            Change Occasion
                        </td>
                        <td style="vertical-align: middle;">
                            <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList><br />
                            <asp:Label ID="lblAlert" runat="server" Text="※換模須填寫" Visible="false" ForeColor="red"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;padding-top: 0px">
                            <table style="width: 100%; height: 100%">
                                <tbody>
                                    <tr>
                                        <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px">
                                            上模</td>
                                        <td class="tdCon">
                                            <asp:TextBox ID="txtUpMold1" runat="server" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px">
                                            下模</td>
                                        <td class="tdCon">
                                            <asp:TextBox ID="txtDownMold1" runat="server" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;padding-top: 0px">
                            <table style="width: 100%; height: 100%">
                                <tbody>
                                    <tr>
                                        <td class="tdCon">
                                            <asp:TextBox ID="txtUpMold2" runat="server" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCon">
                                            <asp:TextBox ID="txtDownMold2" runat="server" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            日期<br />
                            Date
                        </td>
                        <td class="tdCon" colspan="3">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtDate" runat="server" CssClass="input-date"
                                Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            流程卡卡號<br />
                            Batch Card No
                        </td>                           
                        <td class="tdCon" colspan="3">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtBatchCardNo" runat="server"
                                CssClass="input-xlarge" AutoPostBack="True" OnTextChanged="txtBatchCardNo_TextChanged"></asp:TextBox></td>
                    </tr>
                    <!--first title-->
                    <tr>
                        <td class="tdTitle" style="width: 30%"><strong>檢查項目 Check Item</strong></td>
                        <td class="tdTitle" style="width: 30%"><strong>規格 Spec</strong></td>
                        <td class="tdTitle" style="width: 20%"><strong>P1模&nbsp;<asp:CheckBox ID="chkP1" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="chk_CheckedChanged"></asp:CheckBox></strong></td>
                        <td class="tdTitle" style="width: 20%"><strong>P2模&nbsp;<asp:CheckBox ID="chkP2" runat="server" AutoPostBack="true" Checked="false" OnCheckedChanged="chk_CheckedChanged"></asp:CheckBox></strong></td>
                    </tr>
                    
                    <tr>
                        <td class="tdbgt">
                            封膠參數<br />
                            Process Parameter</td>
                        <td class="tdbgt2">
                            參照參數表 Rf Parameter Table</td>
                        <td class="tdCon" colspan="2">
                            <asp:RadioButton ID="rdoRf_Y" runat="server" Text="Normal 正常" 
                                CssClass="radio inline" GroupName="A">
                            </asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoRf_N" runat="server" Text="Abnormal 異常" 
                                CssClass="radio inline" GroupName="A">
                            </asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            餅別<br />
                            Pellet Type</td>
                        <td class="tdbgt2">
                            參照下表 Ref Below</td>
                        <td class="tdCon" colspan="2">
                            <asp:RadioButton ID="rdoPelletType_Y" runat="server" Text="Normal 正常" CssClass="radio inline"
                                GroupName="B"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoPelletType_N" runat="server" Text="Abnormal 異常" CssClass="radio inline"
                                GroupName="B"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            餅使用期限<br />
                            Pellet Life Time</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-3-10-62/688</td>
                        <td class="tdCon" colspan="2">
                            <asp:RadioButton ID="rdoPelletLife_Y" runat="server" Text="Normal 正常" CssClass="radio inline"
                                GroupName="C"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoPelletLife_N" runat="server" Text="Abnormal 異常" CssClass="radio inline"
                                GroupName="C"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            安全電眼(閘門)<br />
                            Safety Sensor (Detector)</td>
                        <td class="tdbgt2">
                            正常 Normal</td>
                        <td class="tdCon" colspan="2">
                            <asp:RadioButton ID="rdoSafetySensor_Y" runat="server" Text="Normal 正常" CssClass="radio inline"
                                GroupName="D"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoSafetySensor_N" runat="server" Text="Abnormal 異常" CssClass="radio inline"
                                GroupName="D"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            模溫<br />
                            Mold Temp</td>
                        <td class="tdbgt2">
                            正常 Normal</td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtMoldTemp1" runat="server"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtMoldTemp2" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            作業shot次<br />
                            Mold shots</td>
                        <td class="tdbgt2">--</td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtMoldShots1" runat="server"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtMoldShots2" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            模具累積班數<br />
                            Mold Inline Shift Accu. Total</td>
                        <td class="tdbgt2">
                            (HV)(HW)(X)QFN/HVSON:最大3班 Max 3 Shifts<br />
                            (H)(L)(T)QFP:最大9班 Max 9 Shifts<br />
                            其他(other): 最大12班 Max 12 Shifts</td>
                        <td class="tdCon">
                            <asp:TextBox Style="color: #999999" onkeydown="preventTextEnterEvent();" ID="txtMoldShiftTotal1"
                                onfocus="moldShiftFocus(this);" runat="server" Width="90%" TextMode="MultiLine"
                                Rows="4"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox Style="color: #999999" onkeydown="preventTextEnterEvent();" ID="txtMoldShiftTotal2"
                                onfocus="moldShiftFocus(this);" runat="server" Width="90%" TextMode="MultiLine"
                                Rows="4"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td rowspan="2" class="tdbgt">
                            靜電放電 (ESD)檢查<br />
                            Electrostatic Discharge (ESD) Check</td>
                        <td class="tdbgt2">
                            機台離子風扇是否有轉動且方向需朝產品方向.</td>
                        <td class="tdCon" colspan="2">
                            <asp:DropDownList ID="ddlEsdCheck" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt2">
                            工作桌/台車/抗靜電桌墊接地線是否連結.</td>
                        <td class="tdCon" colspan="2">
                            <asp:RadioButton ID="rdoConnect_Y" runat="server" Text="Normal 正常" CssClass="radio inline"
                                GroupName="E"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoConnect_N" runat="server" Text="Abnormal 異常" CssClass="radio inline"
                                GroupName="E"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            集塵袋是否更換<br />
                            Dust Bag Clean / Renew</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBN-A52S/F87<br />
                        </td>
                        <td class="tdCon" colspan="2">
                            <asp:RadioButton ID="rdoDustBagChange_Y" runat="server" Text="更換" CssClass="radio inline"
                                GroupName="H"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoDustBagChange_N" runat="server" Text="無更換" CssClass="radio inline"
                                GroupName="H"></asp:RadioButton>
                        </td>
                    </tr>
                    <!--second title-->
                    <tr>
                        <td class="tdTitle" colspan="2"><strong>1 shot自檢項目 One Shot Self Inspection Item</strong></td>
                        <td class="tdTitle"><strong>P1模</strong></td>
                        <td class="tdTitle"><strong>P2模</strong></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            填不滿<br />
                            Incomplete Fill</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtIncompleteFill1" runat="server"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtIncompleteFill2" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            洞孔<br />
                            Void/Pin Hole</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtVoidPinHole1" runat="server"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtVoidPinHole2" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            黏模<br />
                            Sticking Mold</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtStickingMold1" runat="server"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtStickingMold2" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            廢膠/黃膠<br />
                            Mold Flash</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtMoldFlash1" runat="server"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtMoldFlash2" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            L/F抓料變形<br />
                            L/F Pick up Damage</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtLfPickUpDamage1" runat="server"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtLfPickUpDamage2" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            膠體沾污<br />
                            Body Contamination</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtBodyContamination1" runat="server"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtBodyContamination2" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            頂出孔<br />
                            Ejector Marks</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtEjectorMarks1" runat="server"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtEjectorMarks2" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            膠體偏移(差)<br />
                            Body Shift</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtBodyShift1" runat="server"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtBodyShift2" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            其他<br />
                            Others</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtOthers1" runat="server"></asp:TextBox></td>
                        <td class="tdCon">
                            <asp:TextBox onkeydown="preventTextEnterEvent();" ID="txtOthers2" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Submit"
                                CssClass="btn btn-primary" OnClientClick="return confirm('確定送出表單?');"></asp:Button>
                            <asp:Button ID="btnUpdateSubmit" OnClick="btnUpdateSubmit_Click" runat="server" Text="Submit"
                                Visible="False" CssClass="btn btn-primary" OnClientClick="return confirm('確定送出表單?');">
                            </asp:Button>
                            <asp:Button ID="btnReset" OnClick="btnReset_Click" runat="server" Text="Reset" CssClass="btn btn-inverse"
                                OnClientClick="return confirm('確定把資料清空?');"></asp:Button>
                            <asp:Button ID="btnSwitch" OnClick="btnSwitch_Click" runat="server" Text="Quick Mode"
                                CssClass="btn btn-success" Visible="false"></asp:Button>
                            <asp:Button ID="btnMail" runat="server" Text="Mail Set" CssClass="btn btn-warning"
                                OnClientClick="open_win();"></asp:Button>

                            <script type="text/javascript">
                                function open_win() {
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

