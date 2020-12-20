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
        private readonly IStocksTransactionService _stocksTransactionsService;

        public StocksController(IStocksTransactionService stockTransactionsService)
        {
            _stocksTransactionsService = stockTransactionsService ?? throw new ArgumentNullException(nameof(stockTransactionsService));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("ok");
        }

        /// <summary>
        /// Buys stocks
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        // POST: api/stocks/buy
        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> Buy([FromBody] StocksTransactionRequest requestModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {                
                await _stocksTransactionsService.BuyStocks(requestModel);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Sell stocks
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        // POST: api/stocks/sell
        [HttpPost]
        [Route("sell")]
        public async Task<IActionResult> Sell([FromBody] StocksTransactionRequest requestModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                await _stocksTransactionsService.SellStocks(requestModel);
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
