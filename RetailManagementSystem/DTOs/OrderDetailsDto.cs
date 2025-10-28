using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.DTOs
{
    public class OrderDetailsDto
    {
        // Order info 
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        // Customer info
        
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        // Product info
        public List<OrderProductDto> Products { get; set; } = new();

        // --- Summary ---
        public decimal Subtotal { get; set; }

    }
}
