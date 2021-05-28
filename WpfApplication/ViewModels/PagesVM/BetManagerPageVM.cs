using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfApplication.ViewModels.ViewModelTypes;

namespace WpfApplication.ViewModels.PagesVM
{
    public class BetManagerPageVM : ViewModelBase
    {
        public BetManagerPageVM()
        {
            MatchList = WindowsVM.MainWindowVM.MatchList;
            SearchTeam = "";

            BetType = BetBase.BetTypes.CUSTOM;
            BetDescriptionColor = new SolidColorBrush(defaultWhiteColor);
            OddColor            = new SolidColorBrush(defaultWhiteColor);

            isBetsOutcome = false;
        }

        private static string OddPattern = @"^[0-9\,]+$";

        private static Color defaultWhiteColor  = Colors.White;
        private static Color wrongColor         = Color.FromRgb(255, 87, 87);

        // FOR SEARCHING
        private string _search_team;
        public string SearchTeam
        {
            get { return _search_team; }
            set
            {
                _search_team = value;
                UpdateMatchList();
                OnPropertyChanged(nameof(SearchTeam));
            }
        }
        // FOR SORING
        private bool _is_upcoming;
        private bool _is_online;
        private bool _is_completed;
        public bool IsUpcoming
        {
            get { return _is_upcoming; }
            set
            {
                _is_upcoming = value;
                UpdateMatchList();
                OnPropertyChanged(nameof(IsUpcoming));
            }
        }
        public bool IsOnline
        {
            get { return _is_online; }
            set
            {
                _is_online = value;
                UpdateMatchList();
                OnPropertyChanged(nameof(IsOnline));
            }
        }
        public bool IsCompleted
        {
            get { return _is_completed; }
            set
            {
                _is_completed = value;
                UpdateMatchList();
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        private void UpdateMatchList()
        {
            foreach (var el in MatchList)
            {
                el.ItemVisibility = Visibility.Collapsed;
                if (el.matchType == MatchItem.MatchType.Online      && IsOnline     && (el.Home.Name.ToUpper().Contains(SearchTeam.ToUpper()) 
                    || el.Away.Name.ToUpper().Contains(SearchTeam.ToUpper()))) { el.ItemVisibility = Visibility.Visible; continue; }
                if (el.matchType == MatchItem.MatchType.Upcoming    && IsUpcoming   && (el.Home.Name.ToUpper().Contains(SearchTeam.ToUpper()) 
                    || el.Away.Name.ToUpper().Contains(SearchTeam.ToUpper()))) { el.ItemVisibility = Visibility.Visible; continue; }
                if (el.matchType == MatchItem.MatchType.Completed   && IsCompleted  && (el.Home.Name.ToUpper().Contains(SearchTeam.ToUpper()) 
                    || el.Away.Name.ToUpper().Contains(SearchTeam.ToUpper()))) { el.ItemVisibility = Visibility.Visible; continue; }
            }
        }


        private bool _is_selected_upcoming;
        private bool _is_selected_online;
        private bool _is_selected_completed;
        private bool _is_selected_hidden;
        private bool _is_selected_visible;
        public bool IsSelectedUpcoming
        {
            get { return _is_selected_upcoming; }
            set
            {
                if (value == true && SelectedMatch.MatchEvents.Count != 0 && (IsSelectedOnline || IsSelectedCompleted))
                {
                    if (MessageBox.Show("rly?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes) SelectedMatch.ClearAllEvents();
                }

                _is_selected_upcoming = value;

                if (_is_selected_upcoming)
                {
                    SelectedMatchEnentsListVisibility = Visibility.Collapsed;
                    Enability = false;
                    if (SelectedMatch != null)
                    {
                        if (WindowsVM.MainWindowVM.db.UpdateMatch(SelectedMatch, Match.MatchType.Upcoming) == DataLevel.DataActionResults.ALL_RIGHT)
                            SelectedMatch.matchType = Match.MatchType.Upcoming;
                    }
                }

                OnPropertyChanged(nameof(IsSelectedUpcoming));
            }
        }
        public bool IsSelectedOnline
        {
            get { return _is_selected_online; }
            set
            {
                _is_selected_online = value;

                if (_is_selected_online)
                {
                    SelectedMatchEnentsListVisibility = Visibility.Visible;
                    Enability = true;
                    if (SelectedMatch != null)
                    {
                        if (WindowsVM.MainWindowVM.db.UpdateMatch(SelectedMatch, Match.MatchType.Online) == DataLevel.DataActionResults.ALL_RIGHT)
                            SelectedMatch.matchType = Match.MatchType.Online;
                    }
                }

                OnPropertyChanged(nameof(IsSelectedOnline));
            }
        }
        public bool IsSelectedCompleted
        {
            get { return _is_selected_completed; }
            set
            {
                _is_selected_completed = value;

                if (_is_selected_completed)
                {
                    SelectedMatchEnentsListVisibility = Visibility.Visible;
                    Enability = false;
                    if (SelectedMatch != null)
                    {
                        if (WindowsVM.MainWindowVM.db.UpdateMatch(SelectedMatch, Match.MatchType.Completed) == DataLevel.DataActionResults.ALL_RIGHT)
                            SelectedMatch.matchType = Match.MatchType.Completed;
                    }
                }

                OnPropertyChanged(nameof(IsSelectedCompleted));
            }
        }
        public bool IsSelectedHidden
        {
            get { return _is_selected_hidden; }
            set
            {
                _is_selected_hidden = value;

                if (_is_selected_hidden && SelectedMatch != null)
                        if (WindowsVM.MainWindowVM.db.UpdateMatch(SelectedMatch, false) == DataLevel.DataActionResults.ALL_RIGHT)
                            SelectedMatch.MatchVisible = false;

                OnPropertyChanged(nameof(IsSelectedHidden));
            }
        }
        public bool IsSelectedVisible
        {
            get { return _is_selected_visible; }
            set
            {
                _is_selected_visible = value;
                if (_is_selected_visible && SelectedMatch != null)
                    if (WindowsVM.MainWindowVM.db.UpdateMatch(SelectedMatch, true) == DataLevel.DataActionResults.ALL_RIGHT)
                        SelectedMatch.MatchVisible = true;
                OnPropertyChanged(nameof(IsSelectedVisible));
            }
        }


        private Visibility _selected_match_enents_list_visibility;
        public Visibility SelectedMatchEnentsListVisibility
        {
            get { return _selected_match_enents_list_visibility; }
            set
            {
                _selected_match_enents_list_visibility = value;
                OnPropertyChanged(nameof(SelectedMatchEnentsListVisibility));
            }
        }
        private bool _enability;
        public bool Enability
        {
            get { return _enability; }
            set
            {
                _enability = value;
                OnPropertyChanged(nameof(Enability));
            }
        }


        private ObservableCollection<MatchItem> _match_list;
        public ObservableCollection<MatchItem> MatchList
        {
            get { return _match_list; }
            set
            {
                _match_list = value;
                OnPropertyChanged(nameof(MatchList));
            }
        }

        private MatchItem _selected_match;
        public MatchItem SelectedMatch
        {
            get { return _selected_match; }
            set
            {
                _selected_match = value;

                IsSelectedUpcoming = IsSelectedOnline = IsSelectedCompleted = false;

                if (_selected_match != null)
                {
                    if (_selected_match.matchType == Match.MatchType.Upcoming)  IsSelectedUpcoming = true;
                    if (_selected_match.matchType == Match.MatchType.Online)    IsSelectedOnline = true;
                    if (_selected_match.matchType == Match.MatchType.Completed) IsSelectedCompleted = true;
                    if (_selected_match.MatchVisible == true) IsSelectedVisible = true; else IsSelectedHidden = true;
                }

                OnPropertyChanged(nameof(SelectedMatch));
            }
        }

        private RelayCommand _sort_events_command;
        public RelayCommand SortEventCommand
        {
            get
            {
                return _sort_events_command ?? (new RelayCommand(obj =>
                {
                    if (SelectedMatch != null) SelectedMatch.SortEvents();
                }));
            }
        }
        private RelayCommand _goal_score_home_command;
        public RelayCommand GoalScoreHomeCommand
        {
            get
            {
                return _goal_score_home_command ?? (new RelayCommand(obj =>
                {
                    if (SelectedMatch != null && !IsSelectedUpcoming)
                    {
                        SelectedMatch.HomeTeamScore();
                        UpdateOutome(false);
                    }
                }));
            }
        }

        private RelayCommand _goal_score_away_command;
        public RelayCommand GoalScoreAwayCommand
        {
            get
            {
                return _goal_score_away_command ?? (new RelayCommand(obj =>
                {
                    if (SelectedMatch != null && !IsSelectedUpcoming)
                    {
                        SelectedMatch.AwayTeamScore();
                        UpdateOutome(false);
                    }
                }));
            }
        }

        private RelayCommand _cancel_match_event;
        public RelayCommand CancelMatchEvent
        {
            get
            {
                return _cancel_match_event ?? (new RelayCommand(obj =>
                {
                    if (SelectedMatch != null && !IsSelectedUpcoming)
                    {
                        if (obj is int)
                        {
                            SelectedMatch.CancelEvent((int)obj);
                            UpdateOutome(false);
                        }
                    }
                }));
            }
        }

        private RelayCommand _delete_match;
        public RelayCommand DeleteMatch
        {
            get
            {
                return _delete_match ?? (new RelayCommand(obj =>
                {
                    foreach(var el in MatchList)
                    {
                        if (el.Equals(SelectedMatch)) { MatchList.Remove(el); SelectedMatch = null; break; }
                    }
                }));
            }
        }


        private BetBase.BetTypes betType;
        public BetBase.BetTypes BetType
        {
            get { return betType; }
            set
            {
                if (betType != value)
                {
                    betType = value;
                    OnPropertyChanged(nameof(BetType));
                }
            }
        }

        private string _bet_description;
        public string BetDescription
        {
            get { return _bet_description; }
            set
            {
                _bet_description = value;
                OnPropertyChanged(nameof(BetDescription));
            }
        }

        private string _odd;
        public string Odd
        {
            get { return _odd; }
            set
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(value, OddPattern) && !String.IsNullOrEmpty(value)
                    && !value.Substring(value.IndexOf(",")+1).Contains(",") && value.Length < 7)
                {
                    _odd = value;
                    OddColor.Color = defaultWhiteColor;
                }

                if (String.IsNullOrEmpty(value)) { _odd = ""; OddColor.Color = defaultWhiteColor; }

                OnPropertyChanged(nameof(Odd));
            }
        }

        private SolidColorBrush _odd_color;
        private SolidColorBrush _bet_description_color;
        public SolidColorBrush OddColor
        {
            get { return _odd_color; }
            set
            {
                _odd_color = value;
                OnPropertyChanged(nameof(OddColor));
            }
        }
        public SolidColorBrush BetDescriptionColor
        {
            get { return _bet_description_color; }
            set
            {
                _bet_description_color = value;
                OnPropertyChanged(nameof(BetDescriptionColor));
            }
        }

        private bool CheckOdd()
        {
            if (!Bet.isOddValid(Odd)) { OddColor.Color = wrongColor; return false; }
            return true;
        }
        private bool CheckBetDescription()
        {
            if (!new[] { Bet.BetTypes.TOTAL1_LESS, Bet.BetTypes.TOTAL1_MORE, Bet.BetTypes.TOTAL2_LESS, Bet.BetTypes.TOTAL2_MORE,
            Bet.BetTypes.TOTAL_FIRTS_HALF_LESS, Bet.BetTypes.TOTAL_FIRTS_HALF_MORE, Bet.BetTypes.TOTAL_LESS,
            Bet.BetTypes.TOTAL_MORE, Bet.BetTypes.TOTAL_SECOND_HALF_LESS, Bet.BetTypes.TOTAL_SECOND_HALF_MORE}.All(x => x != BetType))
                if (Bet.isGoalGradeValid(BetDescription)) return true;
                else { BetDescriptionColor.Color = wrongColor; return false; }

            return true;
        }

        private RelayCommand _create_bet;
        public RelayCommand CreateBetCommand
        {
            get
            {
                return _create_bet ?? (new RelayCommand(obj =>
                {
                    OddColor.Color = defaultWhiteColor;
                    BetDescriptionColor.Color = defaultWhiteColor;

                    if (SelectedMatch == null || SelectedMatch.IsNull()) { MessageBox.Show("Select match", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                    if (!CheckOdd()) {MessageBox.Show("Eneter correct odd", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                    if (!CheckBetDescription()) { MessageBox.Show("Eneter correct grade in description", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }

                    double grade = 0.0;
                    Double.TryParse(BetDescription, out grade);
                    if (!SelectedMatch.BetList.AddBet(BetType, Double.Parse(Odd), SelectedMatch, BetDescription, grade))
                    { MessageBox.Show("Not added into BetList", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                    else
                    {
                        if (WindowsVM.MainWindowVM.db.InsertBet(SelectedMatch.BetList.Bets.Last(), SelectedMatch) == DataLevel.DataActionResults.ALL_RIGHT) return;
                        else
                        {
                            MessageBox.Show("BET IS Not added into DB", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            SelectedMatch.BetList.RemoveBet(SelectedMatch.BetList.Bets.Last());
                        }
                    }
                }));
            }
        }
        private RelayCommand _compute_outcome;
        private bool isBetsOutcome = false;
        public RelayCommand ComputeOutcome
        {
            get
            {
                return _compute_outcome ?? (new RelayCommand(obj =>
                {
                    UpdateOutome(true);
                }));
            }
        }

        private void UpdateOutome(bool isChangeStatus)
        {
            if (SelectedMatch != null && !isBetsOutcome)
            { 
                SelectedMatch.BetList.ComputeOutcome(SelectedMatch);
                if (isChangeStatus) isBetsOutcome = !isBetsOutcome;
            }
            else
            {
                if (SelectedMatch != null && isBetsOutcome)
                {
                    SelectedMatch.BetList.SleepOutcome(SelectedMatch);
                    if (isChangeStatus) isBetsOutcome = !isBetsOutcome;
                }
            }
        }

        private RelayCommand _remove_bet;
        public RelayCommand RemoveBetCommand
        {
            get
            {
                return _remove_bet ?? (new RelayCommand(obj =>
                {
                    if (obj is int)
                    {
                        if (WindowsVM.MainWindowVM.db.RemoveBet((int)obj) == DataLevel.DataActionResults.ALL_RIGHT)
                            if (SelectedMatch.BetList.RemoveBet((int)obj))
                                return;
                            else
                                MessageBox.Show("BET IS Not removed from list", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        MessageBox.Show("BET IS Not removed from DB", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }));
            }
        }

    }
}
