<%@ Page Language="C#" MasterPageFile="../Master/MasterPage.master" AutoEventWireup="true" CodeFile="TowaMold.aspx.cs" Inherits="ESC_Web.Alan.TowaMold" Title="封膠機開機檢查表 TOWA Mold ENCAPSULATION MACHINE STARTUP CHECK LIST" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">

<asp:UpdatePanel id="up" runat="server">
    <ContentTemplate>
    <div>
        <h2>
            TOWA Mold 封膠機開機檢查表<br />
            TOWA Mold ENCAPSULATION MACHINE STARTUP CHECK LIST</h2>
        <table id="autoTable">
            <tbody>
                <tr>
                    <td style="text-align: right;" class="tdbgt">機台編號 Machine No.：</td>
                    <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMachine" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
                    <td style="text-align: right" class="tdbgt">機台位置 Location.：</td>
                    <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtLocation" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align: right" class="tdbgt">時間班別 Time / Shift：</td>
                    <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtTimeShift" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
                    <td style="text-align: right" class="tdbgt">操作人員 Operator：</td>
                    <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtOperator" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
                </tr>
            </tbody>
        </table>
        <br />
        <table class="content_table" style="width: 95%;">
                <tbody>
                    <tr>
                        <td style="vertical-align: middle" class="tdbgt">
                            檢查時機<br />
                            Change Occasion
                        </td>
                        <td style="vertical-align: middle"  class="tdbgt2">
                            <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList><br />
                            <asp:Label ID="lblAlert" runat="server" Text="※換模須填寫" Visible="false" ForeColor="red" Font-Bold="true"></asp:Label>
                        </td>
                        <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">
                            <table style="width: 100%; height: 100%">
                                <tbody>
                                    <tr>
                                        <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;" class="tdCon">
                                            上模</td>
                                        <td>
                                            <asp:TextBox ID="txtUpMold1" runat="server" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;" class="tdCon">
                                            下模</td>
                                        <td>
                                            <asp:TextBox ID="txtDownMold1" runat="server" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">
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
                        <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">
                            <table style="width: 100%; height: 100%">
                                <tbody>
                                    <tr>
                                        <td class="tdCon">
                                            <asp:TextBox ID="txtUpMold3" runat="server" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCon">
                                            <asp:TextBox ID="txtDownMold3" runat="server" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">
                            <table style="width: 100%; height: 100%">
                                <tbody>
                                    <tr>
                                        <td class="tdCon">
                                            <asp:TextBox ID="txtUpMold4" runat="server" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdCon">
                                            <asp:TextBox ID="txtDownMold4" runat="server" visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            時間<br />
                            Date</td>
                        <td class="tdbgt2">--</td>
                        <td colspan="4" class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDate" runat="server" CssClass="input-date" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            流程卡<br />
                            Batch Card</td>
                        <td class="tdbgt2">
                            正確<br />
                            Correct</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBatchCardNo1" runat="server" CssClass="input-medium" AutoPostBack="True" OnTextChanged="txtBatchCardNo_TextChanged"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBatchCardNo2" runat="server" CssClass="input-medium" AutoPostBack="True" OnTextChanged="txtBatchCardNo_TextChanged"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBatchCardNo3" runat="server" CssClass="input-medium" AutoPostBack="True" OnTextChanged="txtBatchCardNo_TextChanged"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBatchCardNo4" runat="server" CssClass="input-medium" AutoPostBack="True" OnTextChanged="txtBatchCardNo_TextChanged"></asp:TextBox></td>
                    </tr>
                    <!--first title-->
                    <tr>
                        <td class="tdTitle" style="width:16%;">
                            <strong>檢查項目</strong>
                        </td>
                        <td class="tdTitle" style="width:20%;" >
                            <strong>Check Item</strong>
                        </td>
                        <td class="tdTitle" style="width:16%;">
                            <strong>P1模&nbsp;<asp:CheckBox ID="chkP1" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="chk_CheckedChanged"></asp:CheckBox></strong>
                        </td>
                        <td class="tdTitle" style="width:16%;">
                            <strong>P2模&nbsp;<asp:CheckBox ID="chkP2" runat="server" AutoPostBack="true" Checked="false" OnCheckedChanged="chk_CheckedChanged"></asp:CheckBox></strong>
                        </td>
                        <td class="tdTitle" style="width:16%;">
                            <strong>P3模&nbsp;<asp:CheckBox ID="chkP3" runat="server" AutoPostBack="true" Checked="false" OnCheckedChanged="chk_CheckedChanged"></asp:CheckBox></strong>
                        </td>
                        <td class="tdTitle" style="width:16%;">
                            <strong>P4模&nbsp;<asp:CheckBox ID="chkP4" runat="server" AutoPostBack="true" Checked="false" OnCheckedChanged="chk_CheckedChanged"></asp:CheckBox></strong>
                        </td>
                    </tr>
                    
                    <tr>
                        <td rowspan="3" class="tdbgt">
                            封膠參數<br />
                            Parameter參照(Refer to) EBV-3-10-62/685</td>
                        <td class="tdbgt2">
                            Clamp Force</td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoClampForce1_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="A1"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoClampForce1_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="A1"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoClampForce2_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="A2"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoClampForce2_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="A2"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoClampForce3_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="A3"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoClampForce3_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="A3"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoClampForce4_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="A4"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoClampForce4_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="A4"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt2">
                            烘烤時間<br />
                            Cure Time</td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoCureTime1_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="B1"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoCureTime1_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="B1"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoCureTime2_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="B2"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoCureTime2_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="B2"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoCureTime3_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="B3"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoCureTime3_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="B3"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoCureTime4_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="B4"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoCureTime4_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="B4"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt2">
                            壓鑄力<br />
                            Transfer Pressure</td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoTransferPressure1_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="C1"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoTransferPressure1_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="C1"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoTransferPressure2_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="C2"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoTransferPressure2_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="C2"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoTransferPressure3_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="C3"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoTransferPressure3_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="C3"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoTransferPressure4_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="C4"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoTransferPressure4_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="C4"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            模溫<br />
                            Mold Temperature參照(Refer to) EBV-3-10-62/685</td>
                        <td class="tdbgt2">
                            正確167~183oC<br />
                            Correct</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldTemperature1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldTemperature2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldTemperature3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldTemperature4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            餅別 / 克數<br />
                            Pellet Type / Weight<br />
                            參照(Refer to) EBV-3-10-62/685</td>
                        <td class="tdbgt2">
                            EMEG770(A)(SL)(LW)<br />
                            EME7372A<br />
                            CEL-9750(Z)HF-10F/10(C)(H)K/10L/10TH<br />
                            CEL-9220HF-13; CEL9240HF10AN<br />
                            CV8710P</td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoPelletTypeWeight1_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="D1"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoPelletTypeWeight1_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="D1"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoPelletTypeWeight2_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="D2"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoPelletTypeWeight2_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="D2"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoPelletTypeWeight3_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="D3"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoPelletTypeWeight3_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="D3"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoPelletTypeWeight4_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="D4"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoPelletTypeWeight4_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="D4"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            是否須清模<br />
                            Mold Clean</td>
                        <td class="tdbgt2">
                            機台清模次數設定值<br />
                            Mold Cleaning Setting</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldClean1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldClean2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldClean3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldClean4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            餅是否在使用期間<br />
                            Working Time<br />
                            (&lt;2 DAYS , 兩天內)</td>
                        <td class="tdbgt2">
                            正確<br />
                            Correct</td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoWorkingTime1_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="E1"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoWorkingTime1_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="E1"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoWorkingTime2_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="E2"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoWorkingTime2_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="E2"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoWorkingTime3_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="E3"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoWorkingTime3_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="E3"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoWorkingTime4_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="E4"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoWorkingTime4_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="E4"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            壓鑄圖起始值是否小於0.25ton<br />
                            Initial Transfer Pressure&lt;0.25ton</td>
                        <td class="tdbgt2">
                            正確<br />
                            Correct</td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoInitialTransferPressure1_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="F1"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoInitialTransferPressure1_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="F1"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoInitialTransferPressure2_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="F2"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoInitialTransferPressure2_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="F2"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoInitialTransferPressure3_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="F3"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoInitialTransferPressure3_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="F3"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoInitialTransferPressure4_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="F4"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoInitialTransferPressure4_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="F4"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            氣壓<br />
                            Air Pressure</td>
                        <td class="tdbgt2">
                            0.4~0.6MPA</td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoAirPressure1_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="G1"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoAirPressure1_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="G1"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoAirPressure2_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="G2"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoAirPressure2_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="G2"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoAirPressure3_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="G3"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoAirPressure3_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="G3"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoAirPressure4_Y" runat="server" Text="V 正確" CssClass="radio inline"
                                GroupName="G4"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoAirPressure4_N" runat="server" Text="V 錯誤" CssClass="radio inline"
                                GroupName="G4"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            真空壓力<br />
                            VACUUM PRESSURE</td>
                        <td class="tdbgt2">
                            &lt; 6 Torr</td>
                        <td class="tdCon">
                            <asp:DropDownList ID="ddlVacuumPressure1" runat="server" CssClass="input-medium">
                            </asp:DropDownList></td>
                        <td class="tdCon">
                            <asp:DropDownList ID="ddlVacuumPressure2" runat="server" CssClass="input-medium">
                            </asp:DropDownList></td>
                        <td class="tdCon">
                            <asp:DropDownList ID="ddlVacuumPressure3" runat="server" CssClass="input-medium">
                            </asp:DropDownList></td>
                        <td class="tdCon">
                            <asp:DropDownList ID="ddlVacuumPressure4" runat="server" CssClass="input-medium">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td rowspan="2" class="tdbgt">
                            靜電放電 (ESD)檢查<br />
                            Electrostatic Discharge (ESD) Check</td>
                        <td class="tdbgt2">
                            機台離子風扇是否有轉動且方向需朝產品方向.<br />
                            Ion Fan Working and Facing to Products. </td>
                        <td class="tdCon">
                            <asp:DropDownList ID="ddlEsdCheck1" runat="server" CssClass="input-medium">
                            </asp:DropDownList></td>
                        <td class="tdCon">
                            <asp:DropDownList ID="ddlEsdCheck2" runat="server" CssClass="input-medium">
                            </asp:DropDownList></td>
                        <td class="tdCon">
                            <asp:DropDownList ID="ddlEsdCheck3" runat="server" CssClass="input-medium">
                            </asp:DropDownList></td>
                        <td class="tdCon">
                            <asp:DropDownList ID="ddlEsdCheck4" runat="server" CssClass="input-medium">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="tdbgt2">
                            工作桌/台車/抗靜電桌墊接地線是否連接. <br />
                            A Ground Wire Working</td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoConnect1_Y" runat="server" Text="V 正常" CssClass="radio inline"
                                GroupName="Z1"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoConnect1_N" runat="server" Text="X 異常" CssClass="radio inline"
                                GroupName="Z1"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoConnect2_Y" runat="server" Text="V 正常" CssClass="radio inline"
                                GroupName="Z2"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoConnect2_N" runat="server" Text="X 異常" CssClass="radio inline"
                                GroupName="Z2"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoConnect3_Y" runat="server" Text="V 正常" CssClass="radio inline"
                                GroupName="Z3"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoConnect3_N" runat="server" Text="X 異常" CssClass="radio inline"
                                GroupName="Z3"></asp:RadioButton>
                        </td>
                        <td class="tdCon">
                            <asp:RadioButton ID="rdoConnect4_Y" runat="server" Text="V 正常" CssClass="radio inline"
                                GroupName="Z4"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoConnect4_N" runat="server" Text="X 異常" CssClass="radio inline"
                                GroupName="Z4"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            安全電眼(閘門)<br />
                            Safety Sensor (Detector)</td>
                        <td class="tdbgt2">
                            正常 Normal</td>
                        <td colspan="4" class="tdCon">
                            <asp:RadioButton ID="rdoSafetySensor_Y" runat="server" Text="Normal 正常" CssClass="radio inline"
                                GroupName="D"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoSafetySensor_N" runat="server" Text="Abnormal 異常" CssClass="radio inline"
                                GroupName="D"></asp:RadioButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            集塵袋是否更換<br />
                            Filter Bag Change</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBN-A52S/F121</td>
                        <td colspan="4" class="tdCon">
                            <asp:RadioButton ID="rdoFilterBagChange_Y" runat="server" Text="更換" CssClass="radio inline"
                                GroupName="H1"></asp:RadioButton>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdoFilterBagChange_N" runat="server" Text="無更換" CssClass="radio inline"
                                GroupName="H1"></asp:RadioButton>
                        </td>
                    </tr>
                    
                    <!--second title-->
                    <tr>
                        <td colspan="2" class="tdTitle">
                            <strong>1 shot自檢項目<br />One Shot Self Inspection Item</strong>
                        </td>
                        <td class="tdTitle">
                            <strong>P1模</strong>                                
                        </td>
                        <td class="tdTitle">
                            <strong>P2模</strong>
                        </td>
                        <td class="tdTitle">
                            <strong>P3模</strong>
                        </td>
                        <td class="tdTitle">
                            <strong>P4模</strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            洞孔<br />
                            Voids</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtVoids1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtVoids2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtVoids3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtVoids4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            填不滿<br />
                            Incomplete Fill</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtIncompleteFill1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtIncompleteFill2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtIncompleteFill3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtIncompleteFill4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            氣泡/膠體突出<br />
                            Bubble / Popcorn</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBubble1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBubble2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBubble3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBubble4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            餘膠<br />
                            Gate Remain</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtGateRemain1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtGateRemain2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtGateRemain3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtGateRemain4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            溢膠<br />
                            Compound Extruding</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCompoundExtruding1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCompoundExtruding2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCompoundExtruding3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCompoundExtruding4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            膠體沾污<br />
                            Body Contamination</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBodyContamination1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBodyContamination2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBodyContamination3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBodyContamination4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            基板/柵片損傷,裂傷,變形<br />
                            Sub / LF Damage, Crack, Deform</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtSubLfDamage1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtSubLfDamage2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtSubLfDamage3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtSubLfDamage4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            銲線弧度不良<br />
                            (X-ray Inspect One Shot per Shit)<br />
                            Wire Defmration / Sweep</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtWireDefmration1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtWireDefmration2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtWireDefmration3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtWireDefmration4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            膠體偏差/龜裂<br />
                            (One Shot per Shift)<br />
                            Body Shift /Crack</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBodyShift1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBodyShift2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBodyShift3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBodyShift4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            黏膜<br />
                            Mold Sticking</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldSticking1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldSticking2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldSticking3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMoldSticking4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            重疊<br />
                            Double Subt / LF</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDoubleSubtLf1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDoubleSubtLf2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDoubleSubtLf3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDoubleSubtLf4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tdbgt">
                            其它<br />
                            Others</td>
                        <td class="tdbgt2">
                            參照(Refer to) EBV-5-1-1/132</td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtOthers1" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtOthers2" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtOthers3" runat="server" CssClass="input-medium"></asp:TextBox></td>
                        <td class="tdCon">
                             <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtOthers4" runat="server" CssClass="input-medium"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Text="Submit"
                                CssClass="btn btn-primary" OnClientClick="return confirm('確定送出表單?');"></asp:Button>
                            <asp:Button ID="btnUpdateSubmit" OnClick="btnUpdateSubmit_Click" runat="server" Text="Submit"
                                Visible="false" CssClass="btn btn-primary" OnClientClick="return confirm('確定送出表單?');">
                            </asp:Button>
                            <asp:Button ID="btnReset" OnClick="btnReset_Click" runat="server" Text="Reset" CssClass="btn btn-inverse"
                                OnClientClick="return confirm('確定把資料清空?');"></asp:Button>
                            <asp:Button ID="btnSwitch" OnClick="btnSwitch_Click" runat="server" Text="Quick Mode"
                                Visible="false" CssClass="btn btn-success"></asp:Button>
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

