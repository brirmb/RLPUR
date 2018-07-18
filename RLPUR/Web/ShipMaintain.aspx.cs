using System;
using System.Collections.Generic;
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
    public partial class ShipMaintain : LocalPage
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

                //初始化
                this.Initialize();

                #region 获取参数

                string shipNo = Request.QueryString["ShipNo"];
                if (shipNo != null && shipNo.Trim().Length != 0)
                {
                    this.ShipNo.Text = shipNo;
                }

                #endregion

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
            this.ShipDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

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
                string shipNo = ShipNo.Text.Trim();
                if (shipNo.Length > 0)
                {
                    DataTable table = purProvider.GetShipList(shipNo);
                    if (table != null && table.Rows.Count > 0)
                    {
                        ORDNO.Text = table.Rows[0]["shipsono"].ToString();
                        CustNo.Text = table.Rows[0]["shipcustno"].ToString();
                        CustName.Text = table.Rows[0]["shipcustname"].ToString();
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

                    //计划发货数量
                    string planQty = ((DataRowView)e.Row.DataItem)["shipqplan"].ToString().Trim();
                    //实际发货数量
                    string actualQty = ((DataRowView)e.Row.DataItem)["shipqact"].ToString().Trim();

                    string noShipQty = (Util.ToInt(planQty) - Util.ToInt(actualQty)).ToString(); //未出货数量
                    e.Row.Cells[6].Text = noShipQty;
                    ((TextBox)e.Row.FindControl("shipqact")).Text = noShipQty;

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
            string shipNo = ShipNo.Text.Trim();
            string orNo = ORDNO.Text.Trim();

            #region 检测

            if (shipNo.Length <= 0)
            {
                this.ShowWarningMessage("请输入出货单号");
                return;
            }

            #endregion

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
                    //仓库
                    var whs = purProvider.GetBaseParam("WH", "1");
                    string whsCode = string.Empty;
                    if (whs != null && whs.Rows.Count > 0)
                    {
                        whsCode = whs.Rows[0]["description"].ToString().Trim();
                    }

                    bool flag = false;
                    foreach (GridViewRow row in List.Rows)
                    {
                        HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)row.FindControl("RowCheck");
                        if (rowCheckControl.Checked)
                        {
                            string seq = row.Cells[1].Text.Trim();
                            string drawNo = row.Cells[2].Text.Trim();
                            string itemName = row.Cells[3].Text.Trim();
                            string rQty = row.Cells[4].Text.Trim();
                            string um = row.Cells[5].Text.Trim();
                            string planQty = row.Cells[6].Text.Trim(); //计划出货数量
                            string actualQty = ((TextBox)row.FindControl("shipqact")).Text.Trim(); //出货数量

                            #region 检测

                            if (actualQty.Length <= 0)
                            {
                                tran.Rollback();
                                this.ShowWarningMessage("请填写出货数量！");
                                return;
                            }
                            if (Util.ToInt(actualQty) <= 0)
                            {
                                tran.Rollback();
                                this.ShowWarningMessage("出货数量必须大于0！");
                                return;
                            }
                            if (Util.ToInt(actualQty) > Util.ToInt(planQty))
                            {
                                tran.Rollback();
                                this.ShowWarningMessage("出货数量不能大于计划出货数量！");
                                return;
                            }

                            #endregion

                            cmd.CommandText = purProvider.UpdateShipQtySql(shipNo, seq, Util.ToInt(actualQty));
                            cmd.ExecuteNonQuery();

                            var invTable = purProvider.GetInventoryByItem(whsCode, drawNo);
                            if (invTable != null && invTable.Rows.Count > 0)
                            {
                                cmd.CommandText = purProvider.UpdateInventorySql(whsCode, drawNo, actualQty);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmd.CommandText = purProvider.InsertInventorySql(whsCode, drawNo, "", "", itemName, actualQty, um);
                                cmd.ExecuteNonQuery();
                            }

                            cmd.CommandText = purProvider.InsertTransDetailSql(orNo, drawNo, whsCode, actualQty, shipNo, orNo.Substring(0, 7), "B");
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = purProvider.UpdateShipStatusSql(shipNo, seq);
                            cmd.ExecuteNonQuery();

                            decimal zbAmt = 0, limit = 0; //质保金 质保期限
                            string lastDate = string.Empty; //质保金到期日
                            var conTable = purProvider.GetContractInfo(orNo, seq);
                            if (conTable != null && conTable.Rows.Count > 0)
                            {
                                decimal unitPrice = Util.ToDecimal(conTable.Rows[0]["unitprice"].ToString());
                                zbAmt = unitPrice * Util.ToDecimal(actualQty);

                                limit = Util.ToDecimal(conTable.Rows[0]["protectterm"].ToString());
                                int diff = Util.ToInt(limit * 12);
                                lastDate = LocalGlobal.ConvertDateFormat(ShipDate.Text.Trim()).AddMonths(diff).ToString("yyyy-MM-dd");

                            }

                            cmd.CommandText = purProvider.InsertZhibaojinSql(shipNo, orNo, seq, itemName, drawNo, CustNo.Text.Trim(), CustName.Text.Trim(), actualQty, zbAmt.ToString(), "0", ShipDate.Text.Trim(), DateTime.Now.ToString("HH:mm:ss"), limit.ToString(), lastDate);
                            cmd.ExecuteNonQuery();

                            flag = true;
                        }
                    }

                    if (!flag)
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

                this.ShowInfoMessage("出货成功");
                this.Initialize();
                this.BindList();
            }
        }

        #endregion
    }
}