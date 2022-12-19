using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Queenslab.Models
{
    public class Customers
    {
        public partial class Customer
        {
            public int ID { get; set; }
            public string Customer_Name { get; set; }
            public List<ServicesDto> Services { get; set; }
            public int Number_Of_Free_Days { get; set; }

        }

        public partial class ServicesDto
        {
            public string Service_Name { get; set; }
            public decimal Specific_Price { get; set; }
            public DiscountDto Discount { get; set; }
        }

        public partial class DiscountDto
        {
            public DateTime Start_Date { get; set; }
            public DateTime End_Date { get; set; }
            public int Discount_In_Percent { get; set; }
        }
    }
}