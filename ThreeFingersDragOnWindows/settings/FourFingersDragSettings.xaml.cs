using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;

namespace ThreeFingersDragOnWindows.settings;

public sealed partial class FourFingersDragSettings {

    public FourFingersDragSettings(){
        InitializeComponent();
    }

    private void OpenSettings(object sender, RoutedEventArgs e){
        _ = Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:devices-touchpad"));
    }
    
    public bool EnabledProperty
    {
        get { return App.SettingsData.FourFingersDrag; }
        set { App.SettingsData.FourFingersDrag = value; }
    }
    
    public bool AllowReleaseAndRestartProperty
    {
        get { return App.SettingsData.FourFingersDragAllowReleaseAndRestart; }
        set { App.SettingsData.FourFingersDragAllowReleaseAndRestart = value; }
    }
    
    public int ReleaseDelayProperty
    {
        get { return App.SettingsData.FourFingersDragReleaseDelay; }
        set { App.SettingsData.FourFingersDragReleaseDelay = value; }
    }
    
    public bool CursorMoveProperty
    {
        get { return App.SettingsData.FourFingersDragCursorMove; }
        set { App.SettingsData.FourFingersDragCursorMove = value; }
    }
    
    public float CursorSpeedProperty
    {
        get { return App.SettingsData.FourFingersDragCursorSpeed; }
        set { App.SettingsData.FourFingersDragCursorSpeed = value; }
    }
    
    public float CursorAccelerationProperty
    {
        get { return App.SettingsData.FourFingersDragCursorAcceleration; }
        set { App.SettingsData.FourFingersDragCursorAcceleration = value; }
    }
}