using Model;

namespace WpfApplication.ViewModels.ViewModelTypes
{
    /// <summary>
    /// View Model shell for Team object
    /// </summary>
    public class TeamTeam : ViewModelBase
    {
        private Team _team_obj;
        public Team TeamObj
        {
            get { return _team_obj; }
            set
            {
                _team_obj = value;
                OnPropertyChanged(nameof(TeamObj));
            }
        }
        public TeamTeam(Team team)
        {
            TeamObj = team;
        }
        public void Set(Team t)
        {
            TeamObj = t;
        }
    }
}
