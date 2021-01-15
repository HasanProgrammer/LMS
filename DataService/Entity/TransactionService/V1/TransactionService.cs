using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.LinQExtensions;
using DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model;

namespace DataService.Entity.TransactionService.V1
{
    /*Configs*/
    public partial class TransactionService : TransactionService<TransactionsViewModel, Transaction>
    {
        //Context
        private readonly DatabaseContext _Context;
        
        //Config
        private readonly IConfiguration _Config;
        
        public TransactionService(DatabaseContext Context, IConfiguration Config)
        {
            //Context
            _Context = Context;
            
            //Config
            _Config = Config;
        }
    }
    
    /*ViewModel*/
    public partial class TransactionService
    {
        public override async Task<PaginatedList<TransactionsViewModel>> FindAllWithNoTrackingAndPaginateAsync(int page, int count)
        {
            return await _Context.Transactions.AsNoTracking().OrderByDescending(Tran => Tran.Id).Select(Tran => new TransactionsViewModel
            {
                UserName   = Tran.User.UserName,
                UserImage  = Tran.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Tran.User.Image.Path }" : null,
                TermName   = Tran.Term.Name,
                RefId      = Tran.RefId,
                UserEmail  = Tran.UserEmail,
                UserPhone  = Tran.UserPhone,
                Status     = Tran.Status == Model.Enums.Transaction.Status.Active ? "تایید شده" : "تایید نشده",
                DateCreate = Tran.CreatedAt,
                Price      = Tran.Price
            }).ToPaginatedListAsync(count, page);
        }
        
        public override async Task<PaginatedList<TransactionsViewModel>> FindAllForUserWithNoTrackingAndPaginateAsync(User user, int page, int count)
        {
            return await _Context.Transactions.AsNoTracking().OrderByDescending(Tran => Tran.Id).Where(Tran => Tran.Term.UserId.Equals(user.Id)).Select(Tran => new TransactionsViewModel
            {
                UserName   = Tran.User.UserName,
                UserImage  = Tran.User.Image != null ? $"{ _Config.GetValue<string>("File:UploadPathImagePublic").Replace("\\", "/") }{ Tran.User.Image.Path }" : null,
                TermName   = Tran.Term.Name,
                RefId      = Tran.RefId,
                UserEmail  = Tran.UserEmail,
                UserPhone  = Tran.UserPhone,
                Status     = Tran.Status == Model.Enums.Transaction.Status.Active ? "تایید شده" : "تایید نشده",
                DateCreate = Tran.CreatedAt,
                Price      = Tran.Price
            }).ToPaginatedListAsync(count, page);
        }
    }
    
    /*Model*/
    public partial class TransactionService
    {
        
    }
}