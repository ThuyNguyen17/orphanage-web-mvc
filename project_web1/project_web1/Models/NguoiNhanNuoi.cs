using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class NguoiNhanNuoi
{
    public int MaSoNnn { get; set; }

    public string? HoTen { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? Sdt { get; set; }

    public string? DiaChi { get; set; }

    public string? Email { get; set; }

    public string? GhiChu { get; set; }

    public int MaSoTre { get; set; }

    public virtual Tre MaSoTreNavigation { get; set; } = null!;
}
