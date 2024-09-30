using DiscountCore.BLL;
using DiscountCore.BLL.CustomAttribute;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DiscountCore
{
    [DataContract(Namespace = "Discount.v1")]
    public class DiscountData
    {
        #region Variables
        private string _code;
        private UInt16 _value;
        private DateTime _expirationDate;
        #endregion

        #region Members
        [DataMember(IsRequired = true)]
        [StringLength(maximumLength: 8, MinimumLength = 7, ErrorMessage = ERROR_MESSAGE.STRING_LENGTH)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = ERROR_MESSAGE.ONLY_ALPHANUMERIC)]
        public string Code { get => _code; set { LogicLayer.ValidateProperty(value, this, nameof(Code)); _code = value; } }

        [DataMember(IsRequired = true)]
        //[DefaultValue(1)]
        [Range(minimum: 1, maximum: 100, ErrorMessage = ERROR_MESSAGE.UNREAL_PERCENTAGE_DISCOUNT)]
        public UInt16 Percentage { get => _value; set { LogicLayer.ValidateProperty(value, this, nameof(Percentage)); _value = value; } }

        [DataMember(IsRequired = true)]
        [DefaultValue(false)]
        public bool IsUsed { get; set; }

        [DataMember(IsRequired = false)]
        [FutureDate(ErrorMessage = ERROR_MESSAGE.DATE_IN_PAST)]
        public DateTime ExpirationDate { get => _expirationDate; set { LogicLayer.ValidateProperty(value, this, nameof(ExpirationDate)); _expirationDate = value; } }
        #endregion
    }
}
