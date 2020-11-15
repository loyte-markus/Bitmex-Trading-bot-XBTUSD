using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ConsoleApp1.Models;
using Newtonsoft.Json;

namespace ConsoleApp1 {
  public class Program {
    private static string bitmexKey = System.IO.File.ReadAllText(@"C:\bitmexKey.txt");
    private static string bitmexSecret = System.IO.File.ReadAllText(@"C:\bitmexSecret.txt");
    private const double BTC_per_trade = 0.02;
    private double stopLoss = 0;
    private double takeProfit = 0;

    private static void Main(string[] args) {
      Program p = new Program();
      p.Run(args);
    }

    private void Run(string[] args) {
      BitMEXApi bitmex = new BitMEXApi(bitmexKey, bitmexSecret);
      DateTime now = DateTime.Now;
      
      while(true) {
        var openOrderReponse = bitmex.GetOpenOrder();
        var order = JsonConvert.DeserializeObject<List<OpenOrder>>(openOrderReponse.ResponseData).FirstOrDefault();
        try { now = openOrderReponse.TimeStampUTC; } catch(Exception e) {
          Console.WriteLine(e);
        }

        if (order == null) {
          if(now.Minute == 00) {
            Log(now.ToString());
            var HH = now.Subtract(new TimeSpan(2, 0, 0)).ToString("HH");
            var old3 = JsonConvert.DeserializeObject<List<TradeBucketed>>(bitmex.Get1hCandle(HH).ResponseData).FirstOrDefault();

            HH = now.Subtract(new TimeSpan(1, 0, 0)).ToString("HH");
            var old2 = JsonConvert.DeserializeObject<List<TradeBucketed>>(bitmex.Get1hCandle(HH).ResponseData).FirstOrDefault();

            HH = now.Subtract(new TimeSpan(0, 0, 0)).ToString("HH");
            var old1 = JsonConvert.DeserializeObject<List<TradeBucketed>>(bitmex.Get1hCandle(HH).ResponseData).FirstOrDefault();

            if(old3.Close > old2.Close  && old2.Close > old1.Close) {
              Log("Placing short order...");
              var priceResponse = bitmex.GetPrice();
              var priceList = JsonConvert.DeserializeObject<List<Trade>>(priceResponse.ResponseData);
              double usdval = priceList.FirstOrDefault().Price * BTC_per_trade;
              bitmex.ShortOrder((int)usdval);
            } else if(old1.Close> old2.Close && old2.Close > old3.Close) {
              Log("Placing long order...");
              var priceResponse = bitmex.GetPrice();
              var priceList = JsonConvert.DeserializeObject<List<Trade>>(priceResponse.ResponseData);
              double usdval = priceList.FirstOrDefault().Price * BTC_per_trade;
              bitmex.LongOrder((int)usdval);
            }
            Thread.Sleep(5000);
          }
        }else {
          var priceResponse = bitmex.GetPrice();
          var priceList = JsonConvert.DeserializeObject<List<Trade>>(priceResponse.ResponseData);
          if(order.CurrentQty<0) {
            //Short
            if(stopLoss == 0) stopLoss = (double)order.AvgCostPrice * 1.01;
            if(takeProfit == 0) takeProfit = (double)order.AvgCostPrice * 0.99;
            if(priceList.FirstOrDefault().Price > stopLoss) {
              bitmex.closePosition();
              takeProfit = 0;
              stopLoss = 0;
              Log("Order closed.");
            }
            if(priceList.FirstOrDefault().Price < takeProfit) {
              stopLoss = takeProfit * 1.002;
              takeProfit = takeProfit * 0.995;
              Log("Stop Loss & Take Profit ++");
            }
          } else {
            //Long
            if(stopLoss == 0) stopLoss = (double)order.AvgCostPrice * 0.99;
            if(takeProfit == 0) takeProfit = (double)order.AvgCostPrice * 1.01;
            if(priceList.FirstOrDefault().Price < stopLoss) {
              bitmex.closePosition();
              takeProfit = 0;
              stopLoss = 0;
              Log("Order closed.");
            }
            if(priceList.FirstOrDefault().Price > takeProfit) {
              stopLoss = takeProfit * 0.998;
              takeProfit = takeProfit * 1.005;
              Log("Stop Loss & Take Profit ++");
            }
          }
        }
        Thread.Sleep(1250);
      }
    }

    private void setSLTP() {

    }
    private void Log(string msg) {
      Console.WriteLine(msg);
    }
  }
}