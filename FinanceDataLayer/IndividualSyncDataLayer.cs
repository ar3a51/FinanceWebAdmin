using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using AppDomain;
using FinanceDataLayer.FinanceDataTableAdapters;

namespace FinanceDataLayer
{
    public class IndividualSyncDataLayer: IDisposable
    {
        private csuBIZIndividualSyncTableAdapter _indivTableAdapter;
        private IndividualTableAdapter _indivUserTableAdapter;
        private FinanceData financeDataSet;
        private Boolean _isTransactionNeeded;
      

        public IndividualSyncDataLayer(Boolean isTransactionNeeded = false, String connectionString = "")
        {
           

            this._isTransactionNeeded = isTransactionNeeded;  
            this._indivTableAdapter = new csuBIZIndividualSyncTableAdapter();
            this._indivUserTableAdapter = new IndividualTableAdapter();

            if (!connectionString.Equals(""))
            {
                this._indivTableAdapter.Connection.ConnectionString = connectionString;
                this._indivUserTableAdapter.Connection.ConnectionString = connectionString;
            }

            //this._indivTableAdapter.Adapter.AcceptChangesDuringUpdate = false;

            if (isTransactionNeeded)
            {
                this._indivTableAdapter.Connection.Open();
                this._indivTableAdapter.Transaction = this._indivTableAdapter.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            }

            this.financeDataSet = new FinanceData();
            
        }

        public void addNewData(DataTable indivDataTable)
        {
            
            this._indivTableAdapter.Update((FinanceData.csuBIZIndividualSyncDataTable)indivDataTable);
            
            
        }

        public DataTable getAllData()
        {
            //return this.financeDataSet.csuBIZCompanySync;
            try
            {
                return this._indivTableAdapter.GetData();
            }
            catch (Exception exception)
            { throw exception; }
            
        }

        public DataTable getUserFromId(int userId)
        {
            try
            {
                return this._indivUserTableAdapter.GetData(userId);
            }
            catch (Exception exception) { throw exception; }

        }

        public void updateData(DataRow updatedRow)
        {
            this._indivTableAdapter.Update(updatedRow);
           // this.financeDataSet.Tables.Add(
        }

        public void commitChanges()
        {
           
                this._indivTableAdapter.Transaction.Commit();
          
            
        }

        public void rejectChanges()
        {
            this._indivTableAdapter.Transaction.Rollback();
        }
       

        public void Dispose()
        {
             if(this._isTransactionNeeded)
                this._indivTableAdapter.Connection.Close();

            this._indivTableAdapter = null;
        }
    }
}
