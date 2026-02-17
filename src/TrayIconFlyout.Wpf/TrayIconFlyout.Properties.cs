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
		/// <summary>
		/// Gets or sets the collection of islands.
		/// </summary>
		public ObservableCollection<TrayIconFlyoutIsland> Islands { get; set; } = [];

		/// <summary>
		/// Identifies the <see cref="IslandsSource"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty IslandsSourceProperty =
			DependencyProperty.Register(nameof(IslandsSource), typeof(object), typeof(TrayIconFlyout),
				new PropertyMetadata(null, OnIslandsSourceChanged));

		/// <summary>
		/// Gets or sets the source object used to generate the content of the Islands collection.
		/// </summary>
		public object? IslandsSource
		{
			get => GetValue(IslandsSourceProperty);
			set => SetValue(IslandsSourceProperty, value);
		}

		/// <summary>
		/// Identifies the <see cref="IsBackdropEnabled"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty IsBackdropEnabledProperty =
			DependencyProperty.Register(nameof(IsBackdropEnabled), typeof(bool), typeof(TrayIconFlyout),
				new PropertyMetadata(true, OnIsBackdropEnabledPropertyChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the backdrop is enabled.
        /// </summary>
        public bool IsBackdropEnabled
		{
			get => (bool)GetValue(IsBackdropEnabledProperty);
			set => SetValue(IsBackdropEnabledProperty, value);
		}

		/// <summary>
		/// Identifies the <see cref="HideOnLostFocus"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty HideOnLostFocusProperty =
			DependencyProperty.Register(nameof(HideOnLostFocus), typeof(bool), typeof(TrayIconFlyout),
				new PropertyMetadata(true));

		/// <summary>
		/// Gets or sets a value indicating whether to hide the flyout when it loses focus.
		/// </summary>
		public bool HideOnLostFocus
		{
			get => (bool)GetValue(HideOnLostFocusProperty);
			set => SetValue(HideOnLostFocusProperty, value);
		}

		/// <summary>
		/// Identifies the <see cref="IsTransitionAnimationEnabled"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty IsTransitionAnimationEnabledProperty =
			DependencyProperty.Register(nameof(IsTransitionAnimationEnabled), typeof(bool), typeof(TrayIconFlyout),
				new PropertyMetadata(true));

		/// <summary>
		/// Gets or sets a value indicating whether transition animations are enabled.
		/// </summary>
		public bool IsTransitionAnimationEnabled
		{
			get => (bool)GetValue(IsTransitionAnimationEnabledProperty);
			set => SetValue(IsTransitionAnimationEnabledProperty, value);
		}

		/// <summary>
		/// Identifies the <see cref="PopupDirection"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty PopupDirectionProperty =
			DependencyProperty.Register(nameof(PopupDirection), typeof(Orientation), typeof(TrayIconFlyout),
				new PropertyMetadata(Orientation.Vertical));

		/// <summary>
		/// Gets or sets the popup direction.
		/// </summary>
		public Orientation PopupDirection
		{
			get => (Orientation)GetValue(PopupDirectionProperty);
			set => SetValue(PopupDirectionProperty, value);
		}

		/// <summary>
		/// Identifies the <see cref="IslandsOrientation"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty IslandsOrientationProperty =
			DependencyProperty.Register(nameof(IslandsOrientation), typeof(Orientation), typeof(TrayIconFlyout),
				new PropertyMetadata(Orientation.Vertical));

		/// <summary>
		/// Gets or sets the orientation of the islands.
		/// </summary>
		public Orientation IslandsOrientation
		{
			get => (Orientation)GetValue(IslandsOrientationProperty);
			set => SetValue(IslandsOrientationProperty, value);
		}

		/// <summary>
		/// Identifies the <see cref="TrayIconFlyoutPlacement"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty TrayIconFlyoutPlacementProperty =
			DependencyProperty.Register(nameof(TrayIconFlyoutPlacement), typeof(TrayIconFlyoutPlacementMode), typeof(TrayIconFlyout),
				new PropertyMetadata(TrayIconFlyoutPlacementMode.BottomRight));

		/// <summary>
		/// Gets or sets the placement mode of the flyout.
		/// </summary>
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

        private static void OnIsBackdropEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TrayIconFlyout flyout || (bool)e.NewValue == (bool)e.OldValue)
                return;

			flyout.UpdateBackdrop();
        }
    }
}
