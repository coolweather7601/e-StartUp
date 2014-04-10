<%@ Page Language="C#" MasterPageFile="../Master/MasterPage.master" AutoEventWireup="true" CodeFile="LMKsheet31062643.aspx.cs" Inherits="ESC_Web.Alan.LMKsheet31062643" Title="LMKsheet31062643" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:UpdatePanel id="up" runat="server">
    <ContentTemplate>  
    <h2>雷射蓋印開機檢查表<br />
        Laser Marking StartUp Check List(LMKsheet31062643)</h2>
    <table id='autoTable'>
        <tr>
            <td style="text-align:right;" class="tdbgt">機台編號 Machine No.：</td>
            <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMachine" Enabled="true" runat="server" CssClass="input-large"></asp:TextBox></td>       
            <td style="text-align:right;" class="tdbgt">機台位置 Location.：</td>
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
                <td class="tdbgt">日期<br /> Date</td>
                <td class="tdCon" colspan="3"> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDate" runat="server" CssClass="input-date" Enabled="false"/></td>
            </tr>
            <tr>
                <td class="tdbgt">流程卡<br /> Batch Card No.</td>
                <td class="tdCon" colspan="3"><asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtBatchCardNo" runat="server"  CssClass="input-xlarge" AutoPostBack="True"  OnTextChanged="txtBatchCardNo_TextChanged"/></td>
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
                <td class="tdbgt" rowspan="2" style="width:30%;">理水水位(無理水機構免填)<br />( D.I. Water Level )</td>
                <td class="tdCon" rowspan="2" style="width:20%;">正確產品 Correct pkg.<br />
                    <asp:RadioButton ID="rdoDiWaterLevel_Y" runat="server" Text="V 正確" 
                        GroupName="A" CssClass="radio inline" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoDiWaterLevel_N" runat="server" Text="X 錯誤" 
                        GroupName="A"  CssClass="radio inline"/>
                </td>
                <td rowspan="2" class="tdbgt" style="width:30%;">冷卻水溫度(無理水機構免填)<br />( Cooling Water Temp. )</td>
                <td class="tdCon" style="width:20%;">27~33 ℃ ( AMCC )<br /> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCoolWaterTempAmcc" runat="server"/> </td>
            </tr>
            <tr>
                <td class="tdCon">24~29 ℃ ( E&amp;R )<br />  <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCoolWaterTempEr" runat="server"/></td>
            </tr>
            <tr>
                <td rowspan="2" class="tdbgt">燈管/ 二極體使用時數<br />( Lamp / Diode Operation Hours )</td>
                <td class="tdCon">&lt; 2700 Hours<br />( 燈管 AMCC )<br /> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtAmccOperation" runat="server"/></td>
                <td rowspan="2" class="tdbgt">核對產品流程卡蓋印內容<br />(Check Product,B/C, Marking Content)</td>
                <td class="tdCon"rowspan="2">正常 Correct<br />
                    <asp:RadioButton ID="rdoCheckProductMarking_Y" runat="server" Text="V 正確" 
                        GroupName="B" CssClass="radio inline" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoCheckProductMarking_N" runat="server" Text="X 錯誤" 
                        GroupName="B" CssClass="radio inline" />
                </td>
            </tr>
            <tr>
                <td class="tdCon">< 24000 Hours<br />( 二極體 E&amp;R )<br />  <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtErOperation" runat="server"/></td>
            </tr>
            <tr>
                <td class="tdbgt">蓋反防呆CCD<br />( Wrong Marking Orientation Foolproof )</td>
                <td class="tdCon">正常 Normal<br />
                    <asp:RadioButton ID="rdoWrongMarkingCCD_Y" runat=  "server" Text="V 正常" 
                        GroupName="C" CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoWrongMarkingCCD_N" runat="server" Text="X 異常" 
                        GroupName="C" CssClass="radio inline"/>
                </td>
                <td class="tdbgt">影像檢查CCD <br />( Image Inspection CCD )</td>
                <td class="tdCon">
                    正常 Normal<br />
                    <asp:RadioButton ID="rdoImageInspection_Y" runat="server" Text="V 正常" 
                        GroupName="Z" CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoImageInspection_N" runat="server" Text="X 異常" 
                        GroupName="Z" CssClass="radio inline"/>
                 </td>
            </tr>
            <tr>
                <td class="tdbgt">雷射能量<br /> ( Laser Power )</td>
                <td class="tdCon">
                        7.0~17.0 Watt(AMCC/E&amp;R)
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
                <td class="tdbgt" >濾網次數( AMCC )<br /> ( Filter Counter )</td>
                <td class="tdCon"> &lt;參數設定 ( &lt;Parameter Setting )<br /> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtFilterCounter" runat="server"/> </td>
            </tr>
            <tr>
                <td class="tdbgt">影像辨認 <br />( 含pin1方向，蓋印位置與漏蓋 )<br />CCD Function (pin1 Rotation, Marking Position and  No Marking Included )</td>
                <td class="tdCon">正常 Normal<br />
                    <asp:RadioButton ID="rdoCcdFunction_Y" runat="server" Text="V 正常" GroupName="D" 
                        CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoCcdFunction_N" runat="server" Text="X 異常" GroupName="D" 
                        CssClass="radio inline"/>
                </td>
                <td class="tdbgt">工作桌接地線是否連結<br /> Ground Wrist Connect with Work Table</td>
                <td class="tdCon">
                    連結 Connect<br />
                    <asp:RadioButton ID="rdoGroundWristConnectTable_Y" runat="server" Text="V 是" 
                        GroupName="E" CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoGroundWristConnectTable_N" runat="server" Text="X 否" 
                        GroupName="E"  CssClass="radio inline"/>
                </td>
            </tr>
            <tr>
                <td class="tdbgt">機台離子風扇<br />是否有轉動且方向需朝產品方向<br />Electrostatic Discharge (ESD) Check</td>
                <td class="tdCon"><asp:DropDownList runat="server" ID="ddlEsdCheck"/></td>
                <td class="tdbgt">安全門檢查<br /> ( Safe Door Function )</td>
                <td class="tdCon">正常 Normal<br />
                    <asp:RadioButton ID="rdoSafedoor_Y" runat="server" Text="V 正常" GroupName="H" 
                        CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoSafedoor_N" runat="server" Text="X 異常" GroupName="H"  
                        CssClass="radio inline"/>
                </td>
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
                <td class="tdbgt">--</td>
                <td class="tdCon">--</td>
            </tr>   
            <tr>
                <td colspan="4">
                    <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" OnClientClick="return confirm('確定送出表單?');" CssClass="btn btn-primary"/>
                    <asp:Button ID="btnUpdateSubmit" Text="Submit" runat="server" OnClientClick="return confirm('確定送出表單?');" CssClass="btn btn-primary" Visible="false" OnClick="btnUpdateSubmit_Click"/>
                    <asp:Button ID="btnReset" Text="Reset" runat="server" OnClientClick ="return confirm('確定把資料清空?');" CssClass= "btn btn-inverse" OnClick="btnReset_Click"/>
                    <asp:Button ID="btnSwitch" runat="server" Text="Quick Mode" CssClass= "btn btn-success" OnClick="btnSwitch_Click" visible="false"/>
                    <asp:Button ID="btnMail" runat="server" Text="Mail Set" CssClass= "btn btn-warning" OnClientClick="open_win();"/>
                    
                    <script type="text/javascript">
                    function open_win(){
                        window.open('../others/MailSet.aspx?sheet_categoryID=<%=sheet_categoryID %>', '_blank', '_blank', 'width=1100,height=700,scrollbars=yes');
                    }
                    </script>
                </td>
            </tr>
        </table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

