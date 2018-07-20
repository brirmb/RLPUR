using RLPUR.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
        /// 获取材料请购列表
        /// </summary>
        public DataTable GetMatPRDetailList(string prNo)
        {
            string sql = string.Format("select * from purprl,purprh where prhtyp='S' and prhid='RH' and prhno=prlno and prlno=N'{0}' order by prlseq ", prNo);

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
        /// 请购查询
        /// </summary>
        /// <returns></returns>
        public DataTable PurQuery(string prNoFrom, string prNoTo, string orNoFrom, string orNoTo, string venFrom, string venTo, string dateFrom, string dateTo, string drawNoFrom, string drawNoTo, string prlNo, string prlName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select prlno,prlseq,PRLSORD,PRLMNO,PRLTNO,bomnam,bommat,PRLSTATION,PRLQTY,PRLVND,prlvndm,PRLPACST,PRLUM,PRLCUR,prlcdte,prlpdte from PURPRL,tsfcbom where PRLID='RL' and bomwno=prlsord and prlsoseq=bomseq");

            if (!string.IsNullOrWhiteSpace(orNoFrom))
            {
                sql.AppendFormat(" and PRLSORD>='{0}' ", orNoFrom);
            }
            if (!string.IsNullOrWhiteSpace(orNoTo))
            {
                sql.AppendFormat(" and PRLSORD<='{0}' ", orNoTo);
            }

            string condition = GetPurQueryCondition(prNoFrom, prNoTo, orNoFrom, orNoTo, venFrom, venTo, dateFrom, dateTo,
                drawNoFrom, drawNoTo, prlNo, prlName);
            if (!string.IsNullOrWhiteSpace(condition))
            {
                sql.Append(condition);
            }

            sql.Append(" union ");
            sql.Append(" select prlno,prlseq,PRLSORD,PRLMNO,PRLTNO,prloutno,prlpicno,PRLSTATION,PRLQTY,PRLVND,prlvndm,PRLPACST,PRLUM,PRLCUR,prlcdte,prlpdte from purprl where prlsord='' ");

            if (!string.IsNullOrWhiteSpace(condition))
            {
                sql.Append(condition);
            }

            sql.Append(" order by prlno,prlseq ");

            return this.Query(sql.ToString());
        }

        /// <summary>
        /// 拼接查询条件
        /// </summary>
        private string GetPurQueryCondition(string prNoFrom, string prNoTo, string orNoFrom, string orNoTo, string venFrom, string venTo, string dateFrom, string dateTo, string drawNoFrom, string drawNoTo, string prlNo, string prlName)
        {
            StringBuilder sql = new StringBuilder("");
            if (!string.IsNullOrWhiteSpace(prNoFrom))
            {
                sql.AppendFormat(" and PRLNO>={0} ", prNoFrom);
            }
            if (!string.IsNullOrWhiteSpace(prNoTo))
            {
                sql.AppendFormat(" and PRLNO<={0} ", prNoTo);
            }
            //if (!string.IsNullOrWhiteSpace(orNoFrom))
            //{
            //    sql.AppendFormat(" and PRLSORD>='{0}' ", orNoFrom);
            //}
            //if (!string.IsNullOrWhiteSpace(orNoTo))
            //{
            //    sql.AppendFormat(" and PRLSORD<='{0}' ", orNoTo);
            //}
            if (!string.IsNullOrWhiteSpace(venFrom))
            {
                sql.AppendFormat(" and PRLVND>='{0}' ", venFrom);
            }
            if (!string.IsNullOrWhiteSpace(venTo))
            {
                sql.AppendFormat(" and PRLVND<='{0}' ", venTo);
            }
            sql.AppendFormat(" and PRLCDTE between '{0}' and '{1}' ", dateFrom, dateTo);
            //if (!string.IsNullOrWhiteSpace(dateFrom))
            //{
            //    sql.AppendFormat(" and PRLCDTE>='{0}' ", dateFrom);
            //}
            //if (!string.IsNullOrWhiteSpace(dateTo))
            //{
            //    sql.AppendFormat(" and PRLCDTE<='{0}' ", dateTo);
            //}
            if (!string.IsNullOrWhiteSpace(drawNoFrom))
            {
                sql.AppendFormat(" and PRLMNO>='{0}' ", drawNoFrom);
            }
            if (!string.IsNullOrWhiteSpace(drawNoTo))
            {
                sql.AppendFormat(" and PRLMNO<='{0}' ", drawNoTo);
            }
            if (!string.IsNullOrWhiteSpace(prlNo))
            {
                sql.AppendFormat(" and PRLTNO like N'%{0}%' ", prlNo);
            }
            if (!string.IsNullOrWhiteSpace(prlName))
            {
                sql.AppendFormat(" and bomnam like N'%{0}%' ", prlName);
            }

            return sql.ToString();
        }

        /// <summary>
        /// 出货查询
        /// </summary>
        /// <returns></returns>
        public DataTable ShipQuery(string orNo, string custNo, string dateFrom, string dateTo)
        {
            StringBuilder sql = new StringBuilder("");
            sql.Append("select tsfcbomh.bomhmno mno2,* from shiping left join tsfcbomh on bomhwno=shipso where 1=1 ");
            sql.AppendFormat(" and shipdate between '{0}' and '{1}' ", dateFrom, dateTo);
            //if (!string.IsNullOrWhiteSpace(dateFrom))
            //{
            //    sql.AppendFormat(" and shipdate>='{0}' ", dateFrom);
            //}
            //if (!string.IsNullOrWhiteSpace(dateTo))
            //{
            //    sql.AppendFormat(" and shipdate<='{0}' ", dateTo);
            //}
            if (!string.IsNullOrWhiteSpace(orNo))
            {
                sql.AppendFormat(" and shipsono='{0}' ", orNo);
            }
            if (!string.IsNullOrWhiteSpace(custNo))
            {
                sql.AppendFormat(" and shipcustno='{0}' ", custNo);
            }

            return this.Query(sql.ToString());
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
        /// 新增请购单头表sql prType:N一般请购,F委外请购,S材料请购 prTypeDesc:一般请购、委外请购、材料请购
        /// </summary>
        public string InsertPRSql(string prNo, string orNo, string drawNo, string prType, string curr, string prTypeDesc, string sysUsername, string strDate, string prhapby, string prhapdte)
        {
            string sql = string.Format("INSERT INTO PURPRH VALUES('RH',N'{0}',N'{1}',N'{2}',N'{3}','NE',N'{4}',N'{5}',N'{6}',0,N'{7}',N'{8}',0,N'{9}') ",
                prNo, orNo, drawNo, prType, curr, sysUsername, strDate, prhapby, prhapdte, prTypeDesc
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
            string sql = string.Format("update purprl set prlpacst=N'{2}',prlvnd=N'{3}',prlvndm=N'{4}',prlcur=N'{5}',prlwhs=N'{6}',prlrdte=N'{7}' where prlno=N'{0}' and prlseq=N'{1}' ", prNo, prlseq, price, vendorNo, vendorName, curr, isWeight, prlrdte);

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
        /// 删除材料请购单
        /// </summary>
        public int DeleteMatPR(string prNo)
        {
            string sql = string.Format("update purprh set prhid='RZ' where prhtyp='S' and prhno=N'{0}' ", prNo);

            return this.Execute(sql);
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
        /// 新增材料请购明细sql 
        /// </summary>
        public string InsertMatPRDetailSql(string prNo, string prSeq, string prQty, string prDate, string prlwhs, string prltno, string prlstation, string prlrule, string um, string sysUsername, string strDate, string strTime, string remark, string prloutno, string prlpicno)
        {
            string sql = string.Format("INSERT INTO PURPRL VALUES('RL',N'{0}',N'{1}',N'RL',N'{2}',0,N'{3}',0,0,N'RL',N'{4}','RMB',0,'','',0,'','',N'{5}',N'{6}',0,N'{7}','N','N','N',N'{8}',N'{9}',N'{10}',N'{11}',N'{12}',0,0,N'材料采购',0,0,0,N'{13}',N'{14}',N'{15}') ",
                prNo, prSeq, prQty, prDate, prlwhs, prltno, prlstation, prlrule, um, sysUsername, strDate, strTime, sysUsername, remark, prloutno, prlpicno
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

        #region 出货

        /// <summary>
        /// 根据工令号获取出货通知列表
        /// </summary>
        public DataTable GetShipNoticeList(string orNo)
        {
            string sql = string.Format(" select a.*,b.*,rcnam from contract a,contratdetail b,salrcm where a.ordno=b.ordno and a.custno=rccust and a.ordno=N'{0}' order by seq", orNo);

            return this.Query(sql);
        }

        /// <summary>
        /// 获取实际出货数量
        /// </summary>
        public int GetActualShipQty(string orNo, string seq)
        {
            string sql = string.Format("select sum(shipqact) rqty from shiping where shipsono=N'{0}' and shipseq=N'{1}' ", orNo, seq);

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
        /// 根据出货单号获取出货列表
        /// </summary>
        public DataTable GetShipList(string shipNo)
        {
            string sql = string.Format(" select * from shiping,contract where shipno=N'{0}' and left(shipsono,7)=ordno ", shipNo);

            return this.Query(sql);
        }

        /// <summary>
        /// 根据工令号获取生产任务单
        /// </summary>
        public DataTable GetMOrder(string orNo)
        {
            string sql = string.Format(" select * from morder where mono=N'{0}' ", orNo);

            return this.Query(sql);
        }

        /// <summary>
        /// 根据工令号获取备品工令
        /// </summary>
        public DataTable GetBeiping(string orNo)
        {
            string sql = string.Format(" select * from beipingongling where bpsono=N'{0}' ", orNo);

            return this.Query(sql);
        }

        /// <summary>
        /// 获取工件库存Sql
        /// </summary>
        public DataTable GetInventoryByItem(string whCode, string drawNo)
        {
            string sql = string.Format("select * from inventory where whcode=N'{0}' and pono=N'{1}' ", whCode, drawNo);

            return this.Query(sql);
        }

        /// <summary>
        /// 获取合同相关信息
        /// </summary>
        public DataTable GetContractInfo(string orNo, string seq)
        {
            string sql = string.Format("select * from contratdetail,contract where contratdetail.ordno=contract.ordno and contract.ordno=N'{0}' and seq=N'{1}' ", orNo, seq);

            return this.Query(sql);
        }

        /// <summary>
        /// 新增出库记录Sql
        /// </summary>
        public string InsertShipSql(string spNo, string seq, string orNo, string drawNo, string custNo, string custName, string itemNo, string um, string orQty, string planQty, string actQty, string spDate, string status)
        {
            string sql = string.Format("insert into shiping values(N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}',N'{9}',N'{10}',N'{11}',N'{12}',N'{13}') ", spNo, seq, orNo, orNo, drawNo, custNo, custName, itemNo, um, orQty, planQty, actQty, spDate, status);
            return sql;
        }

        /// <summary>
        /// 更新出货数量sql
        /// </summary>
        public string UpdateShipQtySql(string shipNo, string shipSeq, int qty)
        {
            string sql = string.Format(" update shiping set shipqact=shipqact+{2} where shipno=N'{0}' and shipseq=N'{1}' ", shipNo, shipSeq, qty);

            return sql;
        }

        /// <summary>
        /// 更新出货状态sql
        /// </summary>
        public string UpdateShipStatusSql(string shipNo, string shipSeq)
        {
            string sql = string.Format(" update shiping set shipstatus=1 where shipno=N'{0}' and shipseq=N'{1}' and shipqplan=shipqact ", shipNo, shipSeq);

            return sql;
        }

        /// <summary>
        /// 新增工件库存Sql
        /// </summary>
        public string InsertInventorySql(string whCode, string drawNo, string orNO, string procCode, string itemNo, string qty, string um)
        {
            string sql = string.Format("insert into inventory values(N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}') ", whCode, drawNo, orNO, procCode, itemNo, -1 * Util.ToDecimal(qty), um);
            return sql;
        }

        /// <summary>
        /// 更新工件库存Sql
        /// </summary>
        public string UpdateInventorySql(string whCode, string drawNo, string qty)
        {
            string sql = string.Format(" update inventory set qty=qty-{2} where whcode=N'{0}' and pono=N'{1}' ", whCode, drawNo, Util.ToDecimal(qty));

            return sql;
        }

        /// <summary>
        /// 新增出入库明细Sql
        /// </summary>
        public string InsertTransDetailSql(string orNo, string drawNo, string whCode, string qty, string kpNo, string orNo2, string type)
        {
            string sql = string.Format("insert into trans_detail values(N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}') ", orNo, drawNo, whCode, qty, DateTime.Now, kpNo, orNo2, type);
            return sql;
        }

        /// <summary>
        /// 新增质保金Sql
        /// </summary>
        public string InsertZhibaojinSql(string shipNo, string orNo, string seq, string itemNo, string drawNo, string custNo, string custName, string qty, string zbAmt, string zbSkamt, string shipDate, string shipTime, string limit, string lastDate)
        {
            string sql = string.Format("insert into zhibaojin values(N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}',N'{9}',N'{10}',N'{11}',N'{12}',N'{13}') ", shipNo, orNo, seq, itemNo, drawNo, custNo, custName, qty, zbAmt, zbSkamt, shipDate, shipTime, limit, lastDate);
            return sql;
        }

        #endregion

        #region 基础 配置

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

        /// <summary>
        /// 更新基础配置
        /// </summary>
        public int UpdateBaseParam(string type, string code, string desc)
        {
            string sql = string.Format("update baseparameter set description=N'{2}' where type=N'{0}' and code=N'{1}'", type, code, desc);

            return this.Execute(sql);
        }

        /// <summary>
        /// 查询请购明细（状态等）
        /// </summary>
        public DataRow GetVendor(string venNo)
        {
            string sql = string.Format(" select avnam,avcur from puravm where avid='A' and avend=N'{0}' ", venNo);

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

        #endregion
    }
}