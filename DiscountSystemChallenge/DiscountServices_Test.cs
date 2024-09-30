using DiscountCore;
using DiscountServices;
using DiscountServices.Interfaces;
using Moq;
namespace DiscountSystemChallenge
{
    [TestFixture]
    public class DiscountServices_Test
    {
        #region Variables
        private IDiscountManager _discountManager;
        #endregion

        #region ctor
        public DiscountServices_Test()
        {
            var fileIO =
                new Mock<DataFileIO.FileIO>();
                //Mock.Of<DataFileIO.FileIO>();
            var dataHandler = 
                new Mock<FileManager.DataHandler>(fileIO.Object);
                //Mock.Of<FileManager.DataHandler>();
            dataHandler
                .Setup(dh => dh.LoadData())
                .Verifiable();
            _discountManager = new DiscountManager(dataHandler.Object);
        }
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
          
        }
        #endregion
        #region Test
        [Test,NonParallelizable]
        [TestCase("1")]
        [TestCase("20")]
        [TestCase("2000")]
        [TestCase("20000")]
        //[TestCase("65535")]//Need trouble shoot (race condition problem)
        public void GenerateDiscounts(ushort batches)
        {
            int real_total = 0;
            int distinct_total = 0;
            Assert.DoesNotThrow(() =>
            {
                IEnumerable<DiscountData> results;
                results = Task.Run( ()=>_discountManager.GenerateDiscountCode(batches)).Result;
                real_total = results.Count();
                distinct_total = results.DistinctBy(x => x.Code).Count();
            });

            Assert.IsTrue(real_total == distinct_total, $"Reapeated codes generated diff: {real_total - distinct_total}");
            Assert.IsTrue(batches == distinct_total, $"Codes request doesn't match with the requirements: {distinct_total}");


        }
        #endregion
    }
}
