using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSignLive.Lottery.Domain
{
    public interface ILotteryRepository
    {
        List<Ticket> SimulatedPurchase(int maxNumberOfTicket);

        int[] Purchase(UserInput userInput);

        DrawResult Draw();

        List<Winner> IdentifierWinner();
    }
}
