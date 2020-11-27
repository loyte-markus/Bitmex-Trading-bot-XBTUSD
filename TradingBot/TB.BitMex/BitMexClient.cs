
using Nyranith.Network;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TB.BitMex {
  public class BitMexClient {
    private string _domain;
    private string _apiKey;
    private string _apiSecret;

    public BitMexClient(string baseDomain, string apiKey, string apiSecret) {
      _domain = baseDomain;
      _apiKey = apiKey;
      _apiSecret = apiSecret;
    }

    public async Task Test() {

      var x = new HttpClient();
    }


    private long GetExpires() {
      return DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 3600; // set expires one hour in the future
    }


  }
}
