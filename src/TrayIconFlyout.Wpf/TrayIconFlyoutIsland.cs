// Copyright (c) Jack251970. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace U5BFA.Libraries
{
	/// <summary>
	/// Represents a content island within a TrayIconFlyout for WPF.
	/// </summary>
	public partial class TrayIconFlyoutIsland : ContentControl
	{
        private const string PART_RootGrid = "PART_RootGrid";
        private const string PART_MainContentPresenter = "PART_MainContentPresenter";

		private Grid? RootGrid;
        private ContentPresenter? MainContentPresenter;

		private WeakReference<TrayIconFlyout>? _owner;

        static TrayIconFlyoutIsland()
        {
            ContentProperty.OverrideMetadata(typeof(TrayIconFlyoutIsland), new FrameworkPropertyMetadata(OnContentChanged));
        }

        public TrayIconFlyoutIsland()
		{
			DefaultStyleKey = typeof(TrayIconFlyoutIsland);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			RootGrid = GetTemplateChild(PART_RootGrid) as Grid
				?? throw new InvalidOperationException($"Could not find {PART_RootGrid} in the given {nameof(TrayIconFlyoutIsland)}'s style.");
            MainContentPresenter = GetTemplateChild(PART_MainContentPresenter) as ContentPresenter
				?? throw new InvalidOperationException($"Could not find {PART_MainContentPresenter} in the given {nameof(TrayIconFlyoutIsland)}'s style.");
		}

		internal void SetOwner(TrayIconFlyout owner)
		{
			_owner = new(owner);
		}

        internal void UpdateBackdrop(bool isLightTheme, bool isColorEnabled)
        {
            if (_owner is null || !_owner.TryGetTarget(out var owner))
                return;

            if (owner.IsBackdropEnabled)
            {
                if (isColorEnabled)
                {
                    // TODO: Use the actual system accent color instead of hardcoding it
                }
                else
                {
                    // #F2F2F2 for light theme & #242424 for dark theme (for all transparancy effects)
                    Background = isLightTheme ? new SolidColorBrush(Color.FromArgb(0xFF, 0xF2, 0xF2, 0xF2)) :
                        new SolidColorBrush(Color.FromArgb(0xFF, 0x24, 0x24, 0x24));
                }
            }
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TrayIconFlyoutIsland island)
				island.HandleContentChanged(e.OldValue, e.NewValue);
		}

		private void HandleContentChanged(object? oldValue, object? newValue)
		{
			if (newValue is not FrameworkElement newContent || MainContentPresenter is null)
				return;

            MainContentPresenter.Content = newContent;
        }

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(TrayIconFlyoutIsland),
                new PropertyMetadata(new CornerRadius(0)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
