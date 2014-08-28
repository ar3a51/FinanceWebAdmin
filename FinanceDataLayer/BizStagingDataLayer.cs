using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
/*using System.Data.Sql;
using System.Data.SqlClient;*/
using AppDomain;
using FinanceDataLayer.FinanceDataTableAdapters;


namespace FinanceDataLayer
{
    public class BizStagingDataLayer: IDisposable
    {
        private csuBIZStagingTransTableAdapter _stagingDA;
        private OleDbParameter totalRecordsParam;
        private OleDbConnection connection;
        private OleDbCommand command;

        private Boolean _istransactionNeeded;

            public BizStagingDataLayer(Boolean isTransactionneeded, String connectionString="")
            {

               
                this._stagingDA = new csuBIZStagingTransTableAdapter();
                
                this._istransactionNeeded = isTransactionneeded;

                if (!connectionString.Equals(""))
                {
                    this._stagingDA.Connection.ConnectionString = connectionString;
                    this.connection = new OleDbConnection(connectionString);
                }

                
               this.initUpdateStatement();
               this.initSelectStatement();
               this.initSqlConnection();

              
                

                if (isTransactionneeded)
                {
                    this._stagingDA.Adapter.UpdateCommand.Connection.Open();
                    this._stagingDA.Adapter.UpdateCommand.Transaction = this._stagingDA.Adapter.UpdateCommand.Connection.BeginTransaction();
                }


                    
            }

            public DataTable getAllBizStagingData(int iStartRow, int recsPerPage, String transStatus, DateTime fromDate, DateTime toDate, String keyword, out int? totalRecords) {

                totalRecords = 0;
                 DataTable dataTable = new DataTable();
                 dataTable.Columns.Add("TransID");
                 dataTable.Columns.Add("Document ID");
                 dataTable.Columns.Add("Status");
                 dataTable.Columns.Add("Document type");
                 dataTable.Columns.Add("Member ID");
                 dataTable.Columns.Add("LNE_AMT1");
                 dataTable.Columns.Add("Date Created");
                //return this._stagingDA.GetData(iStartRow, recsPerPage, transStatus, fromDate, toDate, ref totalRecords);
                //this._stagingDA.Fill(dataTable2, iStartRow, recsPerPage, transStatus, fromDate, toDate, ref totalRecords);
                this._stagingDA.Adapter.SelectCommand.Parameters["@iStartRow"].Value = iStartRow;
                this._stagingDA.Adapter.SelectCommand.Parameters["@RecsPerPage"].Value = recsPerPage;
                this._stagingDA.Adapter.SelectCommand.Parameters["@transStatus"].Value = transStatus;
                this._stagingDA.Adapter.SelectCommand.Parameters["@transStartDate"].Value = fromDate;
                this._stagingDA.Adapter.SelectCommand.Parameters["@transEndDate"].Value = toDate;
                this._stagingDA.Adapter.SelectCommand.Parameters["@keyword"].Value = keyword;
                //this._stagingDA.Adapter.UpdateCommand.Parameters["@TotalRecords"].Value = oldStatus;

                this._stagingDA.Adapter.SelectCommand.Connection.Open();
                OleDbDataReader reader = this._stagingDA.Adapter.SelectCommand.ExecuteReader();
                while (reader.Read())
                {
                    DataRow newRow = dataTable.NewRow();
                    newRow["TransID"] = reader["ONYX_iTranID"].ToString();
                    newRow["Document ID"] = reader["DOC_REF1"].ToString();
                    newRow["Status"] = reader["ONYX_StagingStatus"].ToString();
                    newRow["Document Type"] = reader["BAT_DOC_TYPE"].ToString();
                    newRow["Member ID"] = reader["ONYX_iOwnerId"].ToString();
                    newRow["LNE_AMT1"] = reader["LNE_AMT1"].ToString();
                    newRow["Date Created"] = DateTime.Parse(reader["ONYX_dtTransactionDate"].ToString());

                    dataTable.Rows.Add(newRow);
                }

                

                this._stagingDA.Adapter.SelectCommand.Connection.Close();

                totalRecords = (int?)totalRecordsParam.Value;
                return dataTable;
                                                    
            }

            public bizSync getTransDetails(int transId)
            {
                bizSync transDetails = null;
                this.command.Parameters["@iTranId"].Value = transId;

                this.connection.Open();

                OleDbDataReader reader = this.command.ExecuteReader();

                while (reader.Read())
                {
                    transDetails = new bizSync
                    {
                        ONYX_iTransId = int.Parse(reader["ONYX_iTranID"].ToString()),
                        DOC_ID = reader["DOC_REF1"].ToString(),
                        ONYX_iOwnerId = int.Parse(reader["ONYX_iOwnerID"].ToString()),
                        ONYX_dtTransactionDate = DateTime.Parse(reader["ONYX_dtTransactionDate"].ToString()),
                        ONYX_StagingStatus = reader["ONYX_StagingStatus"].ToString(),
                        ONYX_StagingErrorDescription = reader["ONYX_StagingErrorDescription"].ToString()
                    };
                }

                this.connection.Close();

                return transDetails;
            }

            public void saveData(String transStatus,String oldStatus, int transId) {

                //this._stagingDA.Update((FinanceData.csuBIZStagingTransDataTable)newTable);
                this._stagingDA.Adapter.UpdateCommand.Parameters["@STATUS"].Value = transStatus;
                this._stagingDA.Adapter.UpdateCommand.Parameters["@TRANID"].Value = transId;
                this._stagingDA.Adapter.UpdateCommand.Parameters["@OLDSTATUS"].Value = oldStatus;

                this._stagingDA.Adapter.UpdateCommand.ExecuteNonQuery();
               
            }

            public void saveChanges()
            {
                this._stagingDA.Adapter.UpdateCommand.Transaction.Commit();
               
            }

            public void rejectChanges()
            { this._stagingDA.Adapter.UpdateCommand.Transaction.Rollback(); }

         
            public void Dispose()
            {
                if (this._istransactionNeeded)
                    this._stagingDA.Adapter.UpdateCommand.Connection.Close();

                //this.command.Dispose();
                this.connection.Dispose();

                this.command = null;
                this.connection = null;

                this._stagingDA = null;
            }

            private void initSelectStatement()
            {

                 this._stagingDA.Adapter.SelectCommand = new System.Data.OleDb.OleDbCommand();
                this._stagingDA.Adapter.SelectCommand.Connection = this._stagingDA.Connection;
                this._stagingDA.Adapter.SelectCommand.CommandText = "GetInCATransList";
                this._stagingDA.Adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                this._stagingDA.Adapter.SelectCommand.Parameters.Add("@iStartRow", System.Data.OleDb.OleDbType.Integer);
                this._stagingDA.Adapter.SelectCommand.Parameters.Add("@RecsPerPage", System.Data.OleDb.OleDbType.Integer);
                this._stagingDA.Adapter.SelectCommand.Parameters.Add("@transStatus", System.Data.OleDb.OleDbType.VarChar);
                this._stagingDA.Adapter.SelectCommand.Parameters.Add("@transStartDate", System.Data.OleDb.OleDbType.Date);
                this._stagingDA.Adapter.SelectCommand.Parameters.Add("@transEndDate", System.Data.OleDb.OleDbType.Date);
                this._stagingDA.Adapter.SelectCommand.Parameters.Add("@keyword", System.Data.OleDb.OleDbType.VarChar);
                 totalRecordsParam = this._stagingDA.Adapter.SelectCommand.Parameters.Add("@TotalRecords", System.Data.OleDb.OleDbType.Integer);
                 totalRecordsParam.Direction = ParameterDirection.Output;

            }

            private void initUpdateStatement()
            {
                this._stagingDA.Adapter.UpdateCommand = new System.Data.OleDb.OleDbCommand();
                this._stagingDA.Adapter.UpdateCommand.Connection = this._stagingDA.Connection;
                this._stagingDA.Adapter.UpdateCommand.CommandText = "UpdateInCATransStatus";
                this._stagingDA.Adapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
                this._stagingDA.Adapter.UpdateCommand.Parameters.Add("@STATUS", System.Data.OleDb.OleDbType.VarChar);
                this._stagingDA.Adapter.UpdateCommand.Parameters.Add("@TRANID", System.Data.OleDb.OleDbType.Integer);
                this._stagingDA.Adapter.UpdateCommand.Parameters.Add("@OLDSTATUS", System.Data.OleDb.OleDbType.VarChar);
            }

            private String createDetailsStatement()
            {
                String sqlStatement;

                sqlStatement = "SELECT ONYX_iTranID, DOC_REF1,"; 
                sqlStatement += "ONYX_iOwnerID, ONYX_dtTransactionDate, ONYX_StagingErrorCode, ONYX_StagingErrorDescription, ONYX_StagingUpdateDate, ONYX_StagingStatus ";
                sqlStatement += "FROM csuBIZStagingTrans ";
                sqlStatement += "WHERE ONYX_iTranID=?";

                return sqlStatement;
            }

            private void initSqlConnection()
            {
                this.command = new OleDbCommand(this.createDetailsStatement(), this.connection);
                this.command.Parameters.Add("@iTranId", SqlDbType.Int).Direction = ParameterDirection.Input;
            }
    }
}
