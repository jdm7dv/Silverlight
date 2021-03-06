﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Silverlight.Weblog.Client.DAL.Macros
{
    public interface IChangeRootVisual
    {
        void ChangeTo(UIElement NewRootvisual);
        void RevertToOriginalRootVisual();
    }
}
