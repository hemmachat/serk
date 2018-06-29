using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImportEmail.Models
{
    public class XmlRoot
    {
        [JsonProperty("request")]
        public Request Request { get; set; }
    }
}