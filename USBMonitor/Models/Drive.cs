namespace USBMonitor.Models;

public class Drive
{
    public string Name { get; set; }
    public string VolumeLabel { get; set; }
    public string DriveFormat { get; set; }
    public long TotalSize { get; set; }
    public long AvailableFreeSpace { get; set; }
    public bool IsReady { get; set; }
    public bool IsReadOnly { get; set; }
}