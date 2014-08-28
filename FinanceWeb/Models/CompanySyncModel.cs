using AppDomain;
using AppDomain.AuditDomain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FinanceDataLayer;
using FinanceDataLayer.FinanceDataTableAdapters;
using FinanceDataLayer.auditDataLayer;

namespace FinanceWeb.Models
{
    public class CompanySyncModel: IDisposable
    {
        private DataTable _currentTable;
        private String connectionString;
        private String _connStringAudit;

        public CompanySyncModel(String connectionString="", String connStringAudit="")
        {
            this._currentTable = null;
            this.connectionString = connectionString;

            this._connStringAudit = connStringAudit;
        }

        public DataTable getAllCompanySyncList()
        {
            using (CompSyncDataLayer compDL = new CompSyncDataLayer(false, this.connectionString))
            {
                this._currentTable = compDL.GetAllData();
            }

            return this._currentTable;
        }

        public DataTable addData(indivSync CompSyncData, DataTable currentTable, AuditDebtorSync auditData)
        {

            DataRow newRow = currentTable.NewRow();

            newRow["batch_id"] = null;
            newRow["company_id"] = CompSyncData.individual_id;
            newRow["insert_update"] = CompSyncData.insert_update;
            newRow["update_date"] = DateTime.Now;
            newRow["record_status"] = CompSyncData.recordStatus;

            currentTable.Rows.Add(newRow);

            CompSyncDataLayer compDL = new CompSyncDataLayer(true, this.connectionString);
            AuditDl audit = new AuditDl(this._connStringAudit);

            try
            {
                compDL.saveData(currentTable);
                audit.saveDebtorSyncAudit(auditData);

                audit.commitChanges();
                compDL.commitTrans();

                
                this._currentTable = compDL.GetAllData();


                audit.Dispose();
                compDL.Dispose();

                audit = null;

                compDL = null;
               

            }
            catch (Exception exception)
            { 
                 compDL.rejectChanges();

                 audit.Dispose();
                 compDL.Dispose();

                 audit = null;

                 compDL = null;

                throw exception;
            }

            

            return this._currentTable;
        }

        public Boolean checkCompanyId(int companyId)
        {
            int numberOfRows = 0;
            
            using (CompSyncDataLayer compDL = new CompSyncDataLayer(false, this.connectionString)) {

                numberOfRows = compDL.getCompanyFromId(companyId).Rows.Count;
            }

            if (numberOfRows > 0)
                return true;
            else
                return false;
        }
        public void Dispose()
        {
            this._currentTable = null;
        }
    }
}