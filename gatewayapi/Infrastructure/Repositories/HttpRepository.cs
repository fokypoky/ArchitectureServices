using gatewayapi.Models;

namespace gatewayapi.Infrastructure.Repositories
{
	public class HttpRepository
	{
		private readonly HttpClient _httpClient;
		public HttpRepository(string baseAddress)
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = new Uri(baseAddress);
		}

		public async Task<ResponseModel> GetData(string request)
		{
			var response = await _httpClient.GetAsync(request);
			var responseBody = await response.Content.ReadAsStringAsync();
			return new ResponseModel() { Response = responseBody, Status = (int)response.StatusCode };
		}
	}
}
