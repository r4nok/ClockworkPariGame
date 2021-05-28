using DataLevel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Model;
using WpfApplication.ViewModels.ViewModelTypes;

namespace WpfApplication.ViewModels.WindowsVM
{
    public class MainWindowVM : ViewModels.ViewModelBase
    {
        public static ClockworkDataBase db = new ClockworkDataBase();

        public static ObservableCollection<MatchItem> MatchList = new ObservableCollection<MatchItem>();

        public MainWindowVM()
        {
            InitPages();
            CurrentPage = GreetingPage;

            ColumnFrame = 0;
            ColumnFrameSpan = 3;
            VisibilityMenu = Visibility.Hidden;

            foreach(var el in db.ReadMatches())
            {
                MatchList.Add(new MatchItem(el));
            }
        }

        private View.Pages.Greeting _main_page;
        private View.Pages.Messenger _messenger_page;
        private View.Pages.Tournaments _tournaments_page;
        private View.Pages.BetManager _bet_manager_page;
        private Page _current_page;

        private int _column_frame;
        private int _column_frame_span;
        private Visibility _visibility_menu;

        private RelayCommand _open_menu;
        private RelayCommand _close_menu;
        private RelayCommand _open_greeting_page;
        private RelayCommand _open_messenger;
        private RelayCommand _open_tournaments;
        private RelayCommand _open_betmanager_page;

        public View.Pages.Greeting GreetingPage
        {
            get { return _main_page; }
            set { _main_page = value; }
        }
        public View.Pages.Messenger MessengerPage
        {
            get { return _messenger_page; }
            set { _messenger_page = value; }
        }
        public View.Pages.Tournaments TournamentsPage
        {
            get { return _tournaments_page; }
            set { _tournaments_page = value; }
        }
        public View.Pages.BetManager BetManagerPage
        {
            get { return _bet_manager_page; }
            set { _bet_manager_page = value; }
        }

        public Page CurrentPage
        {
            get { return _current_page; }
            set
            {
                _current_page = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private void InitPages()
        {
            GreetingPage = new View.Pages.Greeting();
            GreetingPage.DataContext = new ViewModels.PagesVM.GreetingPageVM();

            MessengerPage = new View.Pages.Messenger();
            MessengerPage.DataContext = new ViewModels.PagesVM.MessengerPageVM();

            TournamentsPage = new View.Pages.Tournaments();
            TournamentsPage.DataContext = new ViewModels.PagesVM.TournamentsPageVM();

            BetManagerPage = new View.Pages.BetManager();
            BetManagerPage.DataContext = new ViewModels.PagesVM.BetManagerPageVM();
        }

        public int ColumnFrame
        {
            get { return _column_frame; }
            set
            {
                _column_frame = value;
                OnPropertyChanged(nameof(ColumnFrame));
            }
        }
        public int ColumnFrameSpan
        {
            get { return _column_frame_span; }
            set
            {
                _column_frame_span = value;
                OnPropertyChanged(nameof(ColumnFrameSpan));
            }
        }
        public Visibility VisibilityMenu
        {
            get { return _visibility_menu; }
            set
            {
                _visibility_menu = value;
                OnPropertyChanged(nameof(VisibilityMenu));
            }
        }

        public RelayCommand OpenMenu
        {
            get
            {
                return _open_menu ?? (new RelayCommand(obj =>
                {
                    ColumnFrame = 1;
                    ColumnFrameSpan = 2;
                    VisibilityMenu = Visibility.Visible;
                }));
            }
        }
        public RelayCommand CloseMenu
        {
            get
            {
                return _close_menu ?? (new RelayCommand(obj =>
                {
                    ColumnFrame = 0;
                    ColumnFrameSpan = 3;
                    VisibilityMenu = Visibility.Hidden;
                }));
            }
        }
        public RelayCommand OpenGreetingPage
        {
            get
            {
                return _open_greeting_page ?? (new RelayCommand(obj =>
                {
                    CurrentPage = GreetingPage;
                }));
            }
        }
        public RelayCommand OpenMessenger
        {
            get
            {
                return _open_messenger ?? (new RelayCommand(obj =>
                {
                    CurrentPage = MessengerPage;
                }));
            }
        }
        public RelayCommand OpenTournaments
        {
            get
            {
                return _open_tournaments ?? (new RelayCommand(obj =>
                {
                    CurrentPage = TournamentsPage;
                }));
            }
        }
        public RelayCommand OpenBetManager
        {
            get
            {
                return _open_betmanager_page ?? (new RelayCommand(obj =>
                {
                    CurrentPage = BetManagerPage;
                }));
            }
        }

    }
}
