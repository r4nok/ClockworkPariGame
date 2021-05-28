using Model;
using System;
using System.Windows;
using WpfApplication.ViewModels.ViewModelTypes;

namespace WpfApplication.ViewModels.WindowsVM
{
    public class AddAndEditTeamWindowVM : ViewModelBase
    {
        public ViewModels.PagesVM.TournamentsPageVM ParentVM { set; get; }

        public AddAndEditTeamWindowVM(ViewModels.PagesVM.TournamentsPageVM parentVM)
        {
            EditName    = new EditTextBox();
            Edit1Par    = new EditTextBox(); // manager or abbreviation
            Edit2Par    = new EditTextBox(); // stadium or tournamentType

            ParentVM = parentVM;
        }

        public void Update(EditType editType)
        {
            EditTypeWindow = editType;
        }

        private string _title_label;
        public string TitleLabel
        {
            get { return _title_label; }
            set
            {
                _title_label = value;
                OnPropertyChanged(nameof(TitleLabel));
            }
        }

        private EditTextBox _edit_name;
        private EditTextBox _edit_manager;
        private EditTextBox _edit_stadium;
        public EditTextBox EditName
        {
            get { return _edit_name; }
            set
            {
                _edit_name = value;
                OnPropertyChanged(nameof(EditName));
            }
        }
        public EditTextBox Edit1Par
        {
            get { return _edit_manager; }
            set
            {
                _edit_manager = value;
                OnPropertyChanged(nameof(Edit1Par));
            }
        }
        public EditTextBox Edit2Par
        {
            get { return _edit_stadium; }
            set
            {
                _edit_stadium = value;
                OnPropertyChanged(nameof(Edit2Par));
            }
        }

        private EditType _edit_type_window;
        public EditType EditTypeWindow
        {
            get { return _edit_type_window; }
            set
            {
                _edit_type_window = value;

                SetEnterLabel();

                switch (_edit_type_window)
                {
                    case EditType.EditTournament:
                        {
                            EditName.SetText(ParentVM.EditingTournament.Name ?? "Null");
                            Edit1Par.SetText(ParentVM.EditingTournament.Abbreviation ?? "Null");
                            Edit2Par.SetText(ParentVM.EditingTournament.TournamentType.ToString() ?? "Null");

                            TitleLabel = "Tournament editing";
                            break;
                        }
                    case EditType.CreateTournament:
                        {
                            EditName.SetText("");
                            Edit1Par.SetText("");
                            Edit2Par.SetText("");

                            TitleLabel = "Tournament creating";
                            break;
                        }
                    case EditType.EditTeam:
                        {
                            EditName.SetText(ParentVM.EditingTeam.Name ?? "Null");
                            Edit1Par.SetText(ParentVM.EditingTeam.Manager ?? "Null");
                            Edit2Par.SetText(ParentVM.EditingTeam.Stadium ?? "Null");

                            TitleLabel = "Team editing";
                            break;
                        }
                    case EditType.CreateTeam:
                        {
                            EditName.SetText("");
                            Edit1Par.SetText("");
                            Edit2Par.SetText("");

                            TitleLabel = "Team creating";
                            break;
                        }
                }

                OnPropertyChanged(nameof(EditTypeWindow));
            }
        }

        private void SetEnterLabel()
        {
            if (EditTypeWindow == EditType.EditTournament || EditTypeWindow == EditType.CreateTournament)
            {
                EditName.EnterLabel = "Enter tournament name";
                Edit1Par.EnterLabel = "Enter tournament abbreviation";
                Edit2Par.EnterLabel = "Enter tournament type";
            }

            if (EditTypeWindow == EditType.EditTeam || EditTypeWindow == EditType.CreateTeam)
            {
                EditName.EnterLabel = "Enter team name";
                Edit1Par.EnterLabel = "Enter manager";
                Edit2Par.EnterLabel = "Enter stadium";
            }
        }

        private RelayCommand _close_window_command;
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return _close_window_command ?? (new RelayCommand(obj =>
                {
                    try
                    {
                        ((View.Windows.AddAndEditTeam)obj).Close();
                        ParentVM.IsAddAndEditTeamWindowOpened = false;
                    }
                    catch (Exception ex) { MessageBox.Show("Can not close this window!", "Window Error", MessageBoxButton.OK, MessageBoxImage.Error); }
                }));
            }
        }

        private RelayCommand _apply_command;
        public RelayCommand ApplyCommand
        {
            get
            {
                return _apply_command ?? (new RelayCommand(obj =>
                {
                    CreateTournament();
                    EditTournament();
                    CreateTeam();
                    EditTeam();
                }));
            }
        }

        private void CreateTournament()
        {
            if (EditTypeWindow == EditType.CreateTournament)
            {
                Tournament editingTournament = new Tournament();

                EditName.ApplyName(editingTournament);
                Edit1Par.ApplyAbbreviation(editingTournament);
                Edit2Par.ApplyTournamentType(editingTournament);

                if (editingTournament.IsReadyToDB())
                {
                    switch (ViewModels.WindowsVM.MainWindowVM.db.InsertTournament(editingTournament))
                    {
                        case DataLevel.DataActionResults.ALL_RIGHT:
                            {
                                ParentVM.EditingTournament = editingTournament;
                                ParentVM.AddTournament();
                                break;
                            }
                        case DataLevel.DataActionResults.NOT_UNIQUE_EX:
                            {
                                EditName.SetIncorrect("Tournament name must be unique");
                                break;
                            }
                        default:
                            {
                                MessageBox.Show("You got one of data base exceptions", "DB error!", MessageBoxButton.OK, MessageBoxImage.Error); break;
                                break;
                            }
                    }
                }
            }
        }
        private void EditTournament()
        {
            if (EditTypeWindow == EditType.EditTournament && ParentVM.EditingTournament != null)
            {
                Tournament editingTournament = new Tournament();

                EditName.ApplyName(editingTournament);
                Edit1Par.ApplyAbbreviation(editingTournament);
                Edit2Par.ApplyTournamentType(editingTournament);

                if (editingTournament.IsReadyToDB())
                {
                    editingTournament.SetId((int)ParentVM.EditingTournament.Id);

                    switch (ViewModels.WindowsVM.MainWindowVM.db.UpdateTournament(editingTournament))
                    {
                        case DataLevel.DataActionResults.ALL_RIGHT:
                            {
                                ParentVM.EditingTournament = editingTournament;
                                ParentVM.EditTournament();
                                break;
                            }
                        case DataLevel.DataActionResults.NOT_UNIQUE_EX:
                            {
                                EditName.SetIncorrect("Tournament name must be unique");
                                break;
                            }
                        default:
                            {
                                MessageBox.Show("You got one of data base exceptions", "DB error!", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                            }
                    }
                }
            }
        }
        private void CreateTeam()
        {
            if (EditTypeWindow == EditType.CreateTeam && ParentVM.EditingTournament != null)
            {
                Team editingTeam = new Team();

                EditName.ApplyName(editingTeam);
                Edit1Par.ApplyManager(editingTeam);
                Edit2Par.ApplyStadium(editingTeam);

                if (editingTeam.IsReadyToDB())
                {
                    switch (ViewModels.WindowsVM.MainWindowVM.db.InsertTeam(editingTeam))
                    {
                        case DataLevel.DataActionResults.NOT_UNIQUE_EX:
                            {
                                // get original team
                                editingTeam = MainWindowVM.db.ReadTeam(editingTeam.Name);

                                if (editingTeam != null)
                                {
                                    EditName.TextContent = editingTeam.Name;
                                    Edit1Par.TextContent = editingTeam.Manager;
                                    Edit2Par.TextContent = editingTeam.Stadium;

                                    if (MainWindowVM.db.ConnectTournamentTeam(ParentVM.EditingTournament, editingTeam) == DataLevel.DataActionResults.ALL_RIGHT)
                                    {
                                        ParentVM.EditingTeam = editingTeam;
                                        MessageBox.Show(editingTeam.Id.ToString() + " " + ParentVM.EditingTournament.Id.ToString() + " " + "origin");
                                        ParentVM.AddTeam(editingTeam);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Team is already in tournament", "Team message!", MessageBoxButton.OK, MessageBoxImage.Information); break;
                                    }
                                }
                                break;
                            }
                        case DataLevel.DataActionResults.ALL_RIGHT:
                            {
                                if (MainWindowVM.db.ConnectTournamentTeam(ParentVM.EditingTournament, editingTeam) == DataLevel.DataActionResults.ALL_RIGHT)
                                {
                                    ParentVM.EditingTeam = editingTeam;
                                    ParentVM.AddTeam(editingTeam);
                                }
                                else MessageBox.Show("You got one of data base exceptions", "DB error!", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                            }
                        default:
                            {
                                MessageBox.Show("You got one of data base exceptions", "DB error!", MessageBoxButton.OK, MessageBoxImage.Error); break;
                            }
                    }
                }
            }
        }
        private void EditTeam()
        {
            if (EditTypeWindow == EditType.EditTeam && ParentVM.EditingTeam != null)
            {
                Team editingTeam = new Team();

                EditName.ApplyName(editingTeam);
                Edit1Par.ApplyManager(editingTeam);
                Edit2Par.ApplyStadium(editingTeam);

                if (editingTeam.IsReadyToDB())
                {
                    editingTeam.SetId((int)ParentVM.EditingTeam.Id);

                    switch (ViewModels.WindowsVM.MainWindowVM.db.UpdateTeam(editingTeam))
                    {
                        case DataLevel.DataActionResults.ALL_RIGHT:
                            {
                                ParentVM.EditingTeam = editingTeam;
                                ParentVM.EditTeam();
                                break;
                            }
                        case DataLevel.DataActionResults.NOT_UNIQUE_EX:
                            {
                                EditName.SetIncorrect("Team name must be unique");
                                break;
                            }
                        default:
                            {
                                MessageBox.Show("You got one of data base exceptions", "DB error!", MessageBoxButton.OK, MessageBoxImage.Error); break;
                            }
                    }
                }
            }
        }
    }
}
