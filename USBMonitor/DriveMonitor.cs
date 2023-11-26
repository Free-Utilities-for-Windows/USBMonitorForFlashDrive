using System.Management;
using USBMonitor.Models;

namespace USBMonitor;

public class DriveMonitor : IDisposable
{
    private ManagementEventWatcher watcher;

    public delegate void DriveInsertedEventHandler(object sender, DriveEventArgs e);

    public delegate void DriveRemovedEventHandler(object sender, DriveEventArgs e);

    public event DriveInsertedEventHandler DriveInserted;
    public event DriveRemovedEventHandler DriveRemoved;

    public DriveMonitor()
    {
        watcher = new ManagementEventWatcher();
        watcher.EventArrived += new EventArrivedEventHandler(USBChanged);
        watcher.Query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 OR EventType = 3");
    }

    public void Start()
    {
        watcher.Start();
    }

    public void Stop()
    {
        watcher.Stop();
    }

    public void Dispose()
    {
        watcher.Dispose();
    }

    private void USBChanged(object sender, EventArrivedEventArgs e)
    {
        string driveName = e.NewEvent.Properties["DriveName"].Value.ToString();

        if (Convert.ToInt32(e.NewEvent.Properties["EventType"].Value) == 2)
        {
            Console.WriteLine($"Drive {driveName} has been inserted");

            DriveInserted?.Invoke(this, new DriveEventArgs { DriveName = driveName });
        }
        else if (Convert.ToInt32(e.NewEvent.Properties["EventType"].Value) == 3)
        {
            Console.WriteLine($"Drive {driveName} has been removed");

            DriveRemoved?.Invoke(this, new DriveEventArgs { DriveName = driveName });
        }
    }
}

public class DriveEventArgs : EventArgs
{
    public string DriveName { get; set; }
}