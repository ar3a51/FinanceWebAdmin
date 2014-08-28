using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FinanceDataLayer;
using FinanceDataLayer.FinanceDataTableAdapters;

namespace FinanceDataLayer
{
   public class CompSyncDataLayer: IDisposable
    {
        private csuBIZCompanySyncTableAdapter _compSyncAdapter;
        private CompanyTableAdapter _compAdapter;
        private Boolean _isTransactionNeeded;

        public CompSyncDataLayer(Boolean isTransactionNeeded, String connectionString = "")
        {
            this._compSyncAdapter = new csuBIZCompanySyncTableAdapter();
            this._compAdapter = new CompanyTableAdapter();

            if (!connectionString.Equals(""))
            {
                this._compSyncAdapter.Connection.ConnectionString = connectionString;
                this._compAdapter.Connection.ConnectionString = connectionString;
            }


            this._isTransactionNeeded = isTransactionNeeded;
            if (isTransactionNeeded)
            {
                this._compSyncAdapter.Connection.Open();
                this._compSyncAdapter.Transaction = this._compSyncAdapter.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            }
        }

        public DataTable GetAllData()
        {
            return this._compSyncAdapter.GetData();
        }

        public DataTable getCompanyFromId(int companyId)
        {
            return this._compAdapter.GetData(companyId);
        }

        public void saveData(DataTable newTableData)
        {
            this._compSyncAdapter.Update((FinanceData.csuBIZCompanySyncDataTable)newTableData);
        }

        public void saveData(DataRow currentRow)
        {
            this._compSyncAdapter.Update(currentRow);
        }

        public void commitTrans()
        {
           
                this._compSyncAdapter.Transaction.Commit();
           
        }

        public void rejectChanges()
        { this._compSyncAdapter.Transaction.Rollback(); }

       

        public void Dispose()
        {
            if (this._isTransactionNeeded)
                this._compSyncAdapter.Connection.Close();

            this._compSyncAdapter = null;
        }
    }
}
