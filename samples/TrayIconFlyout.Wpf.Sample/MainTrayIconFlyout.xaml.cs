// Copyright (c) 0x5BFA. All rights reserved.
// Licensed under the MIT license.

namespace U5BFA.Libraries
{
    public partial class MainTrayIconFlyout : TrayIconFlyout
    {
        public MainTrayIconFlyout() : base(new MainTrayIconFlyoutWindow())
        {
            InitializeComponent();
        }
    }
}
