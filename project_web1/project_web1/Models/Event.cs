using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public string Time { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string? ImageUrl { get; set; } = null!;

    public string Description { get; set; } = null!;
}
