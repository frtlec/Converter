using System.Xml.Serialization;

namespace Blazor.UI.Models
{
    public class Item
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Id { get; set; }
        public string Barcode { get; set; }
        public string ImageLink { get; set; }
        public string AdditionalImageLink1 { get; set; }
        public string ModelNumber { get; set; }
        public decimal Price { get; set; }
        public string GoogleProductCategory { get; set; }
        public List<string> ProductType { get; set; }


        [XmlIgnore]
        public string ProductTypeStr { get; set; }
        [XmlIgnore]
        public string CategoryTypeStr { get; set; }
        public List<string> Category { get; set; }
        public int Quantity { get; set; }
        public string Availability { get; set; }
    }
}
