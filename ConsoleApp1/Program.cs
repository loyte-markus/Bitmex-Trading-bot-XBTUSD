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
    private static double BTC_per_trade = 0.02;

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
              Log("We can short!");
              var priceResponse = bitmex.GetPrice();
              var priceList = JsonConvert.DeserializeObject<List<Trade>>(priceResponse.ResponseData);
              double usdval = priceList.FirstOrDefault().Price * BTC_per_trade;
              bitmex.ShortOrder((int)usdval);
            } else if(old1.Close> old2.Close && old2.Close > old3.Close) {
              Log("We can long!");
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

        }
        /*
        else{
           //Vi har nå en posisjon åpen, pass på rundt pris sånn at den blir solgt riktig
           //Stoplossen settes først til +/-X%(1?) av entryprice
           //Om vi plusser 1% prosent så settes stoplossen til å være på f.eks 0.6% profit
           //Stoplossen skal gå opp dynamisk, så om vi er på 2% profit så skal stoplossen være på 1.2%
        }
        */

        Thread.Sleep(1250);
      }
    }

    private void Log(string msg) {
      Console.WriteLine(msg);
    }
  }
}