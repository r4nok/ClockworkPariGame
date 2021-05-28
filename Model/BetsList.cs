using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class BetsList : ViewModelBase
    {
        public BetsList()
        {
            Bets = new ObservableCollection<Bet>();
        }

        private ObservableCollection<Bet> _bets;
        public ObservableCollection<Bet> Bets
        {
            get { return _bets; }
            set
            {
                _bets = value;
                OnPropertyChanged(nameof(Bets));
            }
        }

        public bool AddBet(Bet.BetTypes betType, double odd, Match match, string description, double grade)
        {
            if (match.IsNull())                 return false;

            if (!Bet.isOddValid(odd))           return false;
            if (!Bet.isGoalGradeValid(grade))   return false;

            switch (betType)
            {
                case Bet.BetTypes.W1:
                    {
                        Bets.Add(new Bet(betType, odd, match.Home.Name));
                        return true;
                    }
                case Bet.BetTypes.W2:
                    {
                        Bets.Add(new Bet(betType, odd, match.Away.Name));
                        return true;
                    }
                case Bet.BetTypes.X:
                    {
                        Bets.Add(new Bet(betType, odd));
                        return true;
                    }
                case Bet.BetTypes.W1X:
                    {
                        Bets.Add(new Bet(betType, odd, match.Home.Name));
                        return true;
                    }
                case Bet.BetTypes.W2X:
                    {
                        Bets.Add(new Bet(betType, odd, match.Away.Name));
                        return true;
                    }
                case (Bet.BetTypes.TOTAL_MORE):
                    {
                        Bets.Add(new Bet(betType, odd, grade));
                        return true;
                    }
                case (Bet.BetTypes.TOTAL_LESS):
                    {
                        Bets.Add(new Bet(betType, odd, grade));
                        return true;
                    }
                case Bet.BetTypes.TOTAL1_MORE:
                    {
                        Bets.Add(new Bet(betType, odd, match.Home.Name, grade));
                        return true;
                    }
                case Bet.BetTypes.TOTAL1_LESS:
                    {
                        Bets.Add(new Bet(betType, odd, match.Home.Name, grade));
                        return true;
                    }
                case Bet.BetTypes.TOTAL2_MORE:
                    {
                        Bets.Add(new Bet(betType, odd, match.Away.Name, grade));
                        return true;
                    }
                case Bet.BetTypes.TOTAL2_LESS:
                    {
                        Bets.Add(new Bet(betType, odd, match.Away.Name, grade));
                        return true;
                    }
                case Bet.BetTypes.TOTAL_FIRTS_HALF_MORE:
                    {
                        Bets.Add(new Bet(betType, odd, grade));
                        return true;
                    }
                case Bet.BetTypes.TOTAL_FIRTS_HALF_LESS:
                    {
                        Bets.Add(new Bet(betType, odd, grade));
                        return true;
                    }
                case Bet.BetTypes.TOTAL_SECOND_HALF_MORE:
                    {
                        Bets.Add(new Bet(betType, odd, grade));
                        return true;
                    }
                case Bet.BetTypes.TOTAL_SECOND_HALF_LESS:
                    {
                        Bets.Add(new Bet(betType, odd, grade));
                        return true;
                    }
                case Bet.BetTypes.CUSTOM:
                    {
                        Bets.Add(new Bet(betType, odd, description));
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }

        }
        public bool RemoveBet(int betId)
        {
            foreach(var bet in Bets)
            {
                if (bet.Id == betId)
                {
                    Bets.Remove(bet);
                    return true;
                }
            }
            return false;
        }
        public bool RemoveBet(Bet removeBet)
        {
            foreach (var bet in Bets)
            {
                if (bet.Equals(removeBet))
                {
                    Bets.Remove(bet);
                    return true;
                }
            }
            return false;
        }
        public bool ComputeOutcome(Match match)
        {
            if (match.IsNull()) return false;

            foreach(var el in Bets)
            {
                el.ComputeOutcome(match);
            }
            return true;
        }
        public bool SleepOutcome(Match match)
        {
            if (match.IsNull()) return false;

            foreach (var el in Bets)
            {
                el.SetSleep();
            }
            return true;
        }

        public void ConsoleOutput()
        {
            Console.WriteLine("------------------------------------ " + Bets.Count.ToString() + " elements -----------------------------------");
            foreach (var bet in Bets)
            {
                Console.WriteLine(bet.Id.ToString() + " " + bet.BetType.ToString() + " " + bet.Description + " " + bet.Odd.ToString() + " " + bet.Outcome.ToString() + "\n");
            }
        }
    }
}
