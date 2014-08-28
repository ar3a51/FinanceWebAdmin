using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FinanceDataLayer;
using System.Transactions;
//using FinanceDataLayer.FinanceDataTableAdapters;

namespace DataLayerTester
{
    class Program
    {
        static IndividualSyncDataLayer indivDl;
        static void Main(string[] args)
        { 
            //TransactionUtil aTrans = new TransactionUtil();
            indivDl = new IndividualSyncDataLayer(false);
            try
            {


               

               // insertNewData();

                //updateData();
                //indivDl.commitChanges();

                testDisplayData();


                //currTrans.Complete();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               // indivDl.rejectChanges();
                testDisplayData();


            }
            finally
            {
                indivDl.Dispose();
                indivDl = null;
            }
            
          
        }

        static void testDisplayData()
        {
            
            DataTable currentTable = indivDl.getAllData();

            foreach (DataRow aRow in currentTable.Rows)
            {
                foreach (DataColumn aColumn in currentTable.Columns)
                {
                    Console.WriteLine(aRow[aColumn]);
                }

                Console.WriteLine("----------------");
            }

           
            Console.ReadLine();

            
        }

        static void insertNewData()
        {
           

           
               

                DataTable resultTable = indivDl.getAllData();
                DataRow newRow = resultTable.NewRow();

                newRow["batch_id"] = null;
                newRow["individual_id"] = 160002;
                newRow["insert_update"] = "I";
               

                newRow["update_date"] = DateTime.Now;
                newRow["record_status"] = 1;


                resultTable.Rows.Add(newRow);

                indivDl.addNewData(resultTable);
               // updateData();

               
            

        }

        static void updateData()
        {
           

            DataTable resultTable = indivDl.getAllData();
            DataRow selectedRow = resultTable.Rows.Find(179815);

            selectedRow["insert_update"] = "i";

            if (selectedRow["insert_update"].ToString().ToLower() == "u")
                throw new Exception("error update");

            indivDl.updateData(selectedRow);
        }
    }
}
