using RLPUR.Common;
using RLPUR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RLPUR.Web
{
    public partial class PurPrint : LocalPage
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
            VenNo.Text = string.Empty;
            PrType.SelectedIndex = 0;

            #endregion

        }

        #endregion

        #region 操作

        /// <summary>
        /// 打印
        /// </summary>
        protected void OKButton_Click(object sender, EventArgs e)
        {
            using (PurProvider purProvider = new PurProvider())
            {
                string prType = PrType.SelectedValue;

                //请购数据源
                DataTable table = purProvider.GetPROrderPrint(prType, PRNoFrom.Text.Trim(), PRNoTo.Text.Trim(), VenNo.Text.Trim());




            }


        }

        /// <summary>
        /// 取消
        /// </summary>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            this.Initialize();
        }

        #endregion

    }
}