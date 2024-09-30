using DataFileIO.Interfaces;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
namespace DataFileIO
{
    public class FileIO : IFileIO
    {
        #region Variable

        #endregion

        #region ctor
        public FileIO()
        {

        }

        #endregion

        #region Methods
        public virtual async Task SaveFile(string filePath, byte[] data)
        {
            await SaveFile(filePath, data, new FileIOSettings());
        }
        public virtual async Task SaveFile(string filePath, byte[] data, FileIOSettings settings)
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
                await File.WriteAllBytesAsync(filePath, data_to_write);
            }
            catch (Exception e)
            {
                //log error
                throw e;
            }
        }

        public virtual async Task<byte[]> ReadFile(string filePath)
        {
            return await ReadFile(filePath, new FileIOSettings());

        }
        public virtual async Task<byte[]> ReadFile(string filePath, FileIOSettings settings)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", Path.GetFileName(filePath));

            if (settings == null)
                settings = new FileIOSettings();

            byte[] data = settings.IsProtected ? DescryptData(await File.ReadAllBytesAsync(filePath)) : await File.ReadAllBytesAsync(filePath);

            return data;
        }

        public byte[] ObjectToBytes<T>(T obj)
        {
            return ObjectToBytes(obj, null);
        }
        public byte[] ObjectToBytes<T>(T obj, IEnumerable<Type> knownTypes)
        {
            if (obj == null)
                throw new ArgumentNullException($"Parameter is null: {nameof(obj)}");

            byte[] result= Array.Empty<byte>();
            lock(this)
            {
                DataContractSerializer xmlSerializer = knownTypes == null
                   ? new DataContractSerializer(typeof(T))
                   : new DataContractSerializer(typeof(T), knownTypes);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (XmlWriter writer = XmlWriter.Create(ms, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                    {
                        // Serialize the object to the XML writer
                        xmlSerializer.WriteObject(writer, obj);
                        writer.Flush(); // Ensure all the data is written to the stream
                    }
                    result = ms.ToArray(); // Return the byte array
                }

            }
            return result;

        }

        public T BytesToObject<T>(byte[] obj)
        {
            return BytesToObject<T>(obj, null);
        }
        public T BytesToObject<T>(byte[] obj, IEnumerable<Type> knownTypes)
        {
            if (obj == null || obj.Length == 0)
                throw new ArgumentNullException("No data to convert");

            using (MemoryStream ms = new MemoryStream(obj))
            {
                DataContractSerializer xmlSerializer = knownTypes == null
                    ? new DataContractSerializer(typeof(T))
                    : new DataContractSerializer(typeof(T), knownTypes);

                ms.Position = 0;

                return (T)xmlSerializer.ReadObject(ms);
            }
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

        #endregion
    }

}
