namespace RequestsVisualizer.Models.Lab2
{
	public class ReportRequest
	{
		public string Token { get; set; } = null!;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string CourseTitle { get; set; } = null!;
	}
}
