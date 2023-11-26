using USBMonitor.Models;
using File = System.IO.File;

namespace USBMonitor;

public class DriveManager
{
    private DriveMonitor monitor;

    public DriveManager()
    {
        monitor = new DriveMonitor();
        monitor.DriveInserted += OnDriveInserted;
        monitor.DriveRemoved += OnDriveRemoved;
    }

    private async void OnDriveInserted(object sender, DriveEventArgs e)
    {
        Console.WriteLine($"Drive {e.DriveName} was inserted");

        try
        {
            DirectoryScanner scanner = new DirectoryScanner();
            Console.WriteLine("Scanning directory...");
            MyFolder rootFolder = await Task.Run(() => scanner.ScanDirectory(e.DriveName));

            Console.WriteLine("Flattening folder structure...");
            List<MyFolder> folders = scanner.FlattenFolderStructure(rootFolder);

            FolderExporter exporter = new FolderExporter();
            Console.WriteLine("Saving to Excel...");
            string deviceName = new DriveInfo(e.DriveName).VolumeLabel;
            string deviceFolderPath = await Task.Run(() => exporter.SaveToExcel(folders, deviceName));

            Console.WriteLine("Copying files...");
            var (textFilesCount, imageFilesCount) = await Task.Run(() => exporter.CopyFiles(folders, deviceFolderPath));
            Console.WriteLine($"Copied {textFilesCount} text files and {imageFilesCount} image files.");

            Console.WriteLine("Done.");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access to the path is denied: {ex.Message}");
            throw;
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An I/O error occurred: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }

    private async void OnDriveRemoved(object sender, DriveEventArgs e)
    {
        Console.WriteLine($"Drive {e.DriveName} was removed");

        try
        {
            DirectoryScanner scanner = new DirectoryScanner();
            MyFolder rootFolder = await Task.Run(() => scanner.ScanDirectory(e.DriveName));

            List<MyFolder> folders = scanner.FlattenFolderStructure(rootFolder);
            FolderExporter exporter = new FolderExporter();
            await Task.Run(() => exporter.SaveToExcel(folders, e.DriveName));
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access to the path is denied: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An I/O error occurred: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public void StartMonitoring()
    {
        monitor.Start();
    }

    public void StopMonitoring()
    {
        monitor.Stop();
    }
}