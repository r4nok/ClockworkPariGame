using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApplication.ViewModels.ViewModelTypes
{
    public class MatchEventItem : Events
    {
        private static string GoalImage = "/View/Images/soccer_ball_32px.png";
        private static string DefaultImage = "/View/Images/close_window_50px.png";

        private static string onlyDigits = @"^[0-9\ ]+$";

        private static Color defaultColor = Colors.White;
        private static Color wrongColor = Color.FromArgb(100, 200, 32, 0);

        public static int HomeColumn = 1;
        public static int AwayColumn = 3;

        private string _image;
        private int _column;
        private SolidColorBrush _color_correct;

        private bool IsDBActivated;
        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        public int Column
        {
            get { return _column; }
            set
            {
                _column = value;
                OnPropertyChanged(nameof(Column));
            }
        }
        public SolidColorBrush ColorCorrect
        {
            get { return _color_correct; }
            set
            {
                _color_correct = value;
                OnPropertyChanged(nameof(ColorCorrect));
            }
        }

        private string _minute_string;
        public string MinuteString
        {
            get { return _minute_string; }
            set
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(value, onlyDigits) && value.Length < 3)
                {
                    int minute = (Convert.ToInt32(value) <= 0) ? 1 : (Convert.ToInt32(value) > 90 ? 90 : Convert.ToInt32(value));

                    if (!IsDBActivated || (IsDBActivated && WindowsVM.MainWindowVM.db.UpdateMatchEvent(Id, minute) == DataLevel.DataActionResults.ALL_RIGHT))
                    {
                        _minute_string = minute.ToString();
                        Minute = minute;

                        ColorCorrect.Color = defaultColor;
                    }
                    else { ColorCorrect.Color = wrongColor; }
                }
                else
                {
                    _minute_string = "";
                    Minute = 1;
                    ColorCorrect.Color = wrongColor;
                }

                OnPropertyChanged(nameof(MinuteString));
            }
        }

        public MatchEventItem(Model.Events matchEvent, bool IsHomeTeam) : base(matchEvent.Type, matchEvent.Minute, matchEvent.Id)
        {
            ColorCorrect = new SolidColorBrush();

            switch (matchEvent.Type)
            {
                case EventTypes.Goal: Image = GoalImage; break;
                default: Image = DefaultImage; break;
            }
            if (IsHomeTeam) Column = HomeColumn; else Column = AwayColumn;
            MinuteString = matchEvent.Minute.ToString();

            IsDBActivated = true;
        }
    }
}
