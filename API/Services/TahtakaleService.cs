using API.Databases.Mongo.DataAccess;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using API.Databases.Mongo.DataModels;
using API.Databases.Mongo;
using MongoDB.Driver;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace API.Services
{
    public class TahtakaleService
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<TahtakaleProduct> _tahtaKaleProductCollection;
        public TahtakaleService(IMongoDBSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
            _tahtaKaleProductCollection = _database.GetCollection<TahtakaleProduct>(nameof(TahtakaleProduct));
        }
        public async Task<GetAllByFilterResponse> GetAllByFilter(GetAllByFilter filter)
        {

           

            GetAllByFilterResponse resp = new GetAllByFilterResponse();
            var builder = Builders<TahtakaleProduct>.Filter;
            FilterDefinition<TahtakaleProduct> query = builder.Empty;
            if (string.IsNullOrEmpty(filter.ModelNumber) == false)
            {
                query &= builder.Regex(f => f.ModelNumber, new BsonRegularExpression(filter.ModelNumber));
            }
            if (filter.Quantity != null)
            {
                query &= builder.Eq(f => f.Quantity, filter.Quantity);
            }
            if (string.IsNullOrEmpty(filter.Barcode)==false)
            {
                query &= builder.Regex(f => f.Barcode, new BsonRegularExpression(filter.Barcode));
            }
            if (string.IsNullOrEmpty(filter.Title) == false)
            {
                query &= builder.Regex(f => f.Title, new BsonRegularExpression(filter.Title));
            }
            if (filter.BeginPrice != null && filter.EndPrice != null )
            {
                query &= builder.And(builder.Gte(f => f.Price, filter.BeginPrice) & builder.Lte(f => f.Price, filter.EndPrice))  ;
            }

            SortDefinition<TahtakaleProduct> sort = null;

            if (filter.Sort.Direction==SortDirection.Descending)
            {
                sort = Builders<TahtakaleProduct>.Sort.Descending(filter.Sort.Column);
            }
            else
            {
                sort = Builders<TahtakaleProduct>.Sort.Ascending(filter.Sort.Column);
            }

            long totalCount = _tahtaKaleProductCollection.Count(query);
            var findItems = await this._tahtaKaleProductCollection.FindAsync(query,new FindOptions<TahtakaleProduct> { Skip=filter.Skip,Limit=filter.Take,Sort= sort });
            
            return new GetAllByFilterResponse()
            {
                Total = totalCount,
                Data = findItems.ToList(),
                Filters = null
            };

        }
        public async Task RemoveAndAddMany(List<TahtakaleProduct> tahtakaleProducts)
        {
            await _tahtaKaleProductCollection.DeleteManyAsync(_ => true);
            await _tahtaKaleProductCollection.InsertManyAsync(tahtakaleProducts);
        }
    }
    public class GetAllByFilterResponse
    {
        public long Total { get; set; }
        public int PageSize { get { return this.Data.Count; } }
        public List<TahtakaleProduct> Data { get; set; }
        public Filter Filters { get; set; }
        public class Filter
        {


        }
    }
    public class GetAllByFilter
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string? Title { get; set; }
        public string? Barcode { get; set; }
        public int? Quantity { get; set; }
        public string? ModelNumber { get; set; }
        public decimal? BeginPrice { get; set; }
        public decimal? EndPrice { get; set; }

        public Sort Sort { get; set; } = new Sort { Direction=SortDirection.Descending,
            Column= "ProductId" };   
    }
    public class Sort
    {
        public SortDirection Direction { get; set;}
        public string Column { get; set;}
    }
    public enum SortDirection
    {
        [Description("none")]
        None,
        [Description("ascending")]
        Ascending,
        [Description("descending")]
        Descending
    }


}
