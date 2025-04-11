using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class ThuocMuaNgoai
{
    public int MaSoMuaThuoc { get; set; }

    public DateTime? NgayMua { get; set; }

    public string? NoiMua { get; set; }

    public string? GhiChu { get; set; }

    public decimal? TongTien { get; set; }

    public virtual ICollection<CtphieuMua> CtphieuMuas { get; set; } = new List<CtphieuMua>();
}
