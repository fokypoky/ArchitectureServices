using System;
using System.Collections.Generic;

namespace lab2api.Models;

public partial class Lecture
{
    public int Id { get; set; }

    public string? Annotation { get; set; }

    public int? TypeId { get; set; }

    public int? CourseId { get; set; }

    public string? Requirements { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<LectureMaterial> LectureMaterials { get; set; } = new List<LectureMaterial>();

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();

    public virtual LectureType? Type { get; set; }
}
