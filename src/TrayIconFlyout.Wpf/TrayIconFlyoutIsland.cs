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
        private const string PART_BackdropTargetBorder = "PART_BackdropTargetBorder";
        private const string PART_MainContentPresenter = "PART_MainContentPresenter";

		private Grid? RootGrid;
        private Border? BackdropTargetBorder;
        private ContentPresenter? MainContentPresenter;

		private WeakReference<TrayIconFlyout>? _owner;

        private bool? _wasTaskbarLightLastTimeChecked;
        private bool? _wasTaskbarColorPrevalenceLastTimeChecked;

        static TrayIconFlyoutIsland()
        {
            ContentProperty.OverrideMetadata(typeof(TrayIconFlyoutIsland), new FrameworkPropertyMetadata(OnContentChanged));
        }

        /// <inheritdoc />
        public TrayIconFlyoutIsland()
		{
			DefaultStyleKey = typeof(TrayIconFlyoutIsland);
		}

        /// <inheritdoc />
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			RootGrid = GetTemplateChild(PART_RootGrid) as Grid
				?? throw new InvalidOperationException($"Could not find {PART_RootGrid} in the given {nameof(TrayIconFlyoutIsland)}'s style.");
            BackdropTargetBorder = GetTemplateChild(PART_BackdropTargetBorder) as Border
                ?? throw new InvalidOperationException($"Could not find {PART_BackdropTargetBorder} in the given {nameof(TrayIconFlyoutIsland)}'s style.");
            MainContentPresenter = GetTemplateChild(PART_MainContentPresenter) as ContentPresenter
				?? throw new InvalidOperationException($"Could not find {PART_MainContentPresenter} in the given {nameof(TrayIconFlyoutIsland)}'s style.");
		}

		internal void SetOwner(TrayIconFlyout owner)
		{
			_owner = new(owner);
		}

        internal void UpdateBackdrop(bool isTaskbarLight, bool isTaskbarColorPrevalence)
        {
            if (_owner is null || !_owner.TryGetTarget(out var owner) || BackdropTargetBorder is null)
                return;

            if (owner.IsBackdropEnabled)
            {
                BackdropTargetBorder.Visibility = Visibility.Visible;

                var shouldUpdateBackdrop = _wasTaskbarLightLastTimeChecked != isTaskbarLight || _wasTaskbarColorPrevalenceLastTimeChecked != isTaskbarColorPrevalence;
                _wasTaskbarLightLastTimeChecked = isTaskbarLight;
                _wasTaskbarColorPrevalenceLastTimeChecked = isTaskbarColorPrevalence;
                if (!shouldUpdateBackdrop)
                    return;

                BackdropTargetBorder.Background = isTaskbarColorPrevalence ?
                    new SolidColorBrush(BackdropColorHelpers.GetAccentedBackgroundColor()) :
                        isTaskbarLight ?
                            new SolidColorBrush(BackdropColorHelpers.GetLightBackgroundColor()) :
                            new SolidColorBrush(BackdropColorHelpers.GetDarkBackgroundColor());
            }
            else
            {
                BackdropTargetBorder.Visibility = Visibility.Collapsed;
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
