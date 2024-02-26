using Blazor.UI.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;

namespace Blazor.UI.Services
{
   
    public class FollowedProductService
    {
        private readonly HttpClient _httpClient;

        public FollowedProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<FollowedProductResponse> GetAll()
        {
            try
            {
                var result = await _httpClient.GetStringAsync("/api/followedproducts/getall");
                var resp = JsonConvert.DeserializeObject<FollowedProductResponse>(result);


                return resp;

            }
            catch (Exception ex)
            {

                throw;
            }
       
        }
        public async Task Add(FollowedProductAddDto item)
        {
            try
            {
                var resp = await _httpClient.PostAsJsonAsync("/api/followedproducts/add", item);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task Remove(string id)
        {
            try
            {
                var resp = await _httpClient.GetAsync($"/api/followedproducts/remove/{id}");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task RemoveByProductIdAndSourceSite(int productId, SourceSite sourceSite)
        {
            try
            {
                var resp = await _httpClient.GetAsync($"/api/followedproducts/RemoveByProductIdAndSourceSite?productId={productId}&sourcesite={sourceSite}");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task Update(FollowedProductUpdate update)
        {
            try
            {
                var resp = await _httpClient.PostAsJsonAsync("/api/followedproducts/Update", update);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task RemoveAndAdd(List<FollowedProductAddDto> items)
        {
            try
            {
                var resp = await _httpClient.PostAsJsonAsync("/api/followedproducts/RemoveAndAdd", items);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
