using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model;
using DataLevel;

namespace ModelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ClockworkDataBase db = new ClockworkDataBase();

            //UserMessage m = new UserMessage(1, 2, "Hello, friend!", DateTime.Now);
            //Console.WriteLine(db.InsertUserMessage(m));
            //Console.WriteLine(db.ReadUserMessages(1, out int error_rows).Count);

            //Tournament tournament = new Tournament(@"La Liga 2020\2021", "Small", Tournament.TournamentTypes.PlayOff1);
            //Console.WriteLine(db.InsertTournament(tournament));

            //foreach(var el in db.ReadTeams(1, out int error))
            //{
            //    Console.WriteLine(el.Name);
            //}

            //foreach (var el in db.ReadMatches())
            //{
            //    Console.WriteLine(el.Id + " " + el.Home.Name + " vs " + el.Away.Name);
            //    Console.WriteLine("\n-------------------------\n");
            //    foreach (var e in el.HomeEvents)
            //    {
            //        Console.WriteLine(e.Id + " " + e.Minute);
            //    }
            //}

            //Console.WriteLine("Arsenal".ToUpper().Contains("ar".ToUpper()));

            //BetsList list = new BetsList();

            //Team homeTeam = new Team("Barca", "Camp Now", "Pep", 1);
            //Team awayTeam = new Team("Real", "Berna", "Mour", 2);
            //Tournament tour = new Tournament("Tournam1", "TTT", Tournament.TournamentTypes.League, 1);

            //Match match = new Match(homeTeam, awayTeam, tour, null, null, null);
            //match.SetId(1);

            //list.AddBet(BetBase.BetTypes.W1, 1.32, match, "some text", 2.5);
            //list.AddBet(BetBase.BetTypes.W2, 1.32, match, "some text", 0.0);
            //list.AddBet(BetBase.BetTypes.W1X, 1.32, match, "some text", 2.5);
            //list.AddBet(BetBase.BetTypes.W2X, 1.32, match, "some text", 2.5);
            //list.AddBet(BetBase.BetTypes.X, 1.32, match, "some text", 2.5);

            //list.AddBet(BetBase.BetTypes.TOTAL_MORE, 1.32, match, "some text", 2.5);
            //list.AddBet(BetBase.BetTypes.TOTAL1_MORE, 1.32, match, "some text", 2.5);
            //list.AddBet(BetBase.BetTypes.TOTAL2_LESS, 1.32, match, "some text", 2.5);
            //list.AddBet(BetBase.BetTypes.TOTAL_FIRTS_HALF_LESS, 1.32, match, "some text", 2.5);
            //list.AddBet(BetBase.BetTypes.TOTAL_SECOND_HALF_LESS, 1.32, match, "some text", 2.5);
            //list.AddBet(BetBase.BetTypes.CUSTOM, 1.32, match, "some text", 2.5);

            //list.ConsoleOutput();
            //Console.WriteLine(list.ComputeOutcome(match));
            //list.ConsoleOutput();

            Console.WriteLine(Double.TryParse("321332131232132131321321321321312321312321232", out double realOdd).ToString());
            Console.WriteLine(realOdd);
        }
    }
}
