using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSignLive.Lottery.Domain;
using eSignLive.Lottery.Configuration;

namespace eSignLive.Lottery.Application
{
    public interface ILotteryService
    {
        bool InitializeParameter();

        List<Ticket> SimulatedPurchase(int maxNumberOfTicket);

        int[] Purchase(UserInput userInput);

        DrawResult Draw();

        List<Winner> IdentifyWinner();
    }
}
