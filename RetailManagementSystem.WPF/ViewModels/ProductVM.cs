using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailManagementSystem.WPF.Models;

namespace RetailManagementSystem.WPF.ViewModels
{
    public class ProductVM:Utils.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public string ProductAvailability
        {
            get => _pageModel.ProductStatus;
            set
            {
                _pageModel.ProductStatus = value;
                OnPropertyChanged(nameof(ProductAvailability));
            }
        }


            public ProductVM(){
            _pageModel= new PageModel();
            ProductAvailability = "Out of Stock";
        }
    }
}
