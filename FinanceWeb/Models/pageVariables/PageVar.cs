using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinanceWeb.Models.pageVariables
{
    public class PageVar
    {
        public String btnSubmitId { set; get; }
        public String btnResetId { set; get; }
        public String btnSelectId { set; get; }
        public String inputFromId { set; get; }
        public String inputToId { set; get; }
        public SelectView[] selectViews { set; get; }
    }
}