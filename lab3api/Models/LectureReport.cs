using lab1api.Models;

namespace lab3api.Models
{
	public class LectureReport
	{
		public Lecture Lecture { get; set; }
		public string Materials { get; set; }
		public int HoursCount { get; set; }
	}
}
