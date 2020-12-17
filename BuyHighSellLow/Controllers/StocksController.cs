using BuyHighSellLow.Logic.Models.Enums;
using BuyHighSellLow.Logic.Models.Requests;
using BuyHighSellLow.Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyHighSellLow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class StocksController : ControllerBase
    {
        private readonly IStocksTransactionsService _stocksTransactionsService;

        public StocksController(IStocksTransactionsService stockTransactionsService)
        {
            _stocksTransactionsService = stockTransactionsService ?? throw new ArgumentNullException(nameof(stockTransactionsService));
        }

        /// <summary>
        /// Buys or sells stock(s)
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        // POST: api/stocks/transaction
        [HttpPost]
        [Route("transaction")]
        public async Task<IActionResult> Transaction([FromBody] StocksTransactionRequest requestModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                if(requestModel.TransactionType == (int)TransactionTypes.Buy)
                {
                    await _stocksTransactionsService.BuyStocks(requestModel);                    
                }
                else if(requestModel.TransactionType == (int)TransactionTypes.Sell)
                {
                    await _stocksTransactionsService.SellStocks(requestModel);
                }

                return Ok();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }
    }
}
