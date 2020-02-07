namespace Kit3D.Windows.Media
{
    using System;

    public enum HitTestFilterBehavior
    {
        Continue = 6,
        ContinueSkipChildren = 2,
        ContinueSkipSelf = 4,
        ContinueSkipSelfAndChildren = 0,
        Stop = 8
    }
}

