using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSignLive.Lottery.Domain
{
    public struct Winner
    {
        public string Name { get; set; }
        public Prize ThePrize { get; set; }
        public double PrizeValue { get; set; }
    }

    public enum Prize
    {
        First,
        Second,
        Third
    }
}
