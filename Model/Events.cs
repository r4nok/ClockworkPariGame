using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Events : ViewModelBase
    {
        public enum EventTypes
        {
            Goal = 1,
            YellowCard,
            RedCard,
            Penalty,
            Foul,
            Corner,
            Offside,
            None
        }
        public int Id { private set; get; }
        public void SetId(int id)
        {
            if (id > 0 && id < int.MaxValue) Id = id;
        }

        public EventTypes _type;
        public EventTypes Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public int _minute;
        public int Minute
        {
            get { return _minute; }
            set
            {
                _minute = (value <= 0) ? 1 : ((value > 90) ? 90 : value);
                OnPropertyChanged(nameof(Minute));
            }
        }

        public Events(EventTypes eventTypes, int minute, int id = 0)
        {
            Id = id;
            Type = eventTypes; 
            Minute = minute; 
        }
    }
}
