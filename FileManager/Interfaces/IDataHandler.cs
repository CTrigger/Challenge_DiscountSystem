using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscountCore;
namespace FileManager.Interfaces
{
    public interface IDataHandler
    {
        IEnumerable<DiscountData> DiscountRepository { get; set; }
        Task SaveData();
        Task SaveData_DiscountContract();
    }
}
