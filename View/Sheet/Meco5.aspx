<%@ Page AutoEventWireup="true" CodeFile="Meco5.aspx.cs" Inherits="ESC_Web.Alan.Meco5"
    Language="C#" MasterPageFile="../Master/MasterPage.master" Title="Meco5" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:UpdatePanel id="up" runat="server">
   <ContentTemplate>  
    <h1>MECO#5  霧錫電鍍檢查表<br /> Matte Tin Plating Check List </h1>
    <table id='autoTable' >
        <tr>
            <td style="text-align:right;" class="tdbgt">機台編號 Machine No.：</td>
            <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtMachine" Enabled="true" runat="server" CssClass="input-large"></asp:TextBox></td>
            <td style="text-align:right;" class="tdbgt">機台位置 Location.：</td>
            <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtLocation" Enabled="true" runat="server" CssClass="input-large"></asp:TextBox></td>
        </tr>
        <tr>
            <td style="text-align:right;" class="tdbgt">時間班別 Time / Shift：</td>
            <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtTimeShift" Enabled="true" runat="server" CssClass="input-large"></asp:TextBox></td>
            <td style="text-align:right;" class="tdbgt">操作人員 Operator：</td>
            <td> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtOperator" runat="server" CssClass="input-large" Enabled="true"></asp:TextBox></td>
        </tr>
    </table>
    <br />  
    <table class="content_table" style="width:80%;">            
            <tr>
                <td class="tdbgt">檢查時機<br />Change Occasion </td>
                <td class="tdCon" colspan="3"><asp:DropDownList ID="ddlStatus" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"></asp:DropDownList></td>
            </tr>
            <tr>
                <td class="tdbgt">日期<br /> Date</td>
                <td class="tdCon" colspan="3"> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtDate" runat="server" CssClass="input-date" Enabled="false"/></td>
            </tr>
            <tr>
                <td class="tdbgt">流程卡<br />
                    Batch Card No.</td>
                <td class="tdCon" colspan="3"> <asp:TextBox onKeyDown="preventTextEnterEvent();" id="txtBatchCardNo" runat="server"  CssClass="input-xlarge" AutoPostBack="True"  OnTextChanged="txtBatchCardNo_TextChanged"/></td>
            </tr>  
            <tr>
                <td class="tdTitle" colspan="4"><strong>檢查項目 Check Items</strong></td>
            </tr>
            <tr>
                <td rowspan="2" class="tdbgt" style="width:30%;">檢查下槽液位是否足夠<br />Check Solution Level of Sump Tanks<br /></td>
                <td class="tdCon" rowspan="2" style="width:19%;">
                    正常 Normal <br />
                    <asp:RadioButton ID="rdoSumpTank_Y" runat="server" Text="V 正常" GroupName="A" 
                        CssClass="radio inline" />&nbsp;
                    <asp:RadioButton ID="rdoSumpTank_N" runat="server" Text="X 異常" GroupName="A"  
                        CssClass="radio inline"/>&nbsp;
                    <asp:RadioButton ID="rdoSumpTank_R" runat="server" Text="Ready x" GroupName="A"  CssClass="radio inline"/>
                </td>
                <td rowspan="2" class="tdbgt" style="width:30%;">高壓噴水壓力  <br />30~70 bar High 
                    pressure rinse </td>
                <td class="tdCon" style="width:20%;">
                    右噴嘴正常 Normal<br />
                    <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtRightRinse" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>                
                <td class="tdCon">
                    左噴嘴正常 Normal<br />
                    <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtLeftRinse" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tdbgt">檢查各風刀無堵塞<br /> Check Blockage In Air Knife</td>
                <td class="tdCon" style="width: 19%">
                    正常 Normal<br />
                    <asp:RadioButton ID="rdoBlockageAir_Y" runat="server" Text="V 正常" GroupName="K" 
                        CssClass="radio inline" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoBlockageAir_N" runat="server" Text="X 異常" GroupName="K" 
                        CssClass="radio inline" />
                </td>
                <td class="tdbgt">檢查各水刀無堵塞<br /> Check Blockage On Rinse Nozzle</td>
                <td class="tdCon">
                    正常 Normal <br /> 
                    <asp:RadioButton ID="rdoBlockageRinse_Y" runat="server" Text="V 正常" 
                        GroupName="L"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoBlockageRinse_N" runat="server" Text="X 異常" 
                        GroupName="L"  CssClass="radio inline"/>
                </td>                
            </tr>
            <tr>
                <td class="tdbgt">噴鋼帶水柱(7-1,7-3)<br /> Water Nozzle On Belt</td>
                <td class="tdCon" style="width: 19%">
                    無阻塞 No blockage<br />
                    <asp:RadioButton ID="rdoBlockageNozzle_Y" runat="server" Text="V 無" 
                        GroupName="M"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoBlockageNozzle_N" runat="server" Text="X 有" 
                        GroupName="M" CssClass="radio inline" />
                </td>
                <td class="tdbgt">各理水刀垂直噴水(7-4流量80 l/hr以上)<br />Vertical To Belt On DI Rinse (7-4 flow rate over 80 l/hr)</td>
                <td class="tdCon">
                    垂直Vertical <br />
                    <asp:RadioButton ID="rdoVertical_Y" runat="server" Text="V 是" GroupName="N" CssClass="radio inline" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoVertical_N" runat="server" Text="X 否" GroupName="N" CssClass="radio inline" />
                </td>                
            </tr>
            <tr>
                <td class="tdbgt">檢查各槽的液位<br /> Check Solution Level</td>
                <td class="tdCon" style="width: 19%">
                    接觸鋼帶 Contact with belt <br /> 
                    <asp:RadioButton ID="rdoSolutionLevel_Y" runat="server" Text="V 是" GroupName="O" CssClass="radio inline" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoSolutionLevel_N" runat="server" Text="X 否" GroupName="O" CssClass="radio inline" />
                </td>
                <td class="tdbgt">檢查各藥水槽比重/PHCheck Chemical Spec. gr./PH<br />Deflash LCT 101N (Meco# 5)-----1.030 ~ 1.080 (g/cm3)</td>
                <td class="tdCon"> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCheckPH" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tdbgt">PH值- LCT 101N (2槽)<br />----- 8.0  ~ 10.0</td>
                <td class="tdCon" style="width: 19%"> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCheckLCT101N" runat="server"></asp:TextBox></td>
                <td class="tdbgt">調整劑添加量 (ml)</td>
                <td class="tdCon"> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtAdjustML" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tdbgt">調整劑添加後PH值<br />----- 8.0 ~ 10.0</td>
                <td class="tdCon" style="width: 19%"> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtAfterAdjustPH" runat="server"></asp:TextBox></td>
                <td class="tdbgt">3槽硫酸 H2SO4<br />-----1.030 ~ 1.150 (g/cm3)</td>
                <td class="tdCon"> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCheckH2SO4" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tdbgt">4槽 H2SO4 (PPF)<br />-----1.080 ~ 1.200(g/cm3)</td>
                <td class="tdCon" style="width: 19%"> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCheckPPF" runat="server"></asp:TextBox></td>
                <td class="tdbgt">4槽LCT-208S (Cu) <br />1.030 ~ 1.050(g/cm3)</td>
                <td class="tdCon"> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCheckCu" runat="server"></asp:TextBox></td>
            </tr>
            <tr>                
                <td class="tdbgt">Plating (6槽)<br />Above  1.150 以上 (g/cm3)</td>
                <td class="tdCon" style="width: 19%"> <asp:TextBox onKeyDown="preventTextEnterEvent();" ID="txtCheckPlating" runat="server"></asp:TextBox></td>
                <td class="tdbgt">Clean Filter Mesh 清洗過濾網<br /> 2-1//4*2, 2-5*1, 2-6*1</td>
                <td class="tdCon">
                    清潔 Clean<br />
                    <asp:RadioButton ID="rdoCleanFilter_Y" runat="server" Text="V 是" GroupName="D"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoCleanFilter_N" runat="server" Text="X 否" GroupName="D"  CssClass="radio inline"/>
                </td>
            </tr>
            <tr>
                <td class="tdbgt">換水 6-6, 6-7, 6-8,<br />11-4/7,2-1 Water Renew </td>
                <td class="tdCon" style="width: 19%">
                    換新 Renew<br />
                    <asp:RadioButton ID="rdoWaterRenew_Y" runat="server" Text="V 是" GroupName="E"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoWaterRenew_N" runat="server" Text="X 否" GroupName="E"  CssClass="radio inline"/>
                </td>
                <td class="tdbgt">添加1Kg磷酸三鈉於7-1槽<br />Replenish Na3PO4 1 Kg In Tank 7-1</td>
                <td class="tdCon">
                    添加 Replenish <br />
                    <asp:RadioButton ID="rdoReplenishNa_Y" runat="server" Text="V 是" GroupName="H"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoReplenishNa_N" runat="server" Text="X 否" GroupName="H"  CssClass="radio inline"/>
                </td>                
            </tr>
            <tr>
                <td class="tdbgt">7-1, 7-2, 7-3 換新 (I) / 每週<br />Tank 7-1, 7-2, 7-3 Renew  (I)</td>
                <td class="tdCon" style="width: 19%">
                    換新 Renew <br />
                    <asp:RadioButton ID="rdoTankRenew_Y" runat="server" Text="V 是" GroupName="F" CssClass="radio inline" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoTankRenew_N" runat="server" Text="X 否" GroupName="F"  CssClass="radio inline"/>
                </td>
                <td class="tdbgt">添加補充純錫半球<br />Replenish Sn ball In Anode Basket <br />I (6-1) II(6-2,3) III(6-4,5)</td>
                <td class="tdCon">
                    <asp:RadioButton ID="rdoReplenishSn_Y" runat="server" Text="V 是" GroupName="I"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoReplenishSn_N" runat="server" Text="X 否" GroupName="I"  CssClass="radio inline"/>
                </td>                
            </tr>   
            <tr>
                <td class="tdbgt">5-1預鍍槽添加(I) / 每天<br />Replenish HC In Tank 5-1  (I)</td>
                <td class="tdCon" style="width: 19%">
                    添加 Replenish <br />
                    <asp:RadioButton ID="rdoReplenishHC_Y" runat="server" Text="V 是" GroupName="G"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoReplenishHC_N" runat="server" Text="X 否" GroupName="G"  CssClass="radio inline"/>
                </td>
                <td class="tdbgt">5-1預鍍槽更新(II)/每月1,15日<br />Replenish HC In Tank 5-1  <br />(II) 1st & 15th / Month</td>
                <td class="tdCon">
                    換新 Renew <br />
                    <asp:RadioButton ID="rdoReplenishHcHalfMonth_Y" runat="server" Text="V 是" GroupName="U1"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoReplenishHcHalfMonth_N" runat="server" Text="X 否" GroupName="U1"  CssClass="radio inline"/>
                </td>
            </tr>  
            <tr>
                <td class="tdbgt">4槽-LCT-208S <br />添加 (III) / 天 Replenish</td>
                <td class="tdCon" style="width: 19%">
                    添加 Replenish <br />
                    <asp:RadioButton ID="rdoLctDay_Y" runat="server" Text="V 是" GroupName="U2"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoLctDay_N" runat="server" Text="X 否" GroupName="U2"  CssClass="radio inline"/>
                </td>
                <td class="tdbgt">4槽-LCT-208S <br />換新 (II) / 週wk Renew</td>
                <td class="tdCon">
                    換新 Renew <br />
                    <asp:RadioButton ID="rdoLctWeek_Y" runat="server" Text="V 是" GroupName="U3"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoLctWeek_N" runat="server" Text="X 否" GroupName="U3"  CssClass="radio inline"/>
                </td>
            </tr> 
            <tr>
                <td class="tdbgt">4槽PPF-H2SO4 <br />添加 (II) / 天 Replenish</td>
                <td class="tdCon" style="width: 19%">
                    添加 Replenish <br />
                    <asp:RadioButton ID="rdoPpfDay_Y" runat="server" Text="V 是" GroupName="U4"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoPpfDay_N" runat="server" Text="X 否" GroupName="U4"  CssClass="radio inline"/>
                </td>
                <td class="tdbgt">4槽PPF-H2SO4 <br />換新 (II) / 週wk Renew</td>
                <td class="tdCon">
                    換新 Renew <br />
                    <asp:RadioButton ID="rdoPpfWeek_Y" runat="server" Text="V 是" GroupName="U5"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoPpfWeek_N" runat="server" Text="X 否" GroupName="U5"  CssClass="radio inline"/>
                </td>
            </tr>       
            <tr>
                <td class="tdbgt">抽風系統<br /> Exhaust </td>
                <td class="tdCon" style="width: 19%">
                    運轉 Open <br /> 
                    <asp:RadioButton ID="rdoExhaust_Y" runat="server" Text="V 是" GroupName="P"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoExhaust_N" runat="server" Text="X 否" GroupName="P" CssClass="radio inline" />
                </td>
                <td class="tdbgt">鋼架運轉安全門(上下料)<br />Magazine Holder Safety Door (LD-UL) </td>
                <td class="tdCon">
                    關閉 Close <br />
                    <asp:RadioButton ID="rdoSafedoorClose_Y" runat="server" Text="V 是" GroupName="Q"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoSafedoorClose_N" runat="server" Text="X 否" GroupName="Q" CssClass="radio inline" />
                </td>
            </tr>
            <tr>
                <td class="tdbgt">更換型別吸嘴 <br />Change PKG Vacuum Gripper   </td>
                <td class="tdCon" style="width: 19%">
                    無破損 No Broken<br />  
                    <asp:RadioButton ID="rdoChangeGripper_Y" runat="server" Text="V 是" GroupName="R"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoChangeGripper_N" runat="server" Text="X 否" GroupName="R"  CssClass="radio inline"/>
                </td>
                <td class="tdbgt">擺放產品接地線<br /> ESD Protect   </td>
                <td class="tdCon" colspan="3">
                    連結 Connect  <br /> 
                    <asp:RadioButton ID="rdoEsdConnect_Y" runat="server" Text="V 是" GroupName="S1"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoEsdConnect_N" runat="server" Text="X 否" GroupName="S1"  CssClass="radio inline"/>
                </td>
            </tr>
            <tr>
                <td class="tdTitle" colspan="2"><strong>產品製程檢查( EBV-5-1-1/132 )</strong></td>
                <td class="tdTitle" colspan="2"><strong>檢驗樣本 Sample: 5條/ 鋼架5 Strips / Mag</strong></td>
            </tr>
            <tr>
                <td class="tdbgt" colspan="2">電鍍腳沾污  <br />Contamination Lead </td>
                <td class="tdCon" colspan="2">
                    <asp:RadioButton ID="rdoContamination_Y" runat="server" Text="合格" GroupName="V" CssClass="radio inline" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoContamination_N" runat="server" Text="不合格" GroupName="V" CssClass="radio inline" />
                </td>
            </tr>
            <tr>
                <td class="tdbgt" colspan="2">錫鬚<br />Solder Bridging</td>
                <td class="tdCon" colspan="2">
                    <asp:RadioButton ID="rdoSolderBridging_Y" runat="server" Text="合格" GroupName="U"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoSolderBridging_N" runat="server" Text="不合格" GroupName="U"  CssClass="radio inline"/>
                </td>
            </tr>
            <tr>
                <td class="tdbgt" colspan="2">鍍不全<br /> Insuff.  Lead Finish  </td>
                <td class="tdCon" colspan="2">
                    <asp:RadioButton ID="rdoInsuffLead_Y" runat="server" Text="合格" GroupName="X"  CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoInsuffLead_N" runat="server" Text="不合格" GroupName="X"  CssClass="radio inline"/>
                </td>
            </tr>
            <tr>
                <td class="tdbgt" colspan="2">其他<br />Others</td>
                <td class="tdCon" colspan="2">
                    <asp:RadioButton ID="rdoOthers_Y" runat="server" Text="合格" GroupName="Y" CssClass="radio inline"/>&nbsp;&nbsp;
                    <asp:RadioButton ID="rdoOthers_N" runat="server" Text="不合格" GroupName="Y" CssClass="radio inline"/>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="btnSubmit" Text="Submit" runat="server"  OnClientClick="return confirm('確定送出表單?');" CssClass="btn btn-primary" OnClick="btnSubmit_Click"/>
                    <asp:Button ID="btnUpdateSubmit" Text="Submit" runat="server" OnClientClick="return confirm('確定送出表單?');" CssClass="btn btn-primary" Visible="false" OnClick="btnUpdateSubmit_Click"/>
                    <asp:Button ID="btnReset" Text="Reset" runat="server" OnClientClick="return confirm('確定把資料清空?');"  CssClass= "btn btn-inverse" OnClick="btnReset_Click"/>
                    <asp:Button ID="btnSwitch" runat="server" Text="Quick Mode" CssClass= "btn btn-success" OnClick="btnSwitch_Click" visible="false"/>
                    <asp:Button ID="btnMail" runat="server" Text="Mail Set" CssClass= "btn btn-warning" OnClientClick="open_win();"/>
                    
                    <script type="text/javascript">
                        function open_win() {
                            window.open('../others/MailSet.aspx?sheet_categoryID=<%=sheet_categoryID %>', '_blank', 'width=1100,height=700,scrollbars=yes');
                        }
                    </script>
                </td>
            </tr>
        </table>
    V : Pass  合格   X : Reject  不合格   X : Ready for operation  已校正, 可 操作    EBV-3-10-62/402
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

