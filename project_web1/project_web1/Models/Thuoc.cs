using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class Thuoc
{
    public int MaSoThuoc { get; set; }

    public string TenThuoc { get; set; } = null!;

    public string HoatChat { get; set; } = null!;

    public string? CongDung { get; set; }

    public string? LieuLuongDeXuat { get; set; }

    public DateTime? NgayHetHan { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<Ctdonthuoc> Ctdonthuocs { get; set; } = new List<Ctdonthuoc>();

    public virtual ICollection<CtphieuMua> CtphieuMuas { get; set; } = new List<CtphieuMua>();
}
