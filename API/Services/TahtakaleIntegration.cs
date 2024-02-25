using OfficeOpenXml;
using System.Xml;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Xml.Serialization;
using System.Linq.Expressions;
using System.Web;
using API.Databases.Mongo.DataAccess;
using API.Databases.Mongo.DataModels;

namespace API.Services
{
    public class TahtakaleIntegration
    {
        private readonly TahtakaleService tahtakaleService;
        public TahtakaleIntegration(TahtakaleService tahtakaleService)
        {
            this.tahtakaleService = tahtakaleService;
        }
        public async Task<byte[]> GetExcelBytes()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(await Request());
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");

                    // XML dosyasındaki verileri Excel dosyasına aktar

                    string[] columns = { "Başlık", "Barkod", "ModelNO", "Fiyat", "Stok", "Açıklama", "Link", "kategori", "ürün tipi" };

                    for (int i = 0; i < columns.Length; i++)
                    {

                        worksheet.Cells[1, i + 1].Value = columns[i];
                    }



                    XmlNodeList items = doc.SelectNodes("//item");
                    int row = 2;
                    foreach (XmlNode item in items)
                    {

                        string categoriesStr;
                        XmlNodeList productTypes = item.SelectNodes("product_type");

                        XmlNodeList categories = item.SelectNodes("category");

                        string joinedProductTypes = string.Empty;
                        if (productTypes.Count > 0)
                        {
                            // XmlNodeList'i bir diziye dönüştürme
                            string[] productTypeArray = new string[productTypes.Count];
                            for (int i = 0; i < productTypes.Count; i++)
                            {
                                productTypeArray[i] = productTypes[i].InnerText;
                            }

                            // Diziyi birleştirme
                            joinedProductTypes = string.Join(", ", productTypeArray);
                            joinedProductTypes = joinedProductTypes.Replace("&gt;", ">");
                        }


                        string joinedCategoriesTypes = string.Empty;
                        if (categories.Count > 0)
                        {
                            // XmlNodeList'i bir diziye dönüştürme
                            string[] categoriesArray = new string[categories.Count];
                            for (int i = 0; i < categories.Count; i++)
                            {
                                categoriesArray[i] = categories[i].InnerText;
                            }

                            // Diziyi birleştirme
                            joinedCategoriesTypes = string.Join(", ", categoriesArray);
                            joinedCategoriesTypes = joinedCategoriesTypes.Replace("&gt;", ">");
                        }

                        worksheet.Cells[row, 1].Value = item.SelectSingleNode("title").InnerText;
                        worksheet.Cells[row, 2].Value = item.SelectSingleNode("barcode").InnerText;
                        worksheet.Cells[row, 3].Value = item.SelectSingleNode("model_number").InnerText;
                        worksheet.Cells[row, 4].Value = item.SelectSingleNode("price").InnerText;
                        worksheet.Cells[row, 5].Value = item.SelectSingleNode("quantity").InnerText;
                        worksheet.Cells[row, 6].Value = item.SelectSingleNode("description").InnerText;
                        worksheet.Cells[row, 7].Value = item.SelectSingleNode("link").InnerText;
                        worksheet.Cells[row, 8].Value = joinedCategoriesTypes;
                        worksheet.Cells[row, 9].Value = joinedProductTypes;
                        row++;
                    }

                    // Excel dosyasını kaydet
                    // Excel dosyasını byte dizisi olarak al
                    byte[] excelBytes = package.GetAsByteArray();

                    return excelBytes;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<TahtaKaleResponseDto> GetAll()
        {
            var xmlData = await XmlToData();

            TahtaKaleResponseDto response = new TahtaKaleResponseDto()
            {
                Datas = xmlData.Channel.Items.Select(f => f).ToList()
            };


            return response;
        }
        public async Task Sync()
        {

            var resp = await GetAll();
            if (resp != null && resp.Datas.Count > 0)
            {
                var mapped = resp.Datas.Select(f => new TahtakaleProduct
                {
                    AdditionalImageLink1 = f.AdditionalImageLink1,
                    Availability = f.Availability,
                    Barcode = f.Barcode,
                    Brand = f.Brand,
                    Category = f.Category,
                    Description = f.Description,
                    GoogleProductCategory = f.GoogleProductCategory,
                    ProductId = f.Id,
                    ImageLink = f.ImageLink,
                    Link = f.Link,
                    ModelNumber = f.ModelNumber,
                    Price = f.Price,
                    ProductType = f.ProductType,
                    Quantity = f.Quantity,
                    Title = f.Title,
                }).ToList();
                await tahtakaleService.RemoveAndAddMany(mapped);
            }

        }
        public async Task<TahtaKaleResponseDto> GetAllByFilter(int skip = 0, int take = 50)
        {
            var xmlData = await XmlToData();

            TahtaKaleResponseDto response = new TahtaKaleResponseDto()
            {
                Datas = xmlData.Channel.Items.Select(f => f).Take(take).Skip(skip).ToList()
            };


            return response;
        }

        private async Task<string> Request()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.tahtakaletoptanticaret.com/export.xml");
            request.Headers.Add("Cookie", "OCSESSID=918bb4eb048ac267bf91e46441; currency=TRY; language=tr-tr");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string xml = await response.Content.ReadAsStringAsync();
            return xml;
        }
        private async Task<Rss> XmlToData()
        {
            try
            {
                string xmlData = await Request();
                Rss rss;
                XmlSerializer serializer = new XmlSerializer(typeof(Rss));
                using (TextReader reader = new StringReader(xmlData))
                {
                    rss = (Rss)serializer.Deserialize(reader);
                }


                return rss;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
    public class TahtaKaleResponseDto
    {
        public List<Item> Datas { get; set; } = new();
    }
    [XmlRoot(ElementName = "item")]
    public class Item
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "brand")]
        public string Brand { get; set; }
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "barcode")]
        public string Barcode { get; set; }
        [XmlElement(ElementName = "image_link")]
        public string ImageLink { get; set; }
        [XmlElement(ElementName = "additional_image_link1")]
        public string AdditionalImageLink1 { get; set; }
        [XmlElement(ElementName = "model_number")]
        public string ModelNumber { get; set; }
        [XmlElement(ElementName = "price")]
        public decimal Price { get; set; }
        [XmlElement(ElementName = "google_product_category")]
        public string GoogleProductCategory { get; set; }
        [XmlElement(ElementName = "product_type")]
        public List<string> ProductType { get; set; }


        [XmlIgnore]
        public string ProductTypeStr { get; set; }
        [XmlIgnore]
        public string CategoryTypeStr { get; set; }

        [XmlElement(ElementName = "category")]
        public List<string> Category { get; set; }
        [XmlElement(ElementName = "quantity")]
        public int Quantity { get; set; }
        [XmlElement(ElementName = "availability")]
        public string Availability { get; set; }
    }

    [XmlRoot(ElementName = "channel")]
    public class Channel
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        [XmlElement(ElementName = "item")]
        public List<Item> Items { get; set; }
    }

    [XmlRoot(ElementName = "rss")]
    public class Rss
    {
        [XmlElement(ElementName = "channel")]
        public Channel Channel { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "g", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string G { get; set; }
    }
}
