using Microsoft.AspNetCore.Mvc;
using PriceAlertAPI.Models;
using PriceAlertAPI.Service;

namespace PriceAlertAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceAlertController : ControllerBase
    {
        private IConfiguration _configuration { get; }

        public PriceAlertController(IConfiguration configuration) {
            _configuration = configuration;
        }

        [HttpGet("setLowPriceAlert/{username}")]
        public async Task SetLowPriceAlert(string username, [FromQuery] string ticker, [FromQuery] decimal priceLimit) {
            IPriceAlertService priceAlertService = new PriceAlertService(_configuration);
            await priceAlertService.SetLowPriceAlertAsync(username, ticker, priceLimit);
        }

        [HttpGet("getAlerts")]
        public async Task<ActionResult<IList<PriceAlert>>> GetAlerts() {
            IPriceAlertService priceAlertService = new PriceAlertService(_configuration);
            IList<PriceAlert> alerts = await priceAlertService.GetAlertsAsync();
            return new OkObjectResult(alerts);
        }

        [HttpGet("checkAlerts/{ticker}")]
        public async Task<ActionResult<IList<PriceAlert>>> CheckAlerts(string ticker, [FromQuery] decimal priceLimit) {
            IPriceAlertService priceAlertService = new PriceAlertService(_configuration);
            IList<PriceAlert> alerts = await priceAlertService.CheckAlertsAsync(ticker, priceLimit);
            return new OkObjectResult(alerts);
        }

        [HttpDelete("removeAlert/{username}")]
        public async Task RemoveAlert(string username, string ticker)
        {
            IPriceAlertService priceAlertService = new PriceAlertService(_configuration);
            await priceAlertService.RemoveAlertAsync(username, ticker);
        }
    }
}
