using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ImportEmail.Models
{
    public class Request
    {
        [JsonProperty("vendor")]
        public string Vendor { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("expense")]
        public Expense Expense { get; set; }
    }
}