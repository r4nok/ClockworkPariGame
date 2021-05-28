using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Model
{   public class Bet : BetBase, INotifyPropertyChanged
    {
        public enum OutcomeType
        {
            Win = 1,
            Lost,
            Refund,
            Sleep
        }

        private static double MIN_ODD = 1.01;
        private static double MAX_ODD = 300.00;

        private static Color SleepColor = Color.FromArgb(100, 205, 205, 205);
        private static Color WinColor = Color.FromArgb(100, 185, 228, 165);
        private static Color LostColor = Color.FromArgb(100, 240, 128, 128);
        private static Color RefundColor = Color.FromArgb(100, 234, 210, 103);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private SolidColorBrush _bet_color;
        public SolidColorBrush BetColor
        {
            get { return _bet_color; }
            set
            {
                _bet_color = value;
                OnPropertyChanged(nameof(BetColor));
            }
        }

        public int? Id { private set; get; }
        public void SetId(int? id)
        {
            if (id != null && id > 0 && id < int.MaxValue) Id = id;
        }

        private OutcomeType _outcome;
        public OutcomeType Outcome
        {
            get { return _outcome; }
            set
            {
                _outcome = value;

                if (_outcome == OutcomeType.Sleep) BetColor.Color = SleepColor;
                if (_outcome == OutcomeType.Win) BetColor.Color = WinColor;
                if (_outcome == OutcomeType.Lost) BetColor.Color = LostColor;
                if (_outcome == OutcomeType.Refund) BetColor.Color = RefundColor;

                OnPropertyChanged(nameof(Outcome));
            }
        }

        private double _odd;
        public double Odd
        {
            get { return _odd; }
            set
            {
                _odd = value;
                OnPropertyChanged(nameof(Odd));
            }
        }

        public Bet(BetTypes betType, double odd) : base(betType) { Init(odd); }
        public Bet(BetTypes betType, double odd, string teamNameOrDescription) : base(betType, teamNameOrDescription) { Init(odd); }
        public Bet(BetTypes betType, double odd, double grade = 0.0) : base(betType, grade) { Init(odd); }
        public Bet(BetTypes betType, double odd, string teamName, double grade = 0.0) : base(betType, teamName, grade) { Init(odd); }
        private void Init(double odd)
        {
            BetColor = new SolidColorBrush();
            Outcome = OutcomeType.Sleep;
            Odd = odd;
        }

        public void ComputeOutcome(Match match)
        {
            switch (BetType)
            {
                case BetTypes.W1:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home1halh + goals.home2halh > goals.away1halh + goals.away2halh ? OutcomeType.Win : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.W2:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home1halh + goals.home2halh < goals.away1halh + goals.away2halh ? OutcomeType.Win : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.X:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home1halh + goals.home2halh == goals.away1halh + goals.away2halh ? OutcomeType.Win : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.W1X:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home1halh + goals.home2halh >= goals.away1halh + goals.away2halh ? OutcomeType.Win : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.W2X:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home1halh + goals.home2halh <= goals.away1halh + goals.away2halh ? OutcomeType.Win : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.TOTAL_MORE:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home1halh + goals.home2halh + goals.away1halh + goals.away2halh > Goals ? OutcomeType.Win :
                            (goals.home1halh + goals.home2halh + goals.away1halh + goals.away2halh == Goals) ? OutcomeType.Refund : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.TOTAL_LESS:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home1halh + goals.home2halh + goals.away1halh + goals.away2halh < Goals ? OutcomeType.Win :
                            (goals.home1halh + goals.home2halh + goals.away1halh + goals.away2halh == Goals) ? OutcomeType.Refund : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.TOTAL1_MORE:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home1halh + goals.home2halh > Goals ? OutcomeType.Win :
                            (goals.home1halh + goals.home2halh == Goals) ? OutcomeType.Refund : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.TOTAL1_LESS:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home1halh + goals.home2halh < Goals ? OutcomeType.Win :
                            (goals.home1halh + goals.home2halh == Goals) ? OutcomeType.Refund : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.TOTAL2_MORE:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.away1halh + goals.away2halh > Goals ? OutcomeType.Win :
                            (goals.away1halh + goals.away2halh == Goals) ? OutcomeType.Refund : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.TOTAL2_LESS:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.away1halh + goals.away2halh < Goals ? OutcomeType.Win :
                            (goals.away1halh + goals.away2halh == Goals) ? OutcomeType.Refund : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.TOTAL_FIRTS_HALF_MORE:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home1halh + goals.away1halh > Goals ? OutcomeType.Win :
                            (goals.home1halh + goals.away1halh == Goals) ? OutcomeType.Refund : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.TOTAL_FIRTS_HALF_LESS:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home1halh + goals.away1halh < Goals ? OutcomeType.Win :
                            (goals.home1halh + goals.away1halh == Goals) ? OutcomeType.Refund : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.TOTAL_SECOND_HALF_MORE:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home2halh + goals.away2halh > Goals ? OutcomeType.Win :
                            (goals.home2halh + goals.away2halh == Goals) ? OutcomeType.Refund : OutcomeType.Lost;
                        break;
                    }
                case BetTypes.TOTAL_SECOND_HALF_LESS:
                    {
                        var goals = TeamHalfGoals(match);
                        Outcome = goals.home2halh + goals.away2halh < Goals ? OutcomeType.Win :
                            (goals.home2halh + goals.away2halh == Goals) ? OutcomeType.Refund : OutcomeType.Lost;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void SetWin() { Outcome = OutcomeType.Win; }
        public void SetLost() { Outcome = OutcomeType.Lost; }
        public void SetRefund() { Outcome = OutcomeType.Refund; }
        public void SetSleep() { Outcome = OutcomeType.Sleep; }

        public static bool isOddValid(double odd)
        {
            return (odd >= MIN_ODD && odd <= MAX_ODD) ? true : false;
        }
        public static bool isOddValid(string odd)
        {
            if (!Double.TryParse(odd, out double result)) return false;
            return isOddValid(result);
        }
        public static bool isGoalGradeValid(double goals)
        {
            return (goals % 0.5 == 0.0) ? true : false;
        }
        public static bool isGoalGradeValid(string goals)
        {
            if (!Double.TryParse(goals, out double result)) return false;
            return isGoalGradeValid(result);
        }

        private (double home1halh, double home2halh, double away1halh, double away2halh) TeamHalfGoals(Match match)
        {
            double g1_1halh = 0.0;
            double g1_2halh = 0.0;
            double g2_1halh = 0.0;
            double g2_2halh = 0.0;
            foreach (var el in match.HomeEvents)
            {
                if (el.Type == Events.EventTypes.Goal && (el.Minute > 0 && el.Minute <= 45)) g1_1halh++;
                if (el.Type == Events.EventTypes.Goal && (el.Minute > 45 && el.Minute <= 90)) g1_2halh++;
            }
            foreach (var el in match.AwayEvents)
            {
                if (el.Type == Events.EventTypes.Goal && (el.Minute > 0 && el.Minute <= 45)) g2_1halh++;
                if (el.Type == Events.EventTypes.Goal && (el.Minute > 45 && el.Minute <= 90)) g2_2halh++;
            }

            return (home1halh: g1_1halh, home2halh: g1_2halh, away1halh: g2_1halh, away2halh: g2_2halh);
        }
    }
}
