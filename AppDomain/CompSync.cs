﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppDomain
{
    public class CompSync
    {
        public int sync_Id { set; get; }
        public String batch_id { set; get; }
        public int company_id { set; get; }
        public String insert_update { set; get; }
        public DateTime updateDate { set; get; }
        public Byte recordStatus { set; get; }
    }
}
