using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Model
{
    public class Match : ViewModelBase
    {
        public enum MatchType
        {
            Upcoming = 1,
            Online,
            Completed
        }
        public int? Id { private set; get; }
        public void SetId(int? id)
        {
            if (id != null && id > 0 && id < int.MaxValue) Id = id;
        }

        private string _tournament_name;
        public string TournamentName
        {
            get { return _tournament_name; }
            set
            {
                _tournament_name = value;
                OnPropertyChanged(nameof(TournamentName));
            }
        }

        private MatchType _match_type;
        public MatchType matchType
        {
            get { return _match_type; }
            set
            {
                _match_type = value;
                OnPropertyChanged(nameof(matchType));
            }
        }

        private bool _match_visible;
        public bool MatchVisible
        {
            get { return _match_visible; }
            set
            {
                _match_visible = value;
                OnPropertyChanged(nameof(MatchVisible));
            }
        }

        private Team _home;
        public Team Home
        {
            get { return _home; }
            set
            {
                _home = value;
                OnPropertyChanged(nameof(Home));
            }
        }

        private Team _away;
        public Team Away
        {
            get { return _away; }
            set
            {
                _away = value;
                OnPropertyChanged(nameof(Away));
            }
        }
        public ObservableCollection<Events> HomeEvents { private set; get; }
        public ObservableCollection<Events> AwayEvents { private set; get; }

        private BetsList _bet_list;
        public BetsList BetList
        {
            get { return _bet_list; }
            set
            {
                _bet_list = value;
                OnPropertyChanged(nameof(BetList));
            }
        }

        public Match(Team home, Team away, Tournament tournament, ObservableCollection<Events> homeEvents,
            ObservableCollection<Events> awayEvents, int? id, MatchType match_type = MatchType.Upcoming, bool matchVisibility = false)
        {
            SetId(id);
            if (!home.IsNull()) Home = home;
            if (!away.IsNull()) Away = away;
            if (!tournament.IsNull()) TournamentName = tournament.Name;

            matchType = match_type;
            MatchVisible = matchVisibility;

            HomeEvents = homeEvents ?? new ObservableCollection<Events>();
            AwayEvents = awayEvents ?? new ObservableCollection<Events>();

            BetList = new BetsList();
        }

        public Match(Match match)
        {
            if (match != null)
            {
                Id = match.Id;

                Home = match.Home;
                Away = match.Away;
                TournamentName = match.TournamentName;

                matchType = match.matchType;
                MatchVisible = match.MatchVisible;

                HomeEvents = match.HomeEvents ?? new ObservableCollection<Events>();
                AwayEvents = match.AwayEvents ?? new ObservableCollection<Events>();

                BetList = match.BetList ?? new BetsList();
            }
        }

        public bool IsNull()
        {
            if (!Id.HasValue || Home == null || Away == null || TournamentName == null || HomeEvents == null || AwayEvents == null) return true;
            else return false;
        }

    }
}
