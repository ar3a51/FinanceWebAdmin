using AppDomain.AuditDomain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FinanceDataLayer.FinanceDataTableAdapters;


namespace FinanceDataLayer.auditDataLayer
{
    public class DebtorAuditDl
    {
        public DebtorAuditDl()
        {

        }

        public void saveData(AuditDebtorSync auditData, csuAuditDebtorSyncTableAdapter tableAdapter)
        {
            using (FinanceData.csuAuditDebtorSyncDataTable auditTable = new FinanceData.csuAuditDebtorSyncDataTable())
            {
                DataRow newRow = auditTable.NewRow();
                newRow["vchModifiedBy"] = auditData.modifiedBy;
                newRow["dtModifiedDate"] = auditData.updateDate;
                newRow["vchAction"] = auditData.action;
                newRow["iIndividualId"] = auditData.MemberId;
                newRow["vchEntityType"] = auditData.entityType;

                auditTable.Rows.Add(newRow);

                tableAdapter.Update(auditTable);
            }
           
            //tableAdapter.Update()
        }

       /* public IEnumerable<AuditDebtorSync> getData(String modifyCriteria, DateTime StartModifyDate, DateTime endModifyDate, csuAuditDebtorSyncTableAdapter tableAdapter)
        {

            return tableAdapter.GetData(StartModifyDate, endModifyDate, modifyCriteria).Select(null,"orderby;

        }*/
    }
}
