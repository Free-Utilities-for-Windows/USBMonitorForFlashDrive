namespace USBMonitor.Models;

public class MyFile
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FullPath { get; set; }
    public Drive Drive { get; set; }
    public int FolderId { get; set; }
    public MyFolder Folder { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastAccessTime { get; set; }
    public DateTime LastWriteTime { get; set; }
}