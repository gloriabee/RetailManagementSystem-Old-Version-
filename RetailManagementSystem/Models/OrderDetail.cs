using System;
using System.Collections.Generic;

namespace RetailManagementSystem.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public string? DeletedBy { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public virtual Order? Order { get; set; }
}
