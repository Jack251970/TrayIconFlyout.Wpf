// Copyright (c) 0x5BFA. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Win32;

namespace U5BFA.Libraries
{
    /// <summary>
    /// Provides general helper methods for the library.
    /// </summary>
    public static class GeneralHelpers
	{
        /// <summary>
        /// Determines whether the taskbar is using the light theme.
        /// </summary>
        /// <returns></returns>
        public static bool IsTaskbarLight()
		{
			const string keyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
			using var key = Registry.CurrentUser.OpenSubKey(keyPath);
			var value = key?.GetValue("SystemUsesLightTheme");
			return value is int v && v != 0;
		}

		/// <summary>
		/// Determines whether the Windows taskbar is configured to show the accent color based on the current user settings.
		/// </summary>
		/// <remarks>This method reads the user's personalization settings from the Windows registry. It may return
		/// <see langword="false"/> if the registry key or value is missing, or if the feature is not supported on the current
		/// version of Windows.</remarks>
		/// <returns>A value indicating whether taskbar color prevalence is enabled. Returns <see langword="true"/> if the taskbar is
		/// set to display the accent color; otherwise, <see langword="false"/>.</returns>
        public static bool IsTaskbarColorPrevalenceEnabled()
		{
			const string keyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
			using var key = Registry.CurrentUser.OpenSubKey(keyPath);
			var value = key?.GetValue("ColorPrevalence");
			return value is int v && v != 0;
		}
	}
}
