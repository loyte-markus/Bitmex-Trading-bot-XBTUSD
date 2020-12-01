using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TB.BitMex {
  public interface IBitMexClient {
    Task<bool> CloseAllOrders();
    Task<double> GetPrice();
    Task<string> GetOpenOrders();
    Task<bool> PlaceOrder(string side, double amountBTC);
    Task<double> GetHourCandlePrice(string hh);
    Task<bool> ShouldWeTrade();
  }
}
