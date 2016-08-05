using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace eSignLive.Lottery.Domain
{
    public class Ticket
    {
        public string TicketNo { get; private set; }
        public string Name { get; private set; }
        public bool IsAutoGeneratingNumbers { get; private set; }
        public LotteryNumbers NumberSelected { get; private set; }

        public Ticket(string name, IRandomGenerator randomGenerator)
        {
            this.Name = name;
            this.TicketNo = GeneratingTickeNo();
            this.IsAutoGeneratingNumbers = true;
            this.NumberSelected = randomGenerator.GetLoterryBallsForEachTicket(); // For Sinalis lottery, only one ball can be chosen per ticket
        }

        public Ticket(string name, bool IsAutoGeneratingNumbers, LotteryNumbers numberSelected)
        {
            this.Name = name;
            this.TicketNo = GeneratingTickeNo();
            this.IsAutoGeneratingNumbers = false;
            this.NumberSelected = numberSelected;
        }

        private string GeneratingTickeNo()
        {
            return DateTime.Now.ToString("yyMMdd", CultureInfo.InvariantCulture) + RandomGenerator.GeneratingTicketSuffixNo(999999);
        }
    }
}
