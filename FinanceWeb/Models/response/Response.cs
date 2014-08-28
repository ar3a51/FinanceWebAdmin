using AppDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinanceWeb.Models.response
{
    public class Response
    {
        public int sEcho;
        public String iTotalRecords;
        public String iTotalDisplayRecords;
        public String[][] aaData;
    }
}