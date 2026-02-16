// Copyright (c) 0x5BFA. All rights reserved.
// Licensed under the MIT license.

using System;

namespace U5BFA.Libraries
{
	internal partial class TrayIconManager : IDisposable
	{
		private static readonly Lazy<TrayIconManager> _default = new(() => new TrayIconManager());
		internal static TrayIconManager Default => _default.Value;

		internal SystemTrayIcon? SystemTrayIcon { get; set; }
		internal TrayIconFlyout? TrayIconFlyout { get; set; }

        private TrayIconManager() { }

		internal void Initialize(SystemTrayIcon trayIcon)
		{
            TrayIconFlyout = new MainTrayIconFlyout();

            SystemTrayIcon = trayIcon;
			SystemTrayIcon.Show();
			SystemTrayIcon.LeftClicked += SystemTrayIcon_LeftClicked;
			SystemTrayIcon.RightClicked += SystemTrayIcon_RightClicked;
		}

		private void SystemTrayIcon_LeftClicked(object? sender, MouseEventReceivedEventArgs e)
		{
			if (TrayIconFlyout is null)
				return;

			if (TrayIconFlyout.IsOpen)
				TrayIconFlyout.Hide();
			else
				TrayIconFlyout.Show();
		}

		private void SystemTrayIcon_RightClicked(object? sender, MouseEventReceivedEventArgs e)
		{
            if (TrayIconFlyout is null)
                return;

            if (TrayIconFlyout.IsOpen)
                TrayIconFlyout.Hide();
            else
                TrayIconFlyout.Show();
        }

        public void Dispose()
		{
            SystemTrayIcon?.Hide();
            SystemTrayIcon?.LeftClicked -= SystemTrayIcon_LeftClicked;
			SystemTrayIcon?.RightClicked -= SystemTrayIcon_RightClicked;
			SystemTrayIcon?.Dispose();
			TrayIconFlyout?.Dispose();
		}
	}
}
