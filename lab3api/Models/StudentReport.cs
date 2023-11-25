using lab1api.Models;

namespace lab3api.Models
{
	public class StudentReport
	{
		public Student Student { get; set; }
		public List<VisitsReport> VisitsReports { get; set; } = new List<VisitsReport>();
	}
}
