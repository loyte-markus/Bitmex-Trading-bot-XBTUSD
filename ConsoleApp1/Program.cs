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
    private const double BTC_per_trade = 0.025;
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
        errorCheck(openOrderReponse, args);
        var order = JsonConvert.DeserializeObject<List<OpenOrder>>(openOrderReponse.ResponseData).FirstOrDefault();
        try { now = openOrderReponse.TimeStampUTC; } catch(Exception e) {
          Console.WriteLine("try: " + e);
        }
        
        if (order == null) {
          if(now.Minute == 00) {
            //TODO: Legg til logikk på at vi ikke vil gå inn i samme trend flere ganger
            Log(now.ToString()+":");
            var HH = now.Subtract(new TimeSpan(3, 0, 0)).ToString("HH");
            var old4 = JsonConvert.DeserializeObject<List<TradeBucketed>>(bitmex.Get1hCandle(HH).ResponseData).FirstOrDefault();

            HH = now.Subtract(new TimeSpan(2, 0, 0)).ToString("HH");
            var old3 = JsonConvert.DeserializeObject<List<TradeBucketed>>(bitmex.Get1hCandle(HH).ResponseData).FirstOrDefault();

            HH = now.Subtract(new TimeSpan(1, 0, 0)).ToString("HH");
            var old2 = JsonConvert.DeserializeObject<List<TradeBucketed>>(bitmex.Get1hCandle(HH).ResponseData).FirstOrDefault();

            var HHH = now.Hour.ToString();
            var old1 = JsonConvert.DeserializeObject<List<TradeBucketed>>(bitmex.Get1hCandle(HHH).ResponseData).FirstOrDefault();

            if(old4.Close > old3.Close && old3.Close > old2.Close  && old2.Close > old1.Close) {
              Log("Placing short order...");
              var priceResponse = bitmex.GetPrice();
              errorCheck(priceResponse, args);
              var priceList = JsonConvert.DeserializeObject<List<Trade>>(priceResponse.ResponseData);
              double usdval = priceList.FirstOrDefault().Price * BTC_per_trade;
              bitmex.ShortOrder((int)usdval);
            } else if(old1.Close > old2.Close && old2.Close > old3.Close && old3.Close > old4.Close) {
              Log("Placing long order...");
              var priceResponse = bitmex.GetPrice();
              errorCheck(priceResponse, args);
              var priceList = JsonConvert.DeserializeObject<List<Trade>>(priceResponse.ResponseData);
              double usdval = priceList.FirstOrDefault().Price * BTC_per_trade;
              bitmex.LongOrder((int)usdval);
            }
            Thread.Sleep(5000);
          }
        }else {
          var priceResponse = bitmex.GetPrice();
          errorCheck(priceResponse, args);
          var priceList = JsonConvert.DeserializeObject<List<Trade>>(priceResponse.ResponseData);
          if(order.CurrentQty<0) {
            //Short
            if(stopLoss == 0) stopLoss = (long)order.AvgCostPrice * 1.01;
            if(takeProfit == 0) takeProfit = (long)order.AvgCostPrice * 0.99;
            if(priceList.FirstOrDefault().Price > stopLoss) {
              bitmex.closePosition();
              Log(now + ": Order closed.");
              Log("Entry: " + order.AvgCostPrice + "Sold at ~"+ stopLoss);
              takeProfit = 0;
              stopLoss = 0;
            }
            if(priceList.FirstOrDefault().Price < takeProfit) {
              stopLoss = takeProfit * 1.002;
              takeProfit = takeProfit * 0.995;
              Log(now + ": Stop Loss & Take Profit ++");
            }
          } else {
            //Long
            if(stopLoss == 0) stopLoss = (long)order.AvgCostPrice * 0.99;
            if(takeProfit == 0) takeProfit = (long)order.AvgCostPrice * 1.01;
            if(priceList.FirstOrDefault().Price < stopLoss) {
              bitmex.closePosition();
              takeProfit = 0;
              stopLoss = 0;
              Log(now + ": Order closed.");
              Log("Entry: " + order.AvgCostPrice + "Sold at ~" + stopLoss);

            }
            if(priceList.FirstOrDefault().Price > takeProfit) {
              stopLoss = takeProfit * 0.998;
              takeProfit = takeProfit * 1.005;
              Log(now + ": Stop Loss & Take Profit ++");
            }
          }
        }
       Thread.Sleep(2550);
      }
    }

    private void errorCheck(APIResponse a, string[] args) {
      if(a.ToString().Contains("error")) {
        Thread.Sleep(5000);
        Log("Re-running Run()...");
        Run(args);
      }
    }

    private void Log(string msg) {
      Console.WriteLine(msg);
    }
  }
}