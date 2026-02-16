// Copyright (c) Jack251970. All rights reserved.
// Licensed under the MIT license.

using iNKORE.UI.WPF.Modern.Common;
using System;
using System.Windows;

namespace U5BFA.Libraries
{
	public partial class App : Application
	{
        private Window? _window;

        public App()
        {
            ShadowAssist.UseBitmapCache = false;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            TrayIconManager.Default.Initialize(new(
                new("Assets\\Tray.ico"),
                "TrayIconFlyout sample app (WPF)",
                new("21B7FA20-C95D-450E-9AB8-DA6E9719AEBA")));

            _window = new MainWindow();
            _window.Show();

            RegisterExitEvents();
        }

        private void RegisterExitEvents()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                Dispose();
            };

            Current.Exit += (s, e) =>
            {
                Dispose();
            };

            Current.SessionEnding += (s, e) =>
            {
                Dispose();
            };
        }

        private void Dispose()
        {
            TrayIconManager.Default.Dispose();
        }
    }
}
