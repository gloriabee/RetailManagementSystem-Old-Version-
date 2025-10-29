using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.DTOs
{
    public class TopCategoryDto
    {
        public string Category { get; set; } = string.Empty;
        public int TotalProducts { get; set; }
       
    }
}
