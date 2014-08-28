using AppDomain.AuditDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinanceDataLayer.auditDataLayer;

namespace FinanceWeb.Models
{
    public class AuditModel:IDisposable
    {
        private AuditDl _auditDataLayer;
        

        public AuditModel(String connectionString)
        {
            this._auditDataLayer = new AuditDl(connectionString);
           
        }

        public List<AuditDebtorSync> getAuditDebtorSyncParam(String entityType, DateTime fromDate, DateTime toDate)
        {
            
                return this._auditDataLayer.getAllAuditDebtorWithParam(entityType, fromDate, toDate).ToList<AuditDebtorSync>();
           
        }

        public List<AuditTransSync> getAuditTransSyncParam(String entityType, DateTime fromDate, DateTime toDate)
        {
          

            return this._auditDataLayer.getAllAuditTransWithParam(entityType, fromDate, toDate/*.Add(_lateTime)*/).ToList<AuditTransSync>();

        }

        public void Dispose()
        {
            this._auditDataLayer = null;
        }
    }
}