using System;
using System.Collections.Generic;

namespace lab1api.Models;

public partial class Department
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? InstituteId { get; set; }

    public int? MainSpecialityId { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual Specialty? MainSpeciality { get; set; }
}
