using Newtonsoft.Json;

namespace ImportEmail.Models
{
    public class Expense
    {
        [JsonProperty("cost_centre")]
        public string CostCentre { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }
    }
}