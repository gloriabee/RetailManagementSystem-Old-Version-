using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailManagementSystem.Models;

public partial class Product : INotifyPropertyChanged
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public string Category { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public string? DeletedBy { get; set; }

    [NotMapped]
    private bool _isSelected;
    [NotMapped]
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
