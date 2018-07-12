using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RLPUR.Common;
using RLPUR.Models;

namespace RLPUR.Web
{
    public partial class PurMaintain : LocalPage
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
            this.DRAWNO.Text = string.Empty;
            this.PRNo.Text = string.Empty;
            this.PRStatus.Text = string.Empty;
            this.PRType.Text = string.Empty;
            this.IsWeight.Checked = false;

            this.PostButton.Enabled = false;

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
                string prNo = PRNo.Text.Trim();

                var pur = purProvider.GetPRDetail(prNo);
                if (pur != null)
                {
                    ORDNO.Text = pur["PRHSORD"].ToString();
                    DRAWNO.Text = pur["PRHMNO"].ToString();
                    PRType.Text = pur["prhpgm"].ToString();
                    PRStatus.Text = pur["PRHSTAT"].ToString();

                    DataTable table;
                    if (ORDNO.Text.Trim().Length > 0) //非材料请购
                    {
                        table = purProvider.GetPRDetailNotMat(prNo);
                    }
                    else  //材料请购
                    {
                        table = purProvider.GetPRDetailMat(prNo);
                    }

                    List.DataSource = table;
                    List.DataBind();

                    PostButton.Enabled = true;
                }
                else
                {
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

                    //状态
                    string price = ((DataRowView)e.Row.DataItem)["prlpacst"].ToString().Trim();
                    string prlvnd = ((DataRowView)e.Row.DataItem)["prlvnd"].ToString().Trim();
                    string prlvndm = ((DataRowView)e.Row.DataItem)["prlvndm"].ToString().Trim();

                    e.Row.Cells[14].Text = (Util.ToDecimal(price) > 0 && Util.ToInt(prlvnd) > 0 && prlvndm.Length > 0) ? "OK" : "";

                    //是否急件
                    string isUrgent = ((DataRowView)e.Row.DataItem)["prlnedm"].ToString().Trim();
                    //控件
                    HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)e.Row.FindControl("UrgentCheck");
                    //指派
                    rowCheckControl.Checked = (isUrgent == "Y");

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
        /// 取消
        /// </summary>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            this.Initialize();
            this.BindList();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            #region 检测

            if (PRNo.Text.Trim().Length <= 0)
            {
                this.ShowWarningMessage("请购单号不存在");
                return;
            }

            #endregion

            SqlConnection con = LocalGlobal.DbConnect();
            con.Open();
            SqlTransaction tran = con.BeginTransaction();//使用事务
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Transaction = tran;

            var dateModel = LocalGlobal.GetDateModel();

            using (PurProvider purProvider = new PurProvider())
            {
                try
                {
                    bool flag = false;
                    foreach (GridViewRow row in List.Rows)
                    {
                        HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)row.FindControl("RowCheck");
                        if (rowCheckControl.Checked)
                        {
                            string seq = row.Cells[2].ToString().Trim();
                            string price = ((TextBox)row.FindControl("prlpacst")).Text.Trim();
                            string vendorNo = ((TextBox)row.FindControl("prlvnd")).Text.Trim();
                            string vendorName = ((TextBox)row.FindControl("prlvndm")).Text.Trim();
                            string curr = ((TextBox)row.FindControl("prlcur")).Text.Trim();
                            string isWeight = IsWeight.Checked ? "Y" : "";

                            cmd.CommandText = purProvider.UpdatePRDetailSql(PRNo.Text.Trim(), seq.ToString(), price, vendorNo, vendorName, curr, isWeight, dateModel.DateStr);
                            cmd.ExecuteNonQuery();

                            flag = true;
                        }
                    }

                    if (flag)
                    {
                        //更新状态
                        cmd.CommandText = purProvider.UpdatePRStatusSql(PRNo.Text.Trim(), "UP");
                        cmd.ExecuteNonQuery();
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
                    this.ShowErrorMessage("保存失败。" + error.Message);
                    return;
                }

                tran.Commit();

                this.BindList();
                this.PostButton.Enabled = true;
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        protected void PostButton_Click(object sender, EventArgs e)
        {
            #region 检测

            if (PRNo.Text.Trim().Length <= 0)
            {
                this.ShowWarningMessage("请购单号不存在");
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
                    bool flag = false;
                    foreach (GridViewRow row in List.Rows)
                    {
                        HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)row.FindControl("RowCheck");
                        if (rowCheckControl.Checked)
                        {
                            string seq = row.Cells[2].ToString().Trim();
                            string status = row.Cells[14].Text.Trim();

                            if (status == "OK")
                            {
                                cmd.CommandText = purProvider.PostPRDetailSql(PRNo.Text.Trim(), seq.ToString());
                                cmd.ExecuteNonQuery();

                                flag = true;
                            }
                        }
                    }

                    if (flag)
                    {
                        //更新状态
                        cmd.CommandText = purProvider.UpdatePRStatusSql(PRNo.Text.Trim(), "PS");
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        this.ShowInfoMessage("未选中要提交的记录，请确认请购状态，或未填写价格/厂商！");
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

                tran.Commit();

                this.BindList();
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        protected void PrintButton_Click(object sender, EventArgs e)
        {
            #region 检测

            if (PRNo.Text.Trim().Length <= 0)
            {
                this.ShowWarningMessage("请输入请购单号！");
                return;
            }

            #endregion

            using (PurProvider purProvider = new PurProvider())
            {
                string prNo = PRNo.Text.Trim();
                var pur = purProvider.GetPRDetail(prNo);
                if (pur == null)
                {
                    this.ShowWarningMessage("请购单号码有误！");
                    return;
                }

                DataTable table = new DataTable();
                //一般请购、委外请购数据源
                table = purProvider.GetPRPrintNotMat(prNo);


                //材料请购数据源
                table = purProvider.GetPRPrintMat(prNo);

            }

        }


        #endregion


    }
}