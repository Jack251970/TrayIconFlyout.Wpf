// Copyright (c) 0x5BFA. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;

namespace U5BFA.Libraries
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty()]
        public partial string? IconPath { get; set; }

        [ObservableProperty()]
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

        public Dictionary<Orientation, string> PopupDirections { get; private set; } = [];
        public Dictionary<Orientation, string> IslandsOrientations { get; private set; } = [];
        public Dictionary<TrayIconFlyoutPlacementMode, string> FlyoutPlacements { get; private set; } = [];

        internal MainWindowViewModel()
        {
            IconPath = "Assets\\Tray.ico";
            TooltipText = TrayIconManager.Default.SystemTrayIcon?.Tooltip;

            IsBackdropEnabled = true;
            HideOnLostFocus = true;

            PopupDirections.Add(Orientation.Vertical, "Vertical");
            PopupDirections.Add(Orientation.Horizontal, "Horizontal");
            SelectedPopupDirectionIndex = 0;

            IslandsOrientations.Add(Orientation.Vertical, "Vertical");
            IslandsOrientations.Add(Orientation.Horizontal, "Horizontal");
            SelectedPopupDirectionIndex = 0;

            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.TopEdgeAlignedLeft, "Top left");
            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.TopEdgeAlignedRight, "Top right");
            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.BottomEdgeAlignedLeft, "Bottom left");
            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.BottomEdgeAlignedRight, "Bottom right");
            SelectedFlyoutPlacementIndex = 3;
        }

        partial void OnIconPathChanged(string? value)
        {
            TrayIconManager.Default.SystemTrayIcon?.Icon = new Icon(value ?? string.Empty);
        }

        partial void OnTooltipTextChanged(string? value)
        {
            TrayIconManager.Default.SystemTrayIcon?.Tooltip = value ?? string.Empty;
        }

        partial void OnIsBackdropEnabledChanged(bool value)
        {
            TrayIconManager.Default.TrayIconFlyout?.IsBackdropEnabled = value;
        }

        partial void OnHideOnLostFocusChanged(bool value)
        {
            TrayIconManager.Default.TrayIconFlyout?.HideOnLostFocus = value;
        }

        partial void OnIsTransitionAnimationEnabledChanged(bool value)
        {
            TrayIconManager.Default.TrayIconFlyout?.IsTransitionAnimationEnabled = value;
        }

        partial void OnSelectedPopupDirectionIndexChanged(int value)
        {
            TrayIconManager.Default.TrayIconFlyout?.PopupDirection = PopupDirections.ElementAt(value).Key;
        }

        partial void OnSelectedIslandsOrientationIndexChanged(int value)
        {
            TrayIconManager.Default.TrayIconFlyout?.IslandsOrientation = IslandsOrientations.ElementAt(value).Key;
        }

        partial void OnSelectedFlyoutPlacementIndexChanged(int value)
        {
            TrayIconManager.Default.TrayIconFlyout?.TrayIconFlyoutPlacement = FlyoutPlacements.ElementAt(value).Key;
        }
    }
}
