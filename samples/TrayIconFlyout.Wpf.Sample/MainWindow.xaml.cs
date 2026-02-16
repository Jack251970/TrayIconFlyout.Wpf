// Copyright (c) Jack251970. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Windows;

namespace U5BFA.Libraries
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			DataContext = new MainWindowViewModel();
			InitializeComponent();
		}

        private void Window_Closed(object sender, EventArgs e)
        {
			Application.Current.Shutdown();
        }
    }
}
