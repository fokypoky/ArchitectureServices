using Neo4jClient;
using Neo4jClient.Cypher;

namespace lab3api.Infrastructure.Repositories
{
	public class Neo4jRepository
	{
		public async Task<IEnumerable<string>> GetCoursesByGroupAndDepartment(string groupNumber,
			string departmentTitle, string mainSpecialityCode)
		{
			var client = new BoltGraphClient(new Uri("bolt://neo4j:7687"), "neo4j", "dr22042002");
			await client.ConnectAsync();


			var query = new CypherFluentQuery(client)
				.Match("(d:Department)-[:PRODUCES]->(s:Speciality)-[:TEACHS]->(c:Course), (g:Group)-[:LEARNS]->(c)")
				.Where($"d.name='{departmentTitle}' and s.code='{mainSpecialityCode}' and g.name='{groupNumber}'")
				.Return<string>("distinct c.title");

			return await query.ResultsAsync;
		}
	}
}
