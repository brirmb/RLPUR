﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Plusii.iiWeb;

namespace RLPUR.Framework
{
    /// <summary>
    /// 页眉
    /// </summary>
    public partial class Header : iiPage
    {
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            this.Title = this.PageID;

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

            SystemID.Text = iiGlobal.SystemID;
            SystemVersion.Text = iiGlobal.SystemVersion;

            UserName.Text = iiGlobal.CurrentUser;
            LogoutButton.NavigateUrl = string.Format("{0}/", iiGlobal.VirtualPath);

            #endregion
        }

        #endregion
    }
}