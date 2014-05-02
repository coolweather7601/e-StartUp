<%@ Page Language="C#" MasterPageFile="../Master/MasterPage.master" AutoEventWireup="true" CodeFile="LMKsheet31062410.aspx.cs" Inherits="ESC_Web.Alan.LMKsheet31062410" Title="LMKsheet31062410" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:UpdatePanel id="up" runat="server">
    <ContentTemplate>
    <h2>雷射蓋印開機檢查表<br />
        Laser Marking StartUp Check List(LMKsheet31062410)</h2>
    <table id="autoTable">
        <tbody>
            <tr>
                <td style="text-align:right;" class="tdbgt">機台編號 Machine no.：</td>
                <td style="text-align:right;"> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMachine" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
                <td style="text-align:right;" class="tdbgt">機台位置 Location.：</td>
                <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtLocation" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align:right;" class="tdbgt">時間班別 Time / Shift：</td>
                <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtTimeShift" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
                <td style="text-align:right;" class="tdbgt">操作人員 Operator：</td>
                <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtOperator" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
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
                        <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        日期 <br />Date</td>
                    <td class="tdCon" colspan="3">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDate" runat="server" CssClass="input-date" Enabled="false"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        流程卡<br /> Batch Card No.</td>
                    <td class="tdCon" colspan="3">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBatchCardNo" runat="server" CssClass="input-xlarge" AutoPostBack="True"  OnTextChanged="txtBatchCardNo_TextChanged"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        機台選擇</td>
                    <td class="tdCon" colspan="3">
                        <asp:DropDownList ID="ddlMachineType" runat="server" OnSelectedIndexChanged="ddlMachineType_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tdTitle" colspan="4"><strong>檢查項目 Check Items</strong></td>
                </tr>
                <tr>
                    <td class="tdbgt" style="width:30%;">
                        核對產品流程卡蓋印內容<br />
                        (Check Product,B/C, Marking Content)</td>
                    <td class="tdCon" style="width:20%;">
                        正確產品 Correct pkg.<br />
                        <asp:RadioButton ID="rdoCheckProductMarking_Y" runat="server" CssClass="radio inline"
                            Text="V 正確" GroupName="A"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoCheckProductMarking_N" runat="server" CssClass="radio inline"
                            Text="X 錯誤" GroupName="A"></asp:RadioButton>
                    </td>
                    <td class="tdbgt" style="width:30%;">
                        L/F 反向偵測 <br />( Inverse L/F Detect )</td>
                    <td class="tdCon" style="width:20%;">
                        正常 Normal<br />
                        <asp:RadioButton ID="rdoLfDetect_Y" runat="server" CssClass="radio inline" Text="V 正常"
                            GroupName="B"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoLfDetect_N" runat="server" CssClass="radio inline" Text="X 異常"
                            GroupName="B"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" class="tdbgt">
                        燈管/ 二極體使用時數<br />
                        ( Lamp/Diode Operation Hours )</td>
                    <td class="tdCon">
                        &lt; 2700 Hours<br />
                        ( 燈管 AMCC )<br />
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtAmccOperation" runat="server"></asp:TextBox>
                    </td>
                    <td class="tdbgt" rowspan="2">
                        雷射能量 <br />
                        ( Laser Power )</td>
                    <td class="tdCon" rowspan="2">
                        7.0~14.0 Watt(AMCC/E&amp;R)
                        <br />
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtLaserPower" runat="server"></asp:TextBox>
                         <br />
                         10.0~25.0 Watt(AMCC/E&amp;R, BGA散熱片)
                        <br />
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtLaserPowerBGA" runat="server"></asp:TextBox>
                         <br />
                         <32A(Alltec)
                        <br />
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtLaserPowerAlltec" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdCon">
                        &lt; 24000 Hours<br />
                        ( 二極體 E&amp;R )<br />
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtErOperation" runat="server"></asp:TextBox>
                    </td>                    
                </tr>
                <tr>
                    <td class="tdbgt">
                        濾網次數( AMCC )<br />
                         ( Filter Counter )</td>
                    <td class="tdCon">
                        &lt;參數設定 ( &lt;Parameter Setting )
                        <br />
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtFilterCounter" runat="server"></asp:TextBox>
                    </td>
                    <td class="tdbgt">
                        理水高度 <br />
                        ( DI Water Level )</td>
                    <td class="tdCon">
                        正常 Normal<br />
                        <asp:RadioButton ID="rdoDiWaterLevel_Y" runat="server" CssClass="radio inline" Text="V 正常"
                            GroupName="C"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoDiWaterLevel_N" runat="server" CssClass="radio inline" Text="X 異常"
                            GroupName="C"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" class="tdbgt">
                        冷卻水溫度 <br />
                        ( Cool Water Temperature )</td>
                    <td class="tdCon">
                        AMCC: 27~33 ℃
                        <br />
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCoolWaterAmcc" runat="server"></asp:TextBox>
                    </td>
                    <td class="tdbgt" rowspan="2">
                        影像辨認<br />
                        ( 含Pin1方向，蓋印位置與漏蓋 )CCD Function (Pin1 Rotation,Marking Position
                        and No Marking Included )</td>
                    <td class="tdCon" rowspan="2">
                        正常 Normal<br />
                        <asp:RadioButton ID="rdoCcdFunction_Y" runat="server" CssClass="radio inline" Text="V 正常"
                            GroupName="D"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoCcdFunction_N" runat="server" CssClass="radio inline" Text="X 異常"
                            GroupName="D"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdCon">
                        E&amp;R: 24~29 ℃
                        <br />
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCoolWaterEr" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        安全門檢查 ( Safe Door Function )</td>
                    <td class="tdCon">
                        正常 Normal<br />
                        <asp:RadioButton ID="rdoSafedoor_Y" runat="server" CssClass="radio inline" Text="V 正常"
                            GroupName="E"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoSafedoor_N" runat="server" CssClass="radio inline" Text="X 異常"
                            GroupName="E"></asp:RadioButton>
                    </td>
                    <td class="tdbgt">
                        工作桌接地線是否連結
                        <br />
                        Ground Wrist Connect With Work Table</td>
                    <td class="tdCon">
                        連結 Connect<br />
                        <asp:RadioButton ID="rdoGroundWristConnectTable_Y" runat="server" CssClass="radio inline"
                            Text="V 是" GroupName="F"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoGroundWristConnectTable_N" runat="server" CssClass="radio inline"
                            Text="X 否" GroupName="F"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">機台離子風扇<br />是否有轉動且方向需朝產品方向<br />Electrostatic Discharge (ESD) Check</td>
                    <td class="tdCon"><asp:DropDownList runat="server" ID="ddlEsdCheck"/></td>
                    <td class="tdbgt">--</td>
                    <td class="tdCon"></td>
                </tr>  
                
                <tr>
                    <td class="tdTitle" colspan="4"><strong>產品製程檢查</strong></td>
                </tr>
                <tr>
                    <td class="tdbgt">蓋偏 <br />
                    Marking Shift</td>
                    <td class="tdCon">                        
                        <asp:RadioButton ID="rdoMarkingShift_Y" runat="server" CssClass="radio inline" Text="合格"
                            GroupName="S1"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoMarkingShift_N" runat="server" CssClass="radio inline" Text="不合格"
                            GroupName="S1"></asp:RadioButton>
                    </td>
                    <td class="tdbgt">字斷 <br />
                    Broken Words</td>
                    <td class="tdCon">                        
                        <asp:RadioButton ID="rdoBrokenWords_Y" runat="server" CssClass="radio inline" Text="合格"
                            GroupName="S2"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoBrokenWords_N" runat="server" CssClass="radio inline" Text="不合格"
                            GroupName="S2"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">蓋錯 <br />
                    Wrong Marking</td>
                    <td class="tdCon">                        
                        <asp:RadioButton ID="rdoWrongMarking_Y" runat="server" CssClass="radio inline" Text="合格"
                            GroupName="S3"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoWrongMarking_N" runat="server" CssClass="radio inline" Text="不合格"
                            GroupName="S3"></asp:RadioButton>
                    </td>
                    <td class="tdbgt">字跡退色 <br />
                    Faded Marking</td>
                    <td class="tdCon">                        
                        <asp:RadioButton ID="rdoFadedMarking_Y" runat="server" CssClass="radio inline" Text="合格"
                            GroupName="S4"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoFadedMarking_N" runat="server" CssClass="radio inline" Text="不合格"
                            GroupName="S4"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">膠體沾汙 <br />
                    Body Contamination</td>
                    <td class="tdCon">                        
                        <asp:RadioButton ID="rdoBodyContamination_Y" runat="server" CssClass="radio inline" Text="合格"
                            GroupName="S5"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoBodyContamination_N" runat="server" CssClass="radio inline" Text="不合格"
                            GroupName="S5"></asp:RadioButton>
                    </td>
                    <td class="tdbgt">產品斷裂 <br />
                    Substrate Broken</td>
                    <td class="tdCon">                        
                        <asp:RadioButton ID="rdoSubstrateBroken_Y" runat="server" CssClass="radio inline" Text="合格"
                            GroupName="S6"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoSubstrateBroken_N" runat="server" CssClass="radio inline" Text="不合格"
                            GroupName="S6"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">產品龜裂 <br />
                    Substrate Crack</td>
                    <td class="tdCon">                        
                        <asp:RadioButton ID="rdoSubstrateCrack_Y" runat="server" CssClass="radio inline" Text="合格"
                            GroupName="S7"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoSubstrateCrack_N" runat="server" CssClass="radio inline" Text="不合格"
                            GroupName="S7"></asp:RadioButton>
                    </td>
                    <td class="tdbgt">產品變型 <br />
                    Substrate Deformation</td>
                    <td class="tdCon">                        
                        <asp:RadioButton ID="rdoSubstrateDeformation_Y" runat="server" CssClass="radio inline" Text="合格"
                            GroupName="S8"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoSubstrateDeformation_N" runat="server" CssClass="radio inline" Text="不合格"
                            GroupName="S8"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" CssClass="btn btn-primary" OnClientClick="return confirm('確定送出表單?');" Text="Submit"></asp:Button>
                        <asp:Button ID="btnUpdateSubmit" OnClick="btnUpdateSubmit_Click" runat="server" Visible="false" CssClass="btn btn-primary" OnClientClick="return confirm('確定送出表單?');" Text="Submit"></asp:Button>
                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-inverse" OnClientClick="return confirm('確定把資料清空?');" Text="Reset" OnClick="btnReset_Click"></asp:Button>
                        <asp:Button ID="btnSwitch" OnClick="btnSwitch_Click" runat="server" CssClass="btn btn-success" visible="false" Text="Quick Mode"></asp:Button>
                        <asp:Button ID="btnMail" runat="server" CssClass="btn btn-warning" OnClientClick="open_win();" Text="Mail Set"></asp:Button>
                        
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

