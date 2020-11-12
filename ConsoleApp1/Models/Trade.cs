﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    public class Trade
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("tickDirection")]
        public string TickDirection { get; set; }

        [JsonProperty("trdMatchID")]
        public string TrdMatchId { get; set; }

        [JsonProperty("grossValue")]
        public long GrossValue { get; set; }

        [JsonProperty("homeNotional")]
        public long HomeNotional { get; set; }

        [JsonProperty("foreignNotional")]
        public long ForeignNotional { get; set; }
    }
}
