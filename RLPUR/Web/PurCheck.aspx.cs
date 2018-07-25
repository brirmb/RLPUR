using RLPUR.Common;
using RLPUR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace RLPUR.Web
{
    public partial class PurCheck : LocalPage
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
                //this.Initialize();
            }

            #region 页面标题

            this.Title = PageTitle.Text;

            #endregion

            #region 页面要素


            #endregion
        }

        #region 绑定数据

        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindList()
        {
            using (PurProvider purProvider = new PurProvider())
            {
                DataTable table = purProvider.GetPRCheckList();
                List.DataSource = table;
            }
            List.DataBind();

            this.List.SelectedIndex = -1;
            DetailList.DataSource = null;
            DetailList.DataBind();
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

                    //string price = ((DataRowView)e.Row.DataItem)["PRLPACST"].ToString().Trim();
                    //string qty = ((DataRowView)e.Row.DataItem)["PRLQTY"].ToString().Trim();

                    //decimal amt = Util.ToDecimal(price) * Util.ToDecimal(qty);
                    //e.Row.Cells[13].Text = amt.ToString();
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

        protected void List_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            this.List.SelectedIndex = e.NewSelectedIndex;
            string no = this.List.DataKeys[this.List.SelectedIndex]["PRHNO"].ToString();

            //绑定详情列表
            //DetailLabel.Visible = true;
            using (PurProvider purProvider = new PurProvider())
            {
                DetailList.DataSource = purProvider.GetPRCheckDetail(no);
            }
            DetailList.DataBind();
        }

        protected void DetailList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                //数据行
                case DataControlRowType.DataRow:
                    #region 数据绑定

                    //var unitPrice = Util.ToDecimal(e.Row.Cells[5].Text.Trim()); //单价

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

        #endregion

        #region 操作

        /// <summary>
        /// 查询
        /// </summary>
        protected void OKButton_Click(object sender, EventArgs e)
        {
            this.BindList();
        }

        /// <summary>
        /// 核准
        /// </summary>
        protected void ApproveButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = LocalGlobal.DbConnect();
            con.Open();
            SqlTransaction tran = con.BeginTransaction();//使用事务
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Transaction = tran;

            using (PurProvider purProvider = new PurProvider())
            {
                try
                {
                    bool flag = false;
                    foreach (GridViewRow row in DetailList.Rows)
                    {
                        HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)row.FindControl("RowCheck");
                        if (rowCheckControl.Checked)
                        {
                            string prNo = row.Cells[1].Text.Trim();
                            string seq = row.Cells[2].Text.Trim();

                            cmd.CommandText = purProvider.ApprovePr(prNo, seq);
                            cmd.ExecuteNonQuery();

                            flag = true;
                        }
                    }

                    if (flag)
                    {
                        tran.Commit();
                        this.BindList();
                    }
                    else
                    {
                        this.ShowInfoMessage(this.GetGlobalResourceString("NotSelectMessage"));
                        tran.Rollback();
                        return;
                    }

                }
                catch (Exception error)
                {
                    tran.Rollback();
                    this.ShowErrorMessage("提交失败。" + error.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// 否决
        /// </summary>
        protected void RejectButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = LocalGlobal.DbConnect();
            con.Open();
            SqlTransaction tran = con.BeginTransaction();//使用事务
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Transaction = tran;

            using (PurProvider purProvider = new PurProvider())
            {
                try
                {
                    bool flag = false;
                    foreach (GridViewRow row in DetailList.Rows)
                    {
                        HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)row.FindControl("RowCheck");
                        if (rowCheckControl.Checked)
                        {
                            string prNo = row.Cells[1].Text.Trim();
                            string seq = row.Cells[2].Text.Trim();

                            cmd.CommandText = purProvider.RejectPr(prNo, seq);
                            cmd.ExecuteNonQuery();

                            flag = true;
                        }
                    }

                    if (flag)
                    {
                        tran.Commit();
                        this.BindList();
                    }
                    else
                    {
                        this.ShowInfoMessage(this.GetGlobalResourceString("NotSelectMessage"));
                        tran.Rollback();
                        return;
                    }

                }
                catch (Exception error)
                {
                    tran.Rollback();
                    this.ShowErrorMessage("提交失败。" + error.Message);
                    return;
                }
            }
        }

        #endregion

    }
}