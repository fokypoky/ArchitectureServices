namespace RequestsVisualizer.Models.Lab1
{
	public class ReportRequest
	{
		public string Token { get; set; } = null!;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Phrase { get; set; } = null!;
	}
}
