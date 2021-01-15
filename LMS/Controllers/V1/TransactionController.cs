using System.Threading.Tasks;
using Common;
using DataAccess.CustomClasses;
using DataAccess.CustomRepositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using WebFramework.Extensions;

namespace LMS.Controllers.V1
{
    [ApiVersion(version: "1.0")]
    public class TransactionController : BaseTransactionController
    {
        //Services
        private readonly TransactionService<TransactionsViewModel, Transaction> _TransactionService;
        
        //Configs
        private readonly Config.StatusCode _StatusCode;
        private readonly Config.Messages   _StatusMessage;
        
        //Managers
        private readonly UserManager<User> _UserManager;
        
        public TransactionController (
            TransactionService<TransactionsViewModel, Transaction> TransactionService,
            UserManager<User>           UserManager,
            IOptions<Config.StatusCode> StatusCode, 
            IOptions<Config.Messages>   StatusMessage 
        )
        {
            //Services
            _TransactionService = TransactionService;
            
            //Configs
            _StatusCode    = StatusCode.Value;
            _StatusMessage = StatusMessage.Value;
            
            //Managers
            _UserManager = UserManager;
        }

        [HttpGet]
        [Route(template: "", Name = "Transaction.All.Paginate")]
        public async Task<JsonResult> Index([FromQuery] PaginateQueryViewModel model)
        {
            PaginatedList<TransactionsViewModel> Transactions = await _UserManager.HasRoleAsync(Request.HttpContext, "Admin") ?
                                                                await _TransactionService.FindAllWithNoTrackingAndPaginateAsync(model.PageNumber, model.CountSizePerPage) :
                                                                await _TransactionService.FindAllForUserWithNoTrackingAndPaginateAsync(await _UserManager.GetCurrentUserAsync(Request.HttpContext), model.PageNumber, model.CountSizePerPage);

            JsonResponse.Handle(Request.HttpContext, "X-Pagination", new
            {
                Transactions.CurrentPage,
                Transactions.CountSizePerPage,
                Transactions.TotalPages,
                Transactions.HasNext,
                Transactions.HasPrev
            });
            
            return JsonResponse.Return(_StatusCode.SuccessFetchData, _StatusMessage.SuccessFetchData, new { Transactions });
        }
    }
}