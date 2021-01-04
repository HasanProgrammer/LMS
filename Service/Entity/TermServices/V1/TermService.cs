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

namespace Service.Entity.TermServices.V1
{
    /*Configs*/
    public partial class TermService : ITermService<TermsViewModel, Term>
    {
        private readonly DatabaseContext _Context;

        public TermService(IServiceProvider ServiceProvider)
        {
            _Context = ServiceProvider.GetRequiredService<DatabaseContext>();
        }
    }
    
    /*ViewModel*/
    public partial class TermService
    {
        public IEnumerable<TermsViewModel> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TermsViewModel> FindAllWithNoTracking()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TermsViewModel> FindAllWithPaginate(int page, int count)
        {
            throw new NotImplementedException();
        }

        public PaginatedList<TermsViewModel> FindAllWithNoTrackingAndPaginate(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Task<List<TermsViewModel>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TermsViewModel>> FindAllWithPaginateAsync(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Task<List<TermsViewModel>> FindAllWithNoTrackingAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<TermsViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            throw new NotImplementedException();
        }

        public TermsViewModel FindWithId(int? id)
        {
            throw new NotImplementedException();
        }

        public TermsViewModel FindWithIdAsNoTracking(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<TermsViewModel> FindWithIdAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<TermsViewModel> FindWithIdAsNoTrackingAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TermsViewModel> FindAllActive()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TermsViewModel> FindAllActiveAsNoTracking()
        {
            throw new NotImplementedException();
        }

        public PaginatedList<TermsViewModel> FindAllActiveWithPaginate(int page, int count)
        {
            throw new NotImplementedException();
        }

        public PaginatedList<TermsViewModel> FindAllActiveAsNoTrackingWithPaginate(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Task<List<TermsViewModel>> FindAllActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TermsViewModel>> FindAllActiveAsNoTrackingAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TermsViewModel>> FindAllActiveWithPaginateAsync(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<TermsViewModel>> FindAllActiveAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new NotImplementedException();
        }

        public TermsViewModel FindWithIdActive(int? id)
        {
            throw new NotImplementedException();
        }

        public TermsViewModel FindWithIdActiveAsNoTracking(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<TermsViewModel> FindWithIdActiveAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<TermsViewModel> FindWithIdActiveAsNoTrackingAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public TermsViewModel ConvertToViewModel(Term model)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<TermsViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count)
        {
            throw new NotImplementedException();
        }
    }
    
    /*Model*/
    public partial class TermService
    {
        public IEnumerable<Term> FindAllEntity()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Term> FindAllEntityWithNoTracking()
        {
            throw new NotImplementedException();
        }

        public PaginatedList<Term> FindAllEntityWithPaginate(int page, int count)
        {
            throw new NotImplementedException();
        }

        public PaginatedList<Term> FindAllEntityWithNoTrackingAndPaginate(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Task<List<Term>> FindAllEntityAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Term>> FindAllEntityWithNoTrackingAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<Term>> FindAllEntityWithPaginateAsync(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<Term>> FindAllEntityWithAsNoTrackingAndPaginateAsync(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Term FindWithIdEntity(int? id)
        {
            throw new NotImplementedException();
        }

        public Term FindWithIdEntityAsNoTracking(int? id)
        {
            throw new NotImplementedException();
        }

        public Term FindWithIdEntityWithEagerLoading(int? id)
        {
            throw new NotImplementedException();
        }

        public Term FindWithIdEntityWithEagerLoadingAsNoTracking(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Term> FindWithIdEntityAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Term> FindWithIdEntityAsNoTrackingAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Term> FindWithIdEntityWithEagerLoadingAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Term> FindWithIdEntityWithEagerLoadingAsNoTrackingAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Term> FindAllActiveEntity()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Term> FindAllActiveEntityAsNoTracking()
        {
            throw new NotImplementedException();
        }

        public PaginatedList<Term> FindAllActiveEntityWithPaginate(int page, int count)
        {
            throw new NotImplementedException();
        }

        public PaginatedList<Term> FindAllActiveEntityAsNoTrackingWithPaginate(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Task<List<Term>> FindAllActiveEntityAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Term>> FindAllActiveEntityAsNoTrackingAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<Term>> FindAllActiveEntityWithPaginateAsync(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<Term>> FindAllActiveEntityAsNoTrackingWithPaginateAsync(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Term FindWithIdActiveEntity(int? id)
        {
            throw new NotImplementedException();
        }

        public Term FindWithIdActiveEntityAsNoTracking(int? id)
        {
            throw new NotImplementedException();
        }

        public Term FindWithIdActiveEntityAsNoTrackingWithEagerLoading(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Term> FindWithIdActiveEntityAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Term> FindWithIdActiveEntityAsNoTrackingAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Term> FindWithIdActiveEntityAsNoTrackingWithEagerLoadingAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Term FindWithNameEntityWithNoTracking(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Term> FindWithNameEntityWithNoTrackingAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}