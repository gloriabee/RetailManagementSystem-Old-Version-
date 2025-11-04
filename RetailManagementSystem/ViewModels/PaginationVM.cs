using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RetailManagementSystem.ViewModels
{
    public class PaginationVM:ViewModelBase
    {
        private int _currentPage=1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanGoPrevious));
                    OnPropertyChanged(nameof(CanGoNext));

                }

            }
        }

       

        
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set
            {

                _pageSize = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(CanGoNext));
                CurrentPage = 1; 

            }
        }

        private int _totalCount;
        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(CanGoNext));
            }
        }


        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public List<int> PageSizes { get; } = new List<int> { 10,25,50,100};

        public bool CanGoPrevious => CurrentPage > 1;
        public bool CanGoNext => CurrentPage < TotalPages;

        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }
        public ICommand ChangePageSizeCommand { get; }

        public PaginationVM()
        {
            FirstPageCommand = new RelayCommand(_ => CurrentPage = 1);
            PreviousPageCommand = new RelayCommand(_ => {
                if (CanGoPrevious)
                    CurrentPage--;

            });
            NextPageCommand = new RelayCommand(_ => {if (CanGoNext) CurrentPage++; });
            LastPageCommand = new RelayCommand(_ => CurrentPage = TotalPages);
            ChangePageSizeCommand = new RelayCommand(param =>
            {
                if (param == null) return;
                if (int.TryParse(param.ToString(), out int newSize) && newSize > 0)
                {
                    PageSize = newSize;
                    
                }
            });
        }


    }
}
