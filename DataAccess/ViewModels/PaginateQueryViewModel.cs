using Microsoft.Extensions.Configuration;

namespace DataAccess.ViewModels
{
    public class PaginateQueryViewModel
    {
        private const int MaxCountSizePerPage = 20;
        
        /*-----------------------------------------------------------*/

        private int _CountSizePerPage = 3;

        /*-----------------------------------------------------------*/

        public int PageNumber { get; set; } = 1;

        public int CountSizePerPage
        {
            get => _CountSizePerPage;
            set
            {
                _CountSizePerPage = (value > MaxCountSizePerPage) ? MaxCountSizePerPage : value;
            }
        }
    }
}