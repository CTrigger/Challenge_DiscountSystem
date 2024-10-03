using DataFileIO.Interfaces;
using DiscountCore;
using FileManager.Interfaces;
using System.Runtime.CompilerServices;
using System.Xml;
[assembly: InternalsVisibleTo("DiscountSystemChallenge")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace FileManager
{
    public class DataHandler : IDataHandler
    {
        #region Properties
        public IEnumerable<DiscountData> DiscountRepository { get; set; }

        private readonly XmlReaderSettings ReaderSettings = new XmlReaderSettings() { IgnoreComments = true, IgnoreWhitespace = true };
        private readonly XmlWriterSettings WriterSettings = new XmlWriterSettings() { Indent = false };
        private static string PathFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private static string DiscountRepositoryPath = Path.Combine(PathFolder, "DiscountContract.xml");

        private readonly IFileIO _access;

        #endregion

        #region Ctor
        public DataHandler(IFileIO access)
        {
            _access = access;
            LoadData();
        }
        ~DataHandler()
        {
            SaveData();
        }
        #endregion

        #region Methods

        #region Read
        internal virtual async Task LoadData()
        {
            await LoadData_DiscountContract();
        }
        internal async Task LoadData_DiscountContract()
        {
            if (!File.Exists(DiscountRepositoryPath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(DiscountRepositoryPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(DiscountRepositoryPath));
                IEnumerable<DiscountData> empty = Enumerable.Empty<DiscountData>();
                byte[] empty_data = _access.ObjectToBytes(empty);
                await _access.SaveFile(DiscountRepositoryPath, empty_data);
            }
            byte[] data = await _access.ReadFile(DiscountRepositoryPath);

            DiscountRepository = data == Array.Empty<byte>() ? Enumerable.Empty<DiscountData>() : _access.BytesToObject<IEnumerable<DiscountData>>(data);

        }
        #endregion

        #region Write
        public async Task SaveData()
        {
            await SaveData_DiscountContract();
        }
        public async Task SaveData_DiscountContract()
        {
            byte[] data = _access.ObjectToBytes<IEnumerable<DiscountData>>(DiscountRepository);
            await _access.SaveFile(DiscountRepositoryPath, data);
        }

        public async Task<bool> CodeUse(string code)
        {
            DiscountData filter = DiscountRepository.Where(w => w.Code == code && !w.IsUsed).Single();
            if (filter == null)
                return false;
            else
            {
                filter.IsUsed = true;
                await SaveData_DiscountContract();
            }

            return true;
        }
        #endregion

        #endregion
    }
}
