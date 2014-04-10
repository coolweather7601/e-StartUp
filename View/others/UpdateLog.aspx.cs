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

namespace ESC_Web.Alan
{
    public partial class UpdateLog : System.Web.UI.Page
    {
        public static string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["ESC"].ToString();
        public Common.AdoDbConn ado;

        public static string sheetID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sheetID = Request.QueryString["sheetID"];
                initial();
            }
        }
        private void initial()
        {
            //account describes
            ado = new Common.AdoDbConn(Common.AdoDbConn.AdoDbType.Oracle, Conn);
            string strsql = string.Format(@"Select u.account,vw.item_desc,l.oldValue,l.newValue,l.Time
                                            From   log l
                                            Inner  join vw_sheet_value vw on l.sheet_valueID=vw.sheet_valueid
                                            Inner  join users u on vw.usersid=u.usersid
                                            WHERE  l.sheetID='{0}'
                                            Order  by Time DESC", sheetID);
            DataTable dt = ado.loadDataTable(strsql, null, "Log");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}