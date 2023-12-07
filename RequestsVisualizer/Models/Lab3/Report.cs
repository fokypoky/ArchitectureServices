namespace RequestsVisualizer.Models.Lab3
{
	public class Report
	{
		public string GroupNumber { get; set; }
		public int StudentsCount { get; set; }
		public string DepartmentTitle { get; set; }
		public string SpecialityCode { get; set; }
		public List<CourseReport> CourseReports { get; set; }
		public string? Error { get; set; }
	}
}
