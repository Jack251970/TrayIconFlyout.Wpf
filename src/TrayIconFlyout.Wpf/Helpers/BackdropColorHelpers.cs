// Copyright (c) 0x5BFA. All rights reserved.
// Licensed under the MIT license.

using System.Windows.Media;
using Windows.UI.ViewManagement;

namespace U5BFA.Libraries
{
	internal static class BackdropColorHelpers
	{
        private static UISettings UISettings => field ??= new UISettings();

        internal static Color GetDarkBackgroundColor()
		{
			return Color.FromArgb(0xFF, 0x1C, 0x1C, 0x1C);
		}

		internal static Color GetLightBackgroundColor()
		{
			return Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE);
		}

		internal static Color GetAccentedBackgroundColor()
		{
            var systemAccentColorDark2 = UISettings.GetColorValue(UIColorType.AccentDark2);
            return Color.FromArgb(
				systemAccentColorDark2.A,
				systemAccentColorDark2.R,
				systemAccentColorDark2.G,
				systemAccentColorDark2.B);
        }
	}
}
