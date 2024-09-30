
namespace DataFileIO.Interfaces
{
    public interface IFileIO
    {
        Task SaveFile(string filePath, byte[] data);
        Task SaveFile(string filePath, byte[] data, FileIOSettings settings);
        Task<byte[]> ReadFile(string filePath);
        Task<byte[]> ReadFile(string filePath, FileIOSettings settings);
        byte[] ObjectToBytes<T>(T obj);
        byte[] ObjectToBytes<T>(T obj, IEnumerable<Type> knownTypes);
        T BytesToObject<T>(byte[] obj);
        T BytesToObject<T>(byte[] obj, IEnumerable<Type> knownTypes);

    }
}
