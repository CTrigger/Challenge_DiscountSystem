namespace DataFileIO
{
    public class FileIOSettings
    {
        public bool OverwriteFile { get; set; }
        public bool IsProtected { get; set; }
        public FileIOSettings()
        {
            OverwriteFile = true;
            IsProtected = false;
        }
    }
}
