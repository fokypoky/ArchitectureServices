using Newtonsoft.Json;
using RequestsVisualizer.Models;

namespace RequestsVisualizer.Infrastructure.Repositories
{
	public class HttpRepository<T> where T : class
	{
		private readonly HttpClient _httpClient;
		public HttpRepository(string baseAddress, string token)
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = new Uri(baseAddress);
			_httpClient.DefaultRequestHeaders.Add("Authorization", token);
		}

		public async Task<ResponseModel<T>> GetData(string request)
		{
			var response = await _httpClient.GetAsync(request);
			var responseBody = await response.Content.ReadAsStringAsync();
			
			if ((int)response.StatusCode != 200)
			{
				return new ResponseModel<T>()
				{
					Status = (int)response.StatusCode,
					ErrorMessage = responseBody
				};
			}

			return new ResponseModel<T>()
			{
				Response = JsonConvert.DeserializeObject<T>(responseBody),
				Status = 200
			};
		}

	}
}
