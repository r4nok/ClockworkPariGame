using Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfApplication.ViewModels.ViewModelTypes
{
    /// <summary>
    /// View Model shell for tournament object, including teamList
    /// </summary>
    public class TournamentTeam : ViewModelBase
    {
        //tournament
        private Tournament _tournament_obj;
        // team list
        private ObservableCollection<TeamTeam> _teams;

        private string _tournament_type_label;      // label depends on tournament type
        private string _teams_count_label;          // label depends on teams count
        private bool _team_list_state;              // collapse or expand list
        private string _extend_image;               //expand image (0 or 180)
        private Visibility _team_list_visibility;   // visible or collapsed

        public Tournament TournamentObj
        {
            get { return _tournament_obj; }
            set
            {
                _tournament_obj = value;

                switch (TournamentObj.TournamentType)
                {
                    case Tournament.TournamentTypes.League:     TournamentTypeLabel = "Format: League"; break;
                    case Tournament.TournamentTypes.PlayOff1:   TournamentTypeLabel = "Format: One leg playoff"; break;
                    default:                                    TournamentTypeLabel = "Format: Two legs playoff"; break;
                }

                OnPropertyChanged(nameof(TournamentObj));
            }
        }
        public ObservableCollection<TeamTeam> Teams
        {
            get { return _teams; }
            set
            {
                _teams = value;
                OnPropertyChanged(nameof(Teams));
            }
        }

        public string TournamentTypeLabel
        {
            get { return _tournament_type_label; }
            set
            {
                _tournament_type_label = value;
                OnPropertyChanged(nameof(TournamentTypeLabel));
            }
        }
        public string TeamsCountLabel
        {
            get { return _teams_count_label; }
            set
            {
                _teams_count_label = value;
                OnPropertyChanged(nameof(TeamsCountLabel));
            }
        }
        public bool TeamListState
        {
            get { return _team_list_state; }
            set
            {
                _team_list_state = value;
                if (_team_list_state)
                {
                    TeamListVisibility = Visibility.Collapsed;
                    ExtendImage = "/View/Images/scroll_down_48px.png";
                }
                else
                {
                    if (Teams.Count != 0) TeamListVisibility = Visibility.Visible;
                    ExtendImage = "/View/Images/scroll_up_48px.png";
                }
            }
        }
        public string ExtendImage
        {
            get { return _extend_image; }
            set
            {
                _extend_image = value;
                OnPropertyChanged(nameof(ExtendImage));
            }
        }
        public Visibility TeamListVisibility
        {
            get { return _team_list_visibility; }
            set
            {
                _team_list_visibility = value;
                OnPropertyChanged(nameof(TeamListVisibility));
            }
        }

        /// <summary>
        /// Update all information via DB
        /// </summary>
        public static void Update(ObservableCollection<TournamentTeam> tournaments_teams)
        {
            tournaments_teams.Clear();
            foreach (var tournament in WindowsVM.MainWindowVM.db.ReadTournaments(out int error_rows))
            {
                TournamentTeam ttObj = new TournamentTeam();
                ttObj.TournamentObj = tournament;
                ttObj.Teams = new ObservableCollection<TeamTeam>();

                ttObj.TeamListState = true;

                foreach (var team in WindowsVM.MainWindowVM.db.ReadTeams(ttObj.TournamentObj.Id ?? 0, out int error_rows_teams))
                {
                    TeamTeam ttObj2 = new TeamTeam(team);
                    ttObj.Teams.Add(ttObj2);
                }

                ttObj.TeamsCountLabel = ttObj.Teams.Count + " teams,";
                tournaments_teams.Add(ttObj);
            }
        }

        /// <summary>
        /// Remove team from teamList if it has
        /// </summary>
        public void RemoveTeam(Team team)
        {
            int index = 0;
            foreach (var te in Teams)
            {
                if (te.TeamObj.Equals(team))
                {
                    Teams.RemoveAt(index);

                    TeamsCountLabel = Teams.Count + " teams,";
                    if (Teams.Count == 0) TeamListState = true;

                    return;
                }
                index++;
            }
        }
    }
}
