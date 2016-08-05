using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSignLive.Lottery.Configuration
{
    public sealed class ConfigurationParameters
    {
        public static bool SyncReading { get; private set; } // sync if configuration file is properly read out

        public static int TotalNumberOfBallPerDraw { get; private set; }
        public static int TotalBallInEachTicket { get; private set; }
        public static int MinNumber { get; private set; }
        public static int MaxNumber { get; private set; }
        public static bool HasPowerBall { get; private set; }
        public static int MaxPowerNumber { get; private set; }
        public static string LottyeryType { get; private set; }
        public static double TicketCost { get; private set; }
        public static double PotRewardPercentage { get; private set; }
        public static string[] PrizePercentages { get; private set; }
        public static double CostForPowerBall { get; private set; }
        public static bool IsSimulatingWinners { get; private set; }
        public static string[] SimulatedWinners { get; private set; }

        public static bool InitializeConfigurationParameters(Dictionary<string, string> appSettings)
        {
            try
            {
                TotalNumberOfBallPerDraw = Int32.Parse(appSettings["NumberOfBallInEachDraw"]);
                TotalBallInEachTicket = Int32.Parse(appSettings["TotalBallInEachTicket"]);
                MinNumber = Int32.Parse(appSettings["MinNumber"]);
                MaxNumber = Int32.Parse(appSettings["MaxNumber"]);
                HasPowerBall = bool.Parse(appSettings["HasPowerBall"]);
                MaxPowerNumber = appSettings["MaxPowerNumber"].Length > 0 ? Int32.Parse(appSettings["MaxPowerNumber"]) : -1;
                LottyeryType = appSettings["LotteryType"];
                TicketCost = double.Parse(appSettings["TicketCost"].Replace("$", string.Empty));
                PotRewardPercentage = double.Parse(appSettings["PotRewardPercentage"]);
                PrizePercentages = appSettings["PrizePercentages"].Replace("(", string.Empty).Replace(")", string.Empty).Split(',');
                CostForPowerBall = double.Parse(appSettings["CostForPowerBall"].Replace("$", string.Empty));
                IsSimulatingWinners = bool.Parse(appSettings["IsSimulatingWinners"]);
                SimulatedWinners = appSettings["SimulatedWinners"].Replace("(", string.Empty).Replace(")", string.Empty).Split(',');

                SyncReading = true;
            }
            catch (Exception ex)
            {
                SyncReading = false;
                return false;
            }
            return true;
        }
    }
}
