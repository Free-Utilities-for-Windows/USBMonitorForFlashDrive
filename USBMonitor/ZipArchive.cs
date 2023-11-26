using System.IO.Compression;

namespace USBMonitor;

public static class ZipArchive
{
    public static void CreateZipArchive(string folderPath, string archivePath)
    {
        ZipFile.CreateFromDirectory(folderPath, archivePath);
    }
}