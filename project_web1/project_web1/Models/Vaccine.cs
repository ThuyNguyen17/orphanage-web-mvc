using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class Vaccine
{
    public int MaSoVaccine { get; set; }

    public string TenVaccine { get; set; } = null!;

    public string? PhongBenh { get; set; }

    public DateTime? NgayHetHan { get; set; }

    public byte? SoMuiCanTiem { get; set; }

    public string? GhiChu { get; set; }

    public virtual PhieuTiemChung? PhieuTiemChung { get; set; }
}
