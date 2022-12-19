using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Queenslab.Models
{
    public class Calculator
    {
        public partial class Request
        {
            public int Customer_ID { get; set; }
            public DateTime Start_Date { get; set; }
            public DateTime End_Date { get; set; }
        }
    }
}