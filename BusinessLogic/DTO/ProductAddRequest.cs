using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO
{
    public record ProductAddRequest(
        string ProductName,
        string Category,
        double UnitPrice,
        int QuantityInStock,
        int UnitsInStock
    )
    {
        public ProductAddRequest() : this(default, default, default,default, default)
        {
        }
    }

}
