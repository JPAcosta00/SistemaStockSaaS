
namespace Application.DTOs{
    public class DashboardDataDto
    {
        public DashboardMetricsDto Metrics { get; set; } = null!;
        public List<TopProductDto> TopProducts { get; set; } = new();
    }
}