using AppDomain;
using AppDomain.AuditDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using FinanceDataLayer;
using FinanceDataLayer.FinanceDataTableAdapters;
using FinanceDataLayer.auditDataLayer;


namespace FinanceWeb.Models
{
    public class IndividualSyncModel:IDisposable
    {
        private DataTable _IndivSyncTable;
        private String connectionString;
        private String _auditConnString;

        public IndividualSyncModel(String connectionString="", String auditconnString="")
        { 
            this._IndivSyncTable = null;
            this.connectionString = connectionString;
            this._auditConnString = auditconnString;
        }

        public DataTable getAllIndividualSyncList()
        {
           // if (this._IndivSyncTable.GetType() == null)
           // {
            using (IndividualSyncDataLayer indivDl = new IndividualSyncDataLayer(false, this.connectionString))
                {
                    this._IndivSyncTable = indivDl.getAllData();
                }
           // }


            return this._IndivSyncTable;

        }

        public DataTable editData(indivSync[] indivSyncData)
        {
            int rowIndex;

            foreach (indivSync data in indivSyncData)
            {
               DataRow[] rows = this._IndivSyncTable.Select("sync_id = " + data.sync_Id);

               foreach (DataRow aRow in rows)
               {
                   rowIndex = this._IndivSyncTable.Rows.IndexOf(aRow);
                   this._IndivSyncTable.Rows[rowIndex]["batch_id"] = data.batch_id;
                   this._IndivSyncTable.Rows[rowIndex]["individual_id"] = data.individual_id;
                   this._IndivSyncTable.Rows[rowIndex]["insert_update"] = data.insert_update;
                   this._IndivSyncTable.Rows[rowIndex]["update_date"] = data.updateDate;
                   this._IndivSyncTable.Rows[rowIndex]["record_status"] = data.recordStatus;
               }
            }

            using (IndividualSyncDataLayer dl = new IndividualSyncDataLayer(true, this.connectionString))
            {
                dl.addNewData(this._IndivSyncTable);
                dl.commitChanges();

            }

            return this._IndivSyncTable;
        }

        public DataTable addData(indivSync indivSyncData, DataTable currentTable,AuditDebtorSync auditData)
        {

                 DataRow newRow = currentTable.NewRow();

                newRow["batch_id"] = null;
                newRow["individual_id"] = indivSyncData.individual_id;
                newRow["insert_update"] = indivSyncData.insert_update;
                newRow["update_date"] = DateTime.Now;
                newRow["record_status"] = indivSyncData.recordStatus;

                currentTable.Rows.Add(newRow);
                
                IndividualSyncDataLayer indivDl;
                AuditDl auditdl;

                indivDl = new IndividualSyncDataLayer(true, this.connectionString);
                auditdl = new AuditDl(this._auditConnString);

                try
                {
                     

                    indivDl.addNewData(currentTable);
                    auditdl.saveDebtorSyncAudit(auditData);

                    auditdl.commitChanges();
                    indivDl.commitChanges();

                    indivDl.Dispose();
                    auditdl.Dispose();

                    indivDl = null;
                    auditdl = null;
                    
                }
                catch (Exception exception)
                {
                    indivDl.rejectChanges();

                    indivDl.Dispose();
                    auditdl.Dispose();

                    indivDl = null;
                    auditdl = null;

                    throw exception;
                    
                }
                
                

            return getAllIndividualSyncList();
        }

        public Boolean checkIndividualId(int individualId)
        {
            int numberOfRows = 0;

            using (IndividualSyncDataLayer indivDl = new IndividualSyncDataLayer(false, this.connectionString))
            {
                numberOfRows = indivDl.getUserFromId(individualId).Rows.Count;
            }

            if (numberOfRows > 0)
                return true;
            else
                return false;
        }

        public void Dispose()
        {
            this._IndivSyncTable = null;
        }
    }
}