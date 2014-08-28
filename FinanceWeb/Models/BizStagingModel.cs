using AppDomain;
using AppDomain.paramInput;
using AppDomain.AuditDomain;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinanceDataLayer;
using FinanceDataLayer.auditDataLayer;
using FinanceDataLayer.FinanceDataTableAdapters;

namespace FinanceWeb.Models
{
    public class BizStagingModel: IDisposable
    {
        private DataTable _dataTable;
        private String connectionString="";
        private String _auditConnString = "";

       

        public BizStagingModel(String connectionString="", String auditConnString = "")
        {
            this._dataTable = null;
            this.connectionString = connectionString;
            this._auditConnString = auditConnString;

           
        }



        public IEnumerable<bizSync> getAllParsedTrans(int startRow, int recsPerPage,String transStatus,DateTime fromDate, DateTime toDate, String keyword, out DataTable unParsedTable, out int? totalRecords)
        {
            List<bizSync> list;
            DataTable result;
            totalRecords = 0;
            unParsedTable = null;
            

            /*fromDate = DateTime.Parse(fromDate.ToString("yyyy-MM-dd hh:mm"));
            toDate = DateTime.Parse(toDate.ToString("yyyy-MM-dd hh:mm"));*/
            using ( BizStagingDataLayer bizDl = new BizStagingDataLayer(false,this.connectionString))
            {
                result = new DataTable();
                result.Columns.Add("TransID");
                result.Columns.Add("Document ID");
                result.Columns.Add("Status");
                result.Columns.Add("Document type");
                result.Columns.Add("Member ID");
                result.Columns.Add("Date Created");

                unParsedTable = bizDl.getAllBizStagingData(startRow,recsPerPage,transStatus,fromDate, toDate,keyword, out totalRecords);
                //DataRow[] resultRows = unParsedTable.Select(null, "Date Created DESC");
                list = new List<bizSync>();
                foreach (DataRow currRow in unParsedTable.Rows)
                {
                    /*DataRow newRow = result.NewRow();
                    newRow["TransID"] = currRow["ONYX_iTranID"];
                    newRow["Document ID"] = currRow["DOC_ID"].ToString();
                    newRow["Status"] = currRow["ONYX_StagingStatus"].ToString();
                    newRow["Document Type"] = currRow["BAT_DOC_TYPE"].ToString();
                    newRow["Member ID"] = currRow["ONYX_iOwnerId"].ToString();
                    newRow["Date Created"] = DateTime.Parse(currRow["ONYX_dtTransactionDate"].ToString());*/

                    //result.Rows.Add(newRow);
                    list.Add(new bizSync
                    {
                        ONYX_iTransId = int.Parse(currRow["TransID"].ToString()),
                        DOC_ID = currRow["Document ID"].ToString(),
                        ONYX_StagingStatus = currRow["Status"].ToString(),
                        BAT_DOC_TYPE = currRow["Document Type"].ToString(),
                        ONYX_iOwnerId = int.Parse(currRow["Member ID"].ToString()),
                        LNE_AMT1 = Decimal.Round(Decimal.Parse(currRow["LNE_AMT1"].ToString(),System.Globalization.NumberStyles.Currency),2),
                        ONYX_dtTransactionDate = DateTime.Parse(currRow["Date Created"].ToString())
                    });
                }
                //this._dataTable.AsDataView().s
            }

            return list;
        }

        public bizSync getTransDetails(int TransId)
        {
            bizSync transDetail = null;
            using (BizStagingDataLayer dl = new BizStagingDataLayer(false, this.connectionString))
            {
                transDetail = dl.getTransDetails(TransId);
            }

            return transDetail;
        }

        public void updateStatus(TransInput[] transDatas,
                                                 DateTime fromDate,
                                                 DateTime toDate,
                                                String transStatus, 
                                                DataTable currentTable, 
                                                out DataTable outputTable,
                                                AuditTransSync auditData) 
        {
             int rowIndex;
             outputTable = null;
            // TransAuditDl transAuditDl = new TransAuditDl();
            AuditDl auditDl = new AuditDl(this._auditConnString);
            BizStagingDataLayer dl = new BizStagingDataLayer(true, this.connectionString);
             //TableAdapters adapter = new TableAdapters("trans", true,this.connectionString);

             try
             {
                // adapter.createTransaction();


                 foreach (TransInput data in transDatas)
                 {
                     DataRow[] rows = currentTable.Select("TransID = " + data.transId);
                     //auditData.documentId = int.Parse(ro);

                     foreach (DataRow aRow in rows)
                     {

                         rowIndex = currentTable.Rows.IndexOf(aRow);
                         dl.saveData(data.status, currentTable.Rows[rowIndex]["Status"].ToString(), int.Parse(currentTable.Rows[rowIndex]["TransID"].ToString()));
                         //currentTable.Rows[rowIndex]["ONYX_StagingStatus"] = data.status;
                         auditData.documentId = aRow["Document ID"].ToString();
                         auditDl.saveTransAudit(auditData);
                         // transAuditDl.saveData(auditData,(csuAuditTransSyncTableAdapter)adapter.getAdapter());
                     }
                 }


                 //dl.saveChanges();
                 //dl.saveData(currentTable);


                 auditDl.commitChanges();  
                 dl.saveChanges();

                 

                 //adapter.executeTrans();
                 

                 auditDl.Dispose();
                 dl.Dispose();
                // adapter.Dispose();

                // adapter = null;
                 dl = null;
                 auditDl = null;
             }
             catch (Exception exception)
             {
                 dl.rejectChanges();

                 auditDl.Dispose();
                 dl.Dispose();

                 dl = null;
                 auditDl = null;

                 throw exception;

             }
            
            
            
            

            //DataTable aTable;
            //return getAllParsedTrans(transStatus,fromDate,toDate, out outputTable);
        }

        public void Dispose()
        {
            this._dataTable = null;
        }
    }
}
