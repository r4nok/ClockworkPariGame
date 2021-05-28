using Model;
using System.Windows.Media;

namespace WpfApplication.ViewModels.ViewModelTypes
{
    /// <summary>
    /// View Model shell for editing textBox
    /// </summary>
    public class EditTextBox : ViewModelBase
    {
        public EditTextBox()
        {
            ColorCorrect = new SolidColorBrush(defaultColor);
            TextContent = "";
        }

        private static Color defaultColor   = Color.FromArgb(95, 104, 244, 228);
        private static Color rightColor     = Color.FromArgb(25, 0, 255, 0);
        private static Color wrongColor     = Color.FromArgb(25, 255, 0, 0);

        private static string WrongTournameNameLabel            = "Tournamet name should only consist letters, digits, spaces and symbols (\\-`,/)";
        private static string WrongTournameAbbreviationLabel    = "Tournamet abbreviation should only consist letters and dot symbol";
        private static string WrongTournameTypeLabel            = "Tournamet type can be: \"league\", \"playoff1\" or \"playoff2\"";

        private static string WrongTeamNameLabel    = "Team name should only consist letters, digits, spaces and symbols (.-#`,)";
        private static string WrongTeamManagerLabel = "Manager name should only consist letters, spaces and symbols (.-`)";
        private static string WrongTeamStadiumLabel = "Stadium name should only consist letters, digits, spaces and symbols (.-,`)";

        public string _text_content;            // text
        public SolidColorBrush _color_correct;  // color that masrks correctness
        public string _error_text;              // message for wrong format text
        public string _enter_label;             // text with enter-messsage

        public string TextContent
        {
            get { return _text_content; }
            set
            {
                _text_content = value;
                DefaultSet();
                OnPropertyChanged(nameof(TextContent));
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
        public string ErrorText
        {
            get { return _error_text; }
            set
            {
                _error_text = value;
                OnPropertyChanged(nameof(ErrorText));
            }
        }
        public string EnterLabel
        {
            get { return _enter_label; }
            set
            {
                _enter_label = value;
                OnPropertyChanged(nameof(EnterLabel));
            }
        }

        public void SetText(string text)
        {
            TextContent = text;
        }
        public bool ApplyName(Tournament tournament)
        {
            if (tournament.SetName(TextContent)) { CorrectSet(); return true; } else { WrongSet(WrongTournameNameLabel); return false; }
        }
        public bool ApplyName(Team team)
        {
            if (team.SetName(TextContent)) { CorrectSet(); return true; } else { WrongSet(WrongTeamNameLabel); return false; }
        }
        public bool ApplyAbbreviation(Tournament tournament)
        {
            if (tournament.SetAbbreviation(TextContent)) { CorrectSet(); return true; } else { WrongSet(WrongTournameAbbreviationLabel); return false; }
        }
        public bool ApplyTournamentType(Tournament tournament)
        {
            if (tournament.SetType(TextContent)) { CorrectSet(); return true; } else { WrongSet(WrongTournameTypeLabel); return false; }
        }
        public bool ApplyManager(Team team)
        {
            if (team.SetManager(TextContent)) { CorrectSet(); return true; } else { WrongSet(WrongTeamManagerLabel); return false; }
        }
        public bool ApplyStadium(Team team)
        {
            if (team.SetStadium(TextContent)) { CorrectSet(); return true; } else { WrongSet(WrongTeamStadiumLabel); return false; }
        }

        private void DefaultSet() { ColorCorrect.Color = defaultColor; ErrorText = ""; }
        private void CorrectSet() { ColorCorrect.Color = rightColor; ErrorText = ""; }
        private void WrongSet(string error_messsag) { ColorCorrect.Color = wrongColor; ErrorText = error_messsag; }
        public void SetIncorrect(string str) { ColorCorrect.Color = wrongColor; ErrorText = str; }
    }
}
