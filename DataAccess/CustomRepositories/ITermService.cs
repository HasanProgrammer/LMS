﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.CustomClasses;
using Model;
using Model.Bases;

namespace DataAccess.CustomRepositories
{
    /*Configs*/
    public partial interface ITermService<TViewModel, TModel> : IRepository<TViewModel, TModel> where TViewModel : IViewEntity where TModel : IEntity
    {
        
    }
    
    /*ViewModel*/
    public partial interface ITermService<TViewModel, TModel>
    {
        public Task<TViewModel> FindWithIdAsNoTrackingAsync(int id, User user); /*این متد برای نمایش جزئیات یک دوره برنامه نویسی مورد استفاده قرار میگیرد ، با در نظر گرفتن کاربر که آیا این دوره توسط کاربر خریداری شده است یا خیر*/
        public Task<TViewModel> FindWithIdAsNoTrackingAndActiveAsync(int id, User user);
        public Task<PaginatedList<TViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count);
        public Task<PaginatedList<TViewModel>> FindAllWithNoTrackingAndActiveForCategoryAndPaginateAsync(string category, int page, int count);
        public Task<List<TViewModel>> FindAllLastPublishWithNoTrackingAndActiveAsync(int count);
        public Task<List<TViewModel>> FindAllLastPublishWithNoTrackingAndActiveForCategoryAsync(string categoey, int count);
        public Task<List<TViewModel>> FindAllByTitleWithNoTrackingAndActiveAsync(string title);
        public Task<List<TViewModel>> FindAllByTitleForCategoryWithNoTrackingAndActiveAsync(string category, string title);
        public Task<List<TViewModel>> FindAllOwnedWithNoTrackingAsync(User user);
    }
    
    /*Model*/
    public partial interface ITermService<TViewModel, TModel>
    {
        public Task<List<TModel>> FindAllEntityForUserWithNoTrackingAndActiveAsync(User user);
        public TModel FindWithNameEntityWithNoTracking(string name);
        public Task<TModel> FindWithNameEntityWithNoTrackingAsync(string name);
        
        public Task<(int, int, int, int, int, int, int, int, int, int)> FindCountEntityForCategoryWithNoTrackingAsync(string csh, string php, string python, string js, string asp, string laravel, string django, string reactjs, string sql, string mysql);
    }
}