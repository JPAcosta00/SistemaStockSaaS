namespace Application.DTOs
{
    public class DashboardMetricsDto{
        public decimal TotalRevenue { get; set; }
        public int TotalSalesCount { get; set; }
        public int ActiveProductsCount { get; set; }
        public int LowStockAlertsCount { get; set; }
    }

}