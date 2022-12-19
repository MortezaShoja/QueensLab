using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Queenslab.Models
{
    public class Services
    {
        public int ID { get; set; }
        public string Service_Name { get; set; }
        public decimal Default_Price { get; set; }
        public bool Active_Only_On_WorkingDays { get; set; }
    }
}