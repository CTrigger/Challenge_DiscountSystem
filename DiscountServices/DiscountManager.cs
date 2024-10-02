using DiscountCore;
using DiscountServices.Interfaces;
using FileManager.Interfaces;
namespace DiscountServices
{
    public class DiscountManager : IDiscountManager
    {
        #region Variables
        private static string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //private static string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly IDataHandler _repository;
        #endregion

        #region Ctor
        public DiscountManager(IDataHandler repository)
        {
            _repository = repository;
        }

        #endregion

        #region Methods
        public async Task<DiscountData> GenerateDiscountCode()
        {
            throw new NotImplementedException();
            //   return await GenerateDiscountCode(1).Result.First();
        }

        public async Task<IEnumerable<DiscountData>> GenerateDiscountCode(ushort batch)
        {
            if (batch == 0)
                return Enumerable.Empty<DiscountData>();
            if (_repository.DiscountRepository == null)
                _repository.DiscountRepository = Enumerable.Empty<DiscountData>();

            uint distinct = batch;
            IEnumerable<DiscountData> result = new List<DiscountData>();
            do
            {
                result = (from code in GenerateDistinctCodes(distinct)
                          select code)
                          .Concat(result)
                          .DistinctBy(x => x.Code)
                          .ToArray();

                result = result
                    .Where(fresh => !_repository.DiscountRepository.Any(existing => existing.Code == fresh.Code))
                    .ToArray();


                distinct = (uint)(batch - result.Count());
            }
            while (distinct > 0);

            _repository.DiscountRepository = _repository.DiscountRepository.Concat(result);
            await _repository.SaveData_DiscountContract();
            return result;
        }
        #endregion

        #region aux

        private IEnumerable<DiscountData> GenerateDistinctCodes(uint batch)
        {
            if (batch == 0)
                yield break;

            while (0 < batch--)
            {
                string code = GetRandomCode();
                yield return new DiscountData()
                {
                    Code = code,
                    Percentage = 1,//just for test
                    ExpirationDate = DateTime.Parse("2099-12-30"),
                };
            }

        }
        private string GetRandomCode()
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            int codeLength = r.Next(7, 9);
            return new string(
                Enumerable
                    .Range(0, codeLength)
                    .Select(_ => characters[r.Next(characters.Length)])
                    .ToArray());
        }
        #endregion
    }
}
