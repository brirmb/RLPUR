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
                DataTable table = purProvider.GetPRDetailMat(prNo);

                List.DataSource = table;
                List.DataBind();
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
            //#region 检测
            //var itemName = ((TextBox)this.List.FooterRow.FindControl("ITEMNO")).Text.Trim();
            //#endregion

            var table = ViewState["ViewDT"] as DataTable;
            var newRow = table.NewRow();
            newRow["ITEMNO"] = ((TextBox)this.List.FooterRow.FindControl("ITEMNO")).Text.Trim();
            newRow["DRAWNO"] = ((TextBox)this.List.FooterRow.FindControl("DRAWNO")).Text.Trim();
            newRow["ORDQTY"] = ((TextBox)this.List.FooterRow.FindControl("ORDQTY")).Text.Trim();
            newRow["UM"] = ((DropDownList)this.List.FooterRow.FindControl("UM")).SelectedValue.Trim();
            newRow["UNITPRICE"] = ((TextBox)this.List.FooterRow.FindControl("UNITPRICE")).Text.Trim();
            newRow["AMT"] = ((TextBox)this.List.FooterRow.FindControl("AMT")).Text.Trim();
            newRow["Remark"] = ((TextBox)this.List.FooterRow.FindControl("Remark")).Text.Trim();

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
                    var itemName = ((DataBoundLiteralControl)row.Cells[2].Controls[0]).Text.Trim(); //名称
                    foreach (DataRow dr in table.Rows)
                    {
                        if (dr["ITEMNO"].ToString().Trim() == itemName)
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
            bool deleted = false;
            foreach (GridViewRow row in List.Rows)
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