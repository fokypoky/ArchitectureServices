namespace RequestsVisualizer.Models.Lab1
{
	public class StudentAttendenceReport
	{
		public List<StudentAttend> Attends { get; set; }
		public string Phrase { get; set; }
		public string? Error { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
