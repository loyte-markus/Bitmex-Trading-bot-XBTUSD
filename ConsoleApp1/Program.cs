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
    

    private static void Main(string[] args) {
      Program p = new Program();
      p.Run(args);
    }



    private void Run(string[] args) {
      BitMEXApi bitmex = new BitMEXApi(bitmexKey, bitmexSecret);


      DateTime now;


      while(true) {

        var openOrderReponse = bitmex.GetOpenOrder();
        var order = JsonConvert.DeserializeObject<List<OpenOrder>>(openOrderReponse.ResponseData).FirstOrDefault();
        now = openOrderReponse.TimeStampUTC;


        if(order == null) {
          if(now.Minute == 00) {
            var HH = now.Subtract(new TimeSpan(3, 0, 0)).ToString("HH");
            var old3 = JsonConvert.DeserializeObject<List<Trade>>(bitmex.GetPrice(HH).ResponseData).FirstOrDefault();

            HH = now.Subtract(new TimeSpan(2, 0, 0)).ToString("HH");
            var old2 = JsonConvert.DeserializeObject<List<Trade>>(bitmex.GetPrice(HH).ResponseData).FirstOrDefault();

            HH = now.Subtract(new TimeSpan(1, 0, 0)).ToString("HH");
            var old1 = JsonConvert.DeserializeObject<List<Trade>>(bitmex.GetPrice(HH).ResponseData).FirstOrDefault();

            if(old3.Price > old2.Price  && old2.Price > old1.Price) {
              Log("We can short!");
            }else if(old1.Price> old2.Price && old2.Price > old3.Price) {
              Log("We can long!");
            }

            Thread.Sleep(60000);
          }
        }else {
          var priceResponse = bitmex.GetPrice();
          var priceList = JsonConvert.DeserializeObject<List<Trade>>(priceResponse.ResponseData);

        }




        /*
        if(a.length==2){
           //Vi har ikke en posisjon åpen, se etter mulighet for åpning av posisjon
           if(time.Minute == 0){
               //Vi skal nå sjekke om vi vil gjøre en trade
               //Om de tre forrige timesbarene går høyere enn hverandre, kjør inn en long posisjon
               //Om de tre forrige timesbarene går lavere enn hverandre, kjør inn en short posisjon
           }
        }
        else{
           //Vi har nå en posisjon åpen, pass på rundt pris sånn at den blir solgt riktig
           //Stoplossen settes først til +/-X%(1?) av entryprice
           //Om vi plusser 1% prosent så settes stoplossen til å være på f.eks 0.6% profit
           //Stoplossen skal gå opp dynamisk, så om vi er på 2% profit så skal stoplossen være på 1.2%
        }
        */

        Thread.Sleep(1000);
      }
    }

    private void Log(string msg) {
      Console.WriteLine(msg);
    }
  }
}