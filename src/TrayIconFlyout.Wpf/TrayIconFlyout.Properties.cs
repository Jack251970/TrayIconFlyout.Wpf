// Copyright (c) Jack251970. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace U5BFA.Libraries
{
	public partial class TrayIconFlyout
	{
		public ObservableCollection<TrayIconFlyoutIsland> Islands { get; set; } = [];

		public static readonly DependencyProperty IslandsSourceProperty =
			DependencyProperty.Register(nameof(IslandsSource), typeof(object), typeof(TrayIconFlyout),
				new PropertyMetadata(null, OnIslandsSourceChanged));

		public object? IslandsSource
		{
			get => GetValue(IslandsSourceProperty);
			set => SetValue(IslandsSourceProperty, value);
		}

		public static readonly DependencyProperty IsBackdropEnabledProperty =
			DependencyProperty.Register(nameof(IsBackdropEnabled), typeof(bool), typeof(TrayIconFlyout),
				new PropertyMetadata(true));

		public bool IsBackdropEnabled
		{
			get => (bool)GetValue(IsBackdropEnabledProperty);
			set => SetValue(IsBackdropEnabledProperty, value);
		}

        public static readonly DependencyProperty HideOnLostFocusProperty =
            DependencyProperty.Register(nameof(HideOnLostFocus), typeof(bool), typeof(TrayIconFlyout),
                new PropertyMetadata(true));

        public bool HideOnLostFocus
        {
            get => (bool)GetValue(HideOnLostFocusProperty);
            set => SetValue(HideOnLostFocusProperty, value);
        }

        public static readonly DependencyProperty IsTransitionAnimationEnabledProperty =
            DependencyProperty.Register(nameof(IsTransitionAnimationEnabled), typeof(bool), typeof(TrayIconFlyout),
                new PropertyMetadata(true));

        public bool IsTransitionAnimationEnabled
        {
            get => (bool)GetValue(IsTransitionAnimationEnabledProperty);
            set => SetValue(IsTransitionAnimationEnabledProperty, value);
        }

        public static readonly DependencyProperty PopupDirectionProperty =
			DependencyProperty.Register(nameof(PopupDirection), typeof(Orientation), typeof(TrayIconFlyout),
				new PropertyMetadata(Orientation.Vertical));

		public Orientation PopupDirection
		{
			get => (Orientation)GetValue(PopupDirectionProperty);
			set => SetValue(PopupDirectionProperty, value);
		}

		public static readonly DependencyProperty IslandsOrientationProperty =
			DependencyProperty.Register(nameof(IslandsOrientation), typeof(Orientation), typeof(TrayIconFlyout),
				new PropertyMetadata(Orientation.Vertical));

		public Orientation IslandsOrientation
		{
			get => (Orientation)GetValue(IslandsOrientationProperty);
			set => SetValue(IslandsOrientationProperty, value);
		}

		public static readonly DependencyProperty TrayIconFlyoutPlacementProperty =
			DependencyProperty.Register(nameof(TrayIconFlyoutPlacement), typeof(TrayIconFlyoutPlacementMode), typeof(TrayIconFlyout),
				new PropertyMetadata(TrayIconFlyoutPlacementMode.RightEdgeAlignedBottom));

		public TrayIconFlyoutPlacementMode TrayIconFlyoutPlacement
		{
			get => (TrayIconFlyoutPlacementMode)GetValue(TrayIconFlyoutPlacementProperty);
			set => SetValue(TrayIconFlyoutPlacementProperty, value);
        }

        private static void OnIslandsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is not TrayIconFlyout flyout || e.NewValue is not IEnumerable<TrayIconFlyoutIsland> newIslands)
				return;

			flyout.Islands.Clear();

			foreach (var island in newIslands)
				flyout.Islands.Add(island);

			flyout.UpdateIslands();
		}
	}
}
