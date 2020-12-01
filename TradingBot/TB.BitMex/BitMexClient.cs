
using Nyranith.Network;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TB.BitMex {
  public class BitMexClient : IBitMexClient {
    private string _domain;
    private string _apiKey;
    private string _apiSecret;

    public BitMexClient(string baseDomain, string apiKey, string apiSecret) {
      _domain = baseDomain;
      _apiKey = apiKey;
      _apiSecret = apiSecret;
    }

    public Task<bool> CloseAllOrders() {
      // Lukker alle åpne handler
      // Returnerer true, kan kanskje være void?
      throw new NotImplementedException();
    }

    public Task<double> GetHourCandlePrice(string hh) {
      // Tar inn inn timen vi skal sjekke på som string
      // Returnerer prisen baren lukket på
      throw new NotImplementedException();
    }

    public Task<string> GetOpenOrders() {
      // Henter åpne orders
      // returnerer de som en string(?)
      throw new NotImplementedException();
    }

    public Task<double> GetPrice() {
      // Henter siste trade plassert på BitMex
      // Returnerer prisen
      throw new NotImplementedException();
    }

    public Task<bool> PlaceOrder(string side, double amountBTC) {
      // Lag en order med "buy" eller "sell" på side
      // Regner ut BTC mengden til USD ved hjelp av GetPrice()
      throw new NotImplementedException();
    }

    public Task<bool> ShouldWeTrade() {
      // Kaller på GetHourCandlePrice() og sammenligner
      // Kaller på PlaceOrder() om conditions stemmer
      // retun true om PlaceOrder kalles, false ellers
      throw new NotImplementedException();
    }

    public async Task Test() {

      var x = new HttpClient();
    }


    private long GetExpires() {
      return DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 3600; // set expires one hour in the future
    }


  }
}
