using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinanceWeb.Models.Email;

namespace FinanceWeb.Models.Tools
{
    public class Tool
    {
        public static DateTime getDate(DateTime fromInput,
                                DateTime? toInput)
        {
            if (toInput == null)
            {
                fromInput = fromInput.Add(new TimeSpan(23, 59, 59));
                return fromInput;
            }
            else
            {
                toInput = DateTime.Parse(toInput.ToString()).Add(new TimeSpan(23, 59, 59));
                return (DateTime)toInput;
            }
        }


        public static void sendException(Exception exception)
        {
            using (ErrorNotify notifier = new ErrorNotify())
            {
                notifier.sendException(exception);
            }
        }
    }

     
}