using System.Diagnostics;
using Neo4j.Driver;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace lab1api.Infrastructure.Repositories
{
	public class Neo4jRepository
	{
		public async Task<IEnumerable<string>> GetStudentsByLectureAndPeriod(List<int> lectureIds, DateTime dateFrom,
			DateTime dateTo)
		{
			var client = new BoltGraphClient(new Uri("bolt://neo4j:7687"), "neo4j", "dr22042002");
			await client.ConnectAsync();

			var query = new CypherFluentQuery(client)
				.Match("(s:Student)-[:STUDY_IN]->(g:Group)-[:VISITS]->(t:Timetable), (l:Lecture)-[:READS]->(t)")
				.Where($"datetime(t.date) >= datetime('{dateFrom:yyyy-MM-ddTHH:mm:ss}') AND datetime(t.date) <= datetime('{dateTo:yyyy-MM-ddTHH:mm:ss}')")
				.AndWhere("t.pg_lecture_id IN $lectureIds")
				.WithParam("lectureIds", lectureIds)
				.Return<string>("distinct s.passbook_number");

			return await query.ResultsAsync;
		}
	}
}
