using System;
using System.Collections.Generic;

namespace lab2api.Models;

public partial class Specialty
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Code { get; set; }

    public int? StudyDuration { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
