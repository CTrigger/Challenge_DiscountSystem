using DiscountCore;
using DiscountCore.BLL;
using System.ComponentModel.DataAnnotations;

namespace DiscountSystemChallenge
{
    [TestFixture]
    public class DiscountCore_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("@t?steas", ERROR_MESSAGE.ONLY_ALPHANUMERIC)]
        [TestCase("012345", ERROR_MESSAGE.STRING_LENGTH)]
        [TestCase("012345678", ERROR_MESSAGE.STRING_LENGTH)]
        [TestCase("4#$%5!", ERROR_MESSAGE.STRING_LENGTH + "; " + ERROR_MESSAGE.ONLY_ALPHANUMERIC)]
        public void InvalidCodes(string input, string expectedResult)
        {
            DiscountData discountData = new DiscountData();
            ValidationException exception = Assert.Throws<ValidationException>(() =>
            {
                discountData.Code = input;
            });
            Assert.IsTrue(exception.Message == expectedResult, $"Actual Result: {exception.Message}");

        }

        [Test]
        [TestCase("2023-01-01",ERROR_MESSAGE.DATE_IN_PAST)]
        [TestCase("2022-01-01",ERROR_MESSAGE.DATE_IN_PAST)]
        public void InvalidDate(DateTime datetime, string expectedResult)
        {
            DiscountData data = new DiscountData();
            ValidationException exception = Assert.Throws<ValidationException>(() =>
            {
                data.ExpirationDate = datetime;
            });
            Assert.IsTrue(exception.Message == expectedResult, $"Actual Result: {exception.Message}");

        }
        
        [Test]
        [TestCase("0",ERROR_MESSAGE.UNREAL_PERCENTAGE_DISCOUNT)]
        [TestCase("120",ERROR_MESSAGE.UNREAL_PERCENTAGE_DISCOUNT)]
        public void InvalidDiscount(UInt16 percentage, string expectedResult)
        {
            DiscountData data = new DiscountData();
            ValidationException exception = Assert.Throws<ValidationException>(() =>
            {
                data.Percentage = percentage;
            });
            Assert.IsTrue(exception.Message == expectedResult, $"Actual Result: {exception.Message}");

        }
    }
}