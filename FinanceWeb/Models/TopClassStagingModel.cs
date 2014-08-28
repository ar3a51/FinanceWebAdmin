using AppDomain;
using AppDomain.paramInput;
using AppDomain.AuditDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using FinanceDataLayer;
using FinanceDataLayer.auditDataLayer;

namespace FinanceWeb.Models
{
    public class TopClassStagingModel : IDisposable
    {
        private TopClassDataLayer _topClass;
        private AuditDl _auditDl;

        private String _connectionString;
        private String _auditConnString;

        private EntityCreator _tcEntity;
        
        public TopClassStagingModel(String connString = "", String auditConnString = "")
        {
            this._auditConnString = auditConnString;
            this._topClass = new TopClassDataLayer();
            this._auditDl = new AuditDl(this._auditConnString);

          

            if (!connString.Equals(""))
            {
                this._connectionString = connString;
            }

            

        }


        public IEnumerable<TopClassSync> getAllTransactions(int iStartRow, int recsPerPage, String transStatus,DateTime fromDate, DateTime toDate, String keyword, out int totalRecords)
        {
            List<TopClassSync> topClassData;
            
            using (_tcEntity = new EntityCreator(this._connectionString))
            {
                topClassData = this._topClass.getAllOrders(iStartRow, recsPerPage, transStatus, this._tcEntity.getConstructor(), fromDate, toDate, keyword, out totalRecords).ToList<TopClassSync>();
            }

            return topClassData;
        }

        public TopClassSync getTransDetail(int guidValue)
        {
            TopClassSync anEntity;
            using (_tcEntity = new EntityCreator(this._connectionString))
            {
                anEntity = this._topClass.getOrder(guidValue,_tcEntity.getConstructor());
            }

            return anEntity;
        }


        public void updateData(TransInput[] datalist,String transStatus,DateTime fromDate, DateTime toDate,AuditTransSync auditData)
        {
            List<TopClassSync> topClassData;
           
            using (this._tcEntity = new EntityCreator(this._connectionString))
            {
                foreach(TransInput data in datalist)
                {
                    auditData.documentId = this._topClass.getOrder(int.Parse(data.transId), this._tcEntity.getConstructor()).DocumentId;
                    this._topClass.updateStatus(data, this._tcEntity.getConstructor());
                    
                    this._auditDl.saveTransAudit(auditData);
                }

                this._auditDl.commitChanges();
                this._tcEntity.saveChanges();


                //topClassData = this._topClass.getAllOrders(transStatus, this._tcEntity.getConstructor(), fromDate, toDate).ToList<TopClassSync>();

            }

            

            //return topClassData;
        }

        public void Dispose()
        {
            this._topClass = null;
            this._auditDl = null;
           
        }
    }
}