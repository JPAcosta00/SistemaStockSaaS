namespace Application.DTOs
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockMinimum { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}