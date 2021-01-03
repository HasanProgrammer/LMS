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

namespace Service.Entity.CategoryServices.V1
{
    /*Configs*/
    public partial class CategoryService : ICategoryService<CategoriesViewModel, Category>
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
        public IEnumerable<CategoriesViewModel> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CategoriesViewModel> FindAllWithNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CategoriesViewModel> FindAllWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<CategoriesViewModel> FindAllWithNoTrackingAndPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<CategoriesViewModel>> FindAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<CategoriesViewModel>> FindAllWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<CategoriesViewModel>> FindAllWithNoTrackingAsync()
        {
            return await _Context.Categories.AsNoTracking().OrderByDescending(category => category.Id).Select(category => new CategoriesViewModel
            {
                Id   = category.Id,
                Name = category.Name
            }).ToListAsync();
        }

        public async Task<PaginatedList<CategoriesViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            return await _Context.Categories.AsNoTracking().OrderByDescending(category => category.Id).Select(category => new CategoriesViewModel
            {
                Id   = category.Id,
                Name = category.Name
            }).ToPaginatedListAsync(count, page);
        }

        public CategoriesViewModel FindWithId(int? id)
        {
            throw new System.NotImplementedException();
        }

        public CategoriesViewModel FindWithIdAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<CategoriesViewModel> FindWithIdAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<CategoriesViewModel> FindWithIdAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CategoriesViewModel> FindAllActive()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CategoriesViewModel> FindAllActiveAsNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<CategoriesViewModel> FindAllActiveWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<CategoriesViewModel> FindAllActiveAsNoTrackingWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<CategoriesViewModel>> FindAllActiveAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<CategoriesViewModel>> FindAllActiveAsNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<CategoriesViewModel>> FindAllActiveWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<CategoriesViewModel>> FindAllActiveAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public CategoriesViewModel FindWithIdActive(int? id)
        {
            throw new System.NotImplementedException();
        }

        public CategoriesViewModel FindWithIdActiveAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<CategoriesViewModel> FindWithIdActiveAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<CategoriesViewModel> FindWithIdActiveAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public CategoriesViewModel ConvertToViewModel(Category model)
        {
            throw new System.NotImplementedException();
        }
    }
    
    /*Model*/
    public partial class CategoryService
    {
        public IEnumerable<Category> FindAllEntity()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Category> FindAllEntityWithNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Category> FindAllEntityWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Category> FindAllEntityWithNoTrackingAndPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Category>> FindAllEntityAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Category>> FindAllEntityWithNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Category>> FindAllEntityWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Category>> FindAllEntityWithAsNoTrackingAndPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Category FindWithIdEntity(int? id)
        {
            return _Context.Categories.Find(id);
        }

        public Category FindWithIdEntityAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Category FindWithIdEntityWithEagerLoading(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Category FindWithIdEntityWithEagerLoadingAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Category> FindWithIdEntityAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Category> FindWithIdEntityAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Category> FindWithIdEntityWithEagerLoadingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Category> FindWithIdEntityWithEagerLoadingAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Category> FindAllActiveEntity()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Category> FindAllActiveEntityAsNoTracking()
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Category> FindAllActiveEntityWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public PaginatedList<Category> FindAllActiveEntityAsNoTrackingWithPaginate(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Category>> FindAllActiveEntityAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Category>> FindAllActiveEntityAsNoTrackingAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Category>> FindAllActiveEntityWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<PaginatedList<Category>> FindAllActiveEntityAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new System.NotImplementedException();
        }

        public Category FindWithIdActiveEntity(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Category FindWithIdActiveEntityAsNoTracking(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Category FindWithIdActiveEntityAsNoTrackingWithEagerLoading(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Category> FindWithIdActiveEntityAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Category> FindWithIdActiveEntityAsNoTrackingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Category> FindWithIdActiveEntityAsNoTrackingWithEagerLoadingAsync(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Category FindWithNameEntityWithNoTracking(string name)
        {
            return _Context.Categories.AsNoTracking().FirstOrDefault(Category => Category.Name.Equals(name));
        }

        public async Task<Category> FindWithNameEntityWithNoTrackingAsync(string name)
        {
            return await _Context.Categories.AsNoTracking().FirstOrDefaultAsync(Category => Category.Name.Equals(name));
        }
    }
}