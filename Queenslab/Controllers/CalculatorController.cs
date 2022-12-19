using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Queenslab.Models;

namespace Queenslab.Controllers
{
    [Authorize]
    public class CalculatorController : ApiController
    {
        List<Services> default_services_db = new List<Services>();
        List<Customers.Customer> customers_db = new List<Customers.Customer>();

        /// <summary>
        /// Calculate Total Price
        /// </summary>
        public CalculatorController()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("sv-SE");

            // Start Creating Default Services DB

            default_services_db.Add(new Services {
                ID = 1,
                Service_Name = "Service A",
                Default_Price = decimal.Parse("0,2"),
                Active_Only_On_WorkingDays = true
            });
            default_services_db.Add(new Services
            {
                ID = 2,
                Service_Name = "Service B",
                Default_Price = decimal.Parse("0,24"),
                Active_Only_On_WorkingDays = true
            });
            default_services_db.Add(new Services
            {
                ID = 3,
                Service_Name = "Service C",
                Default_Price = decimal.Parse("0,4"),
                Active_Only_On_WorkingDays = false
            });
            // End Creating Default Services DB



            // Start Creating Customers DB
            

            List<Customers.ServicesDto> customer_X_services = new List<Customers.ServicesDto>();


            // Adding Customer A to Customers DB----------------------------------
            customer_X_services.Add(new Customers.ServicesDto
            {
                Service_Name = "Service A"
            });

            customer_X_services.Add(new Customers.ServicesDto {
                Service_Name = "Service C",
                Discount = new Customers.DiscountDto {
                    Discount_In_Percent = 20,
                    Start_Date = DateTime.Parse("2019-09-22"),
                    End_Date = DateTime.Parse("2019-09-24")
                },
                Specific_Price = 0
            });

            //Add Customer X to customers DB---------
            customers_db.Add(new Customers.Customer
            {
                ID = 1,
                Customer_Name = "X",
                Number_Of_Free_Days = 0,
                Services = customer_X_services
            });
            //-------------------------------------------------------------

            // Adding Customer Y to Customers DB----------------------------------
            List<Customers.ServicesDto> customer_Y_services = new List<Customers.ServicesDto>();
            customer_Y_services.Add(new Customers.ServicesDto
            {
                Service_Name = "Service B",
                Discount = new Customers.DiscountDto
                {
                    Discount_In_Percent = 30
                }
            });
            customer_Y_services.Add(new Customers.ServicesDto
            {
                Service_Name = "Service C",
                Discount = new Customers.DiscountDto
                {
                    Discount_In_Percent = 30
                }
            });
            // End Creating Customers DB

            //Add Customer Y to customers DB---------
            customers_db.Add(new Customers.Customer
            {
                ID = 2,
                Customer_Name = "Y",
                Number_Of_Free_Days = 200,
                Services = customer_Y_services
            });
        }

        // GET api/<controller>
        /// <summary>
        /// "Customer_ID" : 0,
        /// "Start_Date": "",
        /// "End_Date": ""
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public decimal Get(Calculator.Request data)
        {
            decimal result = 0;

            var customer = customers_db.Where(w => w.ID == data.Customer_ID).FirstOrDefault();

            foreach (var service in customer.Services)
            {
                int number_of_free_days = customer.Number_Of_Free_Days;

                var default_service_info = default_services_db.Where(w => w.Service_Name == service.Service_Name).FirstOrDefault();

                decimal service_price = 0;

                

                for (var day = data.Start_Date; day.Date <= data.End_Date; day = day.AddDays(1))
                {
                    service_price = default_service_info.Default_Price;
                    if (service.Specific_Price > 0)
                    {
                        service_price = service.Specific_Price;
                    }


                    if (service.Discount != null)
                    {
                        decimal discount = 0;

                        if((service.Discount.Start_Date == DateTime.Parse("0001-1-1 00:00:00") || day >= service.Discount.Start_Date ) &
                            (service.Discount.End_Date == DateTime.Parse("0001-1-1 00:00:00") || day <= service.Discount.End_Date  ))
                        {
                            discount = service.Discount.Discount_In_Percent;
                        }

                        if(discount != 0)
                        {
                            service_price = service_price - ((service_price * discount) / 100);
                        }
                    }

                    if(number_of_free_days == 0)
                    {
                        if (default_service_info.Active_Only_On_WorkingDays)
                        {
                            if (day.DayOfWeek == DayOfWeek.Monday ||
                                day.DayOfWeek == DayOfWeek.Tuesday ||
                                day.DayOfWeek == DayOfWeek.Wednesday ||
                                day.DayOfWeek == DayOfWeek.Thursday ||
                                day.DayOfWeek == DayOfWeek.Friday)
                            {
                                result += service_price;
                            }
                        }
                        else
                        {
                            result += service_price;
                        }
                    } else
                    {
                        number_of_free_days -= 1;
                    } 
                }
            }

            return result;
        }


    }
}