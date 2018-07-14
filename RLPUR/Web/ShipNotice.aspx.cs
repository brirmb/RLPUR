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
    public partial class ShipNotice : LocalPage
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

            this.ORDNO.Text = string.Empty;
            this.RDate.Text = string.Empty;
            this.CustNo.Text = string.Empty;
            this.CustName.Text = string.Empty;
            this.ShipNo.Text = string.Empty;

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
                string orNo = ORDNO.Text.Trim();
                if (orNo.Length > 0)
                {
                    DataTable table = purProvider.GetShipNoticeList(orNo);
                    if (table != null && table.Rows.Count > 0)
                    {
                        CustNo.Text = table.Rows[0]["custno"].ToString();
                        CustName.Text = table.Rows[0]["rcnam"].ToString();
                        RDate.Text = table.Rows[0]["deliverydate"].ToString();
                    }

                    List.DataSource = table;
                    List.DataBind();
                }
                else
                {
                    Initialize();
                    List.DataSource = null;
                    List.DataBind();
                }
            }
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

                    string orNo = ((DataRowView)e.Row.DataItem)["ordno"].ToString().Trim();
                    string seq = ((DataRowView)e.Row.DataItem)["seq"].ToString().Trim();
                    string ordQty = ((DataRowView)e.Row.DataItem)["ordqty"].ToString().Trim(); //需求数量

                    using (PurProvider purProvider = new PurProvider())
                    {
                        int actualQty = purProvider.GetActualShipQty(orNo, seq); //实际出货数量

                        string noShipQty = (Util.ToInt(ordQty) - actualQty).ToString(); //未出货数量
                        e.Row.Cells[6].Text = noShipQty;
                    }

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

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            #region 检测

            if (ORDNO.Text.Trim().Length <= 0)
            {
                this.ShowWarningMessage("请输入工令号");
                return;
            }

            #endregion

            SqlConnection con = LocalGlobal.DbConnect();
            con.Open();
            SqlTransaction tran = con.BeginTransaction();//使用事务
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Transaction = tran;

            string shipNo = LocalGlobal.NewSTNo(); //出货单号

            using (PurProvider purProvider = new PurProvider())
            {
                try
                {
                    int seq = 0;
                    foreach (GridViewRow row in List.Rows)
                    {
                        HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)row.FindControl("RowCheck");
                        if (rowCheckControl.Checked)
                        {
                            seq++;

                            #region 检测

                            string noShipQty = row.Cells[6].ToString().Trim(); //未出货数量
                            string planQty = ((TextBox)row.FindControl("shipqplan")).Text.Trim(); //计划出货数量
                            string shipDate = ((TextBox)row.FindControl("shipdate")).Text.Trim(); //出货日期

                            if (shipDate.Length <= 0)
                            {
                                tran.Rollback();
                                this.ShowWarningMessage("请填写出货日期！");
                                return;
                            }
                            if (Util.ToInt(planQty) <= 0)
                            {
                                tran.Rollback();
                                this.ShowWarningMessage("出货数量必须大于0！");
                                return;
                            }
                            if (Util.ToInt(planQty) > Util.ToInt(noShipQty))
                            {
                                tran.Rollback();
                                this.ShowWarningMessage("出货数量不能大于未出货数量！");
                                return;
                            }

                            string orNo = ((DataRowView)row.DataItem)["ordno"].ToString().Trim();
                            var morder = purProvider.GetMOrder(orNo);
                            if (morder == null || morder.Rows.Count == 0)
                            {
                                var beiping = purProvider.GetBeiping(orNo);
                                if (beiping == null || beiping.Rows.Count == 0)
                                {
                                    tran.Rollback();
                                    this.ShowWarningMessage("任务的号码有误！");
                                    return;
                                }
                            }

                            #endregion

                            cmd.CommandText = purProvider.InsertShipSql(shipNo, seq.ToString(), orNo, row.Cells[2].Text.Trim(), CustNo.Text.Trim(), CustName.Text.Trim(), row.Cells[3].Text.Trim(), row.Cells[5].Text.Trim(), row.Cells[4].Text.Trim(), planQty, "0", shipDate, "0");
                            cmd.ExecuteNonQuery();
                        }
                    }

                    if (seq <= 0)
                    {
                        this.ShowInfoMessage(this.GetGlobalResourceString("NotSelectMessage"));
                        tran.Rollback();
                        return;
                    }

                }
                catch (Exception error)
                {
                    tran.Rollback();
                    this.ShowErrorMessage("保存失败。" + error.Message);
                    return;
                }

                tran.Commit();
                ShipNo.Text = shipNo;
                this.BindList();
            }
        }

        /// <summary>
        /// 转出货界面
        /// </summary>
        protected void TransferButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ShipMaintain.aspx?ShipNo=" + ShipNo.Text.Trim());
        }

        #endregion

    }
}