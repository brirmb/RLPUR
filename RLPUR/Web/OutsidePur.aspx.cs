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
    public partial class OutsidePur : LocalPage
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

            using (PurProvider purProvider = new PurProvider())
            {
                var bomType = purProvider.GetBaseParam("PT", "");
                LocalGlobal.BindListItems(BomType, bomType.DefaultView, "Description", "code", true);
            }
            this.ORDNO.Text = string.Empty;
            this.DRAWNO.Text = string.Empty;
            this.PRNo.Text = string.Empty;
            this.PRStatus.Text = string.Empty;
            //this.BomType.SelectedIndex = -1;

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
                if (PRNo.Text.Trim().Length <= 0)
                {
                    DataTable bomTable = purProvider.GetBOMList(ORDNO.Text.Trim(), this.BomType.SelectedValue.Trim());
                    List.DataSource = bomTable;
                    List.DataBind();
                    if (bomTable != null && bomTable.Rows.Count > 0)
                    {
                        DRAWNO.Text = bomTable.Rows[0]["bommno"].ToString();
                    }

                    ViewState["ViewDT"] = this.CreateTable();
                    BindTempData();
                }
                else
                {
                    List.DataSource = null;
                    List.DataBind();

                    DataTable prTable = purProvider.GetPRDetailList(PRNo.Text.Trim());
                    ViewState["ViewDT"] = prTable;
                    BindTempData();
                    //PRList.DataSource = prTable;
                    //PRList.DataBind();
                    if (prTable != null && prTable.Rows.Count > 0)
                    {
                        ORDNO.Text = prTable.Rows[0]["PRHSORD"].ToString();
                        DRAWNO.Text = prTable.Rows[0]["prhmno"].ToString();
                        PRStatus.Text = prTable.Rows[0]["PRHSTAT"].ToString();

                        //默认全部勾选
                        for (int i = 0; i < PRList.Rows.Count; i++)
                        {
                            HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)PRList.Rows[i].FindControl("RowCheck");
                            rowCheckControl.Checked = true;
                        }
                    }
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

                    ////数据
                    //string rowStatus = ((DataRowView)e.Row.DataItem)["Status"].ToString().Trim();

                    ////控件
                    //HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)e.Row.FindControl("RowCheck");

                    ////指派
                    //rowCheckControl.Disabled = (rowStatus == "S");

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
        /// 行绑定
        /// </summary>
        protected void PRList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                //数据行
                case DataControlRowType.DataRow:
                    #region 数据绑定

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

        /// <summary>
        /// 新增PRList临时行
        /// </summary>
        protected void CreateButton_Click(object sender, EventArgs e)
        {
            var table = ViewState["ViewDT"] as DataTable;

            foreach (GridViewRow row in List.Rows)
            {
                HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)row.FindControl("RowCheck");
                if (rowCheckControl.Checked)
                {
                    var newRow = table.NewRow();
                    newRow["prlnedm"] = "N";
                    newRow["prltno"] = row.Cells[2].Text.Trim();
                    newRow["bomnam"] = row.Cells[3].Text.Trim();
                    newRow["bommat"] = row.Cells[4].Text.Trim();
                    newRow["prlrule"] = row.Cells[5].Text.Trim();
                    newRow["prlsoseq"] = row.Cells[1].Text.Trim();
                    newRow["bomreq"] = row.Cells[6].Text.Trim();
                    newRow["prlum"] = row.Cells[7].Text.Trim();
                    newRow["PRLQTY"] = Util.ToInt(row.Cells[6].Text.Trim()) - LocalGlobal.GetBomQty(ORDNO.Text.Trim(), row.Cells[1].Text.Trim(), "F");

                    //保存临时数据后重新绑定gridview
                    table.Rows.Add(newRow);
                }
            }

            ViewState["ViewDT"] = table;
            BindTempData();
        }

        /// <summary>
        /// 删除PRList临时行
        /// </summary>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            DataTable table = ViewState["ViewDT"] as DataTable;

            bool deleted = false;
            foreach (GridViewRow row in PRList.Rows)
            {
                HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)row.FindControl("RowCheck");
                if (rowCheckControl.Checked)
                {
                    var prltno = row.Cells[3].Text.Trim(); //工件号
                    var prlsoseq = row.Cells[7].Text.Trim(); //工令序号
                    foreach (DataRow dr in table.Rows)
                    {
                        if (dr["prltno"].ToString().Trim() == prltno && dr["prlsoseq"].ToString().Trim() == prlsoseq)
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
        /// 保存
        /// </summary>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            #region 检测

            int count = 0;
            foreach (GridViewRow row in PRList.Rows)
            {
                HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)row.FindControl("RowCheck");
                if (rowCheckControl.Checked)
                {
                    count++;

                    string prDate = ((TextBox)row.FindControl("PRLPDTE")).Text.Trim();
                    if (LocalGlobal.ConvertDateFormat(prDate) <= DateTime.Today)
                    {
                        this.ShowInfoMessage("日期须在明天以后!");
                        return;
                    }
                }
            }

            if (count <= 0)
            {
                this.ShowInfoMessage(this.GetGlobalResourceString("NotSelectMessage"));
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
            string prNo = string.Empty;

            using (PurProvider purProvider = new PurProvider())
            {
                try
                {
                    if (PRNo.Text.Trim().Length == 0) //新增
                    {
                        prNo = purProvider.GetMaxPRNo().ToString();  //生成新的请购单号
                        if (DRAWNO.Text.Trim().Length == 0)
                        {
                            DRAWNO.Text = " ";
                        }
                    }
                    else
                    {
                        //修改请购单
                        prNo = PRNo.Text.Trim();

                        //先删除再添加
                        cmd.CommandText = purProvider.DeletePRSql(prNo);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = purProvider.DeletePRDetailSql(prNo);
                        cmd.ExecuteNonQuery();
                    }

                    #region Insert

                    cmd.CommandText = purProvider.InsertPRSql(prNo, ORDNO.Text.Trim(), DRAWNO.Text, "F", " ", "委外请购", LocalGlobal.CurrentUserID, dateModel.DateStr, LocalGlobal.CurrentUserID, dateModel.DateStr);
                    cmd.ExecuteNonQuery();

                    int seq = 0;
                    foreach (GridViewRow row in PRList.Rows)
                    {
                        HtmlInputCheckBox rowCheckControl = (HtmlInputCheckBox)row.FindControl("RowCheck");
                        HtmlInputCheckBox urgentCheckControl = (HtmlInputCheckBox)row.FindControl("UrgentCheck");

                        if (rowCheckControl.Checked)
                        {
                            seq++;
                            string isUrgent = urgentCheckControl.Checked ? "Y" : "N";
                            string prlstation = ((TextBox)row.FindControl("prlstation")).Text.Trim();
                            string prQty = ((TextBox)row.FindControl("PRLQTY")).Text.Trim();
                            string prDate = ((TextBox)row.FindControl("PRLPDTE")).Text.Trim();
                            prDate = LocalGlobal.ConvertDateFormat(prDate).ToString("yyyyMMdd");

                            cmd.CommandText = purProvider.InsertPRDetailSql(prNo, seq.ToString(), " ", prQty, prDate, prDate, "GT", ORDNO.Text.Trim(), "", row.Cells[3].Text.Trim(), prlstation, row.Cells[6].Text.Trim(), isUrgent, row.Cells[9].Text.Trim(), LocalGlobal.CurrentUserID, dateModel.DateStr, dateModel.TimeStr, "委外请购", row.Cells[7].Text.Trim());
                            cmd.ExecuteNonQuery();
                        }
                    }

                    #endregion

                    if (PRNo.Text.Trim().Length > 0)
                    {
                        //更新状态
                        cmd.CommandText = purProvider.UpdatePRStatusSql(prNo, "UP");
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception error)
                {
                    tran.Rollback();
                    this.ShowErrorMessage("保存失败。" + error.Message);
                    return;
                }

                tran.Commit();

                PRNo.Text = prNo;
                PRStatus.Text = LocalGlobal.GetPRStatus(prNo);
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
            dt1.Columns.Add("pRHSORD");
            dt1.Columns.Add("prlnedm");
            dt1.Columns.Add("prlsoseq");
            dt1.Columns.Add("prhmno");
            dt1.Columns.Add("prlmno");
            dt1.Columns.Add("prltno");
            dt1.Columns.Add("PRHSTAT");
            dt1.Columns.Add("PRLSEQ");
            dt1.Columns.Add("PRLPROD");
            dt1.Columns.Add("PRLPDTE");
            dt1.Columns.Add("PRLQTY");
            dt1.Columns.Add("prlrule");
            dt1.Columns.Add("prlum");
            dt1.Columns.Add("prlstation");
            dt1.Columns.Add("bommat");
            dt1.Columns.Add("bomnam");
            dt1.Columns.Add("bomseq");
            dt1.Columns.Add("bomreq");
            //dt1.Rows.Add(dt1.NewRow());
            //ViewState["ViewDT"] = dt1;
            return dt1;
        }

        protected void BindTempData()
        {
            this.PRList.DataSource = ViewState["ViewDT"] as DataTable;
            this.PRList.DataBind();
        }

        #endregion
    }
}