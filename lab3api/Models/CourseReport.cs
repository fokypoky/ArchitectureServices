namespace lab3api.Models
{
	public class CourseReport
	{
		public string CourseTitle { get; set; }
		public List<LectureReport> LectureReports { get; set; }
	}
}
