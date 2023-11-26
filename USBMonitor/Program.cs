using USBMonitor;

class Program
{
    static void Main(string[] args)
    {
        DriveManager driveManager = new DriveManager();
        driveManager.StartMonitoring();

        Console.WriteLine("Press any key to stop monitoring...");
        Console.ReadKey();

        driveManager.StopMonitoring();
    }
};