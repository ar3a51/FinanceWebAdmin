using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FinanceDataLayer
{
  public class TransactionUtil
    {
       
        

       
       public static TransactionScope getTransaction()
        {
            TransactionOptions _transOptions;
            _transOptions = new TransactionOptions();
            _transOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            _transOptions.Timeout = TimeSpan.MaxValue;
            //this._transScope = new TransactionScope();

            return new TransactionScope(TransactionScopeOption.Required, _transOptions);
            
        }

       
    }
}
