namespace Shared.OrderDtos
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string PrductName { get; set; }
        public string PictureUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}