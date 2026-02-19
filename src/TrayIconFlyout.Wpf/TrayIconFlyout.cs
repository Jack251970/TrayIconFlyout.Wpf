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
		private TrayIconFlyoutPopupDirection _lastFlyoutPopupDirection;
		private TrayIconFlyoutPlacementMode _lastFlyoutPlacementMode;

        /// <summary>
        /// Indicates whether the flyout is currently open. This property is updated after the open/close animations complete.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Initializes a new instance of the TrayIconFlyout class with a default host window.
        /// </summary>
		/// <remarks>The default host is a transparent, borderless window that will display the flyout content.</remarks>
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

		/// <summary>
		/// Initializes a new instance of the TrayIconFlyout class and attaches it to the specified host window.
		/// </summary>
		/// <remarks>The flyout is set as the content of the provided host window. The flyout will automatically close
		/// when the host window is deactivated.</remarks>
		/// <param name="host">The window that will host the flyout. Cannot be null.</param>
        public TrayIconFlyout(Window host)
        {
            DefaultStyleKey = typeof(TrayIconFlyout);
            _host = host;
			_host.Content = this;
            host.Deactivated += HostWindow_Deactivated;
        }

        /// <inheritdoc />
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

		/// <summary>
		/// Displays the popup control with an optional transition animation.
		/// </summary>
		/// <remarks>This method applies the control template if it has not already been applied and ensures that the
		/// popup is properly measured and laid out before displaying. If transition animations are enabled, the popup will
		/// animate into view according to the specified direction. If the popup is already animating or the host is not
		/// available, the method does nothing.</remarks>
		/// <exception cref="InvalidOperationException">Thrown if the required template part is missing from the control template.</exception>
		public void Show()
		{
			if (_host is null || _isPopupAnimationPlaying)
				return;

            // Ensure template is applied before accessing template parts
            ApplyTemplate();

            if (RootGrid is null)
                throw new InvalidOperationException($"Template part {PART_RootGrid} is missing. Ensure the control template is correctly defined.");

            _isPopupAnimationPlaying = true;

            // Cache the current animation and placement modes to ensure consistency during the animation
            _lastFlyoutPopupDirection = PopupDirection;
			_lastFlyoutPlacementMode = TrayIconFlyoutPlacement;

            // Ensure the layout is updated to get the correct DesiredSize for animation
            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            UpdateLayout();

            UpdateBackdrop();

            UpdateFlyoutRegion();

            // Ensure the render transform is a mutable TranslateTransform for animation
            if (RootGrid.RenderTransform is not TranslateTransform translateTransform)
			{
                RootGrid.RenderTransform = new TranslateTransform();
            }
            translateTransform = (TranslateTransform)RootGrid.RenderTransform;

            // Ensure to clear any existing animations on the transform
            translateTransform.BeginAnimation(TranslateTransform.XProperty, null);
			translateTransform.BeginAnimation(TranslateTransform.YProperty, null);

			var transformOrientation = Orientation.Vertical;
			var transformSize = 0d;
			if (IsTransitionAnimationEnabled)
			{
				// Ensure to hide first and update the transform
				(transformOrientation, transformSize) = GetTranslateTransformInfo();
				if (transformOrientation is Orientation.Vertical)
				{
					translateTransform.X = 0;
					translateTransform.Y = transformSize;
				}
				else
				{
                    translateTransform.X = transformSize;
                    translateTransform.Y = 0;
                }
			}
			else
			{
				// Ensure the transform is reset to show the popup without animation
				translateTransform.X = translateTransform.Y = 0;
			}

            UpdateLayout();

            _host.Show();

            if (IsTransitionAnimationEnabled)
			{
				var storyboard = transformOrientation is Orientation.Vertical
                    ? TransitionHelpers.GetWindows11BottomToTopTransitionStoryboard(RootGrid, (int)transformSize, 0)
                    : TransitionHelpers.GetWindows11RightToLeftTransitionStoryboard(RootGrid, (int)transformSize, 0);
                storyboard.Completed += OpenAnimationStoryboard_Completed;
                storyboard.Begin();
            }
			else
			{
				IsOpen = true;
				_isPopupAnimationPlaying = false;
			}
		}

		/// <summary>
		/// Initiates the process to hide the popup, optionally using a transition animation if enabled.
		/// </summary>
		/// <remarks>If transition animations are enabled, the method plays the appropriate animation before closing
		/// the popup. If animations are disabled, the popup is closed immediately. If a hide operation is already in progress
		/// or required resources are unavailable, the method has no effect.</remarks>
		public void Hide()
		{
			if (_host is null || RootGrid is null || _isPopupAnimationPlaying)
				return;

			_isPopupAnimationPlaying = true;

			if (IsTransitionAnimationEnabled)
			{
                var transformInfo = GetTranslateTransformInfo();
                var storyboard = transformInfo.Orientation is Orientation.Vertical
                    ? TransitionHelpers.GetWindows11TopToBottomTransitionStoryboard(RootGrid, 0, (int)transformInfo.Size)
                    : TransitionHelpers.GetWindows11LeftToRightTransitionStoryboard(RootGrid, 0, (int)transformInfo.Size);
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

		private (Orientation Orientation, double Size) GetTranslateTransformInfo()
		{
            return _lastFlyoutPopupDirection switch
            {
                TrayIconFlyoutPopupDirection.Up => (Orientation.Vertical, DesiredSize.Height),
                TrayIconFlyoutPopupDirection.Down => (Orientation.Vertical, -DesiredSize.Height),
                TrayIconFlyoutPopupDirection.Right => (Orientation.Horizontal, -DesiredSize.Width),
                TrayIconFlyoutPopupDirection.Left => (Orientation.Horizontal, DesiredSize.Width),
                _ => ((Orientation Orientation, double Size))(Orientation.Vertical, 0),
            };
        }

		private void UpdateFlyoutRegion()
		{
			if (_host is null)
				return;

			// Get the working area of the primary screen
			var workingArea = SystemParameters.WorkArea;

            // Position at the corner
            switch (_lastFlyoutPlacementMode)
			{
				case TrayIconFlyoutPlacementMode.TopLeft:
					_host.Left = workingArea.Left;
					_host.Top = workingArea.Top;
					break;
				case TrayIconFlyoutPlacementMode.TopRight:
					_host.Left = workingArea.Right - DesiredSize.Width;
					_host.Top = workingArea.Top;
					break;
				case TrayIconFlyoutPlacementMode.BottomLeft:
					_host.Left = workingArea.Left;
					_host.Top = workingArea.Bottom - DesiredSize.Height;
					break;
                case TrayIconFlyoutPlacementMode.BottomRight:
                    _host.Left = workingArea.Right - DesiredSize.Width;
                    _host.Top = workingArea.Bottom - DesiredSize.Height;
                    break;
                case TrayIconFlyoutPlacementMode.Custom:
                    _host.Left = TrayIconFlyoutPlacementLocation.X;
                    _host.Top = TrayIconFlyoutPlacementLocation.Y;
                    break;
            }
			_host.Width = DesiredSize.Width;
			_host.Height = DesiredSize.Height;
		}

        private void UpdateBackdrop()
        {
            var isTaskbarLight = GeneralHelpers.IsTaskbarLight();
            var isTaskbarColorPrevalence = GeneralHelpers.IsTaskbarColorPrevalenceEnabled();

            foreach (var island in Islands)
                island.UpdateBackdrop(isTaskbarLight, isTaskbarColorPrevalence);
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

        /// <inheritdoc />
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
