using DataIO;
namespace FileIO
{
    public class FileIO
    {
        #region Variable

        #endregion

        #region ctor
        public FileIO()
        {

        }

        #endregion

        #region Methods
        public virtual void SaveFile(string filePath, byte[] data, FileIOSettings settings = null)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                throw new DirectoryNotFoundException($"Path {Path.GetDirectoryName(filePath)} doesn't exists");

            if (settings == null)
                settings = new FileIOSettings();

            if (settings.OverwriteFile && File.Exists(filePath))
                File.Delete(filePath);


            byte[] data_to_write = settings.IsProtected ? EncryptData(data) : data;
            try
            {
                File.WriteAllBytes(filePath, data_to_write);
            }
            catch (Exception e)
            {
                //log error
                throw e;
            }
        }

        public virtual byte[] ReadFile(string filePath, FileIOSettings settings = null)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", Path.GetFileName(filePath));

            if (settings == null)
                settings = new FileIOSettings();

            byte[] data = settings.IsProtected ? DescryptData(File.ReadAllBytes(filePath)) : File.ReadAllBytes(filePath);

            return data;
        }
        #endregion
        #region Aux
        internal byte[] EncryptData(byte[] data)
        {
            throw new NotImplementedException();
        }
        internal byte[] DescryptData(byte[] data)
        {
            throw new NotImplementedException();
        }

        internal byte[] ObjectToBytes<T>(T obj, IEnumerable<Type> knownTypes = null)
        {
            return new byte[0];
            if (obj == null)
                throw new ArgumentNullException($"Parameter is null: {nameof(obj)}");

            //DataContractSerializer xmlSerializer = knownTypes == null ? new DataContractSerializer(typeof(T): new DataContractSerializer(typeof(T), knownTypes));
            
            using (MemoryStream ms = new MemoryStream())
            {

            }
        }
        #endregion
    }

}
