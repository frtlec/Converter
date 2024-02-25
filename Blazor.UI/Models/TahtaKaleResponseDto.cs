using MudBlazor;

namespace Blazor.UI.Models
{
    public class TahtaKaleGetAllByFilterResponse
    {
        public long Total { get; set; }
        public int PageSize { get { return this.Data.Count; } }
        public List<TahtakaleProduct> Data { get; set; }
    }
    public class TahtaKaleGetAllByFilterRequest
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public string ModelNumber { get; set; } = string.Empty;
        public decimal? BeginPrice { get; set; }
        public decimal? EndPrice { get; set; }

        public Sort Sort { get; set; } = new Sort
        {
            Direction = SortDirection.Descending,
            Column = "ProductId"
        };
    }
    public class Sort
    {
        public SortDirection Direction { get; set; }
        public string Column { get; set; }
    }
    public class TahtakaleProduct
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string ProductId { get; set; }
        public string Barcode { get; set; }
        public string ImageLink { get; set; }
        public string AdditionalImageLink1 { get; set; }
        public string ModelNumber { get; set; }
        public decimal Price { get; set; }
        public string GoogleProductCategory { get; set; }
        public List<string> ProductType { get; set; }
        public List<string> Category { get; set; }
        public int Quantity { get; set; }
        public string Availability { get; set; }
    }
}
