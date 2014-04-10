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
    public partial class CleanStar : System.Web.UI.Page
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
                    //Start up | After repair/adjustment check
                    if (dr["ddlItemsID"].ToString().Equals("7") || dr["ddlItemsID"].ToString().Equals("10"))
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
                func.checkValue(Common.Comfunc.CheckType.NullOrEmpty, sheet_categoryID, sb, Page.Master);
                if (sb.Length > 0) { check = false; sb.Insert(0, "尚有欄位未填或未選擇\\n\\n"); }                
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
                    ? string.Format(@"alert('完成新增');")
                    : string.Format(@"alert('完成新增，{0}');", sb.ToString())
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
                    ? string.Format(@"alert('完成修改');")
                    : string.Format(@"alert('完成修改，{0}');", sb.ToString())
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
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

}
}