namespace lab3api.Models
{
	public class Report
	{
		public string GroupNumber { get; set; }
		public int StudentsCount { get; set; }
		public string DepartmentTitle { get; set; }
		public string SpecialityCode { get; set; }
		public List<CourseReport> CourseReports { get; set; }
	}
}
