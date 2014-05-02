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
    public partial class listIndex : System.Web.UI.Page
    {
        public static string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["ESC"].ToString();
        public Common.AdoDbConn ado;
        public static DataTable dtGv;

        public static string sheet_categoryID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ddlBind
                ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
                string str = "select * from ddlitems where category='status'";
                DataTable dt = ado.loadDataTable(str, null, "ddlitems");
                ddlMold.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    ddlMold.Items.Add(new ListItem(dr["describes"].ToString(), dr["ddlItemsID"].ToString()));
                }
                ddlMold.Items.Insert(0, new ListItem("請選擇", ""));


                str = "Select sheet_categoryID,describes From sheet_category Order by describes";
                dt = ado.loadDataTable(str, null, "sheet_category");
                ddlSheet.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    ddlSheet.Items.Add(new ListItem(dr["describes"].ToString(), dr["sheet_categoryID"].ToString()));
                }
                sheet_categoryID = "1"; //預設為(AutoMold)

                //checkRole
                Common.Comfunc func = new Common.Comfunc();
                func.checkLogin();
                func.checkRole(Page.Master);

                initial();
            }
        }
        private void initial()
        {
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);

            string str = string.Format(@"Select s.sheetID,s.sheet_categoryID,s.remarkid,s.tester,s.location,s.fixengineer,s.fixtime,
                                                l.time,r.describes as remark_desc
                                         From sheet s
                                         Inner join log l on s.sheetID=l.sheetID
                                         Left join remark r on s.remarkid=r.remarkid
                                         Inner join vw_sheet_value vw on s.sheetID=vw.sheetid  And (vw.aspxcontrolid='ddlStatus' and vw.value like'%{5}%')                                         
                                         Where (S.ISDELETE is null or s.isdelete = 'N')
                                               And s.sheet_categoryID = '{0}'
                                               And s.tester like '%{1}%'
                                               And s.location like'%{2}%'
                                               And L.LOGACTION ='INSERT'
                                               And l.time >= TO_DATE ('{3} 00:00:00', 'YYYY/MM/DD hh24:mi:ss')
                                               And l.time <= TO_DATE ('{4} 23:59:59', 'YYYY/MM/DD hh24:mi:ss')
                                         ORDER BY time DESC", sheet_categoryID,
                                                              txtMachine.Text.Trim(), txtLocation.Text.Trim(),
                                                              string.IsNullOrEmpty(txtStart.Text) ? "1001/01/01" : txtStart.Text,
                                                              string.IsNullOrEmpty(txtEnd.Text) ? "9999/12/31" : txtEnd.Text,
                                                              (ddlMold.SelectedItem.Text.Equals("請選擇")) ? "" : ddlMold.SelectedItem.Text,
                                                              ddlSheet.SelectedValue.Equals("10") ? "" : string.Format(@"Inner join vw_sheet_value vw2 on s.sheetID = vw2.sheetID And (vw2.aspxcontrolid like 'txtBatchCardNo%' and vw2.value like'%{0}%')", txtSearch.Text.Trim())
                                                              );

            DataTable dt = ado.loadDataTable(str, null, "sheet");

            GridView gv = new GridView();

            GridView1.DataSource = dt;
            GridView1.DataBind();
            dtGv = dt;
            showPage(GridView1);
        }

        private void deleteData(string sheetID)
        {
            try
            {
                Common.Comfunc func = new Common.Comfunc();
                ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
                List<string> arrStr = new List<string>();

                //==================================================================================================
                //add transaction delete_sql
                //==================================================================================================
                string delete_sql = string.Format(@"Update sheet
                                                    Set    isDelete='Y'
                                                    Where  sheetID='{0}'", sheetID);
                arrStr.Add(delete_sql);


                //==================================================================================================
                //add transaction log
                //==================================================================================================
                string insert_log = string.Format(@"insert into Log(LogID,sheetID,LogAction,Time)
                                                    Values(log_sequence.nextval,'{0}','DELETE',SYSDATE)", sheetID);
                arrStr.Add(insert_log);


                //==================================================================================================
                //execute transaction_SQL
                //==================================================================================================
                string reStr = ado.SQL_transaction(arrStr, Conn);
                if (reStr.ToUpper().Contains("SUCCESS"))
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('該筆資料刪除成功');"), true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('Fail, {0}');", reStr), true);


                initial();
            }
            catch (Exception ex) { }
        }

        protected void ddlSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            sheet_categoryID = ddlSheet.SelectedValue;
            initial();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            initial();
        }
        protected void ddlMold_SelectedIndexChanged(object sender, EventArgs e)
        {
            initial();
        }


        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("view")) 
            {
                string urlStr = string.Format(@"../sheet/{0}.aspx?sheet_CategoryID={1}&sheetID={2}", ddlSheet.SelectedItem.Text, ddlSheet.SelectedValue, e.CommandArgument.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", string.Format(@"window.open('{0}','_blank','width=1100,height=600,scrollbars=yes');", urlStr), true); 
            }

            if (e.CommandName.Equals("del")) { deleteData(e.CommandArgument.ToString()); } 
           
            if (e.CommandName.Equals("log"))
            {
                string str = string.Format(@"UpdateLog.aspx?sheetID={0}", e.CommandArgument.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", "window.open('" + str + "','_blank','width=700,height=600,scrollbars=yes');", true);
            }
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = dtGv;
            GridView1.DataBind();
            showPage(GridView1);
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GridView1.DataSource = dtGv;
            GridView1.DataBind();
            showPage(GridView1);
        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GridView1.DataSource = dtGv;
            GridView1.DataBind();
            showPage(GridView1);
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
            string dk = GridView1.DataKeys[e.RowIndex].Value.ToString();
            string describe = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[0].FindControl("ddlDescribe")).SelectedValue;

            string str = @"Update sheet 
                           Set remarkID=:remarkID,
                               fixEngineer=:fixEngineer,
                               fixTime=SYSDATE
                           WHERE sheetID=:sheetID";
            object[] para = new object[] { describe, Session["account"], dk };

            string reStr = (string)ado.dbNonQuery(str, para);
            if (reStr.ToUpper().Contains("SUCCESS"))
                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('該筆資料修改成功。');", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", reStr), true);

            GridView1.EditIndex = -1;
            initial();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
                string dk = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();

                //=====================================================================================
                //BatchCardNo
                //=====================================================================================
                Label lbl_batch = (Label)e.Row.FindControl("lblBatch");

                string str_batch = string.Format(@"Select item_desc,value 
                                                    From vw_sheet_value
                                                    Where aspxcontrolID like 'txtBatchCardNo%' And sheetID='{0}'", dk);
                DataTable dt_batch = ado.loadDataTable(str_batch, null, "vw_sheet_value");

                if (ddlSheet.SelectedValue.Equals("3"))//Towa(4 batchcard)
                {
                    int Pmold = 1;
                    foreach (DataRow dr in dt_batch.Rows)
                    {
                        lbl_batch.Text += string.IsNullOrEmpty(lbl_batch.Text) ? string.Format(@"(P{0}){1}", Pmold, dr["value"].ToString())
                                                                               : string.Format(@" /(P{0}){1}", Pmold, dr["value"].ToString());
                        Pmold++;
                    }
                }
                else if (ddlSheet.SelectedValue.Equals("10"))//CleanStar(0 batchcard)
                {
                    lbl_batch.Text = "";
                }
                else
                    lbl_batch.Text = dt_batch.Rows[0]["value"].ToString();



                //=====================================================================================
                //Change Kind
                //=====================================================================================
                Label lbl_status = (Label)e.Row.FindControl("lblStatus");

                string str_status = string.Format(@"Select item_desc,value 
                                                    From vw_sheet_value
                                                    Where aspxcontrolID='ddlStatus' And sheetID='{0}'", dk);
                DataTable dt_status = ado.loadDataTable(str_status, null, "vw_sheet_value");
                lbl_status.Text = dt_status.Rows[0]["value"].ToString();
                

                if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
                {
                    //=====================================================================================
                    //Remark
                    //=====================================================================================
                    string str_remark = "select RemarkID, Describes From Remark";
                    DataTable dt_remark = ado.loadDataTable(str_remark, null, "Remark");

                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlDescribe");
                    ddl.Items.Clear();
                    foreach (DataRow dr in dt_remark.Rows)
                    {
                        ddl.Items.Add(new ListItem(dr["Describes"].ToString(), dr["RemarkID"].ToString()));
                    }

                    str_remark = string.Format(@"Select remarkID From sheet Where sheetID='{0}'", dk);
                    dt_remark = ado.loadDataTable(str_remark, null, "sheet");
                    ddl.SelectedValue = dt_remark.Rows[0]["remarkID"].ToString();
                }
            }
        }
        #region PagerTemplate
        protected void lbnFirst_Click(object sender, EventArgs e)
        {
            int num = 0;
            GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
            GridView1_PageIndexChanging(GridView1, ea);
        }
        protected void lbnPrev_Click(object sender, EventArgs e)
        {
            int num = GridView1.PageIndex - 1;
            if (num >= 0)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridView1_PageIndexChanging(GridView1, ea);
            }
        }
        protected void lbnNext_Click(object sender, EventArgs e)
        {
            int num = GridView1.PageIndex + 1;
            if (num < GridView1.PageCount)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridView1_PageIndexChanging(GridView1, ea);
            }
        }
        protected void lbnLast_Click(object sender, EventArgs e)
        {
            int num = GridView1.PageCount - 1;
            GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
            GridView1_PageIndexChanging(GridView1, ea);
        }
        private void showPage(object sender)
        {
            if (((GridView)sender).Rows.Count > 0)
            {
                TextBox txtPage = (TextBox)((GridView)sender).BottomPagerRow.FindControl("txtSizePage");
                Label lblCount = (Label)((GridView)sender).BottomPagerRow.FindControl("lblTotalCount");
                Label lblPage = (Label)((GridView)sender).BottomPagerRow.FindControl("lblPage");
                Label lblbTotal = (Label)((GridView)sender).BottomPagerRow.FindControl("lblTotalPage");

                txtPage.Text = ((GridView)sender).PageSize.ToString();
                lblPage.Text = (((GridView)sender).PageIndex + 1).ToString();
                lblbTotal.Text = ((GridView)sender).PageCount.ToString();
                lblCount.Text = dtGv.Rows.Count.ToString();
            }
        }
        // page change
        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string numPage = ((TextBox)GridView1.BottomPagerRow.FindControl("txtSizePage")).Text.ToString();
                if (!string.IsNullOrEmpty(numPage)) { GridView1.PageSize = Convert.ToInt32(numPage); }

                TextBox pageNum = ((TextBox)GridView1.BottomPagerRow.FindControl("inPageNum"));
                string goPage = pageNum.Text.ToString();

                if (!string.IsNullOrEmpty(goPage))
                {
                    int num = Convert.ToInt32(goPage) - 1;
                    if (num >= 0)
                    {
                        GridViewPageEventArgs ea = new GridViewPageEventArgs(num);

                        GridView1_PageIndexChanging(GridView1, ea);
                        ((TextBox)GridView1.BottomPagerRow.FindControl("inPageNum")).Text = null;
                    }
                }

                GridView1.DataSource = dtGv;
                GridView1.DataBind();
                showPage(GridView1);
            }
            catch (Exception ex) { }
        }
        #endregion

    }
}