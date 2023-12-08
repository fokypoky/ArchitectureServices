namespace RequestsVisualizer.Models
{
	public class ResponseModel<T> where T : class
	{
		public T? Response { get; set; }
		public int Status { get; set; }
		public string? ErrorMessage { get; set; }
	}
}
