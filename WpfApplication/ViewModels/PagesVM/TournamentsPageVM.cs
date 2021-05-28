using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DataLevel;
using Model;
using WpfApplication.ViewModels.ViewModelTypes;

namespace WpfApplication.ViewModels.PagesVM
{
    public class TournamentsPageVM : ViewModelBase
    {
        public TournamentsPageVM()
        {
            TournamentTeamList = new ObservableCollection<TournamentTeam>();
            TournamentTeam.Update(TournamentTeamList);

            IsAddAndEditTeamWindowOpened = false;
        }

        private ObservableCollection<TournamentTeam> _tournaments_team_list;
        public ObservableCollection<TournamentTeam> TournamentTeamList
        {
            get { return _tournaments_team_list; }
            set
            {
                _tournaments_team_list = value;
                OnPropertyChanged(nameof(TournamentTeamList));
            }
        }

        private TournamentTeam _selected_tournament;
        public TournamentTeam SelectedTournament
        {
            get { return _selected_tournament; }
            set
            {
                _selected_tournament = value;
                OnPropertyChanged(nameof(SelectedTournament));
            }
        }

        public Tournament EditingTournament { set; get; }
        public Team EditingTeam { set; get; }

        public bool IsAddAndEditTeamWindowOpened { set; get; }

        // VIEW MODEL CHANGES
        public void AddTournament()
        {
            TournamentTeam ttObj = new TournamentTeam();
            ttObj.TournamentObj = EditingTournament;
            ttObj.Teams = new ObservableCollection<TeamTeam>();
            ttObj.TeamListState = true;

            TournamentTeamList.Add(ttObj);
        }
        public void EditTournament()
        {
            foreach(TournamentTeam tt in TournamentTeamList)
            {
                if (tt.TournamentObj.Id == EditingTournament.Id) { tt.TournamentObj = EditingTournament; break; }
            }
        }
        public void AddTeam(Team team)
        {
            foreach (TournamentTeam tt in TournamentTeamList)
            {
                if (tt.TournamentObj.Id == EditingTournament.Id)
                {
                    tt.Teams.Add(new TeamTeam(team));
                    tt.TeamsCountLabel = tt.Teams.Count + " teams,";
                    if (tt.Teams.Count == 1) tt.TeamListState = false;
                    break;
                }
            }
        }
        public void EditTeam()
        {
            if (EditingTeam != null)
            {
                foreach (TournamentTeam tt in TournamentTeamList)
                {
                    foreach (var te in tt.Teams)
                    {
                        if (te.TeamObj.Id == EditingTeam.Id) { te.Set(EditingTeam); break; }
                    }
                }
            }
        }

        private RelayCommand _extend_command;
        public RelayCommand ExtendCommand
        {
            get
            {
                return _extend_command ?? (new RelayCommand(obj =>
                {
                    Tournament tour = (Tournament)obj;
                    foreach(var t in TournamentTeamList)
                    {
                        if (t.TournamentObj.Equals(tour)) { t.TeamListState = !t.TeamListState; break; }
                    }
                }));
            }
        }

        private RelayCommand _open_edit_window_command;
        public RelayCommand OpenEditWindowCommand
        {
            get
            {
                return _open_edit_window_command ?? (new RelayCommand(obj =>
                {
                    if (!IsAddAndEditTeamWindowOpened)
                    {
                        if (obj is EditType)
                        {
                            EditType editType = (EditType)obj;
                            InitAddAndEditWindow(editType);
                        }
                        if (obj is Tuple<EditType, Tournament>)
                        {
                            Tuple<EditType, Tournament> tuple = (Tuple<EditType, Tournament>)obj;

                            EditingTournament = tuple.Item2;
                            InitAddAndEditWindow(tuple.Item1);
                        }
                        if (obj is Tuple<EditType, Team>)
                        {
                            Tuple<EditType, Team> tuple = (Tuple<EditType, Team>)obj;

                            EditingTeam = tuple.Item2;
                            InitAddAndEditWindow(tuple.Item1);
                        }
                    }
                }));
            }
        }

        private void InitAddAndEditWindow(EditType editType)
        {
            // create window
            View.Windows.AddAndEditTeam AddAndEditWindow = new View.Windows.AddAndEditTeam();
            // init data context for window
            AddAndEditWindow.DataContext = new ViewModels.WindowsVM.AddAndEditTeamWindowVM(this);
            // set editType
            ((WindowsVM.AddAndEditTeamWindowVM)AddAndEditWindow.DataContext).EditTypeWindow = editType;

            IsAddAndEditTeamWindowOpened = true;
            AddAndEditWindow.ShowDialog();
        }

        private RelayCommand _remove_team_command;
        public RelayCommand RemoveTeamCommand
        {
            get
            {
                return _remove_team_command ?? (new RelayCommand(obj =>
                {
                    if (obj is Tuple<Tournament, Team>)
                    {
                        Tuple<Tournament, Team> tuple = (Tuple<Tournament, Team>)obj;

                        if (WindowsVM.MainWindowVM.db.RemoveTeam(tuple.Item1, tuple.Item2) == DataActionResults.ALL_RIGHT)
                        {
                            foreach (TournamentTeam tt in TournamentTeamList)
                            {
                                if (tt.TournamentObj.Equals(tuple.Item1))
                                {
                                    tt.RemoveTeam(tuple.Item2);
                                    break;
                                }
                            }
                        }
                    }
                }));
            }
        }

        private RelayCommand _create_match;
        public RelayCommand CreateMatch
        {
            get
            {
                return _create_match ?? (new RelayCommand(obj =>
                {
                    if (obj is Tuple<ListBox, Tournament>)
                    {
                        Tuple<ListBox, Tournament> tuple = (Tuple<ListBox, Tournament>)obj;

                        ListBox mainListBox = (ListBox)tuple.Item1;
                        Tournament t        = (Tournament)tuple.Item2;

                        if (mainListBox.SelectedItems.Count != 2) { MessageBox.Show("Select 2 teams", "Bookmaker message", MessageBoxButton.OK, MessageBoxImage.Information); return; }

                        Match match = new Match(((TeamTeam)mainListBox.SelectedItems[0]).TeamObj, ((TeamTeam)mainListBox.SelectedItems[1]).TeamObj,
                            t, null, null, null);

                        DataLevel.DataActionResults result = WindowsVM.MainWindowVM.db.InsertMatch(match, t);

                        if (result == DataActionResults.ALL_RIGHT)
                        {
                            MessageBox.Show("Added", "Bookmaker message", MessageBoxButton.OK, MessageBoxImage.Information);
                            WindowsVM.MainWindowVM.MatchList.Add(new MatchItem(match));
                        }
                        else
                        {
                            MessageBox.Show("NOT Added", "Bookmaker message", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }));
            }
        }
    }
}
