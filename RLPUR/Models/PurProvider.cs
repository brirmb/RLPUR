using RLPUR.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace RLPUR.Models
{
    public class PurProvider : LocalDbProvider
    {
        #region 请购

        /// <summary>
        /// 工令BOM明细查询
        /// </summary>
        public DataTable GetBOMList(string orNo, string bomType)
        {
            string sql = string.Format(" select BomItm,bomseq,bomwno,bommno,bompno,bommat,bomreq,bomser,BOMRDT,bomnam,bomspc,bomuit from tsfcbom where bomflg='Y' and bompry='N' and bomreq-bomisu>0 and BOMWNO =N'{0}'", orNo);

            if (!string.IsNullOrWhiteSpace(bomType))
            {
                sql += string.Format(" and bomtype=N'{0}' ", bomType);
            }

            sql += " order by bompno,bomitm ";
            return this.Query(sql);
        }

        /// <summary>
        /// 获取请购明细列表
        /// </summary>
        public DataTable GetPRDetailList(string prNo)
        {
            string sql = string.Format("select pRHSORD,prlnedm,prlsoseq,prhmno,prlmno,prltno,PRHSTAT,PRLSEQ,PRLPROD,PRLPDTE,PRLQTY,prlrule,prlum,prlstation,bommat,bomnam,bomseq,bomreq from purprh,purprl,tsfcbom where prlid='RL' and prhstat in('NE','UP','PS') and PRLNO=PRHNO and prlsord=bomwno and prlsoseq=bomseq and PRLNO =N'{0}' order by prlseq ", prNo);

            return this.Query(sql);
        }

        /// <summary>
        /// 获取请购维护列表-材料请购
        /// </summary>
        public DataTable GetPRDetailMat(string prNo)
        {
            string sql = string.Format("select *,prloutno bomnam,prlpicno bommat,'' prlstation from purprl where prlno=N'{0}' order by prlseq ", prNo);

            return this.Query(sql);
        }

        /// <summary>
        /// 获取请购维护列表-非材料请购
        /// </summary>
        public DataTable GetPRDetailNotMat(string prNo)
        {
            string sql = string.Format("select PURPRL.*,bommat,bomspc,bomqua,bomnam from PURPRL,tsfcbom where bomflg='Y' and bomwno=prlsord and bomseq=prlsoseq and PRLID='RL' and PRLNO=N'{0}' and PRLAPR<>'Y' and prlpmt<>'Y' and PRLPONO=0 order by prlno,prlseq ", prNo);

            return this.Query(sql);
        }

        /// <summary>
        /// 获取请购维护打印信息-非材料请购
        /// </summary>
        public DataTable GetPRPrintNotMat(string prNo)
        {
            string sql = string.Format("select prhno,prhsord,prlseq,prlqty,prlpacst,prlpdte,prlmno,prlum,prlstation,bommat,bomnam from purprh,purprl,tsfcbom where prhno=prlno and prlsord=bomwno and prlsoseq=bomseq and prhno= N'{0}' and prhid='RH' and prlid='RL' and prlpmt='Y' ", prNo);

            return this.Query(sql);
        }

        /// <summary>
        /// 获取请购维护打印信息-材料请购
        /// </summary>
        public DataTable GetPRPrintMat(string prNo)
        {
            string sql = string.Format("select prlno,prlseq,prlqty,prlpacst,prlpdte,prlvndm,prltno,prlum,prlmrk,prloutno,prlpicno ,prhno from purprl,purprh where prhid='RH' and prlno=prhno and prhtyp='S' and prlno= N'{0}' ", prNo);

            return this.Query(sql);
        }

        /// <summary>
        /// 获取请购单号 prType:N一般请购,F委外请购
        /// </summary>
        public DataTable GetPRNoList(string prType)
        {
            string sql = string.Format("select distinct PRLNO from PURPRL,purprh where prhid='RH' and prhno=prlno and prhtyp=N'{0}' and prhstat in('NE','UP','PS') and PRLID='RL' and PRLPONO=0 order by PRLNO ", prType);

            return this.Query(sql);
        }

        /// <summary>
        /// 获取请购明细中的供应商
        /// </summary>
        public DataTable GetPRVendorList(string prNo)
        {
            string sql = string.Format("select distinct prlvndm from purprl where prlno=N'{0}' ", prNo);

            return this.Query(sql);
        }

        /// <summary>
        /// 获取请购单状态
        /// </summary>
        public string GetPRStatus(string prNo)
        {
            var row = GetPRDetail(prNo);
            if (row == null)
            {
                return string.Empty;
            }
            else
            {
                return row["PRHSTAT"].ToString();
            }
        }

        /// <summary>
        /// 查询请购明细（状态等）
        /// </summary>
        public DataRow GetPRDetail(string prNo)
        {
            string sql = string.Format(" select * from PURPRH where PRHNO =N'{0}' ", prNo);

            var table = this.Query(sql);
            if (table != null && table.Rows.Count == 1)
            {
                return table.Rows[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取bom请购数量 prType:N一般请购,F委外请购
        /// </summary>
        public int GetBOMQty(string orNo, string bomSeq, string prType)
        {
            string sql = string.Format("select sum(prlqty) as qty from purprl,purprh where prhid='RH' and prhno=prlno and prhtyp=N'{2}' and prlid='RL' and prlsord=N'{0}' and prlsoseq=N'{1}' ", orNo, bomSeq, prType);

            var obj = this.ExecuteScalar(sql);
            if (obj != null)
            {
                return Util.ToInt(obj.ToString()); ;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取请购单号数量
        /// </summary>
        public int GetPRNoCount(string prNo)
        {
            string sql = string.Format("select count(*) from purprh where prhno=N'{0}' ", prNo);

            var obj = this.ExecuteScalar(sql);
            if (obj != null)
            {
                return Util.ToInt(obj.ToString()); ;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取最大请购单号
        /// </summary>
        public int GetMaxPRNo()
        {
            string sql = " select max(prhno) from PURPRH ";

            var obj = this.ExecuteScalar(sql);
            if (obj != null)
            {
                return Util.ToInt(obj.ToString()) + 1; ;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 新增请购单头表sql prType:N一般请购,F委外请购 prTypeDesc:一般请购、委外请购、材料请购
        /// </summary>
        public string InsertPRSql(string prNo, string orNo, string drawNo, string prType, string prTypeDesc, string sysUsername, string strDate)
        {
            string sql = string.Format("INSERT INTO PURPRH VALUES('RH',N'{0}',N'{1}',N'{2}',N'{3}','NE',' ',N'{4}',N'{5}',0,N'{6}',N'{7}',0,N'{8}') ",
                prNo, orNo, drawNo, prType, sysUsername, strDate, sysUsername, strDate, prTypeDesc
                );
            return sql;
        }

        /// <summary>
        /// 修改请购头表状态为更新 Sql
        /// </summary>
        public string UpdatePRStatusSql(string prNo, string status)
        {
            string sql = string.Format("update PURPRH set PRHSTAT =N'{1}' where PRHNO=N'{0}' ", prNo, status);

            return sql;
        }

        /// <summary>
        /// 请购维护明细更新 Sql
        /// </summary>
        public string UpdatePRDetailSql(string prNo, string prlseq, string price, string vendorNo, string vendorName, string curr, string isWeight, string prlrdte)
        {
            string sql = string.Format("update purprl set prlpacst=N'{2}',prlvnd=N'{3}',prlvndm=N'{4}',prlcur=N'{5}',prlwhs=N'{6}',prlrdte=N'{7}' where prlno=N'{0}' and prlseq=N'{1}' ", prNo, prlseq, price, vendorNo, vendorName, curr, isWeight);

            return sql;
        }

        /// <summary>
        /// 请购维护明细提交 Sql
        /// </summary>
        public string PostPRDetailSql(string prNo, string prlseq)
        {
            string sql = string.Format("update PURPRL set PRLPMT ='Y',PRLAPR = 'Y' where prlno=N'{0}' and prlseq=N'{1}' ", prNo, prlseq);

            return sql;
        }

        /// <summary>
        /// 删除请购头表Sql
        /// </summary>
        public string DeletePRSql(string prNo)
        {
            string sql = string.Format("delete from purprh where prhno=N'{0}' ", prNo);

            return sql;
        }

        /// <summary>
        /// 新增请购明细表sql 
        /// </summary>
        public string InsertPRDetailSql(string prNo, string prSeq, string prlprod, string prQty, string prDate, string prlrdte, string prlfac, string orNo, string drawNo, string prltno, string prlstation, string prlrule, string isUrgent, string um, string sysUsername, string strDate, string strTime, string prTypeDesc, string orSeq)
        {
            string sql = string.Format("INSERT INTO PURPRL(PRLID,PRLNO,PRLSEQ,PRLPROD,PRLQTY,PRLPACST,PRLPDTE,PRLDUE,PRLRDTE,PRLFAC,PRLWHS,PRLCUR,PRLVND,PRLPLNC,PRLPONO,PRLSORD,PRLMNO,PRLTNO,PRLSTATION,PRLPLNH,PRLRULE,PRLPMT,PRLAPR,PRLNEDM,PRLUM,PRLCBY,PRLCDTE,PRLCTM,PRLAPBY,PRLAPDTE,PRLAPTM,PRLPGM,prlqcqty,PRLRTQ,PRLSOSEQ,PRLMRK) VALUES('RL',N'{0}',N'{1}',N'{16}',N'{2}',0,N'{3}',N'{17}',0,N'{18}',' ',' ',0,' ',0,N'{4}',N'{5}',N'{6}',N'{19}',0,N'{7}','N','N',N'{8}',N'{9}',N'{10}',N'{11}',N'{12}',N'{13}',0,0,N'{14}',0,0,N'{15}',' ') ",
                prNo, prSeq, prQty, prDate, orNo, drawNo, prltno, prlrule, isUrgent, um, sysUsername, strDate, strTime, sysUsername, prTypeDesc, orSeq, prlprod, prlrdte, prlfac, prlstation
                );
            return sql;
        }

        /// <summary>
        /// 删除请购明细表Sql
        /// </summary>
        public string DeletePRDetailSql(string prNo)
        {
            string sql = string.Format("delete from purprl where prlno=N'{0}' ", prNo);

            return sql;
        }

        #endregion

        #region 配置

        /// <summary>
        /// 查询基础配置 UM单位 交易条件TC 币别CY 区域AR 付款方式PY 业务员CK 税率TX 工件类型PT
        /// </summary>
        public DataTable GetBaseParam(string type, string code)
        {
            string sql = string.Format(" select * from baseparameter where type=N'{0}'", type);
            if (!string.IsNullOrWhiteSpace(code))
            {
                sql += string.Format(" and code=N'{0}' ", code);
            }
            sql += " order by code ";
            return this.Query(sql);
        }

        #endregion
    }
}