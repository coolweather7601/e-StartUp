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
using System.Data.OracleClient;
using System.Text;
using System.Collections.Generic;

namespace ESC_Web.Alan
{
    public partial class LMKsheet31062410 : System.Web.UI.Page
    {
        public static string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["ESC"].ToString();
        public Common.AdoDbConn ado;
        public static StringBuilder sb = new StringBuilder();

        public static string sheet_categoryID, sheetID;
        //if(string.isNull(sheetID)){Insert_Mode} else { Update_Mode}

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
                DataTable dt = ado.loadDataTable(str, null, "ddlitems");
                foreach (DataRow dr in dt.Rows)
                {
                    //Start up | Change pkg. | After repair/adjustment check
                    if (dr["ddlItemsID"].ToString().Equals("7") || dr["ddlItemsID"].ToString().Equals("9") || dr["ddlItemsID"].ToString().Equals("10"))
                    {
                        ddlStatus.Items.Add(new ListItem(dr["describes"].ToString()));
                    }
                }
                ddlStatus.Items.Insert(0, new ListItem("請選擇", "0"));

                str = "select * from ddlitems where category='esd'";
                dt = ado.loadDataTable(str, null, "ddlitems");
                foreach (DataRow dr in dt.Rows)
                {
                    ddlEsdCheck.Items.Add(new ListItem(dr["describes"].ToString()));
                }
                ddlEsdCheck.Items.Insert(0, new ListItem("請選擇", "0"));

                string[] types = new string[] { "AMCC/BGA散熱片", "AMCC", "E&R/BGA散熱片", "E&R", "Alltec" };
                foreach (string ty in types)
                {
                    ddlMachineType.Items.Add(ty);
                }
                ddlMachineType.Items.Insert(0, new ListItem("請選擇", "0"));

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
            ddlMachineType_SelectedIndexChanged(null, null);
        }
        private void initial()
        {
            //external data(from ACS)            
            Common.Comfunc func = new Common.Comfunc();
            func.getAcsDataByBatchNo("", Page.Master);

            jsShow(string.Format(@"alert('{0}');", func.getLastTime(sheet_categoryID, txtMachine.Text.Trim(), txtLocation.Text.Trim())));
        }


        private bool checkAll()
        {
            sb = new StringBuilder();
            bool check = true;
            Common.Comfunc func = new Common.Comfunc();
            
            //==================================================================================================
            //Check NULL
            //==================================================================================================
            if (check)
            {
                switch (ddlMachineType.SelectedValue)
                {
                    case "AMCC/BGA散熱片":
                        if (string.IsNullOrEmpty(txtLaserPowerBGA.Text)) { sb.Append("【雷射能量(AMCC/E&R, BGA散熱片)】"); }
                        if (string.IsNullOrEmpty(txtCoolWaterAmcc.Text)) { sb.Append("【冷卻水溫度AMCC】"); }
                        if (string.IsNullOrEmpty(txtAmccOperation.Text)) { sb.Append("【燈管/ 二極體使用時數AMCC】"); }
                        if (string.IsNullOrEmpty(txtFilterCounter.Text)) { sb.Append("【濾網次數( AMCC )】"); }
                        break;
                    case "AMCC":
                        if (string.IsNullOrEmpty(txtLaserPower.Text)) { sb.Append("【雷射能量(AMCC/E&R)】"); }
                        if (string.IsNullOrEmpty(txtCoolWaterAmcc.Text)) { sb.Append("【冷卻水溫度AMCC】"); }
                        if (string.IsNullOrEmpty(txtAmccOperation.Text)) { sb.Append("【燈管/ 二極體使用時數AMCC】"); }
                        if (string.IsNullOrEmpty(txtFilterCounter.Text)) { sb.Append("【濾網次數( AMCC )】"); }
                        break;
                    case "E&R/BGA散熱片":
                        if (string.IsNullOrEmpty(txtLaserPowerBGA.Text)) { sb.Append("【雷射能量(AMCC/E&R, BGA散熱片)】"); }
                        if (string.IsNullOrEmpty(txtCoolWaterEr.Text)) { sb.Append("【冷卻水溫度E&R】"); }
                        if (string.IsNullOrEmpty(txtErOperation.Text)) { sb.Append("【燈管/ 二極體使用時數E&R】"); }
                        break;
                    case "E&R":
                        if (string.IsNullOrEmpty(txtLaserPower.Text)) { sb.Append("【雷射能量(AMCC/E&R)】"); }
                        if (string.IsNullOrEmpty(txtCoolWaterEr.Text)) { sb.Append("【冷卻水溫度E&R】"); }
                        if (string.IsNullOrEmpty(txtErOperation.Text)) { sb.Append("【燈管/ 二極體使用時數E&R】"); }
                        break;
                    case "Alltec":
                        if (string.IsNullOrEmpty(txtLaserPowerAlltec.Text)) { sb.Append("【雷射能量(Alltec)】"); }
                        if (string.IsNullOrEmpty(txtFilterCounter.Text)) { sb.Append("【濾網次數( AMCC )】"); }
                        break;
                    case "0":
                        sb.Append("【機台選擇】");
                        break;
                        
                }
                func.checkValue(Common.Comfunc.CheckType.NullOrEmpty, sheet_categoryID, sb, Page.Master);
                if (sb.Length > 0) { check = false; sb.Insert(0, "尚有欄位未填或未選擇\\n\\n"); }                
            }


            //==================================================================================================
            //Check Validity
            //==================================================================================================
            if (check)
            {        
                func.checkValue(Common.Comfunc.CheckType.Validity, sheet_categoryID, sb, Page.Master);
                Common.ValidatorHelper val = new Common.ValidatorHelper();
                switch (ddlMachineType.SelectedValue)
                {
                    case "AMCC/BGA散熱片":
                        if (!val.IsFloat(txtLaserPowerBGA.Text.Trim())) { sb.Append("【雷射能量(AMCC/E&R, BGA散熱片)】"); }
                        if (!val.IsFloat(txtCoolWaterAmcc.Text.Trim())) { sb.Append("【冷卻水溫度AMCC】"); }
                        if (!val.IsFloat(txtAmccOperation.Text.Trim())) { sb.Append("【燈管/ 二極體使用時數AMCC】"); }
                        if (!val.IsFloat(txtFilterCounter.Text.Trim())) { sb.Append("【濾網次數( AMCC )】"); }
                        break;
                    case "AMCC":
                        if (!val.IsFloat(txtLaserPower.Text.Trim())) { sb.Append("【雷射能量(AMCC/E&R)】"); }
                        if (!val.IsFloat(txtCoolWaterAmcc.Text.Trim())) { sb.Append("【冷卻水溫度AMCC】"); }
                        if (!val.IsFloat(txtAmccOperation.Text.Trim())) { sb.Append("【燈管/ 二極體使用時數AMCC】"); }
                        if (!val.IsFloat(txtFilterCounter.Text.Trim())) { sb.Append("【濾網次數( AMCC )】"); }
                        break;
                    case "E&R/BGA散熱片":
                        if (!val.IsFloat(txtLaserPowerBGA.Text.Trim())) { sb.Append("【雷射能量(AMCC/E&R, BGA散熱片)】"); }
                        if (!val.IsFloat(txtCoolWaterEr.Text.Trim())) { sb.Append("【冷卻水溫度E&R】"); }
                        if (!val.IsFloat(txtErOperation.Text.Trim())) { sb.Append("【燈管/ 二極體使用時數E&R】"); }
                        break;
                    case "E&R":
                        if (!val.IsFloat(txtLaserPower.Text.Trim())) { sb.Append("【雷射能量(AMCC/E&R)】"); }
                        if (!val.IsFloat(txtCoolWaterEr.Text.Trim())) { sb.Append("【冷卻水溫度E&R】"); }
                        if (!val.IsFloat(txtErOperation.Text.Trim())) { sb.Append("【燈管/ 二極體使用時數E&R】"); }
                        break;
                    case "Alltec":
                        if (!val.IsFloat(txtLaserPowerAlltec.Text.Trim())) { sb.Append("【雷射能量(Alltec)】"); }
                        if (!val.IsFloat(txtFilterCounter.Text.Trim())) { sb.Append("【濾網次數( AMCC )】"); }
                        break;
                }                
                if (sb.Length > 0) { check = false; sb.Insert(0, "資料輸入錯誤，請填入正確格式\\n\\n"); }                
            }
            

            //==================================================================================================
            //Check Scope & Mail
            //==================================================================================================
            if (check)
            {      
                func.checkValue(Common.Comfunc.CheckType.Correct, sheet_categoryID, sb, Page.Master);
                switch (ddlMachineType.SelectedValue)
                {
                    case "AMCC/BGA散熱片":
                        if (Convert.ToDouble(txtLaserPowerBGA.Text) < 10 || Convert.ToDouble(txtLaserPowerBGA.Text) > 25) { sb.Append("【雷射能量(AMCC/E&R, BGA散熱片),10~25Watt】"); }
                        if (Convert.ToDouble(txtCoolWaterAmcc.Text) < 27 || Convert.ToDouble(txtCoolWaterAmcc.Text) > 33) { sb.Append("【冷卻水溫度AMCC,27~33 ℃】"); }
                        if (Convert.ToDouble(txtAmccOperation.Text) > 2700) { sb.Append("【燈管/ 二極體使用時數AMCC,<2700】"); }
                        if (Convert.ToDouble(txtFilterCounter.Text) > 70000) { sb.Append("【濾網次數( AMCC ),<70000】"); }
                        break;
                    case "AMCC":
                        if (Convert.ToDouble(txtLaserPower.Text) < 7 || Convert.ToDouble(txtLaserPower.Text) > 14) { sb.Append("【雷射能量(AMCC/E&R),7~14Watt】"); }
                        if (Convert.ToDouble(txtCoolWaterAmcc.Text) < 27 || Convert.ToDouble(txtCoolWaterAmcc.Text) > 33) { sb.Append("【冷卻水溫度AMCC,27~33 ℃】"); }
                        if (Convert.ToDouble(txtAmccOperation.Text) > 2700) { sb.Append("【燈管/ 二極體使用時數AMCC,<2700】"); }
                        if (Convert.ToDouble(txtFilterCounter.Text) > 70000) { sb.Append("【濾網次數( AMCC ),<70000】"); }
                        break;
                    case "E&R/BGA散熱片":
                        if (Convert.ToDouble(txtLaserPowerBGA.Text) < 10 || Convert.ToDouble(txtLaserPowerBGA.Text) > 25) { sb.Append("【雷射能量(AMCC/E&R, BGA散熱片),10~25Watt】"); }
                        if (Convert.ToDouble(txtCoolWaterEr.Text) < 24 || Convert.ToDouble(txtCoolWaterEr.Text) > 29) { sb.Append("【冷卻水溫度E&R,24~29 ℃】"); }
                        if (Convert.ToDouble(txtErOperation.Text) > 24000) { sb.Append("【燈管/ 二極體使用時數E&R,<24000】"); }
                        break;
                    case "E&R":
                        if (Convert.ToDouble(txtLaserPower.Text) < 7 || Convert.ToDouble(txtLaserPower.Text) > 14) { sb.Append("【雷射能量(AMCC/E&R),7~14Watt】"); }
                        if (Convert.ToDouble(txtCoolWaterEr.Text) < 24 || Convert.ToDouble(txtCoolWaterEr.Text) > 29) { sb.Append("【冷卻水溫度E&R,24~29 ℃】"); }
                        if (Convert.ToDouble(txtErOperation.Text) > 24000) { sb.Append("【燈管/ 二極體使用時數E&R,<24000】"); }
                        break;
                    case "Alltec":
                        if (Convert.ToDouble(txtLaserPowerAlltec.Text) > 32) { sb.Append("【雷射能量(Alltec),<32A】"); }
                        if (Convert.ToDouble(txtFilterCounter.Text) > 70000) { sb.Append("【濾網次數( AMCC ),<70000】"); }
                        break;
                }  
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
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
            Common.Comfunc func = new Common.Comfunc();

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
            Common.Comfunc func = new Common.Comfunc();
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);

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
            }//end if
        }
        protected void btnSwitch_Click(object sender, EventArgs e)
        {
            Common.Comfunc func = new Common.Comfunc();
            func.QuickMode(Page.Master);
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Common.Comfunc func = new Common.Comfunc();
            func.resetPage(Page.Master);
            initial();
        }
        protected void txtBatchCardNo_TextChanged(object sender, EventArgs e)
        {
            //external data(from ACS)
            Common.Comfunc func = new Common.Comfunc();
            func.getAcsDataByBatchNo(txtBatchCardNo.Text.Trim(), Page.Master);
        }
        protected void ddlMachineType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(ddlMachineType.Text)
            {
                case "AMCC/BGA散熱片":
                    txtLaserPowerAlltec.Text=null; txtLaserPowerAlltec.Enabled=false;
                    txtLaserPower.Text = null; txtLaserPower.Enabled=false;
                    txtCoolWaterEr.Text = null; txtCoolWaterEr.Enabled=false;
                    txtErOperation.Text = null; txtErOperation .Enabled =false;
                    txtLaserPowerBGA.Enabled = true;
                    txtAmccOperation.Enabled =true;
                    txtCoolWaterAmcc.Enabled =true; 
                    txtFilterCounter.Enabled =true; 
                    break;
                case "AMCC":
                    txtLaserPowerAlltec.Text=null; txtLaserPowerAlltec.Enabled=false;
                    txtLaserPowerBGA.Text = null; txtLaserPowerBGA.Enabled=false;
                    txtCoolWaterEr.Text = null; txtCoolWaterEr.Enabled=false;
                    txtErOperation.Text = null; txtErOperation .Enabled =false;
                    txtLaserPower .Enabled = true;
                    txtAmccOperation.Enabled =true;
                    txtCoolWaterAmcc.Enabled =true; 
                    txtFilterCounter.Enabled =true; 
                    break;
                case "E&R/BGA散熱片":
                    txtLaserPowerAlltec.Text=null; txtLaserPowerAlltec.Enabled=false;                                                       
                    txtLaserPower.Text=null; txtLaserPower .Enabled = false;
                    txtAmccOperation.Text = null; txtAmccOperation.Enabled =false;
                    txtCoolWaterAmcc.Text = null; txtCoolWaterAmcc.Enabled =false; 
                    txtLaserPowerBGA.Enabled=true;
                    txtErOperation.Enabled =true;
                    txtCoolWaterEr.Enabled=true;     
                    txtFilterCounter.Enabled =false; 
                    break;
                case "E&R": 
                    txtLaserPowerAlltec.Text=null; txtLaserPowerAlltec.Enabled=false;                    
                    txtCoolWaterAmcc.Text = null; txtCoolWaterAmcc.Enabled=false;   
                    txtAmccOperation.Text = null; txtAmccOperation.Enabled =false;
                    txtLaserPowerBGA.Text = null; txtLaserPowerBGA.Enabled=false;
                    txtLaserPower.Enabled = true;
                    txtErOperation .Enabled =true;
                    txtCoolWaterEr.Enabled=true;  
                    txtFilterCounter.Enabled =false; 
                    break;
                case "Alltec":                                         
                    txtCoolWaterEr.Text = null; txtCoolWaterEr.Enabled=false;   
                    txtCoolWaterAmcc.Text = null; txtCoolWaterAmcc.Enabled=false;   
                    txtAmccOperation.Text = null; txtAmccOperation.Enabled =false;
                    txtLaserPowerBGA.Text = null; txtLaserPowerBGA.Enabled=false;
                    txtLaserPower.Text=null; txtLaserPower .Enabled = false;
                    txtErOperation.Text = null; txtErOperation .Enabled =false;
                    txtLaserPowerAlltec.Enabled=true;
                    txtFilterCounter.Enabled =true; 
                    break;
            }
        }
}
}