using System;
using TB.BitMex;

namespace TB.Strategies {
  public class ThreeHTrendStrategy {
    private readonly IBitMexClient _bitMexClient;

    public ThreeHTrendStrategy(IBitMexClient bitMexClient) {
      _bitMexClient = bitMexClient;
    }
    public void Test() {
      _bitMexClient.GetPrice();
    }
  }
}
