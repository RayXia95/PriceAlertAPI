using PriceAlertAPI.Models;

namespace PriceAlertAPI.Service
{
    public interface IPriceAlertService
    {
        public abstract Task SetLowPriceAlertAsync(string username, string ticker, decimal priceLimit);
        public abstract Task<IList<PriceAlert>> GetAlertsAsync();
        public abstract Task<IList<PriceAlert>> CheckAlertsAsync(string ticker, decimal price);
        public abstract Task RemoveAlertAsync(string username, string ticker);
    }
}
