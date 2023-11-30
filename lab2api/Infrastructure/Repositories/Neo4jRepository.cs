using Neo4jClient;
using Neo4jClient.Cypher;

namespace lab2api.Infrastructure.Repositories
{
	public class Neo4jRepository
	{
		public async Task<IEnumerable<int>> GetVisitsCount(DateTime? date, int lectureId)
		{
			var client = new BoltGraphClient(new Uri("bolt://localhost:1933"), "neo4j", "dr22042002");
			await client.ConnectAsync();


			var query = new CypherFluentQuery(client)
				.Match("(s:Student)-[:STUDY_IN]->(g:Group)-[:VISITS]->(t:Timetable), (l:Lecture)-[:READS]->(t)")
				.Where($"l.pg_id={lectureId} and datetime(t.date)=datetime('{date:yyyy-MM-ddTHH:mm:ss}')")
				.Return<int>("count(distinct(s))");

			return await query.ResultsAsync;
		}
	}
}
