using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using DataLevel;
using Model;

using WpfApplication.ServiceChat;

namespace WpfApplication.ViewModels.PagesVM
{
    public class MessengerPageVM : ViewModelBase
    {
        private static ClockworkDataBase db = new ClockworkDataBase();
        public class MessageContainer
        {
            public HorizontalAlignment Alignment { set; get; }
            public string Text { set; get; }
            public string Date { set; get; }

            public MessageContainer(HorizontalAlignment al, string text, DateTime date)
            {
                Alignment = al;
                Text = text;
                Date = date.Day+"/"+date.Month+"/"+date.Year;
            }
            public MessageContainer(UserMessage message)
            {
                Alignment = (message.Sender == CurrentUserId) ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                Text      = message.Text;
                Date      = message.Date.Day + "/" + message.Date.Month + "/" + message.Date.Year;
            }
        }

        public MessengerPageVM()
        {
            Messages = new ObservableCollection<MessageContainer>();
            MessageHistory();
        }

        //private static ServiceMessengerClient messangerClient = new ServiceMessengerClient();

        private static int CurrentUserId = 1;
        private static int CurrentReceiverId = -1;

        private ObservableCollection<MessageContainer> _messages;
        private string _current_text;
        private string _current_search_text;
        public ObservableCollection<MessageContainer> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }
        public string CurrentText
        {
            get { return _current_text; }
            set
            {
                _current_text = value;
                OnPropertyChanged(nameof(CurrentText));
            }
        }
        public string SearchText
        {
            get { return _current_search_text; }
            set
            {
                _current_search_text = value;
                //CurrentReceiverId = db.GetUserId(_current_search_text);
                CurrentReceiverId = new ServiceMessengerClient().GetCompanion(_current_search_text);

                MessageHistory();

                OnPropertyChanged(nameof(SearchText));
            }
        }

        private void MessageHistory()
        {
            Messages.Clear();
            if (CurrentReceiverId != -1)
            {
                foreach (var um in db.ReadUserMessages(CurrentUserId, CurrentReceiverId, out int error_rows))
                {
                    Messages.Add(new MessageContainer(um));
                }
            }
        }

        private RelayCommand _send_message;
        private RelayCommand _add_newline;
        public RelayCommand SendMessage
        {
            get
            {
                return _send_message ?? (new RelayCommand(obj =>
                {
                    if (!String.IsNullOrEmpty(CurrentText))
                    {
                        if (CurrentReceiverId != -1) // if receiver is found
                        {
                            Messages.Add(new MessageContainer(HorizontalAlignment.Right, CurrentText, DateTime.Now));
                            db.InsertUserMessage(new UserMessage(CurrentUserId, CurrentReceiverId, CurrentText, DateTime.Now));
                            // Scroll down after addition
                            ListBox lb = (ListBox)obj;
                            lb.ScrollIntoView(lb.Items[lb.Items.Count - 1]);
                            // Clear text box
                            CurrentText = "";
                        }
                    }
                }));
            }
        }
        public RelayCommand AddNewline
        {
            get
            {
                return _add_newline ?? (new RelayCommand(obj =>
                {
                    if (!String.IsNullOrEmpty(CurrentText))
                    {
                        CurrentText += "\n";
                        // Change caret index to last
                        TextBox tb = (TextBox)obj;
                        tb.CaretIndex = CurrentText.Length;
                    }
                }));
            }
        }
    }
}
