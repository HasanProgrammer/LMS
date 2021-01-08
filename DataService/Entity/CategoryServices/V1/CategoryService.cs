using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.LinQExtensions;
using DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Model;

namespace DataService.Entity.CategoryServices.V1
{
    /*Configs*/
    public partial class CategoryService : CategoryService<CategoriesViewModel, Category>
    {
        private readonly DatabaseContext _Context;

        public CategoryService(IServiceProvider ServiceProvider)
        {
            _Context = ServiceProvider.GetRequiredService<DatabaseContext>();
        }
    }
    
    /*ViewModel*/
    public partial class CategoryService
    {
        public override async Task<List<CategoriesViewModel>> FindAllWithNoTrackingAsync()
        {
            return await _Context.Categories.AsNoTracking().OrderByDescending(category => category.Id).Select(category => new CategoriesViewModel
            {
                Id   = category.Id,
                Name = category.Name
            }).ToListAsync();
        }

        public override async Task<PaginatedList<CategoriesViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            return await _Context.Categories.AsNoTracking().OrderByDescending(category => category.Id).Select(category => new CategoriesViewModel
            {
                Id   = category.Id,
                Name = category.Name
            }).ToPaginatedListAsync(count, page);
        }
    }
    
    /*Model*/
    public partial class CategoryService
    {
        public override Category FindWithIdEntity(int? id)
        {
            return _Context.Categories.Find(id);
        }

        public override Category FindWithIdEntityAsNoTracking(int? id)
        {
            return _Context.Categories.AsNoTracking().FirstOrDefault(Category => Category.Id == id);
        }

        public override Category FindWithNameEntityWithNoTracking(string name)
        {
            return _Context.Categories.AsNoTracking().FirstOrDefault(Category => Category.Name.Equals(name));
        }

        public override async Task<Category> FindWithNameEntityWithNoTrackingAsync(string name)
        {
            return await _Context.Categories.AsNoTracking().FirstOrDefaultAsync(Category => Category.Name.Equals(name));
        }
    }
}