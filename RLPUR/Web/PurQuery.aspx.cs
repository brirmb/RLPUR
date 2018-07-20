using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RLPUR.Common;
using RLPUR.Models;

namespace RLPUR.Web
{
    public partial class PurQuery : System.Web.UI.Page
    {
        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                #region 验证权限

                #endregion

                #region 获取参数

                #endregion

                //初始化
                this.Initialize();
            }

            #region 页面标题

            this.Title = PageTitle.Text;

            #endregion

            #region 页面要素


            #endregion
        }

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            #region 页面内容

            PRNoFrom.Text = string.Empty;
            PRNoTo.Text = string.Empty;
            OrNoFrom.Text = string.Empty;
            OrNoTo.Text = string.Empty;
            VenFrom.Text = string.Empty;
            VenTo.Text = string.Empty;
            DateFrom.Text = string.Empty;
            DateTo.Text = string.Empty;
            DrawNoFrom.Text = string.Empty;
            DrawNoTo.Text = string.Empty;
            PrlNo.Text = string.Empty;
            PrlName.Text = string.Empty;

            #endregion

            //绑定列表
            //this.BindList();
        }

        #endregion

        #region 绑定数据

        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindList()
        {
            using (PurProvider purProvider = new PurProvider())
            {
                string dateFrom = LocalGlobal.ConvertDateFormat(DateFrom.Text.Trim()).ToString("yyyyMMdd");
                string dateTo = LocalGlobal.ConvertDateFormat(DateTo.Text.Trim()).ToString("yyyyMMdd");
                DataTable table = purProvider.PurQuery(PRNoFrom.Text.Trim(), PRNoTo.Text.Trim(), OrNoFrom.Text.Trim(), OrNoTo.Text.Trim(), VenFrom.Text.Trim(), VenTo.Text.Trim(), dateFrom, dateTo, DrawNoFrom.Text.Trim(), DrawNoTo.Text.Trim(), PrlNo.Text.Trim(), PrlName.Text.Trim());

                List.DataSource = table;

                decimal amt = 0;
                foreach (DataRow row in table.Rows)
                {
                    amt += Util.ToDecimal(row["PRLPACST"].ToString()) * Util.ToDecimal(row["PRLQTY"].ToString());
                }

                SumAmt.Text = amt.ToString();
            }
            List.DataBind();
        }

        /// <summary>
        /// 行绑定
        /// </summary>
        protected void List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                //数据行
                case DataControlRowType.DataRow:
                    #region 数据绑定

                    string price = ((DataRowView)e.Row.DataItem)["PRLPACST"].ToString().Trim();
                    string qty = ((DataRowView)e.Row.DataItem)["PRLQTY"].ToString().Trim();

                    decimal amt = Util.ToDecimal(price) * Util.ToDecimal(qty);
                    e.Row.Cells[13].Text = amt.ToString();
                    #endregion
                    break;

                case DataControlRowType.EmptyDataRow:
                case DataControlRowType.Header:
                case DataControlRowType.Separator:
                case DataControlRowType.Pager:
                case DataControlRowType.Footer:
                default:
                    break;
            }
        }

        /// <summary>
        /// 翻页
        /// </summary>
        protected void List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            List.PageIndex = e.NewPageIndex;
            this.BindList();
        }

        #endregion

        #region 操作

        /// <summary>
        /// 确定
        /// </summary>
        protected void OKButton_Click(object sender, EventArgs e)
        {
            this.BindList();
        }

        /// <summary>
        /// 取消
        /// </summary>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            this.Initialize();
        }

        protected void ExportButton_Click(object sender, EventArgs e)
        {
            this.List.AllowPaging = false;
            this.BindList();
            LocalGlobal.ToExcel(this.List, "请购查询");
            this.List.AllowPaging = true;
            this.BindList();
        }

        #endregion

    }
}