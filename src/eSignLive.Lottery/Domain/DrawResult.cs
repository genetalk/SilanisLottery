using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSignLive.Lottery.Domain
{
    public class DrawResult
    {
        public LotteryNumbers Result { get; set; }
        public double[] PriceList { get; set; }
    }
}
