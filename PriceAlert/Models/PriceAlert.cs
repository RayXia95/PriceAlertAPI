namespace PriceAlertAPI.Models
{
    public class PriceAlert
    {
        public PriceAlert() { }
        public PriceAlert(ulong id, string username, string ticker, decimal priceLimit) {
            Id = id;
            Username = username;
            Ticker = ticker;
            PriceLimit = priceLimit;
        }

        public ulong Id { get; set; }
        public string Username { get; set; }
        public string Ticker { get; set; }
        public decimal PriceLimit {
            get => _priceLimit;
            set
            {
                if (value < 0) {
                    throw new ArgumentOutOfRangeException("Cannot set negative price limit for alert.");
                }

                _priceLimit = value;
            }
        }        

        private decimal _priceLimit;
    }
}
