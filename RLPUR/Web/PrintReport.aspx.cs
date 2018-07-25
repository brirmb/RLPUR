using Microsoft.Reporting.WebForms;
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
    public partial class PrintReport : LocalDialog
    {
        #region 属性
        private string CurrentID
        {
            get
            {
                object tempObject = ViewState["CurrentID"];
                return (tempObject != null) ? (string)tempObject : string.Empty;
            }
            set
            {
                ViewState["CurrentID"] = value;
            }
        }

        private string CurrentSource
        {
            get
            {
                object tempObject = ViewState["CurrentSource"];
                return (tempObject != null) ? (string)tempObject : string.Empty;
            }
            set
            {
                ViewState["CurrentSource"] = value;
            }
        }

        private string PrType
        {
            get
            {
                object tempObject = ViewState["PrType"];
                return (tempObject != null) ? (string)tempObject : string.Empty;
            }
            set
            {
                ViewState["PrType"] = value;
            }
        }
        private string VenNo
        {
            get
            {
                object tempObject = ViewState["VenNo"];
                return (tempObject != null) ? (string)tempObject : string.Empty;
            }
            set
            {
                ViewState["VenNo"] = value;
            }
        }
        private string PrNoFrom
        {
            get
            {
                object tempObject = ViewState["PrNoFrom"];
                return (tempObject != null) ? (string)tempObject : string.Empty;
            }
            set
            {
                ViewState["PrNoFrom"] = value;
            }
        }
        private string PrNoTo
        {
            get
            {
                object tempObject = ViewState["PrNoTo"];
                return (tempObject != null) ? (string)tempObject : string.Empty;
            }
            set
            {
                ViewState["PrNoTo"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 获取参数
                string id = Request.QueryString["ID"];
                if (id != null && id.Trim().Length != 0)
                {
                    this.CurrentID = id.Trim();
                }

                string source = Request.QueryString["Source"];
                if (source != null && source.Trim().Length != 0)
                {
                    this.CurrentSource = source.Trim();
                }

                string prType = Request.QueryString["prType"];
                if (prType != null && prType.Trim().Length != 0)
                {
                    this.PrType = prType.Trim();
                }
                string venNo = Request.QueryString["venNo"];
                if (venNo != null && venNo.Trim().Length != 0)
                {
                    this.VenNo = venNo.Trim();
                }
                string prNoFrom = Request.QueryString["prNoFrom"];
                if (prNoFrom != null && prNoFrom.Trim().Length != 0)
                {
                    this.PrNoFrom = prNoFrom.Trim();
                }
                string prNoTo = Request.QueryString["prNoTo"];
                if (prNoTo != null && prNoTo.Trim().Length != 0)
                {
                    this.PrNoTo = prNoTo.Trim();
                }

                #endregion

                //初始化
                this.Initialize();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            string prNo = this.CurrentID;

            DataTable table = new DataTable();
            using (PurProvider purProvider = new PurProvider())
            {
                if (CurrentSource == "maintain")
                {
                    var pur = purProvider.GetPRDetail(prNo);
                    if (pur == null)
                    {
                        //this.ShowWarningMessage("请购单号码有误！");
                        //return;
                        Response.Write("请购单号码有误！");
                        Response.End();
                        return;
                    }

                    string prType = pur["prhtyp"].ToString().Trim();
                    if (string.IsNullOrWhiteSpace(prType))
                    {
                        Response.Write("请购单号码有误！");
                        Response.End();
                        return;
                    }

                    this.PrType = prType;
                    //请购数据源
                    table = purProvider.GetPRMainPrint(this.PrType, prNo);

                }
                else if (CurrentSource == "print")
                {
                    table = purProvider.GetPROrderPrint(this.PrType, this.PrNoFrom, this.PrNoTo, this.VenNo);
                }
            }

            string report = "RLPUR.Web.Pur" + this.PrType + ".rdlc";
            Viewer.LocalReport.ReportEmbeddedResource = report;
            Viewer.LocalReport.DataSources.Clear();
            Viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", table));
        }
    }
}