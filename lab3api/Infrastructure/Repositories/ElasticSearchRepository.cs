using lab3api.Models.Elastic;
using Neo4jClient.Cypher;
using Neo4jClient.Extensions;
using Nest;

namespace lab3api.Infrastructure.Repositories
{
	public class ElasticSearchRepository
	{
		public LectureMaterials GetLectureMaterialsById(string id)
		{
			var node = new Uri("http://elastic-s:9200");
			var settings = new ConnectionSettings(node)/*.DefaultIndex("courses")*/;

			var client = new ElasticClient(settings);

			return client.Source<LectureMaterials>(id, g => g.Index("lecture_materials")).Body;
		}
		public Course GetCourseById(string id)
		{
			var node = new Uri("http://elastic-s:9200");
			var settings = new ConnectionSettings(node)/*.DefaultIndex("courses")*/;

			var client = new ElasticClient(settings);

			return client.Source<Course>(id, g => g.Index("courses")).Body;
		}
	}
}
