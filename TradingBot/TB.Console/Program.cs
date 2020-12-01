using System;
using TB.BitMex;

namespace TB.Console {
  class Program {
    static void Main(string[] args) {
      System.Console.WriteLine("Hello World!");
      var client = new BitMexClient("","","");
      client.GetPrice();
    }
  }
}
