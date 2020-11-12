using System;
using System.Collections.Generic;
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
                string fetchGetPrice = bitmex.getPrice();
                List<Trade> getPriceList = JsonConvert.DeserializeObject<List<Trade>>(fetchGetPrice);
                XBTUSD_prize = getPriceList[0].price;
                time = getPriceList[0].timestamp;

                Console.WriteLine(XBTUSD_prize);
                Console.WriteLine(time.Hour + ":" + time.Minute + ":" + time.Second);


                Thread.Sleep(1000);
            }            
        }
    }

    class Trade
    {
        public DateTime timestamp { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public int size { get; set; }
        public double price { get; set; }
        public string tickDirection { get; set; }
        public double grossValue { get; set; }
        public double homeNotional { get; set; }
        public double foreignNotional { get; set; }
    }
}