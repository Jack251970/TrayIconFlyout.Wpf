// Copyright (c) 0x5BFA. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
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

        public Dictionary<Orientation, string> PopupDirections { get; private set; } = [];
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

            PopupDirections.Add(Orientation.Vertical, "Vertical");
            PopupDirections.Add(Orientation.Horizontal, "Horizontal");
            SelectedPopupDirectionIndex = TrayIconManager.Default.TrayIconFlyout?.PopupDirection switch
            {
                Orientation.Vertical => 0,
                Orientation.Horizontal => 0,
                _ => 0,
            };

            IslandsOrientations.Add(Orientation.Vertical, "Vertical");
            IslandsOrientations.Add(Orientation.Horizontal, "Horizontal");
            SelectedPopupDirectionIndex = TrayIconManager.Default.TrayIconFlyout?.IslandsOrientation switch
            {
                Orientation.Vertical => 0,
                Orientation.Horizontal => 0,
                _ => 0,
            };

            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.TopEdgeAlignedLeft, "Top left");
            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.TopEdgeAlignedRight, "Top right");
            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.BottomEdgeAlignedLeft, "Bottom left");
            FlyoutPlacements.Add(TrayIconFlyoutPlacementMode.BottomEdgeAlignedRight, "Bottom right");
            SelectedFlyoutPlacementIndex = TrayIconManager.Default.TrayIconFlyout?.TrayIconFlyoutPlacement switch
            {
                TrayIconFlyoutPlacementMode.TopEdgeAlignedLeft => 0,
                TrayIconFlyoutPlacementMode.TopEdgeAlignedRight => 1,
                TrayIconFlyoutPlacementMode.BottomEdgeAlignedLeft => 2,
                TrayIconFlyoutPlacementMode.BottomEdgeAlignedRight => 3,
                _ => 3,
            };

            _isInitialized = true;
        }

        partial void OnIconPathChanged(string? value)
        {
            TrayIconManager.Default.SystemTrayIcon?.Icon = new Icon(value ?? string.Empty);
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
