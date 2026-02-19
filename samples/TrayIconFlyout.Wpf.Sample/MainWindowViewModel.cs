// Copyright (c) 0x5BFA. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace U5BFA.Libraries
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial bool IsInfoBarOpen { get; set; }

        [ObservableProperty]
        public partial string? IconPath { get; set; }

        [ObservableProperty]
        public partial string? TooltipText { get; set; }

        [ObservableProperty]
        public partial bool IsBackdropEnabled { get; set; }

        [ObservableProperty]
        public partial bool HideOnLostFocus { get; set; }

        [ObservableProperty]
        public partial bool IsTransitionAnimationEnabled { get; set; }

        [ObservableProperty]
        public partial int SelectedPopupDirectionIndex { get; set; }

        [ObservableProperty]
        public partial int SelectedIslandsOrientationIndex { get; set; }

        [ObservableProperty]
        public partial int SelectedFlyoutPlacementIndex { get; set; }

        [ObservableProperty]
        public partial Visibility CustomPlacementVisibility { get; set; } = Visibility.Collapsed;

        [ObservableProperty]
        public partial double CustomFlyoutPlacementX { get; set; }

        [ObservableProperty]
        public partial double CustomFlyoutPlacementY { get; set; }

        public Dictionary<TrayIconFlyoutPopupDirection, string> PopupDirections { get; private set; } = [];
        public Dictionary<Orientation, string> IslandsOrientations { get; private set; } = [];
        public Dictionary<TrayIconFlyoutPlacementMode, string> FlyoutPlacements { get; private set; } = [];

        private readonly bool _isInitialized = false;

        internal MainWindowViewModel()
        {
            IconPath = "Assets\\Tray.ico";
            TooltipText = TrayIconManager.Default.SystemTrayIcon?.Tooltip;

            IsBackdropEnabled = TrayIconManager.Default.TrayIconFlyout?.IsBackdropEnabled ?? true;
            HideOnLostFocus = TrayIconManager.Default.TrayIconFlyout?.HideOnLostFocus ?? true;
            IsTransitionAnimationEnabled = TrayIconManager.Default.TrayIconFlyout?.IsTransitionAnimationEnabled ?? true;

            PopupDirections.Add(TrayIconFlyoutPopupDirection.Up, "Up");
            PopupDirections.Add(TrayIconFlyoutPopupDirection.Down, "Down");
            PopupDirections.Add(TrayIconFlyoutPopupDirection.Left, "Left");
            PopupDirections.Add(TrayIconFlyoutPopupDirection.Right, "Right");
            SelectedPopupDirectionIndex = TrayIconManager.Default.TrayIconFlyout?.PopupDirection switch
            {
                TrayIconFlyoutPopupDirection.Up => 0,
                TrayIconFlyoutPopupDirection.Down => 1,
                TrayIconFlyoutPopupDirection.Left => 2,
                TrayIconFlyoutPopupDirection.Right => 3,
                _ => 0,
            };

            IslandsOrientations.Add(Orientation.Vertical, "Vertical");
            IslandsOrientations.Add(Orientation.Horizontal, "Horizontal");
            SelectedPopupDirectionIndex = TrayIconManager.Default.TrayIconFlyout?.IslandsOrientation switch
            {
                Orientation.Vertical => 0,
                Orientation.Horizontal => 1,
                _ => 0,
            };

            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.TopLeft, "Top left");
            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.TopRight, "Top right");
            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.BottomLeft, "Bottom left");
            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.BottomRight, "Bottom right");
            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.Custom, "Custom");
            SelectedFlyoutPlacementIndex = TrayIconManager.Default.TrayIconFlyout?.TrayIconFlyoutPlacement switch
            {
                TrayIconFlyoutPlacementMode.TopLeft => 0,
                TrayIconFlyoutPlacementMode.TopRight => 1,
                TrayIconFlyoutPlacementMode.BottomLeft => 2,
                TrayIconFlyoutPlacementMode.BottomRight => 3,
                TrayIconFlyoutPlacementMode.Custom => 4,
                _ => 3,
            };

            CustomFlyoutPlacementX = TrayIconManager.Default.TrayIconFlyout?.TrayIconFlyoutPlacementLocation.X ?? 0;
            CustomFlyoutPlacementY = TrayIconManager.Default.TrayIconFlyout?.TrayIconFlyoutPlacementLocation.Y ?? 0;

            _isInitialized = true;
        }

        partial void OnIconPathChanged(string? value)
        {
            TrayIconManager.Default.SystemTrayIcon?.Icon = new System.Drawing.Icon(value ?? string.Empty);
            DisplayInfoBar();
        }

        partial void OnTooltipTextChanged(string? value)
        {
            TrayIconManager.Default.SystemTrayIcon?.Tooltip = value ?? string.Empty;
            DisplayInfoBar();
        }

        partial void OnIsBackdropEnabledChanged(bool value)
        {
            TrayIconManager.Default.TrayIconFlyout?.IsBackdropEnabled = value;
            DisplayInfoBar();
        }

        partial void OnHideOnLostFocusChanged(bool value)
        {
            TrayIconManager.Default.TrayIconFlyout?.HideOnLostFocus = value;
            DisplayInfoBar();
        }

        partial void OnIsTransitionAnimationEnabledChanged(bool value)
        {
            TrayIconManager.Default.TrayIconFlyout?.IsTransitionAnimationEnabled = value;
            DisplayInfoBar();
        }

        partial void OnSelectedPopupDirectionIndexChanged(int value)
        {
            TrayIconManager.Default.TrayIconFlyout?.PopupDirection = PopupDirections.ElementAt(value).Key;
            DisplayInfoBar();
        }

        partial void OnSelectedIslandsOrientationIndexChanged(int value)
        {
            TrayIconManager.Default.TrayIconFlyout?.IslandsOrientation = IslandsOrientations.ElementAt(value).Key;
            DisplayInfoBar();
        }

        partial void OnSelectedFlyoutPlacementIndexChanged(int value)
        {
            TrayIconManager.Default.TrayIconFlyout?.TrayIconFlyoutPlacement = FlyoutPlacements.ElementAt(value).Key;
            CustomPlacementVisibility = value == 4 ? Visibility.Visible : Visibility.Collapsed;
            DisplayInfoBar();
        }

        partial void OnCustomFlyoutPlacementXChanged(double value)
        {
            TrayIconManager.Default.TrayIconFlyout?.TrayIconFlyoutPlacementLocation = new Point(value, CustomFlyoutPlacementY);
            DisplayInfoBar();
        }

        partial void OnCustomFlyoutPlacementYChanged(double value)
        {
            TrayIconManager.Default.TrayIconFlyout?.TrayIconFlyoutPlacementLocation = new Point(CustomFlyoutPlacementX, value);
            DisplayInfoBar();
        }

        private async void DisplayInfoBar()
        {
            if (!_isInitialized) return;
            if (IsInfoBarOpen) return;

            IsInfoBarOpen = true;
            await Task.Delay(1500);
            IsInfoBarOpen = false;
        }
    }
}
