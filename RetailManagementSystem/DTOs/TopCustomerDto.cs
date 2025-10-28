using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.DTOs
{
    public class TopCustomerDto
    {
       public string Name { get; set; } = string.Empty;
       public int TotalOrders { get; set; }
       public decimal TotalSpent { get; set; }

    }
}
