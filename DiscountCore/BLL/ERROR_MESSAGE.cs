using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCore.BLL
{
    public static class ERROR_MESSAGE
    {
        public const string STRING_LENGTH = "The length doesn't match with the requirements.";
        public const string ONLY_ALPHANUMERIC = "Only alphanumeric characters are allowed.";
        public const string UNREAL_PERCENTAGE_DISCOUNT = "Discount value is unreal.";
        public const string DATE_IN_PAST = "The date cannot be in the past.";
    }
}
