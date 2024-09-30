using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCore.BLL
{
    public class DiscountException : Exception
    {
        public DiscountException() { }
        public DiscountException(string Message) : base(message: Message)
        {

        }
    }
}
