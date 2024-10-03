using DiscountCore;
namespace FileManager.Interfaces
{
    public interface IDataHandler
    {
        IEnumerable<DiscountData> DiscountRepository { get; set; }
        Task SaveData();
        Task SaveData_DiscountContract();
        Task<bool> CodeUse(string code);
    }
}
