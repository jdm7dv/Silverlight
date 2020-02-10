using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ReflectionIT.Silverlight.CountDown {

    public partial class FlipClock : UserControl {

        private const string formatStringHMS = "00";
        private string formatStringDays = "00";


        private DispatcherTimer _timer = new DispatcherTimer();
        private int _tellerS = 3;
        private int _tellerM = 1;
        private int _tellerH = 0;
        private int _tellerD = 0;
        private int _direction;
        private DateTime _countDownDate;

        private int Direction {
            get { return _direction; }
            set { _direction = value; }
        }


        public DateTime CountDownDate {
            get { return _countDownDate; }
            set {
                _countDownDate = value;
                Direction = -1;

                if (CountDownDate > DateTime.Now) {
                    var ts = CountDownDate - DateTime.Now;

                    _tellerS = ts.Seconds;
                    _tellerM = ts.Minutes;
                    _tellerH = ts.Hours;
                    _tellerD = ts.Days;
                } else {
                    _tellerS = 0;
                    _tellerM = 0;
                    _tellerH = 0;
                    _tellerD = 0;
                    _timer.Stop();
                }

                UpdateClock();
            }
        }

        public string Title {
            get { return textBlockTitle.Text; }
            set { textBlockTitle.Text = value; }
        }

        public FlipClock() {
            // Required to initialize variables
            InitializeComponent();

            _tellerS = DateTime.Now.Second;
            _tellerM = DateTime.Now.Minute;
            _tellerH = DateTime.Now.Hour;
            _tellerD = DateTime.Now.DayOfYear;

            UpdateClock();

            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += new EventHandler(_timer_Tick);

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) {
                _timer.Start();
            }
        }

        private void UpdateClock() {
            if (_tellerD > 100) {
                formatStringDays = "000";
                LayoutRoot.ColumnDefinitions[1].Width = new GridLength(0.31, GridUnitType.Star);
            } else {
                formatStringDays = "00";
                LayoutRoot.ColumnDefinitions[1].Width = LayoutRoot.ColumnDefinitions[2].Width;
            }
            FlipSeconds.Text1 = _tellerS.ToString(formatStringHMS);
            FlipMinutes.Text1 = _tellerM.ToString(formatStringHMS);
            FlipHours.Text1 = _tellerH.ToString(formatStringHMS);
            FlipDays.Text1 = _tellerD.ToString(formatStringDays);
        }

        private int Top(int v) {
            return Direction > 0 ? v : 0;
        }

        private int Bottom(int v) {
            return Direction < 0 ? v : 0;
        }

        void _timer_Tick(object sender, EventArgs e) {
            if (_tellerS == Top(59)) {
                if (Direction == -1 && _tellerD < 1 && _tellerH == 0 && _tellerM == 0) {
                    _timer.Stop();
                    return;
                }
                FlipSeconds.Flip(_tellerS.ToString(formatStringHMS), Bottom(59).ToString(formatStringHMS));
                _tellerS = Bottom(59);
                if (_tellerM == Top(59)) {
                    FlipMinutes.Flip(_tellerM.ToString(formatStringHMS), Bottom(59).ToString(formatStringHMS));
                    _tellerM = 0;
                    if (_tellerH == Top(23)) {
                        FlipHours.Flip(_tellerH.ToString(formatStringHMS), Bottom(23).ToString(formatStringHMS));
                        _tellerH = 0;

                        FlipDays.Flip(_tellerD.ToString(formatStringDays), (_tellerD + Direction).ToString(formatStringDays));
                        _tellerD += Direction;


                    } else {
                        FlipHours.Flip(_tellerH.ToString(formatStringHMS), (_tellerH + Direction).ToString(formatStringHMS));
                        _tellerH += Direction;
                    }
                } else {
                    FlipMinutes.Flip(_tellerM.ToString(formatStringHMS), (_tellerM + Direction).ToString(formatStringHMS));
                    _tellerM += Direction;
                }
            } else {
                FlipSeconds.Flip(_tellerS.ToString(formatStringHMS), (_tellerS + Direction).ToString(formatStringHMS));
                _tellerS += Direction;
            }

        }
    }
}