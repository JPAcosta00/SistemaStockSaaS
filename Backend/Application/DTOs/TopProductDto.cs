namespace Application.DTOs
{
    public class TopProductDto{
        public string ProductName { get; set; } = string.Empty;
        public int SalesCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}