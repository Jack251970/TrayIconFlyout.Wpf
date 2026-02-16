## Usage

### SystemTrayIcon

Use `SystemTrayIcon` to create a tray icon for your app.

```csharp
SystemTrayIcon = new(
    new("Assets\\Tray.ico"),
    "TrayIconFlyout sample app (WPF)",
    new("21B7FA20-C95D-450E-9AB8-DA6E9719AEBA"));
SystemTrayIcon.Show();
```

### TrayIconFlyout

Use `TrayIconFlyout` for the Shell Flyout behavior.

```xml
<me:TrayIconFlyout x:Class="..." ... Width="360">

    <me:TrayIconFlyoutIsland Height="300">
        <!-- Put elements here -->
    </me:TrayIconFlyoutIsland>
    <me:TrayIconFlyoutIsland Height="300">
        <!-- Put elements here -->
    </me:TrayIconFlyoutIsland>

</me:TrayIconFlyout>
```

```csharp
if (_trayIconFlyout.IsOpen)
    _trayIconFlyout.Hide();
else
    _trayIconFlyout.Show();
```

## Screenshots

Visit the repo's README: https://github.com/Jack251970/TrayIconFlyout.Wpf

## License

Copyright (c) 2026 Jack251970. All rights reserved.
