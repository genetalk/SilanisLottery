using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSignLive.Lottery.Domain;
using eSignLive.Lottery.Configuration;

namespace eSignLive.Lottery.Application
{
    public class LotteryService : ILotteryService
    {
        private readonly ILotteryRepository lotteryRepository;
        private readonly IAppSetting appSetting;

        public LotteryService(ILotteryRepository lotteryRepository, IAppSetting appSetting)
        {
            this.lotteryRepository = lotteryRepository;
            this.appSetting = appSetting;
        }

        public bool InitializeParameter()
        {
            Dictionary<string, string> appSettings = appSetting.ReadConfigurationFile();
           return ConfigurationParameters.InitializeConfigurationParameters(appSettings);
        }

        public List<Ticket> SimulatedPurchase(int maxNumberOfTicket)
        {
            return lotteryRepository.SimulatedPurchase(maxNumberOfTicket);
        }

        public int[] Purchase(UserInput userInput)
        {
            return lotteryRepository.Purchase(userInput);
        }

        public DrawResult Draw()
        {
            return lotteryRepository.Draw();
        }

        public List<Winner> IdentifyWinner()
        {
            return lotteryRepository.IdentifierWinner();
        }
    }
}