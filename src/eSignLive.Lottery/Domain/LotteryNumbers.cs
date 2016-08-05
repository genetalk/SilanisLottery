using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSignLive.Lottery.Domain
{
    public struct LotteryNumbers
    {
        public int[] RegularNumbers;
        public bool isPowerBallUsed;
        public int? PowerBall;

        public LotteryNumbers(int[] regularNumbers)
        {
            this.RegularNumbers = regularNumbers;
            this.isPowerBallUsed = false;
            this.PowerBall = null;
        }

        public LotteryNumbers(int[] regularNumbers, int powerBall)
        {
            this.RegularNumbers = regularNumbers;
            this.isPowerBallUsed = true;
            this.PowerBall = powerBall;
        }
    }
}
