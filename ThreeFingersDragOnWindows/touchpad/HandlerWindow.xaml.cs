using System;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using ThreeFingersDragEngine.utils;
using ThreeFingersDragOnWindows.threefingersdrag;
using ThreeFingersDragOnWindows.utils;

namespace ThreeFingersDragOnWindows.touchpad;

public sealed partial class HandlerWindow : Window {
    private readonly App _app;
    private readonly ContactsManager _contactsManager;
    private readonly ThreeFingersDrag _threeFingersDrag;

    public bool TouchpadInitialized;
    public bool TouchpadExists;
    public bool TouchpadRegistered;

    public HandlerWindow(App app){
        Debug.WriteLine("Starting HandlerWindow...");
        InitializeComponent();

        _app = app;
        _contactsManager = new ContactsManager(this);
        _threeFingersDrag = new ThreeFingersDrag();

        // Let the _handlerWindow to be defined in App.xaml.cs before initializing the source
        Utils.runOnMainThreadAfter(100, () => {
            _contactsManager.InitializeSource();
        });
        
    }

    // TaskbarIcon Actions
    private void OpenSettingsWindow(object sender, ExecuteRequestedEventArgs e){
        Debug.WriteLine("Opening SettingsWindow from HandlerWindow TaskbarIcon");
        _app.OpenSettingsWindow();
    }

    private void QuitApp(object sender, ExecuteRequestedEventArgs e){
        Debug.WriteLine("Quitting App from HandlerWindow TaskbarIcon");
        _app.Quit();
    }


    // Touchpad
    // Called when the touchpad is detected and the events handlers are registered (or not)
    public void OnTouchpadInitialized(bool touchpadExists, bool touchpadRegistered){
        TouchpadExists = touchpadExists;
        TouchpadRegistered = touchpadRegistered;
        if(!touchpadExists) Debug.WriteLine("Touchpad is not detected.");
        else if(!touchpadRegistered) Debug.WriteLine("Touchpad is detected but not registered.");
        else Debug.WriteLine("Touchpad is detected and registered.");
        
        TouchpadInitialized = true;
        _app.OnTouchpadInitialized();
    }

    // Called when a new set of contacts has been registered
    
    private TouchpadContact[] _oldContacts = Array.Empty<TouchpadContact>();
    private long _lastContactCtms = Ctms();
    
    public void OnTouchpadContact(TouchpadContact[] contacts){
        _threeFingersDrag.OnTouchpadContact(_oldContacts, contacts, Ctms() - _lastContactCtms);
        _app.OnTouchpadContact(contacts); // Transfer to App for displaying contacts in SettingsWindow
        
        _lastContactCtms = Ctms();
        _oldContacts = contacts;
    }
    
    private static long Ctms(){
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }
}