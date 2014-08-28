using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FinanceDataLayer
{
   public class EntityCreator: IDisposable
    {
       private TopClassEntities _topEnt;
       public EntityCreator(String connectionString="")
       {
           this._topEnt = new TopClassEntities(connectionString);

           /*if (!connectionString.Equals(""))
               this._topEnt.Connection.ConnectionString = connectionString;*/
       }

       public TopClassEntities getConstructor()
       {
           return this._topEnt;
       }

       public void saveChanges()
       {
           try
           {
               this._topEnt.SaveChanges();
           }
           catch (Exception exception)
           {
               throw exception;
           }
       }




       public void Dispose()
       {
           this._topEnt = null;
       }
    }
}
