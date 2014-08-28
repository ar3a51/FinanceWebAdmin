using AppDomain.AuditDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FinanceDataLayer;

namespace FinanceDataLayer.auditDataLayer
{
   public class AuditDl: IDisposable
    {
       private FinanceAuditSyncEntities _entities;

        public AuditDl(String connString = "")
        {


            if (!connString.Equals(""))
                this._entities = new FinanceAuditSyncEntities(connString);
            else
                this._entities = new FinanceAuditSyncEntities();
        }

        public void saveDebtorSyncAudit(AuditDebtorSync debtorSync)
        {
            csuAuditDebtorSync debtorSyncEntity = new csuAuditDebtorSync
            {
                MemberId = debtorSync.MemberId,
                vchUsername = debtorSync.modifiedBy,
                vchType = debtorSync.entityType,
                dtChangeDate = debtorSync.updateDate,
                vchAction = debtorSync.action,
                vchReason = debtorSync.reason
            };

            this._entities.AddTocsuAuditDebtorSyncs(debtorSyncEntity);
            
           

        }

        public IEnumerable<AuditTransSync> getAllAuditTransWithParam(String systemName, DateTime fromDate, DateTime toDate)
        {
            return (from audit in this._entities.csuAuditTransSyncs
                                                       where audit.vchSystem.Equals(systemName) &&
                                                             audit.dtChangeDate >= fromDate &&
                                                             audit.dtChangeDate <= toDate
                                                       select new AuditTransSync { action = audit.vchAction,
                                                        auditId = audit.iAuditId,
                                                        documentId = audit.documentId,
                                                        modifiedBy = audit.chupdateBy,
                                                        reason = audit.vchReason,
                                                        system = audit.vchSystem,
                                                        updateDate = (DateTime)audit.dtChangeDate}).AsEnumerable<AuditTransSync>();
        }

        public IEnumerable<AuditDebtorSync> getAllAuditDebtorWithParam(String entityType,
                                                                       DateTime fromDate,
                                                                       DateTime toDate)
        {

            return (from debtorAudit in this._entities.csuAuditDebtorSyncs
                    where debtorAudit.vchType.Equals(entityType) &&
                    debtorAudit.dtChangeDate >= fromDate &&
                    debtorAudit.dtChangeDate <= toDate
                    select new AuditDebtorSync
                    {
                        action = debtorAudit.vchAction,
                        auditId = debtorAudit.iAuditId,
                        entityType = debtorAudit.vchType,
                        MemberId = (int)debtorAudit.MemberId,
                        modifiedBy = debtorAudit.vchUsername,
                        reason = debtorAudit.vchReason,
                        updateDate = (DateTime)debtorAudit.dtChangeDate
                    }).AsEnumerable<AuditDebtorSync>();
        
        }

        public void saveTransAudit(AuditTransSync transSyncData)
        {
            csuAuditTransSync transSyncEntity = new csuAuditTransSync
            {
                 documentId = transSyncData.documentId,
                 dtChangeDate = transSyncData.updateDate,
                vchAction = transSyncData.action,
                vchSystem = transSyncData.system,
                vchReason = transSyncData.reason,
                 chupdateBy = transSyncData.modifiedBy
            };

            this._entities.AddTocsuAuditTransSyncs(transSyncEntity);
           
        }

        public void commitChanges()
        {
            try
            {
                this._entities.SaveChanges();
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }



        public void Dispose()
        {
            this._entities = null;
        }
    }
}
