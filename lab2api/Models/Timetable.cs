using System;
using System.Collections.Generic;

namespace lab2api.Models;

public partial class Timetable
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public int LectureId { get; set; }

    public DateTime? Date { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual Lecture Lecture { get; set; } = null!;
}
