using System;
using System.Collections.Generic;

namespace lab2api.Models;

public partial class Student
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? GroupId { get; set; }

    public string? PassbookNumber { get; set; }

    public virtual Group? Group { get; set; }
}
