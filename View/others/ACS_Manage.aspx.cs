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
    public partial class ACS_Manage : System.Web.UI.Page
    {
        public static string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["ESC"].ToString();
        public Common.AdoDbConn ado;
        public static DataTable dtGv;
        public static StringBuilder sb = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Common.Comfunc func = new Common.Comfunc();
                func.checkLogin();
                func.checkRole(Page.Master);
                initial();
            }
        }

        private void initial()
        {
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
            string str = @"Select sheet_categoryID,describes 
                           From sheet_category 
                           Where (isDelete is Null OR isDelete='N')
                           Order by describes";
            DataTable dt = ado.loadDataTable(str, null, "sheet_category");

            foreach (DataRow dr in dt.Rows)
            {
                ddlSheet.Items.Add(new ListItem(dr["describes"].ToString(), dr["sheet_categoryID"].ToString()));
            }
            ddlSheet.Items.Insert(0, new ListItem("請選擇", "0"));

            gridviewBind();
        }

        private void gridviewBind()
        {
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
            DataTable dt = new DataTable();
            string str = string.Format(@"Select am.acs_manageid,am.tester,am.location,am.sheet_categoryid,sc.describes as sc_desc
                                         From acs_manage am 
                                         Inner join sheet_category sc on am.sheet_categoryid=sc.sheet_categoryid
                                         Where  am.tester like '%{0}%'
                                                And am.location like '%{1}%'
                                                {2}
                                         Order By ACS_ManageID desc", txtTester.Text.Trim().ToUpper(),
                                                                     txtLocation.Text.Trim().ToUpper(),
                                                                     ddlSheet.SelectedValue.Equals("0") ? "" : " And am.sheet_categoryid='" + ddlSheet.SelectedValue + "'");

            dt = ado.loadDataTable(str, null, "ACS_Manage");
            GridViewAM.DataSource = dt;
            GridViewAM.DataBind();
            dtGv = dt;
            showPage();
        }


        protected void GridViewAM_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression.ToString();
            string sortDirection = "ASC";

            if (sortExpression == this.GridViewAM.Attributes["SortExpression"])
                sortDirection = (this.GridViewAM.Attributes["SortDirection"].ToString() == sortDirection ? "DESC" : "ASC");

            this.GridViewAM.Attributes["SortExpression"] = sortExpression;
            this.GridViewAM.Attributes["SortDirection"] = sortDirection;

            if ((!string.IsNullOrEmpty(sortExpression)) && (!string.IsNullOrEmpty(sortDirection)))
            {
                dtGv.DefaultView.Sort = string.Format("{0} {1}", sortExpression, sortDirection);
            }
            GridViewAM.DataSource = dtGv;
            GridViewAM.DataBind();
            showPage();
        }
        protected void GridViewAM_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewAM.EditIndex = e.NewEditIndex;
            GridViewAM.DataSource = dtGv;
            GridViewAM.DataBind();
            showPage();
        }
        protected void GridViewAM_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewAM.EditIndex = -1;
            GridViewAM.DataSource = dtGv;
            GridViewAM.DataBind();
            showPage();
        }
        protected void GridViewAM_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox gv_txtTester = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtTester");
            TextBox gv_txtLocation = (TextBox)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_txtLocation");
            DropDownList gv_ddlSheet = (DropDownList)GridViewAM.Rows[e.RowIndex].Cells[0].FindControl("gv_ddlSheet");

            string pk = GridViewAM.DataKeys[e.RowIndex].Value.ToString();

            //check
            sb = new StringBuilder();
            bool check = true;

            if (string.IsNullOrEmpty(gv_txtTester.Text)) { sb.Append("【Tester】"); }
            if (string.IsNullOrEmpty(gv_txtLocation.Text)) { sb.Append("【Location】"); }
            if (gv_ddlSheet.SelectedValue.Equals("0")) { sb.Append("【Sheet】"); }
            if (!string.IsNullOrEmpty(sb.ToString())) { check = false; sb.Insert(0, "以下欄位未填\\n\\n"); }

            if (check)
            {
                string oldStr = string.Format(@"Select tester,location From ACS_Manage Where ACS_Manageid='{0}'", pk);
                DataTable old_dt = ado.loadDataTable(oldStr, null, "ACS_Manage");
                
                string TestStr = string.Format(@"Select * From ACS_Manage Where tester='{0}' And tester!='{1}'", gv_txtTester.Text.Trim().ToUpper(),old_dt.Rows[0]["tester"].ToString());
                DataTable tester_dt = ado.loadDataTable(TestStr, null, "ACS_Manage");
                if (tester_dt.Rows.Count > 0) { sb.Append("【Tester】"); }
                
                string LocationStr = string.Format(@"Select * From ACS_Manage Where location='{0}' And location!='{1}'", gv_txtLocation.Text.Trim().ToUpper(),old_dt.Rows[0]["location"].ToString());
                DataTable Location_dt = ado.loadDataTable(LocationStr, null, "ACS_Manage");
                if (Location_dt.Rows.Count > 0) { sb.Append("【Location】"); }
                if (!string.IsNullOrEmpty(sb.ToString())) { check = false; sb.Insert(0, "以下欄位重複定義，請檢查\\n\\n"); }
            }

            if (check)
            {
                ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
                string str = string.Format(@"Update ACS_Manage 
                                             Set    tester = '{0}',
                                                    Location ='{1}',
                                                    sheet_categoryID='{2}'
                                             Where  ACS_ManageID = '{3}'", gv_txtTester.Text.Trim().ToUpper(),
                                                                          gv_txtLocation.Text.Trim().ToUpper(),
                                                                          gv_ddlSheet.SelectedValue,
                                                                          pk);
                //ado.dbNonQuery(str, null);
                string reStr = (string)ado.dbNonQuery(str, null);
                if (reStr.ToUpper().Contains("SUCCESS"))
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('該筆資料修改成功。');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", reStr), true);

                GridViewAM.EditIndex = -1;
                gridviewBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", sb.ToString()), true);
            }
        }
        protected void GridViewAM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                string pk = GridViewAM.DataKeys[e.Row.RowIndex].Value.ToString();
                DropDownList gv_ddlSheet = (DropDownList)e.Row.FindControl("gv_ddlSheet");

                ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
                string str = @"select sheet_categoryID,describes 
                               from sheet_category
                               Where isDelete is Null OR isDelete = 'N'
                               order by describes";
                DataTable dt = ado.loadDataTable(str, null, "sheet_category");

                foreach (DataRow dr in dt.Rows)
                {
                    gv_ddlSheet.Items.Add(new ListItem(dr["describes"].ToString(), dr["sheet_categoryID"].ToString()));
                }
                gv_ddlSheet.Items.Insert(0, new ListItem("請選擇", "0"));

                str = string.Format(@"Select sheet_categoryID 
                                      From acs_Manage 
                                      WHERE acs_ManageID='{0}'", pk);
                dt = ado.loadDataTable(str, null, "acs_Manage");
                gv_ddlSheet.SelectedValue = dt.Rows[0]["sheet_categoryID"].ToString();
            }
        }
        protected void GridViewAM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewAM.PageIndex = e.NewPageIndex;
            GridViewAM.DataSource = dtGv;
            GridViewAM.DataBind();
            showPage();
        }
        protected void GridViewAM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
            string pk = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "myDelete":
                    string sqlStr = string.Format(@"Delete from ACS_Manage
                                                    Where ACS_ManageID='{0}'", pk);
                    //ado.dbNonQuery(sqlStr, null);
                    string reStr = (string)ado.dbNonQuery(sqlStr, null);
                    if (reStr.ToUpper().Contains("SUCCESS"))
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('該筆資料刪除成功。');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", reStr), true);

                    gridviewBind();
                    break;
            }

        }

        #region PagerTemplate
        protected void lbnFirst_Click(object sender, EventArgs e)
        {
            int num = 0;

            GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
            GridViewAM_PageIndexChanging(null, ea);
        }
        protected void lbnPrev_Click(object sender, EventArgs e)
        {
            int num = GridViewAM.PageIndex - 1;

            if (num >= 0)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridViewAM_PageIndexChanging(null, ea);
            }
        }
        protected void lbnNext_Click(object sender, EventArgs e)
        {
            int num = GridViewAM.PageIndex + 1;

            if (num < GridViewAM.PageCount)
            {
                GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                GridViewAM_PageIndexChanging(null, ea);
            }
        }
        protected void lbnLast_Click(object sender, EventArgs e)
        {
            int num = GridViewAM.PageCount - 1;

            GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
            GridViewAM_PageIndexChanging(null, ea);
        }
        private void showPage()
        {
            try
            {
                TextBox txtPage = (TextBox)GridViewAM.BottomPagerRow.FindControl("txtSizePage");
                Label lblCount = (Label)GridViewAM.BottomPagerRow.FindControl("lblTotalCount");
                Label lblPage = (Label)GridViewAM.BottomPagerRow.FindControl("lblPage");
                Label lblbTotal = (Label)GridViewAM.BottomPagerRow.FindControl("lblTotalPage");

                txtPage.Text = GridViewAM.PageSize.ToString();
                lblCount.Text = dtGv.Rows.Count.ToString();
                lblPage.Text = (GridViewAM.PageIndex + 1).ToString();
                lblbTotal.Text = GridViewAM.PageCount.ToString();

                GridViewAM.Columns[2].Visible = true;//IsNumeric
                GridViewAM.Columns[7].Visible = true;//MaxLimit
                GridViewAM.Columns[8].Visible = true;//MinLimit
            }
            catch (Exception ex) { }
        }
        // page change
        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                string numPage = ((TextBox)GridViewAM.BottomPagerRow.FindControl("txtSizePage")).Text.ToString();
                if (!string.IsNullOrEmpty(numPage))
                {
                    GridViewAM.PageSize = Convert.ToInt32(numPage);
                }

                TextBox pageNum = ((TextBox)GridViewAM.BottomPagerRow.FindControl("inPageNum"));
                string goPage = pageNum.Text.ToString();
                if (!string.IsNullOrEmpty(goPage))
                {
                    int num = Convert.ToInt32(goPage) - 1;
                    if (num >= 0)
                    {
                        GridViewPageEventArgs ea = new GridViewPageEventArgs(num);
                        GridViewAM_PageIndexChanging(null, ea);
                        ((TextBox)GridViewAM.BottomPagerRow.FindControl("inPageNum")).Text = null;
                    }
                }

                GridViewAM.DataSource = dtGv;
                GridViewAM.DataBind();
                showPage();
            }
            catch (Exception ex) { }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridviewBind();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            GridViewAM.DataSource = null;
            GridViewAM.DataBind();
            DetailsView1.ChangeMode(DetailsViewMode.Insert);
        }


        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
            TextBox dv_txtTester = ((TextBox)DetailsView1.FindControl("dv_txtTester"));
            TextBox dv_txtLocation = ((TextBox)DetailsView1.FindControl("dv_txtLocation"));
            DropDownList dv_ddlSheet = ((DropDownList)DetailsView1.FindControl("dv_ddlSheet")); 
            //Check
            sb = new StringBuilder();
            bool check = true;

            if (string.IsNullOrEmpty(dv_txtTester.Text)) { sb.Append("【Tester】"); }
            if (string.IsNullOrEmpty(dv_txtLocation.Text)) { sb.Append("【Location】"); }
            if (dv_ddlSheet.SelectedValue.Equals("0")) { sb.Append("【Sheet】"); }
            if (!string.IsNullOrEmpty(sb.ToString())) { check = false; sb.Insert(0, "以下欄位未填\\n\\n"); }

            if (check == true)
            {
                string TestStr = string.Format(@"Select * From ACS_Manage Where tester='{0}'", dv_txtTester.Text.Trim().ToUpper());
                DataTable tester_dt = ado.loadDataTable(TestStr, null, "ACS_Manage");
                if (tester_dt.Rows.Count > 0) { sb.Append("【Tester】"); }
                string LocationStr = string.Format(@"Select * From ACS_Manage Where location='{0}'", dv_txtLocation.Text.Trim().ToUpper());
                DataTable Location_dt = ado.loadDataTable(LocationStr, null, "ACS_Manage");
                if (Location_dt.Rows.Count > 0) { sb.Append("【Location】"); }
                if (!string.IsNullOrEmpty(sb.ToString())) { check = false; sb.Insert(0, "以下欄位重複定義，請檢查\\n\\n"); }
            }

            if (check)
            {
                string sqlStr = string.Format(@"Insert into ACS_Manage(ACS_ManageID,Tester,Location,sheet_categoryID)
                                                Values (Acs_Manage_sequence.nextval,:Tester,:Location,:sheet_categoryID)");
                object[] para = new object[] { dv_txtTester.Text.Trim().ToUpper(), dv_txtLocation.Text.Trim().ToUpper(), dv_ddlSheet.SelectedValue };
                //ado.dbNonQuery(sqlStr, para);
                string reStr = (string)ado.dbNonQuery(sqlStr, para);
                if (reStr.ToUpper().Contains("SUCCESS"))
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", "alert('該筆資料新增成功。');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", reStr), true);

                DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
                DetailsView1.DataSource = null;
                DetailsView1.DataBind();

                gridviewBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "js", string.Format(@"alert('{0}');", sb.ToString()), true);
            }

        }
        protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
            {
                DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
                DetailsView1.DataSource = null;
                DetailsView1.DataBind();

                gridviewBind();
            }
        }
        protected void DetailsView1_DataBound(object sender, EventArgs e)
        {
            if (DetailsView1.CurrentMode == DetailsViewMode.Insert)
            {
                DropDownList dv_ddlSheet = (DropDownList)DetailsView1.FindControl("dv_ddlSheet");

                ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
                string str = @"Select sheet_categoryID,describes 
                               From sheet_category
                               Where isDelete is null Or isDelete = 'N' 
                               Order by describes";
                DataTable dt = ado.loadDataTable(str, null, "sheet_category");

                foreach (DataRow dr in dt.Rows)
                {
                    dv_ddlSheet.Items.Add(new ListItem(dr["describes"].ToString(), dr["sheet_categoryID"].ToString()));
                }
                dv_ddlSheet.Items.Insert(0, new ListItem("請選擇", "0"));
            }
        }
        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {

        }

    }
}