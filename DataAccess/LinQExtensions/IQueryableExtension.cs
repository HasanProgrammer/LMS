using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.CustomClasses;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.LinQExtensions
{
    public static class IQueryableExtension
    {
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> Source, int CountSizePerPage, int CurrentPageNumber)
        {
            List<T> data = Source.Skip((CurrentPageNumber - 1) * CountSizePerPage).Take(CountSizePerPage).ToList();
            return new PaginatedList<T>(data, Source.Count(), CountSizePerPage, CurrentPageNumber);
        }
        
        public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> Source, int CountSizePerPage, int CurrentPageNumber)
        {
            List<T> data = await Source.Skip((CurrentPageNumber - 1) * CountSizePerPage).Take(CountSizePerPage).ToListAsync();
            return new PaginatedList<T>(data, Source.Count(), CountSizePerPage, CurrentPageNumber);
        }
    }
}