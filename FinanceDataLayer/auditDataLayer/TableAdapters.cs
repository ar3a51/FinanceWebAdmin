using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FinanceDataLayer.FinanceDataTableAdapters;

namespace FinanceDataLayer.auditDataLayer
{
   public class TableAdapters: IDisposable
    {
       private csuAuditDebtorSyncTableAdapter _debtorTA;
       private csuAuditTransSyncTableAdapter _transTA;
       private String _tableAdapterType;
       

       private Boolean _isTransactionNeeded;

       public TableAdapters(String tableAdapterType, Boolean isTransactionNeeded, String connectionString="")
       {
           this._isTransactionNeeded = isTransactionNeeded;
           this._tableAdapterType = tableAdapterType;

           if (tableAdapterType.Equals("debtor"))
            this._debtorTA = new csuAuditDebtorSyncTableAdapter();
          else
               this._transTA = new csuAuditTransSyncTableAdapter();

           if (!connectionString.Equals("")) {
               if (tableAdapterType.Equals("debtor"))
                   this._debtorTA.Connection.ConnectionString = connectionString;
               else
                   this._transTA.Connection.ConnectionString = connectionString;
           }

       }

       public void createTransaction()
       {
           if (this._tableAdapterType.ToLower().Equals("debtor"))
           {
               this._debtorTA.Connection.Open();
               this._debtorTA.Transaction = this._debtorTA.Connection.BeginTransaction(IsolationLevel.ReadUncommitted);
           }
           else
           {
               this._transTA.Connection.Open();
               this._transTA.Transaction = this._transTA.Connection.BeginTransaction(IsolationLevel.ReadUncommitted);
           }
       }

       public Object getAdapter()
       {
           if (this._tableAdapterType.ToLower().Equals("debtor"))
               return this._debtorTA;
           else
               return this._transTA;
       }

       public void executeTrans()
       {
          
               if (this._tableAdapterType.ToLower().Equals("debtor"))
                   this._debtorTA.Transaction.Commit();
               else
                   this._transTA.Transaction.Commit();
          
        
                
           
       }

       public void rejectChanges()
       {
           if (this._tableAdapterType.ToLower().Equals("debtor"))
               this._debtorTA.Transaction.Rollback();
           else
               this._transTA.Transaction.Rollback();
       }


       public void Dispose()
       {
           if (this._tableAdapterType.ToLower().Equals("debtor"))
               this._debtorTA.Connection.Close();
           else
               this._transTA.Connection.Close();

           this._debtorTA = null;
           this._transTA = null;
       }
    }
}
