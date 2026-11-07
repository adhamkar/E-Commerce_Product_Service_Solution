namespace BusinessLogic.DTO;

    public record ProductResponse(
        Guid ProductID,
        string ProductName,
        string Category,
        decimal UnitPrice,
        int QuantityInStock,
        int UnitsInStock
    )
    {
        public ProductResponse() : this(default, default, default, default, default, default)
        {
        }
    }

