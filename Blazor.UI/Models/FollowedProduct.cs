namespace Blazor.UI.Models
{
    public class FollowedProduct
    {
        public string Id { get; set; }
        public int ProductId { get; set; }
        public string Barcode { get; set; }
        public decimal TrendyolPrice { get; set; }
        public int TrendyolStock { get; set; }
        public string TrendyolLink { get; set; }
        public SourceSite SourceSite { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class FollowedProductAddDto
    {
        public int ProductId { get; set; }
        public string Barcode { get; set; }
        public SourceSite SourceSite { get; set; }
    }
    public class FollowedProductUpdate
    {
        public string Id { get; set; }
        public decimal TrendyolPrice { get; set; }
        public int TrendyolStock { get; set; }
        public string TrendyolLink { get; set; }
    }
}
