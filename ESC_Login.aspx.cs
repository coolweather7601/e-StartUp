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
using System.Collections.Generic;

namespace ESC_Web.Alan
{
    public partial class ESC_Login : System.Web.UI.Page
    {
        public static string HrConn = System.Configuration.ConfigurationManager.ConnectionStrings["HR"].ToString();
        public static string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["ESC"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["account"] != null) 
                {
                    string sheet_categoryID = Request.QueryString["sheet_categoryID"];
                    string sheetID = Request.QueryString["SheetID"];

                    Session["tester"] = Request.QueryString["tester"];
                    Session["conn"] = Request.QueryString["conn"];

                    PageSwitch(Request.QueryString["tester"], sheet_categoryID, sheetID); 
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string sheet_categoryID = Request.QueryString["sheet_categoryID"];
            string sheetID = Request.QueryString["SheetID"];

            Session["tester"] = Request.QueryString["tester"];
            Session["conn"] = Request.QueryString["conn"];

            #region ESC
            Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
            DataTable dt = new DataTable();
            string sqlstr = @"SELECT * 
                              FROM Users
                              WHERE account=:account 
                                    AND password=:password";
            object[] para = new object[] { txtAcc.Text.Trim(), txtPw.Text.Trim() };

            dt = ado.loadDataTable(sqlstr, para, "Users");
            if (dt.Rows.Count > 0)
            {
                Session["account"] = dt.Rows[0]["account"].ToString();
                Session["RoleID"] = dt.Rows[0]["RoleID"].ToString();
                Session["Name"] = dt.Rows[0]["Name"].ToString();
                Session["Dept_Name"] = dt.Rows[0]["Dept"].ToString();
                Session["UsersID"] = dt.Rows[0]["UsersID"].ToString();
                PageSwitch(Request.QueryString["tester"], sheet_categoryID, sheetID);
                return;
            }
            #endregion
            #region EMP
            Common.AdoDbConn adoHr = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, HrConn);
            DataTable dtEmp = new DataTable();
            string strEmp = string.Format(@"SELECT ltrim(EMP_NO,'0') as account,passwd,Emp_Name,Dept_NAME,EMP_NO,email
                              FROM   EMP_ACCESS_LIST 
                              WHERE  Emp_No like '%{0}%' AND Passwd='{1}'", txtAcc.Text.Trim(), txtPw.Text.Trim());
            dtEmp = adoHr.loadDataTable(strEmp, null, "EMP_ACCESS_LIST");
            if (dtEmp.Rows.Count > 0)
            {
                Session["account"] = dtEmp.Rows[0]["Emp_No"].ToString();
                Session["Name"] = dtEmp.Rows[0]["Emp_Name"].ToString();
                Session["Dept_Name"] = dtEmp.Rows[0]["Dept_Name"].ToString();
                Session["RoleID"] = "2";//general user

                string insertStr = @"INSERT INTO Users(UsersID,RoleID,account,password,name,dept,EMP_NO,Mail)
                                     VALUES (Users_sequence.nextval,'2',:account,:password,:name,:dept,:EMP_NO,:Mail)";
                object[] param = new object[] { dtEmp.Rows[0]["account"].ToString(),dtEmp.Rows[0]["passwd"].ToString(),
                                                dtEmp.Rows[0]["Emp_Name"].ToString(),dtEmp.Rows[0]["Dept_Name"].ToString(),
                                                dtEmp.Rows[0]["EMP_NO"].ToString(),dtEmp.Rows[0]["email"].ToString()};
                ado.dbNonQuery(insertStr, param);
                PageSwitch(Request.QueryString["tester"], sheet_categoryID, sheetID);
                return;
            }
            #endregion

            ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('account/password 輸入錯誤，請重新檢查');", true);
            txtAcc.Text = null;
            txtPw.Text = null;
            SetFocus(txtAcc);
        }
        private void PageSwitch(string tester, string sheet_categoryID, string sheetID)
        {
            try
            {
                string url = "";

                if (!string.IsNullOrEmpty(sheet_categoryID) && !string.IsNullOrEmpty(sheetID))
                {
                    Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
                    string sc_sqlStr = string.Format(@"Select describes
                                                       From sheet_category  
                                                       Where sheet_categoryID='{0}' and (isDelete is Null OR isDelete='N')", sheet_categoryID);
                    DataTable sc_dt = ado.loadDataTable(sc_sqlStr, null, "sheet_category");
                    if (sc_dt.Rows.Count > 0)
                    {
                        url = string.Format(@"View/sheet/{0}.aspx?sheet_categoryID={1}&sheetID={2}", sc_dt.Rows[0]["describes"].ToString(),
                                                                                                     sheet_categoryID,
                                                                                                     sheetID);
                        Response.Redirect(string.Format(@"{0}", url), false);
                    }
                }
                else if (!string.IsNullOrEmpty(tester))
                {
                    Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
                    string Am_sqlStr = string.Format(@"Select am.tester,am.location,am.sheet_categoryid,sc.describes as sc_desc
                                                       From Acs_Manage am 
                                                       Inner Join Sheet_Category sc on am.sheet_categoryid=sc.sheet_categoryid 
                                                       Where am.tester='{0}' and (sc.isDelete is Null OR sc.isDelete='N')", tester.ToUpper());
                    DataTable Am_dt = ado.loadDataTable(Am_sqlStr, null, "Acs_Manage");

                    if (Am_dt.Rows.Count > 0)
                    {
                        url = Am_dt.Rows.Count > 0 ? string.Format(@"View/sheet/{0}.aspx?sheet_categoryID={1}", Am_dt.Rows[0]["sc_desc"].ToString(),
                                                                                                                Am_dt.Rows[0]["sheet_categoryid"].ToString())
                                                   : url;
                        Response.Redirect(string.Format(@"{0}", url), false);
                    }
                }
                else
                {
                    url = "View/others/listIndex.aspx";
                    Response.Redirect(string.Format(@"{0}", url), false);
                }
            }
            catch (Exception ex) { }
        }

        private void importDB()
        {
//            List<string> arrStr = new List<string>();
//            Common.AdoDbConn ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);

//            string str = string.Format(@"SELECT acsmanageid,tester,location,sheet_categoryid from acsmanage order by acsmanageid");


//            DataTable dt = ado.loadDataTable(str, null, "acsmanage");

//            foreach (DataRow dr in dt.Rows)
//            {
//                //insert sheet_value
//                string value_str = string.Format(@"insert into acs_manage(acs_manageid,tester,location,sheet_categoryid)
//                                                   Values(acs_manage_sequence.nextval,'{0}','{1}','{2}')",dr["tester"].ToString(),
//                                                                                                         dr["location"].ToString(),
//                                                                                                         dr["sheet_categoryid"].ToString());

//                arrStr.Add(value_str);
//            }
//            string reStr = ado.SQL_transaction(arrStr, Conn);
//            ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('" + reStr + "');", true);
        }
    }
}