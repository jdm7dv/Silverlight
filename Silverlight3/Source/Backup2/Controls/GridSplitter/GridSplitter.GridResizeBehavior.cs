﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;

namespace System.Windows.Controls
{
    /// <summary>
    /// Represents the control that redistributes space between columns or rows
    /// of a Grid control.
    /// </summary>
    /// <QualityBand>Mature</QualityBand>
    public partial class GridSplitter : Control
    {
        /// <summary>
        /// Inherited code: Requires comment.
        /// </summary>
        /// <QualityBand>Mature</QualityBand>
        internal enum GridResizeBehavior
        {
            /// <summary>
            /// Inherited code: Requires comment.
            /// </summary>
            BasedOnAlignment,

            /// <summary>
            /// Inherited code: Requires comment.
            /// </summary>
            CurrentAndNext,

            /// <summary>
            /// Inherited code: Requires comment.
            /// </summary>
            PreviousAndCurrent,

            /// <summary>
            /// Inherited code: Requires comment.
            /// </summary>
            PreviousAndNext
        }
    }
}