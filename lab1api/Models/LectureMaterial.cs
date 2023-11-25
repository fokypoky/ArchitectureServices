using System;
using System.Collections.Generic;

namespace lab1api.Models;

public partial class LectureMaterial
{
    public int Id { get; set; }

    public int? LectureId { get; set; }

    public string MaterialsId { get; set; } = null!;

    public virtual Lecture? Lecture { get; set; }
}
