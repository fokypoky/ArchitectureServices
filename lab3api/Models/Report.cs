using lab1api.Models;

namespace lab3api.Models
{
	public class Report
	{
		public Group Group { get; set; }
		public List<StudentReport> StudentReports { get; set; } = new List<StudentReport>();
		public List<CourseReport> CourseReports {get; set; } = new List<CourseReport>();
		public List<LectureReport> LectureReports { get; set; } = new List<LectureReport>();
	}
}
