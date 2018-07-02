using RLPUR.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace RLPUR.Models
{
    public class PurProvider : LocalDbProvider
    {
        #region

        /// <summary>
        /// 查询合同概况
        /// </summary>
        public DataTable GetContractHeadInfo(string no)
        {
            string sql = string.Format(" SELECT ORDNO,ORDNAME,CUSTNO,CUSTNAME,CURR,SIGNDATE, DELIVERYDATE,PROTECTTERM,SCH_YF,SCH_JD,SCH_TH,SCH_ZB,ORDAMT,REMARK  FROM contract WHERE OHID='Y' AND ORDNO = N'{0}'", no);

            return this.Query(sql);
        }

        /// <summary>
        /// 新增合同sql
        /// </summary>
        public string InsertContractSql(string orNo, string orName, string custNo, string custName, string curr, string signDate, string deliverDate, string term, string yf, string jd, string th, string zb, string orAmt, string remark, string sysUsername, string strDate, string strTime)
        {
            string sql = string.Format("INSERT INTO CONTRACT (ohid,ordno,ordname,custno,custname,curr,SignDate,DeliveryDate,ProtectTerm,sch_yf,sch_JD,sch_TH,sch_ZB,ordamt,Remark,createuser,createdate,createtime,lastuser,lastudate,lastutime) VALUES('Y',N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}',N'{9}',N'{10}',N'{11}',N'{12}',N'{13}',N'{14}',N'{15}',N'{16}',N'{17}',N'{18}',N'{19}') ",
                orNo, orName, custNo, custName, curr, signDate, deliverDate, term, yf, jd, th, zb, orAmt, remark, sysUsername, strDate, strTime, sysUsername, strDate, strTime
                );
            return sql;
        }

        #endregion

    }
}