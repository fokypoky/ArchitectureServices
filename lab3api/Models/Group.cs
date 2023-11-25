using System;
using System.Collections.Generic;

namespace lab1api.Models;

public partial class Group
{
    public int Id { get; set; }

    public string? Number { get; set; }

    public int? DepartmentId { get; set; }

    public int? SpecialityId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Specialty? Speciality { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
