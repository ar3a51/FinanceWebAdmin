using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppDomain.AuditDomain
{
    public class AuditDebtorSync
    {
        public int auditId { set; get; }
        public String modifiedBy { set; get; }
        public DateTime updateDate { set; get; }
        public String action { set; get; }
        public int MemberId { set; get; }
        public String entityType { set; get; } //Individual or Company
        public String reason { set; get; }
    }
}
