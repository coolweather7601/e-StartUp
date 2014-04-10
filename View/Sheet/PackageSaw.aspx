<%@ Page Language="C#" MasterPageFile="../Master/MasterPage.master" AutoEventWireup="true" CodeFile="PackageSaw.aspx.cs" Inherits="ESC_Web.Alan.PackageSaw" Title="PackageSaw" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
   
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
 <asp:UpdatePanel id="up" runat="server">
    <ContentTemplate>
    <h2>Package Saw 開機檢查表<br />
        Package Saw StartUp Check List</h2>
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
    <table class="content_table" style="width: 70%">
            <tbody>
                <tr>
                    <td class="tdbgt">
                        檢查時機<br />
                        Change Occasion
                    </td>
                    <td class="tdCon" colspan="3">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input-large" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        日期<br />
                        Date</td>
                    <td class="tdCon" colspan="3">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDate" runat="server" CssClass="input-date" Enabled="false"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        流程卡<br />
                        Batch Card No.</td>
                    <td class="tdCon" colspan="3">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBatchCardNo" runat="server" CssClass="input-xlarge" AutoPostBack="True"  OnTextChanged="txtBatchCardNo_TextChanged"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdTitle" style="width: 25%;">
                        <strong>檢查項目 Check Items</strong>
                    </td>
                    <td class="tdTitle" style="width: 30%;">
                        <strong>規格 Spec.</strong>
                    </td>
                    <td class="tdTitle" style="width: 45%;" colspan="2">
                        <strong>結果</strong>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        產品型別<br />
                        Package Type</td>
                    <td class="tdbgt2">--</td>
                    <td class="tdCon" colspan="2">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtPackage" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        檢查刀片破損狀況<br />
                        Blade Broken</td>
                    <td class="tdbgt2">
                        刀片破損長度，如超過１cm須換刀.
                        <br />
                        Broken &gt;1cm, change blade
                    </td>
                    <td class="tdCon" colspan="2">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtbladeChipping" runat="server"></asp:TextBox>cm
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        水刃<br />
                        Water Nozzle Condition</td>
                    <td class="tdbgt2">
                        檢查水刀是否對正、固定牢固</td>
                    <td colspan="2" class="tdCon">
                        <asp:RadioButton ID="rdoWaterNozzle_Y" runat="server" Text="Normal  正常" CssClass="radio inline"
                            GroupName="A"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoWaterNozzle_N" runat="server" Text="Abnormal  不正常" CssClass="radio inline"
                            GroupName="A"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        冷卻水量檢查<br /> Cooling water flow rate</td>
                    <td class="tdbgt2">
                        水量 &gt; 1.0 liter / min.</td>
                    <td colspan="2" class="tdCon">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCoolWater" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td rowspan="2" class="tdbgt">
                        切割米數 記錄 (m)<br />
                        ( 僅供參考 )
                        <br />
                        Z1 / Z2 Cutting Length</td>
                    <td class="tdbgt2" rowspan="2">--</td>
                    <td class="tdTitle"><strong>Z1</strong></td>
                    <td class="tdTitle"><strong>Z2</strong></td>
                </tr>
                <tr>
                    <td class="tdCon">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCuttingLength1" runat="server"></asp:TextBox>
                    </td>
                    <td class="tdCon">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCuttingLength2" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        目前刀刃長度<br />
                        (機台 F3 顯示)
                        <br />
                        Blade Exposure (mm)</td>
                    <td class="tdbgt2">
                        LFBGA / TFBGA : &gt; 2.2 mm (Jig)<br />
                        HVQFN : &gt; 1.8 mm (Jig)<br />
                        LFBGA / TFBGA : &gt; 1.9 (tape)<br />
                        HVQFN : &gt; 1.5 mm (tape)<br />
                        XQFN : &gt; 1.1 mm (tape)
                    </td>
                    <td class="tdCon">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBladeExposure1" runat="server"></asp:TextBox><br />
                    </td>
                    <td class="tdCon">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBladeExposure2" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trBladeDeviation" runat="server">
                    <td class="tdbgt">
                        ( 多刀 ) 刃長差異 
                        <br />
                        Blade Exposure Deviation<br /> ( 每天量一次 )<br /> ( 單刀機型免填 )
                    </td>
                    <td class="tdbgt2">
                        Difference &lt; 0.20 mm<br />
                        差異 &lt; 0.20 mm<br />
                        ( 若超過要換刀 )
                    </td>
                    <td class="tdCon" colspan="2">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBladeDeviation" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trEmulsion" runat="server">
                    <td class="tdbgt">
                        切削液濃度<br /> Emulsion <br />
                        ( 無切削液則免填 )
                    </td>
                    <td class="tdbgt2">
                        2.2 ~ 4.0 % @ 20 °C
                    </td>
                    <td class="tdCon" colspan="2">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtEmulsion" runat="server"></asp:TextBox>%
                    </td>
                </tr>
                <tr id="trCo2Bubbler" runat="server">
                    <td class="tdbgt">水阻值<br /> Water Resistant<br />
                        (HVSON12 只能在 #SA610 (有 CO2 bubbler 的機台) 上切)
                    </td>
                    <td class="tdbgt2">0.2 ~ 0.6 Mohm</td>
                    <td class="tdCon" colspan="2">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCo2Bubbler" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        進刀速度 
                        <br />
                        Feed Speed</td>
                    <td class="tdbgt2">
                        BT-substrate &lt; 100mm/s<br />
                        Leadframe 10-50 mm/s
                    </td>
                    <td class="tdCon" colspan="2">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtFeedSpeed" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td class="tdbgt">確認切偏量在規格內<br /> Package Saw Shift</td>
                    <td class="tdbgt2"> <50 um<br />NX4-00043 </td>
                    <td class="tdCon" colspan="2">
                        <asp:RadioButton ID="rdoCut_Y" runat="server" Text="Normal  正常" CssClass="radio inline"
                            GroupName="A1"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoCut_N" runat="server" Text="Abnormal  不正常" CssClass="radio inline"
                            GroupName="A1"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">確認產品尺寸在規格內  
                        <br />
                        Package Dimension</td>
                    <td class="tdbgt2">詳見標準尺寸表<br />EBV-3-10-62/456 </td>
                    <td class="tdCon" colspan="2">
                        <asp:RadioButton ID="rdoProdSize_Y" runat="server" Text="Normal  正常" CssClass="radio inline"
                            GroupName="A2"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoProdSize_N" runat="server" Text="Abnormal  不正常" CssClass="radio inline"
                            GroupName="A2"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">確認 lead smearing (腳切糊)<br /> No lead smearing </td>
                    <td class="tdbgt2">在規格內 NX4-00043</td>
                    <td class="tdCon" colspan="2">
                        <asp:RadioButton ID="rdoLeadSmearing_Y" runat="server" Text="Normal  正常" CssClass="radio inline"
                            GroupName="A3"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoLeadSmearing_N" runat="server" Text="Abnormal  不正常" CssClass="radio inline"
                            GroupName="A3"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">確認產品無裂痕或刮傷<br /> Package Crack or Scrach</td>
                    <td class="tdbgt2">NX4-00043</td>
                    <td class="tdCon" colspan="2">
                        <asp:RadioButton ID="rdoProdCrack_Y" runat="server" Text="Normal  正常" CssClass="radio inline"
                            GroupName="A4"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoProdCrack_N" runat="server" Text="Abnormal  不正常" CssClass="radio inline"
                            GroupName="A4"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">確認產品無大象腳<br /> No Elephant feet</td>
                    <td class="tdbgt2">NX4-00043</td>
                    <td class="tdCon" colspan="2">
                        <asp:RadioButton ID="rdoProdElephant_Y" runat="server" Text="No 無" CssClass="radio inline"
                            GroupName="A5"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoProdElephant_N" runat="server" Text="YES 有" CssClass="radio inline"
                            GroupName="A5"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">機台<br /> Machine </td>
                    <td class="tdbgt2">動作正常無卡機 (without jamming)</td>
                    <td class="tdCon" colspan="2">
                        <asp:RadioButton ID="rdoMachineNormal_Y" runat="server" Text="Normal  正常" CssClass="radio inline"
                            GroupName="A6"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoMachineNormal_N" runat="server" Text="Abnormal  不正常" CssClass="radio inline"
                            GroupName="A6"></asp:RadioButton>
                    </td>
                </tr>

                <tr>
                    <td class="tdbgt">
                        安全門 (1)
                        <br />
                        Safe Door </td>
                    <td class="tdbgt2">能正確作動</td>
                    <td class="tdCon" colspan="2">
                        <asp:RadioButton ID="rdoSafetydoor_Y" runat="server" Text="Normal  正常" CssClass="radio inline"
                            GroupName="B"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoSafetydoor_N" runat="server" Text="Abnormal  不正常" CssClass="radio inline"
                            GroupName="B"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">
                        安全門 (2)<br /> &nbsp;Safe Door</td>
                    <td class="tdbgt2">已關上</td>
                    <td class="tdCon" colspan="2">
                        <asp:RadioButton ID="rdoSafetydoorClose_Y" runat="server" Text="Closed  關" CssClass="radio inline"
                            GroupName="C"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoSafetydoorClose_N" runat="server" Text="Open 開" CssClass="radio inline"
                            GroupName="C"></asp:RadioButton>
                    </td>
                </tr>
                <tr>
                    <td class="tdbgt">檢查工作桌桌墊<br /> ESD mat on working table
                    </td>
                    <td class="tdbgt2">是否完整無破損</td>
                    <td class="tdCon" colspan="2">
                        <asp:RadioButton ID="rdoTable_Y" runat="server" Text="Normal  正常" CssClass="radio inline"
                            GroupName="D"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoTable_N" runat="server" Text="Abnormal  不正常" CssClass="radio inline"
                            GroupName="D"></asp:RadioButton>
                    </td>    
                </tr>
                <tr>
                    <td class="tdbgt">
                        接地線是否連接完整、無脫落
                     
                        <br />
                        Ground Wrist Connect with Working table</td>
                    <td class="tdbgt2">-</td>
                    <td class="tdCon" colspan="2">
                        <asp:RadioButton ID="rdoEsdWire_Y" runat="server" Text="Normal  正常" CssClass="radio inline"
                            GroupName="E"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoEsdWire_N" runat="server" Text="Abnormal  不正常" CssClass="radio inline"
                            GroupName="E"></asp:RadioButton>
                    </td>    
                </tr>
                <tr>
                    <td class="tdbgt">
                        靜電風扇<br /> Ionizer Check</td>
                    <td class="tdbgt2">離子風扇是否有轉動且方向需朝產品方向</td>
                    <td class="tdCon"colspan="2">
                        <asp:RadioButton ID="rdoEsdFanDirection_Y" runat="server" Text="Normal  正常" CssClass="radio inline"
                            GroupName="F"></asp:RadioButton>&nbsp;&nbsp;
                        <asp:RadioButton ID="rdoEsdFanDirection_N" runat="server" Text="Abnormal  不正常" CssClass="radio inline"
                            GroupName="F"></asp:RadioButton>
                    </td>    
                </tr>
                
                <tr>
                    <td class="tdbgt">
                        備註</td>
                    <td class="tdbgt2">--</td>
                    <td class="tdCon" colspan="2">
                         <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMemo" runat="server" CssClass="input-large" TextMode="MultiLine"
                            Rows="8" Width="300px"></asp:TextBox></td>
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

