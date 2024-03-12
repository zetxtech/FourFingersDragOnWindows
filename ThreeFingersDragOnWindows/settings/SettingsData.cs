using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Windows.Storage;

namespace ThreeFingersDragOnWindows.settings;

public class SettingsData {
    
    // Three fingers drag Settings
    public bool RegularTouchpadCheck{ get; set; } = true;
    public int RegularTouchpadCheckInterval{ get; set; } = 5;
    public bool RegularTouchpadCheckEvenAlreadyRegistered{ get; set; } = false;
    
    // Three fingers drag Settings
    public bool ThreeFingersDrag{ get; set; } = true;
    
    public bool ThreeFingersDragAllowReleaseAndRestart{ get; set; } = true;
    public int ThreeFingersDragReleaseDelay{ get; set; } = 500;

    public bool ThreeFingersDragCursorMove{ get; set; } = true;
    public float ThreeFingersDragCursorSpeed{ get; set; } = 100;
    public float ThreeFingersDragCursorAcceleration{ get; set; } = 2.5F;

    // Four fingers drag Settings
    public bool FourFingersDrag { get; set; } = true;

    public bool FourFingersDragAllowReleaseAndRestart { get; set; } = true;
    public int FourFingersDragReleaseDelay { get; set; } = 500;

    public bool FourFingersDragCursorMove { get; set; } = true;
    public float FourFingersDragCursorSpeed { get; set; } = 40;
    public float FourFingersDragCursorAcceleration { get; set; } = 1.5F;

    // Other settings

    public enum StartupActioType {
        NONE,
        ENABLE_ELEVATED_RUN_WITH_STARTUP,
        DISABLE_ELEVATED_RUN_WITH_STARTUP,
        ENABLE_ELEVATED_STARTUP,
        DISABLE_ELEVATED_STARTUP,
    }

    public StartupActioType StartupAction{ get; set; } = StartupActioType.NONE;
    
    public bool RunElevated{ get; set; } = false;

    // Other
    public static bool IsFirstRun{ get; set; } = false;

    public static SettingsData load(){
        var mySerializer = new XmlSerializer(typeof(SettingsData));
        var myFileStream = new FileStream(getPath(true), FileMode.Open);
        SettingsData up;
        try{
            up = (SettingsData) mySerializer.Deserialize(myFileStream);
            myFileStream.Close();
        } catch(Exception e){
            Console.WriteLine(e);
            myFileStream.Close();
            up = new SettingsData();
            up.save();
        }
        return up;
    }

    public void save(){
        var mySerializer = new XmlSerializer(typeof(SettingsData));
        var myWriter = new StreamWriter(getPath(false));
        mySerializer.Serialize(myWriter, this);
        myWriter.Close();
    }

    private static string getPath(bool createIfEmpty){
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dirPath = Path.Combine(appDataPath, "FourFingersDragOnWindows");
        var filePath = Path.Combine(dirPath, "preferences.xml");
        if (!Directory.Exists(dirPath) || !File.Exists(filePath)){
            Debug.WriteLine("First run: creating settings file");
            Directory.CreateDirectory(dirPath);
            IsFirstRun = true;
            if(createIfEmpty) new SettingsData().save();
        }

        return filePath;
    }
}