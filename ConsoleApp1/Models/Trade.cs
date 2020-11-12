using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    public class Trade
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
