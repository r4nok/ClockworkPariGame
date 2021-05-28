using Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;

namespace WpfApplication.ViewModels.ViewModelTypes
{
    public class MatchItem : Match
    {
        private Visibility _item_visibility;
        public Visibility ItemVisibility
        {
            get { return _item_visibility; }
            set
            {
                _item_visibility = value;
                OnPropertyChanged(nameof(ItemVisibility));
            }
        }

        private ObservableCollection<MatchEventItem> _match_events;
        public ObservableCollection<MatchEventItem> MatchEvents
        {
            get { return _match_events; }
            set
            {
                _match_events = value;
                OnPropertyChanged(nameof(MatchEvents));
            }
        }

        private string _score;
        public string Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        public MatchItem(Match match) : base(match)
        {
            ItemVisibility = System.Windows.Visibility.Visible;

            MatchEvents = new ObservableCollection<MatchEventItem>();
            foreach(var el in HomeEvents)
            {
                MatchEvents.Add(new MatchEventItem(el, true));
            }
            foreach (var el in AwayEvents)
            {
                MatchEvents.Add(new MatchEventItem(el, false));
            }

            CountScore();
            SortEvents();
        }

        public void ClearAllEvents()
        {
            HomeEvents.Clear();
            AwayEvents.Clear();
            MatchEvents.Clear();
            CountScore();
        }
        public void CountScore()
        {
            int s1 = 0; int s2 = 0;
            foreach (var el in HomeEvents)
            {
                if (el.Type == Events.EventTypes.Goal) s1++;
            }
            foreach (var el in AwayEvents)
            {
                if (el.Type == Events.EventTypes.Goal) s2++;
            }
            Score = s1.ToString() + " : " + s2.ToString();
        }
        public void SortEvents()
        {
            MatchEvents = new ObservableCollection<MatchEventItem>(MatchEvents.OrderBy(i => i.Minute));
        }

        public void HomeTeamScore()
        {
            Events me = new Events(Events.EventTypes.Goal, 1);

            if (WindowsVM.MainWindowVM.db.InsertMatchEvent(me, Id ?? 1, true) == DataLevel.DataActionResults.ALL_RIGHT)
            {
                HomeEvents.Add(me);
                MatchEvents.Add(new MatchEventItem(me, true));

                CountScore();
                SortEvents();
            }
        }
        public void AwayTeamScore()
        {
            Events me = new Events(Events.EventTypes.Goal, 1);

            if (WindowsVM.MainWindowVM.db.InsertMatchEvent(me, Id ?? 1, false) == DataLevel.DataActionResults.ALL_RIGHT)
            {
                AwayEvents.Add(me);
                MatchEvents.Add(new MatchEventItem(me, false));

                CountScore();
                SortEvents();
            }
        }
        public void CancelEvent(int event_id)
        {
            if (WindowsVM.MainWindowVM.db.RemoveMatchEvent(event_id) == DataLevel.DataActionResults.ALL_RIGHT)
            {
                foreach (var el in MatchEvents) { if (el.Id == event_id) { MatchEvents.Remove(el); break; } }
                foreach (var el in HomeEvents) { if (el.Id == event_id) { HomeEvents.Remove(el); break; } }
                foreach (var el in AwayEvents) { if (el.Id == event_id) { AwayEvents.Remove(el); break; } }

                CountScore();
                SortEvents();
            }
        }
    }
}
