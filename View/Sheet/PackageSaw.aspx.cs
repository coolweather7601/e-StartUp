using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;

namespace ESC_Web.Alan
{
    public partial class PackageSaw : System.Web.UI.Page
    {        
        public static string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["ESC"].ToString();
        public Common.AdoDbConn ado;
        public static StringBuilder sb = new StringBuilder();

        public static string sheet_categoryID, sheetID;
        //if(string.isNull(sheetID)){Insert_Mode} else { Update_Mode}

        public enum PackageType : int
        {
            TFBGA = 1, LFBGA = 2, HLQFN = 3,
            DHVQFN = 4, HVQFN = 5, HWQFN = 6, HXQFN = 7, XQFN = 8,
            HVSON = 9, HVSON12 = 10
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sheet_categoryID = Request.QueryString["sheet_categoryID"];
                sheetID = Request.QueryString["sheetID"];


                //==================================================================================================
                //DropdownList Data Bind
                //==================================================================================================
                ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
                string str = "select * from ddlitems where category='status'";
                DataTable dt = new DataTable();
                dt = ado.loadDataTable(str, null, "ddlitems");
                foreach (DataRow dr in dt.Rows)
                {
                    //Start up | After repair/adjustment check | (HVQFN <--> BGA) | After change blade     
                    if (dr["ddlItemsID"].ToString().Equals("7") || dr["ddlItemsID"].ToString().Equals("10") || dr["ddlItemsID"].ToString().Equals("11") || dr["ddlItemsID"].ToString().Equals("12"))
                    {
                        ddlStatus.Items.Add(new ListItem(dr["describes"].ToString()));
                    }
                }
                ddlStatus.Items.Insert(0, new ListItem("請選擇", "0"));


                //==================================================================================================
                //Check Role
                //==================================================================================================
                if (string.IsNullOrEmpty(sheetID)) { initial(); } else { UpdateMode(); }  
                Common.Comfunc func = new Common.Comfunc();
                func.checkLogin();
                func.checkRole(Page.Master);
                
            }
        }
        private void UpdateMode()
        {
            ContentPlaceHolder PlaceHolder2 = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder2");
            Common.Comfunc func = new Common.Comfunc();
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
            string sqlStr = string.Format(@"Select aspxControlID,value,DataTypeID,tester,location,timeShift,op
                                            From vw_sheet_value
                                            Where sheet_categoryID='{0}' And sheetID='{1}'", sheet_categoryID, sheetID);
            DataTable dt = ado.loadDataTable(sqlStr, null, "vw_sheet_value");

            foreach (DataRow dr in dt.Rows)
            {
                switch (dr["datatypeID"].ToString())
                {
                    case "1": //TxtBox
                        TextBox txt = (TextBox)PlaceHolder2.FindControl(dr["aspxControlID"].ToString());
                        txt.Text = dr["value"].ToString();
                        break;

                    case "2": //RadioBox
                        RadioButton rdo_Y = (RadioButton)PlaceHolder2.FindControl(dr["aspxControlID"].ToString() + "_Y");
                        RadioButton rdo_N = (RadioButton)PlaceHolder2.FindControl(dr["aspxControlID"].ToString() + "_N");
                        if (dr["value"].ToString().Equals("Y")) { rdo_Y.Checked = true; }
                        else if (dr["value"].ToString().Equals("N")) { rdo_N.Checked = true; }
                        break;

                    case "3": //DropDownList
                        DropDownList ddl = (DropDownList)PlaceHolder2.FindControl(dr["aspxControlID"].ToString());
                        ddl.SelectedValue = dr["value"].ToString();
                        break;
                }
                txtMachine.Text = dr["tester"].ToString();
                txtLocation.Text = dr["location"].ToString();
                txtTimeShift.Text = dr["timeShift"].ToString();
                txtOperator.Text = dr["op"].ToString();
            }

            btnSubmit.Visible = false;
            btnUpdateSubmit.Visible = true;

            DifferentView();
        }
        private void initial()
        {
            //external data(from ACS)
            Common.Comfunc func = new Common.Comfunc();
            func.getAcsDataByBatchNo("", Page.Master);

            jsShow(string.Format(@"alert('{0}');", func.getLastTime(sheet_categoryID, txtMachine.Text.Trim(), txtLocation.Text.Trim())));

            DifferentView();


            //==================================================================================================
            //檢查前一班如果有over spec的情形，期間內是否有重做after repair或是after change blade，若無維護提醒並寄信
            //==================================================================================================
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
            string str = string.Format(@"Select v.tester,v.aspxControlid,v.value,v.sheetID,l.time 
                                         From vw_sheet_value v 
                                         Inner Join log l on v.sheetID=l.sheetID
                                         Where v.sheet_categoryid='{0}' 
                                               And v.tester='{1}' 
                                               And v.sheetID in (Select sheetid
                                                               From vw_sheet_value vw
                                                               Where vw.aspxcontrolid='ddlStatus' And vw.value like '%開機%')
                                               And v.sheetID = (Select max(sheetID)
                                                              From sheet
                                                              Where sheet_categoryID='{0}')", sheet_categoryID, txtMachine.Text.Trim());
            DataTable dt = ado.loadDataTable(str, null, "vw_sheet_value");
            if (dt.Rows.Count < 1) return;

            string strRepair = string.Format(@"Select v.tester,v.aspxControlid,v.value,v.sheetID,l.time 
                                               From vw_sheet_value v
                                               Inner join log l on v.sheetid=l.sheetid 
                                               Where sheet_categoryid='{0}' 
                                                     And tester='{1}'
                                                     And l.time <sysdate and l.time> to_date('{2}','yy-mm-dd hh24:mi:ss') 
                                                     And l.logAction='INSERT'
                                                     And v.sheetID in (Select sheetid
                                                                       From vw_sheet_value vw
                                                                       Where vw.aspxcontrolid='ddlStatus' And vw.value like '%維修%')
                                                     And v.sheetID = (Select max(sheetID)
                                                                      From sheet
                                                                      Where sheet_categoryID='{0}')", sheet_categoryID,
                                                                                                    txtMachine.Text.Trim(), 
                                                                                                    Convert.ToDateTime(dt.Rows[0]["time"]).ToString("yyyy/MM/dd HH:mm:ss"));

            string strBlade = string.Format(@"Select v.tester,v.aspxControlid,v.value,v.sheetID,l.time 
                                              From vw_sheet_value v
                                              Inner join log l on v.sheetid=l.sheetid 
                                              Where sheet_categoryid='{0}' 
                                                    And tester='{1}'
                                                    And l.time <sysdate and l.time> to_date('{2}','yy-mm-dd hh24:mi:ss') 
                                                    And l.logAction='INSERT'
                                                    And v.sheetID in (Select sheetid
                                                                      From vw_sheet_value vw
                                                                      Where vw.aspxcontrolid='ddlStatus' And vw.value like '%刀片%')
                                                    And v.sheetID = (Select max(sheetID)
                                                                     From sheet
                                                                     Where sheet_categoryID='{0}')", sheet_categoryID,
                                                                                                    txtMachine.Text.Trim(),
                                                                                                    Convert.ToDateTime(dt.Rows[0]["time"]).ToString("yyyy/MM/dd HH:mm:ss"));
            DataTable dtRepair = ado.loadDataTable(strRepair, null, "vw_sheet_value");//after repair/adjustment
            DataTable dtBlade = ado.loadDataTable(strBlade, null, "vw_sheet_value");//after change blade

            StringBuilder sb = new StringBuilder();
            string tester= txtMachine.Text.Trim().ToUpper();
            string BladeExposure1 = "", BladeExposure2 = "", packageStr = "", feedSpeed = "", Co2Bubbler = "",
                   bladeChipping = "", coolWater = "", bladeDeviation = "", emulsion="";
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["aspxControlID"].ToString().Equals("txtBladeExposure1")) { BladeExposure1 = dr["value"].ToString(); }
                if (dr["aspxControlID"].ToString().Equals("txtBladeExposure2")) { BladeExposure2 = dr["value"].ToString(); }
                if (dr["aspxControlID"].ToString().Equals("txtPackage")) { packageStr = dr["value"].ToString(); }
                if (dr["aspxControlID"].ToString().Equals("txtFeedSpeed")) { feedSpeed = dr["value"].ToString(); }
                if (dr["aspxControlID"].ToString().Equals("txtCo2Bubbler")) { Co2Bubbler = dr["value"].ToString(); }
                if (dr["aspxControlID"].ToString().Equals("txtbladeChipping")) { bladeChipping = dr["value"].ToString(); }
                if (dr["aspxControlID"].ToString().Equals("txtCoolWater")) { coolWater = dr["value"].ToString(); }
                if (dr["aspxControlID"].ToString().Equals("txtBladeDeviation")) { bladeDeviation = dr["value"].ToString(); }
                if (dr["aspxControlID"].ToString().Equals("txtEmulsion")) { emulsion = dr["value"].ToString(); }  
            }
            

            //Check Urgent status
            #region 目前刀刃長度
            //Jig tester = QS
            //"LFBGA / TFBGA : > 2.2 mm (Jig)
            //HLQFN > 2.2 jig
            //DHVQFN / HWQFN / HVQFN : > 1.8 mm (Jig)
            //Tape tester = DS
            //LFBGA / TFBGA : > 1.8 (tape)
            //HLQFN (tape) : > 2.1 mm
            //HVSON / HVQFN : > 1.35 mm (tape)
            //XQFN : > 1.0 mm (tape)"                          
            if (packageStr.Contains(PackageType.LFBGA.ToString()) || packageStr.Contains(PackageType.TFBGA.ToString()))
            {
                if (tester.ToUpper().Contains("DS") && Convert.ToDouble(BladeExposure1) < 1.8 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}/{1}(Tape)目前Z1刀刃長度必須 > 1.8mm，請換刀】", PackageType.LFBGA.ToString(), PackageType.TFBGA.ToString())); }
                if (tester.ToUpper().Contains("DS") && Convert.ToDouble(BladeExposure2) < 1.8 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}/{1}(Tape)目前Z2刀刃長度必須 > 1.8mm，請換刀】", PackageType.LFBGA.ToString(), PackageType.TFBGA.ToString())); }
                if (tester.ToUpper().Contains("QS") && Convert.ToDouble(BladeExposure1) < 2.2 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}/{1}(Jig)目前Z1刀刃長度必須 > 2.2mm，請換刀】", PackageType.LFBGA.ToString(), PackageType.TFBGA.ToString())); }
                if (tester.ToUpper().Contains("QS") && Convert.ToDouble(BladeExposure2) < 2.2 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}/{1}(Jig)目前Z2刀刃長度必須 > 2.2mm，請換刀】", PackageType.LFBGA.ToString(), PackageType.TFBGA.ToString())); }
            }
            else if (packageStr.Contains(PackageType.HLQFN.ToString()))
            {
                if (tester.ToUpper().Contains("DS") && Convert.ToDouble(BladeExposure1) < 2.1 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}(Tape)目前Z1刀刃長度必須 > 2.1mm，請換刀】", PackageType.HLQFN.ToString())); }
                if (tester.ToUpper().Contains("DS") && Convert.ToDouble(BladeExposure2) < 2.1 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}(Tape)目前Z2刀刃長度必須 > 2.1mm，請換刀】", PackageType.HLQFN.ToString())); }
                if (tester.ToUpper().Contains("QS") && Convert.ToDouble(BladeExposure1) < 2.2 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}(Jig)目前Z1刀刃長度必須 > 2.2mm，請換刀】", PackageType.HLQFN.ToString())); }
                if (tester.ToUpper().Contains("QS") && Convert.ToDouble(BladeExposure2) < 2.2 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}(Jig)目前Z2刀刃長度必須 > 2.2mm，請換刀】", PackageType.HLQFN.ToString())); }
            }
            else if (packageStr.Contains(PackageType.DHVQFN.ToString()) || packageStr.Contains(PackageType.HWQFN.ToString()) || packageStr.Contains(PackageType.HVQFN.ToString()))
            {
                if (tester.ToUpper().Contains("QS") && Convert.ToDouble(BladeExposure1) < 1.8 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}/{1}/{2}(Jig)目前Z1刀刃長度必須 > 1.8mm，請換刀】", PackageType.DHVQFN.ToString(), PackageType.HWQFN.ToString(), PackageType.HVQFN.ToString())); }
                if (tester.ToUpper().Contains("QS") && Convert.ToDouble(BladeExposure2) < 1.8 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}/{1}/{2}(Jig)目前Z2刀刃長度必須 > 1.8mm，請換刀】", PackageType.DHVQFN.ToString(), PackageType.HWQFN.ToString(), PackageType.HVQFN.ToString())); }
            }
            else if (packageStr.Contains(PackageType.HVSON.ToString()) || packageStr.Contains(PackageType.HVQFN.ToString()))
            {
                if (tester.ToUpper().Contains("DS") && Convert.ToDouble(BladeExposure1) < 1.35 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}/{1}(Tape)目前Z1刀刃長度必須 > 1.35mm，請換刀】", PackageType.HVSON.ToString(), PackageType.HVQFN.ToString())); }
                if (tester.ToUpper().Contains("DS") && Convert.ToDouble(BladeExposure2) < 1.35 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}/{1}(Tape)目前Z2刀刃長度必須 > 1.35mm，請換刀】", PackageType.HVSON.ToString(), PackageType.HVQFN.ToString())); }
            }
            else if (packageStr.Contains(PackageType.XQFN.ToString()))
            {
                if (tester.ToUpper().Contains("DS") && Convert.ToDouble(BladeExposure1) < 1.0 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}(Tape)目前Z1刀刃長度必須 > 1.0mm，請換刀】", PackageType.XQFN.ToString())); }
                if (tester.ToUpper().Contains("DS") && Convert.ToDouble(BladeExposure2) < 1.0 && dtBlade.Rows.Count < 1) { sb.Append(string.Format(@"【{0}(Tape)目前Z2刀刃長度必須 > 1.0mm，請換刀】", PackageType.XQFN.ToString())); }
            }
            #endregion
            #region 進刀速度
            //- substrate: TFBGA / LFBGA / HLQFN
            //  BT-substrate < 100mm/s
            //- leadframe: HVQFN / HWQFN / HXQFN / XQFN / DHVQFN
            //  Leadframe 10-50 mm/s
            if (packageStr.Contains(PackageType.TFBGA.ToString()) || packageStr.Contains(PackageType.LFBGA.ToString()) || packageStr.Contains(PackageType.HLQFN.ToString()))
            {
                if (Convert.ToDouble(feedSpeed) > 100 && dtRepair.Rows.Count < 1)
                {
                    sb.Append(string.Format(@"【{0}/{1}/{2}進刀速度BT-substrate應為 < 100mm/s，不可切】", PackageType.TFBGA.ToString(), PackageType.LFBGA.ToString(), PackageType.HLQFN.ToString()));
                }
            }
            else if (packageStr.Contains(PackageType.HVQFN.ToString()) || packageStr.Contains(PackageType.HWQFN.ToString()) || packageStr.Contains(PackageType.HXQFN.ToString())
                     || packageStr.Contains(PackageType.XQFN.ToString()) || packageStr.Contains(PackageType.DHVQFN.ToString()))
            {
                if ((Convert.ToDouble(feedSpeed) < 10 || Convert.ToDouble(feedSpeed) > 50) && dtRepair.Rows.Count < 1)
                {
                    sb.Append(string.Format(@"【{0}/{1}/{2}/{3}/{4}進刀速度Leadframe應為 10-50 mm/s，不可切】", PackageType.HVQFN.ToString(), PackageType.HWQFN.ToString(), PackageType.HXQFN.ToString(), PackageType.XQFN.ToString(), PackageType.DHVQFN.ToString()));
                }
            }
            #endregion
            #region 刃長差異、切削液濃度
            // #601 / #602 / #603 / #701 ~ #707 
            //QS-007 / QS-009 / QS-012/ QS-014 / QS-013 / QS-006 / QS-015 / QS-005 / QS-008 / QS-011
            if (tester.Contains("QS-007") || tester.Contains("QS-009") || tester.Contains("QS-012") ||
                tester.Contains("QS-014") || tester.Contains("QS-013") || tester.Contains("QS-006") ||
                tester.Contains("QS-015") || tester.Contains("QS-005") || tester.Contains("QS-008") ||
                tester.Contains("QS-011"))
            {
                if (Convert.ToDouble(bladeDeviation) > 0.2 && dtBlade.Rows.Count < 1) { sb.Append("【刃長差異必須 < 0.2mm，請換刀】"); }
                
                if (!txtPackage.Text.Trim().Contains(PackageType.TFBGA.ToString()) && !txtPackage.Text.Trim().Contains(PackageType.LFBGA.ToString()))
                {
                    if (Convert.ToDouble(emulsion) < 2.2 && dtRepair.Rows.Count < 1) { sb.Append("【切削液濃度太低，必須加切削液】"); }
                }
            }
            #endregion
            #region CO2 阻抗值 0.2 ~ 0.6 Mohm(HVSON12 只能在 TS05 (有 CO2 bubbler 的機台) 上切)
            //#SA610 -> #SA609 (2014-06-11)
            //DS-053 -> DS-050
            if (tester.Contains("DS-050") && packageStr.Contains(PackageType.HVSON.ToString()))
            {
                if (packageStr.Contains(PackageType.HVSON12.ToString()))
                {
                    if (Convert.ToDouble(Co2Bubbler) < 0.2 || Convert.ToDouble(Co2Bubbler) > 0.6) { sb.Append(string.Format(@"【{0}CO2 阻抗值 (Mohm)必須為0.2 ~ 0.6 Mohm，請報修】", PackageType.HVSON12.ToString())); }
                }
            }
            #endregion
            if (Convert.ToDouble(bladeChipping) > 1 && dtBlade.Rows.Count < 1) { sb.Append("【刀片破損狀況 > 1.0 cm，請換刀】"); }
            if (Convert.ToDouble(coolWater) < 1 && dtRepair.Rows.Count < 1) { sb.Append("【冷卻水量檢查水量太低，請報修】"); }


            //mail
            if (sb.Length > 0)
            {
                sb.Insert(0, "");
                jsShow(string.Format(@"alert('{0}');", sb.ToString()));

                ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
                string str_mail = string.Format(@"Select s.sheetID,s.sheet_categoryID,sc.describes 
                                                      From sheet s 
                                                      Inner join sheet_category sc on s.sheet_categoryid=sc.sheet_categoryid
                                                      Where s.sheet_categoryID='{0}'
                                                      Order By s.sheetID Desc", sheet_categoryID);
                DataTable dt_mail = ado.loadDataTable(str_mail, null, "sheet");

                sb.Insert(0, @"<p><b>前一班超出規格且沒維護更換，請於本班次更新，並再做一次維修/調整後檢查，或是換刀後檢查。<br />欄位有問題，已通知 Cell leader / engineers 前往處理</b></p>");
                sb.Insert(0, @"<Html>
                                <head>
                                    <style type=text/css>
                                        .wrapper {border-collapse:collapse; font-family: verdana; font-size: 11px;}
                                        .DataTD_Green {text-align:left; background: #7E963D;color:white; padding: 1px 5px 1px 5px; border: 1px #C6D2DE solid; border-left: 1px #C6D2DEdotted; border-right: 1px #C6D2DE dotted; }
                                        .DataTD {text-align:left; background: #ffffff; padding: 1px 5px 1px 5px; border: 1px #C6D2DE solid; border-left: 1px #C6D2DE dotted;border-right: 1px #C6D2DE dotted; }
                                    </style>
                                </head>
                            <Body>");

                sb.Append("<table class=wrapper align=left width=70% border=1>");
                sb.Append(string.Format(@"<tr><td class=DataTD_Green>檢視連結</td><td class=DataTD>{0}ESC_Login.aspx?sheet_categoryID={1}&sheetID={2}</td></tr>", ConfigurationManager.AppSettings["InternetURL"].ToString(),
                                                                                                                                                                      dt_mail.Rows[0]["sheet_categoryID"].ToString(),
                                                                                                                                                                      dt_mail.Rows[0]["sheetID"].ToString())); 
                sb.Append(string.Format(@"<tr><td class=DataTD_Green>區域(Location)</td>     <td class=DataTD>{0}</td></tr>", txtLocation.Text.Trim()));
                sb.Append(string.Format(@"<tr><td class=DataTD_Green>機台號碼(MachineNo)</td><td class=DataTD> {0}</td></tr>", txtMachine.Text.Trim()));
                sb.Append(string.Format(@"<tr><td class=DataTD_Green>日期(Date)</td>    <td class=DataTD>{0}</td></tr>", txtDate.Text.Trim().ToUpper()));
                sb.Append(string.Format(@"<tr><td class=DataTD_Green>操作人員(Operator)</td> <td class=DataTD>{0}</td></tr>", txtOperator.Text.Trim().ToUpper()));
                sb.Append("</table>");
                sb.Append("<br /><br /><br /><br /><br /><br />");

                sb.Append("<p>******************e-StartUp Check List Mail Agent [e-StartUp.mail.agent@tw-khh01.nxp.com]*******************</p>");
                sb.Append("</Body></Html>");

                func.getMachineContactList(dt_mail.Rows[0]["sheet_categoryID"].ToString(), sb);
            }
        }
        private void DifferentView()
        {
            trBladeDeviation.Style.Clear();
            trEmulsion.Style.Clear();
            trCo2Bubbler.Style.Clear();

            string machine = txtMachine.Text.Trim().ToUpper();
            // #601 / #602 / #603 / #701 ~ #707 
            //QS-007 / QS-009 / QS-012/ QS-014 / QS-013 / QS-006 / QS-015 / QS-005 / QS-008 / QS-011
            if (machine.Contains("QS-007") || machine.Contains("QS-009") || machine.Contains("QS-012") ||
                machine.Contains("QS-014") || machine.Contains("QS-013") || machine.Contains("QS-006") ||
                machine.Contains("QS-015") || machine.Contains("QS-005") || machine.Contains("QS-008") ||
                machine.Contains("QS-011"))
            {
                trBladeDeviation.Style.Add("display", "inline");

                if (txtPackage.Text.Trim().Contains(PackageType.TFBGA.ToString()) || txtPackage.Text.Trim().Contains(PackageType.LFBGA.ToString()))
                { trEmulsion.Style.Add("display", "inline"); }
            }
            else
            {
                trBladeDeviation.Style.Add("display", "none");
                trEmulsion.Style.Add("display", "none");
            }

            //#SA610 -> #SA609(2014-06-11)
            //DS-053 -> DS-050
            if (!machine.Contains("DS-050"))
            {
                trCo2Bubbler.Style.Add("display", "none");
            }
        }

        private bool checkAll()
        {
            sb = new StringBuilder();
            bool check = true;
            Common.Comfunc func = new Common.Comfunc();
            
            string packageStr = txtPackage.Text.Trim();
            string tester = txtMachine.Text.Trim().ToUpper();

            //==================================================================================================
            //Check NULL
            //==================================================================================================
            if (check)
            {
                
                // #601 / #602 / #603 / #701 ~ #707 
                //QS-007 / QS-009 / QS-012/ QS-014 / QS-013 / QS-006 / QS-015 / QS-005 / QS-008 / QS-011
                if (tester.Contains("QS-007") || tester.Contains("QS-009") || tester.Contains("QS-012") ||
                    tester.Contains("QS-014") || tester.Contains("QS-013") || tester.Contains("QS-006") ||
                    tester.Contains("QS-015") || tester.Contains("QS-005") || tester.Contains("QS-008") ||
                    tester.Contains("QS-011"))
                {
                    if (string.IsNullOrEmpty(txtBladeDeviation.Text)) { sb.Append("【刃長差異】"); }

                    if (!txtPackage.Text.Trim().Contains(PackageType.TFBGA.ToString()) && !txtPackage.Text.Trim().Contains(PackageType.LFBGA.ToString()))
                    {
                        if (string.IsNullOrEmpty(txtEmulsion.Text)) { sb.Append("【切削液濃度】"); }
                    }
                }

                //#SA610 -> #SA609(2014-06-11)
                //DS-053 -> DS-050
                if (tester.Contains("DS-050") && packageStr.Contains(PackageType.HVSON.ToString()))
                {
                    if (string.IsNullOrEmpty(txtCo2Bubbler.Text)) { sb.Append(string.Format(@"【{0}CO2 阻抗值 (Mohm)】", PackageType.HVSON.ToString())); }
                }

                func.checkValue(Common.Comfunc.CheckType.NullOrEmpty, sheet_categoryID, sb, Page.Master);
                if (sb.Length > 0) { check = false; sb.Insert(0, "尚有欄位未填或未選擇\\n\\n"); }
            }


            //==================================================================================================
            //Check Validity
            //==================================================================================================
            if (check)
            {
                Common.ValidatorHelper val = new Common.ValidatorHelper();
                // #601 / #602 / #603 / #701 ~ #707 
                //QS-007 / QS-009 / QS-012/ QS-014 / QS-013 / QS-006 / QS-015 / QS-005 / QS-008 / QS-011
                if (tester.Contains("QS-007") || tester.Contains("QS-009") || tester.Contains("QS-012") ||
                    tester.Contains("QS-014") || tester.Contains("QS-013") || tester.Contains("QS-006") ||
                    tester.Contains("QS-015") || tester.Contains("QS-005") || tester.Contains("QS-008") ||
                    tester.Contains("QS-011"))
                {
                    if (!val.IsFloat(txtBladeDeviation.Text.Trim())) { sb.Append("【刃長差異】"); }

                    if (!txtPackage.Text.Trim().Contains(PackageType.TFBGA.ToString()) && !txtPackage.Text.Trim().Contains(PackageType.LFBGA.ToString()))
                    {
                        if (!val.IsFloat(txtEmulsion.Text.Trim())) { sb.Append("【切削液濃度】"); }
                    }
                }

                //#SA610 ->#SA609 (2014-06-11)
                //DS-053 -> DS-050
                if (tester.Contains("DS-050") && packageStr.Contains(PackageType.HVSON.ToString()))
                {
                    if (!val.IsFloat(txtCo2Bubbler.Text.Trim())) { sb.Append(string.Format(@"【{0}CO2 阻抗值 (Mohm)】", PackageType.HVSON.ToString())); }
                }

                func.checkValue(Common.Comfunc.CheckType.Validity, sheet_categoryID, sb, Page.Master);
                if (sb.Length > 0) { check = false; sb.Insert(0, "資料輸入錯誤，請填入正確格式\\n\\n"); }

            }

            //==================================================================================================
            //Check Scope & Mail
            //==================================================================================================
            if (check)
            {
                //Check Urgent status
                string location = txtLocation.Text.Trim().ToUpper();
                #region 目前刀刃長度
                //Jig tester = QS
                //"LFBGA / TFBGA : > 2.2 mm (Jig)
                //HLQFN > 2.2 jig
                //DHVQFN / HWQFN / HVQFN : > 1.8 mm (Jig)
                //Tape tester = DS
                //LFBGA / TFBGA : > 1.8 (tape)
                //HLQFN (tape) : > 2.1 mm
                //HVSON / HVQFN : > 1.35 mm (tape)
                //XQFN : > 1.0 mm (tape)"    
                if (HttpContext.Current.Session["tester"] != null && string.IsNullOrEmpty(txtMachine.Text.Trim())) { tester = HttpContext.Current.Session["tester"].ToString(); }

                if (packageStr.Contains(PackageType.LFBGA.ToString()) || packageStr.Contains(PackageType.TFBGA.ToString()))
                {
                    if (tester.ToUpper().Contains("DS") && Convert.ToDouble(txtBladeExposure1.Text.Trim()) < 1.8) { sb.Append(string.Format(@"【{0}/{1}(Tape)目前Z1刀刃長度必須 > 1.8mm，請換刀】", PackageType.LFBGA.ToString(), PackageType.TFBGA.ToString())); }
                    if (tester.ToUpper().Contains("DS") && Convert.ToDouble(txtBladeExposure2.Text.Trim()) < 1.8) { sb.Append(string.Format(@"【{0}/{1}(Tape)目前Z2刀刃長度必須 > 1.8mm，請換刀】", PackageType.LFBGA.ToString(), PackageType.TFBGA.ToString())); }
                    if (tester.ToUpper().Contains("QS") && Convert.ToDouble(txtBladeExposure1.Text.Trim()) < 2.2) { sb.Append(string.Format(@"【{0}/{1}(Jig)目前Z1刀刃長度必須 > 2.2mm，請換刀】", PackageType.LFBGA.ToString(), PackageType.TFBGA.ToString())); }
                    if (tester.ToUpper().Contains("QS") && Convert.ToDouble(txtBladeExposure2.Text.Trim()) < 2.2) { sb.Append(string.Format(@"【{0}/{1}(Jig)目前Z2刀刃長度必須 > 2.2mm，請換刀】", PackageType.LFBGA.ToString(), PackageType.TFBGA.ToString())); }
                }
                else if (packageStr.Contains(PackageType.HLQFN.ToString()))
                {
                    if (tester.ToUpper().Contains("DS") && Convert.ToDouble(txtBladeExposure1.Text.Trim()) < 2.1) { sb.Append(string.Format(@"【{0}(Tape)目前Z1刀刃長度必須 > 2.1mm，請換刀】", PackageType.HLQFN.ToString())); }
                    if (tester.ToUpper().Contains("DS") && Convert.ToDouble(txtBladeExposure2.Text.Trim()) < 2.1) { sb.Append(string.Format(@"【{0}(Tape)目前Z2刀刃長度必須 > 2.1mm，請換刀】", PackageType.HLQFN.ToString())); }
                    if (tester.ToUpper().Contains("QS") && Convert.ToDouble(txtBladeExposure1.Text.Trim()) < 2.2) { sb.Append(string.Format(@"【{0}(Jig)目前Z1刀刃長度必須 > 2.2mm，請換刀】", PackageType.HLQFN.ToString())); }
                    if (tester.ToUpper().Contains("QS") && Convert.ToDouble(txtBladeExposure2.Text.Trim()) < 2.2) { sb.Append(string.Format(@"【{0}(Jig)目前Z2刀刃長度必須 > 2.2mm，請換刀】", PackageType.HLQFN.ToString())); }
                }
                else if (packageStr.Contains(PackageType.DHVQFN.ToString()) || packageStr.Contains(PackageType.HWQFN.ToString()) || packageStr.Contains(PackageType.HVQFN.ToString()))
                {
                    if (tester.ToUpper().Contains("QS") && Convert.ToDouble(txtBladeExposure1.Text.Trim()) < 1.8) { sb.Append(string.Format(@"【{0}/{1}/{2}(Jig)目前Z1刀刃長度必須 > 1.8mm，請換刀】", PackageType.DHVQFN.ToString(), PackageType.HWQFN.ToString(), PackageType.HVQFN.ToString())); }
                    if (tester.ToUpper().Contains("QS") && Convert.ToDouble(txtBladeExposure2.Text.Trim()) < 1.8) { sb.Append(string.Format(@"【{0}/{1}/{2}(Jig)目前Z2刀刃長度必須 > 1.8mm，請換刀】", PackageType.DHVQFN.ToString(), PackageType.HWQFN.ToString(), PackageType.HVQFN.ToString())); }
                }
                else if (packageStr.Contains(PackageType.HVSON.ToString()) || packageStr.Contains(PackageType.HVQFN.ToString()))
                {
                    if (tester.ToUpper().Contains("DS") && Convert.ToDouble(txtBladeExposure1.Text.Trim()) < 1.35) { sb.Append(string.Format(@"【{0}/{1}(Tape)目前Z1刀刃長度必須 > 1.35mm，請換刀】", PackageType.HVSON.ToString(), PackageType.HVQFN.ToString())); }
                    if (tester.ToUpper().Contains("DS") && Convert.ToDouble(txtBladeExposure2.Text.Trim()) < 1.35) { sb.Append(string.Format(@"【{0}/{1}(Tape)目前Z2刀刃長度必須 > 1.35mm，請換刀】", PackageType.HVSON.ToString(), PackageType.HVQFN.ToString())); }
                }
                else if (packageStr.Contains(PackageType.XQFN.ToString()))
                {
                    if (tester.ToUpper().Contains("DS") && Convert.ToDouble(txtBladeExposure1.Text.Trim()) < 1.0) { sb.Append(string.Format(@"【{0}(Tape)目前Z1刀刃長度必須 > 1.0mm，請換刀】", PackageType.XQFN.ToString())); }
                    if (tester.ToUpper().Contains("DS") && Convert.ToDouble(txtBladeExposure2.Text.Trim()) < 1.0) { sb.Append(string.Format(@"【{0}(Tape)目前Z2刀刃長度必須 > 1.0mm，請換刀】", PackageType.XQFN.ToString())); }
                }
                #endregion
                #region 進刀速度
                //- substrate: TFBGA / LFBGA / HLQFN
                //  BT-substrate < 100mm/s
                //- leadframe: HVQFN / HWQFN / HXQFN / XQFN / DHVQFN
                //  Leadframe 10-50 mm/s
                if (packageStr.Contains(PackageType.TFBGA.ToString()) || packageStr.Contains(PackageType.LFBGA.ToString()) || packageStr.Contains(PackageType.HLQFN.ToString()))
                {
                    if (Convert.ToDouble(txtFeedSpeed.Text.Trim()) > 100) { sb.Append(string.Format(@"【{0}/{1}/{2}進刀速度BT-substrate應為 < 100mm/s，不可切】", PackageType.TFBGA.ToString(), PackageType.LFBGA.ToString(), PackageType.HLQFN.ToString())); }
                }
                else if (packageStr.Contains(PackageType.HVQFN.ToString()) || packageStr.Contains(PackageType.HWQFN.ToString()) || packageStr.Contains(PackageType.HXQFN.ToString())
                         || packageStr.Contains(PackageType.XQFN.ToString()) || packageStr.Contains(PackageType.DHVQFN.ToString()))
                {
                    if (Convert.ToDouble(txtFeedSpeed.Text.Trim()) < 10 || Convert.ToDouble(txtFeedSpeed.Text.Trim()) > 50) { sb.Append(string.Format(@"【{0}/{1}/{2}/{3}/{4}進刀速度Leadframe應為 10-50 mm/s，不可切】", PackageType.HVQFN.ToString(), PackageType.HWQFN.ToString(), PackageType.HXQFN.ToString(), PackageType.XQFN.ToString(), PackageType.DHVQFN.ToString())); }
                }
                #endregion
                #region CO2 阻抗值 0.2 ~ 0.6 Mohm(HVSON12 只能在 TS05 (有 CO2 bubbler 的機台) 上切)
                //#SA610
                if (location.Contains("610") && packageStr.Contains(PackageType.HVSON.ToString()))
                {
                    if (Convert.ToDouble(txtCo2Bubbler.Text.Trim()) < 0.2 || Convert.ToDouble(txtCo2Bubbler.Text.Trim()) > 0.6) { sb.Append(string.Format(@"【{0}CO2 阻抗值 (Mohm)必須為0.2 ~ 0.6 Mohm，請報修】", PackageType.HVSON12.ToString())); }
                }
                #endregion

                // #601 / #602 / #603 / #701 ~ #707 
                //QS-007 / QS-009 / QS-012/ QS-014 / QS-013 / QS-006 / QS-015 / QS-005 / QS-008 / QS-011
                if (tester.Contains("QS-007") || tester.Contains("QS-009") || tester.Contains("QS-012") ||
                    tester.Contains("QS-014") || tester.Contains("QS-013") || tester.Contains("QS-006") ||
                    tester.Contains("QS-015") || tester.Contains("QS-005") || tester.Contains("QS-008") ||
                    tester.Contains("QS-011"))
                {
                    if (Convert.ToDouble(txtBladeDeviation.Text) > 0.2) { sb.Append("【刃長差異必須 < 0.2mm，請換刀】"); }

                    if (!txtPackage.Text.Trim().Contains(PackageType.TFBGA.ToString()) && !txtPackage.Text.Trim().Contains(PackageType.LFBGA.ToString()))
                    {
                        if (Convert.ToDouble(txtEmulsion.Text) < 2.2) { sb.Append("【切削液濃度太低，必須立即加切削液】"); }
                        if (Convert.ToDouble(txtEmulsion.Text) > 4.0) { sb.Append("【切削液濃度太高，仍可繼續作業，但浪費切削液】"); }
                    }
                }

                func.checkValue(Common.Comfunc.CheckType.Correct, sheet_categoryID, sb, Page.Master);
                if (sb.Length > 0) { sb.Insert(0, "以下狀況請立即處理，並再做一次維修/調整後檢查\\n\\n"); }
                return true;

            }

            return (string.IsNullOrEmpty(sb.ToString()) ? true : false);
        }
        private void jsShow(string str)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "js", str, true);
        }
        
               
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ContentPlaceHolder PlaceHolder2 = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder2");
            Common.Comfunc func = new Common.Comfunc();
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);

            //check
            func.checkLogin();            

            if (!checkAll()) { jsShow(string.Format(@"alert('{0}');", sb.ToString())); }
            else
            {
                //==================================================================================================
                //add transaction sheet
                //==================================================================================================
                List<string> arrStr = new List<string>();
                string insert_Sheet = string.Format(@"Insert into sheet(sheetID,sheet_categoryID,tester,location,TimeShift,op,UsersID)
                                                      Values(sheet_sequence.nextval,'{0}','{1}','{2}','{3}','{4}','{5}')", sheet_categoryID,
                                                                                                                           txtMachine.Text,
                                                                                                                           txtLocation.Text.Trim(),
                                                                                                                           txtTimeShift.Text.Trim(),
                                                                                                                           txtOperator.Text.Trim(),
                                                                                                                           Session["UsersID"].ToString());
                arrStr.Add(insert_Sheet);


                //==================================================================================================
                //add transaction sheet_value
                //==================================================================================================
                string item_str = string.Format(@"Select itemID,aspxControlID,datatypeID From Item Where sheet_categoryID='{0}'", sheet_categoryID);
                DataTable item_dt = ado.loadDataTable(item_str, null, "Item");

                foreach (DataRow dr in item_dt.Rows)
                {
                    string itemValue = "";

                    switch (dr["datatypeID"].ToString())
                    {
                        case "1": //TxtBox
                            TextBox txt = (TextBox)PlaceHolder2.FindControl(dr["aspxControlID"].ToString());
                            itemValue = txt.Text.Trim();
                            break;

                        case "2": //RadioBox
                            RadioButton rdo_Y = (RadioButton)PlaceHolder2.FindControl(dr["aspxControlID"].ToString() + "_Y");
                            RadioButton rdo_N = (RadioButton)PlaceHolder2.FindControl(dr["aspxControlID"].ToString() + "_N");
                            itemValue = (rdo_Y.Checked) ? "Y" : "N";
                            break;

                        case "3": //DropDownList
                            DropDownList ddl = (DropDownList)PlaceHolder2.FindControl(dr["aspxControlID"].ToString());
                            itemValue = ddl.SelectedValue;
                            break;
                    }

                    string insert_Value = string.Format(@"Insert into sheet_Value(sheet_valueID,sheetID,ItemID,value)
                                                          Values(Value_sequence.nextval,sheet_sequence.currval,'{0}','{1}')", dr["itemID"].ToString(),
                                                                                                                                    itemValue);
                    arrStr.Add(insert_Value);
                }


                //==================================================================================================
                //add transaction log
                //==================================================================================================
                string insert_log = string.Format(@"insert into Log(LogID,sheetID,LogAction,Time)
                                                    Values(log_sequence.nextval,sheet_sequence.currval,'INSERT',SYSDATE)");
                arrStr.Add(insert_log);


                //==================================================================================================
                //execute transaction_SQL
                //==================================================================================================
                string reStr = ado.SQL_transaction(arrStr, Conn);
                if (!reStr.ToUpper().Contains("SUCCESS"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('該資料新增失敗，出現Exception');"), true);
                    return;
                }


                //==================================================================================================
                //Alert&Mail
                //================================================================================================== 
                jsShow(
                    (string.IsNullOrEmpty(sb.ToString()))
                    ? string.Format(@"alert('流程卡No.{0}　完成新增');", txtBatchCardNo.Text.Trim())
                    : string.Format(@"alert('流程卡No.{0}　完成新增，{1}');", txtBatchCardNo.Text.Trim(), sb.ToString())
                    );
                if (sb.Length > 0)
                {
                    string str_mail = string.Format(@"Select s.sheetID,s.sheet_categoryID,sc.describes 
                                                      From sheet s 
                                                      Inner join sheet_category sc on s.sheet_categoryid=sc.sheet_categoryid
                                                      Where s.sheet_categoryID='{0}'
                                                      Order By s.sheetID Desc", sheet_categoryID);
                    DataTable dt_mail = ado.loadDataTable(str_mail, null, "sheet");

                    sb.Insert(0, @"<p><b>欄位有問題，已通知 Cell leader / engineers 前往處理</b></p>");
                    sb.Insert(0, @"<Html>
                                <head>
                                    <style type=text/css>
                                        .wrapper {border-collapse:collapse; font-family: verdana; font-size: 11px;}
                                        .DataTD_Green {text-align:left; background: #7E963D;color:white; padding: 1px 5px 1px 5px; border: 1px #C6D2DE solid; border-left: 1px #C6D2DEdotted; border-right: 1px #C6D2DE dotted; }
                                        .DataTD {text-align:left; background: #ffffff; padding: 1px 5px 1px 5px; border: 1px #C6D2DE solid; border-left: 1px #C6D2DE dotted;border-right: 1px #C6D2DE dotted; }
                                    </style>
                                </head>
                            <Body>");

                    sb.Append("<table class=wrapper align=left width=70% border=1>");
                    sb.Append(string.Format(@"<tr><td class=DataTD_Green>檢視連結</td><td class=DataTD>{0}ESC_Login.aspx?sheet_categoryID={1}&sheetID={2}</td></tr>", ConfigurationManager.AppSettings["InternetURL"].ToString(),
                                                                                                                                                                      dt_mail.Rows[0]["sheet_categoryID"].ToString(),
                                                                                                                                                                      dt_mail.Rows[0]["sheetID"].ToString())); 
                    sb.Append(string.Format(@"<tr><td class=DataTD_Green>區域(Location)</td>     <td class=DataTD>{0}</td></tr>", txtLocation.Text.Trim()));
                    sb.Append(string.Format(@"<tr><td class=DataTD_Green>機台號碼(MachineNo)</td><td class=DataTD> {0}</td></tr>", txtMachine.Text.Trim()));
                    sb.Append(string.Format(@"<tr><td class=DataTD_Green>日期(Date)</td>    <td class=DataTD>{0}</td></tr>", txtDate.Text.Trim().ToUpper()));
                    sb.Append(string.Format(@"<tr><td class=DataTD_Green>操作人員(Operator)</td> <td class=DataTD>{0}</td></tr>", txtOperator.Text.Trim().ToUpper()));
                    sb.Append("</table>");
                    sb.Append("<br /><br /><br /><br /><br /><br />");

                    sb.Append("<p>******************e-StartUp Check List Mail Agent [e-StartUp.mail.agent@tw-khh01.nxp.com]*******************</p>");
                    sb.Append("</Body></Html>");

                    func.getMachineContactList(dt_mail.Rows[0]["sheet_categoryID"].ToString(), sb);
                }
                btnReset_Click(null, null);
            }//end if
        }
        protected void btnUpdateSubmit_Click(object sender, EventArgs e)
        {
            ContentPlaceHolder PlaceHolder2 = (ContentPlaceHolder)Page.Master.FindControl("ContentPlaceHolder2");
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
            Common.Comfunc func = new Common.Comfunc();

            if (!checkAll()) { jsShow(string.Format(@"alert('{0}');", sb.ToString())); }
            else
            {
                //==================================================================================================
                //Update
                //==================================================================================================
                List<string> arrStr = new List<string>();
                string item_str = string.Format(@"Select itemID,aspxControlID,datatypeID From Item Where sheet_categoryID='{0}'", sheet_categoryID);
                DataTable item_dt = ado.loadDataTable(item_str, null, "Item");

                foreach (DataRow dr in item_dt.Rows)
                {
                    string itemValue = "";

                    switch (dr["datatypeID"].ToString())
                    {
                        case "1": //TxtBox
                            TextBox txt = (TextBox)PlaceHolder2.FindControl(dr["aspxControlID"].ToString());
                            itemValue = txt.Text.Trim();
                            break;

                        case "2": //RadioBox
                            RadioButton rdo_Y = (RadioButton)PlaceHolder2.FindControl(dr["aspxControlID"].ToString() + "_Y");
                            RadioButton rdo_N = (RadioButton)PlaceHolder2.FindControl(dr["aspxControlID"].ToString() + "_N");
                            itemValue = (rdo_Y.Checked) ? "Y" : "N";
                            break;

                        case "3": //DropDownList
                            DropDownList ddl = (DropDownList)PlaceHolder2.FindControl(dr["aspxControlID"].ToString());
                            itemValue = ddl.SelectedValue;
                            break;
                    }
                    //select old item
                    string old_str = string.Format(@"Select value, sheet_valueID 
                                                     From sheet_value 
                                                     Where sheetID='{0}'
                                                           And ItemID='{1}'", sheetID, dr["itemID"].ToString()); 
                    DataTable OldValue_dt = ado.loadDataTable(old_str, null, "sheet_Value");

                    if (!itemValue.Equals(OldValue_dt.Rows[0]["value"].ToString()))
                    {
                        //==================================================================================================
                        //add transaction sheet_value
                        //==================================================================================================
                        string update_Value = string.Format(@"Update sheet_value 
                                                          set value='{0}'
                                                          Where sheet_valueID='{1}'", itemValue, OldValue_dt.Rows[0]["sheet_ValueID"].ToString());
                        arrStr.Add(update_Value);


                        //==================================================================================================
                        //add transaction Log
                        //==================================================================================================                                    
                        string update_log = string.Format(@"insert into Log(LogID,sheetID,LogAction,Time,OldValue,NewValue,sheet_ValueID)
                                                        Values(log_sequence.nextval,'{0}','UPDATE',SYSDATE,'{1}','{2}','{3}')", sheetID, OldValue_dt.Rows[0]["value"].ToString(), itemValue, OldValue_dt.Rows[0]["sheet_ValueID"].ToString());
                        arrStr.Add(update_log);
                    }
                }
                //==================================================================================================
                //add transaction sheet_remark
                //==================================================================================================
                string insert_Sheet = string.Format(@"Update sheet
                                                      Set    RemarkID='3', FixEngineer='{0}',FixTime=SYSDATE,
                                                             tester='{1}',Location='{2}',TimeShift='{3}',op='{4}'
                                                      Where  SheetID='{5}'", Session["account"].ToString(),
                                                                           txtMachine.Text.Trim(),
                                                                           txtLocation.Text.Trim(),
                                                                           txtTimeShift.Text.Trim(),
                                                                           txtOperator.Text.Trim(),
                                                                           sheetID);
                arrStr.Add(insert_Sheet);


                //==================================================================================================
                //execute transaction_SQL
                //==================================================================================================
                string reStr = ado.SQL_transaction(arrStr, Conn);
                if (!reStr.ToUpper().Contains("SUCCESS"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('該資料更新失敗，出現Exception');"), true);
                    return;
                }


                //==================================================================================================
                //Alert&Mail
                //==================================================================================================
                jsShow(
                    (string.IsNullOrEmpty(sb.ToString()))
                    ? string.Format(@"alert('流程卡No.{0}　完成修改');", txtBatchCardNo.Text.Trim())
                    : string.Format(@"alert('流程卡No.{0}　完成修改，{1}');", txtBatchCardNo.Text.Trim(), sb.ToString())
                    );
                if (sb.Length > 0)
                {
                    string str_mail = string.Format(@"Select s.sheetID,s.sheet_categoryID,sc.describes 
                                                      From sheet s 
                                                      Inner join sheet_category sc on s.sheet_categoryid=sc.sheet_categoryid
                                                      Where s.sheetID='{0}'
                                                      Order By s.sheetID Desc", sheetID);
                    DataTable dt_mail = ado.loadDataTable(str_mail, null, "sheet");

                    sb.Insert(0, @"<p><b>欄位有問題，已通知 Cell leader / engineers 前往處理</b></p>");
                    sb.Insert(0, @"<Html>
                                <head>
                                    <style type=text/css>
                                        .wrapper {border-collapse:collapse; font-family: verdana; font-size: 11px;}
                                        .DataTD_Green {text-align:left; background: #7E963D;color:white; padding: 1px 5px 1px 5px; border: 1px #C6D2DE solid; border-left: 1px #C6D2DEdotted; border-right: 1px #C6D2DE dotted; }
                                        .DataTD {text-align:left; background: #ffffff; padding: 1px 5px 1px 5px; border: 1px #C6D2DE solid; border-left: 1px #C6D2DE dotted;border-right: 1px #C6D2DE dotted; }
                                    </style>
                                </head>
                            <Body>");

                    sb.Append("<table class=wrapper align=left width=70% border=1>");
                    sb.Append(string.Format(@"<tr><td class=DataTD_Green>檢視連結</td><td class=DataTD>{0}View/Sheet/{1}.aspx?sheet_categoryID={2}&sheetID={3}</td></tr>", ConfigurationManager.AppSettings["InternetURL"].ToString(),
                                                                                                                                                                           dt_mail.Rows[0]["describes"].ToString(),
                                                                                                                                                                           dt_mail.Rows[0]["sheet_categoryID"].ToString(),
                                                                                                                                                                           dt_mail.Rows[0]["sheetID"].ToString()));
                    
                    sb.Append(string.Format(@"<tr><td class=DataTD_Green>區域(Location)</td>     <td class=DataTD>{0}</td></tr>", txtLocation.Text.Trim()));
                    sb.Append(string.Format(@"<tr><td class=DataTD_Green>機台號碼(MachineNo)</td><td class=DataTD> {0}</td></tr>", txtMachine.Text.Trim()));
                    sb.Append(string.Format(@"<tr><td class=DataTD_Green>日期(Date)</td>    <td class=DataTD>{0}</td></tr>", txtDate.Text.Trim().ToUpper()));
                    sb.Append(string.Format(@"<tr><td class=DataTD_Green>操作人員(Operator)</td> <td class=DataTD>{0}</td></tr>", txtOperator.Text.Trim().ToUpper()));
                    sb.Append("</table>");
                    sb.Append("<br /><br /><br /><br /><br /><br />");

                    sb.Append("<p>******************e-StartUp Check List Mail Agent [e-StartUp.mail.agent@tw-khh01.nxp.com]*******************</p>");
                    sb.Append("</Body></Html>");

                    func.getMachineContactList(dt_mail.Rows[0]["sheet_categoryID"].ToString(), sb);
                }
            }//end if
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Common.Comfunc func = new Common.Comfunc();
            func.resetPage(Page.Master);
            initial();
        }
        protected void btnSwitch_Click(object sender, EventArgs e)
        {
            Common.Comfunc func = new Common.Comfunc();
            func.QuickMode(Page.Master);
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void txtBatchCardNo_TextChanged(object sender, EventArgs e)
        {
            //external data(from ACS)
            Common.Comfunc func = new Common.Comfunc();
            func.getAcsDataByBatchNo(txtBatchCardNo.Text.Trim(), Page.Master);

            DifferentView();
        }
}
}