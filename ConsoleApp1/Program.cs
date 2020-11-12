using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Program
    {
        private static string bitmexKey = System.IO.File.ReadAllText(@"C:\bitmexKey.txt");
        private static string bitmexSecret = System.IO.File.ReadAllText(@"C:\bitmexSecret.txt");
        double XBTUSD_prize;
        DateTime time;

        private static void Main(string[] args)
        {
            Program p = new Program();
            p.Run(args);
        }

        private void Run(string[] args)
        {
            BitMEXApi bitmex = new BitMEXApi(bitmexKey, bitmexSecret);
            while (true)
            {
                var priceList = bitmex.GetPrice();
                XBTUSD_prize = priceList[0].price;
                time = priceList[0].timestamp;
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
    }
}