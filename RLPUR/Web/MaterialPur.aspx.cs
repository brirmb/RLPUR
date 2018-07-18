using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using RLPUR.Common;
using RLPUR.Models;
using System.Data.SqlClient;

namespace RLPUR.Web
{
    public partial class MaterialPur : LocalPage
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

            this.PRNo.Text = string.Empty;
            this.PRStatus.Text = string.Empty;

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
                if (prNo.Length > 0)
                {
                    PRStatus.Text = LocalGlobal.GetPRStatus(prNo);

                    DataTable table = purProvider.GetMatPRDetailList(prNo);
                    ViewState["ViewDT"] = table;
                    BindTempData();
                }
                else
                {
                    this.Initialize();
                    List.DataSource = null;
                    List.DataBind();
                }

            }
        }

        /// <summary>
        /// 行绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                //数据行
                case DataControlRowType.DataRow:
                    #region 数据绑定

                    #endregion
                    break;
                case DataControlRowType.Footer:
                    break;
                case DataControlRowType.EmptyDataRow:
                case DataControlRowType.Header:
                case DataControlRowType.Separator:
                case DataControlRowType.Pager:
                default:
                    break;
            }
        }

        protected void List_PreRender(object sender, EventArgs e)
        {
            if (this.List.Rows.Count == 0) //无数据时需特殊处理
            {
                BindEmptyList();
            }
        }

        protected void List_Load(object sender, EventArgs e)
        {
        }

        #endregion

        #region 操作

        /// <summary>
        /// 新增行，保存到临时datatable中
        /// </summary>
        protected void CreateRow_Click(object sender, EventArgs e)
        {
            var table = ViewState["ViewDT"] as DataTable;
            var newRow = table.NewRow();
            newRow["prltno"] = ((TextBox)this.List.FooterRow.FindControl("prltno")).Text.Trim();
            newRow["prloutno"] = ((TextBox)this.List.FooterRow.FindControl("prloutno")).Text.Trim();
            newRow["prlpicno"] = ((TextBox)this.List.FooterRow.FindControl("prlpicno")).Text.Trim();
            newRow["prlrule"] = ((TextBox)this.List.FooterRow.FindControl("prlrule")).Text.Trim();
            newRow["prlum"] = ((TextBox)this.List.FooterRow.FindControl("prlum")).Text.Trim();
            newRow["PRLPDTE"] = ((TextBox)this.List.FooterRow.FindControl("PRLPDTE")).Text.Trim();
            newRow["PRLQTY"] = ((TextBox)this.List.FooterRow.FindControl("PRLQTY")).Text.Trim();
            newRow["prlstation"] = ((TextBox)this.List.FooterRow.FindControl("prlstation")).Text.Trim();
            newRow["prlmrk"] = ((TextBox)this.List.FooterRow.FindControl("prlmrk")).Text.Trim();

            //保存临时数据后重新绑定gridview
            table.Rows.Add(newRow);
            ViewState["ViewDT"] = table;
            BindTempData();
        }

        /// <summary>
        /// 删除临时datatable中的行
        /// </summary>
        protected void DeleteRow_Click(object sender, EventArgs e)
        {
            DataTable table = ViewState["ViewDT"] as DataTable;

            bool deleted = false;
            foreach (GridViewRow row in List.Rows)
            {
                HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)row.FindControl("RowCheck");
                if (rowCheckControl.Checked)
                {
                    var itemNo = ((DataBoundLiteralControl)row.Cells[2].Controls[0]).Text.Trim(); //料号
                    foreach (DataRow dr in table.Rows)
                    {
                        if (dr["prltno"].ToString().Trim() == itemNo)
                        {
                            table.Rows.Remove(dr);
                            break;
                        }
                    }
                    //有项被删除
                    deleted = true;
                }
            }
            if (deleted)
            {
                ViewState["ViewDT"] = table;
                BindTempData();
            }
            else
            {
                this.ShowInfoMessage(this.GetGlobalResourceString("NotSelectMessage"));
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        protected void OKButton_Click(object sender, EventArgs e)
        {
            BindList();
        }

        /// <summary>
        /// 删除
        /// </summary>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            using (PurProvider purProvider = new PurProvider())
            {
                string prNo = PRNo.Text.Trim();
                if (prNo.Length <= 0)
                {
                    this.ShowWarningMessage("请输入请购单号");
                    return;
                }

                try
                {
                    purProvider.DeleteMatPR(prNo);

                    //this.ShowMessage("删除成功");
                    this.BindList();
                }
                catch (Exception error)
                {
                    this.ShowErrorMessage("删除失败。" + error.Message);
                    return;
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            var table = ViewState["ViewDT"] as DataTable;
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }

            SqlConnection con = LocalGlobal.DbConnect();
            con.Open();
            SqlTransaction tran = con.BeginTransaction();//使用事务
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.Transaction = tran;

            var dateModel = LocalGlobal.GetDateModel();
            string prNo = string.Empty;

            using (PurProvider purProvider = new PurProvider())
            {
                try
                {
                    if (PRNo.Text.Trim().Length == 0) //新增
                    {
                        prNo = purProvider.GetMaxPRNo().ToString();  //生成新的请购单号

                        cmd.CommandText = purProvider.InsertPRSql(prNo, "", "", "S", "RMB", "材料请购", LocalGlobal.CurrentUserID, dateModel.DateStr, "", "0");
                        cmd.ExecuteNonQuery();

                        PRStatus.Text = "NE";
                    }
                    else
                    {
                        //修改请购单
                        prNo = PRNo.Text.Trim();

                        //先删除再添加
                        cmd.CommandText = purProvider.DeletePRDetailSql(prNo);
                        cmd.ExecuteNonQuery();

                        //更新状态
                        cmd.CommandText = purProvider.UpdatePRStatusSql(prNo, "UP");
                        cmd.ExecuteNonQuery();

                        PRStatus.Text = "UP";
                    }

                    #region Insert

                    int seq = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        seq++;

                        string prDate = row["PRLPDTE"].ToString().Trim();
                        prDate = LocalGlobal.ConvertDateFormat(prDate).ToString("yyyyMMdd");

                        cmd.CommandText = purProvider.InsertMatPRDetailSql(prNo, seq.ToString(), row["PRLQTY"].ToString().Trim(), prDate, prlwhs.Text.Trim(), row["prltno"].ToString(), row["prlstation"].ToString(), row["prlrule"].ToString(), row["prlum"].ToString(), LocalGlobal.CurrentUserID, dateModel.DateStr, dateModel.TimeStr, row["prlmrk"].ToString(), row["prloutno"].ToString(), row["prlpicno"].ToString());

                        cmd.ExecuteNonQuery();
                    }

                    #endregion

                }
                catch (Exception error)
                {
                    tran.Rollback();
                    this.ShowErrorMessage("保存失败。" + error.Message);
                    return;
                }

                tran.Commit();

                PRNo.Text = prNo;
                this.BindList();
            }
        }

        #endregion

        #region 创建表结构及绑定

        protected DataTable CreateTable()
        {
            //定义table结构   
            DataTable dt1 = new DataTable();
            //不设置 默认为System.String  

            dt1.Columns.Add("PRLSEQ");
            dt1.Columns.Add("prltno");
            dt1.Columns.Add("prloutno");
            dt1.Columns.Add("prlpicno");
            dt1.Columns.Add("prlrule");
            dt1.Columns.Add("prlum");
            dt1.Columns.Add("PRLPDTE");
            dt1.Columns.Add("PRLQTY");
            dt1.Columns.Add("prlstation");
            dt1.Columns.Add("prlmrk");

            //dt1.Rows.Add(dt1.NewRow());
            //ViewState["ViewDT"] = dt1;
            return dt1;
        }

        private void BindEmptyList()
        {
            var table = CreateTable();
            //表格无数据时添加一空行，否则footer不显示
            //空行不需要显示，所以隐藏掉
            table.Rows.Add(table.NewRow());
            List.DataSource = table;
            List.DataBind();
            List.Rows[0].Visible = false;

            ViewState["ViewDT"] = CreateTable();
        }

        protected void BindTempData()
        {
            this.List.DataSource = ViewState["ViewDT"] as DataTable;
            this.List.DataBind();
        }

        #endregion

    }
}