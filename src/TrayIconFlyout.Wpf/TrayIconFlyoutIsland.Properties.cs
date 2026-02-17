// Copyright (c) Jack251970. All rights reserved.
// Licensed under the MIT license.

using System.Windows;

namespace U5BFA.Libraries
{
    public partial class TrayIconFlyoutIsland
    {
        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(TrayIconFlyoutIsland),
                new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// Gets or sets the radius of the control's corners.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
