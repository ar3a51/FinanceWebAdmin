using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppDomain.AuditDomain
{
    public class AuditTransSync
    {
        public int auditId { set; get; }
        public String modifiedBy { set; get; }
        public DateTime updateDate { set; get; }
        public String action { set; get; }
        public String documentId { set; get; }
        public String system { set; get; }
        public String reason { set; get; } 

    }
}
