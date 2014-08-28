using AppDomain.AuditDomain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FinanceDataLayer.FinanceDataTableAdapters;

namespace FinanceDataLayer.auditDataLayer
{
    public class TransAuditDl
    {
        public TransAuditDl()
        {

        }

        public void saveData(AuditTransSync auditData, csuAuditTransSyncTableAdapter tableAdapter)
        {
            using (FinanceData.csuAuditTransSyncDataTable auditTable = new FinanceData.csuAuditTransSyncDataTable())
            {
                DataRow newRow = auditTable.NewRow();
                newRow["vchModifiedBy"] = auditData.modifiedBy;
                newRow["dtModifiedDate"] = auditData.updateDate;
                newRow["vchAction"] = auditData.action;
                newRow["iTransID"] = auditData.documentId;
                newRow["vchSystem"] = auditData.system;
                newRow["vchReason"] = auditData.reason;

                auditTable.Rows.Add(newRow);

                tableAdapter.Update(auditTable);
            }

            //tableAdapter.Update()
        }
    }
}
