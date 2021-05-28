using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfApplication.ViewModels.PagesVM
{
    public class GreetingPageVM : ViewModelBase
    {
        public GreetingPageVM()
        {
            DispatcherTimer dispatcherTimerRole = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimerRole.Tick += new EventHandler(ClockImageRole);
            dispatcherTimerRole.Interval = new TimeSpan(0,0,6);
            dispatcherTimerRole.Start();

            DispatcherTimer dispatcherTimerBlink = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimerBlink.Tick += new EventHandler(ClockImageBlink);
            dispatcherTimerBlink.Interval = new TimeSpan(0, 0, 15);
            dispatcherTimerBlink.Start();

            ClockOpacity = 1.0;
            LinesOpacity = 0.0;
            Position = new Thickness(-250, 250, -255, -250);

            ImageClock = "/View/Images/ClockFrames/ClockFrame1.png";

            LinesAnimation();
        }

        private double _clock_opacity;

        private double _lines_opacity;
        private Thickness _position;

        private string _image_clock;

        public double ClockOpacity
        {
            get { return _clock_opacity; }
            set {
                _clock_opacity = value;
                OnPropertyChanged(nameof(ClockOpacity));
            }
        }
        public double LinesOpacity
        {
            get { return _lines_opacity; }
            set
            {
                _lines_opacity = value;
                OnPropertyChanged(nameof(LinesOpacity));
            }
        }
        public Thickness Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        public string ImageClock
        {
            get { return _image_clock; }
            set
            {
                _image_clock = value;
                OnPropertyChanged(nameof(ImageClock));
            }
        }

        public async void ClockImageBlink(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                for (double i = 1.0; i > 0.0; i -= 0.1)
                {
                    ClockOpacity = i;
                    Thread.Sleep(10);
                }
                for (double i = 0.0; i < 1.0; i += 0.1)
                {
                    ClockOpacity = i;
                    Thread.Sleep(20);
                }
            });
        }

        public async void ClockImageRole(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                for (int period = 0; period < 5; period++)
                { 
                    for (int i = 2; i < 14; i++)
                    {
                        ImageClock = "/View/Images/ClockFrames/ClockFrame" + i + ".png";
                        Thread.Sleep(25);
                    }
                }
            });
        }

        public async void LinesAnimation()
        {
            await Task.Factory.StartNew(() =>
            {
                double LeftMargin = Position.Left;

                for (int period = 0; period < 100; period++)
                {
                    LinesOpacity += 0.1;
                    if (LeftMargin > -350) Position = new Thickness(LeftMargin - 20, 250, -255, -250);

                    LeftMargin = Position.Left;
                    Thread.Sleep(90);
                }
            });
        }
    }
}
