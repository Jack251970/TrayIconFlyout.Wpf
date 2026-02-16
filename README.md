<h1 align="center">Tray Icon Flyout</h1>
<p align="center">Empower your app with a flyout that pops up from a tray icon in WPF.</p>

## Installing the package

You can consume this project via NuGet. Use NuGet Package Manager or run the following command in the Package Manager Console:

<a style="text-decoration:none" href="https://www.nuget.org/packages/Jack251970.TrayIconFlyout.Wpf"><img src="https://img.shields.io/nuget/v/Jack251970.TrayIconFlyout.Wpf" alt="NuGet badge" /></a>

```console
> dotnet add package Jack251970.TrayIconFlyout.Wpf
```

> If you want to use the features in UWP or WinUI 3, please check out https://github.com/0x5bfa/TrayIconFlyout.

## Usage

### SystemTrayIcon

Use `SystemTrayIcon` to create a tray icon for your app.

```csharp
SystemTrayIcon = new(
    new("21B7FA20-C95D-450E-9AB8-DA6E9719AEBA"),
    new("Assets\\Tray.ico"),
    "TrayIconFlyout sample app (WPF)");
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

## Building from the source

1. Prerequisites
    - Windows 10 (Build 10.0.17763.0) onwards and Windows 11
    - Visual Studio 2022
    - .NET 9/10 SDK
2. Clone the repo
    ```console
    git clone https://github.com/Jack251970/TrayIconFlyout.Wpf.git
    ```
3. Open the solution
4. Build the solution

## Screenshot

https://github.com/user-attachments/assets/95a63647-1f96-4035-a65d-1b602112c4bf
