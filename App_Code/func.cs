using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Data.OracleClient;
using System.Text;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Net.Mail;

namespace ESC_Web.Alan.Common
{
    public class Comfunc
    {
        public static string EscConn = System.Configuration.ConfigurationManager.ConnectionStrings["ESC"].ToString();//ESC
        public static string ConnOpe2 = System.Configuration.ConfigurationManager.ConnectionStrings["dbconnStr_2"].ToString();//ACS_ope2
        public static string ConnOpe1 = System.Configuration.ConfigurationManager.ConnectionStrings["dbconnStr_1"].ToString();//ACS_ope1
        public static string IsaConn = System.Configuration.ConfigurationManager.ConnectionStrings["ACS_ISA"].ToString();//ACS_ISA
        public static string HrConn = System.Configuration.ConfigurationManager.ConnectionStrings["HR"].ToString();//Hr        

        public enum Role : int { Administrator = 0, Supervisor = 1, User = 2 }
        public enum CheckType : int { NullOrEmpty = 0, Validity = 1, Correct = 2 }

        public void mail(string contact, string mail_data)
        {
            string title = "e-StartUp Check List Fail";
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["MAIL"].ConnectionString))
            {
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                OracleTransaction transaction;
                transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Transaction = transaction;
                try
                {  
                    command.CommandText = "select seq_mail_id.nextval from dual";
                    int mail_id = Convert.ToInt32(command.ExecuteScalar());
                    command.CommandText = "insert into mail_pool (id,from_name,disable,datetime_in,send_period,mail_to,mail_cc,mail_subject,datetime_exp,exclusive_flag,check_sum,html_body) " +
                    "values (" + mail_id + ",'e-Startup Check Mail Agent',0,sysdate,0,'" + contact + "',null,'" + title + "',sysdate+1,0,null,1)";
                    command.ExecuteNonQuery();

                    command.CommandText = "insert into mail_body (id,sn,mail_cont) values (" + mail_id + ",1,'" + mail_data + "')";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    //msg.Text = ex.ToString();
                }
            }
        }
        public void resetPage(MasterPage Master)
        {
            ContentPlaceHolder PlaceHolder2 = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder2");
            UpdatePanel panel = (UpdatePanel)PlaceHolder2.FindControl("up");
            Object ctrs = getAssignObj("Control", ((UpdatePanel)panel).Controls);

            if (ctrs.GetType().Name.ToString().Equals("Control"))
            {
                foreach (object ctr in ((Control)ctrs).Controls)
                {
                    switch (ctr.GetType().Name.ToString())
                    {
                        case "TextBox":
                            ((TextBox)ctr).Text = "";
                            break;
                        case "RadioButton":
                            ((RadioButton)ctr).Checked = false;
                            break;
                        case "DropDownList":
                            if (((DropDownList)ctr).Items.Count > 0) { ((DropDownList)ctr).SelectedIndex = 0; }
                            break;
                    }
                }

            }
        }
        public void QuickMode(MasterPage Master)
        {
            ContentPlaceHolder PlaceHolder2 = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder2");
            UpdatePanel panel = (UpdatePanel)PlaceHolder2.FindControl("up");
            Object ctrs = getAssignObj("Control", ((UpdatePanel)panel).Controls);

            if (ctrs.GetType().Name.ToString().Equals("Control"))
            {
                foreach (object ctrl in ((Control)ctrs).Controls)
                {
                    switch (ctrl.GetType().Name.ToString())
                    {
                        case "RadioButton":
                            if (((RadioButton)ctrl).ID.Contains("_Y") && !((RadioButton)ctrl).ID.Contains("rdoDustBagChange"))
                            {
                                ((RadioButton)ctrl).Checked = true;
                            }
                            break;
                    }
                }

            }
        }

        public object getAssignObj(string typeName, ControlCollection ctrs)
        {
            foreach (object r in ctrs)
            {
                if (r.GetType().Name.ToString().Equals(typeName))
                    return r;
            }
            object null_obj = new object();
            return null_obj;
        }
        public void getMachineContactList(string sheet_CategoryID, StringBuilder sb)
        {
            Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, EscConn);
            DataTable dt = new DataTable();
            string sql = "SELECT mail FROM vw_mailContact WHERE sheet_CategoryID=:sheet_CategoryID";
            object[] para = new object[] { sheet_CategoryID };
            dt = ado.loadDataTable(sql, para, "vw_mailContact");

            string lst = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0) { lst += ","; }
                lst += string.Format(@"{0}@nxp.com", dt.Rows[i]["mail"].ToString());
            }
            mail(lst, sb.ToString().Replace("\\n\\n", "<br/>"));
            //string[] aryMail = lst.Split(';');
            //foreach (string email in aryMail)
            //{
            //    mail(email, sb.ToString().Replace("\\n\\n", "<br/>"));
            //}
        }
        public DataTable getDeptAllData(string deptName)
        {
            Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, EscConn);
            string empStr = string.Format(@"SELECT * FROM Users
                                         WHERE dept='{0}'
                                         AND mail IS NOT NULL
                                         ORDER BY name", deptName);
            DataTable dtEmp = new DataTable();
            dtEmp = ado.loadDataTable(empStr, null, "Users");
            return dtEmp;
        }
        public DataTable getPersonalData(string account)
        {
            Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, EscConn);
            string str = string.Format(@"SELECT UsersID,account,EMP_NO,Name,Mail,Dept
                                         FROM Users
                                         WHERE account='{0}'", account);
            DataTable dt = new DataTable();
            dt = ado.loadDataTable(str, null, "Users");

            return dt;
        }
        public string getLastTime(string sheet_categoryID, string tester, string location)
        {
            Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, EscConn);
            string sqlstr = string.Format(@"Select Time, Value,aspxControlID
                                            From vw_sheet_value vw
                                            Inner join log l on l.sheetid=vw.sheetid and l.logaction='INSERT'
                                            Where vw.sheet_categoryid='{0}' and vw.tester='{1}' and vw.aspxcontrolid='ddlStatus'
                                            Order by time desc
                                            ", sheet_categoryID, tester);

            DataTable dt = ado.loadDataTable(sqlstr, null, "vw_sheet_value");

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["aspxControlID"].ToString().Equals("ddlStatus"))
                {
                    TimeSpan difference = DateTime.Now - DateTime.Parse(dr["Time"].ToString());
                    if (difference.TotalHours > 24 && sheet_categoryID.Equals("1"))// AutoMold連續三班沒有做e-check list , 第四班後有小姐pop alert
                    {
                        return string.Format(@"機台號碼：{0} \n機台位置：{1} \n上次檢查時間：{2} \n檢查時機：{3} \n\n已連續三班未開機, 請登修清模！！！", tester,
                                                                                                                                                    location,
                                                                                                                                                    Convert.ToDateTime(dr["Time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                                                                                                                                    dr["value"].ToString());
                    }

                    return string.Format(@"機台號碼：{0} \n機台位置：{1} \n上次檢查時間：{2} \n檢查時機：{3}", tester,
                                                                                                               location,
                                                                                                               Convert.ToDateTime(dr["Time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                                                                                               dr["value"].ToString());
                }
            }
            return string.Format(@"機台號碼：{0} \n機台位置：{1} \n無檢查紀錄", tester, location);
        }

        //==================================================================================================
        //Common check
        //==================================================================================================        
        public void checkLogin()
        {
            if (HttpContext.Current.Session["account"] == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["isDemo"].ToString().Equals("Y"))
                {
                    HttpContext.Current.Session["account"] = "admin";
                    HttpContext.Current.Session["RoleID"] = "0";
                    HttpContext.Current.Session["Name"] = "系統管理者";
                    HttpContext.Current.Session["Dept_Name"] = "ABE";
                    HttpContext.Current.Session["UsersID"] = "358";
                }
                else
                {
                    HttpContext.Current.Response.Redirect("../../ESC_Login.aspx");
                }
            }
        }
        public void checkRole(MasterPage Master)
        {
            ContentPlaceHolder PlaceHolder1 = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            foreach (object obj in PlaceHolder1.Controls)
            {
                switch (obj.GetType().Name.ToString())
                {
                    #region MasterPage_Button
                    case "Button":
                        if (((Button)obj).ID.Equals("btnRole") && HttpContext.Current.Session["RoleID"].Equals("0"))
                        { ((Button)obj).Visible = true; }
                        break;
                    #endregion
                }
            }


            ContentPlaceHolder PlaceHolder2 = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder2");
            UpdatePanel panel = (UpdatePanel)PlaceHolder2.FindControl("up");
            Object ctrs = getAssignObj("Control", ((UpdatePanel)panel).Controls);

            if (ctrs.GetType().Name.ToString().Equals("Control"))
            {
                foreach (object ctr in ((Control)ctrs).Controls)
                {
                    switch (ctr.GetType().Name.ToString())
                    {
                        case "GridView":
                            string gvID = ((GridView)ctr).ID;

                            //==================================================================================
                            //ACS_Manage
                            //==================================================================================
                            if (gvID.Equals("GridViewAM") && ((Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Administrator) || (Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Supervisor)))
                            {
                                ((GridView)ctr).Columns[4].Visible = true;
                                ((GridView)ctr).Columns[5].Visible = true;
                            }


                            //==================================================================================
                            //ListIndex_Gridview
                            //==================================================================================
                            if (gvID.Equals("GridView1"))
                            {
                                if ((Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Administrator) || ((Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Supervisor)))
                                {
                                    ((GridView)ctr).Columns[9].Visible = true;
                                    ((GridView)ctr).Columns[10].Visible = true;
                                    ((GridView)ctr).Columns[11].Visible = true;
                                    ((GridView)ctr).Columns[12].Visible = true;
                                }
                            }
                            if (gvID.Equals("GridView2"))
                            {
                                if ((Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Administrator) || ((Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Supervisor)))
                                {
                                    ((GridView)ctr).Columns[12].Visible = true;
                                    ((GridView)ctr).Columns[13].Visible = true;
                                    ((GridView)ctr).Columns[14].Visible = true;
                                    ((GridView)ctr).Columns[15].Visible = true;
                                }
                            }
                            break;
                        #region form_button
                        case "Button":
                            if (((Button)ctr).ID.Equals("btnNew") &&
                                (Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Administrator) || Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.Supervisor)
                            {
                                ((Button)ctr).Visible = true; break;
                            }

                            if (((Button)ctr).ID.Equals("btnMail") && HttpContext.Current.Session["RoleID"].Equals("2"))
                            { ((Button)ctr).Visible = false; break; }

                            if (((Button)ctr).ID.Equals("btnSwitch")) { ((Button)ctr).Visible = true; break; }

                            if (((Button)ctr).ID.Equals("btnUpdateSubmit") && (Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == (int)Role.User)) { ((Button)ctr).Visible = false; break; }

                            //    && System.Configuration.ConfigurationManager.AppSettings["isDemo"].ToString().Equals("Y"))
                            //{ ((Button)ctr).Visible = true; break; }
                            break;
                        #endregion
                    }
                }

            }

        }
        public void checkboxNA(object sender, MasterPage Master)
        {
            ContentPlaceHolder PlaceHolder2 = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder2");
            UpdatePanel panel = (UpdatePanel)PlaceHolder2.FindControl("up");
            Object ctrs = getAssignObj("Control", ((UpdatePanel)panel).Controls);

            if (ctrs.GetType().Name.ToString().Equals("Control"))
            {
                foreach (object ctrl in ((Control)ctrs).Controls)
                {
                    switch (ctrl.GetType().Name.ToString())
                    {
                        case "TextBox":
                            TextBox txt = (TextBox)ctrl;
                            #region P1模
                            if (((CheckBox)sender).ID.Contains("1"))
                            {
                                if (txt.ID.Contains("1"))
                                {
                                    if (((CheckBox)sender).Checked == false) { txt.Text = "N/A"; txt.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true)
                                    {
                                        txt.Enabled = true;
                                        if (txt.Text == "N/A") { txt.Text = string.Empty; }
                                    }
                                } break;
                            }
                            #endregion
                            #region P2模
                            if (((CheckBox)sender).ID.Contains("2"))
                            {
                                if (txt.ID.Contains("2"))
                                {
                                    if (((CheckBox)sender).Checked == false) { txt.Text = "N/A"; txt.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true)
                                    {
                                        txt.Enabled = true;
                                        if (txt.Text == "N/A") { txt.Text = string.Empty; }
                                    }
                                } break;
                            }
                            #endregion
                            #region P3模
                            if (((CheckBox)sender).ID.Contains("3"))
                            {
                                if (txt.ID.Contains("3"))
                                {
                                    if (((CheckBox)sender).Checked == false) { txt.Text = "N/A"; txt.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true)
                                    {
                                        txt.Enabled = true;
                                        if (txt.Text == "N/A") { txt.Text = string.Empty; }
                                    }
                                } break;
                            }
                            #endregion
                            #region P4模
                            if (((CheckBox)sender).ID.Contains("4"))
                            {
                                if (txt.ID.Contains("4"))
                                {
                                    if (((CheckBox)sender).Checked == false) { txt.Text = "N/A"; txt.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true)
                                    {
                                        txt.Enabled = true;
                                        if (txt.Text == "N/A") { txt.Text = string.Empty; }
                                    }
                                } break;
                            }
                            break;
                            #endregion
                        case "RadioButton":
                            RadioButton rdo = (RadioButton)ctrl;
                            #region P1模
                            if (((CheckBox)sender).ID.Contains("1"))
                            {
                                if (rdo.ID.Contains("1"))
                                {
                                    if (((CheckBox)sender).Checked == false) { rdo.Checked = false; rdo.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true && rdo.Enabled == false) { rdo.Enabled = true; }
                                } break;
                            }
                            #endregion
                            #region P2模
                            if (((CheckBox)sender).ID.Contains("2"))
                            {
                                if (rdo.ID.Contains("2"))
                                {
                                    if (((CheckBox)sender).Checked == false) { rdo.Checked = false; rdo.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true && rdo.Enabled == false) { rdo.Enabled = true; }
                                } break;
                            }
                            #endregion
                            #region P3模
                            if (((CheckBox)sender).ID.Contains("3"))
                            {
                                if (rdo.ID.Contains("3"))
                                {
                                    if (((CheckBox)sender).Checked == false) { rdo.Checked = false; rdo.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true && rdo.Enabled == false) { rdo.Enabled = true; }
                                } break;
                            }
                            #endregion
                            #region P4模
                            if (((CheckBox)sender).ID.Contains("4"))
                            {
                                if (rdo.ID.Contains("4"))
                                {
                                    if (((CheckBox)sender).Checked == false) { rdo.Checked = false; rdo.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true && rdo.Enabled == false) { rdo.Enabled = true; }
                                } break;
                            }
                            break;
                            #endregion
                        case "DropDownList":
                            DropDownList ddl = (DropDownList)ctrl;
                            #region P1模
                            if (((CheckBox)sender).ID.Contains("1"))
                            {
                                if (ddl.ID.Contains("1"))
                                {
                                    if (((CheckBox)sender).Checked == false) { ddl.Items.Clear(); ddl.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true && ddl.Enabled == false) { ddl.Enabled = true; }
                                } break;
                            }
                            #endregion
                            #region P2模
                            if (((CheckBox)sender).ID.Contains("2"))
                            {
                                if (ddl.ID.Contains("2"))
                                {
                                    if (((CheckBox)sender).Checked == false) { ddl.Items.Clear(); ddl.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true && ddl.Enabled == false) { ddl.Enabled = true; }
                                } break;
                            }
                            #endregion
                            #region P3模
                            if (((CheckBox)sender).ID.Contains("3"))
                            {
                                if (ddl.ID.Contains("3"))
                                {
                                    if (((CheckBox)sender).Checked == false) { ddl.Items.Clear(); ddl.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true && ddl.Enabled == false) { ddl.Enabled = true; }
                                } break;
                            }
                            #endregion
                            #region P4模
                            if (((CheckBox)sender).ID.Contains("4"))
                            {
                                if (ddl.ID.Contains("4"))
                                {
                                    if (((CheckBox)sender).Checked == false) { ddl.Items.Clear(); ddl.Enabled = false; }
                                    if (((CheckBox)sender).Checked == true && ddl.Enabled == false) { ddl.Enabled = true; }
                                } break;
                            }
                            break;
                            #endregion
                    }
                }

            }
        }
        public void checkValue(CheckType CheckType, string sheet_categoryID, StringBuilder sb, MasterPage Master)
        {
            ContentPlaceHolder form = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder2");
            ValidatorHelper val = new ValidatorHelper();

            Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, EscConn);
            string str = string.Format(@"Select i.ItemID,i.describes,i.spec,i.aspxcontrolID,i.maxLimit,i.minLimit,i.isNotNull,i.isFloat,
                                                i.datatypeID,dt.describes as datatype_desc 
                                         From item i
                                         Inner join datatype dt on dt.datatypeid=i.datatypeid
                                         Where i.sheet_categoryid='{0}' and (i.isdelete is null or isdelete='N')", sheet_categoryID);
            DataTable dt = ado.loadDataTable(str, null, "item");

            foreach (DataRow dr in dt.Rows)
            {
                switch (dr["datatypeID"].ToString())
                {
                    #region txt

                    case "1"://TxtBox
                        if (dr["aspxControlID"].ToString().Contains("1")) { CheckBox chk = (CheckBox)form.FindControl("chkP1"); if (chk == null) { break; } if (chk.Checked == false) { break; } }
                        if (dr["aspxControlID"].ToString().Contains("2")) { CheckBox chk = (CheckBox)form.FindControl("chkP2"); if (chk == null) { break; } if (chk.Checked == false) { break; } }
                        if (dr["aspxControlID"].ToString().Contains("3")) { CheckBox chk = (CheckBox)form.FindControl("chkP3"); if (chk == null) { break; } if (chk.Checked == false) { break; } }
                        if (dr["aspxControlID"].ToString().Contains("4")) { CheckBox chk = (CheckBox)form.FindControl("chkP4"); if (chk == null) { break; } if (chk.Checked == false) { break; } }

                        TextBox txt = (TextBox)form.FindControl(dr["aspxControlID"].ToString());
                        switch (CheckType)
                        {
                            case CheckType.NullOrEmpty:
                                if (dr["isNotNull"].ToString().Equals("Y") && string.IsNullOrEmpty(txt.Text))
                                {
                                    sb.Append(string.Format(@"【{0}】", dr["describes"].ToString()));
                                }
                                break;

                            case CheckType.Validity:
                                if (dr["isFloat"].ToString().Equals("Y") && !val.IsFloat(txt.Text.Trim()))
                                { sb.Append(string.Format(@"【{0}】", dr["describes"].ToString())); }
                                break;

                            case CheckType.Correct:
                                string maxLimit = dr["MaxLimit"].ToString();
                                string minLimit = dr["MinLimit"].ToString();

                                if (!string.IsNullOrEmpty(maxLimit))
                                    if (Convert.ToDouble(txt.Text) > Convert.ToDouble(maxLimit)) { sb.Append(string.Format(@"【{0},{1}】", dr["describes"].ToString(), dr["spec"].ToString())); }
                                if (!string.IsNullOrEmpty(minLimit))
                                    if (Convert.ToDouble(txt.Text) < Convert.ToDouble(minLimit)) { sb.Append(string.Format(@"【{0},{1}】", dr["describes"].ToString(), dr["spec"].ToString())); }
                                break;
                        }
                        break;
                    #endregion

                    #region rdo

                    case "2"://RadioBox
                        if (dr["aspxControlID"].ToString().Contains("1")) { CheckBox chk = (CheckBox)form.FindControl("chkP1"); if (chk == null) { break; } if (chk.Checked == false) { break; } }
                        if (dr["aspxControlID"].ToString().Contains("2")) { CheckBox chk = (CheckBox)form.FindControl("chkP2"); if (chk == null) { break; } if (chk.Checked == false) { break; } }
                        if (dr["aspxControlID"].ToString().Contains("3")) { CheckBox chk = (CheckBox)form.FindControl("chkP3"); if (chk == null) { break; } if (chk.Checked == false) { break; } }
                        if (dr["aspxControlID"].ToString().Contains("4")) { CheckBox chk = (CheckBox)form.FindControl("chkP4"); if (chk == null) { break; } if (chk.Checked == false) { break; } }

                        RadioButton rdo_Y = (RadioButton)form.FindControl(dr["aspxControlID"].ToString() + "_Y");
                        RadioButton rdo_N = (RadioButton)form.FindControl(dr["aspxControlID"].ToString() + "_N");
                        switch (CheckType)
                        {
                            case CheckType.NullOrEmpty:
                                if (rdo_Y.Checked == false && rdo_N.Checked == false) sb.Append(string.Format(@"【{0}】", dr["describes"].ToString()));
                                break;

                            case CheckType.Correct:
                                #region don't check
                                //AutoMold & TowaMold 集塵袋
                                //Meco                檢查下槽液位是否足夠
                                //TF                  冷卻水
                                if (dr["aspxControlID"].ToString().Equals("rdoFilterBagChange") || dr["aspxControlID"].ToString().Equals("rdoDustBagChange") ||
                                    dr["aspxControlID"].ToString().Equals("rdoSumpTank ") ||
                                    dr["aspxControlID"].ToString().Equals("rdoCoolWater ")) 
                                    break;
                                #endregion
                                if (rdo_N.Checked) sb.Append(string.Format(@"【{0}】", dr["describes"].ToString()));
                                break;
                        }
                        break;
                    #endregion

                    #region ddl

                    case "3"://DropDownList
                        if (dr["aspxControlID"].ToString().Contains("1")) { CheckBox chk = (CheckBox)form.FindControl("chkP1"); if (chk == null) { break; } if (chk.Checked == false) { break; } }
                        if (dr["aspxControlID"].ToString().Contains("2")) { CheckBox chk = (CheckBox)form.FindControl("chkP2"); if (chk == null) { break; } if (chk.Checked == false) { break; } }
                        if (dr["aspxControlID"].ToString().Contains("3")) { CheckBox chk = (CheckBox)form.FindControl("chkP3"); if (chk == null) { break; } if (chk.Checked == false) { break; } }
                        if (dr["aspxControlID"].ToString().Contains("4")) { CheckBox chk = (CheckBox)form.FindControl("chkP4"); if (chk == null) { break; } if (chk.Checked == false) { break; } }

                        DropDownList ddl = (DropDownList)form.FindControl(dr["aspxControlID"].ToString());
                        switch (CheckType)
                        {
                            case CheckType.NullOrEmpty:
                                if (ddl.SelectedValue.Equals("0"))
                                { sb.Append(string.Format(@"【{0}】", dr["describes"].ToString())); }
                                break;
                            case CheckType.Correct:
                                if (ddl.SelectedValue.Equals("3") || ddl.SelectedValue.Equals("6")) //3:需真空壓力異常 6:需離子風扇異常
                                { sb.Append(string.Format(@"【{0}】", dr["describes"].ToString())); }
                                break;
                        }
                        break;
                    #endregion
                }
            }
        }
        public bool checkFilterBag(string sheet_categoryID, string tester)
        {
            DateTime now = DateTime.Now.Date;

            Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, EscConn);

            string timeShift = getTimeShift();
            string sqlstr = "";
            if (now.Day.ToString().Equals("1"))
            {
                sqlstr = string.Format(@"Select item_desc,spec,aspxcontrolid,value 
                                         From vw_sheet_value
                                         Where sheetID=(Select max(sheetID) 
                                                        From vw_sheet_value 
                                                        Where sheet_categoryID='{0}'    And aspxControlID='rdoDustBagChange' 
                                                              And value='Y'             And tester='{1}'
                                                              And time >= '{2}-{3}-16'  And time <= '{2}-{4}-01')
                                         ", sheet_categoryID, tester, now.Year.ToString(), now.AddMonths(-1).Month.ToString(), now.Month.ToString());

            }
            else if (now.Day.ToString().Equals("15"))
            {
                sqlstr = string.Format(@"Select item_desc,spec,aspxcontrolid,value 
                                         From vw_sheet_value
                                         Where sheetID=(Select max(sheetID) 
                                                        From vw_sheet_value 
                                                        Where sheet_categoryID='{0}'    And aspxControlID='rdoDustBagChange' 
                                                              And value='Y'             And tester='{1}'
                                                              And time >= '{2}-{3}-02'  And time <= '{2}-{3}-15')
                                         ", sheet_categoryID, tester, now.Year.ToString(), now.Month.ToString());
            }
            DataTable dt = new DataTable();
            dt = ado.loadDataTable(sqlstr, null, "vw_sheet_value");
            return (dt.Rows.Count > 0) ? true : false;
        }


        //==================================================================================================
        //Get data from webService
        //==================================================================================================
        public string getCgFromIntrack(string batchno)
        {
            LotData.SFCData sfc = new LotData.SFCData();
            LotData.CompleteLotDataProxy lot = sfc.getCompleteLotData(batchno);
            string CG = lot.AssemblyCG.ToString();
            return CG;
        }


        //==================================================================================================
        //Get data from ACS
        //==================================================================================================        
        public DataTable getAcsDataByBatchNo(string batchno, MasterPage Master)
        {
            Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, IsaConn);//ACS_ISA
            ContentPlaceHolder form = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder2");
            string tester = "";
            if (HttpContext.Current.Session["tester"] != null) { tester = HttpContext.Current.Session["tester"].ToString(); }
                        
            DataTable dt = new DataTable();
            try
            {
                string sql = string.Format(@"Select tester,location From vw_address Where tester='{0}'", tester);
                dt = ado.loadDataTable(sql, null, "vw_address");

                ((TextBox)form.FindControl("txtTimeShift")).Text = getTimeShift();
                ((TextBox)form.FindControl("txtDate")).Text = DateTime.Now.ToShortDateString();
                ((TextBox)form.FindControl("txtMachine")).Text = tester;
                ((TextBox)form.FindControl("txtOperator")).Text = HttpContext.Current.Session["account"].ToString();
                if (dt.Rows.Count > 0)
                {
                    ((TextBox)form.FindControl("txtLocation")).Text = dt.Rows[0]["location"].ToString();

                    //PackageSaw                   
                    if ((TextBox)form.FindControl("txtPackage") != null) { ((TextBox)form.FindControl("txtPackage")).Text = getCgFromIntrack(batchno); }
                }
            }
            catch (Exception ce) { }
            return dt;
        }
        public string getTimeShift()
        {
            Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, ConnOpe2);
            DataTable dt = new DataTable();

            string str = "SELECT Shift_Start_Time FROM Shift_Table";
            dt = ado.loadDataTable(str, null, "Shift_Table");

            string[] day = dt.Rows[0]["Shift_Start_Time"].ToString().Split(':');
            string[] middle = dt.Rows[1]["Shift_Start_Time"].ToString().Split(':');
            string[] night = dt.Rows[2]["Shift_Start_Time"].ToString().Split(':');

            TimeSpan dayStart = new TimeSpan(Convert.ToInt32(day[0]), Convert.ToInt32(day[1]), 00);
            TimeSpan middleStart = new TimeSpan(Convert.ToInt32(middle[0]), Convert.ToInt32(middle[1]), 00); ;
            TimeSpan nightStart = new TimeSpan(Convert.ToInt32(night[0]), Convert.ToInt32(night[1]), 00);

            string timeShift = "";
            TimeSpan now = DateTime.Now.TimeOfDay;
            if (TimeSpan.Compare(now, dayStart) > 0 && TimeSpan.Compare(now, middleStart) < 0) { timeShift = "早班"; }
            if (TimeSpan.Compare(now, middleStart) > 0 && TimeSpan.Compare(now, nightStart) < 0) { timeShift = "中班"; }
            if (TimeSpan.Compare(now, nightStart) > 0 || TimeSpan.Compare(now, dayStart) < 0) { timeShift = "晚班"; }

            return timeShift;
        }    


        //==================================================================================================
        //Get data from Hr
        //==================================================================================================
        public DataTable getEmpDeptDt()
        {
            Common.AdoDbConn adoHr = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, HrConn);
            string deptStr = string.Format(@"SELECT distinct DEPT_NAME from EMP_ACCESS_LIST
                                             WHERE  (DEPT_NAME = 'ABE-CENTRAL' OR DEPT_NAME = 'ABE-CHEMICAL'
                                                     OR DEPT_NAME = 'ABE-MOLDING' OR DEPT_NAME = 'ABE-PROJECT' 
                                                     OR DEPT_NAME = 'ABE-QA' OR DEPT_NAME = 'ABE-SINGULATION'
                                                     OR DEPT_NAME = 'PA-PACKAGE SAW')
                                             AND email IS not NULL
                                             order by DEPT_NAME");
            DataTable dtDept = new DataTable();
            dtDept = adoHr.loadDataTable(deptStr, null, "EMP_ACCESS_LIST");
            return dtDept;
        }

    }
}
