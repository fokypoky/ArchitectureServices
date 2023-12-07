namespace RequestsVisualizer.Models.Lab2
{
	public class Report
	{
		public string? CourseTitle { get; set; }
		public string? Error { get; set; }
		public List<LectureReport>? LectureReports { get; set; }
	}
}
