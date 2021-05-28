using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class UserMessage
    {
        public enum SetResuls
        {
            WRONG_ID = 1,
            WRONG_SENDER,
            WRONG_RECEIVER,
            WRONG_TEXT,
            WRONG_DATE,
            CORRECT
        }
        public int? Id { private set; get; }
        public int? Sender { private set; get; }
        public int? Receiver { private set; get; }
        public string Text { private set; get; }
        public DateTime Date { private set; get; }

        public UserMessage(int sender, int receiver, string text, DateTime date, int id = 0)
        {
            if (isIdCorrect(id))                                Id = id;
            if (isIdCorrect(sender))                            Sender = sender;
            if (isIdCorrect(receiver) && sender != receiver)    Receiver = receiver;
            if (isDateCorrect(date))                            Text = text;
            if (isTextCorrect(text))                            Date = date;
        }

        private bool isIdCorrect(int id)
        {
            if (id > 0 && id <= int.MaxValue) return true;
            return false;
        }
        private bool isTextCorrect(string text)
        {
            if (Text.Length > 250 || Text.Length == 0) return false;
            return true;
        }
        private bool isDateCorrect(DateTime date)
        {
            if (date.Year < 2020) return false;
            return true;
        }
        public bool IsNull()
        {
            return (Id.HasValue && Sender.HasValue && Receiver.HasValue && Text != null && Date != null) ? false : true;
        }
        public bool IsReadyToDB()
        {
            return (Sender.HasValue && Receiver.HasValue && Text != null && Date != null) ? true : false;
        }
        public SetResuls GetWrongField()
        {
            if (IsNull())
            {
                if (!Id.HasValue) return SetResuls.WRONG_ID;
                if (!Sender.HasValue) return SetResuls.WRONG_SENDER;
                if (!Receiver.HasValue) return SetResuls.WRONG_RECEIVER;
                if (Text == null) return SetResuls.WRONG_TEXT;
                if (Date == null) return SetResuls.WRONG_DATE;
            }
            return SetResuls.CORRECT;
        }
    }
}
