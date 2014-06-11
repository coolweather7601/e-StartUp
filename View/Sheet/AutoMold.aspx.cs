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
    public partial class AutoMold : System.Web.UI.Page
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
                string str = "select * from ddlitems where category='esd'";
                ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
                DataTable dt = ado.loadDataTable(str, null, "ddlitems");
                foreach (DataRow dr in dt.Rows)
                {
                   ddlEsdCheck.Items.Add(new ListItem(dr["describes"].ToString()));
                }
                ddlEsdCheck.Items.Insert(0, new ListItem("請選擇", "0"));


                str = "select * from ddlitems where category='status'";
                dt = ado.loadDataTable(str, null, "ddlitems");
                foreach (DataRow dr in dt.Rows)
                {
                    //Start up | After clean mold | After repair/adjustment check
                    if (dr["ddlItemsID"].ToString().Equals("7") || dr["ddlItemsID"].ToString().Equals("8") || dr["ddlItemsID"].ToString().Equals("10"))
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
            DataTable dt = ado.loadDataTable(sqlStr,null,"vw_sheet_value");

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

                        //==================================================================================================
                        //after clean mold
                        //==================================================================================================
                        if (dr["aspxControlID"].ToString().Equals("ddlStatus") && dr["value"].ToString().Equals("換模後|After clean mold"))
                        {
                            lblAlert.Visible = true;
                            txtUpMold1.Visible = true;
                            txtUpMold2.Visible = true;
                            txtDownMold1.Visible = true;
                            txtDownMold2.Visible = true;
                        }
                        break;
                }
                txtMachine.Text = dr["tester"].ToString();
                txtLocation.Text = dr["location"].ToString();
                txtTimeShift.Text = dr["timeShift"].ToString();
                txtOperator.Text = dr["op"].ToString();
            }

            btnSubmit.Visible = false;
            btnUpdateSubmit.Visible = true;

            //==================================================================================================
            //P1、P2是否開模
            //==================================================================================================
            if (string.IsNullOrEmpty(txtMoldTemp1.Text))
            {
                chkP1.Checked = false;
                func.checkboxNA(chkP1, Page.Master);
            }
            else { chkP1.Checked = true; }
            if (string.IsNullOrEmpty(txtMoldTemp2.Text))
            {
                chkP2.Checked = false;
                func.checkboxNA(chkP2, Page.Master);
            }
            else { chkP2.Checked = true; }
        }
        private void initial()
        {
            //external data(from ACS)
            Common.Comfunc func = new Common.Comfunc();
            func.getAcsDataByBatchNo("", Page.Master);

            //default checkbox
            chkP2.Checked = false;
            func.checkboxNA(chkP2, Page.Master);

            //模具累積班數
            getLastMoldShiftTotal();

            //集塵袋檢查
            if ((DateTime.Now.Day.ToString().Equals("15") || DateTime.Now.Day.ToString().Equals("1")) && func.getTimeShift().Equals("晚班"))
            {
                bool isFilterBag = func.checkFilterBag(sheet_categoryID, txtMachine.Text.Trim());
                if (!isFilterBag) { jsShow("alert('本月集塵袋尚未更換，請於本班次更換');"); }
            }

            //上次檢查時間
            jsShow(string.Format(@"alert('{0}');", func.getLastTime(sheet_categoryID, txtMachine.Text.Trim(), txtLocation.Text.Trim())));
        }


        private void getLastMoldShiftTotal()
        {
            string tester = txtMachine.Text.Trim();
            if (Session["tester"] != null && string.IsNullOrEmpty(tester)) { tester = Session["tester"].ToString(); }

            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);

            //==================================================================================================
            //P1模累積班數
            //==================================================================================================
            string P1_str = string.Format(@"Select item_desc,spec,aspxcontrolid,value 
                                             From vw_sheet_value
                                             Where sheetID=(select max(sheetID) 
                                                            from vw_sheet_value 
                                                            where sheet_categoryID='{0}' and aspxControlID='txtMoldShiftTotal1' 
                                                                  And Value is not null And tester='{1}')
                                             ", sheet_categoryID,tester);
            DataTable P1_dt = ado.loadDataTable(P1_str, null, "vw_sheet_value");

            string P1_logTime = "", P1_moldShiftTotal = "";
            foreach (DataRow dr in P1_dt.Rows)
            {
                if (dr["aspxControlID"].ToString().Equals("txtDate")) { P1_logTime = dr["value"].ToString(); }
                if (dr["aspxControlID"].ToString().Equals("txtMoldShiftTotal1")) { P1_moldShiftTotal = dr["value"].ToString(); }
            }
            if(P1_dt.Rows.Count>0)
                txtMoldShiftTotal1.Text = string.Format(@"至上次{0}，模具累積班數為【{1}】", P1_logTime, P1_moldShiftTotal);
            else
                txtMoldShiftTotal1.Text = "尚未有模具累積班數";

            //==================================================================================================
            //P2模累積班數
            //==================================================================================================
            string P2_str = string.Format(@"Select item_desc,spec,aspxcontrolid,value 
                                             From vw_sheet_value
                                             Where sheetID=(select max(sheetID) 
                                                            from vw_sheet_value 
                                                            where sheet_categoryID='{0}' And aspxControlID='txtMoldShiftTotal2' 
                                                                  And value is not null And tester='{1}')
                                             ", sheet_categoryID, tester);
            DataTable P2_dt = ado.loadDataTable(P2_str, null, "vw_sheet_value");

            string P2_logTime = "", P2_moldShiftTotal = "";
            foreach (DataRow dr in P2_dt.Rows)
            {
                if (dr["aspxControlID"].ToString().Equals("txtDate")) { P2_logTime = dr["value"].ToString(); }
                if (dr["aspxControlID"].ToString().Equals("txtMoldShiftTotal2")) { P2_moldShiftTotal = dr["value"].ToString(); }
            }
            if (P2_dt.Rows.Count > 0)
                txtMoldShiftTotal2.Text = string.Format(@"至上次{0}，模具累積班數為【{1}】", P2_logTime, P2_moldShiftTotal);
            else
                txtMoldShiftTotal2.Text = "尚未有模具累積班數";            
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
                func.checkValue(Common.Comfunc.CheckType.NullOrEmpty, sheet_categoryID, sb, Page.Master);                
                #region Mold
                if (ddlStatus.SelectedValue.Equals("換模後|After clean mold"))//after clean mold
                {
                    if (chkP1.Checked)
                    {
                        if (string.IsNullOrEmpty(txtUpMold1.Text)) { sb.Append("【P1,上模模號】"); }
                        if (string.IsNullOrEmpty(txtDownMold1.Text)) { sb.Append("【P1,下模模號】"); }
                    }
                    if (chkP2.Checked)
                    {
                        if (string.IsNullOrEmpty(txtUpMold2.Text)) { sb.Append("【P2,上模模號】"); }
                        if (string.IsNullOrEmpty(txtDownMold2.Text)) { sb.Append("【P2,下模模號】"); }
                    }
                }

                if (chkP1.Checked == true && (string.IsNullOrEmpty(txtMoldShiftTotal1.Text) || txtMoldShiftTotal1.Text.Trim().Contains("模具累積班數為"))) { sb.Append("【P1模,模具累積班數】"); }
                if (chkP2.Checked == true && (string.IsNullOrEmpty(txtMoldShiftTotal2.Text) || txtMoldShiftTotal2.Text.Trim().Contains("模具累積班數為"))) { sb.Append("【P2模,模具累積班數】"); }
                #endregion
                if (sb.Length > 0) { check = false; sb.Insert(0, "尚有欄位未填或未選擇\\n\\n"); }
            }

            //==================================================================================================
            //Check Validity
            //==================================================================================================
            if (check)
            {
                func.checkValue(Common.Comfunc.CheckType.Validity, sheet_categoryID, sb, Page.Master);
                if (sb.Length > 0) { check = false; sb.Insert(0, "資料輸入錯誤，請填入正確格式\\n\\n"); }
            }

            //==================================================================================================
            //Check 該Package累積班數是否超出最大值
            //(HV)(HW)(X)QFN/HVSON: Max 3 Shifts
            //(H)(L)(T)QFP: Max 9 Shifts
            //其他(other): Max 12 Shifts
            //==================================================================================================
            if (check)
            {
                string package = func.getCgFromIntrack(txtBatchCardNo.Text.Trim());
                if (package.ToUpper().Contains("HVQFN") || package.ToUpper().Contains("HWQFN") ||
                    package.ToUpper().Contains("XQFN") || package.ToUpper().Contains("HVSON"))
                {
                    if (chkP1.Checked)
                    {
                        if (Convert.ToInt32(txtMoldShiftTotal1.Text.Trim()) > 3) { sb.Append(string.Format(@"P1模，該Packge【{0}】最大累積班數為：3，請換模。\\n\\n", package)); }
                    }
                    if (chkP2.Checked)
                    {
                        if (Convert.ToInt32(txtMoldShiftTotal2.Text.Trim()) > 3) { sb.Append(string.Format(@"P2模，該Packge【{0}】最大累積班數為：3，請換模。\\n\\n", package)); }
                    }
                }
                else if (package.ToUpper().Contains("HQFP") || package.ToUpper().Contains("LQFP") || package.ToUpper().Contains("TQFP"))
                {
                    if (chkP1.Checked)
                    {
                        if (Convert.ToInt32(txtMoldShiftTotal1.Text.Trim()) > 9) { sb.Append(string.Format(@"P1模，該Packge【{0}】最大累積班數為：9，請換模。\\n\\n", package)); }
                    }
                    if (chkP2.Checked)
                    {
                        if (Convert.ToInt32(txtMoldShiftTotal2.Text.Trim()) > 9) { sb.Append(string.Format(@"P2模，該Packge【{0}】最大累積班數為：9，請換模。\\n\\n", package)); }
                    }
                }
                else
                {
                    if (chkP1.Checked)
                    {
                        if (Convert.ToInt32(txtMoldShiftTotal1.Text.Trim()) > 12) { sb.Append(string.Format(@"P1模，該Packge【{0}】最大累積班數為：12，請換模。\\n\\n", package)); }
                    }
                    if (chkP2.Checked)
                    {
                        if (Convert.ToInt32(txtMoldShiftTotal2.Text.Trim()) > 12) { sb.Append(string.Format(@"P2模，該Packge【{0}】最大累積班數為：12，請換模。\\n\\n", package)); }
                    }
                }

                if (sb.Length > 0) { check = false; sb.Insert(0, "最大累積班數已經超出規定，請換模。\\n\\n"); }
            }

            //==================================================================================================
            //Check 集塵袋
            //==================================================================================================
            if (check)
            {
                if (((DateTime.Now.Day.ToString().Equals("15") || DateTime.Now.Day.ToString().Equals("1"))
                        && func.getTimeShift().Equals("晚班") && (func.checkFilterBag(sheet_categoryID, txtMachine.Text.Trim()) == false))
                        && rdoDustBagChange_N.Checked) { sb.Append("【集塵袋是否更換】"); }
                if (sb.Length > 0) { check = false; sb.Insert(0, "每半個月集塵袋需要清潔，請晚班人員予以更換\\n\\n"); }
            }

            //==================================================================================================
            //Check Scope & Mail
            //==================================================================================================
            if (check)
            {
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
                    string itemValue="";

                    switch (dr["datatypeID"].ToString())
                    {
                        case "1": //TxtBox
                            TextBox txt = (TextBox)PlaceHolder2.FindControl(dr["aspxControlID"].ToString());
                            itemValue = txt.Text.Trim().Contains("模具累積班數") ? null : txt.Text.Trim().Replace("N/A", null);
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
                            itemValue = txt.Text.Trim().Contains("模具累積班數") ? null : txt.Text.Trim().Replace("N/A", null);
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
        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            Common.Comfunc func = new Common.Comfunc();
            func.checkboxNA(sender, Page.Master);
        }
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStatus.SelectedValue.Equals("換模後|After clean mold"))//after clean mold
            {
                lblAlert.Visible = true;
                txtUpMold1.Visible = true;
                txtUpMold2.Visible = true;
                txtDownMold1.Visible = true;
                txtDownMold2.Visible = true;
            }
            else
            {
                lblAlert.Visible = false;
                txtUpMold1.Visible = false;
                txtUpMold2.Visible = false;
                txtDownMold1.Visible = false;
                txtDownMold2.Visible = false;
            }
        }
        protected void txtBatchCardNo_TextChanged(object sender, EventArgs e)
        {
            //external data(from ACS)
            Common.Comfunc func = new Common.Comfunc();
            func.getAcsDataByBatchNo(txtBatchCardNo.Text.Trim(), Page.Master);
        }
    }
}
