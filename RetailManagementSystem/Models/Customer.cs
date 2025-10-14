using System;
using System.Collections.Generic;

namespace RetailManagementSystem.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Country { get; set; } = null!;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }= DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }

    public string? CreatedBy { get; set; } = "System";

    public string? UpdatedBy { get; set; }

    public string? DeletedBy { get; set; }
}
