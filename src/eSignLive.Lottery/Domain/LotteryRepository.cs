using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSignLive.Lottery.Configuration;

namespace eSignLive.Lottery.Domain
{
    public class LotteryRepository : ILotteryRepository
    {
        public static int TheNthDraw { get; set; }
        private double TotalAccumulatedFunds { get; set; }

        public int DrawDateTime { get;  private set; }
        List<Winner> winners = new List<Winner>();

        //Total tickets purchased by customers
        public List<Ticket> tickets = new List<Ticket>();
        public DrawResult drawResult = new DrawResult();

        private readonly IRandomGenerator randomEngine;

        public LotteryRepository(IRandomGenerator randomEngine)
        {
            this.randomEngine = randomEngine;
        }

        public List<Ticket> SimulatedPurchase(int maxNumberOfTicket)
        {
            Ticket ticket = null;
            int numOfTicketRemaining = maxNumberOfTicket;

            if (ConfigurationParameters.IsSimulatingWinners)
            {
                for (int i = 0; i < ConfigurationParameters.SimulatedWinners.Length; i++)
                {
                    string[] winnerBall = ConfigurationParameters.SimulatedWinners[i].Split(':');

                    int[] numbers = new int[1];
                    numbers[0] = Int32.Parse(winnerBall[1]);

                    LotteryNumbers lotteryNumber = new LotteryNumbers(numbers);
                    ticket = new Ticket(winnerBall[0].Trim(), false, lotteryNumber);
                    tickets.Add(ticket);
                }
                numOfTicketRemaining = maxNumberOfTicket - 3;
            }

            for (int i=0; i< numOfTicketRemaining; i++)
            {
                ticket = new Ticket("Client" + i.ToString(), randomEngine);
                tickets.Add(ticket);
            }
            return tickets;
        }

        public int[] Purchase(UserInput userInput)
        {
            Ticket ticket;
            if (userInput.IsAutoGeneratingBall)
                ticket = new Ticket(userInput.Name, randomEngine);
            else
            {
                int number = (userInput.NumberOfBallChosen != null) ? (int) userInput.NumberOfBallChosen : -1;
                int[] numbers = new int[1];
                numbers[0] = number;

                LotteryNumbers lotteryNumber = new LotteryNumbers(numbers);
                ticket = new Ticket(userInput.Name, false, lotteryNumber);
            }
            tickets.Add(ticket);
            return ticket.NumberSelected.RegularNumbers;
        }

        public DrawResult Draw() 
        {
            LotteryNumbers lotteryDrawNumbers;
            
            if (ConfigurationParameters.IsSimulatingWinners) // && TheNthDraw == 0) // default the three winners if simulation is used
            {
                int[] simulatedWinners = new int[3];
                foreach (Ticket ticket in tickets)
                {
                    if (ConfigurationParameters.SimulatedWinners[0].Contains(ticket.Name))
                        simulatedWinners[0] = ticket.NumberSelected.RegularNumbers[0];
                    else if (ConfigurationParameters.SimulatedWinners[1].Contains(ticket.Name))
                        simulatedWinners[1] = ticket.NumberSelected.RegularNumbers[0];
                    else if (ConfigurationParameters.SimulatedWinners[2].Contains(ticket.Name))
                        simulatedWinners[2] = ticket.NumberSelected.RegularNumbers[0];
                }
                lotteryDrawNumbers = new LotteryNumbers(simulatedWinners);
            }
            else // draw winners randomly 
                lotteryDrawNumbers = randomEngine.GetLoterryBallsForDraw();

            drawResult.Result = lotteryDrawNumbers;
            CalculateTotalFondsAccumulated();
            drawResult.PriceList = new double[ConfigurationParameters.PrizePercentages.Length];

            for (int i=0; i< ConfigurationParameters.PrizePercentages.Length; i++)
            {
                drawResult.PriceList[i] = TotalAccumulatedFunds * ConfigurationParameters.PotRewardPercentage /100.0 * double.Parse(ConfigurationParameters.PrizePercentages[i]) / 100.0;
            }

            return drawResult;
        }

        public List<Winner> IdentifierWinner()
        {
            //for Silanis lottery, the sequence of three balls drawn resulting in different prizes
            winners.Clear();

            foreach (Ticket ticket in tickets)
            {
                if (drawResult.Result.RegularNumbers[0] == ticket.NumberSelected.RegularNumbers[0])
                {
                    Winner winner = new Winner();
                    winner.Name = ticket.Name;
                    winner.ThePrize = Prize.First;
                    winners.Add(winner);
                }
                else if (drawResult.Result.RegularNumbers[1] == ticket.NumberSelected.RegularNumbers[0])
                {
                    Winner winner = new Winner();
                    winner.Name = ticket.Name;
                    winner.ThePrize = Prize.Second;
                    winners.Add(winner);
                }
                else if (drawResult.Result.RegularNumbers[2] == ticket.NumberSelected.RegularNumbers[0])
                {
                    Winner winner = new Winner();
                    winner.Name = ticket.Name;
                    winner.ThePrize = Prize.Third;
                    winners.Add(winner);
                }
            }

            // now split the prize depending on number of winners
            // the first prize
            int numberOfPrize = winners.Count(n => n.ThePrize == Prize.First);
            double prize = 0;
            if (numberOfPrize > 0)
            {
                prize = drawResult.PriceList[0] / numberOfPrize;

                //this linq updating item seems not working
                //(winners.Where(n => n.ThePrize == Prize.First)).ToList().ForEach(n =>
                //{
                //    n.PrizeValue = prize;
                //});

                for (int i = 0; i < winners.Count; i++)
                {
                    if (winners[i].ThePrize == Prize.First)
                    {
                        Winner winner = winners[i];
                        winner.PrizeValue = prize;
                        winners[i] = winner;
                    }
                }
            }
            
            //second prize
            numberOfPrize = winners.Count(n => n.ThePrize == Prize.Second);
            if (numberOfPrize > 0)
            {
                prize = drawResult.PriceList[1] / numberOfPrize;
                for (int i = 0; i < winners.Count; i++)
                {
                    if (winners[i].ThePrize == Prize.Second)
                    {
                        Winner winner = winners[i];
                        winner.PrizeValue = prize;
                        winners[i] = winner;
                    }
                }
            }

            //third prize
            numberOfPrize = winners.Count(n => n.ThePrize == Prize.Third);
            if (numberOfPrize > 0)
            {
                prize = drawResult.PriceList[2] / numberOfPrize;
                for (int i = 0; i < winners.Count; i++)
                {
                    if (winners[i].ThePrize == Prize.Third)
                    {
                        Winner winner = winners[i];
                        winner.PrizeValue = prize;
                        winners[i] = winner;
                    }
                }
            }

            //clear up all existing tickets after identifying the winners since we do not need to store
            tickets.Clear();
            winners = winners.OrderBy(p => p.ThePrize).ToList();

            return winners;

            //int matchingNumbers = drawResult.Result.RegularNumbers.Intersect(lottery.RegularNumbers).Count();
            //bool powerBall = (drawResult.Result.RegularNumbers.PowerBall  lottery.PowerBall);
        }

        private void CalculateTotalFondsAccumulated()
        {
            TotalAccumulatedFunds = ConfigurationParameters.TicketCost * tickets.Count;
        }

    }
}
