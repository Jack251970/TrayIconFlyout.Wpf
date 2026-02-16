// Copyright (c) Jack251970. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace U5BFA.Libraries
{
	/// <summary>
	/// A flyout control for WPF that appears from the system tray.
	/// </summary>
	[ContentProperty(nameof(Islands))]
	public partial class TrayIconFlyout : Control, IDisposable
	{
		private const string PART_RootGrid = "PART_RootGrid";
		private const string PART_IslandsGrid = "PART_IslandsGrid";

        private Grid? RootGrid;
        private Grid? IslandsGrid;

        private Window? _host;
		private bool _isPopupAnimationPlaying;

        public bool IsOpen { get; private set; }

		public TrayIconFlyout() : this(new Window
        {
            WindowStyle = WindowStyle.None,
            ResizeMode = ResizeMode.NoResize,
            ShowInTaskbar = false,
            Topmost = true,
            AllowsTransparency = true,
            Background = Brushes.Transparent
        })
		{

		}

        public TrayIconFlyout(Window host)
        {
            DefaultStyleKey = typeof(TrayIconFlyout);
            _host = host;
			_host.Content = this;
            host.Deactivated += HostWindow_Deactivated;
        }

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			RootGrid = GetTemplateChild(PART_RootGrid) as Grid
				?? throw new InvalidOperationException($"Could not find {PART_RootGrid} in the given {nameof(TrayIconFlyout)}'s style.");
			IslandsGrid = GetTemplateChild(PART_IslandsGrid) as Grid
				?? throw new InvalidOperationException($"Could not find {PART_IslandsGrid} in the given {nameof(TrayIconFlyout)}'s style.");

			// Ensure the render transform is mutable
			RootGrid.RenderTransform = new TranslateTransform();

            UpdateIslands();
		}

		public void Show()
		{
			if (_host is null || _isPopupAnimationPlaying)
				return;

            // Ensure template is applied before accessing template parts
            ApplyTemplate();

            if (RootGrid is null)
                throw new InvalidOperationException($"Template part {PART_RootGrid} is missing. Ensure the control template is correctly defined.");

            _isPopupAnimationPlaying = true;

            // Ensure the layout is updated to get the correct DesiredSize for animation
            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            UpdateLayout();

            UpdateBackdrop();

            UpdateFlyoutRegion();

            // Ensure to hide first
            if (RootGrid.RenderTransform is TranslateTransform translateTransform)
            {
                if (PopupDirection is Orientation.Vertical)
                    translateTransform.Y = DesiredSize.Height;
                else
                    translateTransform.X = DesiredSize.Width;
            }

            UpdateLayout();

            _host.Show();

            if (IsTransitionAnimationEnabled)
			{
				var storyboard = PopupDirection is Orientation.Vertical
                    ? TransitionHelpers.GetWindows11BottomToTopTransitionStoryboard(RootGrid, (int)DesiredSize.Height, 0)
                    : TransitionHelpers.GetWindows11RightToLeftTransitionStoryboard(RootGrid, (int)DesiredSize.Width, 0);
                storyboard.Completed += OpenAnimationStoryboard_Completed;
                storyboard.Begin();
            }
			else
			{
				IsOpen = true;
				_isPopupAnimationPlaying = false;
			}
		}

		public void Hide()
		{
			if (_host is null || RootGrid is null || _isPopupAnimationPlaying)
				return;

			_isPopupAnimationPlaying = true;

            if (IsTransitionAnimationEnabled)
			{
				var storyboard = PopupDirection is Orientation.Vertical
                    ? TransitionHelpers.GetWindows11TopToBottomTransitionStoryboard(RootGrid, 0, (int)DesiredSize.Height)
                    : TransitionHelpers.GetWindows11LeftToRightTransitionStoryboard(RootGrid, 0, (int)DesiredSize.Width);
                storyboard.Completed += CloseAnimationStoryboard_Completed;
                storyboard.Begin();
            }
			else
			{
				_host.Hide();
				IsOpen = false;
				_isPopupAnimationPlaying = false;
			}
		}

		private void UpdateFlyoutRegion()
		{
			if (_host is null)
				return;

			// Get the working area of the primary screen
			var workingArea = SystemParameters.WorkArea;

			// Position at bottom-right corner (near system tray)
			_host.Left = workingArea.Right - DesiredSize.Width;
			_host.Top = workingArea.Bottom - DesiredSize.Height;
			_host.Width = DesiredSize.Width;
			_host.Height = DesiredSize.Height;
		}

        private void UpdateBackdrop()
        {
            var isLightTheme = GeneralHelpers.IsTaskbarLight();
            var isColorEnabled = GeneralHelpers.IsTaskbarColorPrevalenceEnabled();

            foreach (var island in Islands)
                island.UpdateBackdrop(isLightTheme, isColorEnabled);
        }

        private void UpdateIslands()
		{
			if (IslandsGrid is null)
				return;

			IslandsGrid.Children.Clear();
			IslandsGrid.RowDefinitions.Clear();
			IslandsGrid.ColumnDefinitions.Clear();

			if (IslandsOrientation is Orientation.Vertical)
			{
				for (var index = 0; index < Islands.Count; index++)
				{
					if (Islands[index] is not TrayIconFlyoutIsland island)
						continue;

					IslandsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					Grid.SetRow(island, index * 2);
					Grid.SetColumn(island, 0);
					island.SetOwner(this);
                    IslandsGrid.Children.Add(island);

					if (index != Islands.Count - 1)
						IslandsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(12) });
				}
            }
			else
			{
				for (var index = 0; index < Islands.Count; index++)
				{
					if (Islands[index] is not TrayIconFlyoutIsland island)
						continue;

					IslandsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
					Grid.SetRow(island, 0);
					Grid.SetColumn(island, index * 2);
					island.SetOwner(this);
                    IslandsGrid.Children.Add(island);

					if (index != Islands.Count - 1)
						IslandsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(12) });
                }
            }
		}

        private void OpenAnimationStoryboard_Completed(object? sender, object e)
		{
            if (sender is not Clock clock || clock.Timeline is not Storyboard storyboard)
                return;

            if (!storyboard.IsFrozen)
                storyboard.Completed -= OpenAnimationStoryboard_Completed;

			_isPopupAnimationPlaying = false;
			IsOpen = true;
        }

		private void CloseAnimationStoryboard_Completed(object? sender, object e)
		{
            if (sender is not Clock clock || clock.Timeline is not Storyboard storyboard)
                return;

            if (!storyboard.IsFrozen)
                storyboard.Completed -= CloseAnimationStoryboard_Completed;

			_isPopupAnimationPlaying = false;
			IsOpen = false;
			_host?.Hide();
        }

		private void HostWindow_Deactivated(object? sender, EventArgs e)
		{
			if (HideOnLostFocus)
				Hide();
		}

		public void Dispose()
		{
			if (_host is not null)
			{
				_host.Deactivated -= HostWindow_Deactivated;
				_host.Close();
				_host = null;
			}

			GC.SuppressFinalize(this);
		}
	}
}
