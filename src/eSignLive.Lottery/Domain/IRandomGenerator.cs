using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSignLive.Lottery.Domain
{
    public interface IRandomGenerator
    {
        LotteryNumbers GetLoterryBallsForDraw();
        LotteryNumbers GetLoterryBallsForEachTicket();
    }
}
