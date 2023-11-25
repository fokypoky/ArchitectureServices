using lab1api.Models.Elastic;
using Nest;

namespace lab1api.Infrastructure.Repositories.Repositories
{
	public class ElasticSearchRepository
	{
		public List<string>? GetByPhrase(string phrase)
		{
			var node = new Uri("http://localhost:10103");
			var settings = new ConnectionSettings(node).DefaultIndex("lecture_materials");

			var client = new ElasticClient(settings);
			var hits = new List<string>();

			try
			{
				var response = client.Search<LectureMaterials>(s => s
					.Query(q => q
						.MatchPhrase(m => m
							.Field(f => f.Content)
							.Query(phrase))));
				if (response.IsValid)
				{
					foreach (var hit in response.Hits)
					{
						hits.Add(hit.Id);
					}
				}
			}
			catch
			{
				return null;
			}

			return hits;
		}
	}
}
