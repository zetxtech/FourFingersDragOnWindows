using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Microsoft.Win32.TaskScheduler;
using Microsoft.Win32;

namespace ThreeFingersDragOnWindows.utils; 

public static class StartupManager {


    public static void EnableElevatedStartup(){
        DisableUnelevatedStartup();
        
        Debug.WriteLine("Enabling elevated startup task...");
        
        using TaskService taskService = new TaskService();
        TaskFolder folder = taskService.RootFolder.CreateFolder("FourFingersDragOnWindows", null, false);
        folder.AllTasks.ToList().ForEach(task => folder.DeleteTask(task.Name, false));

        TaskDefinition taskDefinition = taskService.NewTask();
        taskDefinition.RegistrationInfo.Description = "Starting FourFingersDragOnWindows on system startup with elevated privileges.";
        taskDefinition.RegistrationInfo.Author = "Clément Grennerat";
        taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;

        LogonTrigger logonTrigger = new LogonTrigger();
        taskDefinition.Triggers.Add(logonTrigger);

        ExecAction execAction = new ExecAction(Utils.GetAppPath(), "");
        taskDefinition.Actions.Add(execAction);

        folder.RegisterTaskDefinition(
            "Run on Startup", // Task name
            taskDefinition,
            TaskCreation.CreateOrUpdate,
            null, // User credentials (null for current user)
            null, // User account password
            TaskLogonType.InteractiveToken);
    }
    public static void DisableElevatedStartup(){
        Debug.WriteLine("Disabling elevated startup task...");
        
        using TaskService taskService = new TaskService();
        TaskFolder folder = taskService.RootFolder.CreateFolder("FourFingersDragOnWindows", null, false);
        folder.AllTasks.ToList().ForEach(task => folder.DeleteTask(task.Name, false));
        taskService.RootFolder.DeleteFolder("FourFingersDragOnWindows", false);
    }
    public static bool IsElevatedStartupOn(){
        using TaskService taskService = new TaskService();
        return taskService.RootFolder.SubFolders.Any(folder => folder.Name == "FourFingersDragOnWindows" && folder.Tasks.Count != 0);
    }

    public static void EnableUnelevatedStartup(){
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
        {
            key.SetValue("FourFingersDragOnWindows", "\"" + Utils.GetAppPath() + "\"");
        }
    }
    public static void DisableUnelevatedStartup(){
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
        {
            key.DeleteValue("FourFingersDragOnWindows", false);
        }
    }
    public static bool IsUnelevatedStartupOn(){
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false))
        {
            object value = key.GetValue("FourFingersDragOnWindows");
            if (value != null)
            {
                string registryPath = value.ToString().Replace("\"", "");
                return String.Equals(Utils.GetAppPath(), registryPath, StringComparison.InvariantCultureIgnoreCase);
            }
        }
        return false;
    }
}