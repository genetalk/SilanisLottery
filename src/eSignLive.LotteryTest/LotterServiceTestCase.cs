using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using eSignLive.Lottery.Domain;
using eSignLive.Lottery.Application;
using eSignLive.Lottery.Configuration;
using NUnit.Framework;

namespace eSignLive.LotteryTest
{
    [TestFixture]
    public class LotterServiceTestCase
    {
        private ILotteryService service;

        public LotterServiceTestCase(ILotteryService service)
        {
            this.service = service;
        }

        [TestCase(500)]
        [TestCase(200)]
        [TestCase(100)]
        public List<Ticket> Purchase(int maxNumberOfTicket)
        {
            return service.SimulatedPurchase(maxNumberOfTicket);

            //Assert.That(ballNumber, Is.Equals(isExpected));
        }

        public DrawResult Draw()
        {
            return service.Draw();
        }

        public List<Winner> IdentifyWinner()
        {
            return service.IdentifyWinner();
        }
    }
}
