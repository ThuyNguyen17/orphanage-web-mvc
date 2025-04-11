using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class PhieuTiemChung
{
    public int MaSoTiem { get; set; }

    public DateTime? NgayTiem { get; set; }

    public byte? MuiThu { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public int MaSoTre { get; set; }

    public int MaSoVaccine { get; set; }

    public virtual Tre MaSoTreNavigation { get; set; } = null!;

    public virtual Vaccine MaSoVaccineNavigation { get; set; } = null!;
}
