using System;
using System.Collections.Generic;

namespace lab1api.Models;

public partial class LectureType
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
}
