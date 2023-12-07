namespace RequestsVisualizer.Models.Lab3
{
	public class Report
	{
		public string GroupName { get; set; }
		public List<CourseReport> CourseReports { get; set; }
		public string? Error { get; set; }
	}
}
