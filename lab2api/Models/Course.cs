using System;
using System.Collections.Generic;

namespace lab2api.Models;

public partial class Course
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? Duration { get; set; }

    public string? DescriptionId { get; set; }

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
}
