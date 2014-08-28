using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppDomain
{
   public class TopClassSync
    {
       public int guid { set; get; }
       public String DocumentId { set; get; }
       public String Status { set; get; }
       public String documentType { set; get; }
       public String AccountID { set; get; }
       public DateTime DocumentDate { set; get; }
       public Decimal Amount { set; get; }
       public String StatusDesc {set;get;}
    }
}
