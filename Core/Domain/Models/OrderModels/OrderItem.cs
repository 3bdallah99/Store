namespace Domain.Models.OrderModels
{
    public class OrderItem : BaseEntity<Guid>
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductInOrderItem productInfo, int quantity, decimal price)
        {
            ProductInfo = productInfo;
            Quantity = quantity;
            Price = price;
        }

        public ProductInOrderItem ProductInfo { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}