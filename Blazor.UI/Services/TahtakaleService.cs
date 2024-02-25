using Blazor.UI.Models;
using System.Net.Http.Json;

namespace Blazor.UI.Services
{
    public class TahtakaleService
    {
        private readonly HttpClient _httpClient;

        public TahtakaleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<TahtaKaleGetAllByFilterResponse> GetAllByFilter(TahtaKaleGetAllByFilterRequest filter)
        {
            try
            {
                var resp = await _httpClient.PostAsJsonAsync<TahtaKaleGetAllByFilterRequest>("tahtakale/GetAllByFilter", filter);

                if (resp.IsSuccessStatusCode==false)
                {
                    throw new Exception($"erişim yok {resp.StatusCode.ToString()}");
                }
                return await resp.Content.ReadFromJsonAsync<TahtaKaleGetAllByFilterResponse>();

            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
