using lab1api.Models;

namespace lab3api.Models
{
	public class LectureReport
	{
		public string LectureAnnotation { get; set; }
		public string LectureType { get; set; }
		public string LectureRequirements { get; set; }
		public List<StudentReport> StudentReports { get; set; }
		public int PlannedHours { get; set; }
	}
}
