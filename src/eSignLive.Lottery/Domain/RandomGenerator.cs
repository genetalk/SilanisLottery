using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSignLive.Lottery.Configuration;

namespace eSignLive.Lottery.Domain
{
    public sealed class RandomGenerator : IRandomGenerator
    {
        static Random r = RandomHelper.Instance;

        public static string GeneratingTicketSuffixNo(int maxNumber)
        {
            return r.Next(maxNumber).ToString().PadLeft(maxNumber.ToString().Length, '0');
        }

        public RandomGenerator()
        {
        }

        public LotteryNumbers GetLoterryBallsForEachTicket()
        {
            return GetLoterryBalls(ConfigurationParameters.TotalBallInEachTicket);
        }

        public LotteryNumbers GetLoterryBallsForDraw()
        {
            return GetLoterryBalls(ConfigurationParameters.TotalNumberOfBallPerDraw);
        }

        private LotteryNumbers GetLoterryBalls(int totalBallToBeDrawn)
        {
            int[] result = new int[totalBallToBeDrawn];

            int[] simulatedBalls = null;
            if (ConfigurationParameters.IsSimulatingWinners) // && LotteryRepository.TheNthDraw == 0)
            {
                simulatedBalls = IsInSimulatedBallNumber();
                for (int i = 0; i < totalBallToBeDrawn; i++)
                {
                    int number;
                    do
                    {
                        number = r.Next(ConfigurationParameters.MinNumber, ConfigurationParameters.MaxNumber);        //1-50 for the regular balls
                    } while (simulatedBalls.Contains(number)); //no duplicate values

                    result[i] = number;
                }
            }
            else
            {
                for (int i = 0; i < totalBallToBeDrawn; i++)
                {
                    int number;
                    do
                    {
                        number = r.Next(ConfigurationParameters.MinNumber, ConfigurationParameters.MaxNumber);        //1-50 for the regular balls
                    } while (result.Contains(number)); //no duplicate values

                    result[i] = number;
                }
            }

            if (string.Compare(ConfigurationParameters.LottyeryType, "Sinalis", true) != 0) // not good for Salinis lottery since the order determines the price in sequence
                Array.Sort(result);                 //makes displaying nicer

            if (ConfigurationParameters.HasPowerBall && ConfigurationParameters.MaxPowerNumber >= 0)
            {
                int powerBall = r.Next(ConfigurationParameters.MinNumber, ConfigurationParameters.MaxPowerNumber);      //1-35 for powerball
                return new LotteryNumbers(result, powerBall);
            }
            else
                return new LotteryNumbers(result);
        }

        private int[] IsInSimulatedBallNumber()
        {
            int[] simulatedBalls = new int[ConfigurationParameters.SimulatedWinners.Length];
            for (int i = 0; i < ConfigurationParameters.SimulatedWinners.Length; i++)
            {
                string[] winnerBall = ConfigurationParameters.SimulatedWinners[i].Split(':');
                simulatedBalls[i] = Int32.Parse(winnerBall[1]);
            }
            return simulatedBalls;
        }
    }
}
