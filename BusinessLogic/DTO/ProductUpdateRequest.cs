using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO
{
    public record ProductUpdateRequest(
        Guid ProductID,
        string ProductName,
        string Category,
        double UnitPrice,
        int QuantityInStock,
        int UnitsInStock
    )
    {
        public ProductUpdateRequest() : this(default,default,default,default,default, default)
        {
        }
    }

}
