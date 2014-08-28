using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppDomain;
using AppDomain.paramInput;


namespace FinanceDataLayer
{
   public class TopClassDataLayer
    {
       //private TopClassEntities _topClEnt;

       public TopClassDataLayer()
       {
          // this._topClEnt = new TopClassEntities();
           
       }

       public IEnumerable<TopClassSync> getAllOrders(int iStartRow, int recsPerPage, String status, TopClassEntities ent, DateTime fromDate, DateTime toDate, String keyword, out int recordCount)
       {
           recordCount = 0;
           //keyword = (keyword == null)?"":keyword;
           iStartRow = (iStartRow == 1) ? 0 : iStartRow;


           /*recordCount = (from oh in ent.esdTransactionHeaders
                                             where (oh.DocumentDate >= fromDate && oh.DocumentDate <= toDate)
                                             && oh.Status.Equals(status) && oh.AccountID.Contains((keyword == null)? oh.AccountID : (string)keyword) select oh).Count();*/

           IQueryable<TopClassSync> query = (from oh in ent.esdTransactionHeaders
                                             where (oh.DocumentDate >= fromDate && oh.DocumentDate <= toDate)
                                             && oh.Status.Equals(status) && oh.AccountID.Contains((keyword == null) ? oh.AccountID : (string)keyword)
                                             orderby oh.DocumentDate descending
                                             select new TopClassSync
                                             {
                                                 AccountID = oh.AccountID,
                                                 DocumentId = oh.DocumentID,
                                                 documentType = oh.DocumentType,
                                                 guid = oh.Guid,
                                                 DocumentDate = (DateTime)oh.DocumentDate,
                                                 Status = oh.Status,
                                                 Amount = oh.Amount,
                                             })/*.Skip(iStartRow).Take(recsPerPage)*/.AsQueryable<TopClassSync>();

           recordCount = query.Count();
           return query.Skip(iStartRow).Take(recsPerPage).AsEnumerable<TopClassSync>();

       }

       public TopClassSync getOrder(int guidValue, TopClassEntities ent)
       {

           esdTransactionHeader transHeader = ent.esdTransactionHeaders.First<esdTransactionHeader>(o => o.Guid == guidValue);
          
           return new TopClassSync
           {
               AccountID = transHeader.AccountID,
               DocumentDate =(DateTime)transHeader.DocumentDate,
               DocumentId = transHeader.DocumentID,
               documentType = transHeader.DocumentType,
               guid = transHeader.Guid,
               Status = transHeader.Status,
               StatusDesc = transHeader.StatusDescription
           };
       }


       public void updateStatus(TransInput data, TopClassEntities ent)
       {
           int guidValue;
           //foreach(TransInput data in dataLists){
               guidValue = int.Parse(data.transId);
               esdTransactionHeader transHeader = ent.esdTransactionHeaders.First<esdTransactionHeader>(o => o.Guid == guidValue);
               transHeader.Status = data.status;
          // }

       }
    }
}
