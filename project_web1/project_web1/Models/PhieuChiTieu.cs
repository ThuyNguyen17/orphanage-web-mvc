using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class PhieuChiTieu
{
    public int MaNhanVien { get; set; }

    public int MaPhieuChiTieu { get; set; }

    public DateTime? NgayGhi { get; set; }

    public decimal? TongTien { get; set; }

    public string? Mota { get; set; }

    public virtual ICollection<CtchiTieu> CtchiTieus { get; set; } = new List<CtchiTieu>();

    public virtual NguoiChamSoc MaNhanVienNavigation { get; set; } = null!;
}
