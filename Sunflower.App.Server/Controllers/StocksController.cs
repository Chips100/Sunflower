using Microsoft.AspNetCore.Mvc;
using Sunflower.App.Server.Application.Authentication;
using Sunflower.Business.Contracts;
using Sunflower.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sunflower.App.Server.Controllers
{
    public class StocksController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IAccountService _accountService;
        private readonly IAuthenticatorProvider _authenticatorProvider;

        public StocksController(IStockService stockService, IAccountService accountService, IAuthenticatorProvider authenticatorProvider)
        {
            _stockService = stockService;
            _accountService = accountService;
            _authenticatorProvider = authenticatorProvider;
        }

        public Task<IEnumerable<Stock>> SearchStocks(string searchTerm)
        {
            return _stockService.SearchStocks(searchTerm);
        }

        [HttpPost]
        public async Task Buy(int stockId, int sharesCount, decimal maxShareValue)
        {
            var authenticator = _authenticatorProvider.Provide(HttpContext);
            var account = await _accountService.GetAccountByEmail("dennis.janiak@googlemail.com");

            await _stockService.BuyShares(account.Id, stockId, sharesCount, maxShareValue);
        }
    }
}