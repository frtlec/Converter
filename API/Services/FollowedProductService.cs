using API.Databases.Mongo;
using API.Databases.Mongo.DataAccess;
using API.Databases.Mongo.DataModels;
using API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace API.Services
{
    public class FollowedProductService
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<FollowedProduct> _followedProductCollection;
        private readonly IMongoCollection<TahtakaleProduct> _tahtakaleProductsCollection;
        public FollowedProductService(IMongoDBSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
            _followedProductCollection = _database.GetCollection<FollowedProduct>(nameof(FollowedProduct));
            _tahtakaleProductsCollection = _database.GetCollection<TahtakaleProduct>(nameof(TahtakaleProduct));
        }
        public async Task<FollowedProductDto> GetAll()
        {
            var followedProducts = await _followedProductCollection.Find(f=>true).ToListAsync();

            var filter = Builders<TahtakaleProduct>.Filter.In(f => f.ProductId, followedProducts.Select(f=>f.ProductId.ToString()));

            List<TahtakaleProduct> tahtakaleProducts = await _tahtakaleProductsCollection.Find(filter).ToListAsync();

            var followedProducts_tahtaKale = tahtakaleProducts.Where(f => followedProducts.Any(x => x.ProductId == Convert.ToInt64(f.ProductId))).Select(x => new FollowedProductDto.SubItem
            {
                Id = followedProducts.First(y => y.ProductId == Convert.ToInt64(x.ProductId)).Id,
                Title = x.Title,
                ImageLinks = new List<string> { x.ImageLink, x.AdditionalImageLink1 },
                SourceSite = SourceSite.TAHTAKALE,
                Stock = x.Quantity,
                Barcode = x.Barcode,
                Categories = x.Category,
                Description = x.Description,
                Link = x.Link,
                ModelNo = x.ModelNumber,
                TahtaKaleItem = x,
                Price = x.Price,

                FollowedProduct = followedProducts.First(y => y.ProductId == Convert.ToInt64(x.ProductId)),
                TrendyolStock = followedProducts.First(y => y.ProductId == Convert.ToInt64(x.ProductId)).TrendyolStock,
                TrendyolLink = followedProducts.First(y => y.ProductId == Convert.ToInt64(x.ProductId)).TrendyolLink,
                TrendyolPrice = followedProducts.First(y => y.ProductId == Convert.ToInt64(x.ProductId)).TrendyolPrice,
                ProductId = Convert.ToInt64(x.ProductId),
                ProductTypes = x.ProductType
            }).ToList();
            return new FollowedProductDto()
            {
                Items = followedProducts_tahtaKale
            };

        }
        public async Task Add(FollowedProductAddDto item)
        {
            await _followedProductCollection.InsertOneAsync(new FollowedProduct
            {
                Barcode = item.Barcode,
                SourceSite = SourceSite.TAHTAKALE,
                ProductId = item.ProductId
            });
        }
        public async Task Remove(string id)
        {
            await _followedProductCollection.DeleteOneAsync(m => m.Id == id);
        }
        public async Task Update(FollowedProductUpdate updateItem)
        {
            var filter = Builders<FollowedProduct>.Filter.Eq(f => f.Id, updateItem.Id);
            var update = Builders<FollowedProduct>.Update.Combine(
                    Builders<FollowedProduct>.Update.Set(p => p.TrendyolLink, updateItem.TrendyolLink),
                    Builders<FollowedProduct>.Update.Set(p => p.TrendyolPrice, updateItem.TrendyolPrice),
                    Builders<FollowedProduct>.Update.Set(p => p.TrendyolStock, updateItem.TrendyolStock)
                ); ;
            await _followedProductCollection.UpdateManyAsync(filter, update);
        }
        public async Task RemoveAndAdd(List<FollowedProduct> list)
        {
            await _followedProductCollection.DeleteManyAsync(x => list.Any(f => f.ProductId == x.ProductId) == false);
            var currentDatas = _followedProductCollection.Find(x => list.Any(f => f.ProductId == x.ProductId)).ToList();
            var insertItems = list.Except(currentDatas).ToList();
            insertItems.ForEach(f => f.CreatedDate = DateTime.Now);

            if (insertItems.Count > 0)
            {
                await _followedProductCollection.InsertManyAsync(insertItems);
            }
        }
    }
}
