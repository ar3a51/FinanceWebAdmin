using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace AppDomain
{
   public class bizSync
    {
       public Int32 ONYX_iTransId { set; get; }
       public String DOC_ID { set; get; }
       public String ONYX_StagingStatus { set; get; }
       public String BAT_DOC_TYPE { set; get; }
       public Int32 ONYX_iOwnerId { set; get; }
       public DateTime ONYX_dtTransactionDate { set; get; }
       public Decimal LNE_AMT1 { set; get; }
       public String ONYX_StagingErrorDescription { set; get; }
    }
}
