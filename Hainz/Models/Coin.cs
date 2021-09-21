using System;

namespace Hainz.Models 
{
    public static class Coin 
    {
        public static CoinResult Flip() 
        {
            var random = new Random();
            return (CoinResult)random.Next(0, 2);
        }
    }
}