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

        }

        protected void PostButton_Click(object sender, EventArgs e)
        {

        }

        protected void PrintButton_Click(object sender, EventArgs e)
        {

        }


        #endregion


    }
}