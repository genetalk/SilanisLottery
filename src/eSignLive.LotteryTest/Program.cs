using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using eSignLive.Lottery.Domain;
using eSignLive.Lottery.Application;
using eSignLive.Lottery.Configuration;

namespace eSignLive.LotteryTest
{
    public class Program
    {

        //public IConfiguration Configuration { get; set; }
        //public Dictionary<string, string> Paths { get; set; }
        //public Program(IApplicationEnvironment app,
        //       IRuntimeEnvironment runtime,
        //       IRuntimeOptions options)
        //{
        //    Configuration = new ConfigurationBuilder()
        //        .AddJsonFile(Path.Combine(app.ApplicationBasePath, "config.json"))
        //        .AddEnvironmentVariables()
        //        .Build();
        //}

        public static void Main(string[] args)
        {
            // create service collection
            IServiceCollection services = new ServiceCollection();

            // add descriptors for interfaces
            services.AddSingleton<IAppSetting, AppSetting>();
            services.AddSingleton<IRandomGenerator, RandomGenerator>();
            services.AddScoped<ILotteryRepository, LotteryRepository>();
            services.AddScoped<ILotteryService, LotteryService>();

            //services.AddInstance<IConfigurationRoot>(Configuration);

            // create service provider
            IServiceProvider provider = services.BuildServiceProvider();

            // resolve the ILotteryService
            ILotteryService service = provider.GetService<ILotteryService>();

            //// use the resolved Lotter service
            //List<Widget> widgets = service.GetAllWidgets();

            //initialize parameter based on appSettings.json file
            Console.WriteLine("******************************************************************************************************************");
            Console.WriteLine("This is a demo program for Silanis Lottery written in .Net Core \nBy Ziru He in August 2016!");
            Console.WriteLine("******************************************************************************************************************\n");
            if (!service.InitializeParameter())
            {
                Console.WriteLine("Application has configuration reading error, please check appSettings.json file location and its contents.");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Note: if you enter EXIT at any time, the application will be terminated. \n");

                UserInput userInput = new UserInput();

                string line = string.Empty;
                bool loopDraw = true;
                bool firstTime = true;
                LotteryRepository.TheNthDraw = 0;
                int choice = 1;
                LotterServiceTestCase lotterServiceTestCase = new LotterServiceTestCase(service);

                Console.WriteLine("There are two options to test this lottery game, winners can be simulated for case 1 and 2 (first draw only) if configured in the appSettings.json file: ");
                Console.WriteLine("case 1 - Completely randomly gerating tickets;");
                Console.WriteLine("case 2 - Randomly gerating 20 tickets for the first draw if winners are simulated, then manually generating tickets one-by-one.\n");

                while (loopDraw) // Loop indefinitely
                {
                    Console.WriteLine("Please choose your case 1 or 2:");
                    while (true)
                    {
                        line = Console.ReadLine();
                        if (string.Compare(line, "exit", true) == 0)
                            Environment.Exit(0);
                        else if (line == "1")
                        {
                            choice = 1;
                            break;
                        }
                        else if (line == "2")
                        {
                            choice = 2;
                            break;
                        }
                        else
                            Console.WriteLine("Invalid choice, please try again:");
                    }
                    Console.WriteLine("\n");

                    if (choice == 2)
                    {
                        if (ConfigurationParameters.IsSimulatingWinners && firstTime)
                        {
                            Console.WriteLine("Step 1: Automatically purchase tickets based on the appSettings.json file. \n");
                            service.SimulatedPurchase(20);
                        }
                        else
                        {
                            Console.WriteLine("Step 1: purchase tickets  \n");
                            bool loopTicket = true;
                            while (loopTicket)
                            {
                                //Step 1: purchase a ticket
                                Console.WriteLine("Enter your first:"); // Prompt
                                line = Console.ReadLine(); // Get string from user
                                if (string.Compare(line, "exit", true) == 0)
                                    Environment.Exit(0);
                                userInput.Name = line;

                                while (true)
                                {
                                    Console.WriteLine("Would you like to manually select a ball by yourself (Enter N or No will yield to machine to automatically generate for you)? ");
                                    line = Console.ReadLine();
                                    if (string.Compare(line, "exit", true) == 0)
                                        Environment.Exit(0);
                                    else if (string.Compare(line, "N", true) == 0 || string.Compare(line, "No", true) == 0)
                                    {
                                        userInput.IsAutoGeneratingBall = true;
                                    }
                                    else
                                    {
                                        userInput.IsAutoGeneratingBall = false;
                                        bool error = true;
                                        int ballChosen;
                                        while (error)
                                        {
                                            Console.WriteLine("Please choose your ball number between " + ConfigurationParameters.MinNumber.ToString()
                                                + " and " + ConfigurationParameters.MaxNumber.ToString() + ": ");
                                            line = Console.ReadLine();
                                            if (string.Compare(line, "exit", true) == 0)
                                                Environment.Exit(0);
                                            else if (int.TryParse(line, out ballChosen))
                                            {
                                                userInput.NumberOfBallChosen = ballChosen;
                                                if (ballChosen < ConfigurationParameters.MinNumber || ballChosen > ConfigurationParameters.MaxNumber)
                                                    Console.WriteLine("The number chosen is out of the range. Please choose again! ");
                                                else
                                                    error = false;
                                            }
                                        }
                                    }
                                    int[] balls = service.Purchase(userInput);
                                    Console.WriteLine("You have purchased the Silanis lottery ticket with ball number of " + String.Join(",", balls.Select(p => p.ToString()).ToArray()));
                                    Console.WriteLine();

                                    Console.WriteLine("Would you like to purchase additional tickets (Enter N or No to finish purchasing)? ");
                                    line = Console.ReadLine();
                                    if (string.Compare(line, "exit", true) == 0)
                                        Environment.Exit(0);
                                    else if (string.Compare(line, "N", true) == 0 || string.Compare(line, "No", true) == 0)
                                    {
                                        loopTicket = false;
                                        break;
                                    }
                                    Console.WriteLine("\n");
                                }
                            }
                        }
                    }
                    else // choice 1 for random simulation
                    {
                        int n = 0;
                        while (true) {
                            Console.WriteLine("Number of tickets to be generated, please enter a nature number: ");
                            line = Console.ReadLine();
                            if (string.Compare(line, "exit", true) == 0)
                                Environment.Exit(0);
                            else
                            {
                                bool isNumeric = int.TryParse(line, out n);
                                if (isNumeric && n> 0)
                                    break;
                                else
                                    Console.WriteLine("Invalid number, please try again.");
                            }
                        }
                        Console.WriteLine("\n");
                        List<Ticket> tickets = lotterServiceTestCase.Purchase(n);
                        Console.WriteLine("Ticket purchased: ");
                        foreach (Ticket ticket in tickets)
                        {
                            Console.WriteLine(ticket.Name + "\t" + ticket.TicketNo + "\t" + String.Join(",", ticket.NumberSelected.RegularNumbers.Select(p => p.ToString()).ToArray()));
                        }
                        Console.WriteLine("\n");
                    }

                    //Step 2: draw processing
                    Console.WriteLine("Step 2: Draw lottery  \n");
                    DrawResult drawResult;
                    if (choice == 2)
                    {
                        drawResult = service.Draw();
                    }
                    else
                    {
                        drawResult = lotterServiceTestCase.Draw();
                    }
                    Console.WriteLine("The resulting draw numbers: " + String.Join(",", drawResult.Result.RegularNumbers.Select(p => p.ToString()).ToArray()) + "\n");

                    //Step 3: display winner
                    Console.WriteLine("Step 3: Display winners  \n");
                    List<Winner> winners = null;
                    if (choice == 2)
                    {
                        winners = service.IdentifyWinner();
                    }
                    else
                    {
                        winners = lotterServiceTestCase.IdentifyWinner();
                    }

                    Console.WriteLine("The " + (++LotteryRepository.TheNthDraw).ToString() + " draw result: \n");
                    if (winners != null && winners.Count == 0)
                        Console.WriteLine("Unfortunately, there is no winner for this draw. Good luck for next time!");
                    else
                        DisplayWinner(winners);

                    Console.WriteLine("\n");
                    Console.WriteLine("Would you like to continue for next draw (enter N or No or Exit for exit)? ");
                    line = Console.ReadLine();
                    if (string.Compare(line, "exit", true) == 0)
                        Environment.Exit(0);
                    else if (string.Compare(line, "N", true) == 0 || string.Compare(line, "No", true) == 0)
                    {
                        Console.WriteLine("Simutation application terminated!");
                        loopDraw = false;
                    }

                    LotteryRepository.TheNthDraw++;
                    firstTime = false;
                }
            }
        }

        private static void DisplayWinner(List<Winner> winners)
        {
            string winnerList = string.Empty;

            Console.Write("1st ball: \n");
            foreach (Winner winner in winners)
            {
                if (string.Compare(winner.ThePrize.ToString(), "First", true) == 0)
                    winnerList += winner.Name + ": " + winner.PrizeValue.ToString("$0.00") + "\t";
            }
            Console.WriteLine(winnerList + "\n");

            Console.Write("2nd ball: \n");
            winnerList = string.Empty;
            foreach (Winner winner in winners)
            {
                if (string.Compare(winner.ThePrize.ToString(), "Second", true) == 0)
                    winnerList += winner.Name + ": " + winner.PrizeValue.ToString("$0.00") + "\t";
            }
            Console.WriteLine(winnerList + "\n");

            Console.Write("3rd ball: \n");
            winnerList = string.Empty;
            foreach (Winner winner in winners)
            {
                if (string.Compare(winner.ThePrize.ToString(), "Third", true) == 0)
                    winnerList += winner.Name + ": " + winner.PrizeValue.ToString("$0.00") + "\t";
            }
            Console.WriteLine(winnerList + "\n");

            //foreach (Winner winner in winners)
            //{
            //    if (string.Compare(winner.ThePrize.ToString(), "First", true) == 0)
            //        Console.Write("1st ball" + "\t");
            //    else if (string.Compare(winner.ThePrize.ToString(), "Second", true) == 0)
            //        Console.Write("2nd ball" + "\t");
            //    else if (string.Compare(winner.ThePrize.ToString(), "Third", true) == 0)
            //        Console.Write("3rd ball" + "\t");
            //}
            //Console.WriteLine();

            //foreach (Winner winner in winners)
            //{
            //    Console.Write(winner.Name + ": " + winner.PrizeValue.ToString("$0.00") + "\t");
            //}
            //Console.WriteLine();
        }

    }
}
