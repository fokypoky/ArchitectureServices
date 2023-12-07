namespace RequestsVisualizer.Models.Lab3
{
	public class ReportRequest
	{
		public string? Token { get; set; }
		public string? GroupNumber { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}
