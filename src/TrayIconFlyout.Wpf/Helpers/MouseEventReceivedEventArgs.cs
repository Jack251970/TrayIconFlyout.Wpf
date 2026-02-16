// Copyright (c) 0x5BFA. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Drawing;

namespace U5BFA.Libraries
{
    /// <summary>
    /// Provides data for the mouse event that occurred on the tray icon.
    /// </summary>
    public class MouseEventReceivedEventArgs : EventArgs
	{
        /// <summary>
        /// Gets the screen coordinates of the mouse event that occurred on the tray icon.
        /// </summary>
        public Point Point { get; }

		internal MouseEventReceivedEventArgs(Point point)
		{
			Point = point;
		}
	}
}
