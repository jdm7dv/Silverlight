using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace VideoRSSViewer
{
    public enum PlayPauseButtonState
    {
        Play,
        Pause
    }

    public partial class PlayPauseButton : UserControl
    {
        private PlayPauseButtonState _state;
        public PlayPauseButton()
        {
            InitializeComponent();
            this.MouseLeftButtonUp += new MouseButtonEventHandler(PlayPauseButton_MouseLeftButtonUp);
            this.Cursor = Cursors.Hand;
            State = PlayPauseButtonState.Pause;
        }

        public event MouseEventHandler Clicked;
        void PlayPauseButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Clicked != null)
                Clicked(this, e);
        }

        public PlayPauseButtonState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                UpdateState();
            }
        }

        private void UpdateState()
        {
            if (_state == PlayPauseButtonState.Pause)
            {
                pauseFace.Opacity = 1;
                playFace.Opacity = 0;
            }
            else if (_state == PlayPauseButtonState.Play)
            {
                pauseFace.Opacity = 0;
                playFace.Opacity = 1;
            }
        }
    }
}
