using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models {
  class TradeBucketed {
    [JsonProperty("timestamp")]
    public DateTimeOffset Timestamp { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("open")]
    public long Open { get; set; }

    [JsonProperty("high")]
    public double High { get; set; }

    [JsonProperty("low")]
    public long Low { get; set; }

    [JsonProperty("close")]
    public long Close { get; set; }

    [JsonProperty("trades")]
    public long Trades { get; set; }

    [JsonProperty("volume")]
    public long Volume { get; set; }

    [JsonProperty("vwap")]
    public double Vwap { get; set; }

    [JsonProperty("lastSize")]
    public long LastSize { get; set; }

    [JsonProperty("turnover")]
    public long Turnover { get; set; }

    [JsonProperty("homeNotional")]
    public double HomeNotional { get; set; }

    [JsonProperty("foreignNotional")]
    public long ForeignNotional { get; set; }
  }
}
