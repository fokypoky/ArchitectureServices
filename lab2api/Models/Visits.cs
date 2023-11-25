﻿namespace lab2api.Models
{
	public class Visits
	{
		public int StudentId { get; set; }

		public int LectureId { get; set; }

		public DateTime Date { get; set; }

		public virtual Lecture Lecture { get; set; } = null!;

		public virtual Student Student { get; set; } = null!;
	}
}
